using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Drawing.Imaging;

namespace ProcessamentoImagens
{
    public partial class frmPrincipal : Form
    {
        private Image image;
        private Bitmap imageBitmap;
        private Image thinnedImage;
        private Image counteredImage;

        public frmPrincipal()
        {
            InitializeComponent();
        }

        private void btnAbrirImagem_Click(object sender, EventArgs e)
        {
            openFileDialog.FileName = "";
            openFileDialog.Filter = "Arquivos de Imagem (*.jpg;*.gif;*.bmp;*.png)|*.jpg;*.gif;*.bmp;*.png";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                image = Image.FromFile(openFileDialog.FileName);
                pictBoxImg1.Image = image;
                pictBoxImg1.SizeMode = PictureBoxSizeMode.Normal;
            }
        }

        private void btnLimpar_Click(object sender, EventArgs e)
        {
            pictBoxImg1.Image = null;
            pictBoxImg2.Image = null;
        }

        private void btnZhangSuen_Click(object sender, EventArgs e)
        {
            Bitmap imgDest = new Bitmap(image);
            imageBitmap = (Bitmap)image;
            Filtros.zhangSuen(imageBitmap, imgDest);
            pictBoxImg2.Image = imgDest;
            thinnedImage = imgDest;
            //frmResult frmResult = new frmResult((Bitmap)pictBoxImg2.Image, "Zhang Suen");
            //frmResult.Show();
            string savePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "ZhangSuenResult.png");
            imgDest.Save(savePath, ImageFormat.Png);
        }

        private void btnCounterFollowing_Click(object sender, EventArgs e)
        {
            Bitmap imgDest = new Bitmap(image.Width, image.Height);
            using (Graphics g = Graphics.FromImage(imgDest))
            {
                g.Clear(Color.White);
            }
            pictBoxImg1.Image = thinnedImage;
            Filtros.counterFollowing((Bitmap)thinnedImage, imgDest);
            pictBoxImg2.Image = imgDest;
            counteredImage = imgDest;
            pictBoxImg1.Image = thinnedImage;
            frmResult frmResult = new frmResult((Bitmap)pictBoxImg2.Image, "Counter Following");
            frmResult.Show();
            string savePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "CounterFollowingResult.png");
            imgDest.Save(savePath, ImageFormat.Png);
        }
    }
}
