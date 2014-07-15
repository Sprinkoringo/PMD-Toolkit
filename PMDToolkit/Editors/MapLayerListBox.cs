using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows;
using System.Drawing;

namespace PMDToolkit.Editors
{
    public class MapLayerListBox : CheckedListBox
    {
        private const int WM_LBUTTONDOWN = 0x0201;

        private const int WM_LBUTTONUP = 0x0202;

        private const int WM_LBUTTONDBLCLK = 0x0203;

        protected override void WndProc(ref Message m)
        {
            switch (m.Msg)
            {
                case WM_LBUTTONDOWN:
                    {
                        Point point = PointToClient(Cursor.Position);

                        for (int i = 0; i < Items.Count; i++)
                        {
                            Rectangle checkboxRect = GetItemRectangle(i);
                            checkboxRect.Width = checkboxRect.Height;
                            if (checkboxRect.Contains(point))
                            {
                                SetItemChecked(i, !GetItemChecked(i));
                                break;
                            }
                            else if (GetItemRectangle(i).Contains(point))
                            {
                                SelectedIndex = i;
                                break;
                            }
                        }

                        return;
                    }
                case WM_LBUTTONDBLCLK:
                    {
                        Point point = PointToClient(Cursor.Position);

                        for (int i = 0; i < Items.Count; i++)
                        {
                            Rectangle checkboxRect = GetItemRectangle(i);
                            checkboxRect.Width = checkboxRect.Height;
                            if (checkboxRect.Contains(point))
                            {
                                break;
                            }
                            else if (GetItemRectangle(i).Contains(point))
                            {
                                SelectedIndex = i;
                                break;
                            }
                        }

                        return;
                    }
            }

            base.WndProc(ref m);
        }

    }
}
