namespace ProcessamentoImagens
{
    partial class frmPrincipal
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.pictBoxImg1 = new System.Windows.Forms.PictureBox();
            this.pictBoxImg2 = new System.Windows.Forms.PictureBox();
            this.btnAbrirImagem = new System.Windows.Forms.Button();
            this.btnLimpar = new System.Windows.Forms.Button();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.btnLuminanciaSemDMA = new System.Windows.Forms.Button();
            this.btnLuminanciaComDMA = new System.Windows.Forms.Button();
            this.btnNegativoComDMA = new System.Windows.Forms.Button();
            this.btnNegativoSemDMA = new System.Windows.Forms.Button();
            this.btnEspHorSDMA = new System.Windows.Forms.Button();
            this.btnEspHorCDMA = new System.Windows.Forms.Button();
            this.btnRot90SDMA = new System.Windows.Forms.Button();
            this.btnRot90CMDA = new System.Windows.Forms.Button();
            this.btnSegmentarObjetos = new System.Windows.Forms.Button();
            this.btnSegmentarObjetosDMA = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictBoxImg1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictBoxImg2)).BeginInit();
            this.SuspendLayout();
            // 
            // pictBoxImg1
            // 
            this.pictBoxImg1.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.pictBoxImg1.Location = new System.Drawing.Point(7, 7);
            this.pictBoxImg1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.pictBoxImg1.Name = "pictBoxImg1";
            this.pictBoxImg1.Size = new System.Drawing.Size(800, 615);
            this.pictBoxImg1.TabIndex = 102;
            this.pictBoxImg1.TabStop = false;
            // 
            // pictBoxImg2
            // 
            this.pictBoxImg2.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.pictBoxImg2.Location = new System.Drawing.Point(815, 7);
            this.pictBoxImg2.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.pictBoxImg2.Name = "pictBoxImg2";
            this.pictBoxImg2.Size = new System.Drawing.Size(800, 615);
            this.pictBoxImg2.TabIndex = 105;
            this.pictBoxImg2.TabStop = false;
            // 
            // btnAbrirImagem
            // 
            this.btnAbrirImagem.Location = new System.Drawing.Point(7, 630);
            this.btnAbrirImagem.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnAbrirImagem.Name = "btnAbrirImagem";
            this.btnAbrirImagem.Size = new System.Drawing.Size(135, 28);
            this.btnAbrirImagem.TabIndex = 106;
            this.btnAbrirImagem.Text = "Abrir Imagem";
            this.btnAbrirImagem.UseVisualStyleBackColor = true;
            this.btnAbrirImagem.Click += new System.EventHandler(this.btnAbrirImagem_Click);
            // 
            // btnLimpar
            // 
            this.btnLimpar.Location = new System.Drawing.Point(7, 666);
            this.btnLimpar.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnLimpar.Name = "btnLimpar";
            this.btnLimpar.Size = new System.Drawing.Size(135, 28);
            this.btnLimpar.TabIndex = 107;
            this.btnLimpar.Text = "Limpar";
            this.btnLimpar.UseVisualStyleBackColor = true;
            this.btnLimpar.Click += new System.EventHandler(this.btnLimpar_Click);
            // 
            // btnLuminanciaSemDMA
            // 
            this.btnLuminanciaSemDMA.Location = new System.Drawing.Point(149, 630);
            this.btnLuminanciaSemDMA.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnLuminanciaSemDMA.Name = "btnLuminanciaSemDMA";
            this.btnLuminanciaSemDMA.Size = new System.Drawing.Size(220, 28);
            this.btnLuminanciaSemDMA.TabIndex = 108;
            this.btnLuminanciaSemDMA.Text = "Luminância sem DMA";
            this.btnLuminanciaSemDMA.UseVisualStyleBackColor = true;
            this.btnLuminanciaSemDMA.Click += new System.EventHandler(this.btnLuminanciaSemDMA_Click);
            // 
            // btnLuminanciaComDMA
            // 
            this.btnLuminanciaComDMA.Location = new System.Drawing.Point(149, 666);
            this.btnLuminanciaComDMA.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnLuminanciaComDMA.Name = "btnLuminanciaComDMA";
            this.btnLuminanciaComDMA.Size = new System.Drawing.Size(220, 28);
            this.btnLuminanciaComDMA.TabIndex = 109;
            this.btnLuminanciaComDMA.Text = "Luminância com DMA";
            this.btnLuminanciaComDMA.UseVisualStyleBackColor = true;
            this.btnLuminanciaComDMA.Click += new System.EventHandler(this.btnLuminanciaComDMA_Click);
            // 
            // btnNegativoComDMA
            // 
            this.btnNegativoComDMA.Location = new System.Drawing.Point(377, 665);
            this.btnNegativoComDMA.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnNegativoComDMA.Name = "btnNegativoComDMA";
            this.btnNegativoComDMA.Size = new System.Drawing.Size(220, 28);
            this.btnNegativoComDMA.TabIndex = 111;
            this.btnNegativoComDMA.Text = "Negativo com DMA";
            this.btnNegativoComDMA.UseVisualStyleBackColor = true;
            this.btnNegativoComDMA.Click += new System.EventHandler(this.btnNegativoComDMA_Click);
            // 
            // btnNegativoSemDMA
            // 
            this.btnNegativoSemDMA.Location = new System.Drawing.Point(377, 629);
            this.btnNegativoSemDMA.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnNegativoSemDMA.Name = "btnNegativoSemDMA";
            this.btnNegativoSemDMA.Size = new System.Drawing.Size(220, 28);
            this.btnNegativoSemDMA.TabIndex = 110;
            this.btnNegativoSemDMA.Text = "Negativo sem DMA";
            this.btnNegativoSemDMA.UseVisualStyleBackColor = true;
            this.btnNegativoSemDMA.Click += new System.EventHandler(this.btnNegativoSemDMA_Click);
            // 
            // btnEspHorSDMA
            // 
            this.btnEspHorSDMA.Location = new System.Drawing.Point(604, 629);
            this.btnEspHorSDMA.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnEspHorSDMA.Name = "btnEspHorSDMA";
            this.btnEspHorSDMA.Size = new System.Drawing.Size(220, 28);
            this.btnEspHorSDMA.TabIndex = 112;
            this.btnEspHorSDMA.Text = "Espelhar Horizontal s DMA";
            this.btnEspHorSDMA.UseVisualStyleBackColor = true;
            this.btnEspHorSDMA.Click += new System.EventHandler(this.btnEspHorSDMA_Click);
            // 
            // btnEspHorCDMA
            // 
            this.btnEspHorCDMA.Location = new System.Drawing.Point(604, 666);
            this.btnEspHorCDMA.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnEspHorCDMA.Name = "btnEspHorCDMA";
            this.btnEspHorCDMA.Size = new System.Drawing.Size(220, 28);
            this.btnEspHorCDMA.TabIndex = 113;
            this.btnEspHorCDMA.Text = "Espelhar Horizontal c DMA";
            this.btnEspHorCDMA.UseVisualStyleBackColor = true;
            this.btnEspHorCDMA.Click += new System.EventHandler(this.btnEspHorCDMA_Click);
            // 
            // btnRot90SDMA
            // 
            this.btnRot90SDMA.Location = new System.Drawing.Point(829, 629);
            this.btnRot90SDMA.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnRot90SDMA.Name = "btnRot90SDMA";
            this.btnRot90SDMA.Size = new System.Drawing.Size(220, 28);
            this.btnRot90SDMA.TabIndex = 114;
            this.btnRot90SDMA.Text = "Rotação 90º S DMA";
            this.btnRot90SDMA.UseVisualStyleBackColor = true;
            this.btnRot90SDMA.Click += new System.EventHandler(this.btnRot90SDMA_Click);
            // 
            // btnRot90CMDA
            // 
            this.btnRot90CMDA.Location = new System.Drawing.Point(829, 662);
            this.btnRot90CMDA.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnRot90CMDA.Name = "btnRot90CMDA";
            this.btnRot90CMDA.Size = new System.Drawing.Size(220, 28);
            this.btnRot90CMDA.TabIndex = 115;
            this.btnRot90CMDA.Text = "Rotação 90º C DMA";
            this.btnRot90CMDA.UseVisualStyleBackColor = true;
            this.btnRot90CMDA.Click += new System.EventHandler(this.btnRot90CMDA_Click);
            // 
            // btnSegmentarObjetos
            // 
            this.btnSegmentarObjetos.Location = new System.Drawing.Point(1055, 630);
            this.btnSegmentarObjetos.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnSegmentarObjetos.Name = "btnSegmentarObjetos";
            this.btnSegmentarObjetos.Size = new System.Drawing.Size(220, 28);
            this.btnSegmentarObjetos.TabIndex = 116;
            this.btnSegmentarObjetos.Text = "Segmentar Objetos";
            this.btnSegmentarObjetos.UseVisualStyleBackColor = true;
            this.btnSegmentarObjetos.Click += new System.EventHandler(this.btnSegmentarObjetos_Click);
            // 
            // btnSegmentarObjetosDMA
            // 
            this.btnSegmentarObjetosDMA.Location = new System.Drawing.Point(1055, 662);
            this.btnSegmentarObjetosDMA.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnSegmentarObjetosDMA.Name = "btnSegmentarObjetosDMA";
            this.btnSegmentarObjetosDMA.Size = new System.Drawing.Size(220, 28);
            this.btnSegmentarObjetosDMA.TabIndex = 117;
            this.btnSegmentarObjetosDMA.Text = "Segmentar Objetos DMA";
            this.btnSegmentarObjetosDMA.UseVisualStyleBackColor = true;
            this.btnSegmentarObjetosDMA.Click += new System.EventHandler(this.btnSegmentarObjetosDMA_Click);
            // 
            // frmPrincipal
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1627, 748);
            this.Controls.Add(this.btnSegmentarObjetosDMA);
            this.Controls.Add(this.btnSegmentarObjetos);
            this.Controls.Add(this.btnRot90CMDA);
            this.Controls.Add(this.btnRot90SDMA);
            this.Controls.Add(this.btnEspHorCDMA);
            this.Controls.Add(this.btnEspHorSDMA);
            this.Controls.Add(this.btnNegativoComDMA);
            this.Controls.Add(this.btnNegativoSemDMA);
            this.Controls.Add(this.btnLuminanciaComDMA);
            this.Controls.Add(this.btnLuminanciaSemDMA);
            this.Controls.Add(this.btnLimpar);
            this.Controls.Add(this.btnAbrirImagem);
            this.Controls.Add(this.pictBoxImg2);
            this.Controls.Add(this.pictBoxImg1);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "frmPrincipal";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Formulário Principal";
            ((System.ComponentModel.ISupportInitialize)(this.pictBoxImg1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictBoxImg2)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pictBoxImg1;
        private System.Windows.Forms.PictureBox pictBoxImg2;
        private System.Windows.Forms.Button btnAbrirImagem;
        private System.Windows.Forms.Button btnLimpar;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.Button btnLuminanciaSemDMA;
        private System.Windows.Forms.Button btnLuminanciaComDMA;
        private System.Windows.Forms.Button btnNegativoComDMA;
        private System.Windows.Forms.Button btnNegativoSemDMA;
        private System.Windows.Forms.Button btnEspHorSDMA;
        private System.Windows.Forms.Button btnEspHorCDMA;
        private System.Windows.Forms.Button btnRot90SDMA;
        private System.Windows.Forms.Button btnRot90CMDA;
        private System.Windows.Forms.Button btnSegmentarObjetos;
        private System.Windows.Forms.Button btnSegmentarObjetosDMA;
    }
}

