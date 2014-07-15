using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.IO;
using PMDToolkit.Core;
using PMDToolkit.Logs;

namespace PMDToolkit.Data {
    public class DexFormEntry {


        #region Constructors

        public DexFormEntry() {
            LevelUpMoves = new List<LevelUpMove>();
            TMMoves = new List<int>();
            EggMoves = new List<int>();
            TutorMoves = new List<int>();
            DWMoves = new List<int>();
            EventMoves = new List<int>();

        }

        #endregion Constructors

        #region Properties

        public string Ability1 { get; set; }

        public string Ability2 { get; set; }

        public string Ability3 { get; set; }

        public int BaseAtk { get; set; }

        public int BaseSpeed { get; set; }

        public int BaseDef { get; set; }

        public int BaseSpAtk { get; set; }

        public int BaseSpDef { get; set; }

        public int BaseHP { get; set; }

        public int ExpYield { get; set; }

        public int FormIndex { get; set; }

        public List<LevelUpMove> LevelUpMoves { get; set; }

        public string FormName { get; set; }

        public int RecruitRate { get; set; }

        public Enums.BodyShape BodyStyle { get; set; }

        public List<int> TMMoves { get; set; }

        public List<int> EggMoves { get; set; }

        public List<int> TutorMoves { get; set; }

        public List<int> DWMoves { get; set; }

        public List<int> EventMoves { get; set; }

        public Enums.Element Type1 { get; set; }

        public Enums.Element Type2 { get; set; }

        public double Height { get; set; }

        public double Weight { get; set; }

        public int MaleRatio { get; set; }

        public int FemaleRatio { get; set; }


        #endregion Properties

        #region Methods


        /// <summary>
        /// Returns the level that this Pokémon learns a certain move.
        /// </summary>
        /// <param name="moveToFind">The move to look for.</param>
        /// <returns></returns>
        public int FindMoveLevel(int moveToFind) {
            for (int i = 0; i < LevelUpMoves.Count; i++) {
                if (LevelUpMoves[i].Move == moveToFind) {
                    return LevelUpMoves[i].Level;
                }
            }
            return -1;
        }

        public int FindLevelMove(int levelLearned) {
            for (int i = 0; i < LevelUpMoves.Count; i++) {
                if (LevelUpMoves[i].Level == levelLearned) {
                    return LevelUpMoves[i].Move;
                }
            }
            return -1;
        }

        public bool CanRelearnLevelUpMove(int moveToTest, int level) {
            int moveLevel = FindMoveLevel(moveToTest);
            if (moveLevel > -1 && moveLevel <= level) {
                return true;
            } else {
                return false;
            }
        }


        public int GetMaxHP(int level) {
            if (BaseHP > 1) {
                //return BaseHP*(level + 6)/50 + level *3/2 + 6;
                return (BaseHP + 100) * (level + 1) / 70 + 10;
            } else {
                return (level / 5 + 1);
            }
        }

        public int GetMaxHPLimit() {
            if (BaseHP > 1) {
                int scaledStat = 1530 * BaseHP / (BaseHP + BaseAtk + BaseDef + BaseSpAtk + BaseSpDef + BaseSpeed);
                return (scaledStat + 100) * (100 + 1) / 70 + 10;
            } else {
                return 41;
            }
        }

        public int GetAtk(int level) {
            //return ((((BaseAtt / 2) + 45) * level / 100) + 5);
            return (BaseAtk + 100) * (level + 1) / 70 + 10;
        }

        public int GetAtkLimit() {
            if (BaseHP > 1) {
                int scaledStat = 1530 * BaseAtk / (BaseHP + BaseAtk + BaseDef + BaseSpAtk + BaseSpDef + BaseSpeed);
                return (scaledStat + 100) * (100 + 1) / 70 + 10;
            } else {
                int scaledStat = 1325 * BaseAtk / (BaseAtk + BaseDef + BaseSpAtk + BaseSpDef + BaseSpeed);
                return (scaledStat + 100) * (100 + 1) / 70 + 10;
            }
        }

        public int GetDef(int level) {
            //return (((BaseDef / 2) + 20) * level / 100) + 3;
            return (BaseDef + 100) * (level + 1) / 70 + 10;
        }

        public int GetDefLimit() {
            if (BaseHP > 1) {
                int scaledStat = 1530 * BaseDef / (BaseHP + BaseAtk + BaseDef + BaseSpAtk + BaseSpDef + BaseSpeed);
                return (scaledStat + 100) * (100 + 1) / 70 + 10;
            } else {
                int scaledStat = 1325 * BaseDef / (BaseAtk + BaseDef + BaseSpAtk + BaseSpDef + BaseSpeed);
                return (scaledStat + 100) * (100 + 1) / 70 + 10;
            }
        }

        public int GetSpAtk(int level) {
            //return ((((BaseSpAtt / 2) + 45) * level / 100) + 5);
            return (BaseSpAtk + 100) * (level + 1) / 70 + 10;
        }

        public int GetSpAtkLimit() {
            if (BaseHP > 1) {
                int scaledStat = 1530 * BaseSpAtk / (BaseHP + BaseAtk + BaseDef + BaseSpAtk + BaseSpDef + BaseSpeed);
                return (scaledStat + 100) * (100 + 1) / 70 + 10;
            } else {
                int scaledStat = 1325 * BaseSpAtk / (BaseAtk + BaseDef + BaseSpAtk + BaseSpDef + BaseSpeed);
                return (scaledStat + 100) * (100 + 1) / 70 + 10;
            }
        }

        public int GetSpDef(int level) {
            //return (((BaseSpDef / 2) + 20) * level / 100) + 3;
            return (BaseSpDef + 100) * (level + 1) / 70 + 10;
        }

        public int GetSpDefLimit() {
            if (BaseHP > 1) {
                int scaledStat = 1530 * BaseSpDef / (BaseHP + BaseAtk + BaseDef + BaseSpAtk + BaseSpDef + BaseSpeed);
                return (scaledStat + 100) * (100 + 1) / 70 + 10;
            } else {
                int scaledStat = 1325 * BaseSpDef / (BaseAtk + BaseDef + BaseSpAtk + BaseSpDef + BaseSpeed);
                return (scaledStat + 100) * (100 + 1) / 70 + 10;
            }
        }

        public int GetSpeed(int level) {
            //return (((BaseSpd / 2 + 40) * level / 100) + 5);
            return (BaseSpeed + 100) * (level + 1) / 70 + 10;
        }

        public int GetSpeedLimit() {
            if (BaseHP > 1) {
                int scaledStat = 1530 * BaseSpeed / (BaseHP + BaseAtk + BaseDef + BaseSpAtk + BaseSpDef + BaseSpeed);
                return (scaledStat + 100) * (100 + 1) / 70 + 10;
            } else {
                int scaledStat = 1325 * BaseSpeed / (BaseAtk + BaseDef + BaseSpAtk + BaseSpDef + BaseSpeed);
                return (scaledStat + 100) * (100 + 1) / 70 + 10;
            }
        }

        public int GetRewardExp(int level) {
            return (((ExpYield * 3 / 5) * (level - 1) / 10) + (ExpYield * 3 / 5)) * 1;
        }

        public bool HasLevelUpMove(int level) {
            for (int i = 0; i < LevelUpMoves.Count; i++) {
                if (LevelUpMoves[i].Level == level) {
                    return true;
                }
            }
            return false;
        }

        //public Enums.Sex GenerateLegalSex() {
        //    if (MaleRatio + FemaleRatio <= 0) {
        //        return Enums.Sex.Genderless;
        //    } else if (Server.Math.Rand(0, MaleRatio + FemaleRatio) < MaleRatio) {
        //        return Enums.Sex.Male;
        //    } else {
        //        return Enums.Sex.Female;
        //    }
        //}

        public void Load(int ID, int formNum) {
            FormIndex = formNum;
            using (XmlReader reader = XmlReader.Create(Paths.DataPath + "Dex\\" + ID.ToString() + "-" + FormIndex + ".xml")) {
                while (reader.Read()) {
                    if (reader.IsStartElement()) {
                        switch (reader.Name) {
                            #region Basic Data
                            case "FormName": {
                                    FormName = reader.ReadString();
                                }
                                break;
                            case "Height": {
                                    Height = reader.ReadString().ToDbl();
                                }
                                break;
                            case "Weight": {
                                    Weight = reader.ReadString().ToDbl();
                                }
                                break;
                            case "Male": {
                                    MaleRatio = reader.ReadString().ToInt();
                                }
                                break;
                            case "Female": {
                                    FemaleRatio = reader.ReadString().ToInt();
                                }
                                break;
                            case "RecruitRate": {
                                    RecruitRate = reader.ReadString().ToInt();
                                }
                                break;
                            case "BodyStyle": {
                                    string style = reader.ReadString();
                                    if (style.IsEnum<Enums.BodyShape>())
                                        BodyStyle = style.ToEnum<Enums.BodyShape>();
                                }
                                break;
                            #endregion
                            #region Stats
                            case "HP": {
                                    BaseHP = reader.ReadString().ToInt();
                                }
                                break;
                            case "Atk": {
                                    BaseAtk = reader.ReadString().ToInt();
                                }
                                break;
                            case "Def": {
                                    BaseDef = reader.ReadString().ToInt();
                                }
                                break;
                            case "SpAtk": {
                                    BaseSpAtk = reader.ReadString().ToInt();
                                }
                                break;
                            case "SpDef": {
                                    BaseSpDef = reader.ReadString().ToInt();
                                }
                                break;
                            case "Speed": {
                                    BaseSpeed = reader.ReadString().ToInt();
                                }
                                break;
                            #endregion
                            #region Pokemon Info
                            case "Type1": {
                                    Type1 = reader.ReadString().ToEnum<Enums.Element>();
                                }
                                break;
                            case "Type2": {
                                    Type2 = reader.ReadString().ToEnum<Enums.Element>();
                                }
                                break;
                            case "Ability1": {
                                    Ability1 = reader.ReadString();
                                }
                                break;
                            case "Ability2": {
                                    Ability2 = reader.ReadString();
                                }
                                break;
                            case "Ability3": {
                                    Ability3 = reader.ReadString();
                                }
                                break;
                            case "Exp": {
                                    ExpYield = reader.ReadString().ToInt();
                                }
                                break;
                            #endregion
                            #region Moves
                            case "LevelUpMove": {
                                    if (reader.Read()) {
                                        int level = reader.ReadElementString("Level").ToInt(-1);
                                        int move = GameData.GetMove(reader.ReadElementString("Name"));
                                        if (level > -1 && move > -1) {
                                            LevelUpMoves.Add(new LevelUpMove(move, level));
                                        }
                                    }
                                }
                                break;
                            case "TMMove": {
                                    if (reader.Read()) {
                                        int move = GameData.GetMove(reader.ReadElementString("Name"));
                                        if (move > -1) {
                                            TMMoves.Add(move);
                                        }
                                    }
                                }
                                break;
                            case "EggMove": {
                                    if (reader.Read()) {
                                        int move = GameData.GetMove(reader.ReadElementString("Name"));
                                        if (move > -1) {
                                            EggMoves.Add(move);
                                        }
                                    }
                                }
                                break;
                            case "DWMove": {
                                    if (reader.Read()) {
                                        int move = GameData.GetMove(reader.ReadElementString("Name"));
                                        if (move > -1) {
                                            DWMoves.Add(move);
                                        }
                                    }
                                }
                                break;
                            case "TutorMove": {
                                    if (reader.Read()) {
                                        int move = GameData.GetMove(reader.ReadElementString("Name"));
                                        if (move > -1) {
                                            TutorMoves.Add(move);
                                        }
                                    }
                                }
                                break;
                            case "EventMove": {
                                    if (reader.Read()) {
                                        int move = GameData.GetMove(reader.ReadElementString("Name"));
                                        if (move > -1) {
                                            EventMoves.Add(move);
                                        }
                                    }
                                }
                                break;
                            #endregion
                        }
                    }
                }
            }
        }

        public void Save(int ID) {
            using (XmlWriter writer = XmlWriter.Create(Paths.DataPath + "Dex\\" + ID.ToString() + "-" + FormIndex + ".xml", Logger.XmlWriterSettings)) {
                writer.WriteStartDocument();
                writer.WriteStartElement("FormEntry");

                #region Basic data
                writer.WriteStartElement("General");
                writer.WriteElementString("FormName", FormName);
                writer.WriteElementString("Height", Height.ToString());
                writer.WriteElementString("Weight", Weight.ToString());
                writer.WriteElementString("Male", MaleRatio.ToString());
                writer.WriteElementString("Female", FemaleRatio.ToString());
                writer.WriteElementString("RecruitRate", RecruitRate.ToString());
                writer.WriteElementString("BodyStyle", BodyStyle.ToString());
                writer.WriteEndElement();
                #endregion
                #region Stats
                writer.WriteStartElement("BaseStats");
                writer.WriteElementString("HP", BaseHP.ToString());
                writer.WriteElementString("Atk", BaseAtk.ToString());
                writer.WriteElementString("Def", BaseDef.ToString());
                writer.WriteElementString("SpAtk", BaseSpAtk.ToString());
                writer.WriteElementString("SpDef", BaseSpDef.ToString());
                writer.WriteElementString("Speed", BaseSpeed.ToString());
                writer.WriteEndElement();
                #endregion
                #region Pokemon Info
                writer.WriteStartElement("FormInfo");
                writer.WriteElementString("Type1", Type1.ToString());
                writer.WriteElementString("Type2", Type2.ToString());
                writer.WriteElementString("Ability1", Ability1);
                writer.WriteElementString("Ability2", Ability2);
                writer.WriteElementString("Ability3", Ability3);
                writer.WriteElementString("Exp", ExpYield.ToString());
                writer.WriteEndElement();
                #endregion
                #region Moves
                writer.WriteStartElement("Moves");
                #region Level Up Moves
                writer.WriteStartElement("LevelUpMoves");
                for (int i = 0; i < LevelUpMoves.Count; i++) {
                    writer.WriteStartElement("LevelUpMove");
                    writer.WriteElementString("Level", LevelUpMoves[i].Level.ToString());
                    writer.WriteElementString("Name", GameData.MoveDex[LevelUpMoves[i].Move].Name);
                    writer.WriteEndElement();
                }
                writer.WriteEndElement();
                #endregion
                #region TM Moves
                writer.WriteStartElement("TMMoves");
                for (int i = 0; i < TMMoves.Count; i++) {
                    writer.WriteStartElement("TMMove");
                    writer.WriteElementString("Name", GameData.MoveDex[TMMoves[i]].Name);
                    writer.WriteEndElement();
                }
                writer.WriteEndElement();
                #endregion
                #region Egg Moves
                writer.WriteStartElement("EggMoves");
                for (int i = 0; i < EggMoves.Count; i++) {
                    writer.WriteStartElement("EggMove");
                    writer.WriteElementString("Name", GameData.MoveDex[EggMoves[i]].Name);
                    writer.WriteEndElement();
                }
                writer.WriteEndElement();
                #endregion
                #region DW Moves
                writer.WriteStartElement("DWMoves");
                for (int i = 0; i < DWMoves.Count; i++) {
                    writer.WriteStartElement("DWMove");
                    writer.WriteElementString("Name", GameData.MoveDex[DWMoves[i]].Name);
                    writer.WriteEndElement();
                }
                writer.WriteEndElement();
                #endregion
                #region Event Moves
                writer.WriteStartElement("EventMoves");
                for (int i = 0; i < EventMoves.Count; i++) {
                    writer.WriteStartElement("EventMove");
                    writer.WriteElementString("Name", GameData.MoveDex[EventMoves[i]].Name);
                    writer.WriteEndElement();
                }
                writer.WriteEndElement();
                #endregion
                writer.WriteEndElement();
                #endregion

                writer.WriteEndElement();
                writer.WriteEndDocument();
            }
        }



        #endregion Methods
    }
}
