using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;
using PMDToolkit.Logs;

namespace PMDToolkit.Data
{
    public static class Paths
    {
#if DEBUG
        public const string AssetPath = "..\\..\\..\\Dependencies\\Assets\\";
#else
        public const string AssetPath = "";
#endif
        public static string DataPath { get { return AssetPath + "Base\\"; } }

        public static string BaseGFXPath { get { return AssetPath + "Base\\Graphics\\"; } }

        public static string ShadersPath { get { return AssetPath + "Base\\Shaders\\"; } }

        public static string CachedGFXPath { get { return "Cache\\"; } }

        public static string MapPath { get { return "Maps\\"; } }

        public static string TilesPath { get; set; }

        public static string SpritesPath { get; set; }

        public static string PortraitsPath { get; set; }

        public static string EffectsPath { get; set; }

        public static string ItemsPath { get; set; }

        public static string MusicPath { get; set; }

        public static string SoundsPath { get; set; }

        public static void Init()
        {

            if (File.Exists("Paths.xml"))
            {
                try
                {
                    LoadPaths();
                    return;
                }
                catch (Exception ex)
                {
                    Logs.Logger.LogError(new Exception("Error loading Paths file.\n", ex));
                }
            }
            CreateDefaultPaths();
        }

        public static void CreateDefaultPaths()
        {
            //set all paths to default
            
            TilesPath = AssetPath + "Tile\\";
            SpritesPath = AssetPath + "Sprite\\";
            PortraitsPath = AssetPath + "Portrait\\";
            EffectsPath = AssetPath + "Effect\\";
            ItemsPath = AssetPath + "Item\\";
            MusicPath = AssetPath + "Music\\";
            SoundsPath = AssetPath + "Sound\\";

            //save
            SavePaths();
        }

        static void LoadPaths()
        {

            using (XmlReader reader = XmlReader.Create("Paths.xml"))
            {
                while (reader.Read())
                {
                    if (reader.IsStartElement())
                    {
                        switch (reader.Name)
                        {
                            case "Tile":
                                {
                                    TilesPath = reader.ReadString();
                                    break;
                                }
                            case "Sprite":
                                {
                                    SpritesPath = reader.ReadString();
                                    break;
                                }
                            case "Portrait":
                                {
                                    PortraitsPath = reader.ReadString();
                                    break;
                                }
                            case "Effect":
                                {
                                    EffectsPath = reader.ReadString();
                                    break;
                                }
                            case "Item":
                                {
                                    ItemsPath = reader.ReadString();
                                    break;
                                }
                            case "Music":
                                {
                                    MusicPath = reader.ReadString();
                                    break;
                                }
                            case "Sound":
                                {
                                    SoundsPath = reader.ReadString();
                                    break;
                                }
                        }
                    }
                }
            }
        }

        public static void SavePaths()
        {
            using (XmlWriter writer = XmlWriter.Create("Paths.xml", Logger.XmlWriterSettings))
            {
                writer.WriteStartDocument();
                writer.WriteStartElement("Paths");

                writer.WriteElementString("Tile", TilesPath);
                writer.WriteElementString("Sprite", SpritesPath);
                writer.WriteElementString("Portrait", PortraitsPath);
                writer.WriteElementString("Effect", EffectsPath);
                writer.WriteElementString("Item", ItemsPath);
                writer.WriteElementString("Music", MusicPath);
                writer.WriteElementString("Sound", SoundsPath);

                writer.WriteEndElement();
                writer.WriteEndDocument();
            }
        }
    }
}
