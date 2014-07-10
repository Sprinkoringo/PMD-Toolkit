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
using System.Linq;
using System.Text;

namespace PMDToolkit.Maps {
    public struct TileData {


        public Enums.TileType Type;
        public int Data1;
        public int Data2;
        public int Data3;

        public string String1;
        public string String2;
        public string String3;

        public TileData(Enums.TileType type, int data1, int data2, int data3, string String1, string String2, string String3) {
            Type = type;
            Data1 = data1;
            Data2 = data2;
            Data3 = data3;
            this.String1 = String1;
            this.String2 = String2;
            this.String3 = String3;
        }

    }
}
