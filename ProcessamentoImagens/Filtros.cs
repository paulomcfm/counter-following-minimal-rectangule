using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using static System.Net.Mime.MediaTypeNames;
using System.Linq;
using System.Data.SqlTypes;

namespace ProcessamentoImagens
{
    class Filtros
    {
        //sem acesso direto a memoria
        public static void convert_to_gray(Bitmap imageBitmapSrc, Bitmap imageBitmapDest)
        {
            int width = imageBitmapSrc.Width;
            int height = imageBitmapSrc.Height;
            int r, g, b;
            Int32 gs;

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    //obtendo a cor do pixel
                    Color cor = imageBitmapSrc.GetPixel(x, y);

                    r = cor.R;
                    g = cor.G;
                    b = cor.B;
                    gs = (Int32)(r * 0.2990 + g * 0.5870 + b * 0.1140);

                    //nova cor
                    Color newcolor = Color.FromArgb(gs, gs, gs);

                    imageBitmapDest.SetPixel(x, y, newcolor);
                }
            }
        }

        //com acesso direto a memória
        public static void convert_to_blackWhiteDMA(Bitmap imageBitmapSrc, Bitmap imageBitmapDest)
        {
            int width = imageBitmapSrc.Width;
            int height = imageBitmapSrc.Height;
            int pixelSize = 3;
            Int32 bw;

            //lock dados bitmap origem
            BitmapData bitmapDataSrc = imageBitmapSrc.LockBits(new Rectangle(0, 0, width, height),
                ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
            //lock dados bitmap destino
            BitmapData bitmapDataDst = imageBitmapDest.LockBits(new Rectangle(0, 0, width, height),
                ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);

            int padding = bitmapDataSrc.Stride - (width * pixelSize);

            unsafe
            {
                byte* src = (byte*)bitmapDataSrc.Scan0.ToPointer();
                byte* dst = (byte*)bitmapDataDst.Scan0.ToPointer();

                int r, g, b;
                for (int y = 0; y < height; y++)
                {
                    for (int x = 0; x < width; x++)
                    {
                        b = *(src++); //está armazenado dessa forma: b g r 
                        g = *(src++);
                        r = *(src++);
                        byte gray = (byte)((r + b + g) / 3);
                        
                        if(gray > 220)
                        {
                            bw = 255;
                        }
                        else
                        {
                            bw = 0;
                        }
                        *(dst++) = (byte)bw;
                        *(dst++) = (byte)bw;
                        *(dst++) = (byte)bw;
                    }
                    src += padding;
                    dst += padding;
                }
            }
            //unlock imagem origem
            imageBitmapSrc.UnlockBits(bitmapDataSrc);
            //unlock imagem destino
            imageBitmapDest.UnlockBits(bitmapDataDst);
        }

        internal static void zhangSuen(Bitmap imageBitmapSrc, Bitmap imageBitmapDest)
        {
            int width = imageBitmapSrc.Width;
            int height = imageBitmapSrc.Height;
            int pixelSize = 3;

            convert_to_blackWhiteDMA(imageBitmapSrc, imageBitmapDest);

            //lock dados bitmap origem
            BitmapData bitmapDataSrc = imageBitmapSrc.LockBits(new Rectangle(0, 0, width, height),
                ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
            //lock dados bitmap destino
            BitmapData bitmapDataDst = imageBitmapDest.LockBits(new Rectangle(0, 0, width, height),
                ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);

            int padding = bitmapDataSrc.Stride - (width * pixelSize);

            Stack<IntPtr> bytePointerStack = new Stack<IntPtr>();

            unsafe
            {
                byte* src = (byte*)bitmapDataSrc.Scan0.ToPointer();
                byte* dst = (byte*)bitmapDataDst.Scan0.ToPointer();

                byte* firstPx = dst;
                bool mustContinue = true;
                int connectivity, neighboors;

                while (mustContinue)
                {
                    mustContinue = false;

                    //first iteration
                    for (int y = 1; y < height-1; y++)
                    {
                        for (int x = 1; x < width-1; x++)
                        {
                            byte* actual = firstPx + ((bitmapDataDst.Stride * y) + (x * 3));
                            if (*actual == 0)
                            {
                                connectivity = 0;
                                connectivity = countConnectivity(firstPx, bitmapDataDst, x, y);
                                if (connectivity == 1)
                                {
                                    neighboors = countNeighboors(firstPx, bitmapDataDst, x, y);
                                    if (neighboors >= 2 && neighboors <= 6)
                                    {
                                        if (checkUpperBackground(firstPx, bitmapDataDst, x, y))
                                        {
                                            if (checkLeftBackground(firstPx, bitmapDataDst, x, y))
                                            {
                                                mustContinue = true;
                                                bytePointerStack.Push((IntPtr)actual);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }

                    while(bytePointerStack.Count > 0)
                    {
                        dst = (byte*)bytePointerStack.Pop();
                        *(dst++) = 255;
                        *(dst++) = 255;
                        *(dst++) = 255;
                    }


                    //second iteration
                    for (int y = 1; y < height - 1; y++)
                    {
                        for (int x = 1; x < width - 1; x++)
                        {
                            byte* actual = firstPx + ((bitmapDataDst.Stride * y) + (x * 3));
                            if (*actual == 0)
                            {
                                connectivity = 0;
                                connectivity = countConnectivity(firstPx, bitmapDataDst, x, y);
                                if (connectivity == 1)
                                {
                                    neighboors = countNeighboors(firstPx, bitmapDataDst, x, y);
                                    if (neighboors >= 2 && neighboors <= 6)
                                    {
                                        if (checkRigthBackground(firstPx, bitmapDataDst, x, y))
                                        {
                                            if (checkUnderBackground(firstPx, bitmapDataDst, x, y))
                                            {
                                                mustContinue = true;
                                                bytePointerStack.Push((IntPtr)actual);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }

                    while (bytePointerStack.Count > 0)
                    {
                        dst = (byte*)bytePointerStack.Pop();
                        *(dst++) = 255;
                        *(dst++) = 255;
                        *(dst++) = 255;
                    }
                }
            }
            //unlock imagem origem
            imageBitmapSrc.UnlockBits(bitmapDataSrc);
            //unlock imagem destino
            imageBitmapDest.UnlockBits(bitmapDataDst);
        }

        private static unsafe bool checkUnderBackground(byte* firstPx, BitmapData bitmapDataDst, int x, int y)
        {
            byte* p4 = firstPx + ((bitmapDataDst.Stride * y) + ((x + 1) * 3));
            byte* p6 = firstPx + ((bitmapDataDst.Stride * (y + 1)) + (x * 3));
            byte* p8 = firstPx + ((bitmapDataDst.Stride * y) + ((x - 1) * 3));

            int background = 0;

            if (*p4 == 255)
                background++;
            if (*p6 == 255)
                background++;
            if (*p8 == 255)
                background++;

            return background >= 1;
        }

        private static unsafe bool checkRigthBackground(byte* firstPx, BitmapData bitmapDataDst,int x, int y)
        {
            byte* p2 = firstPx + ((bitmapDataDst.Stride * (y - 1)) + (x * 3));
            byte* p4 = firstPx + ((bitmapDataDst.Stride * y) + ((x + 1) * 3));
            byte* p6 = firstPx + ((bitmapDataDst.Stride * (y + 1)) + (x * 3));

            int background = 0;

            if (*p2 == 255)
                background++;
            if (*p4 == 255)
                background++;
            if (*p6 == 255)
                background++;

            return background >= 1;
        }

        private static unsafe bool checkLeftBackground(byte* firstPx,BitmapData bitmapDataDst, int x, int y)
        {
            byte* p2 = firstPx + ((bitmapDataDst.Stride * (y - 1)) + (x * 3));
            byte* p6 = firstPx + ((bitmapDataDst.Stride * (y + 1)) + (x * 3));
            byte* p8 = firstPx + ((bitmapDataDst.Stride * y) + ((x - 1) * 3));

            int background = 0;

            if (*p2 == 255)
                background++;
            if (*p8 == 255)
                background++;
            if (*p6 == 255)
                background++;

            return background >= 1;
        }

        private static unsafe bool checkUpperBackground(byte*firstPx, BitmapData bitmapDataDst, int x, int y)
        {
            byte* p2 = firstPx + ((bitmapDataDst.Stride * (y - 1)) + (x * 3));
            byte* p4 = firstPx + ((bitmapDataDst.Stride * y) + ((x + 1) * 3));
            byte* p8 = firstPx + ((bitmapDataDst.Stride * y) + ((x - 1) * 3));

            int background = 0;

            if (*p2 == 255)
                background++;
            if (*p4 == 255)
                background++;
            if (*p8 == 255)
                background++;

            return background >= 1;
        }

        private static unsafe int countNeighboors(byte*firstPx, BitmapData bitmapDataDst, int x, int y)
        {
            byte* p2 = firstPx + (bitmapDataDst.Stride * (y - 1)) + (x * 3); // (x, y-1)
            byte* p3 = firstPx + (bitmapDataDst.Stride * (y - 1)) + ((x + 1) * 3); // (x+1, y-1)
            byte* p4 = firstPx + (bitmapDataDst.Stride * y) + ((x + 1) * 3); // (x+1, y)
            byte* p5 = firstPx + (bitmapDataDst.Stride * (y + 1)) + ((x + 1) * 3); // (x+1, y+1)
            byte* p6 = firstPx + (bitmapDataDst.Stride * (y + 1)) + (x * 3); // (x, y+1)
            byte* p7 = firstPx + (bitmapDataDst.Stride * (y + 1)) + ((x - 1) * 3); // (x-1, y+1)
            byte* p8 = firstPx + (bitmapDataDst.Stride * y) + ((x - 1) * 3); // (x-1, y)
            byte* p9 = firstPx + (bitmapDataDst.Stride * (y - 1)) + ((x - 1) * 3); // (x-1, y-1)

            int neighboors = 0;

            if (*p2 == 0)
                neighboors++;
            if (*p3 == 0)
                neighboors++;
            if (*p4 == 0)
                neighboors++;
            if (*p5 == 0)
                neighboors++;
            if (*p6 == 0)
                neighboors++;
            if (*p7 == 0)
                neighboors++;
            if (*p8 == 0)
                neighboors++;
            if (*p9 == 0)
                neighboors++;

            return neighboors;
        }

        private static unsafe int countConnectivity(byte* firstPx, BitmapData bitmapDataDst, int x, int y)
        {
            // P9 P2 P3
            // P8 P1 P4
            // P7 P6 P5

            byte* p2 = firstPx + (bitmapDataDst.Stride * (y - 1)) + (x * 3); // (x, y-1)
            byte* p3 = firstPx + (bitmapDataDst.Stride * (y - 1)) + ((x + 1) * 3); // (x+1, y-1)
            byte* p4 = firstPx + (bitmapDataDst.Stride * y) + ((x + 1) * 3); // (x+1, y)
            byte* p5 = firstPx + (bitmapDataDst.Stride * (y + 1)) + ((x + 1) * 3); // (x+1, y+1)
            byte* p6 = firstPx + (bitmapDataDst.Stride * (y + 1)) + (x * 3); // (x, y+1)
            byte* p7 = firstPx + (bitmapDataDst.Stride * (y + 1)) + ((x - 1) * 3); // (x-1, y+1)
            byte* p8 = firstPx + (bitmapDataDst.Stride * y) + ((x - 1) * 3); // (x-1, y)
            byte* p9 = firstPx + (bitmapDataDst.Stride * (y - 1)) + ((x - 1) * 3); // (x-1, y-1)

            int connectivity = 0;
            if (*p2 == 255 && *p3 == 0)
                connectivity++;
            if (*p3 == 255 && *p4 == 0)
                connectivity++;
            if (*p4 == 255 && *p5 == 0)
                connectivity++;
            if (*p5 == 255 && *p6 == 0)
                connectivity++;
            if (*p6 == 255 && *p7 == 0)
                connectivity++;
            if (*p7 == 255 && *p8 == 0)
                connectivity++;
            if (*p8 == 255 && *p9 == 0)
                connectivity++;
            if (*p9 == 255 && *p2 == 0)
                connectivity++;

            return connectivity;
        }

        internal static void counterFollowing(Bitmap imageBitmapSrc, Bitmap imageBitmapDest)
        {
            int width = imageBitmapSrc.Width;
            int height = imageBitmapSrc.Height;
            int pixelSize = 3;

            //lock dados bitmap origem
            BitmapData bitmapDataSrc = imageBitmapSrc.LockBits(new Rectangle(0, 0, width, height),
                ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
            //lock dados bitmap destino
            BitmapData bitmapDataDst = imageBitmapDest.LockBits(new Rectangle(0, 0, width, height),
                ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);

            int padding = bitmapDataSrc.Stride - (width * pixelSize);

            unsafe
            {
                byte* src = (byte*)bitmapDataSrc.Scan0.ToPointer();
                byte* dst = (byte*)bitmapDataDst.Scan0.ToPointer();
                byte* firstPxSrc = src;
                byte* firstPxDst = dst;

                for (int y = 1; y < height - 1; y++)
                {
                    for (int x = 1; x < width - 1; x++)
                    {
                        byte* actual = firstPxSrc + ((bitmapDataDst.Stride * y) + (x * 3));
                        byte* next = actual + 3;
                        if (*actual == 255 && *next == 0)
                        {
                            paintCounterObject(actual, firstPxSrc, firstPxDst, bitmapDataSrc, bitmapDataDst, x, y);
                        }
                    }
                }
            }
        
            imageBitmapSrc.UnlockBits(bitmapDataSrc);
            imageBitmapDest.UnlockBits(bitmapDataDst);
        }

        private static unsafe void paintCounterObject(byte* actual, byte* firstPxSrc, byte* firstPxDst, BitmapData bitmapDataSrc, BitmapData bitmapDataDst, int x, int y)
        {
            int biggestX = x;
            int biggestY = y;
            int smallestX = x;
            int smallestY = y;
            char direction = 'r';
            byte* initial = actual;
            int next = getNextTransitionDiag(firstPxSrc, bitmapDataSrc, x, y, ref direction);
            if(next == 8)
                next = getNextTransition(firstPxSrc, bitmapDataSrc, x, y, ref direction);
            if(next == 8)
                next = getNextTransitionGray(firstPxSrc, bitmapDataSrc, x, y, ref direction);
            (*actual++) = 126;
            (*actual++) = 126;
            (*actual++) = 126;
            byte* dst = firstPxDst + ((bitmapDataDst.Stride * y) + (x * 3));
            (*dst++) = 0;
            (*dst++) = 0;
            (*dst++) = 0;
            setActual(ref actual, firstPxSrc, bitmapDataSrc, initial, ref x, ref y, next);
            if(x> biggestX)
                biggestX = x;
            if (x < smallestX)
                smallestX = x;
            if (y > biggestY)
                biggestY = y;
            if (y < smallestY)
                smallestY = y;
            while (actual != initial)
            {
                (*actual++) = 126;
                (*actual++) = 126;
                (*actual++) = 126;
                dst = firstPxDst + ((bitmapDataDst.Stride * y) + (x * 3));
                (*dst++) = 0;
                (*dst++) = 0;
                (*dst++) = 0;
                if (x == 51 && y == 161)
                    x = x;
                next = getNextTransitionDiag(firstPxSrc, bitmapDataSrc, x, y, ref direction);
                if (next == 8)
                    next = getNextTransition(firstPxSrc, bitmapDataSrc, x, y, ref direction);
                if (next == 8)
                    next = getNextTransitionGray(firstPxSrc, bitmapDataSrc, x, y, ref direction);
                setActual(ref actual, firstPxSrc, bitmapDataSrc, initial, ref x, ref y, next);
                if (x > biggestX)
                    biggestX = x;
                if (x < smallestX)
                    smallestX = x;
                if (y > biggestY)
                    biggestY = y;
                if (y < smallestY)
                    smallestY = y;
            }
            drawRectangle(firstPxDst, bitmapDataDst, smallestX, smallestY, biggestX, biggestY);
        }

        private static unsafe void drawRectangle(byte* firstPxDst, BitmapData bitmapDataDst, int smallestX, int smallestY, int biggestX, int biggestY)
        {
            byte* dst = firstPxDst + ((bitmapDataDst.Stride * smallestY) + (smallestX * 3));
            for (int i = smallestX; i <= biggestX; i++)
            {
                (*dst++) = 0;
                (*dst++) = 0;
                (*dst++) = 255;
            }
            dst = firstPxDst + ((bitmapDataDst.Stride * biggestY) + (smallestX * 3));
            for (int i = smallestX; i <= biggestX; i++)
            {
                (*dst++) = 0;
                (*dst++) = 0;
                (*dst++) = 255;
            }
            for (int i = smallestY; i <= biggestY; i++)
            {
                dst = firstPxDst + ((bitmapDataDst.Stride * i) + (smallestX * 3));
                (*dst++) = 0;
                (*dst++) = 0;
                (*dst++) = 255;
                dst = firstPxDst + ((bitmapDataDst.Stride * i) + (biggestX * 3));
                (*dst++) = 0;
                (*dst++) = 0;
                (*dst++) = 255;
            }
        }

        private static unsafe int getNextTransitionGray(byte* firstPxSrc, BitmapData bitmapDataSrc, int x, int y, ref char direction)
        {
            byte* fstByte = firstPxSrc + ((bitmapDataSrc.Stride * (y + 1)) + ((x + 1) * 3)); //7
            byte* sndByte = firstPxSrc + ((bitmapDataSrc.Stride * (y)) + ((x + 1) * 3)); //0
            if (*fstByte == 255 && *sndByte == 0)
            {
                direction = 'u';
                return 7;
            }
            else
            {
                fstByte = firstPxSrc + ((bitmapDataSrc.Stride * (y)) + ((x + 1) * 3)); //0
                sndByte = firstPxSrc + ((bitmapDataSrc.Stride * (y - 1)) + ((x + 1) * 3)); //1
                if (*fstByte == 255 && *sndByte == 0)
                {
                    direction = 'u';
                    return 0;
                }
                else
                {
                    fstByte = firstPxSrc + ((bitmapDataSrc.Stride * (y - 1)) + ((x + 1) * 3)); //1
                    sndByte = firstPxSrc + ((bitmapDataSrc.Stride * (y - 1)) + (x * 3)); //2
                    if (*fstByte == 255 && *sndByte == 0)
                    {
                        direction = 'l';
                        return 1;
                    }
                    else
                    {
                        fstByte = firstPxSrc + ((bitmapDataSrc.Stride * (y - 1)) + (x * 3)); //2
                        sndByte = firstPxSrc + ((bitmapDataSrc.Stride * (y - 1)) + ((x - 1) * 3)); //3
                        if (*fstByte == 255 && *sndByte == 0)
                        {
                            direction = 'l';
                            return 2;
                        }
                        else
                        {
                            fstByte = firstPxSrc + ((bitmapDataSrc.Stride * (y - 1)) + ((x - 1) * 3)); //3
                            sndByte = firstPxSrc + ((bitmapDataSrc.Stride * (y)) + ((x - 1) * 3)); //4
                            if (*fstByte == 255 && *sndByte == 0)
                            {
                                direction = 'd';
                                return 3;
                            }
                            else
                            {
                                fstByte = firstPxSrc + ((bitmapDataSrc.Stride * (y)) + ((x - 1) * 3)); //4
                                sndByte = firstPxSrc + ((bitmapDataSrc.Stride * (y + 1)) + ((x - 1) * 3)); //5
                                if (*fstByte == 255 && *sndByte == 0)
                                {
                                    direction = 'd';
                                    return 4;
                                }
                                else
                                {
                                    fstByte = firstPxSrc + ((bitmapDataSrc.Stride * (y + 1)) + ((x - 1) * 3)); //5
                                    sndByte = firstPxSrc + ((bitmapDataSrc.Stride * (y + 1)) + (x * 3)); //6
                                    if (*fstByte == 255 && *sndByte == 0)
                                    {
                                        direction = 'r';
                                        return 5;
                                    }
                                    else
                                    {
                                        fstByte = firstPxSrc + ((bitmapDataSrc.Stride * (y + 1)) + (x * 3)); //6
                                        sndByte = firstPxSrc + ((bitmapDataSrc.Stride * (y + 1)) + ((x + 1) * 3)); //7
                                        if (*fstByte == 255 && *sndByte == 0)
                                        {
                                            direction = 'r';
                                            return 6;
                                        }
                                        else
                                        {
                                            fstByte = firstPxSrc + ((bitmapDataSrc.Stride * (y + 1)) + ((x + 1) * 3)); //7
                                            sndByte = firstPxSrc + ((bitmapDataSrc.Stride * (y)) + ((x + 1) * 3)); //0
                                            if (*fstByte == 255 && *sndByte == 0)
                                            {
                                                direction = 'u';
                                                return 7;
                                            }
                                            else
                                            {
                                                return 8;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        private static unsafe int getNextTransitionDiag(byte* firstPxSrc, BitmapData bitmapDataSrc, int x, int y, ref char direction)
        {
            byte* diago = firstPxSrc + ((bitmapDataSrc.Stride * (y - 1)) + ((x + 1) * 3)); //1
            byte* beforeDiago = firstPxSrc + ((bitmapDataSrc.Stride * (y)) + ((x + 1) * 3)); //0
            if(*diago == 0 && *beforeDiago == 255)
            {
                direction = 'u';
                return 0;
            }
            else
            {
                diago = firstPxSrc + ((bitmapDataSrc.Stride * (y - 1)) + ((x - 1) * 3)); //3
                beforeDiago = firstPxSrc + ((bitmapDataSrc.Stride * (y-1)) + ((x) * 3)); //2
                if (*diago == 0 && *beforeDiago == 255)
                {
                    direction = 'l';
                    return 2;
                }
                else
                {
                    diago = firstPxSrc + ((bitmapDataSrc.Stride * (y + 1)) + ((x - 1) * 3)); //5
                    beforeDiago = firstPxSrc + ((bitmapDataSrc.Stride * (y)) + ((x - 1) * 3)); //4
                    if (*diago == 0 && *beforeDiago == 255)
                    {
                        direction = 'd';
                        return 4;
                    }
                    else
                    {
                        diago = firstPxSrc + ((bitmapDataSrc.Stride * (y + 1)) + ((x + 1) * 3)); //7
                        beforeDiago = firstPxSrc + ((bitmapDataSrc.Stride * (y + 1)) + ((x) * 3)); //6
                        if (*diago == 0 && *beforeDiago == 255)
                        {
                            direction = 'r';
                            return 6;
                        }
                        else return 8;
                    }
                }
            }
        }

        private static unsafe void setActual(ref byte* actual, byte* firstPxSrc, BitmapData bitmapDataSrc, byte* initial, ref int x, ref int y, int next)
        {
            switch(next){
                case 0:
                    actual = firstPxSrc + ((bitmapDataSrc.Stride * (y)) + ((x + 1) * 3)); //0
                    x++;
                    break;
                case 1:
                    actual = firstPxSrc + ((bitmapDataSrc.Stride * (y - 1)) + ((x + 1) * 3)); //1
                    x++;
                    y--;
                    break;
                case 2:
                    actual = firstPxSrc + ((bitmapDataSrc.Stride * (y - 1)) + (x * 3)); //2
                    y--;
                    break;
                case 3:
                    actual = firstPxSrc + ((bitmapDataSrc.Stride * (y - 1)) + ((x - 1) * 3)); //3
                    x--;
                    y--;
                    break;
                case 4:
                    actual = firstPxSrc + ((bitmapDataSrc.Stride * (y)) + ((x - 1) * 3)); //4
                    x--;
                    break;
                case 5:
                    actual = firstPxSrc + ((bitmapDataSrc.Stride * (y + 1)) + ((x - 1) * 3)); //5
                    x--;
                    y++;
                    break;
                case 6:
                    actual = firstPxSrc + ((bitmapDataSrc.Stride * (y + 1)) + (x * 3)); //6
                    y++;
                    break;
                case 7:
                    actual = firstPxSrc + ((bitmapDataSrc.Stride * (y + 1)) + ((x + 1) * 3)); //7
                    x++;
                    y++;
                    break;
                case 8:
                    actual = initial;
                    break;
            }
        }

        private static unsafe int getNextTransition(byte* firstPxSrc, BitmapData bitmapDataSrc, int x, int y, ref char direction)
        {
            switch (direction)
            {
                case 'r':
                    return getNextTrasitionFromLeft(firstPxSrc, bitmapDataSrc, x, y, ref direction);
                case 'l':
                    return getNextTrasitionFromRight(firstPxSrc, bitmapDataSrc, x, y, ref direction);
                case 'u':
                    return getNextTrasitionFromDown(firstPxSrc, bitmapDataSrc, x, y, ref direction);
                case 'd':
                    return getNextTrasitionFromUp(firstPxSrc, bitmapDataSrc, x, y, ref direction);
                default:
                    return 8;
            }
        }

        private static unsafe int getNextTrasitionFromUp(byte* firstPxSrc, BitmapData bitmapDataSrc, int x, int y, ref char direction)
        {   
            byte* fstByte = firstPxSrc + ((bitmapDataSrc.Stride * (y - 1)) + ((x + 1) * 3)); //1
            byte* sndByte = firstPxSrc + ((bitmapDataSrc.Stride * (y - 1)) + (x * 3)); //2
            if(*fstByte == 255 && *sndByte == 0)
            {
                direction = 'l';
                return 1;
            }
            {
                fstByte = firstPxSrc + ((bitmapDataSrc.Stride * (y - 1)) + (x * 3)); //2
                sndByte = firstPxSrc + ((bitmapDataSrc.Stride * (y - 1)) + ((x - 1) * 3)); //3
                if (*fstByte == 255 && *sndByte == 0)
                {
                    direction = 'l';
                    return 2;
                }
                else
                {
                    fstByte = firstPxSrc + ((bitmapDataSrc.Stride * (y - 1)) + ((x - 1) * 3)); //3
                    sndByte = firstPxSrc + ((bitmapDataSrc.Stride * (y)) + ((x - 1) * 3)); //4
                    if (*fstByte == 255 && *sndByte == 0)
                    {
                        direction = 'd';
                        return 3;
                    }
                    else
                    {
                        fstByte = firstPxSrc + ((bitmapDataSrc.Stride * (y)) + ((x - 1) * 3)); //4
                        sndByte = firstPxSrc + ((bitmapDataSrc.Stride * (y + 1)) + ((x - 1) * 3)); //5
                        if (*fstByte == 255 && *sndByte == 0)
                        {
                            direction = 'd';
                            return 4;
                        }
                        else
                        {
                            fstByte = firstPxSrc + ((bitmapDataSrc.Stride * (y + 1)) + ((x - 1) * 3)); //5
                            sndByte = firstPxSrc + ((bitmapDataSrc.Stride * (y + 1)) + (x * 3)); //6
                            if (*fstByte == 255 && *sndByte == 0)
                            {
                                direction = 'r';
                                return 5;
                            }
                            else
                            {
                                fstByte = firstPxSrc + ((bitmapDataSrc.Stride * (y + 1)) + (x * 3)); //6
                                sndByte = firstPxSrc + ((bitmapDataSrc.Stride * (y + 1)) + ((x + 1) * 3)); //7
                                if (*fstByte == 255 && *sndByte == 0)
                                {
                                    direction = 'r';
                                    return 6;
                                }
                                else
                                {
                                    fstByte = firstPxSrc + ((bitmapDataSrc.Stride * (y + 1)) + ((x + 1) * 3)); //7
                                    sndByte = firstPxSrc + ((bitmapDataSrc.Stride * (y)) + ((x + 1) * 3)); //0
                                    if (*fstByte == 255 && *sndByte == 0)
                                    {
                                        direction = 'u';
                                        return 7;
                                    }
                                    else
                                    {
                                        fstByte = firstPxSrc + ((bitmapDataSrc.Stride * (y)) + ((x + 1) * 3)); //0
                                        sndByte = firstPxSrc + ((bitmapDataSrc.Stride * (y - 1)) + ((x + 1) * 3)); //1
                                        if (*fstByte == 255 && *sndByte == 0)
                                        {
                                            direction = 'u';
                                            return 0;
                                        }
                                        else
                                        {
                                            return 8;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        private static unsafe int getNextTrasitionFromDown(byte* firstPxSrc, BitmapData bitmapDataSrc, int x, int y, ref char direction)
        {
            byte* fstByte = firstPxSrc + ((bitmapDataSrc.Stride * (y + 1)) + ((x - 1) * 3)); //5
            byte* sndByte = firstPxSrc + ((bitmapDataSrc.Stride * (y + 1)) + (x * 3)); //6
            if (*fstByte == 255 && *sndByte == 0)
            {
                direction = 'r';
                return 5;
            }
            else
            {
                fstByte = firstPxSrc + ((bitmapDataSrc.Stride * (y + 1)) + (x * 3)); //6
                sndByte = firstPxSrc + ((bitmapDataSrc.Stride * (y + 1)) + ((x + 1) * 3)); //7
                if (*fstByte == 255 && *sndByte == 0)
                {
                    direction = 'r';
                    return 6;
                }
                else
                {
                    fstByte = firstPxSrc + ((bitmapDataSrc.Stride * (y + 1)) + ((x + 1) * 3)); //7
                    sndByte = firstPxSrc + ((bitmapDataSrc.Stride * (y)) + ((x + 1) * 3)); //0
                    if (*fstByte == 255 && *sndByte == 0)
                    {
                        direction = 'u';
                        return 7;
                    }
                    else
                    {
                        fstByte = firstPxSrc + ((bitmapDataSrc.Stride * (y)) + ((x + 1) * 3)); //0
                        sndByte = firstPxSrc + ((bitmapDataSrc.Stride * (y - 1)) + ((x + 1) * 3)); //1
                        if (*fstByte == 255 && *sndByte == 0)
                        {
                            direction = 'u';
                            return 0;
                        }
                        else
                        {
                            fstByte = firstPxSrc + ((bitmapDataSrc.Stride * (y - 1)) + ((x + 1) * 3)); //1
                            sndByte = firstPxSrc + ((bitmapDataSrc.Stride * (y - 1)) + (x * 3)); //2
                            if (*fstByte == 255 && *sndByte == 0)
                            {
                                direction = 'l';
                                return 1;
                            }
                            else
                            {
                                fstByte = firstPxSrc + ((bitmapDataSrc.Stride * (y - 1)) + (x * 3)); //2
                                sndByte = firstPxSrc + ((bitmapDataSrc.Stride * (y - 1)) + ((x - 1) * 3)); //3
                                if (*fstByte == 255 && *sndByte == 0)
                                {
                                    direction = 'l';
                                    return 2;
                                }
                                else
                                {
                                    fstByte = firstPxSrc + ((bitmapDataSrc.Stride * (y - 1)) + ((x - 1) * 3)); //3
                                    sndByte = firstPxSrc + ((bitmapDataSrc.Stride * (y)) + ((x - 1) * 3)); //4
                                    if (*fstByte == 255 && *sndByte == 0)
                                    {
                                        direction = 'd';
                                        return 3;
                                    }
                                    else
                                    {
                                        fstByte = firstPxSrc + ((bitmapDataSrc.Stride * (y)) + ((x - 1) * 3)); //4
                                        sndByte = firstPxSrc + ((bitmapDataSrc.Stride * (y + 1)) + ((x - 1) * 3)); //5
                                        if (*fstByte == 255 && *sndByte == 0)
                                        {
                                            direction = 'd';
                                            return 4;
                                        }
                                        else
                                        {
                                            fstByte = firstPxSrc + ((bitmapDataSrc.Stride * (y + 1)) + ((x - 1) * 3)); //5
                                            sndByte = firstPxSrc + ((bitmapDataSrc.Stride * (y + 1)) + (x * 3)); //6
                                            if (*fstByte == 255 && *sndByte == 0)
                                            {
                                                direction = 'r';
                                                return 5;
                                            }
                                            else
                                            {
                                                return 8;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        private static unsafe int getNextTrasitionFromRight(byte* firstPxSrc, BitmapData bitmapDataSrc, int x, int y, ref char direction)
        {
            byte* fstByte = firstPxSrc + ((bitmapDataSrc.Stride * (y + 1)) + ((x + 1) * 3)); //7
            byte* sndByte = firstPxSrc + ((bitmapDataSrc.Stride * (y)) + ((x + 1) * 3)); //0
            if (*fstByte == 255 && *sndByte == 0)
            {
                direction = 'u';
                return 7;
            }
            else
            {
                fstByte = firstPxSrc + ((bitmapDataSrc.Stride * (y)) + ((x + 1) * 3)); //0
                sndByte = firstPxSrc + ((bitmapDataSrc.Stride * (y - 1)) + ((x + 1) * 3)); //1
                if (*fstByte == 255 && *sndByte == 0)
                {
                    direction = 'u';
                    return 0;
                }
                else
                {
                    fstByte = firstPxSrc + ((bitmapDataSrc.Stride * (y - 1)) + ((x + 1) * 3)); //1
                    sndByte = firstPxSrc + ((bitmapDataSrc.Stride * (y - 1)) + (x * 3)); //2
                    if (*fstByte == 255 && *sndByte == 0)
                    {
                        direction = 'l';
                        return 1;
                    }
                    else
                    {
                        fstByte = firstPxSrc + ((bitmapDataSrc.Stride * (y - 1)) + (x * 3)); //2
                        sndByte = firstPxSrc + ((bitmapDataSrc.Stride * (y - 1)) + ((x - 1) * 3)); //3
                        if (*fstByte == 255 && *sndByte == 0)
                        {
                            direction = 'l';
                            return 2;
                        }
                        else
                        {
                            fstByte = firstPxSrc + ((bitmapDataSrc.Stride * (y - 1)) + ((x - 1) * 3)); //3
                            sndByte = firstPxSrc + ((bitmapDataSrc.Stride * (y)) + ((x - 1) * 3)); //4
                            if (*fstByte == 255 && *sndByte == 0)
                            {
                                direction = 'd';
                                return 3;
                            }
                            else
                            {
                                fstByte = firstPxSrc + ((bitmapDataSrc.Stride * (y)) + ((x - 1) * 3)); //4
                                sndByte = firstPxSrc + ((bitmapDataSrc.Stride * (y + 1)) + ((x - 1) * 3)); //5
                                if (*fstByte == 255 && *sndByte == 0)
                                {
                                    direction = 'd';
                                    return 4;
                                }
                                else
                                {
                                    fstByte = firstPxSrc + ((bitmapDataSrc.Stride * (y + 1)) + ((x - 1) * 3)); //5
                                    sndByte = firstPxSrc + ((bitmapDataSrc.Stride * (y + 1)) + (x * 3)); //6
                                    if (*fstByte == 255 && *sndByte == 0)
                                    {
                                        direction = 'r';
                                        return 5;
                                    }
                                    else
                                    {
                                        fstByte = firstPxSrc + ((bitmapDataSrc.Stride * (y + 1)) + (x * 3)); //6
                                        sndByte = firstPxSrc + ((bitmapDataSrc.Stride * (y + 1)) + ((x + 1) * 3)); //7
                                        if (*fstByte == 255 && *sndByte == 0)
                                        {
                                            direction = 'r';
                                            return 6;
                                        }
                                        else
                                        {
                                            fstByte = firstPxSrc + ((bitmapDataSrc.Stride * (y + 1)) + ((x + 1) * 3)); //7
                                            sndByte = firstPxSrc + ((bitmapDataSrc.Stride * (y)) + ((x + 1) * 3)); //0
                                            if (*fstByte == 255 && *sndByte == 0)
                                            {
                                                direction = 'u';
                                                return 7;
                                            }
                                            else
                                            {
                                                return 8;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        private static unsafe int getNextTrasitionFromLeft(byte* firstPxSrc, BitmapData bitmapDataSrc, int x, int y, ref char direction)
        {
            byte* fstByte = firstPxSrc + ((bitmapDataSrc.Stride * (y - 1)) + ((x - 1) * 3)); //3
            byte* sndByte = firstPxSrc + ((bitmapDataSrc.Stride * (y)) + ((x - 1) * 3)); //4
            if (*fstByte == 255 && *sndByte == 0)
            {
                direction = 'd';
                return 3;
            }
            else
            { 
                fstByte = firstPxSrc + ((bitmapDataSrc.Stride * (y)) + ((x-1) * 3)); //4
                sndByte = firstPxSrc + ((bitmapDataSrc.Stride * (y + 1)) + ((x - 1) * 3)); //5
                if (*fstByte == 255 && *sndByte == 0)
                {
                    direction = 'd';
                    return 4;
                }
                else
                {
                    fstByte = firstPxSrc + ((bitmapDataSrc.Stride * (y + 1)) + ((x - 1) * 3)); //5
                    sndByte = firstPxSrc + ((bitmapDataSrc.Stride * (y + 1)) + (x * 3)); //6
                    if (*fstByte == 255 && *sndByte == 0)
                    {
                        direction = 'r';
                        return 5;
                    }
                    else
                    {
                        fstByte = firstPxSrc + ((bitmapDataSrc.Stride * (y + 1)) + (x * 3)); //6
                        sndByte = firstPxSrc + ((bitmapDataSrc.Stride * (y + 1)) + ((x + 1) * 3)); //7
                        if (*fstByte == 255 && *sndByte == 0)
                        {
                            direction = 'r';
                            return 6;
                        }
                        else
                        {
                            fstByte = firstPxSrc + ((bitmapDataSrc.Stride * (y + 1)) + ((x + 1) * 3)); //7
                            sndByte = firstPxSrc + ((bitmapDataSrc.Stride * (y)) + ((x + 1) * 3)); //0
                            if (*fstByte == 255 && *sndByte == 0)
                            {
                                direction = 'u';
                                return 7;
                            }
                            else
                            {
                                fstByte = firstPxSrc + ((bitmapDataSrc.Stride * (y)) + ((x + 1) * 3)); //0
                                sndByte = firstPxSrc + ((bitmapDataSrc.Stride * (y - 1)) + ((x + 1) * 3)); //1
                                if (*fstByte == 255 && *sndByte == 0)
                                {
                                    direction = 'u';
                                    return 0;
                                }
                                else
                                {
                                    fstByte = firstPxSrc + ((bitmapDataSrc.Stride * (y - 1)) + ((x + 1) * 3)); //1
                                    sndByte = firstPxSrc + ((bitmapDataSrc.Stride * (y - 1)) + (x * 3)); //2
                                    if (*fstByte == 255 && *sndByte == 0)
                                    {
                                        direction = 'l';
                                        return 1;
                                    }
                                    else
                                    {
                                        fstByte = firstPxSrc + ((bitmapDataSrc.Stride * (y - 1)) + (x * 3)); //2
                                        sndByte = firstPxSrc + ((bitmapDataSrc.Stride * (y - 1)) + ((x - 1) * 3)); //3
                                        if (*fstByte == 255 && *sndByte == 0)
                                        {
                                            direction = 'l';
                                            return 2;
                                        }
                                        else
                                        {
                                            fstByte = firstPxSrc + ((bitmapDataSrc.Stride * (y - 1)) + ((x - 1) * 3)); //3
                                            sndByte = firstPxSrc + ((bitmapDataSrc.Stride * (y)) + ((x - 1) * 3)); //4
                                            if (*fstByte == 255 && *sndByte == 0)
                                            {
                                                direction = 'd';
                                                return 3;
                                            }
                                            else
                                            {
                                                return 8;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
