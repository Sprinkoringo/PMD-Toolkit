using System;
using System.Collections.Generic;
using System.Text;

namespace PMDToolkit.Core
{
    public class MathFunctions
    {
        #region Methods

        public static int CalculatePercent(int currentValue, int maxValue) {
            return currentValue * 100 / maxValue;
        }


        public static int RoundToMultiple(int number, int multiple) {
            double d = number / multiple;
            d = System.Math.Round(d, 0);
            return Convert.ToInt32(d * multiple);
        }

        public enum Compare {
            Less = -1,
            Equal = 0,
            Greater = 1
        }

        public static Compare CompareValues(int int1, int int2) {
            if (int1 < int2) {
                return Compare.Less;
            } else if (int1 > int2) {
                return Compare.Greater;
            } else {
                return Compare.Equal;
            }
        }

        #endregion Methods
    }
}
