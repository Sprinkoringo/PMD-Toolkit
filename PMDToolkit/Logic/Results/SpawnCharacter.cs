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
using PMDToolkit.Maps;
using PMDToolkit.Logic.Gameplay;
using PMDToolkit.Logic.Display;
using PMDToolkit.Graphics;

namespace PMDToolkit.Logic.Results {
    public class SpawnCharacter : IResult {

        //public ResultType Type { get { return ResultType.SpawnNpc; } }
        public RenderTime Delay { get { return RenderTime.Zero; } }

        int charIndex;
        Loc2D charLoc;
        Direction8 charDir;
        FormData form;

        public SpawnCharacter(ActiveChar character, int charIndex) {

            this.charIndex = charIndex;
            this.charLoc = character.CharLoc;
            this.charDir = character.CharDir;
            this.form = character.CharData;
        }

        public void Execute() {
            if (charIndex < 0)
            {
                Screen.Players[charIndex + Gameplay.Processor.MAX_TEAM_SLOTS] = new PlayerSprite(charLoc, charDir, form);
            }
            else
            {
                Screen.Npcs[charIndex] = new NpcSprite(charLoc, charDir, form);
            }
        }

    }
}
