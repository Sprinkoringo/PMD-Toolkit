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
