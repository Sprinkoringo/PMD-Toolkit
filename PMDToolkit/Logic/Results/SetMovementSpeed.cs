using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PMDToolkit.Logic.Gameplay;
using PMDToolkit.Logic.Display;
using PMDToolkit.Graphics;

namespace PMDToolkit.Logic.Results {
    public class SetMovementSpeed : IResult {

        //public ResultType Type { get { return ResultType.MovementSpeed; } }
        public RenderTime Delay { get { return RenderTime.Zero; } }

        int charIndex;
        int speed;

        public SetMovementSpeed(int charIndex, int speed) {
            this.charIndex = charIndex;
            this.speed = speed;
        }

        public void Execute() {
            CharSprite sprite;
            if (charIndex < 0) {
                sprite = Screen.Players[charIndex + Gameplay.Processor.MAX_TEAM_SLOTS];
            } else {
                sprite = Screen.Npcs[charIndex];
            }
            sprite.MovementSpeed = speed;
        }

    }
}
