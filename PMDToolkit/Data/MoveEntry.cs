using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;
using PMDToolkit.Core;
using PMDToolkit.Logs;

namespace PMDToolkit.Data {
    public class MoveEntry {


        public string Name { get; set; }
        public string Desc { get; set; }

        public int PP { get; set; }

        public Enums.Element Type { get; set; }
        public Enums.MoveCategory Category { get; set; }
        public bool Contact { get; set; }
        public bool SoundBased { get; set; }
        public bool FistBased { get; set; }
        public bool PulseBased { get; set; }
        public bool BulletBased { get; set; }
        public bool JawBased { get; set; }
        public int Power { get; set; }
        public int Accuracy { get; set; }

        public int Effect { get; set; }
        public int Effect1 { get; set; }
        public int Effect2 { get; set; }
        public int Effect3 { get; set; }


        public MoveRange Range { get; set; }

        //note: every attack is assumed to have a different animation
        //however, some animations use the same graphics/effects on sprites
        //attack is split into user, traveling, target animations
        public MoveAnimation StartAnim { get; set; }
        public CharAnimation StartUserAnim { get; set; }

        public int StartSound { get; set; }

        public MoveAnimation MidAnim { get; set; }
        public CharAnimation MidUserAnim { get; set; }
        public CharAnimation MidTargetAnim { get; set; }

        public int MidSound { get; set; }

        public MoveAnimation EndAnim { get; set; }
        public CharAnimation EndUserAnim { get; set; }
        public CharAnimation EndTargetAnim { get; set; }

        public int EndSound { get; set; }

        public MoveEntry() {
            Name = "";
            Desc = "";
            Range = new MoveRange();
            StartAnim = new MoveAnimation();
            StartUserAnim = new CharAnimation();
            MidAnim = new MoveAnimation();
            MidUserAnim = new CharAnimation();
            MidTargetAnim = new CharAnimation();
            EndAnim = new MoveAnimation();
            EndUserAnim = new CharAnimation();
            EndTargetAnim = new CharAnimation();
        }

        public MoveEntry(MoveEntry copy) {
            Name = copy.Name;
            Desc = copy.Desc;

            PP = copy.PP;

            Type = copy.Type;
            Category = copy.Category;
            Contact = copy.Contact;
            SoundBased = copy.SoundBased;
            FistBased = copy.FistBased;
            PulseBased = copy.PulseBased;
            BulletBased = copy.BulletBased;
            JawBased = copy.JawBased;

            Power = copy.Power;
            Accuracy = copy.Accuracy;

            Effect = copy.Effect;
            Effect1 = copy.Effect1;
            Effect2 = copy.Effect2;
            Effect3 = copy.Effect3;

            Range = new MoveRange(copy.Range);

            StartAnim = new MoveAnimation(copy.StartAnim);
            StartUserAnim = new CharAnimation(copy.StartUserAnim);
            StartSound = copy.StartSound;
            MidAnim = new MoveAnimation(copy.MidAnim);
            MidUserAnim = new CharAnimation(copy.MidUserAnim);
            MidTargetAnim = new CharAnimation(copy.MidTargetAnim);
            MidSound = copy.MidSound;
            EndAnim = new MoveAnimation(copy.EndAnim);
            EndUserAnim = new CharAnimation(copy.EndUserAnim);
            EndTargetAnim = new CharAnimation(copy.EndTargetAnim);
            EndSound = copy.EndSound;
        }


        public void Load(int moveNum) {
            using (XmlReader reader = XmlReader.Create(Paths.DataPath + "Move\\" + moveNum + ".xml")) {
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
                            case "PP": {
                                    PP = reader.ReadString().ToInt();
                                    break;
                                }
                            case "Type": {
                                    Type = reader.ReadString().ToEnum<Enums.Element>();
                                    break;
                                }
                            case "Category": {
                                    Category = reader.ReadString().ToEnum<Enums.MoveCategory>();
                                    break;
                                }
                            case "Contact": {
                                    Contact = reader.ReadString().ToBool();
                                    break;
                                }
                            case "SoundBased": {
                                    SoundBased = reader.ReadString().ToBool();
                                    break;
                                }
                            case "FistBased": {
                                    FistBased = reader.ReadString().ToBool();
                                    break;
                                }
                            case "PulseBased": {
                                    PulseBased = reader.ReadString().ToBool();
                                    break;
                                }
                            case "BulletBased": {
                                    BulletBased = reader.ReadString().ToBool();
                                    break;
                                }
                            case "JawBased": {
                                    JawBased = reader.ReadString().ToBool();
                                    break;
                                }
                            case "Power": {
                                    Power = reader.ReadString().ToInt();
                                    break;
                                }
                            case "Accuracy": {
                                    Accuracy = reader.ReadString().ToInt();
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
                            case "HitsSelf": {
                                    Range.HitsSelf = reader.ReadString().ToBool();
                                    break;
                                }
                            case "HitsFriend": {
                                    Range.HitsFriend = reader.ReadString().ToBool();
                                    break;
                                }
                            case "HitsFoe": {
                                    Range.HitsFoe = reader.ReadString().ToBool();
                                    break;
                                }
                            case "Range": {
                                    Range.RangeType = reader.ReadString().ToEnum<Enums.RangeType>();
                                    break;
                                }
                            case "Mobility": {
                                    Range.Mobility = reader.ReadString().ToInt();
                                    break;
                                }
                            case "CutsCorners": {
                                    Range.CutsCorners = reader.ReadString().ToBool();
                                    break;
                                }
                            case "Distance": {
                                    Range.Distance = reader.ReadString().ToInt();
                                    break;
                                }
                            case "StartAnim": {
                                    StartAnim.Load(reader.ReadSubtree());
                                    break;
                                }
                            case "StartUserAnim": {
                                    StartUserAnim.Load(reader.ReadSubtree());
                                    break;
                                }
                            case "StartSound": {
                                    StartSound = reader.ReadString().ToInt();
                                    break;
                                }
                            case "MidAnim": {
                                    MidAnim.Load(reader.ReadSubtree());
                                    break;
                                }
                            case "MidUserAnim": {
                                    MidUserAnim.Load(reader.ReadSubtree());
                                    break;
                                }
                            case "MidTargetAnim": {
                                    MidTargetAnim.Load(reader.ReadSubtree());
                                    break;
                                }
                            case "MidSound": {
                                    MidSound = reader.ReadString().ToInt();
                                    break;
                                }
                            case "EndAnim": {
                                    EndAnim.Load(reader.ReadSubtree());
                                    break;
                                }
                            case "EndUserAnim": {
                                    EndUserAnim.Load(reader.ReadSubtree());
                                    break;
                                }
                            case "EndTargetAnim": {
                                    EndTargetAnim.Load(reader.ReadSubtree());
                                    break;
                                }
                            case "EndSound": {
                                    EndSound = reader.ReadString().ToInt();
                                    break;
                                }
                        }
                    }
                }
            }
        }


        public void Save(int moveNum) {
            if (!Directory.Exists(Paths.DataPath + "Move"))
                Directory.CreateDirectory(Paths.DataPath + "Move");
            using (XmlWriter writer = XmlWriter.Create(Paths.DataPath + "Move\\" + moveNum + ".xml", Logger.XmlWriterSettings)) {
                writer.WriteStartDocument();
                writer.WriteStartElement("MoveEntry");

                #region Basic data
                writer.WriteStartElement("General");
                writer.WriteElementString("Name", Name);
                writer.WriteElementString("Description", Desc);
                writer.WriteElementString("PP", PP.ToString());
                writer.WriteElementString("Type", Type.ToString());
                writer.WriteElementString("Category", Category.ToString());
                writer.WriteElementString("Contact", Contact.ToString());
                writer.WriteElementString("SoundBased", SoundBased.ToString());
                writer.WriteElementString("FistBased", FistBased.ToString());
                writer.WriteElementString("PulseBased", PulseBased.ToString());
                writer.WriteElementString("BulletBased", BulletBased.ToString());
                writer.WriteElementString("JawBased", BulletBased.ToString());
                writer.WriteElementString("Power", Power.ToString());
                writer.WriteElementString("Accuracy", Accuracy.ToString());
                writer.WriteElementString("Effect", Effect.ToString());
                writer.WriteElementString("Effect1", Effect1.ToString());
                writer.WriteElementString("Effect2", Effect2.ToString());
                writer.WriteElementString("Effect3", Effect3.ToString());
                writer.WriteElementString("HitsSelf", Range.HitsSelf.ToString());
                writer.WriteElementString("HitsFriend", Range.HitsFriend.ToString());
                writer.WriteElementString("HitsFoe", Range.HitsFoe.ToString());
                writer.WriteElementString("Range", Range.RangeType.ToString());
                writer.WriteElementString("Mobility", Range.Mobility.ToString());
                writer.WriteElementString("CutsCorners", Range.CutsCorners.ToString());
                writer.WriteElementString("Distance", Range.Distance.ToString());
                writer.WriteStartElement("StartAnim");
                StartAnim.Save(writer);
                writer.WriteEndElement();
                writer.WriteStartElement("StartUserAnim");
                StartUserAnim.Save(writer);
                writer.WriteEndElement();
                writer.WriteElementString("StartSound", StartSound.ToString());
                writer.WriteStartElement("MidAnim");
                MidAnim.Save(writer);
                writer.WriteEndElement();
                writer.WriteStartElement("MidUserAnim");
                MidUserAnim.Save(writer);
                writer.WriteEndElement();
                writer.WriteStartElement("MidTargetAnim");
                MidTargetAnim.Save(writer);
                writer.WriteEndElement();
                writer.WriteElementString("MidSound", MidSound.ToString());
                writer.WriteStartElement("EndAnim");
                EndAnim.Save(writer);
                writer.WriteEndElement();
                writer.WriteStartElement("EndUserAnim");
                EndUserAnim.Save(writer);
                writer.WriteEndElement();
                writer.WriteStartElement("EndTargetAnim");
                EndTargetAnim.Save(writer);
                writer.WriteEndElement();
                writer.WriteElementString("EndSound", EndSound.ToString());
                writer.WriteEndElement();
                #endregion

                writer.WriteEndElement();
                writer.WriteEndDocument();
            }
        }
    }
}
