using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PMDToolkit.Graphics;

namespace PMDToolkit.Logic.Results
{
    public class ResultBranch
    {
        public RenderTime Delay;

        public Queue<IResult> Results { get; set; }

        public ResultBranch()
        {
            Results = new Queue<IResult>();
        }

        public bool IsEmpty()
        {
            return (Results.Count == 0);
        }

        public bool IsReady()
        {
            return (Delay <= RenderTime.Zero);
        }
    }
}
