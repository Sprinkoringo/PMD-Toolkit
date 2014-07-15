using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

using System.IO;
using PMDToolkit.Core;

namespace PMDToolkit.Graphics {

    public enum FrameType
    {
        Idle = 0,
        Walk,
        Attack,
        AttackArm,
        AltAttack,
        SpAttack,
        SpAttackCharge,
        SpAttackShoot,
        Hurt,
        Sleep
    }

    public class FrameData {
        #region Fields

        int frameWidth;
        int frameHeight;

        #endregion Fields

        #region Properties

        public int FrameWidth {
            get { return frameWidth; }
        }

        public int FrameHeight {
            get { return frameHeight; }
        }

        Dictionary<FrameType, Dictionary<PMDToolkit.Maps.Direction8, int>> frameCount;

        #endregion Properties

        public FrameData() {
            frameCount = new Dictionary<FrameType, Dictionary<PMDToolkit.Maps.Direction8, int>>();
        }

        #region Methods

        public void SetFrameSize(int animWidth, int animHeight, int frames) {
            frameWidth = animWidth / frames;

            frameHeight = animHeight;
        }

        public void SetFrameCount(FrameType type, PMDToolkit.Maps.Direction8 dir, int count) {
            if (frameCount.ContainsKey(type) == false) {
                frameCount.Add(type, new Dictionary<PMDToolkit.Maps.Direction8, int>());
            }
            if (frameCount[type].ContainsKey(dir) == false) {
                frameCount[type].Add(dir, count);
            } else {
                frameCount[type][dir] = count;
            }
        }

        public int GetFrameCount(FrameType type, PMDToolkit.Maps.Direction8 dir) {
            Dictionary<PMDToolkit.Maps.Direction8, int> dirs = null;
            if (frameCount.TryGetValue(type, out dirs)) {
                int value = 0;
                if (dirs.TryGetValue(dir, out value)) {
                    return value;
                }
            }

            return 0;
        }

        #endregion Methods
    }

    public class SpriteSheet : IDisposable {
        #region Fields

        FrameData frameData;

        int sizeInBytes;

        #endregion Fields

        #region Constructors

        public SpriteSheet() {

            frameData = new FrameData();
            animations = new Dictionary<FrameType, Dictionary<PMDToolkit.Maps.Direction8, TileSheet>>();
        }

        #endregion Constructors

        #region Properties

        public int BytesUsed {
            get { return sizeInBytes; }
        }

        public FrameData FrameData {
            get { return frameData; }
        }

        Dictionary<FrameType, Dictionary<PMDToolkit.Maps.Direction8, TileSheet>> animations;

        #endregion Properties

        #region Methods


        public Rectangle GetFrameBounds(FrameType frameType, PMDToolkit.Maps.Direction8 direction, int frameNum) {
            Rectangle rec = new Rectangle();
            rec.X = frameNum * frameData.FrameWidth;
            rec.Y = 0;
            rec.Width = frameData.FrameWidth;
            rec.Height = frameData.FrameHeight;

            return rec;
        }

        public void LoadFromData(BinaryReader reader, int totalByteSize)
        {
            foreach (FrameType frameType in Enum.GetValues(typeof(FrameType)))
            {
                if (IsFrameTypeDirectionless(frameType) == false)
                {
                    for (int i = 0; i < 8; i++)
                    {
                        Maps.Direction8 dir = (Maps.Direction8)i;
                        int frameCount = reader.ReadInt32();
                        frameData.SetFrameCount(frameType, dir, frameCount);
                        int size = reader.ReadInt32();
                        if (size > 0)
                        {
                            byte[] imgData = reader.ReadBytes(size);
                            TileSheet sheetSurface = new TileSheet();
                            using (MemoryStream stream = new MemoryStream(imgData))
                            {
                                try
                                {
                                    sheetSurface.LoadPixelsFromBytes(stream);

                                    sheetSurface.LoadTextureFromPixels32();
                                    sheetSurface.GenerateDataBuffer(sheetSurface.ImageWidth / frameCount, sheetSurface.ImageHeight);
                                }
                                catch (Exception ex)
                                {
                                    sheetSurface.Dispose();
                                    throw new Exception("Error reading image data for " + frameType.ToString() + " " + dir.ToString() + "\n", ex);
                                }
                            }
                            AddSheet(frameType, dir, sheetSurface);

                            frameData.SetFrameSize(sheetSurface.ImageWidth, sheetSurface.ImageHeight, frameCount);
                        }
                    }
                }
                else
                {
                    int frameCount = reader.ReadInt32();
                    frameData.SetFrameCount(frameType, PMDToolkit.Maps.Direction8.Down, frameCount);
                    int size = reader.ReadInt32();
                    if (size > 0)
                    {
                        byte[] imgData = reader.ReadBytes(size);
                        TileSheet sheetSurface = new TileSheet();
                        using (MemoryStream stream = new MemoryStream(imgData))
                        {
                            try
                            {
                                sheetSurface.LoadPixelsFromBytes(stream);
                                sheetSurface.LoadTextureFromPixels32();
                            }
                            catch (Exception ex)
                            {
                                sheetSurface.Dispose();
                                throw new Exception("Error reading image data for " + frameType.ToString() + "\n", ex);
                            }
                        }
                        sheetSurface.GenerateDataBuffer(sheetSurface.ImageWidth / frameCount, sheetSurface.ImageHeight);
                        AddSheet(frameType, PMDToolkit.Maps.Direction8.Down, sheetSurface);

                        frameData.SetFrameSize(sheetSurface.ImageWidth, sheetSurface.ImageHeight, frameCount);
                    }
                }
            }
            

            this.sizeInBytes = totalByteSize;

        }


        public TileSheet GetSheet(FrameType type, PMDToolkit.Maps.Direction8 dir) {
            if (IsFrameTypeDirectionless(type)) {
                dir = PMDToolkit.Maps.Direction8.Down;
            }
            if (animations.ContainsKey(type))
            {
                if (animations[type].ContainsKey(dir))
                {
                    return animations[type][dir];
                }
            }

            return TextureManager.ErrorTexture;
        }

        public void AddSheet(FrameType type, PMDToolkit.Maps.Direction8 dir, TileSheet surface) {
            if (!animations.ContainsKey(type)) {
                animations.Add(type, new Dictionary<PMDToolkit.Maps.Direction8, TileSheet>());
            }
            if (animations[type].ContainsKey(dir) == false) {
                animations[type].Add(dir, surface);
            } else {
                animations[type][dir] = surface;
            }
        }

        public static bool IsFrameTypeDirectionless(FrameType frameType) {
            switch (frameType) {
                case FrameType.Sleep:
                    return true;
            }
            return false;
        }

        public void Dispose()
        {
            foreach(FrameType frameType in animations.Keys)
            {
                foreach(Maps.Direction8 dir in animations[frameType].Keys)
                {
                    if (animations[frameType][dir] != null)
                    {
                        animations[frameType][dir].Dispose();
                    }
                }
            }

        }

        #endregion Methods
    }
}
