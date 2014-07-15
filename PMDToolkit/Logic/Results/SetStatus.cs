using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PMDToolkit.Logic.Gameplay;
using PMDToolkit.Logic.Display;
using PMDToolkit.Graphics;

namespace PMDToolkit.Logic.Results {
    public class SetStatus : IResult {

        //public ResultType Type { get { return ResultType.Statused; } }
        public RenderTime Delay { get { return RenderTime.Zero; } }

        int charIndex;
        Enums.StatusAilment ailment;

        public SetStatus(int charIndex, Enums.StatusAilment ailment) {
            this.charIndex = charIndex;
            this.ailment = ailment;
        }

        public void Execute() {
            CharSprite sprite;
            if (charIndex < 0) {
                sprite = Screen.Players[charIndex + Gameplay.Processor.MAX_TEAM_SLOTS];
            } else {
                sprite = Screen.Npcs[charIndex];
            }
            sprite.StatusAilment = ailment;
        }

    }
}
