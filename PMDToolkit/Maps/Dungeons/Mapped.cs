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
using PMDToolkit;
using PMDToolkit.Maps;
using PMDToolkit.Data;


namespace PMDToolkit.Maps.Dungeons {
    public class Mapped : BasicDungeon {

        string fileName;

        public override void Generate(int seed, RDungeonEntry entry)
        {
            Generate(seed, "");
        }

        public void Generate(int seed, string fileName )
        {
            this.seed = seed;
            this.entry = null;

            this.fileName = fileName;
            rand = new Random(seed);

            Start = new Loc3D(this[0].BorderPoints[0].X, this[0].BorderPoints[0].Y, 0);
        }

        //re-call-able floor getting methods, caching where needed
        protected override void generateFloor(int floor)
        {
            if (floor == 0)
            {
                Floors.StandardMap map = new Floors.StandardMap();
                map.Generate(rand.Next(), fileName);
                maps.Add(floor, map);
            }
        }
    }
}
