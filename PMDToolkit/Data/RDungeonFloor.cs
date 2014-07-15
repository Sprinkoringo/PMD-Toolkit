using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;
using PMDToolkit.Core;
using PMDToolkit.Logs;

namespace PMDToolkit.Data {
    public class RDungeonFloor {

        public int Algorithm { get; set; }

        public Dictionary<string, int> FloorSettings { get; set; }

        public Dictionary<string, List<Maps.TileAnim>> TileLayers { get; set; }
        public Dictionary<string, List<Maps.TileData>> TileData { get; set; }

        public Dictionary<Enums.Weather, int> FloorWeather { get; set; }

        public int Darkness { get; set; }
        public string Music { get; set; }


        public int NpcSpawnTime { get; set; }
        public int NpcMin { get; set; }
        public int NpcMax { get; set; }

        public Dictionary<string, List<Tuple<ItemPreset, int>>> Items { get; set; }
        public Dictionary<string, List<Tuple<NpcPreset, int>>> Npcs { get; set; }
        public Dictionary<string, List<Tuple<int, int>>> Rooms { get; set; }

        public RDungeonFloor() {
            Algorithm = -1;
            FloorSettings = new Dictionary<string, int>();
            TileLayers = new Dictionary<string, List<Maps.TileAnim>>();
            TileData = new Dictionary<string, List<Maps.TileData>>();
            FloorWeather = new Dictionary<Enums.Weather, int>();
            Items = new Dictionary<string, List<Tuple<ItemPreset, int>>>();
            Npcs = new Dictionary<string, List<Tuple<NpcPreset, int>>>();
            Rooms = new Dictionary<string, List<Tuple<int, int>>>();
        }


        public void LoadAlgorithmSettings() {
            FloorSettings.Clear();
            TileLayers.Clear();
            TileData.Clear();
            Items.Clear();
            Npcs.Clear();
            Rooms.Clear();
            if (Algorithm > -1) {
                FloorAlgorithm settingsGuide = GameData.FloorAlgorithmDex[Algorithm];
                for (int i = 0; i < settingsGuide.FloorSettings.Count; i++) {
                    FloorSettings.Add(settingsGuide.FloorSettings[i].Item1, 0);
                }
                for (int i = 0; i < settingsGuide.LayerSettings.Count; i++) {
                    TileLayers.Add(settingsGuide.LayerSettings[i], new List<Maps.TileAnim>());
                }
                for (int i = 0; i < settingsGuide.TileSettings.Count; i++) {
                    TileData.Add(settingsGuide.TileSettings[i], new List<Maps.TileData>());
                }
                for (int i = 0; i < settingsGuide.ItemGroups.Count; i++) {
                    Items.Add(settingsGuide.ItemGroups[i], new List<Tuple<ItemPreset, int>>());
                }
                for (int i = 0; i < settingsGuide.NpcGroups.Count; i++) {
                    Npcs.Add(settingsGuide.NpcGroups[i], new List<Tuple<NpcPreset, int>>());
                }
                for (int i = 0; i < settingsGuide.RoomGroups.Count; i++) {
                    Rooms.Add(settingsGuide.RoomGroups[i], new List<Tuple<int, int>>());
                }
            }
        }

        public void Load(int rdungeonNum, int floorNum) {
            using (XmlReader reader = XmlReader.Create(Paths.DataPath + "RDungeon\\" + rdungeonNum.ToString() + "\\" + floorNum + ".xml")) {
                while (reader.Read()) {
                    if (reader.IsStartElement()) {
                        switch (reader.Name) {
                            case "Algorithm": {
                                    Algorithm = reader.ReadString().ToInt();
                                    LoadAlgorithmSettings();
                                    break;
                                }
                            case "FloorSettings": {
                                    XmlReader settingsReader = reader.ReadSubtree();
                                    while (settingsReader.Read()) {
                                        if (settingsReader.IsStartElement() && FloorSettings.ContainsKey(settingsReader.Name)) {
                                            FloorSettings[settingsReader.Name] = settingsReader.ReadString().ToInt();
                                        }
                                    }
                                    break;
                                }
                            case "TileLayers": {
                                    XmlReader settingsReader = reader.ReadSubtree();
                                    while (settingsReader.Read()) {
                                        if (settingsReader.IsStartElement() && FloorSettings.ContainsKey(settingsReader.Name)) {
                                            string settingName = settingsReader.Name;
                                            if (settingsReader.Read()) {
                                                List<Maps.TileAnim> layerList = new List<Maps.TileAnim>();
                                                while (settingsReader.Read())
                                                {
                                                    int tileNumX = settingsReader.ReadElementString("TextureX").ToInt();
                                                    int tileNumY = settingsReader.ReadElementString("TextureY").ToInt();
                                                    int tileSheet = settingsReader.ReadElementString("Sheet").ToInt();
                                                    layerList.Add(new Maps.TileAnim(new Maps.Loc2D(tileNumX, tileNumY), tileSheet));
                                                }
                                                TileLayers[settingName] = layerList;
                                            }
                                        }
                                    }
                                    break;
                                }
                            case "TileData": {
                                    XmlReader settingsReader = reader.ReadSubtree();
                                    while (settingsReader.Read()) {
                                        if (settingsReader.IsStartElement() && FloorSettings.ContainsKey(settingsReader.Name)) {
                                            string settingName = settingsReader.Name;
                                            if (settingsReader.Read()) {
                                                List<Maps.TileData> tileDataList = new List<Maps.TileData>();
                                                while (settingsReader.Read()) {
                                                    Enums.TileType type = settingsReader.ReadString().ToEnum<Enums.TileType>();
                                                    int data1 = settingsReader.ReadElementString("Data1").ToInt();
                                                    int data2 = settingsReader.ReadElementString("Data2").ToInt();
                                                    int data3 = settingsReader.ReadElementString("Data3").ToInt();
                                                    string tileString = settingsReader.ReadElementString("String");
                                                    tileDataList.Add(new Maps.TileData(type, data1, data2, data3, tileString, "", ""));
                                                }
                                                TileData[settingName] = tileDataList;
                                            }
                                        }
                                    }
                                    break;
                                }
                            case "Weather": {
                                FloorWeather.Add(reader.ReadString().ToEnum<Enums.Weather>(), reader.ReadString().ToInt());
                                    break;
                                }
                            case "Darkness": {
                                    Darkness = reader.ReadString().ToInt();
                                    break;
                                }
                            case "Music": {
                                    Music = reader.ReadString();
                                    break;
                                }
                            case "NpcSpawnTime": {
                                    NpcSpawnTime = reader.ReadString().ToInt();
                                    break;
                                }
                            case "NpcMin": {
                                    NpcMin = reader.ReadString().ToInt();
                                    break;
                                }
                            case "NpcMax": {
                                    NpcMax = reader.ReadString().ToInt();
                                    break;
                                }
                            case "Items": {
                                    XmlReader settingsReader = reader.ReadSubtree();
                                    while (settingsReader.Read()) {
                                        if (settingsReader.IsStartElement()) {
                                            string settingName = settingsReader.Name;
                                            settingsReader.Read();
                                            if (settingsReader.Read()) {
                                                List<Tuple<ItemPreset, int>> itemList = new List<Tuple<ItemPreset, int>>();
                                                while (settingsReader.Read()) {
                                                    if (settingsReader.IsStartElement() && settingsReader.Name == "Element") {
                                                        settingsReader.Read();
                                                        ItemPreset preset = new ItemPreset();
                                                        preset.ItemNum = settingsReader.ReadElementString("ItemNum").ToInt();
                                                        preset.MinAmount = settingsReader.ReadElementString("MinAmount").ToInt();
                                                        preset.MaxAmount = settingsReader.ReadElementString("MaxAmount").ToInt();
                                                        int appearanceRate = settingsReader.ReadElementString("AppearanceRate").ToInt();
                                                        preset.StickyRate = settingsReader.ReadElementString("StickyRate").ToInt();
                                                        preset.Tag = settingsReader.ReadElementString("Tag");
                                                        preset.Hidden = settingsReader.ReadElementString("Hidden").ToBool();
                                                        preset.OnGround = settingsReader.ReadElementString("Ground").ToBool();
                                                        preset.OnWater = settingsReader.ReadElementString("Water").ToBool();
                                                        preset.OnWall = settingsReader.ReadElementString("Wall").ToBool();
                                                        itemList.Add(new Tuple<ItemPreset, int>(preset, appearanceRate));
                                                    }
                                                }
                                                Items[settingName] = itemList;
                                            }
                                        }
                                    }
                                    break;
                                }
                            case "Npcs": {
                                    XmlReader settingsReader = reader.ReadSubtree();
                                    while (settingsReader.Read()) {
                                        if (settingsReader.IsStartElement()) {
                                            string settingName = settingsReader.Name;
                                            settingsReader.Read();
                                            if (settingsReader.Read()) {
                                                List<Tuple<NpcPreset, int>> npcList = new List<Tuple<NpcPreset, int>>();
                                                while (settingsReader.Read()) {
                                                    if (settingsReader.IsStartElement() && settingsReader.Name == "Element") {
                                                        settingsReader.Read();
                                                        NpcPreset preset = new NpcPreset();
                                                        preset.NpcNum = settingsReader.ReadElementString("NpcNum").ToInt();
                                                        preset.SpawnX = settingsReader.ReadElementString("SpawnX").ToInt();
                                                        preset.SpawnY = settingsReader.ReadElementString("SpawnY").ToInt();
                                                        preset.MinLevel = settingsReader.ReadElementString("MinLevel").ToInt();
                                                        preset.MaxLevel = settingsReader.ReadElementString("MaxLevel").ToInt();
                                                        int appearanceRate = settingsReader.ReadElementString("AppearanceRate").ToInt();
                                                        npcList.Add(new Tuple<NpcPreset, int>(preset, appearanceRate));
                                                    }
                                                }
                                                Npcs[settingName] = npcList;
                                            }
                                        }
                                    }
                                    break;
                                }
                            case "Rooms": {
                                    XmlReader settingsReader = reader.ReadSubtree();
                                    while (settingsReader.Read()) {
                                        if (settingsReader.IsStartElement()) {
                                            string settingName = settingsReader.Name;
                                            settingsReader.Read();
                                            if (settingsReader.Read()) {
                                                List<Tuple<int, int>> roomList = new List<Tuple<int, int>>();
                                                while (settingsReader.Read()) {
                                                    if (settingsReader.IsStartElement() && settingsReader.Name == "Element") {
                                                        settingsReader.Read();
                                                        int num = settingsReader.ReadElementString("RoomNum").ToInt();
                                                        int appearanceRate = settingsReader.ReadElementString("AppearanceRate").ToInt();
                                                        roomList.Add(new Tuple<int, int>(num, appearanceRate));
                                                    }
                                                }
                                                Rooms[settingName] = roomList;
                                            }
                                        }
                                    }
                                    break;
                                }
                        }
                    }
                }
            }
        }

        public void Save(int rdungeonNum, int floorNum) {
            if (!Directory.Exists(Paths.DataPath + "RDungeon\\" + rdungeonNum))
                Directory.CreateDirectory(Paths.DataPath + "RDungeon\\" + rdungeonNum);
            using (XmlWriter writer = XmlWriter.Create(Paths.DataPath + "RDungeon\\" + rdungeonNum + "\\" + floorNum + ".xml", Logger.XmlWriterSettings)) {
                writer.WriteStartDocument();
                writer.WriteStartElement("FloorEntry");
                #region Basic data
                writer.WriteStartElement("General");
                writer.WriteElementString("Algorithm", Algorithm.ToString());
                writer.WriteElementString("Darkness", Darkness.ToString());
                writer.WriteElementString("Music", Music);
                writer.WriteElementString("NpcSpawnTime", NpcSpawnTime.ToString());
                writer.WriteElementString("NpcMin", NpcMin.ToString());
                writer.WriteElementString("NpcMax", NpcMax.ToString());
                writer.WriteEndElement();
                #endregion
                #region FloorSettings
                writer.WriteStartElement("FloorSettings");
                for (int i = 0; i < FloorSettings.Count; i++) {
                    string key = FloorSettings.Keys.ElementAt(i);
                    writer.WriteElementString(key, FloorSettings[key].ToString());
                }
                writer.WriteEndElement();
                #endregion
                #region TileLayers
                writer.WriteStartElement("TileLayers");
                for (int i = 0; i < TileLayers.Count; i++) {
                    string key = TileLayers.Keys.ElementAt(i);
                    List<Maps.TileAnim> layers = TileLayers[key];
                    writer.WriteStartElement(key);
                    for (int j = 0; j < layers.Count; j++) {
                        writer.WriteStartElement("Element");
                        writer.WriteElementString("TextureX", layers[j].Frames[0].Texture.X.ToString());
                        writer.WriteElementString("TextureY", layers[j].Frames[0].Texture.Y.ToString());
                        writer.WriteElementString("Sheet", layers[j].Frames[0].Sheet.ToString());
                        writer.WriteEndElement();
                    }
                    writer.WriteEndElement();
                }
                writer.WriteEndElement();
                #endregion
                #region TileData
                writer.WriteStartElement("TileData");
                for (int i = 0; i < TileData.Count; i++) {
                    string key = TileData.Keys.ElementAt(i);
                    List<Maps.TileData> data = TileData[key];
                    writer.WriteStartElement(key);
                    for (int j = 0; j < data.Count; j++) {
                        writer.WriteStartElement("Element");
                        writer.WriteElementString("Type", data[j].Type.ToString());
                        writer.WriteElementString("Data1", data[j].Data1.ToString());
                        writer.WriteElementString("Data2", data[j].Data2.ToString());
                        writer.WriteElementString("Data3", data[j].Data3.ToString());
                        writer.WriteElementString("String", data[j].String1);
                        writer.WriteEndElement();
                    }
                    writer.WriteEndElement();
                }
                writer.WriteEndElement();
                #endregion
                #region Weather
                writer.WriteStartElement("FloorWeather");
                for (int i = 0; i < FloorWeather.Count; i++) {
                    writer.WriteStartElement("Weather");
                    Enums.Weather key = FloorWeather.Keys.ElementAt(i);
                    writer.WriteElementString(key.ToString(), FloorWeather[key].ToString());
                    writer.WriteEndElement();
                }
                writer.WriteEndElement();
                #endregion
                #region Items
                writer.WriteStartElement("Items");
                for (int i = 0; i < Items.Count; i++) {
                    string key = Items.Keys.ElementAt(i);
                    List<Tuple<ItemPreset, int>> data = Items[key];
                    writer.WriteStartElement(key);
                    for (int j = 0; j < data.Count; j++) {
                        writer.WriteStartElement("Element");
                        writer.WriteElementString("ItemNum", data[j].Item1.ItemNum.ToString());
                        writer.WriteElementString("MinAmount", data[j].Item1.MinAmount.ToString());
                        writer.WriteElementString("MaxAmount", data[j].Item1.MaxAmount.ToString());
                        writer.WriteElementString("AppearanceRate", data[j].Item2.ToString());
                        writer.WriteElementString("StickyRate", data[j].Item1.StickyRate.ToString());
                        writer.WriteElementString("Tag", data[j].Item1.Tag);
                        writer.WriteElementString("Hidden", data[j].Item1.Hidden.ToString());
                        writer.WriteElementString("Ground", data[j].Item1.OnGround.ToString());
                        writer.WriteElementString("Water", data[j].Item1.OnWater.ToString());
                        writer.WriteElementString("Wall", data[j].Item1.OnWall.ToString());
                        writer.WriteEndElement();
                    }
                    writer.WriteEndElement();
                }
                writer.WriteEndElement();
                #endregion
                #region Npcs
                writer.WriteStartElement("Npcs");
                for (int i = 0; i < Npcs.Count; i++) {
                    string key = Npcs.Keys.ElementAt(i);
                    List<Tuple<NpcPreset, int>> data = Npcs[key];
                    writer.WriteStartElement(key);
                    for (int j = 0; j < data.Count; j++) {
                        writer.WriteStartElement("Element");
                        writer.WriteElementString("NpcNum", data[j].Item1.NpcNum.ToString());
                        writer.WriteElementString("SpawnX", data[j].Item1.SpawnX.ToString());
                        writer.WriteElementString("SpawnY", data[j].Item1.SpawnY.ToString());
                        writer.WriteElementString("MinLevel", data[j].Item1.MinLevel.ToString());
                        writer.WriteElementString("MaxLevel", data[j].Item1.MaxLevel.ToString());
                        writer.WriteElementString("AppearanceRate", data[j].Item2.ToString());
                        writer.WriteEndElement();
                    }
                    writer.WriteEndElement();
                }
                writer.WriteEndElement();
                #endregion
                #region Rooms
                writer.WriteStartElement("Rooms");
                for (int i = 0; i < Rooms.Count; i++) {
                    string key = Rooms.Keys.ElementAt(i);
                    List<Tuple<int, int>> data = Rooms[key];
                    writer.WriteStartElement(key);
                    for (int j = 0; j < data.Count; j++) {
                        writer.WriteStartElement("Element");
                        writer.WriteElementString("RoomNum", data[j].Item1.ToString());
                        writer.WriteElementString("AppearanceRate", data[j].Item2.ToString());
                        writer.WriteEndElement();
                    }
                    writer.WriteEndElement();
                }
                writer.WriteEndElement();
                #endregion

                writer.WriteEndElement();
                writer.WriteEndDocument();
            }
        }
    }
}
