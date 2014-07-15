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
