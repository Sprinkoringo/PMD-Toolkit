using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PMDToolkit.Maps;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using PMDToolkit.Graphics;
using PMDToolkit.Core;

namespace PMDToolkit.Logic.Gameplay {
    public static partial class Processor {



        private static void SetStatusAilment(ActiveChar character, Enums.StatusAilment ailment) {

            switch (ailment) {
                case Enums.StatusAilment.OK: {
                        SetStatusAilment(character, ailment, 0);
                    }
                    break;
                case Enums.StatusAilment.Burn: {
                        //on fire!
                        SetStatusAilment(character, ailment, 0);
                    }
                    break;
                case Enums.StatusAilment.Freeze: {
                        //frozen!
                        SetStatusAilment(character, ailment, 1);
                    }
                    break;
                case Enums.StatusAilment.Poison: {
                        //poisoned!
                        SetStatusAilment(character, ailment, 1);
                    }
                    break;
            }
        }

        private static void SetStatusAilment(ActiveChar character, Enums.StatusAilment ailment, int counter) {
            //check status protection
            if (character.dead) return;

            bool changeStatus = (character.Status != ailment);

            switch (ailment) {
                case Enums.StatusAilment.OK: {
                        switch (character.Status) {
                            case Enums.StatusAilment.Burn: {
                                    //no longer on fire!
                                    Display.Screen.AddResult(new Results.BattleMsg("The flames on " + character.Name + " were put out!"));
                                }
                                break;
                            case Enums.StatusAilment.Freeze: {
                                    //thawed out!
                                    //TODO: break through ice
                                    Display.Screen.AddResult(new Results.BattleMsg(character.Name + " broke through the ice!"));
                                }
                                break;
                            case Enums.StatusAilment.Poison: {
                                    //recovered from poisoning
                                    Display.Screen.AddResult(new Results.BattleMsg(character.Name + " recovered from poisoning!"));
                                }
                                break;
                        }
                        character.Status = Enums.StatusAilment.OK;
                        character.StatusCounter = 0;

                    }
                    break;
                case Enums.StatusAilment.Burn: {
                        //on fire!
                        Display.Screen.AddResult(new Results.BattleMsg(character.Name + " is on fire!"));
                        character.Status = Enums.StatusAilment.Burn;
                        character.StatusCounter = counter;
                    }
                    break;
                case Enums.StatusAilment.Freeze: {
                        //frozen!
                        Display.Screen.AddResult(new Results.BattleMsg(character.Name + " is frozen solid!"));
                        character.Status = Enums.StatusAilment.Freeze;
                        character.StatusCounter = counter;
                    }
                    break;
                case Enums.StatusAilment.Poison: {
                        //poisoned!
                        Display.Screen.AddResult(new Results.BattleMsg(character.Name + " is poisoned!"));
                        character.Status = Enums.StatusAilment.Poison;
                        character.StatusCounter = counter;
                    }
                    break;
            }
            if (changeStatus) {
                Display.Screen.AddResult(new Results.SetStatus(CharIndex(character), character.Status));
            }
        }

        public static bool CheckStatusProtection(ActiveChar character, string status, bool msg) {
            return CheckStatusProtection(character, null, status, msg);
        }

        public static bool CheckStatusProtection(ActiveChar character, ActiveChar attacker, string status, bool msg) {
            return CheckStatusProtection(character, attacker, status, 0, new List<int>(), msg);
        }

        public static bool CheckStatusProtection(ActiveChar character, ActiveChar attacker, string status, int counter, List<int> tag, bool msg) {
            return false;
        }

        public static void AddExtraStatus(ActiveChar character, string name, int counter, int target, params int[] tag) {
            List<int> args = new List<int>(tag);
            AddExtraStatus(character, null, name, counter, target, args, true);
        }
        public static void AddExtraStatus(ActiveChar character, string name, int counter, int target, List<int> tag, bool msg) {
            AddExtraStatus(character, null, name, counter, target, tag, msg);
        }

        public static void AddExtraStatus(ActiveChar character, ActiveChar attacker, string name, int counter, int target, List<int> tag, bool msg) {
            
            if (CheckStatusProtection(character, attacker, name, counter, tag, true)) {
                return;
            }


            if (character.VolatileStatus.ContainsKey(name) &&
                name == "MovementSpeed") {
                RemoveExtraStatus(character, name, false);
            }

            if (!character.VolatileStatus.ContainsKey(name)) {
                ExtraStatus newStatus = new ExtraStatus();
                newStatus.Name = name;
                newStatus.Emoticon = -1;
                newStatus.Counter = counter;
                newStatus.Target = target;
                newStatus.Args = tag;
                character.VolatileStatus.Add(name, newStatus);

                //announce to map

                switch (name) {
                    case "MovementSpeed": {
                        int oldSpeed = character.MovementSpeed;
                            RefreshCharacterSpeed(character);
                            ExtraStatus addedStatus = newStatus;
                            switch (character.MovementSpeed) {
                                case -3: {
                                        Display.Screen.AddResult(new Results.BattleMsg(character.Name + " is at quarter speed!"));
                                    }
                                    break;
                                case -2: {
                                        Display.Screen.AddResult(new Results.BattleMsg(character.Name + " is at one-third speed!"));
                                    }
                                    break;
                                case -1: {
                                        Display.Screen.AddResult(new Results.BattleMsg(character.Name + " is at half speed!"));
                                    }
                                    break;
                                case 0: {
                                        Display.Screen.AddResult(new Results.BattleMsg(character.Name + " is at normal speed!"));
                                        RemoveExtraStatus(character, name, false);
                                    }
                                    break;
                                case 1: {
                                        Display.Screen.AddResult(new Results.BattleMsg(character.Name + " is at double speed!"));
                                    }
                                    break;
                                case 2: {
                                        Display.Screen.AddResult(new Results.BattleMsg(character.Name + " is at triple speed!"));
                                    }
                                    break;
                                case 3: {
                                        Display.Screen.AddResult(new Results.BattleMsg(character.Name + " is at quadruple speed!"));
                                    }
                                    break;
                            }
                        }
                        break;
                }

            }

        }

        public static void RemoveExtraStatus(ActiveChar character, string name) {
            RemoveExtraStatus(character, name, true);
        }

        public static void RemoveExtraStatus(ActiveChar character, string name, bool sendInfo) {
            if (!character.VolatileStatus.ContainsKey(name)) return;
            
            ExtraStatus statusToRemove = character.VolatileStatus[name];

            if (statusToRemove != null) {

                bool forme = false, sprite = false, type = false, ability = false, atkSpeed = false,
                    confusion = false, speedLimit = false, mobility = false, visibility = false,
                    darkness = false, swapLock = false, moves = false, extraStatus = false;

                //switch (name) {
                //    case "Confusion": {
                //            //if (sendInfo) hitlist.AddPacketToMap(map, PacketBuilder.CreateBattleMsg(character.Name + " is no longer confused!"));
                //            confusion = true;
                //            extraStatus = true;
                //        }
                //        break;
                //}

                character.VolatileStatus.Remove(name);

                RefreshCharacterTraits(character, forme, sprite, type, ability, atkSpeed, confusion, speedLimit, mobility, visibility, darkness, swapLock, moves, extraStatus);

            }

        }


        public static void RefreshCharacterTraits(ActiveChar character) {
            RefreshCharacterTraits(character, true, true, true, true, true, true, true, true, true, true, true, true, true);
        }

        public static void RefreshCharacterTraits(ActiveChar character, bool forme, bool sprite, bool type, bool ability,
            bool atkSpeed, bool confusion, bool speedLimit, bool mobility, bool visibility, bool darkness, bool swapLock, bool moves, bool extraStatus) {

            if (forme) {
                RefreshCharacterForme(character);
                sprite = true;
                type = true;
                ability = true;
            }

            if (sprite) {
                RefreshCharacterSprite(character);
            }

            if (type) {
                RefreshCharacterType(character);
                atkSpeed = true;
                speedLimit = true;
                mobility = true;
            }

            if (ability) {
                RefreshCharacterAbility(character);
                atkSpeed = true;
                confusion = true;
                speedLimit = true;
                mobility = true;
                darkness = true;
            }

            RefreshCharacterAttackSpeed(character);

            RefreshCharacterConfusion(character);

            RefreshCharacterSpeed(character);

            RefreshCharacterMobility(character);

            RefreshCharacterVisibility(character);

            RefreshCharacterDarkness(character);

            RefreshCharacterMoves(character);

            RefreshCharacterVolatileStatus(character);

        }

        public static void RefreshCharacterForme(ActiveChar character) {

        }

        public static void RefreshCharacterSprite(ActiveChar character) {

        }

        public static void RefreshCharacterType(ActiveChar character) {

        }

        public static void RefreshCharacterAbility(ActiveChar character) {
            
        }

        public static void RefreshCharacterAttackSpeed(ActiveChar character) {
            
        }

        public static void RefreshCharacterConfusion(ActiveChar character) {
            
        }

        public static void RefreshCharacterSpeed(ActiveChar character) {
            //Movement Speed

            int speed = 0;

            if (character.VolatileStatus.ContainsKey("MovementSpeed")) {

                speed += character.VolatileStatus["MovementSpeed"].Args[0];
            }

            character.MovementSpeed = speed;
            Display.Screen.AddResult(new Results.SetMovementSpeed(CharIndex(character), character.MovementSpeed));
        }

        public static void RefreshCharacterMobility(ActiveChar character) {
            //Mobility

        }

        public static void RefreshCharacterVisibility(ActiveChar character) {
            //invisibility
        }

        public static void RefreshCharacterDarkness(ActiveChar character) {
            
        }
        
        public static void RefreshCharacterMoves(ActiveChar character) {
            
        }

        public static void RefreshCharacterVolatileStatus(ActiveChar character) {
            //volatile status
            //if (character.CharacterType == Enums.CharacterType.Recruit) {
            //    PacketBuilder.AppendVolatileStatus(((Recruit)character).Owner, hitlist);
            //} else if (character.CharacterType == Enums.CharacterType.MapNpc) {
            //    if (map != null) {
            //        PacketBuilder.AppendNpcVolatileStatus(map, hitlist, ((MapNpc)character).MapSlot);
            //    }
            //}
        }

    }
}
