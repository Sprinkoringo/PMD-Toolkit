using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using PMDToolkit.Core;
using PMDToolkit.Data;

namespace PMDToolkit.Graphics {
    public class Conversion {
        #region Sprites


        public static bool CompileAllSprites(string spriteRootDirectory, string spriteCacheDirectory)
        {
            if (!Directory.Exists(spriteCacheDirectory))
                Directory.CreateDirectory(spriteCacheDirectory);

            foreach(int index in GetAllNumDirs(spriteRootDirectory, "Sprite"))
            {
                Game.UpdateLoadMsg("Converting Sprite #" + index);
                CompileSprite(spriteRootDirectory + "Sprite" + index + "\\", spriteCacheDirectory + "\\Sprite"+index, index);
            }
            return true;
        }

        public static void CompileSprite(string spriteDir, string cacheDir, int index)
        {
            try
            {
                if (Directory.Exists(spriteDir) && NeedsUpdate(spriteDir, true, cacheDir + ".sprite"))
                {
                    //check main folder
                    Dictionary<string, byte[]> spriteData = new Dictionary<string, byte[]>();

                    CompileSpriteToFile(spriteDir, spriteData, index, -1, -1, -1);

                    //check all subfolders
                    foreach (int formDirs in GetAllNumDirs(spriteDir, "form"))
                    {
                        //get subframes (discount if negative)
                        CompileSpriteToFile(spriteDir + "form" + formDirs + "\\", spriteData, index, formDirs, -1, -1);

                        foreach (int shinyDirs in GetAllNumDirs(spriteDir + "form" + formDirs + "\\", "shiny"))
                        {
                            //get subframes
                            CompileSpriteToFile(spriteDir + "form" + formDirs + "\\shiny" + shinyDirs + "\\",
                                spriteData, index, formDirs, shinyDirs, -1);

                            foreach (int genderDirs in GetAllNumDirs(spriteDir + "form" + formDirs + "\\shiny" + shinyDirs + "\\", "gender"))
                            {
                                //get subframes
                                CompileSpriteToFile(spriteDir + "form" + formDirs + "\\shiny" + shinyDirs + "\\gender" + genderDirs + "\\",
                                    spriteData, index, formDirs, shinyDirs, genderDirs);
                            }
                        }
                    }

                    WriteSpriteFile(cacheDir + ".sprite", spriteData);
                }
            }
            catch (Exception ex)
            {
                Logs.Logger.LogError(new Exception("Error converting sprite #" + index + "\n", ex));
            }
        }

        static void CompileSpriteToFile(string spriteDir, Dictionary<string, byte[]> spriteData, int num, int form, int shiny, int gender) {
            //check to see if files exist
            string[] pngs = Directory.GetFiles(spriteDir, "*.png", SearchOption.TopDirectoryOnly);
            if (pngs.Length < 1) return;

            String outForm = "r";
            if (form >= 0) {
                outForm += "-" + form;
                if (shiny >= 0) {
                    outForm += "-" + shiny;
                    if (gender >= 0) {
                        outForm += "-" + gender;
                    }
                }
            }

            try
            {
                SpriteSheet sprite = CompileSpriteInternal(spriteDir);

                using (MemoryStream spriteStream = new MemoryStream())
                {
                    using (BinaryWriter writer = new BinaryWriter(spriteStream))
                    {
                        //Go through each frame type, get metadata, sprite data
                        foreach (FrameType frameType in Enum.GetValues(typeof(FrameType)))
                        {
                            if (!SpriteSheet.IsFrameTypeDirectionless(frameType))
                            {
                                for (int j = 0; j < 8; j++)
                                {
                                    Maps.Direction8 dir = (Maps.Direction8)j;
                                    int frameCountSize = sprite.FrameData.GetFrameCount(frameType, dir);

                                    TileSheet anim = sprite.GetSheet(frameType, dir);

                                    // Add frame count size
                                    writer.Write(frameCountSize);

                                    int sheetSize = 0;
                                    byte[] memStreamArray = new byte[0];
                                    if (anim != null)
                                    {
                                        using (MemoryStream memoryStream = new MemoryStream())
                                        {
                                            anim.Save(memoryStream, System.Drawing.Imaging.ImageFormat.Png);
                                            memStreamArray = memoryStream.ToArray();
                                            sheetSize = memStreamArray.Length;
                                        }
                                        anim.Dispose();
                                    }
                                    // Add the animation size
                                    writer.Write(sheetSize);
                                    //add the animation itself
                                    if (sheetSize > 0)
                                    {
                                        spriteStream.Write(memStreamArray, 0, memStreamArray.Length);
                                    }
                                }
                            }
                            else
                            {
                                int frameCountSize = sprite.FrameData.GetFrameCount(frameType, Maps.Direction8.Down);

                                TileSheet anim = sprite.GetSheet(frameType, Maps.Direction8.Down);

                                // Add frame count size
                                writer.Write(frameCountSize);

                                int sheetSize = 0;
                                byte[] memStreamArray = new byte[0];
                                if (anim != null)
                                {
                                    using (MemoryStream memoryStream = new MemoryStream())
                                    {
                                        anim.Save(memoryStream, System.Drawing.Imaging.ImageFormat.Png);
                                        memStreamArray = memoryStream.ToArray();
                                        sheetSize = memStreamArray.Length;
                                    }
                                    anim.Dispose();
                                }
                                // Add the animation size
                                writer.Write(sheetSize);
                                //add the animation itself
                                if (sheetSize > 0)
                                {
                                    spriteStream.Write(memStreamArray, 0, memStreamArray.Length);
                                }
                            }
                        }
                    }

                    byte[] writingBytes = spriteStream.ToArray();
                    spriteData.Add(outForm, writingBytes);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error compiling sprite" + spriteDir + "\n", ex);
            }

        }

        private static void WriteSpriteFile(string destinationPath, Dictionary<string, byte[]> spriteData) {
            // File format:
            // [form-count(4)]
            // [form-name-size(n*4)][form-name(n*variable)][form-position(n*4)][form-size(n*4)]
            // [form-1(variable)][form-2(variable)][form-n(variable)]
            using (FileStream stream = new FileStream(destinationPath, System.IO.FileMode.Create, System.IO.FileAccess.Write)) {
                using (BinaryWriter writer = new BinaryWriter(stream))
                {
                    // Write form count
                    writer.Write(spriteData.Count);

                    // Write header information about each form, starting at the correct place
                    int formDataPosition = 0;
                    for (int i = 0; i < spriteData.Count; i++)
                    {
                        // Write the form name
                        writer.Write(spriteData.Keys.ElementAt(i));
                        // Write the form position
                        writer.Write(formDataPosition);
                        // Write the form size
                        writer.Write(spriteData.Values.ElementAt(i).Length);

                        // Add the form size to the position
                        formDataPosition += spriteData.Values.ElementAt(i).Length;
                    }

                    // Now that the header is written, write form data
                    foreach (byte[] formData in spriteData.Values)
                    {
                        stream.Write(formData, 0, formData.Length);
                    }
                }
            }
        }

        private static SpriteSheet CompileSpriteInternal(string spriteRootDirectory) {
            Dictionary<FrameType, Dictionary<Maps.Direction8, List<Bitmap>>> frameCollection = new Dictionary<FrameType, Dictionary<Maps.Direction8, List<Bitmap>>>();
            Dictionary<FrameType, List<Bitmap>> miscFrameCollection = new Dictionary<FrameType, List<Bitmap>>();
            List<string> usedFrames = new List<string>();
            //get all frames
            foreach (FrameType frameType in Enum.GetValues(typeof(FrameType))) {

                if (!SpriteSheet.IsFrameTypeDirectionless(frameType)) {
                    Dictionary<Maps.Direction8, List<Bitmap>> frameTypeDirectionCollection = new Dictionary<Maps.Direction8, List<Bitmap>>();
                    for (int j = 0; j < 8; j++) {
                        Maps.Direction8 direction = (Maps.Direction8)j;

                        List<Bitmap> frameTypeCollection = new List<Bitmap>();
                        frameTypeDirectionCollection.Add(direction, frameTypeCollection);

                        foreach (Tuple<string, Bitmap> frameSurface in GetAllFrames(spriteRootDirectory, frameType + "-" + direction + "-")) {
                            frameTypeCollection.Add(frameSurface.Item2);
                            usedFrames.Add(frameSurface.Item1);
                        }
                        //conversionNotes.Text += "Found " + frameTypeCollection.Count + " frames for " + frameType.ToString() + " (" + direction.ToString() + ") animation.\r\n";
                    }

                    frameCollection.Add(frameType, frameTypeDirectionCollection);


                } else {

                    List<Bitmap> frameTypeCollection = new List<Bitmap>();
                    miscFrameCollection.Add(frameType, frameTypeCollection);

                    foreach (Tuple<string, Bitmap> frameSurface in GetAllFrames(spriteRootDirectory, frameType.ToString() + "-"))
                    {
                        frameTypeCollection.Add(frameSurface.Item2);
                        usedFrames.Add(frameSurface.Item1);
                    }
                    //conversionNotes.Text += "Found " + frameTypeCollection.Count + " frames for " + frameType.ToString() + " (Directionless) animation.\r\n";
                }
            }

            if (usedFrames.Count < 1)
                throw new Exception("No frames");

            List<string> pngs = new List<string>(Directory.GetFiles(spriteRootDirectory, "*.png", SearchOption.TopDirectoryOnly));
            if (pngs.Count > usedFrames.Count)
            {
                foreach (Dictionary<Maps.Direction8, List<Bitmap>> dict in frameCollection.Values)
                    foreach (List<Bitmap> list in dict.Values)
                        foreach (Bitmap bitmap in list)
                            bitmap.Dispose();

                foreach (List<Bitmap> list in miscFrameCollection.Values)
                    foreach (Bitmap bitmap in list)
                        bitmap.Dispose();

                foreach (string path in usedFrames)
                    for (int i = pngs.Count - 1; i >= 0; i--)
                        if (pngs[i] == path)
                            pngs.RemoveAt(i);

                string errorMsg = spriteRootDirectory + ": The following files do not match the naming pattern, or a preceding file is missing:";
                for (int i = 0; i < pngs.Count; i++)
                    errorMsg += "\n"+pngs[i].Substring(pngs[i].LastIndexOf('\\'));

                throw new Exception(errorMsg);
            }

            SpriteSheet sprite = BuildSpriteSurface(frameCollection, miscFrameCollection, spriteRootDirectory);

            return sprite;
        }

        private static SpriteSheet BuildSpriteSurface(Dictionary<FrameType, Dictionary<Maps.Direction8, List<Bitmap>>> frameCollection, Dictionary<FrameType, List<Bitmap>> miscFrameCollection, string spriteRootDirectory) {

            int frameHeight = 0;
            int frameWidth = 0;

            SpriteSheet sprite = new SpriteSheet();

            //set frame count and frame size
            for (int i = 0; i < frameCollection.Keys.Count; i++) {
                Dictionary<Maps.Direction8, List<Bitmap>> frameDirectionCollection = frameCollection.Values.ElementAt(i);
                for (int j = 0; j < 8; j++) {
                    Maps.Direction8 direction = (Maps.Direction8)j;
                    int totalSurfaceWidth = 0;
                    int lastX = 0;

                    List<Bitmap> frameList = frameDirectionCollection[direction];
                    sprite.FrameData.SetFrameCount(frameCollection.Keys.ElementAt(i), direction, frameList.Count);

                    foreach (Bitmap frame in frameList) {
                        if (frameHeight == 0) {
                            frameHeight = frame.Height;
                        } else if (frameHeight != frame.Height) {
                            throw new Exception("Frameheight mismatch: " + frameCollection.Keys.ElementAt(i).ToString() + " " + direction.ToString() + " (" + frameHeight + " vs. " + frame.Height + ")" );
                        }
                        if (frameWidth == 0) {
                            frameWidth = frame.Width;
                        } else if (frameWidth != frame.Width) {
                            throw new Exception("Framewidth mismatch: " + frameCollection.Keys.ElementAt(i).ToString() + " " + direction.ToString() + " (" + frameWidth + " vs. " + frame.Width + ")");
                        }
                        totalSurfaceWidth += frame.Width;
                    }

                    TileSheet animSheet = null;
                    if (totalSurfaceWidth > 0) {
                        animSheet = new TileSheet();
                        animSheet.CreatePixels32(totalSurfaceWidth, frameHeight);
                    }
                    foreach (Bitmap frame in frameList) {
                        BitmapData frameData = frame.LockBits(new Rectangle(0, 0, frame.Width, frame.Height), ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
                        animSheet.Blit(frameData, 0, 0, frame.Width, frame.Height, lastX, 0);
                        frame.UnlockBits(frameData);
                        lastX += frame.Width;
                        frame.Dispose();
                    }

                    sprite.AddSheet(frameCollection.Keys.ElementAt(i), direction, animSheet);
                }
            }

            //set frame count and frame size
            for (int i = 0; i < miscFrameCollection.Count; i++) {
                int totalSurfaceWidth = 0;
                int lastX = 0;

                List<Bitmap> frameList = miscFrameCollection.Values.ElementAt(i);
                sprite.FrameData.SetFrameCount(miscFrameCollection.Keys.ElementAt(i), Maps.Direction8.Down, frameList.Count);

                foreach (Bitmap frame in frameList) {
                    if (frameHeight == 0) {
                        frameHeight = frame.Height;
                    } else if (frameHeight != frame.Height) {
                        throw new Exception("Frameheight mismatch: " + frameCollection.Keys.ElementAt(i).ToString() + " (" + frameHeight + " vs. " + frame.Height + ")");
                    }
                    if (frameWidth == 0) {
                        frameWidth = frame.Width;
                    } else if (frameWidth != frame.Width) {
                        throw new Exception("Framewidth mismatch: " + frameCollection.Keys.ElementAt(i).ToString() + " (" + frameWidth + " vs. " + frame.Width + ")");
                    }
                    totalSurfaceWidth += frame.Width;
                }

                TileSheet animSheet = null;
                if (totalSurfaceWidth > 0) {
                    animSheet = new TileSheet();
                    animSheet.CreatePixels32(totalSurfaceWidth, frameHeight);
                }
                foreach (Bitmap frame in frameList) {
                    BitmapData frameData = frame.LockBits(new Rectangle(0, 0, frame.Width, frame.Height), ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
                    animSheet.Blit(frameData, 0, 0, frame.Width, frame.Height, lastX, 0);
                    frame.UnlockBits(frameData);
                    lastX += frame.Width;
                    frame.Dispose();
                }

                sprite.AddSheet(miscFrameCollection.Keys.ElementAt(i), Maps.Direction8.Down, animSheet);
            }

            return sprite;
        }

        private static IEnumerable<Tuple<string, Bitmap>> GetAllFrames(string spriteRootDirectory, string frameBaseFileName) {
            int frameCount = 1;
            bool allFramesAdded = false;

            while (allFramesAdded == false) {

                string frameFileName = frameBaseFileName + frameCount + ".png";
                string frameFullPath = GetFrameFilePath(spriteRootDirectory, frameFileName);

                if (File.Exists(frameFullPath)) {
                    Bitmap frameSurface = new Bitmap(frameFullPath);

                    yield return new Tuple<string, Bitmap>(frameFullPath, frameSurface);

                    frameCount++;
                } else {
                    allFramesAdded = true;
                }

            }
        }

        private static string GetFrameFilePath(string spriteRootDirectory, string frameFileName) {
            // If there is no form specified, or there is no frame for the specified form, try to load from the global directory
            if (File.Exists(spriteRootDirectory + "\\" + frameFileName)) {
                return spriteRootDirectory + "\\" + frameFileName;
            }
            return null;
        }

        #endregion

        #region Portraits


        public static bool CompileAllPortraits(string spriteRootDirectory, string spriteCacheDirectory)
        {
            if (!Directory.Exists(spriteCacheDirectory))
                Directory.CreateDirectory(spriteCacheDirectory);

            foreach (int index in GetAllNumDirs(spriteRootDirectory, "Sprite"))
            {
                Game.UpdateLoadMsg("Converting Portrait #" + index);
                CompilePortrait(spriteRootDirectory + "\\Sprite" + index + "\\", spriteCacheDirectory + "\\Portrait" + index, index);
            }
            return true;
        }

        public static void CompilePortrait(string spriteDir, string cacheDir, int index)
        {
            try
            {
                if (Directory.Exists(spriteDir) && NeedsUpdate(spriteDir, true, cacheDir + ".portrait"))
                {
                    //check main folder
                    Dictionary<string, byte[]> spriteData = new Dictionary<string, byte[]>();

                    CompilePortraitToFile(spriteDir, spriteData, index, -1, -1, -1);

                    //check all subfolders
                    foreach (int formDirs in GetAllNumDirs(spriteDir, "form"))
                    {
                        //get subframes (discount if negative)
                        CompilePortraitToFile(spriteDir + "form" + formDirs + "\\", spriteData, index, formDirs, -1, -1);

                        foreach (int shinyDirs in GetAllNumDirs(spriteDir + "form" + formDirs + "\\", "shiny"))
                        {
                            //get subframes
                            CompilePortraitToFile(spriteDir + "form" + formDirs + "\\shiny" + shinyDirs + "\\",
                                spriteData, index, formDirs, shinyDirs, -1);

                            foreach (int genderDirs in GetAllNumDirs(spriteDir + "form" + formDirs + "\\shiny" + shinyDirs + "\\", "gender"))
                            {
                                //get subframes
                                CompilePortraitToFile(spriteDir + "form" + formDirs + "\\shiny" + shinyDirs + "\\gender" + genderDirs + "\\",
                                    spriteData, index, formDirs, shinyDirs, genderDirs);
                            }
                        }
                    }

                    WriteSpriteFile(cacheDir + ".portrait", spriteData);
                }
            }
            catch (Exception ex)
            {
                Logs.Logger.LogError(new Exception("Error converting portrait #" + index+"\n", ex));
            }
        }

        static void CompilePortraitToFile(string spriteDir, Dictionary<string, byte[]> spriteData, int num, int form, int shiny, int gender)
        {
            //check to see if files exist
            string[] pngs = Directory.GetFiles(spriteDir, "*.png", SearchOption.TopDirectoryOnly);
            if (pngs.Length < 1) return;

            String outForm = "r";
            if (form >= 0)
            {
                outForm += "-" + form;
                if (shiny >= 0)
                {
                    outForm += "-" + shiny;
                    if (gender >= 0)
                    {
                        outForm += "-" + gender;
                    }
                }
            }

            try
            {
                int totalFrames;
                TileSheet sprite = CompilePortraitInternal(spriteDir, out totalFrames);

                using (MemoryStream spriteStream = new MemoryStream())
                {
                    using (BinaryWriter writer = new BinaryWriter(spriteStream))
                    {
                        writer.Write(totalFrames);

                        int sheetSize = 0;
                        byte[] memStreamArray = new byte[0];
                        if (sprite != null)
                        {
                            using (MemoryStream memoryStream = new MemoryStream())
                            {
                                sprite.Save(memoryStream, System.Drawing.Imaging.ImageFormat.Png);
                                memStreamArray = memoryStream.ToArray();
                                sheetSize = memStreamArray.Length;
                            }
                            sprite.Dispose();
                        }

                        // Add the animation size
                        writer.Write(sheetSize);
                        //add the animation itself
                        if (sheetSize > 0)
                            spriteStream.Write(memStreamArray, 0, memStreamArray.Length);

                    }

                    byte[] writingBytes = spriteStream.ToArray();
                    spriteData.Add(outForm, writingBytes);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error compiling portait" + spriteDir+"\n", ex);
            }

        }

        static TileSheet CompilePortraitInternal(string spriteRootDirectory, out int totalFrames) {
            List<Bitmap> portraitCollection = new List<Bitmap>();
            List<string> usedFrames = new List<string>();
            
            foreach (Tuple<string, Bitmap> frameSurface in GetAllFrames(spriteRootDirectory, ""))
            {
                portraitCollection.Add(frameSurface.Item2);
                usedFrames.Add(frameSurface.Item1);
            }

            if (usedFrames.Count < 1)
                throw new Exception("No frames");

            List<string> pngs = new List<string>(Directory.GetFiles(spriteRootDirectory, "*.png", SearchOption.TopDirectoryOnly));
            if (pngs.Count > usedFrames.Count)
            {
                foreach (Bitmap bitmap in portraitCollection)
                    bitmap.Dispose();

                foreach (string path in usedFrames)
                    for (int i = pngs.Count - 1; i >= 0; i--)
                        if (pngs[i] == path)
                            pngs.RemoveAt(i);

                string errorMsg = spriteRootDirectory + ": The following files do not match the naming pattern, or a preceding file is missing:";
                for (int i = 0; i < pngs.Count; i++)
                    errorMsg += "\n" + pngs[i].Substring(pngs[i].LastIndexOf('\\'));

                throw new Exception(errorMsg);
            }

            totalFrames = usedFrames.Count;
            return BuildPortraitSurface(portraitCollection, spriteRootDirectory);

        }

        private static TileSheet BuildPortraitSurface(List<Bitmap> frameCollection, string spriteRootDirectory)
        {

            int frameHeight = 0;
            int frameWidth = 0;

            TileSheet animSheet = null;

            int totalSurfaceWidth = 0;
            int lastX = 0;
            int frameNum = 0;

            foreach (Bitmap frame in frameCollection)
            {
                if (frameHeight == 0)
                    frameHeight = frame.Height;
                else if (frameHeight != frame.Height)
                {
                    throw new Exception("Frameheight mismatch: " + frameNum + " (" + frameHeight + " vs. " + frame.Height + ")");
                }
                if (frameWidth == 0)
                {
                    frameWidth = frame.Width;
                }
                else if (frameWidth != frame.Width)
                {
                    throw new Exception("Framewidth mismatch: " + frameNum + " (" + frameWidth + " vs. " + frame.Width + ")");
                }
                totalSurfaceWidth += frame.Width;
                frameNum++;
            }

            if (totalSurfaceWidth > 0)
            {
                animSheet = new TileSheet();
                animSheet.CreatePixels32(totalSurfaceWidth, frameHeight);
            }
            foreach (Bitmap frame in frameCollection)
            {
                BitmapData frameData = frame.LockBits(new Rectangle(0, 0, frame.Width, frame.Height), ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
                animSheet.Blit(frameData, 0, 0, frame.Width, frame.Height, lastX, 0);
                frame.UnlockBits(frameData);
                lastX += frame.Width;
                frame.Dispose();
            }

            return animSheet;
        }

        #endregion

        public static bool NeedsUpdate(string inFile, bool inDir, string outFile)
        {
            if (inDir)
            {
                if (!Directory.Exists(inFile))
                    return false;
            }
            else
            {
                if (!File.Exists(inFile))
                    return false;
            }

            if (!File.Exists(outFile))
                return true;

            if (File.GetLastWriteTimeUtc(inFile) > File.GetLastWriteTimeUtc(outFile))
                return true;

            return false;
        }

        public static IEnumerable<int> GetAllNumDirs(string spriteRootDirectory, string dirName)
        {
            string[] dirs = Directory.GetDirectories(spriteRootDirectory, dirName + "*", SearchOption.TopDirectoryOnly);

            for (int i = 0; i < dirs.Length; i++)
            {
                string num = dirs[i].Substring((spriteRootDirectory + dirName).Length);
                if (num.IsNumeric())
                    yield return num.ToInt();
            }
        }

        #region Tiles
        public static void CompileAllTiles(string sourceDir, string cacheDir)
        {
            if (!Directory.Exists(cacheDir))
                Directory.CreateDirectory(cacheDir);

            string[] dirs = Directory.GetFiles(sourceDir, "Tiles*.png");
            //go through each sprite folder, and each form folder
            for (int i = 0; i < dirs.Length; i++) {
                if (dirs[i].EndsWith(".png")) {
                    int lastIndex = dirs[i].LastIndexOf('\\');
                    string outputFile = (cacheDir + "\\" + dirs[i].Substring(lastIndex + 1, dirs[i].LastIndexOf('.') - lastIndex - 1) + ".tile");
                    if (NeedsUpdate(dirs[i], false, outputFile))
                    {
                        try
                        {
                            Game.UpdateLoadMsg("Converting Tile #" + i);
                            Bitmap tileset = new Bitmap(dirs[i]);
                            SaveTileMap(tileset, outputFile);
                            tileset.Dispose();
                        } catch (Exception ex) {
                            Logs.Logger.LogError(new Exception("Error converting Tiles #" + i+"\n", ex));
                        }
                    }
                }
            }
        }

        static void SaveTileMap(Bitmap bitmap, string destinationPath) {
            BitmapData data = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            // File format:
            // [tileset-width(4)][tileset-height(4)][tile-count(4)]
            // [tileposition-1(4)][tilesize-1(4)][tileposition-2(4)][tilesize-2(4)][tileposition-n(n*4)][tilesize-n(n*4)]
            // [tile-1(variable)][tile-2(variable)][tile-n(variable)]
            using (System.IO.FileStream spriteStream = new System.IO.FileStream(destinationPath, System.IO.FileMode.Create, System.IO.FileAccess.Write))
            {
                using (BinaryWriter writer = new BinaryWriter(spriteStream))
                {
                    // Write tileset width
                    writer.Write(bitmap.Width);
                    // Write tileset height
                    writer.Write(bitmap.Height);

                    int count = (bitmap.Width / TextureManager.TILE_SIZE) * (bitmap.Height / TextureManager.TILE_SIZE);

                    long tileDataPosition = 0;
                    // Write header information about each tile
                    List<byte[]> tiles = new List<byte[]>();
                    for (int i = 0; i < count; i++)
                    {
                        //cut off the corresponding piece
                        using (Texture tileTex = new Texture())
                        {

                            tileTex.CreatePixels32(TextureManager.TILE_SIZE, TextureManager.TILE_SIZE);

                            int x = i % (bitmap.Width / TextureManager.TILE_SIZE);
                            int y = i / (bitmap.Width / TextureManager.TILE_SIZE);
                            tileTex.Blit(data, x * TextureManager.TILE_SIZE, y * TextureManager.TILE_SIZE, TextureManager.TILE_SIZE, TextureManager.TILE_SIZE, 0, 0);

                            //save as a PNG to a stream
                            using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
                            {
                                tileTex.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                                byte[] bytes = ms.ToArray();

                                tiles.Add(bytes);

                                // Write the position of the tile
                                writer.Write(tileDataPosition);
                                // Write the size of the tile
                                writer.Write(bytes.Length);
                                // Add the tile size to the position
                                tileDataPosition += bytes.Length;
                            }
                        }
                    }

                    // Now that all header is written, write tile data
                    for (int i = 0; i < count; i++)
                    {
                        writer.Write(tiles[i], 0, tiles[i].Length);
                    }
                }
            }
        }
        #endregion
    }
}
