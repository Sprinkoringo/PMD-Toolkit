using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PMDToolkit.Maps;
using PMDToolkit.Logic.Gameplay;
using PMDToolkit.Logic.Display;
using PMDToolkit.Graphics;

namespace PMDToolkit.Logic.Results {
    public class Loc : IResult {

        //public ResultType Type { get { return ResultType.Loc; } }
        public RenderTime Delay { get { return RenderTime.Zero; } }

        int charIndex;
        Loc2D newLoc;

        public Loc(int charIndex, Loc2D newLoc) {
            this.charIndex = charIndex;
            this.newLoc = newLoc;
        }

        public void Execute() {
            if (charIndex < 0) {
                Screen.Players[charIndex + Gameplay.Processor.MAX_TEAM_SLOTS].CharLoc = newLoc;
            } else {
                Screen.Npcs[charIndex].CharLoc = newLoc;
            }
        }
    }
}
