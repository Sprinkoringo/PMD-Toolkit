/*The MIT License (MIT)

Copyright (c) 2014 Sprinkoringo

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE.
*/


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PMDToolkit.Maps {
    public struct TileTexture {

        public Loc2D Texture;
        public int Sheet;

        
        public TileTexture(Loc2D texture, int sheet) {
            Texture = texture;
            Sheet = sheet;
        }

        public static bool operator ==(TileTexture param1, TileTexture param2)
        {
            return (param1.Texture == param2.Texture && param1.Sheet == param2.Sheet);
        }

        public static bool operator !=(TileTexture param1, TileTexture param2)
        {
            return !(param1 == param2);
        }

        public void Draw() {
            Graphics.Texture texture = Graphics.TextureManager.GetTile(Sheet, Texture);
            texture.Render(null);
        }

    }
}
