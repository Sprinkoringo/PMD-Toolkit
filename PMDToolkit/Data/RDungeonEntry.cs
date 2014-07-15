using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;
using PMDToolkit.Core;
using PMDToolkit.Logs;

namespace PMDToolkit.Data {
    public class RDungeonEntry {

        public string Name { get; set; }

        public int Algorithm { get; set; }

        public Dictionary<string, int> DungeonSettings { get; set; }

        public List<RDungeonFloor> Floors { get; set; }

        public RDungeonEntry() {
            DungeonSettings = new Dictionary<string, int>();
            Floors = new List<RDungeonFloor>();
            Algorithm = -1;
        }

        public void LoadAlgorithmSettings() {
            DungeonSettings.Clear();
            if (Algorithm > -1){
                List<Tuple<string, bool>> settings = GameData.DungeonAlgorithmDex[Algorithm].DungeonSettings;
                for (int i = 0; i < settings.Count; i++) {
                    DungeonSettings.Add(settings[i].Item1, 0);
                }
            }
        }

        public void Load(int rdungeonNum) {
            //load settings from algorithm

            using (XmlReader reader = XmlReader.Create(Paths.DataPath + "RDungeon\\" + rdungeonNum + "\\base.xml")) {
                while (reader.Read()) {
                    if (reader.IsStartElement()) {
                        switch (reader.Name) {
                            case "Name": {
                                    Name = reader.ReadString();
                                    break;
                                }
                            case "Algorithm": {
                                    Algorithm = reader.ReadString().ToInt();
                                    LoadAlgorithmSettings();
                                    break;
                                }
                            case "DungeonSettings": {
                                    XmlReader settingsReader = reader.ReadSubtree();
                                    while (settingsReader.Read()) {
                                        if (settingsReader.IsStartElement() && DungeonSettings.ContainsKey(settingsReader.Name)) {
                                            DungeonSettings[settingsReader.Name] = settingsReader.ReadString().ToInt();
                                        }
                                    }
                                    break;
                                }
                        }
                    }
                }
            }
            int floorNum = 0;
            while (File.Exists(Paths.DataPath + "RDungeon\\" + rdungeonNum + "\\" + floorNum + ".xml")) {
                RDungeonFloor floor = new RDungeonFloor();
                floor.Load(rdungeonNum, floorNum);
                Floors.Add(floor);
                floorNum++;
            }
        }

        public void Save(int rdungeonNum) {
            if (!Directory.Exists(Paths.DataPath + "RDungeon\\" + rdungeonNum))
                Directory.CreateDirectory(Paths.DataPath + "RDungeon\\" + rdungeonNum);
            using (XmlWriter writer = XmlWriter.Create(Paths.DataPath + "RDungeon\\" + rdungeonNum + "\\base.xml", Logger.XmlWriterSettings)) {
                writer.WriteStartDocument();
                writer.WriteStartElement("RDungeonEntry");
                #region Basic data
                writer.WriteStartElement("General");
                writer.WriteElementString("Name", Name);
                writer.WriteElementString("Algorithm", Algorithm.ToString());
                writer.WriteEndElement();
                #endregion
                #region Dungeon Settings
                writer.WriteStartElement("DungeonSettings");
                for (int i = 0; i < DungeonSettings.Count; i++) {
                    string key = DungeonSettings.Keys.ElementAt(i);
                    writer.WriteElementString(key, DungeonSettings[key].ToString());
                }
                writer.WriteEndElement();
                #endregion
                writer.WriteEndElement();
                writer.WriteEndDocument();
            }
            for (int i = 0; i < Floors.Count; i++) {
                Floors[i].Save(rdungeonNum, i);
            }
        }
    }
}
