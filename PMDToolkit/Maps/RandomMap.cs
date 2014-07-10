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
using PMDToolkit.Logic.Gameplay;
using PMDToolkit.Logic.Display;
using PMDToolkit.Logic.Results;
using System.IO;

namespace PMDToolkit.Maps {
    public abstract class RandomMap : ActiveMap {

        protected int seed;
        protected Random rand;

        protected Data.RDungeonFloor entry;

        public List<FloorBorder> FloorBorders { get; set; }
        public Dictionary<int, List<int>> BorderLinks { get; set; }

        //includes all start points and end points
        public Loc2D[] BorderPoints { get; set; }

        public RandomMap()
        {

        }

        //recalculates wall/water/slippery textures after a change
        public void RecalculateArea(Loc2D start, Loc2D end) {
            for (int x = start.X; x <= end.X; x++) {
                for (int y = start.Y; y <= end.Y; y++) {
                    //recalculate water

                    //recalculate slippery (cover)

                    //recalculate wall
                }
            }
        }

        public abstract void Generate(int seed, Data.RDungeonFloor entry, List<FloorBorder> floorBorders, Dictionary<int, List<int>> borderLinks);



        public void Save(BinaryWriter writer)
        {
            base.Save(writer);
            //write to memory stream

        }

        public void Load(BinaryReader reader)
        {
            base.Load(reader);
            //read from memory stream
        }

    }
}
