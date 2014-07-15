using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DragonOgg.MediaPlayer;

using OpenTK.Audio;
using OpenTK.Audio.OpenAL;
using PMDToolkit.Data;
using PMDToolkit.Graphics;

namespace PMDToolkit.Logic.Results {
    public class SE : IResult {

        //public ResultType Type { get { return ResultType.SE; } }
        public RenderTime Delay { get { return RenderTime.Zero; } }

        string newSE;

        public SE(string newSE) {
            this.newSE = Paths.SoundsPath + newSE + ".ogg";
        }

        public void Execute() {
            AudioManager.SE.PlaySE(newSE, 1);
        }
    }
}
