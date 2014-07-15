using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PMDToolkit.Maps;
using PMDToolkit.Graphics;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace PMDToolkit.Logic.Display {
    public interface ISprite {

        Loc2D MapLoc { get; }
        int MapHeight { get; }

        bool ActionDone { get; }
        void Begin();
        void Process(RenderTime elapsedTime);
        void Draw();

        Loc2D GetStart();

        Loc2D GetEnd();
    }
}
