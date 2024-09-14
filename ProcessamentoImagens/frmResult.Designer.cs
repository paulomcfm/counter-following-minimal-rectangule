// frmResult.Designer.cs
namespace ProcessamentoImagens
{
    partial class frmResult
    {
        private System.Windows.Forms.PictureBox pictureBoxResult;

        private void InitializeComponent()
        {
            this.pictureBoxResult = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxResult)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBoxResult
            // 
            this.pictureBoxResult.Location = new System.Drawing.Point(12, 12);
            this.pictureBoxResult.Name = "pictureBoxResult";
            this.pictureBoxResult.Size = new System.Drawing.Size(1618, 720);
            this.pictureBoxResult.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBoxResult.TabIndex = 0;
            this.pictureBoxResult.TabStop = false;
            // 
            // frmResult
            // 
            this.ClientSize = new System.Drawing.Size(1642, 744);
            this.Controls.Add(this.pictureBoxResult);
            this.Name = "frmResult";
            this.Text = "Resultado Parcial";
            this.Load += new System.EventHandler(this.frmResult_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxResult)).EndInit();
            this.ResumeLayout(false);
        }
    }
}
