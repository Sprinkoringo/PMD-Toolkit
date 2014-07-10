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
    public abstract class BasicDungeon {

        protected int seed;
        protected Random rand;
        public int Seed { get { return seed; } }
        
        protected Data.RDungeonEntry entry;

        //All dungeons have precisely one entry point
        public Loc3D Start { get; set; }

        protected Dictionary<int, RandomMap> maps;

        public int MapCount { get { return maps.Count; } }

        protected Dictionary<FloorLink, FloorLink> floorLinks;
        

        public RandomMap this[int i] {
            get {
                if (!maps.ContainsKey(i)) {
                    generateFloor(i);
                }
                if (!maps.ContainsKey(i))
                    return null;

                return maps[i];
            }
        }


		public BasicDungeon() {
            maps = new Dictionary<int, RandomMap>();
            floorLinks = new Dictionary<FloorLink, FloorLink>();
        }

        //dungeon structure is always decided upon before the details
        public abstract void Generate(int seed, Data.RDungeonEntry entry);

        protected abstract void generateFloor(int floor);

        //if given an exit from a floor, returns the entrance of the floor it will wind up in
        public FloorLink GetFloorLink(int floor, int exit)
        {
            FloorLink landing = floorLinks[new FloorLink(floor, exit)];
            return landing;
        }
    }
}
