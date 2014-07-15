using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using PMDToolkit.Graphics;

namespace PMDToolkit.Maps {
    public class TileAnim {

        public List<TileTexture> Frames;
        public RenderTime FrameLength;

        public TileAnim()
        {
            Frames = new List<TileTexture>();
            FrameLength = RenderTime.FromMillisecs(1);
        }

        public TileAnim(TileAnim oldTileAnim)
        {
            Frames = new List<TileTexture>();
            for (int i = 0; i < oldTileAnim.Frames.Count; i++)
            {
                Frames.Add(oldTileAnim.Frames[i]);
            }
            FrameLength = oldTileAnim.FrameLength;
        }
        
        public TileAnim(Loc2D texture, int sheet) {
            Frames = new List<TileTexture>();
            Frames.Add(new TileTexture(texture, sheet));
            FrameLength = RenderTime.FromMillisecs(1);
        }

        public void Draw(ulong totalTick)
        {
            if (Frames.Count > 0)
            {
                int currentFrame = (int)(totalTick / (ulong)FrameLength.Ticks % (ulong)Frames.Count);
                Frames[currentFrame].Draw();
            }
        }

        public void Save(BinaryWriter writer)
        {
            writer.Write(FrameLength.ToMillisecs());
            writer.Write(Frames.Count);
            for (int j = 0; j < Frames.Count; j++)
            {
                writer.Write(Frames[j].Texture.X);
                writer.Write(Frames[j].Texture.Y);
                writer.Write(Frames[j].Sheet);
            }
        }

        public void Load(BinaryReader reader)
        {
            FrameLength = RenderTime.FromMillisecs(reader.ReadInt32());
            int frameCount = reader.ReadInt32();
            for (int j = 0; j < frameCount; j++)
            {
                TileTexture layer = new TileTexture();

                layer.Texture.X = reader.ReadInt32();
                layer.Texture.Y = reader.ReadInt32();
                layer.Sheet = reader.ReadInt32();
                Frames.Add(layer);
            }
        }



        public static bool operator ==(TileAnim param1, TileAnim param2)
        {
            if (param1.FrameLength != param2.FrameLength)
                return false;
            if (param1.Frames.Count != param2.Frames.Count)
                return false;

            for (int i = 0; i < param1.Frames.Count; i++)
            {
                if (param1.Frames[0] != param2.Frames[0])
                    return false;
            }
            return true;
        }

        public static bool operator !=(TileAnim param1, TileAnim param2)
        {
            return !(param1 == param2);
        }
    }
}
