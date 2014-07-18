/*The MIT License (MIT)

Copyright (c) 2014 PMU Staff

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
using PMDToolkit.Logs;

namespace PMDToolkit.Data {
    public class ItemEntry {


        public string Name { get; set; }
        public string Desc { get; set; }
        public Enums.ItemType Type { get; set; }
        public int Price { get; set; }
        public int Rarity { get; set; }

        public int Sprite { get; set; }

        public int Req { get; set; }
        public int Req1 { get; set; }
        public int Req2 { get; set; }
        public int Req3 { get; set; }
        public int Req4 { get; set; }
        public int Req5 { get; set; }

        public int Effect { get; set; }
        public int Effect1 { get; set; }
        public int Effect2 { get; set; }
        public int Effect3 { get; set; }

        public int ThrowEffect { get; set; }
        public int Throw1 { get; set; }
        public int Throw2 { get; set; }
        public int Throw3 { get; set; }

        public ItemEntry() {
            Name = "";
            Desc = "";
        }

        public ItemEntry(ItemEntry copy) {

            Name = copy.Name;
            Desc = copy.Desc;
            Type = copy.Type;
            Price = copy.Price;
            Rarity = copy.Rarity;

            Sprite = copy.Sprite;

            Req = copy.Req;
            Req1 = copy.Req1;
            Req2 = copy.Req2;
            Req3 = copy.Req3;
            Req4 = copy.Req4;
            Req5 = copy.Req5;

            Effect = copy.Effect;
            Effect1 = copy.Effect1;
            Effect2 = copy.Effect2;
            Effect3 = copy.Effect3;

            ThrowEffect = copy.ThrowEffect;
            Throw1 = copy.Throw1;
            Throw2 = copy.Throw2;
            Throw3 = copy.Throw3;

        }


        public void Load(int itemNum) {
            using (XmlReader reader = XmlReader.Create(Paths.DataPath + "Item\\" + itemNum + ".xml")) {
                while (reader.Read()) {
                    if (reader.IsStartElement()) {
                        switch (reader.Name) {
                            case "Name": {
                                    Name = reader.ReadString();
                                    break;
                                }
                            case "Description": {
                                    Desc = reader.ReadString();
                                    break;
                                }
                            case "ItemType": {
                                    Type = reader.ReadString().ToEnum<Enums.ItemType>();
                                    break;
                                }
                            case "Price": {
                                    Price = reader.ReadString().ToInt();
                                    break;
                                }
                            case "Rarity": {
                                    Rarity = reader.ReadString().ToInt();
                                    break;
                                }
                            case "Sprite": {
                                    Sprite = reader.ReadString().ToInt();
                                    break;
                                }
                            case "Requirement": {
                                    Req = reader.ReadString().ToInt();
                                    break;
                                }
                            case "Req1": {
                                    Req1 = reader.ReadString().ToInt();
                                    break;
                                }
                            case "Req2": {
                                    Req2 = reader.ReadString().ToInt();
                                    break;
                                }
                            case "Req3": {
                                    Req3 = reader.ReadString().ToInt();
                                    break;
                                }
                            case "Req4": {
                                    Req4 = reader.ReadString().ToInt();
                                    break;
                                }
                            case "Req5": {
                                    Req5 = reader.ReadString().ToInt();
                                    break;
                                }
                            case "Effect": {
                                    Effect = reader.ReadString().ToInt();
                                    break;
                                }
                            case "Effect1": {
                                    Effect1 = reader.ReadString().ToInt();
                                    break;
                                }
                            case "Effect2": {
                                    Effect2 = reader.ReadString().ToInt();
                                    break;
                                }
                            case "Effect3": {
                                    Effect3 = reader.ReadString().ToInt();
                                    break;
                                }
                            case "ThrowEffect": {
                                    ThrowEffect = reader.ReadString().ToInt();
                                    break;
                                }
                            case "Throw1": {
                                    Throw1 = reader.ReadString().ToInt();
                                    break;
                                }
                            case "Throw2": {
                                    Throw2 = reader.ReadString().ToInt();
                                    break;
                                }
                            case "Throw3": {
                                    Throw3 = reader.ReadString().ToInt();
                                    break;
                                }
                        }
                    }
                }
            }
        }


        public void Save(int itemNum) {
            if (!Directory.Exists(Paths.DataPath + "Item"))
                Directory.CreateDirectory(Paths.DataPath + "Item");
            using (XmlWriter writer = XmlWriter.Create(Paths.DataPath + "Item\\" + itemNum + ".xml", Logger.XmlWriterSettings)) {
                writer.WriteStartDocument();
                writer.WriteStartElement("ItemEntry");

                #region Basic data
                writer.WriteStartElement("General");
                writer.WriteElementString("Name", Name);
                writer.WriteElementString("Description", Desc);
                writer.WriteElementString("ItemType", Type.ToString());
                writer.WriteElementString("Price", Price.ToString());
                writer.WriteElementString("Rarity", Rarity.ToString());
                writer.WriteElementString("Sprite", Sprite.ToString());
                writer.WriteElementString("Requirement", Req.ToString());
                writer.WriteElementString("Req1", Req1.ToString());
                writer.WriteElementString("Req2", Req2.ToString());
                writer.WriteElementString("Req3", Req3.ToString());
                writer.WriteElementString("Req4", Req4.ToString());
                writer.WriteElementString("Req5", Req5.ToString());
                writer.WriteElementString("Effect", Effect.ToString());
                writer.WriteElementString("Effect1", Effect1.ToString());
                writer.WriteElementString("Effect2", Effect2.ToString());
                writer.WriteElementString("Effect3", Effect3.ToString());
                writer.WriteElementString("ThrowEffect", ThrowEffect.ToString());
                writer.WriteElementString("Throw1", Throw1.ToString());
                writer.WriteElementString("Throw2", Throw2.ToString());
                writer.WriteElementString("Throw3", Throw3.ToString());
                writer.WriteEndElement();
                #endregion

                writer.WriteEndElement();
                writer.WriteEndDocument();
            }
        }

    }
}
