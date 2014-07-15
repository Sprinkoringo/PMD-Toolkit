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
    public class CreateEmitter : IResult {

        //public ResultType Type { get { return ResultType.CreateEmitter; } }

        public RenderTime Delay { get { return RenderTime.Zero; } }

        IEmitter emitter;

        public CreateEmitter(IEmitter emitter) {
            this.emitter = emitter;
        }

        public void Execute() {
            Screen.Emitters.Add(emitter);
        }
    }
}
