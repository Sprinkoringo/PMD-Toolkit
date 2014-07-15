using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PMDToolkit.Graphics;

namespace PMDToolkit.Logic.Display {
    public interface IEmitter {

        bool ActionDone { get; }

        void Process(RenderTime elapsedTime);
    }
}
