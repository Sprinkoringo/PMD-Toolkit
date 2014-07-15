using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace PMDToolkit.Maps
{

    class BranchLine
    {
        public PointF End { get; set; }
        public PointF Start { get; set; }
        public List<BranchLine> Children { get; set; }

        public BranchLine(PointF start, PointF end)
        {
            Start = start;
            End = end;
            Children = new List<BranchLine>();
        }
    }
}
