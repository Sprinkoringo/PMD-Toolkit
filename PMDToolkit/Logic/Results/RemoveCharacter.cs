using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PMDToolkit.Logic.Gameplay;
using PMDToolkit.Logic.Display;
using PMDToolkit.Graphics;

namespace PMDToolkit.Logic.Results {
    public class RemoveCharacter : IResult {

        //public ResultType Type { get { return ResultType.RemoveNpc; } }
        public RenderTime Delay { get { return RenderTime.Zero; } }

        int charIndex;

        public RemoveCharacter(int charIndex) {
            this.charIndex = charIndex;
        }

        public void Execute() {
            if (charIndex < 0)
            {
                Screen.Players[charIndex + Gameplay.Processor.MAX_TEAM_SLOTS] = new PlayerSprite();
            }
            else
            {
                Screen.Npcs[charIndex] = new NpcSprite();
            }
        }

    }
}
