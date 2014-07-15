namespace PMDToolkit.Data {
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Xml;
    using System.IO;
    using PMDToolkit.Core;
    using PMDToolkit.Logs;

    public class DexEntry {


        public int DexNum { get; set; }

        public string Name { get; set; }

        public string SpeciesName { get; set; }

        public Enums.GrowthGroup Growth { get; set; }

        public Enums.EggGroup EggGroup1 { get; set; }

        public Enums.EggGroup EggGroup2 { get; set; }

        public List<DexFormEntry> Forms { get; set; }

        public DexEntry() {
            Forms = new List<DexFormEntry>();
        }


        public void Load(int dexNum) {
            DexNum = dexNum;
            using (XmlReader reader = XmlReader.Create(Paths.DataPath + "Dex\\" + DexNum + ".xml")) {
                while (reader.Read()) {
                    if (reader.IsStartElement()) {
                        switch (reader.Name) {
                            case "DexNum": {
                                    DexNum = reader.ReadString().ToInt();
                                    break;
                                }
                            case "Name": {
                                    Name = reader.ReadString();
                                    break;
                                }
                            case "SpeciesName": {
                                    SpeciesName = reader.ReadString();
                                    break;
                                }
                            case "GrowthGroup": {
                                    Growth = reader.ReadString().ToEnum<Enums.GrowthGroup>();
                                    break;
                                }
                            case "EggGroup1":
                                {
                                    string group = reader.ReadString();
                                    if (group.IsEnum<Enums.EggGroup>())
                                        EggGroup1 = group.ToEnum<Enums.EggGroup>();
                                    break;
                                }
                            case "EggGroup2":
                                {
                                    string group = reader.ReadString();
                                    if (group.IsEnum<Enums.EggGroup>())
                                        EggGroup2 = group.ToEnum<Enums.EggGroup>();
                                    break;
                                }
                        }
                    }
                }
            }

            int formNum = 0;
            while (File.Exists(Paths.DataPath + "Dex\\" + DexNum + "-" + formNum + ".xml")) {
                DexFormEntry form = new DexFormEntry();
                form.Load(DexNum, formNum);
                Forms.Add(form);
                formNum++;
            }
        }


        public void Save() {
            if (!Directory.Exists(Paths.DataPath + "Dex\\" + DexNum))
                Directory.CreateDirectory(Paths.DataPath + "Dex\\" + DexNum);
            using (XmlWriter writer = XmlWriter.Create(Paths.DataPath + "Dex\\" + DexNum + ".xml", Logger.XmlWriterSettings)) {
                writer.WriteStartDocument();
                writer.WriteStartElement("DexEntry");

                #region Basic data
                writer.WriteStartElement("General");
                writer.WriteElementString("Name", Name);
                writer.WriteElementString("SpeciesName", SpeciesName);
                writer.WriteElementString("GrowthGroup", Growth.ToString());
                writer.WriteElementString("EggGroup1", EggGroup1.ToString());
                writer.WriteElementString("EggGroup2", EggGroup2.ToString());
                writer.WriteEndElement();
                #endregion

                writer.WriteEndElement();
                writer.WriteEndDocument();
            }

            foreach (DexFormEntry form in Forms) {
                form.Save(DexNum);
            }
        }

    }
}