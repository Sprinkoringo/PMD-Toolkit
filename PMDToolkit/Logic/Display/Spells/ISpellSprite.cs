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

    public enum MoveAnimationType {
        Normal,
        Arrow,
        Throw,
        Beam,
        Overlay,
        Tile,
        ItemArrow,
        ItemThrow,
        Spray,//TODO
        Directional,//TODO
        Particle
    }

    public interface ISpellSprite : ISprite {
        
        MoveAnimationType AnimType {
            get;
        }

        int AnimationIndex {
            get;
        }

        Loc2D StartLoc { get; }

        Direction8 Direction { get; }

        RenderTime FrameLength { get; }

    }
}
