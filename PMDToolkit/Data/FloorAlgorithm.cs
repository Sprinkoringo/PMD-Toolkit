using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;
using PMDToolkit.Core;
using System.Reflection;
using PMDToolkit.Logs;

namespace PMDToolkit.Data {
    public class FloorAlgorithm {

        //begin with:
        //loading of set maps
        //floor-by-floor
        //intertwining dungeon

        public int Num { get; set; }

        public string Name { get; set; }

        public List<Tuple<string, bool>> FloorSettings { get; set; }
        public List<string> LayerSettings { get; set; }
        //tile data
        public List<string> TileSettings { get; set; }
        public List<string> FoliageSettings { get; set; }
        public List<string> CoverSettings { get; set; }

        public List<string> ItemGroups { get; set; }
        public List<string> NpcGroups { get; set; }
        public List<string> RoomGroups { get; set; }

        public FloorAlgorithm() {
            FloorSettings = new List<Tuple<string, bool>>();
            LayerSettings = new List<string>();
            TileSettings = new List<string>();
            FoliageSettings = new List<string>();
            CoverSettings = new List<string>();

            ItemGroups = new List<string>();
            NpcGroups = new List<string>();
            RoomGroups = new List<string>();
        }

        

        public void Load(int algorithmNum) {
            Num = algorithmNum;
            using (XmlReader reader = XmlReader.Create(Paths.DataPath + "FloorAlgorithm\\" + algorithmNum + ".xml")) {
                while (reader.Read()) {
                    if (reader.IsStartElement()) {
                        switch (reader.Name) {
                            case "Name": {
                                    Name = reader.ReadString();
                                    break;
                                }
                            case "FloorSetting": {
                                    if (reader.Read()) {
                                        string settingName = reader.ReadElementString("FloorInt");
                                        bool settingBool = reader.ReadElementString("FloorIntBool").ToBool();
                                        FloorSettings.Add(new Tuple<string, bool>(settingName, settingBool));
                                    }
                                }
                                break;
                            case "LayerSetting": {
                                    LayerSettings.Add(reader.ReadString());
                                }
                                break;
                            case "TileSetting": {
                                    TileSettings.Add(reader.ReadString());
                                }
                                break;
                            case "FoliageSetting": {
                                    FoliageSettings.Add(reader.ReadString());
                                }
                                break;
                            case "CoverSetting": {
                                    CoverSettings.Add(reader.ReadString());
                                }
                                break;
                            case "ItemGroup": {
                                    ItemGroups.Add(reader.ReadString());
                                }
                                break;
                            case "NpcGroup": {
                                    NpcGroups.Add(reader.ReadString());
                                }
                                break;
                            case "RoomGroup": {
                                    RoomGroups.Add(reader.ReadString());
                                }
                                break;
                        }
                    }
                }
            }
        }


        public void Save(int algorithmNum) {
            if (!Directory.Exists(Paths.DataPath + "FloorAlgorithm"))
                Directory.CreateDirectory(Paths.DataPath + "FloorAlgorithm");
            using (XmlWriter writer = XmlWriter.Create(Paths.DataPath + "FloorAlgorithm\\" + algorithmNum + ".xml", Logger.XmlWriterSettings)) {
                writer.WriteStartDocument();
                writer.WriteStartElement("AlgorithmEntry");

                #region Basic data
                writer.WriteStartElement("General");
                writer.WriteElementString("Name", Name);
                writer.WriteEndElement();
                #endregion
                #region Floor Settings
                writer.WriteStartElement("FloorSettings");
                for (int i = 0; i < FloorSettings.Count; i++) {
                    writer.WriteStartElement("FloorSetting");
                    writer.WriteElementString("FloorInt", FloorSettings[i].Item1);
                    writer.WriteElementString("FloorIntBool", FloorSettings[i].Item2.ToString());
                    writer.WriteEndElement();
                }
                writer.WriteEndElement();
                #endregion
                #region Layer Settings
                writer.WriteStartElement("LayerSettings");
                for (int i = 0; i < LayerSettings.Count; i++) {
                    writer.WriteElementString("LayerSetting", LayerSettings[i]);
                }
                writer.WriteEndElement();
                #endregion
                #region Tile Settings
                writer.WriteStartElement("TileSettings");
                for (int i = 0; i < TileSettings.Count; i++) {
                    writer.WriteElementString("TileSetting", TileSettings[i]);
                }
                writer.WriteEndElement();
                #endregion
                #region Foliage Settings
                writer.WriteStartElement("FoliageSettings");
                for (int i = 0; i < FoliageSettings.Count; i++) {
                    writer.WriteElementString("FoliageSetting", FoliageSettings[i]);
                }
                writer.WriteEndElement();
                #endregion
                #region Cover Settings
                writer.WriteStartElement("CoverSettings");
                for (int i = 0; i < CoverSettings.Count; i++) {
                    writer.WriteElementString("CoverSetting", CoverSettings[i]);
                }
                writer.WriteEndElement();
                #endregion
                #region Item Settings
                writer.WriteStartElement("ItemGroups");
                for (int i = 0; i < ItemGroups.Count; i++) {
                    writer.WriteElementString("ItemGroup", ItemGroups[i]);
                }
                writer.WriteEndElement();
                #endregion
                #region NPC Settings
                writer.WriteStartElement("NpcGroups");
                for (int i = 0; i < NpcGroups.Count; i++) {
                    writer.WriteElementString("NpcGroup", NpcGroups[i]);
                }
                writer.WriteEndElement();
                #endregion
                #region Room Settings
                writer.WriteStartElement("RoomGroups");
                for (int i = 0; i < RoomGroups.Count; i++) {
                    writer.WriteElementString("RoomGroup", RoomGroups[i]);
                }
                writer.WriteEndElement();
                #endregion

                writer.WriteEndElement();
                writer.WriteEndDocument();
            }
        }

        public Maps.RandomMap CreateFloor() {
            switch (Num) {
                case 0: {
                        //return new Maps.Floors.GridRooms();
                    return new Maps.Floors.BranchRooms();
                    }
                default: {
                    return null;
                    }
            }
        }
    }
}
