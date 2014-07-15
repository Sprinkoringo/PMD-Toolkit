using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PMDToolkit.Logic.Display;
using PMDToolkit.Graphics;

namespace PMDToolkit.Logic.Results
{
    public class FocusCharacter : IResult
    {
        //public ResultType Type { get { return ResultType.Statused; } }
        public RenderTime Delay { get { return RenderTime.Zero; } }

        int charIndex;
        
        public FocusCharacter(int charIndex) {
            this.charIndex = charIndex;
        }

        public void Execute() {
            Screen.FocusedIndex = charIndex;
        }

    }
}
