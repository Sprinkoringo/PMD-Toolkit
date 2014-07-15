using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;

namespace PMDToolkit.Editors
{
    public class PathTextBox : TextBox
    {
        [DllImport("user32.dll")]
        static extern bool HideCaret(IntPtr hWnd);

        public PathTextBox()
        {
            this.ReadOnly = true;
            this.BackColor = Color.White;
            this.Click += TextBox_Click;
            this.Enter += TextBox_Enter;
            //this.GotFocus += TextBox_GotFocus;
            this.Cursor = Cursors.Arrow; // mouse cursor like in other controls
        }

        private void TextBox_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbdPath = new FolderBrowserDialog();
            fbdPath.SelectedPath = Path.GetFullPath(Text);
            if (fbdPath.ShowDialog() == DialogResult.OK)
            {
                Text = fbdPath.SelectedPath.EndsWith("\\") ? fbdPath.SelectedPath : fbdPath.SelectedPath + "\\";
            }
        }

        private void TextBox_Enter(object sender, EventArgs e)
        {
            Enabled = false;
            Enabled = true;
        }

        //private void TextBox_GotFocus(object sender, EventArgs e)
        //{
        //    HideCaret(this.Handle);
        //}
    }
}
