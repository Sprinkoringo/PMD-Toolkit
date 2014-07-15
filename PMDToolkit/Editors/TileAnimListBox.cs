using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows;
using System.Drawing;

namespace PMDToolkit.Editors
{
    public class TileAnimListBox : ListBox
    {
        private const int WM_LBUTTONDOWN = 0x0201;

        private const int WM_LBUTTONUP = 0x0202;

        private const int WM_LBUTTONDBLCLK = 0x0203;

        protected override void WndProc(ref Message m)
        {
            switch (m.Msg)
            {
                case WM_LBUTTONDOWN:
                case WM_LBUTTONDBLCLK:
                    {
                        Point point = PointToClient(Cursor.Position);

                        for (int i = 0; i < Items.Count; i++)
                        {
                            if (GetItemRectangle(i).Contains(point))
                            {
                                if (SelectedIndex != i)
                                {
                                    SelectedIndex = i;
                                }
                                else
                                {
                                    SelectedIndex = -1;
                                }
                                return;
                            }
                        }
                        break;
                    }
            }

            base.WndProc(ref m);
        }




    }
}
