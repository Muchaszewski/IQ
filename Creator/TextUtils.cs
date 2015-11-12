using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Creator
{
    public class TextUtils
    {
        public static void IsNumeric(ref TextCompositionEventArgs e, bool isInt)
        {
            if (isInt)
            {
                try
                {
                    Convert.ToInt32(e.Text);
                }
                catch
                {
                    e.Handled = true;
                }
            }
            else
            {
                if (!char.IsDigit(e.Text, e.Text.Length - 1) && e.Text[e.Text.Length - 1] != ',' && e.Text[0] != '-')
                {
                    e.Handled = true;
                }
            }
        }

    }
}
