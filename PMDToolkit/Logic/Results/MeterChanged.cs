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
using PMDToolkit.Logic.Gameplay;
using PMDToolkit.Logic.Display;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using PMDToolkit.Graphics;

namespace PMDToolkit.Logic.Results {
    public class MeterChanged : IResult {

        //public ResultType Type { get { return ResultType.MeterChanged; } }
        public RenderTime Delay { get { return RenderTime.Zero; } }

        int charIndex;
        bool isPP;
        int amt;

        public MeterChanged(int charIndex, bool PP, int amount) {
            this.charIndex = charIndex;
            isPP = PP;
            amt = amount;
        }

        public void Execute() {
            CharSprite sprite;
            if (charIndex < 0) {
                sprite = Screen.Players[charIndex + Gameplay.Processor.MAX_TEAM_SLOTS];
            } else {
                sprite = Screen.Npcs[charIndex];
            }
            if (amt < 0) {
                if (isPP) {
                    Screen.Effects[Screen.EffectPriority.Overlay].Add(new TextSprite(sprite.CharLoc, amt.ToString(), Color4.MediumPurple));
                } else {
                    Screen.Effects[Screen.EffectPriority.Overlay].Add(new TextSprite(sprite.CharLoc, amt.ToString(), Color4.Orange));
                }
            } else if (amt > 0) {
                if (isPP) {
                    Screen.Effects[Screen.EffectPriority.Overlay].Add(new TextSprite(sprite.CharLoc, amt.ToString(), Color4.LightBlue));
                } else {
                    Screen.Effects[Screen.EffectPriority.Overlay].Add(new TextSprite(sprite.CharLoc, amt.ToString(), Color4.LightGreen));
                }
            }
        }

    }
}
