using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using System.IO;
using PMDToolkit.Core;
using PMDToolkit.Data;

namespace PMDToolkit.Graphics {
    public static class TextureManager {
        public enum SpellAnimType {
            Spell,
            Arrow,
            Beam
        }

        #region Fields

        public const int TILE_SIZE = 32;
        public const int SCREEN_WIDTH = 640;
        public const int SCREEN_HEIGHT = 480;
        public const int BITS_PER_PIX = 32;
        public const int FPS_CAP = 60;

        const int SPRITE_CACHE_SIZE = 200;
        const int MUGSHOT_CACHE_SIZE = 100;
        const int SPELL_CACHE_SIZE = 100;
        const int STATUS_CACHE_SIZE = 100;
        const int ITEM_CACHE_SIZE = 100;
        const int OBJECT_CACHE_SIZE = 100;
        const int TILE_CACHE_SIZE = 1000;
        const int TOTAL_TILE_SHEETS = 11;

        static MultiNameLRUCache<string, SpriteSheet> spriteCache;
        static MultiNameLRUCache<string, TileSheet> mugshotCache;
        static LRUCache<string, AnimSheet> spellCache;
        static LRUCache<int, AnimSheet> statusCache;
        static LRUCache<int, AnimSheet> itemCache;
        static LRUCache<int, AnimSheet> objectCache;
        static LRUCache<string, Texture> tileCache;

        #endregion Fields

        #region Properties

        public static Graphics.TextureProgram TextureProgram { get; set; }


        public static AnimSheet ErrorTexture { get; set; }
        public static Texture BlankTexture { get; set; }

        public static Font SingleFont { get; set; }

        static TileMetadata[] tileData;

        public static TileSheet MenuBack { get; set; }

        public static Texture Picker { get; set; }

        public static bool NeedSpriteReload { get; set; }
        public static bool NeedTileReload { get; set; }
        public static bool NeedItemReload { get; set; }
        public static bool NeedPortraitReload { get; set; }
        public static bool NeedEffectReload { get; set; }

        #endregion Properties

        #region Methods

        public static void InitBase()
        {

            //initialize clear color
            GL.ClearColor(Color4.Black);

            GL.Enable(EnableCap.Texture2D);

            //set blending
            GL.Enable(EnableCap.Blend);
            GL.Disable(EnableCap.DepthTest);
            GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);

            //Initialize stencil clear value
            GL.ClearStencil(0);

            TextureProgram = new Graphics.TextureProgram();
            //Load basic shader program
            TextureProgram.LoadProgram();

            //Bind basic shader program
            TextureProgram.Bind();

            //Set texture unit
            TextureProgram.SetTextureUnit(0);
            //Set program for texture
            Graphics.Texture.SetTextureProgram(TextureProgram);

            //load font
            SingleFont = new Font();
            string[] dirs = Directory.GetFiles(Paths.BaseGFXPath+"Font", "pmd-*.png", SearchOption.TopDirectoryOnly);
            for (int i = 0; i < dirs.Length; i++)
            {
                int startLength = (Paths.BaseGFXPath + "Font\\" + "pmd-").Length;
                string num = dirs[i].Substring(startLength, dirs[i].Length - startLength - ".png".Length);
                if (num.IsNumeric())
                    SingleFont.LoadFont(dirs[i], num.ToInt());
            }
        }

        public static void Init()
        {
            //run conversions
            if (!Directory.Exists(Paths.CachedGFXPath))
                Directory.CreateDirectory(Paths.CachedGFXPath);

            Conversion.CompileAllSprites(Paths.SpritesPath, Paths.CachedGFXPath + "Sprite");
            Conversion.CompileAllTiles(Paths.TilesPath, Paths.CachedGFXPath + "Tile");
            Conversion.CompileAllPortraits(Paths.PortraitsPath, Paths.CachedGFXPath + "Portrait");

            Game.UpdateLoadMsg("Loading Base Textures");
            //load error texture
            ErrorTexture = new AnimSheet();
            ErrorTexture.LoadPixelsFromFile32(Paths.BaseGFXPath+"Error.png");

            //load blank texture
            BlankTexture = new Texture();
            BlankTexture.LoadPixelsFromFile32(Paths.BaseGFXPath + "UI\\Blank.png");

            //load menu data
            MenuBack = new TileSheet();
            MenuBack.LoadPixelsFromFile32(Paths.BaseGFXPath + "UI\\Back.png");

            Picker = new TileSheet();
            Picker.LoadPixelsFromFile32(Paths.BaseGFXPath + "UI\\Picker.png");


            Game.UpdateLoadMsg("Loading Caches");
            //initialize caches
            spriteCache = new MultiNameLRUCache<string, SpriteSheet>(SPRITE_CACHE_SIZE);
            spriteCache.OnItemRemoved = DisposeCachedObject;
            mugshotCache = new MultiNameLRUCache<string, TileSheet>(MUGSHOT_CACHE_SIZE);
            mugshotCache.OnItemRemoved = DisposeCachedObject;
            spellCache = new LRUCache<string, AnimSheet>(SPELL_CACHE_SIZE);
            spellCache.OnItemRemoved = DisposeCachedObject;
            statusCache = new LRUCache<int, AnimSheet>(STATUS_CACHE_SIZE);
            statusCache.OnItemRemoved = DisposeCachedObject;
            itemCache = new LRUCache<int, AnimSheet>(ITEM_CACHE_SIZE);
            itemCache.OnItemRemoved = DisposeCachedObject;
            objectCache = new LRUCache<int, AnimSheet>(OBJECT_CACHE_SIZE);
            objectCache.OnItemRemoved = DisposeCachedObject;
            tileCache = new LRUCache<string, Texture>(TILE_CACHE_SIZE);
            tileCache.OnItemRemoved = DisposeCachedObject;

            //load metadata
            tileData = new TileMetadata[TOTAL_TILE_SHEETS];
        }

        public static void PostInit()
        {
            //load error texture
            ErrorTexture.LoadTextureFromPixels32();
            ErrorTexture.GenerateDataBuffer(1, 1);

            //load blank texture
            BlankTexture.LoadTextureFromPixels32();

            //load menu data
            MenuBack.LoadTextureFromPixels32();
            MenuBack.GenerateDataBuffer(MenuBack.ImageWidth / 3, MenuBack.ImageHeight / 3);

            Picker.LoadTextureFromPixels32();
        }

        public static void Exit()
        {
            Picker.Dispose();
            MenuBack.Dispose();
            
            tileCache.Clear();
            objectCache.Clear();
            itemCache.Clear();
            statusCache.Clear();
            spellCache.Clear();
            mugshotCache.Clear();
            spriteCache.Clear();

            BlankTexture.Dispose();
            ErrorTexture.Dispose();

            SingleFont.Dispose();

            TextureProgram.Dispose();
        }

        public static void Update()
        {
            try
            {
                if (NeedTileReload)
                {
                    tileData = new TileMetadata[TOTAL_TILE_SHEETS];
                    tileCache.Clear();
                    Logs.Logger.LogDebug("Tilesets Reloaded.");
                }

                if (NeedSpriteReload)
                {
                    spriteCache.Clear();
                    Logs.Logger.LogDebug("Sprites Reloaded.");
                }

                if (NeedPortraitReload)
                {
                    mugshotCache.Clear();
                    Logs.Logger.LogDebug("Portraits Reloaded.");
                }

                if (NeedEffectReload)
                {
                    spellCache.Clear();
                    Logs.Logger.LogDebug("Effects Reloaded.");
                }

                if (NeedItemReload)
                {
                    itemCache.Clear();
                    Logs.Logger.LogDebug("Items Reloaded.");
                }
            }
            catch (Exception ex)
            {
                Logs.Logger.LogError(new Exception("Could not reload asset.\n", ex));
            }

            NeedTileReload = false;
            NeedSpriteReload = false;
            NeedPortraitReload = false;
            NeedEffectReload = false;
            NeedItemReload = false;
        }


        public static SpriteSheet GetSpriteSheet(int num, int form, Enums.Coloration shiny, Enums.Gender gender) {
            string formString = "r";

            if (form >= 0)
            {
                formString += "-" + form;
                if (shiny >= 0)
                {
                    formString += "-" + (int)shiny;
                    if (gender >= 0)
                    {
                        formString += "-" + (int)gender;
                    }
                }
            }

            SpriteSheet sheet = spriteCache.Get(num + formString);
            
            if (sheet != null)
                return sheet;

            try
            {
                // If we are still here, that means the sprite wasn't in the cache
                if (System.IO.File.Exists(Paths.CachedGFXPath + "Sprite\\Sprite" + num + ".sprite"))
                {

                    sheet = new SpriteSheet();
                    string changedFormString = formString;

                    using (FileStream fileStream = File.OpenRead(Paths.CachedGFXPath + "Sprite\\Sprite" + num + ".sprite"))
                    {
                        using (BinaryReader reader = new BinaryReader(fileStream))
                        {
                            int formCount = reader.ReadInt32();
                            Dictionary<string, int[]> formData = new Dictionary<string, int[]>();

                            for (int i = 0; i < formCount; i++)
                            {
                                // Read the form name
                                string formName = reader.ReadString();

                                int[] formIntData = new int[2];

                                // Load form position
                                formIntData[0] = reader.ReadInt32();
                                // Load form size
                                formIntData[1] = reader.ReadInt32();

                                // Add form data to collection
                                formData.Add(formName, formIntData);
                            }

                            while (true)
                            {
                                if (spriteCache.ContainsKey(num + changedFormString))
                                {//this point will be hit if the first fallback data to be found is already in the cache
                                    //the cache needs to be updated for aliases, but that's it.  No need to load any new data.

                                    sheet = spriteCache.Get(num + changedFormString);
                                    break;
                                }
                                else if (formData.ContainsKey(changedFormString) || changedFormString == "r")
                                {//we've found a spritesheet in the file, so load it.
                                    int[] formInt = formData[changedFormString];

                                    // Jump to the correct position
                                    fileStream.Seek(formInt[0], SeekOrigin.Current);

                                    sheet.LoadFromData(reader, formInt[1]);

                                    spriteCache.Add(num + changedFormString, sheet);

                                    break;
                                }

                                // If the form specified wasn't found, continually revert to the backup until only "r" is reached
                                changedFormString = changedFormString.Substring(0, changedFormString.LastIndexOf('-'));
                            }
                        }
                    }

                    //continually add aliases
                    string aliasString = formString;
                    while (aliasString != changedFormString)
                    {
                        //add aliases here
                        spriteCache.AddAlias(num + aliasString, num + changedFormString);
                        // If the form specified wasn't found, continually revert to the backup until only "r" is reached
                        aliasString = aliasString.Substring(0, aliasString.LastIndexOf('-'));
                    }

                    //string rootForm = spriteCache.GetOriginalKeyFromAlias(num + formString);
                    //if (rootForm != num + formString)
                    //{
                    //    Logs.Logger.LogDebug("Could not load " + num + formString + ", loaded " + num + rootForm +" instead.");
                    //}

                    return sheet;
                }
            }
            catch (Exception ex)
            {
                Logs.Logger.LogError(new Exception("Error retrieving sprite " + num + " " + formString+"\n", ex));
            }

            //add error sheet
            sheet = new SpriteSheet();
            spriteCache.Add(num + formString, sheet);
            return sheet;
        }

        public static AnimSheet GetSpellSheet(SpellAnimType animType, int num) {
            AnimSheet cacheSheet = spellCache.Get(animType.ToString() + num);
            if (cacheSheet != null) return cacheSheet;

            if (System.IO.File.Exists(Paths.EffectsPath + animType.ToString() + "-" + num + ".png")) {
                AnimSheet sheet = new AnimSheet();
                sheet.LoadPixelsFromFile32(Paths.EffectsPath + animType.ToString() + "-" + num + ".png");
                sheet.LoadTextureFromPixels32();
                switch (animType) {
                    case SpellAnimType.Spell:
                        sheet.GenerateDataBuffer(1, 1);
                        break;
                    case SpellAnimType.Arrow:
                        sheet.GenerateDataBuffer(8, 1);
                        break;
                    case SpellAnimType.Beam:
                        sheet.GenerateDataBuffer(8, 4);
                        break;
                }
                spellCache.Add(animType.ToString() + num, sheet);
                return sheet;
            }
            spellCache.Add(animType.ToString() + num, ErrorTexture);
            return ErrorTexture;
        }

        public static AnimSheet GetStatusSheet(int num)
        {
            AnimSheet cacheSheet = statusCache.Get(num);
            if (cacheSheet != null) return cacheSheet;

            if (System.IO.File.Exists(Paths.DataPath + "\\Graphics\\Status\\Status-" + num + ".png"))
            {
                AnimSheet sheet = new AnimSheet();
                sheet.LoadPixelsFromFile32(Paths.DataPath + "\\Graphics\\Status\\Status-" + num + ".png");
                sheet.LoadTextureFromPixels32();
                sheet.GenerateDataBuffer(1, 1);
                statusCache.Add(num, sheet);
                return sheet;
            }
            statusCache.Add(num, ErrorTexture);
            return ErrorTexture;
        }

        public static AnimSheet GetItemSheet(int num) {
            AnimSheet cacheSheet = itemCache.Get(num);
            if (cacheSheet != null) return cacheSheet;

            if (System.IO.File.Exists(Paths.ItemsPath + num + ".png")) {
                AnimSheet sheet = new AnimSheet();
                sheet.LoadPixelsFromFile32(Paths.ItemsPath + num + ".png");
                sheet.LoadTextureFromPixels32();
                sheet.GenerateDataBuffer(1, 1);
                itemCache.Add(num, sheet);
                return sheet;
            }
            itemCache.Add(num, ErrorTexture);
            return ErrorTexture;
        }

        public static AnimSheet GetObjectSheet(int num) {
            AnimSheet cacheSheet = objectCache.Get(num);
            if (cacheSheet != null) return cacheSheet;

            if (System.IO.File.Exists(Paths.DataPath + "Graphics\\Object\\Object-" + num + ".png")) {
                AnimSheet sheet = new AnimSheet();
                sheet.LoadPixelsFromFile32(Paths.DataPath + "Graphics\\Object\\Object-" + num + ".png");
                sheet.LoadTextureFromPixels32();
                sheet.GenerateDataBuffer(1, 1);
                objectCache.Add(num, sheet);
                return sheet;
            }

            objectCache.Add(num, ErrorTexture);
            return ErrorTexture;
        }


        public static TileSheet GetMugshot(int num, int form, Enums.Coloration shiny, Enums.Gender gender)
        {
            string formString = "r";

            if (form >= 0)
            {
                formString += "-" + form;
                if (shiny >= 0)
                {
                    formString += "-" + (int)shiny;
                    if (gender >= 0)
                    {
                        formString += "-" + (int)gender;
                    }
                }
            }

            TileSheet sheet = mugshotCache.Get(num + formString);

            if (sheet != null)
                return sheet;

            try
            {
                // If we are still here, that means the sprite wasn't in the cache
                if (System.IO.File.Exists(Paths.CachedGFXPath + "Portrait\\Portrait" + num + ".portrait"))
                {

                    sheet = new TileSheet();
                    string changedFormString = formString;

                    using (FileStream fileStream = File.OpenRead(Paths.CachedGFXPath + "Portrait\\Portrait" + num + ".portrait"))
                    {
                        using (BinaryReader reader = new BinaryReader(fileStream))
                        {
                            int formCount = reader.ReadInt32();
                            Dictionary<string, int[]> formData = new Dictionary<string, int[]>();

                            for (int i = 0; i < formCount; i++)
                            {
                                // Read the form name
                                string formName = reader.ReadString();

                                int[] formIntData = new int[2];

                                // Load form position
                                formIntData[0] = reader.ReadInt32();
                                // Load form size
                                formIntData[1] = reader.ReadInt32();

                                // Add form data to collection
                                formData.Add(formName, formIntData);
                            }

                            while (true)
                            {
                                if (mugshotCache.ContainsKey(num + changedFormString))
                                {//this point will be hit if the first fallback data to be found is already in the cache
                                    //the cache needs to be updated for aliases, but that's it.  No need to load any new data.

                                    sheet = mugshotCache.Get(num + changedFormString);
                                    break;
                                }
                                else if (formData.ContainsKey(changedFormString) || changedFormString == "r")
                                {//we've found a spritesheet in the file, so load it.
                                    int[] formInt = formData[changedFormString];

                                    // Jump to the correct position
                                    fileStream.Seek(formInt[0], SeekOrigin.Current);

                                    try
                                    {
                                        int frameCount = reader.ReadInt32();
                                        int size = reader.ReadInt32();
                                        byte[] imgData = reader.ReadBytes(size);
                                        using (MemoryStream stream = new MemoryStream(imgData))
                                        {
                                            sheet.LoadPixelsFromBytes(stream);
                                            sheet.LoadTextureFromPixels32();
                                        }

                                        sheet.GenerateDataBuffer(sheet.ImageWidth / frameCount, sheet.ImageHeight);
                                    }
                                    catch (Exception ex)
                                    {
                                        sheet.Dispose();
                                        throw new Exception("Error reading image data.\n", ex);
                                    }

                                    mugshotCache.Add(num + changedFormString, sheet);

                                    break;
                                }

                                // If the form specified wasn't found, continually revert to the backup until only "r" is reached
                                changedFormString = changedFormString.Substring(0, changedFormString.LastIndexOf('-'));
                            }
                        }
                    }

                    //continually add aliases
                    string aliasString = formString;
                    while (aliasString != changedFormString)
                    {
                        //add aliases here
                        mugshotCache.AddAlias(num + aliasString, num + changedFormString);
                        // If the form specified wasn't found, continually revert to the backup until only "r" is reached
                        aliasString = aliasString.Substring(0, aliasString.LastIndexOf('-'));
                    }

                    return sheet;
                }
            }
            catch (Exception ex)
            {
                Logs.Logger.LogError(new Exception("Error retrieving portrait " + num + " " + formString + "\n", ex));
            }

            //add error sheet
            mugshotCache.Add(num + formString, ErrorTexture);
            return ErrorTexture;
        }

        public static void GetTilesetTileDimensions(int sheetNum, out int x, out int y)
        {
            if (tileData[sheetNum] == null)
            {
                tileData[sheetNum] = new TileMetadata();
                tileData[sheetNum].Load(Paths.CachedGFXPath + "Tile\\Tiles" + sheetNum + ".tile");
            }
            x = Graphics.TextureManager.tileData[sheetNum].Size.Width / Graphics.TextureManager.TILE_SIZE;
            y = Graphics.TextureManager.tileData[sheetNum].Size.Height / Graphics.TextureManager.TILE_SIZE;
        }

        public static Texture GetTile(int sheetNum, Maps.Loc2D tileLoc)
        {
            try
            {
                if (tileData[sheetNum] == null)
                {
                    tileData[sheetNum] = new TileMetadata();
                    tileData[sheetNum].Load(Paths.CachedGFXPath + "Tile\\Tiles" + sheetNum + ".tile");
                }

                int tileNum = (tileData[sheetNum].Size.Width / TILE_SIZE) * tileLoc.Y + tileLoc.X;
                Texture cacheSheet = tileCache.Get(sheetNum + "-" + tileLoc.X + "-" + tileLoc.Y);
                if (cacheSheet != null) return cacheSheet;

                if (sheetNum > -1 && sheetNum < TOTAL_TILE_SHEETS && tileNum > -1 && tileNum < tileData[sheetNum].TotalTiles)
                {
                    Texture sheet = new Texture();

                    byte[] tileBytes = new byte[tileData[sheetNum].TileSizes[tileNum]];
                    using (FileStream stream = new FileStream(Paths.CachedGFXPath + "Tile\\Tiles" + sheetNum + ".tile", FileMode.Open, FileAccess.Read, FileShare.Read))
                    {
                        // Seek to the location of the tile
                        stream.Seek(tileData[sheetNum].GetTilePosition(tileNum), SeekOrigin.Begin);
                        stream.Read(tileBytes, 0, tileBytes.Length);
                    }
                    using (MemoryStream tileStream = new MemoryStream(tileBytes))
                    {
                        sheet.LoadPixelsFromBytes(tileStream);
                        sheet.LoadTextureFromPixels32();
                    }

                    tileCache.Add(sheetNum + "-" + tileLoc.X + "-" + tileLoc.Y, sheet);
                    return sheet;
                }
            }
            catch (Exception ex)
            {
                Logs.Logger.LogError(new Exception("Error retrieving tile " + tileLoc.X + ", " + tileLoc.Y + " from Tileset #" + sheetNum + "\n", ex));
            }

            tileCache.Add(sheetNum + "-" + tileLoc.X + "-" + tileLoc.Y, ErrorTexture);
            return ErrorTexture;
        }

        public static void DisposeCachedObject(IDisposable obj)
        {
            obj.Dispose();
        }


        public static void SetViewport(int x, int y, int width, int height) {

            GL.Viewport(x, y, width, height);

            //Bind basic shader program
            TextureProgram.Bind();
            //Initialize projection
            TextureProgram.SetProjection(Matrix4.CreateOrthographicOffCenter(0, width, height, 0, 1, -1));
            TextureProgram.UpdateProjection();
        }

        #endregion Methods

    }
}
