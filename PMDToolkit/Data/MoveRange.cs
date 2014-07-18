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

namespace PMDToolkit.Data {
    public class MoveRange {


        public Enums.RangeType RangeType { get; set; }
        public int Mobility { get; set; }
        public bool CutsCorners { get; set; }
        public int Distance { get; set; }

        public bool HitsSelf { get; set; }
        public bool HitsFriend { get; set; }
        public bool HitsFoe { get; set; }

        public MoveRange() {

        }

        public MoveRange(Enums.RangeType rangeType, int mobility, bool cutsCorners, int distance, bool hitsSelf, bool hitsFriend, bool hitsFoe) {
            RangeType = rangeType;
            Mobility = mobility;
            CutsCorners = cutsCorners;
            Distance = distance;
            HitsSelf = hitsSelf;
            HitsFriend = hitsFriend;
            HitsFoe = hitsFoe;
        }

        public MoveRange(MoveRange copy) {
            RangeType = copy.RangeType;
            Mobility = copy.Mobility;
            CutsCorners = copy.CutsCorners;
            Distance = copy.Distance;


            HitsSelf = copy.HitsSelf;
            HitsFriend = copy.HitsFriend;
            HitsFoe = copy.HitsFoe;
        }

    }
}
