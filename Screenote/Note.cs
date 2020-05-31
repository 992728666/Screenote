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
        Size NoteSize;

        bool ResizeWindow = false;

        internal enum Direction
        {
            Left,
            Right,
            Top,
            Bottom,
            LeftTop,
            LeftBottom,
            RightTop,
            RightBottom
        }

        Direction direction;

        public Note(Bitmap bitmap, Point location, Size size)
        {
            InitializeComponent();
            NoteSize = new Size(size.Width + 2, size.Height + 2);
            this.MinimumSize = new Size(16, 16);
            this.MaximumSize = SystemInformation.VirtualScreen.Size;
            this.Location = new Point(location.X - 1, location.Y - 1);
            this.Width = NoteSize.Width;
            this.Height = NoteSize.Height;

            picture.BackgroundImage = bitmap;
            picture.MouseDown += Note_MouseDown;
            picture.MouseUp += Note_MouseUp;
            picture.MouseMove += Note_MouseMove;
            picture.DoubleClick += Note_DoubleClick;
        }

        private void Note_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if ((Control.MousePosition.X - this.Location.X < 7) || ((this.Location.X + this.Width) - Control.MousePosition.X < 7) || (Control.MousePosition.Y - this.Location.Y < 7) || ((this.Location.Y + this.Height) - Control.MousePosition.Y < 7))
                {
                    WindowRight = this.Right;
                    WindowBottom = this.Bottom;
                    ResizeWindow = true;
                    return;
                }

                MoveX = e.X;
                MoveY = e.Y;
                MoveWindow = true;
            }
            if (e.Button == MouseButtons.Right)
            {
                picture.BackgroundImage.Dispose();
                this.Close();
                GC.Collect();
            }
            if (e.Button == MouseButtons.Middle)
            {
                Clipboard.SetImage(picture.BackgroundImage);
            }
        }

        private void Note_MouseUp(object sender, MouseEventArgs e)
        {
            if (ResizeWindow == true)
            {
                ResizeWindow = false;
                return;
            }

            if (MoveWindow == true)
            {
                MoveWindow = false;
            }
        }

        int WindowRight, WindowBottom;
        private void ResizeWindowLeft()
        {
            if (WindowRight - MousePosition.X > 15)
            {
                this.Width = WindowRight - MousePosition.X;
                this.Location = new Point(MousePosition.X, this.Location.Y);
            }
        }

        private void ResizeWindowTop()
        {
            if (WindowBottom - MousePosition.Y > 15)
            {
                this.Height = WindowBottom - MousePosition.Y;
                this.Location = new Point(this.Location.X, MousePosition.Y);
            }
        }

        private void Note_MouseMove(object sender, MouseEventArgs e)
        {
            if (MoveWindow == true)
            {
                this.Location = new Point(Control.MousePosition.X - MoveX, Control.MousePosition.Y - MoveY);
                return;
            }

            if (ResizeWindow == true)
            {
                switch (direction)
                {
                    case Direction.Left:
                        ResizeWindowLeft();
                        Cursor.Current = Cursors.SizeWE;
                        break;
                    case Direction.Right:
                        this.Width = MousePosition.X - this.Left;
                        Cursor.Current = Cursors.SizeWE;
                        break;
                    case Direction.Top:
                        ResizeWindowTop();
                        Cursor.Current = Cursors.SizeNS;
                        break;
                    case Direction.Bottom:
                        this.Height = MousePosition.Y - this.Top;
                        Cursor.Current = Cursors.SizeNS;
                        break;
                    case Direction.LeftTop:
                        ResizeWindowLeft();
                        ResizeWindowTop();
                        Cursor.Current = Cursors.SizeNWSE;
                        break;
                    case Direction.LeftBottom:
                        ResizeWindowLeft();
                        this.Height = MousePosition.Y - this.Top;
                        Cursor.Current = Cursors.SizeNESW;
                        break;
                    case Direction.RightTop:
                        this.Width = MousePosition.X - this.Left;
                        ResizeWindowTop();
                        Cursor.Current = Cursors.SizeNESW;
                        break;
                    case Direction.RightBottom:
                        this.Width = MousePosition.X - this.Left;
                        this.Height = MousePosition.Y - this.Top;
                        Cursor.Current = Cursors.SizeNWSE;
                        break;
                }

                return;
            }
            else
            {
                if (Control.MousePosition.X - this.Location.X < 7)
                {
                    Cursor.Current = Cursors.SizeWE;
                    direction = Direction.Left;
                }
                if ((this.Location.X + this.Width) - Control.MousePosition.X < 7)
                {
                    Cursor.Current = Cursors.SizeWE;
                    direction = Direction.Right;
                }
                if (Control.MousePosition.Y - this.Location.Y < 7)
                {
                    Cursor.Current = Cursors.SizeNS;
                    direction = Direction.Top;
                }
                if ((this.Location.Y + this.Height) - Control.MousePosition.Y < 7)
                {
                    Cursor.Current = Cursors.SizeNS;
                    direction = Direction.Bottom;
                }
                if ((Control.MousePosition.X - this.Location.X < 7) && (Control.MousePosition.Y - this.Location.Y < 7))
                {
                    Cursor.Current = Cursors.SizeNWSE;
                    direction = Direction.LeftTop;
                }
                if ((Control.MousePosition.X - this.Location.X < 7) && ((this.Location.Y + this.Height) - Control.MousePosition.Y < 7))
                {
                    Cursor.Current = Cursors.SizeNESW;
                    direction = Direction.LeftBottom;
                }
                if (((this.Location.X + this.Width) - Control.MousePosition.X < 7) && (Control.MousePosition.Y - this.Location.Y < 7))
                {
                    Cursor.Current = Cursors.SizeNESW;
                    direction = Direction.RightTop;
                }
                if (((this.Location.X + this.Width) - Control.MousePosition.X < 7) && ((this.Location.Y + this.Height) - Control.MousePosition.Y < 7))
                {
                    Cursor.Current = Cursors.SizeNWSE;
                    direction = Direction.RightBottom;
                }
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
                        picture.BackgroundImage.Save(saveFileDialog.FileName, System.Drawing.Imaging.ImageFormat.Png);
                        break;
                    case ".jpg":
                        picture.BackgroundImage.Save(saveFileDialog.FileName, System.Drawing.Imaging.ImageFormat.Jpeg);
                        break;
                    case ".bmp":
                        picture.BackgroundImage.Save(saveFileDialog.FileName, System.Drawing.Imaging.ImageFormat.Bmp);
                        break;
                    case ".tif":
                        picture.BackgroundImage.Save(saveFileDialog.FileName, System.Drawing.Imaging.ImageFormat.Tiff);
                        break;
                }

            }

        }

        private void Note_KeyDown(object sender, KeyEventArgs e)
        {
            int shift = 1;
            if (e.Shift)
            {
                shift = 10;
            }

            switch (e.KeyCode)
            {
                case Keys.Oemtilde:
                    {
                        int width = this.Width, height = this.Height;
                        if (e.Shift)
                        {
                            this.Width = NoteSize.Width;
                            this.Height = NoteSize.Height;
                        }
                        else
                        {
                            if (this.Width / this.Height > NoteSize.Width / NoteSize.Height)
                            {
                                this.Width = this.Height * NoteSize.Width / NoteSize.Height;
                            }
                            else
                            {
                                this.Height = this.Width * NoteSize.Height / NoteSize.Width;
                            }
                        }
                        this.Location = new Point(this.Location.X + (width - this.Width) / 2, this.Location.Y + (height - this.Height) / 2);
                        break;
                    }
                case Keys.Left:
                    this.Location = new Point(this.Location.X - shift, this.Location.Y);
                    break;
                case Keys.Right:
                    this.Location = new Point(this.Location.X + shift, this.Location.Y);
                    break;
                case Keys.Up:
                    this.Location = new Point(this.Location.X, this.Location.Y - shift);
                    break;
                case Keys.Down:
                    this.Location = new Point(this.Location.X, this.Location.Y + shift);
                    break;
                case Keys.Space:
                    Note_MouseDown(this, new MouseEventArgs(MouseButtons.Middle, 0, Cursor.Position.X, Cursor.Position.Y, 0));
                    break;
                case Keys.Escape:
                    Note_MouseDown(this, new MouseEventArgs(MouseButtons.Right, 0, Cursor.Position.X, Cursor.Position.Y, 0));
                    break;
                case Keys.Enter:
                    Note_DoubleClick(this, new EventArgs());
                    break;
            }
        }

    }
}
