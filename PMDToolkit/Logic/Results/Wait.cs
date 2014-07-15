using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PMDToolkit.Graphics;

namespace PMDToolkit.Logic.Results {
    public class Wait : IResult {

        //public ResultType Type { get { return ResultType.Wait; } }
        public RenderTime Delay { get { return time; } }

        RenderTime time;

        public Wait(RenderTime time)
        {
            this.time = time;
        }


        public void Execute() {

        }

    }
}
