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
    public class NpcSprite : CharSprite {

        public NpcSprite()
        {

        }

        public NpcSprite(Loc2D charLoc, Direction8 charDir, Gameplay.FormData data)
            : base(charLoc, charDir)
        {
            CharData = data;
        }

    }
}
