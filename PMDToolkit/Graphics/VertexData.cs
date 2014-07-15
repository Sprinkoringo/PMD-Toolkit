using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK;

namespace PMDToolkit.Graphics {
    struct VertexData {
        public Vector2 position;
        public Vector2 texCoord;


        public VertexData(float x, float y, float s, float t) {
            position = new Vector2(x, y);
            texCoord = new Vector2(s, t);
        }

        public static int SizeInBytes = Vector2.SizeInBytes * 2;
        public static int PositionOffset = 0;
        public static int TexCoordOffset = Vector2.SizeInBytes;
    }
}
