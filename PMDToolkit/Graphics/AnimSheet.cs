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

namespace PMDToolkit.Graphics {
    public class AnimSheet : TileSheet {

        int totalFrames;
        int dirs;
        int framesPerDir;
        public int TotalFrames { get { return totalFrames; } }
        public int TotalDirs { get { return dirs; } }
        public int FramesPerDir { get { return framesPerDir; } }

        public override void GenerateDataBuffer(int dirs, int framesPerDir) {
            this.dirs = dirs;
            this.framesPerDir = framesPerDir;
            totalFrames = ImageWidth / (ImageHeight / dirs / framesPerDir);
            base.GenerateDataBuffer(ImageHeight / dirs / framesPerDir, ImageHeight / dirs / framesPerDir);
        }

         public void RenderAnim(int frame, int dir, int frameOfDir)
         {
            if (frame >= totalFrames || dir >= TotalDirs || frameOfDir >= FramesPerDir)
            {
                TextureManager.ErrorTexture.RenderAnim(0, 0, 0);
                return;
            }

            RenderTile(frame, dir * framesPerDir + frameOfDir);
         }
    }
}
