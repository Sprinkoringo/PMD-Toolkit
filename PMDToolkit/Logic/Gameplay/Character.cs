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
using PMDToolkit.Maps;

namespace PMDToolkit.Logic.Gameplay {

    public class Character {

        public string BaseName { get; set; }

        public FormData BaseCharData;

        public int Level { get; set; }

        public Move[] BaseMoves { get; set; }

        //stat boosts from vitamins
        public int MaxHPBonus { get; set; }
        public int AtkBonus { get; set; }
        public int DefBonus { get; set; }
        public int SpAtkBonus { get; set; }
        public int SpDefBonus { get; set; }
        public int SpeedBonus { get; set; }

        
        public Character() {
            BaseMoves = new Move[Processor.MAX_MOVE_SLOTS];
            for (int i = 0; i < Processor.MAX_MOVE_SLOTS; i++)
            {
                BaseMoves[i] = new Move();
            }
        }
        
        public Character(Loc2D newLoc, Direction8 charDir) {
            //clean variables
            BaseMoves = new Move[Processor.MAX_MOVE_SLOTS];
            for (int i = 0; i < Processor.MAX_MOVE_SLOTS; i++)
            {
                BaseMoves[i] = new Move();
            }
        }
    }
}
