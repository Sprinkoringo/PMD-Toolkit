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
using System.Collections;
using PMDToolkit.Maps;

namespace PMDToolkit.Logic.Gameplay {
    public class TargetCollection {

        List<Target> targets;
        List<TileTarget> tiles;

        int self;
        public int Self { get { return self; } }
        int friends;
        public int Friends { get { return friends; } }
        int foes;
        public int Foes { get { return foes; } }

        public List<Target> Targets { get { return targets; } }
        public List<TileTarget> Tiles { get { return tiles; } }

        public TargetCollection() {
            targets = new List<Target>();
            tiles = new List<TileTarget>();
        }

        public void Add(Target target) {
            if (target.TargetAlignment == Enums.Alignment.Self) {
                self++;
            } else if (target.TargetAlignment == Enums.Alignment.Friend) {
                friends++;
            } else if (target.TargetAlignment == Enums.Alignment.Foe) {
                foes++;
            }
            targets.Add(target);
        }

        public void Add(TileTarget tile) {
            tiles.Add(tile);
        }

    }
}
