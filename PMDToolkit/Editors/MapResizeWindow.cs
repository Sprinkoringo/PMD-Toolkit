using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace PMDToolkit.Editors
{
    public partial class MapResizeWindow : Form
    {
        public int Width { get; set; }
        public int Height { get; set; }

        public Maps.Direction8 ResizeDir { get; set; }

        public MapResizeWindow(int width, int height)
        {
            InitializeComponent();

            Width = width;
            Height = height;

            nudWidth.Value = width;
            nudHeight.Value = height;

            ResizeDir = Maps.Direction8.None;

            RefreshResizeDir();
        }

        void RefreshResizeDir()
        {
            
            btnCenter.Text = "";
            btnBottom.Text = "";
            btnLeft.Text = "";
            btnTop.Text = "";
            btnRight.Text = "";
            btnBottomLeft.Text = "";
            btnTopLeft.Text = "";
            btnTopRight.Text = "";
            btnBottomRight.Text = "";

            switch (ResizeDir)
            {
                case Maps.Direction8.None:
                    btnCenter.Text = "X";
                    break;
                case Maps.Direction8.Down:
                    btnBottom.Text = "X";
                    break;
                case Maps.Direction8.Left:
                    btnLeft.Text = "X";
                    break;
                case Maps.Direction8.Up:
                    btnTop.Text = "X";
                    break;
                case Maps.Direction8.Right:
                    btnRight.Text = "X";
                    break;
                case Maps.Direction8.DownLeft:
                    btnBottomLeft.Text = "X";
                    break;
                case Maps.Direction8.UpLeft:
                    btnTopLeft.Text = "X";
                    break;
                case Maps.Direction8.UpRight:
                    btnTopRight.Text = "X";
                    break;
                case Maps.Direction8.DownRight:
                    btnBottomRight.Text = "X";
                    break;
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            Width = (int)nudWidth.Value;
            Height = (int)nudHeight.Value;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnTopLeft_Click(object sender, EventArgs e)
        {
            ResizeDir = Maps.Direction8.UpLeft;
            RefreshResizeDir();
        }

        private void btnTop_Click(object sender, EventArgs e)
        {
            ResizeDir = Maps.Direction8.Up;
            RefreshResizeDir();
        }

        private void btnTopRight_Click(object sender, EventArgs e)
        {
            ResizeDir = Maps.Direction8.UpRight;
            RefreshResizeDir();
        }

        private void btnLeft_Click(object sender, EventArgs e)
        {
            ResizeDir = Maps.Direction8.Left;
            RefreshResizeDir();
        }

        private void btnCenter_Click(object sender, EventArgs e)
        {
            ResizeDir = Maps.Direction8.None;
            RefreshResizeDir();
        }

        private void btnRight_Click(object sender, EventArgs e)
        {
            ResizeDir = Maps.Direction8.Right;
            RefreshResizeDir();
        }

        private void btnBottomLeft_Click(object sender, EventArgs e)
        {
            ResizeDir = Maps.Direction8.DownLeft;
            RefreshResizeDir();
        }

        private void btnBottom_Click(object sender, EventArgs e)
        {
            ResizeDir = Maps.Direction8.Down;
            RefreshResizeDir();
        }

        private void btnBottomRight_Click(object sender, EventArgs e)
        {
            ResizeDir = Maps.Direction8.DownRight;
            RefreshResizeDir();
        }
    }
}
