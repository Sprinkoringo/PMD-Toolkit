using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using PMDToolkit.Core;

namespace PMDToolkit.Editors
{
    public class IntNumericUpDown : NumericUpDown
    {
        protected override void OnTextBoxKeyPress(object source, KeyPressEventArgs e)
        {
            if (e.KeyChar == 44 || e.KeyChar == 46)
                e.Handled = true;

            base.OnTextBoxKeyPress(source, e);
        }
    }
}
