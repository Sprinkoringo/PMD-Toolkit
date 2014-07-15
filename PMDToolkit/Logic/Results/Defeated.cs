using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PMDToolkit.Logic.Gameplay;
using PMDToolkit.Logic.Display;
using PMDToolkit.Graphics;

namespace PMDToolkit.Logic.Results {
    public class Defeated : IResult {

        //public ResultType Type { get { return ResultType.Defeated; } }

        public RenderTime Delay
        {
            get
            {
                return CharSprite.GetPassTime(charData, dir, CharSprite.ActionType.Defeated);
            }
        }

        int charIndex;
        FormData charData;
        Maps.Direction8 dir;
        public Defeated(int charIndex, ActiveChar character) {
            this.charIndex = charIndex;
            this.charData = character.CharData;
            this.dir = character.CharDir;
        }

        public void Execute() {
            CharSprite sprite;
            if (charIndex < 0) {
                sprite = Screen.Players[charIndex + Gameplay.Processor.MAX_TEAM_SLOTS];
            } else {
                sprite = Screen.Npcs[charIndex];
            }
            sprite.CurrentAction = CharSprite.ActionType.Defeated;
            sprite.ActionTime = RenderTime.Zero;
            sprite.ActionDone = false;
        }

    }
}
