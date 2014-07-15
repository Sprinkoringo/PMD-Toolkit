using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PMDToolkit.Logic.Gameplay;
using PMDToolkit.Logic.Display;
using PMDToolkit.Graphics;

namespace PMDToolkit.Logic.Results {
    public class RemoveItem : IResult {

        //public ResultType Type { get { return ResultType.RemoveNpc; } }
        public RenderTime Delay { get { return RenderTime.Zero; } }

        int itemIndex;

        public RemoveItem(int itemIndex)
        {
            this.itemIndex = itemIndex;
        }

        public void Execute() {
            Screen.Items[itemIndex] = new ItemAnim();
        }

    }
}
