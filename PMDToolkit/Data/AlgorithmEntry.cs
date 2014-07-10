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
using System.Xml;
using System.IO;
using PMDToolkit.Core;
using System.Reflection;
using PMDToolkit.Logs;

namespace PMDToolkit.Data {
    public class AlgorithmEntry {

        //begin with:
        //loading of set maps
        //floor-by-floor
        //intertwining dungeon

        public int Num { get; set; }

        public string Name { get; set; }

        public List<Tuple<string, bool>> DungeonSettings { get; set; }

        public AlgorithmEntry() {
            DungeonSettings = new List<Tuple<string, bool>>();
        }


        public void Load(int algorithmNum) {
            Num = algorithmNum;
            using (XmlReader reader = XmlReader.Create(Paths.DataPath + "DungeonAlgorithm\\" + algorithmNum + ".xml")) {
                while (reader.Read()) {
                    if (reader.IsStartElement()) {
                        switch (reader.Name) {
                            case "Name": {
                                    Name = reader.ReadString();
                                    break;
                                }
                            case "DungeonSetting": {
                                    if (reader.Read()) {
                                        string settingName = reader.ReadElementString("DungeonInt");
                                        bool settingBool = reader.ReadElementString("DungeonIntBool").ToBool();
                                        DungeonSettings.Add(new Tuple<string, bool>(settingName, settingBool));
                                    }
                                }
                                break;
                        }
                    }
                }
            }
        }

        public void Save(int algorithmNum) {
            if (!Directory.Exists(Paths.DataPath + "DungeonAlgorithm"))
                Directory.CreateDirectory(Paths.DataPath + "DungeonAlgorithm");
            using (XmlWriter writer = XmlWriter.Create(Paths.DataPath + "DungeonAlgorithm\\" + algorithmNum + ".xml", Logger.XmlWriterSettings)) {
                writer.WriteStartDocument();
                writer.WriteStartElement("AlgorithmEntry");

                #region Basic data
                writer.WriteStartElement("General");
                writer.WriteElementString("Name", Name);
                writer.WriteEndElement();
                #endregion
                #region Dungeon Settings
                writer.WriteStartElement("DungeonSettings");
                for (int i = 0; i < DungeonSettings.Count; i++) {
                    writer.WriteStartElement("DungeonSetting");
                    writer.WriteElementString("DungeonInt", DungeonSettings[i].Item1);
                    writer.WriteElementString("DungeonIntBool", DungeonSettings[i].Item2.ToString());
                    writer.WriteEndElement();
                }
                writer.WriteEndElement();
                #endregion

                writer.WriteEndElement();
                writer.WriteEndDocument();
            }
        }

        public Maps.BasicDungeon CreateDungeon() {
            switch (Num) {
                case 0: {
                        return new Maps.Dungeons.FloorByFloor();
                    }
                default: {
                        return null;
                    }
            }
        }
    }
}
