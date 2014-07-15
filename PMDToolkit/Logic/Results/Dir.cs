using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PMDToolkit.Maps;
using PMDToolkit.Logic.Gameplay;
using PMDToolkit.Logic.Display;
using PMDToolkit.Graphics;

namespace PMDToolkit.Logic.Results {
    public class Dir : IResult {

        //public ResultType Type { get { return ResultType.Dir; } }
        public RenderTime Delay { get { return RenderTime.Zero; } }

        int charIndex;
        Direction8 newDir;

        public int CharIndex { get { return charIndex; } }

        public Dir(int charIndex, Direction8 newDir) {
            this.charIndex = charIndex;
            this.newDir = newDir;
        }

        public void Execute() {
            if (charIndex < 0) {
                Screen.Players[charIndex + Gameplay.Processor.MAX_TEAM_SLOTS].CharDir = newDir;
            } else {
                Screen.Npcs[charIndex].CharDir = newDir;
            }
        }
    }
}
