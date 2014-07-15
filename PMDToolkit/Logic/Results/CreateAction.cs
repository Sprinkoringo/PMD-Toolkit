using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PMDToolkit.Maps;
using PMDToolkit.Logic.Gameplay;
using PMDToolkit.Logic.Display;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using PMDToolkit.Graphics;

namespace PMDToolkit.Logic.Results {
    public class CreateAction : IResult {
        //an animation performed by a character as though in a spell
        //differs from hurt or defeat in that it can contain those animations, but is not limited to them
        //may require attack, throw, and jump to be removed (they are solely used in battle context)
        //public ResultType Type { get { return ResultType.CreateAction; } }

        public RenderTime Delay { get { return InstantPass ? RenderTime.Zero : CharSprite.GetPassTime(charData, dir, Action); } }
        
        int charIndex;
        FormData charData;
        Maps.Direction8 dir;
        public CharSprite.ActionType Action { get; set; }
        public bool Looping { get; set; }
        public bool InPlace { get; set; }
        public bool InstantPass { get; set; }

        public int CharIndex { get { return charIndex; } }

        public CreateAction(int charIndex, ActiveChar character, CharSprite.ActionType action)
        {
            this.charIndex = charIndex;
            this.Action = action;
            this.charData = character.CharData;
            this.dir = character.CharDir;
        }

        public CreateAction(int charIndex, ActiveChar character, CharSprite.ActionType action, bool looping, bool inPlace)
        {
            this.charIndex = charIndex;
            this.Action = action;
            this.charData = character.CharData;
            this.dir = character.CharDir;
            this.Looping = looping;
            this.InPlace = inPlace;
        }


        public void Execute() {
            CharSprite sprite;
            if (charIndex < 0) {
                sprite = Screen.Players[charIndex + Gameplay.Processor.MAX_TEAM_SLOTS];
            } else {
                sprite = Screen.Npcs[charIndex];
            }
            if (sprite.CurrentAction == Action)
                sprite.PrevActionTime += sprite.ActionTime;
            else
                sprite.PrevActionTime = RenderTime.Zero;
            sprite.CurrentAction = Action;
            sprite.ActionTime = RenderTime.Zero;
            sprite.ActionDone = false;
            sprite.ActionLoop = Looping;
            sprite.MoveInPlace = InPlace;
        }

    }
}
