using System;
using System.Drawing;
using System.Windows.Forms;

namespace Screenote
{
    public partial class Note : Form
    {
        bool MoveWindow = false;
        int MoveX;
        int MoveY;

        public Note(Bitmap bitmap, Point location)
        {
            InitializeComponent();
            this.MinimumSize = new Size(1, 1);
            this.Location = new Point(location.X - 1, location.Y - 1);
            this.Width = bitmap.Width + 2;
            this.Height = bitmap.Height + 2;

            picture.Image = bitmap;
            picture.MouseDown += Note_MouseDown;
            picture.MouseUp += Note_MouseUp;
            picture.MouseMove += Note_MouseMove;
            picture.DoubleClick += Note_DoubleClick;
        }

        private void Note_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                MoveWindow = true;
                MoveX = Control.MousePosition.X - this.Location.X;
                MoveY = Control.MousePosition.Y - this.Location.Y;
            }
            if (e.Button == MouseButtons.Right)
            {
                picture.Image.Dispose();
                this.Close();
                GC.Collect();
            }
            if (e.Button == MouseButtons.Middle)
            {
                Clipboard.SetImage(picture.Image);
            }
        }

        private void Note_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                MoveWindow = false;
            }
        }

        private void Note_MouseMove(object sender, MouseEventArgs e)
        {
            if (MoveWindow == true)
            {
                this.Location = new Point(Control.MousePosition.X - MoveX, Control.MousePosition.Y - MoveY);
            }
        }

        private void Note_DoubleClick(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.DefaultExt = ".png|.jpg|.bmp|.tif";
            saveFileDialog.Filter = "PNG|*.png|JPEG|*.jpg|BMP|*.bmp|TIFF|*.tif";
            saveFileDialog.DefaultExt = ".png";
            saveFileDialog.FileName = DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss") + ".png";
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                switch (saveFileDialog.FileName.Substring(saveFileDialog.FileName.LastIndexOf("."), saveFileDialog.FileName.Length - saveFileDialog.FileName.LastIndexOf(".")).ToLower())
                {
                    case ".png":
                        picture.Image.Save(saveFileDialog.FileName, System.Drawing.Imaging.ImageFormat.Png);
                        break;
                    case ".jpg":
                        picture.Image.Save(saveFileDialog.FileName, System.Drawing.Imaging.ImageFormat.Jpeg);
                        break;
                    case ".bmp":
                        picture.Image.Save(saveFileDialog.FileName, System.Drawing.Imaging.ImageFormat.Bmp);
                        break;
                    case ".tif":
                        picture.Image.Save(saveFileDialog.FileName, System.Drawing.Imaging.ImageFormat.Tiff);
                        break;
                }

            }




        }
    }
}
