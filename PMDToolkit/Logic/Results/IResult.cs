using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PMDToolkit.Graphics;

namespace PMDToolkit.Logic.Results {


    public interface IResult {

        //ResultType Type { get; }
        RenderTime Delay { get; }

        void Execute();
    }
}
