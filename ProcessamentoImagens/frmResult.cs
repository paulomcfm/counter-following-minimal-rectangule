﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProcessamentoImagens
{
    public partial class frmResult : Form
    {
        private Bitmap imageBitmap;
        public frmResult(Bitmap imageBitmap, String title)
        {
            InitializeComponent();

            this.imageBitmap = imageBitmap;
            this.Text = title;
        }

        private void frmResult_Load(object sender, EventArgs e)
        {
            pictureBoxResult.Image = imageBitmap;
        }
    }
}
