using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PMDToolkit.Maps;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using PMDToolkit.Graphics;

namespace PMDToolkit.Logic.Gameplay {
    public class MenuText {

        public string Text { get; set; }
        public Color4 Color { get; set; }
        public Loc2D Loc { get; set; }

        public MenuText(string text, Loc2D loc) {
            Text = text;
            Loc = loc;
            Color = Color4.White;
        }

        public MenuText(string text, Loc2D loc, Color4 color) {
            Text = text;
            Loc = loc;
            Color = color;
        }

    }
}
