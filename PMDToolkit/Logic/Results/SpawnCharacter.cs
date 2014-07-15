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
