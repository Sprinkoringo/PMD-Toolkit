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
using System.Drawing;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using System.IO;
using PMDToolkit.Core;

namespace PMDToolkit.Graphics {
    public class TileMetadata {

        long headerSize;
        long[] tilePositions;

        public int[] TileSizes { get; set; }
        

        Size size;

        public Size Size { get { return size; } }

        public int TotalTiles { get { return tilePositions.Length; } }

        public long GetTilePosition(int index)
        {
            return tilePositions[index] + headerSize;
        }

        public void Load(string filePath) {
            // File format:
            // [tileset-width(4)][tileset-height(4)][tile-count(4)]
            // [tileposition-1(4)][tilesize-1(4)][tileposition-2(4)][tilesize-2(4)][tileposition-n(n*4)][tilesize-n(n*4)]
            // [tile-1(variable)][tile-2(variable)][tile-n(variable)]
            using (FileStream fileStream = File.OpenRead(filePath))
            {
                using (BinaryReader reader = new BinaryReader(fileStream))
                {
                    // Read tileset width
                    this.size.Width = reader.ReadInt32();
                    // Read tileset height
                    this.size.Height = reader.ReadInt32();

                    int tileCount = (size.Width / TextureManager.TILE_SIZE) * (size.Height / TextureManager.TILE_SIZE);

                    // Prepare tile information cache
                    this.tilePositions = new long[tileCount];
                    this.TileSizes = new int[tileCount];

                    // Load tile information
                    for (int i = 0; i < tileCount; i++)
                    {
                        // Read tile position data
                        this.tilePositions[i] = reader.ReadInt64();
                        // Read tile size data
                        this.TileSizes[i] = reader.ReadInt32();
                    }
                    headerSize = fileStream.Position;
                }
            }

        }


    }
}
