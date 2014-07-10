/*The MIT License (MIT)

Copyright (c) 2014 Sprinkoringo

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE.
*/


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
