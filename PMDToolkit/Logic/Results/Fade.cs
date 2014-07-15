using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PMDToolkit.Graphics;

namespace PMDToolkit.Logic.Results {
    public class Fade : IResult {

        //public ResultType Type { get { return ResultType.Fade; } }

        public RenderTime Delay { get { return Display.Screen.TOTAL_FADE_TIME; } }

        Display.Screen.FadeType fadeType;

        public Fade(Display.Screen.FadeType fadeType) {
            this.fadeType = fadeType;
        }

        public void Execute() {
            Display.Screen.FadeTime = Display.Screen.TOTAL_FADE_TIME;
            Display.Screen.CurrentFade = fadeType;
        }

    }
}
