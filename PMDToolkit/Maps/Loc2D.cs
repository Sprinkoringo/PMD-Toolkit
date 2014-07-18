/*The MIT License (MIT)

Copyright (c) 2014 PMU Staff

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
using System.Linq;
using System.Text;

namespace PMDToolkit.Maps {
    public struct Loc2D {

        public int X;
        public int Y;

        public Loc2D(int n)
        {
            X = n;
            Y = n;
        }

        public Loc2D(int x, int y)
        {
            X = x;
            Y = y;
        }

        public Loc2D(Loc2D loc) {
            X = loc.X;
            Y = loc.Y;
        }


        public static bool operator ==(Loc2D param1, Loc2D param2) {
            return (param1.X == param2.X && param1.Y == param2.Y);
        }

        public static bool operator !=(Loc2D param1, Loc2D param2) {
            return !(param1 == param2);
        }

        public static Loc2D operator +(Loc2D param1, Loc2D param2) {
            return new Loc2D(param1.X + param2.X, param1.Y + param2.Y);
        }

        public static Loc2D operator -(Loc2D param1, Loc2D param2) {
            return new Loc2D(param1.X - param2.X, param1.Y - param2.Y);
        }

        public static Loc2D operator *(Loc2D param1, int param2) {
            return new Loc2D(param1.X * param2, param1.Y * param2);
        }

        public static Loc2D operator /(Loc2D param1, int param2) {
            return new Loc2D(param1.X / param2, param1.Y / param2);
        }

        public static int Dot(Loc2D param1, Loc2D param2)
        {
            return param1.X * param2.X + param1.Y * param2.Y;
        }
    }
}
