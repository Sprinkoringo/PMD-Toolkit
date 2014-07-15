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
    public class CreateAnim : IResult {

        //public ResultType Type { get { return ResultType.CreateAnim; } }

        public RenderTime Delay { get { return RenderTime.Zero; } }

        ISprite anim;
        Display.Screen.EffectPriority priority;

        public CreateAnim(ISprite anim, Display.Screen.EffectPriority priority) {
            this.anim = anim;
            this.priority = priority;
        }

        public void Execute() {
            Screen.Effects[priority].Add(anim);
            anim.Begin();
        }

    }
}
