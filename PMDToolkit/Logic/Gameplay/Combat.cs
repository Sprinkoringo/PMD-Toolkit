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


        public static Enums.Alignment GetMatchup(ActiveChar attacker, ActiveChar defender) {
            if (attacker == null) return Enums.Alignment.Foe;
            if (defender == null) return Enums.Alignment.Foe;
            if (attacker is Player && ((Player)attacker).dead) return Enums.Alignment.Foe;
            if (defender is Player && ((Player)defender).dead) return Enums.Alignment.None;
            if (attacker == defender) {
                return Enums.Alignment.Self;
            }
            if ((attacker is Player) == (defender is Player)) {
                return Enums.Alignment.Friend;
            } else {
                return Enums.Alignment.Foe;
            }
        }

        public static bool IsTargeted(ActiveChar attacker, ActiveChar defender, bool hitSelf, bool hitFriend, bool hitFoe) {
            if (defender.dead) return false;
            if (defender is Player && Intangible) return false;
            Enums.Alignment alignment = GetMatchup(attacker, defender);
            switch (alignment) {
                case Enums.Alignment.Self: {
                        return hitSelf;
                    }
                    break;
                case Enums.Alignment.Friend: {
                        return hitFriend;
                    }
                    break;
                case Enums.Alignment.Foe: {
                        return hitFoe;
                    }
                    break;
            }
            return false;
        }

        public static bool IsInAreaRange(int range, Loc2D loc1, Loc2D loc2) {
            int DistanceX = System.Math.Abs(loc1.X - loc2.X);
            int DistanceY = System.Math.Abs(loc1.Y - loc2.Y);

            // Are they in range?
            if (DistanceX <= range && DistanceY <= range) {
                return true;
            } else {
                return false;
            }
        }

        public static bool IsInFront(int range, Direction8 userDir, Loc2D userLoc, Loc2D targetLoc) {
            for (int i = 0; i < range; i++) {
                if (userLoc == targetLoc) return true;
                Operations.MoveInDirection8(ref userLoc, userDir, 1);
            }
            if (userLoc == targetLoc) return true;
            return false;
        }

        public static bool StopsAtFirst(Enums.RangeType rangeType) {
            if (rangeType == Enums.RangeType.FlyInArc || rangeType == Enums.RangeType.FrontUntil) {
                return true;
            } else {
                return false;
            }
        }


        public static List<Loc2D> GetExclusiveTiles(Loc2D startLoc, Direction8 direction, Enums.RangeType rangeType, int distance) {
            List<Loc2D> returnList = new List<Loc2D>();

            switch (rangeType) {
                case Enums.RangeType.Front:
                case Enums.RangeType.FrontUntil:
                case Enums.RangeType.Room: {//room
                        #region Room
                        for (int x = startLoc.X - distance; x <= startLoc.X + distance; x++) {
                            for (int y = startLoc.Y - distance; y <= startLoc.Y + distance; y++) {
                                if (x == startLoc.X - distance || x == startLoc.X + distance ||
                                    y == startLoc.Y - distance || y == startLoc.Y + distance) {
                                    if (Operations.IsInBound(CurrentMap.Width, CurrentMap.Height, x, y))
                                        returnList.Add(new Loc2D(x, y));
                                }
                            }
                        }
                        #endregion
                    }
                    break;
                case Enums.RangeType.FrontAndSides:
                case Enums.RangeType.FlyInArc: {//flies-in-arc
                        #region Flies-in-Arc
                        if (Operations.IsDiagonal(direction)) {
                            Loc2D endLoc = startLoc;
                            Loc2D diffLoc = startLoc;
                            Operations.MoveInDirection8(ref diffLoc, direction, 1);
                            diffLoc = diffLoc - startLoc;
                            Operations.MoveInDirection8(ref endLoc, direction, distance);

                            for (int x = startLoc.X; x != endLoc.X + diffLoc.X; x += diffLoc.X) {
                                for (int y = startLoc.Y; y != endLoc.Y + diffLoc.Y; y += diffLoc.Y) {
                                    if (x == endLoc.X || y == endLoc.Y) {
                                        if (Operations.IsInBound(CurrentMap.Width, CurrentMap.Height, x, y))
                                            returnList.Add(new Loc2D(x, y));
                                    }
                                }
                            }
                        } else {
                            Loc2D endLoc = startLoc;
                            Operations.MoveInDirection8(ref endLoc, direction, distance);


                            if (Operations.IsInBound(CurrentMap.Width, CurrentMap.Height, endLoc.X, endLoc.Y))
                                returnList.Add(new Loc2D(endLoc.X, endLoc.Y));

                            Direction8 left = Operations.AddDir(direction, Direction8.Right);
                            Direction8 right = Operations.AddDir(direction, Direction8.Left);

                            for (int i = 0; i < distance; i++) {
                                Operations.MoveInDirection8(ref endLoc, left, i * 2 + 1);
                                if (Operations.IsInBound(CurrentMap.Width, CurrentMap.Height, endLoc.X, endLoc.Y))
                                    returnList.Add(new Loc2D(endLoc.X, endLoc.Y));
                                Operations.MoveInDirection8(ref endLoc, right, (i + 1) * 2);
                                if (Operations.IsInBound(CurrentMap.Width, CurrentMap.Height, endLoc.X, endLoc.Y))
                                    returnList.Add(new Loc2D(endLoc.X, endLoc.Y));
                            }
                        }
                        #endregion
                    }
                    break;
            }

            return returnList;
        }

        public static TargetCollection SimulateAttackRange(ActiveChar user, Loc2D userLoc, Direction8 userDir, Data.MoveRange range)
        {
            return GetTargetsInRange(user, userLoc, userDir, range, true);
        }

        public static TargetCollection GetTargetsInRange(ActiveChar user, Loc2D userLoc, Direction8 userDir, Data.MoveRange range)
        {
            return GetTargetsInRange(user, userLoc, userDir, range, false);
        }

        public static TargetCollection GetTargetsInRange(ActiveChar user, Loc2D userLoc, Direction8 userDir, Data.MoveRange range, bool blockOff) {
            TargetCollection targetlist = new TargetCollection();

            switch (range.RangeType) {
                case Enums.RangeType.Front: {//foe in front
                        #region FrontOfUser
                        Loc2D targetLoc = userLoc;
                        for (int r = 0; r <= range.Distance; r++) {
                            for (int i = 0; i < MAX_TEAM_SLOTS; i++) {
                                if (IsTargeted(user, Players[i], range.HitsSelf, range.HitsFriend, range.HitsFoe) && targetLoc == Players[i].CharLoc) {
                                    targetlist.Add(new Target(Players[i], GetMatchup(user, Players[i]), r));
                                }
                            }
                            for (int i = 0; i < BasicMap.MAX_NPC_SLOTS; i++) {
                                if (IsTargeted(user, Npcs[i], range.HitsSelf, range.HitsFriend, range.HitsFoe) && targetLoc == Npcs[i].CharLoc) {
                                    targetlist.Add(new Target(Npcs[i], GetMatchup(user, Npcs[i]), r));
                                }
                            }
                            List<Loc2D> tileLocs = GetExclusiveTiles(userLoc, userDir, range.RangeType, r);
                            bool blockFound = false;
                            foreach (Loc2D loc in tileLocs) {
                                targetlist.Add(new TileTarget(loc, r));
                                if (TileBlocked(loc, Enums.WalkMode.Air))
                                    blockFound = true;
                            }
                            if (blockFound)
                            {
                                if (blockOff)
                                    range.Distance = r;
                                break;
                            }
                            Operations.MoveInDirection8(ref targetLoc, userDir, 1);
                        }
                        #endregion
                    }
                    break;
                case Enums.RangeType.FrontUntil: {//foe in front until
                    #region FrontOfUserUntil
                        Loc2D targetLoc = userLoc;
                        bool stopattile = false;
                        for (int r = 0; r <= range.Distance; r++) {
                            for (int i = 0; i < MAX_TEAM_SLOTS; i++) {
                                if (IsTargeted(user, Players[i], range.HitsSelf, range.HitsFriend, range.HitsFoe) && targetLoc == Players[i].CharLoc) {
                                    targetlist.Add(new Target(Players[i], GetMatchup(user, Players[i]), r));
                                    stopattile = true;
                                }
                            }
                            for (int i = 0; i < BasicMap.MAX_NPC_SLOTS; i++) {
                                if (IsTargeted(user, Npcs[i], range.HitsSelf, range.HitsFriend, range.HitsFoe) && targetLoc == Npcs[i].CharLoc) {
                                    targetlist.Add(new Target(Npcs[i], GetMatchup(user, Npcs[i]), r));
                                    stopattile = true;
                                }
                            }

                            List<Loc2D> tileLocs = GetExclusiveTiles(userLoc, userDir, range.RangeType, r);
                            bool blockFound = false;
                            foreach (Loc2D loc in tileLocs)
                            {
                                targetlist.Add(new TileTarget(loc, r));
                                if (TileBlocked(loc, Enums.WalkMode.Air))
                                    blockFound = true;
                            }
                            if (blockFound)
                            {
                                if (blockOff)
                                    range.Distance = r;
                                break;
                            }
                            Operations.MoveInDirection8(ref targetLoc, userDir, 1);
                        }

                        #endregion
                    }
                    break;
                case Enums.RangeType.Room: {//room
                        #region Room
                    for (int r = 0; r <= range.Distance; r++) {
                            for (int i = 0; i < MAX_TEAM_SLOTS; i++) {
                                if (IsTargeted(user, Players[i], range.HitsSelf, range.HitsFriend, range.HitsFoe) && IsInAreaRange(r, userLoc, Players[i].CharLoc)
                                    && (r == 0 || !IsInAreaRange(r - 1, userLoc, Players[i].CharLoc))) {
                                    targetlist.Add(new Target(Players[i], GetMatchup(user, Players[i]), r));
                                }
                            }
                            
                            for (int i = 0; i < BasicMap.MAX_NPC_SLOTS; i++) {
                                if (IsTargeted(user, Npcs[i], range.HitsSelf, range.HitsFriend, range.HitsFoe) && IsInAreaRange(r, userLoc, Npcs[i].CharLoc)
                                && (r == 0 || !IsInAreaRange(r - 1, userLoc, Npcs[i].CharLoc))) {
                                    targetlist.Add(new Target(Npcs[i], GetMatchup(user, Npcs[i]), r));
                                }
                            }

                            List<Loc2D> tileLocs = GetExclusiveTiles(userLoc, userDir, range.RangeType, r);
                            foreach (Loc2D loc in tileLocs) {
                                targetlist.Add(new TileTarget(loc, r));
                            }
                        }
                        #endregion
                    }
                    break;
                case Enums.RangeType.FrontAndSides: {//front and sides
                        #region Front-And-Sides
                    Loc2D targetLoc = userLoc;
                    for (int r = 0; r <= range.Distance; r++) {
                        //check directly forward
                        for (int i = 0; i < MAX_TEAM_SLOTS; i++) {
                            if (IsTargeted(user, Players[i], range.HitsSelf, range.HitsFriend, range.HitsFoe) && targetLoc == Players[i].CharLoc) {
                                targetlist.Add(new Target(Players[i], GetMatchup(user, Players[i]), r));
                            }
                        }

                            
                            for (int i = 0; i < BasicMap.MAX_NPC_SLOTS; i++) {
                                if (IsTargeted(user, Npcs[i], range.HitsSelf, range.HitsFriend, range.HitsFoe) && targetLoc == Npcs[i].CharLoc) {
                                    targetlist.Add(new Target(Npcs[i], GetMatchup(user, Npcs[i]), r));
                                }
                            }

                            Loc2D leftLoc = targetLoc;
                            Loc2D rightLoc = targetLoc;
                            for (int s = 1; s <= r; s++) {
                                if (Operations.IsDiagonal(userDir)) {
                                    Operations.MoveInDirection8(ref leftLoc, Operations.AddDir(userDir, Direction8.UpRight), 1);
                                    Operations.MoveInDirection8(ref rightLoc, Operations.AddDir(userDir, Direction8.UpLeft), 1);
                                } else {
                                    Operations.MoveInDirection8(ref leftLoc, Operations.AddDir(userDir, Direction8.Right), 1);
                                    Operations.MoveInDirection8(ref rightLoc, Operations.AddDir(userDir, Direction8.Left), 1);
                                }

                                //check sides
                                for (int i = 0; i < MAX_TEAM_SLOTS; i++) {
                                    if (IsTargeted(user, Players[i], range.HitsSelf, range.HitsFriend, range.HitsFoe) && leftLoc == Players[i].CharLoc) {
                                        targetlist.Add(new Target(Players[i], GetMatchup(user, Players[i]), r));
                                    }
                                }

                                for (int i = 0; i < BasicMap.MAX_NPC_SLOTS; i++) {
                                    if (IsTargeted(user, Npcs[i], range.HitsSelf, range.HitsFriend, range.HitsFoe) && leftLoc == Npcs[i].CharLoc) {
                                        targetlist.Add(new Target(Npcs[i], GetMatchup(user, Npcs[i]), r));
                                    }
                                }

                                for (int i = 0; i < MAX_TEAM_SLOTS; i++) {
                                    if (IsTargeted(user, Players[i], range.HitsSelf, range.HitsFriend, range.HitsFoe) && rightLoc == Players[i].CharLoc) {
                                        targetlist.Add(new Target(Players[i], GetMatchup(user, Players[i]), r));
                                    }
                                }

                                for (int i = 0; i < BasicMap.MAX_NPC_SLOTS; i++) {
                                    if (IsTargeted(user, Npcs[i], range.HitsSelf, range.HitsFriend, range.HitsFoe) && rightLoc == Npcs[i].CharLoc) {
                                        targetlist.Add(new Target(Npcs[i], GetMatchup(user, Npcs[i]), r));
                                    }
                                }
                            }

                            List<Loc2D> tileLocs = GetExclusiveTiles(userLoc, userDir, range.RangeType, r);
                            foreach (Loc2D loc in tileLocs) {
                                targetlist.Add(new TileTarget(loc, r));
                            }

                            Operations.MoveInDirection8(ref targetLoc, userDir, 1);
                        }
                        #endregion
                    }
                    break;
                case Enums.RangeType.FlyInArc: {//flies-in-arc
                        #region Flies-in-Arc
                        bool stopAtTile = false;
                        Loc2D targetLoc = userLoc;
                        for (int r = 0; r <= range.Distance; r++) {

                            //check directly forward
                            for (int i = 0; i < MAX_TEAM_SLOTS; i++) {
                                if (IsTargeted(user, Players[i], range.HitsSelf, range.HitsFriend, range.HitsFoe) && targetLoc == Players[i].CharLoc)
                                {
                                    targetlist.Add(new Target(Players[i], GetMatchup(user, Players[i]), r));
                                    stopAtTile = true;
                                }
                            }
                            for (int i = 0; i < BasicMap.MAX_NPC_SLOTS; i++) {
                                if (IsTargeted(user, Npcs[i], range.HitsSelf, range.HitsFriend, range.HitsFoe) && targetLoc == Npcs[i].CharLoc) {
                                    targetlist.Add(new Target(Npcs[i], GetMatchup(user, Npcs[i]), r));
                                    stopAtTile = true;
                                }
                            }
                            if (stopAtTile) {
                                targetlist.Add(new TileTarget(targetLoc, r));
                                break;
                            }

                            Loc2D leftLoc = targetLoc;
                            Loc2D rightLoc = targetLoc;
                            for (int s = 1; s <= r; s++) {
                                if (Operations.IsDiagonal(userDir)) {
                                    Operations.MoveInDirection8(ref leftLoc, Operations.AddDir(userDir, Direction8.UpRight), 1);
                                    Operations.MoveInDirection8(ref rightLoc, Operations.AddDir(userDir, Direction8.UpLeft), 1);
                                } else {
                                    Operations.MoveInDirection8(ref leftLoc, Operations.AddDir(userDir, Direction8.Right), 1);
                                    Operations.MoveInDirection8(ref rightLoc, Operations.AddDir(userDir, Direction8.Left), 1);
                                }

                                //check sides
                                for (int i = 0; i < MAX_TEAM_SLOTS; i++) {
                                    if (IsTargeted(user, Players[i], range.HitsSelf, range.HitsFriend, range.HitsFoe) && leftLoc == Players[i].CharLoc) {
                                        targetlist.Add(new Target(Players[i], GetMatchup(user, Players[i]), r));
                                        stopAtTile = true;
                                    }
                                }

                                for (int i = 0; i < BasicMap.MAX_NPC_SLOTS; i++) {
                                    if (IsTargeted(user, Npcs[i], range.HitsSelf, range.HitsFriend, range.HitsFoe) && leftLoc == Npcs[i].CharLoc) {
                                        targetlist.Add(new Target(Npcs[i], GetMatchup(user, Npcs[i]), r));
                                        stopAtTile = true;
                                    }
                                }

                                if (stopAtTile) {
                                    targetlist.Add(new TileTarget(leftLoc, r));
                                    break;
                                }

                                for (int i = 0; i < MAX_TEAM_SLOTS; i++) {
                                    if (IsTargeted(user, Players[i], range.HitsSelf, range.HitsFriend, range.HitsFoe) && rightLoc == Players[i].CharLoc) {
                                        targetlist.Add(new Target(Players[i], GetMatchup(user, Players[i]), r));
                                        stopAtTile = true;
                                    }
                                }

                                for (int i = 0; i < BasicMap.MAX_NPC_SLOTS; i++) {
                                    if (IsTargeted(user, Npcs[i], range.HitsSelf, range.HitsFriend, range.HitsFoe) && rightLoc == Npcs[i].CharLoc) {
                                        targetlist.Add(new Target(Npcs[i], GetMatchup(user, Npcs[i]), r));
                                        stopAtTile = true;
                                    }
                                }

                                if (stopAtTile) {
                                    targetlist.Add(new TileTarget(rightLoc, r));
                                    break;
                                }

                            }

                            if (stopAtTile) {
                                break;
                            }

                            Operations.MoveInDirection8(ref targetLoc, userDir, 1);
                        }
                        #endregion
                    }
                    break;
            }

            return targetlist;
        }


        public static void Hop(ActiveChar character, ref bool moveMade) {

        }

        public static void Attack(ActiveChar character, ref bool moveMade) {

            BattleSetup setup = new BattleSetup();
            setup.Attacker = character;
            setup.moveSlot = -1;
            setup.moveIndex = 0;
            HandleAttack(setup, ref moveMade);
        }
        
        public static void UseMove(ActiveChar character, int moveSlot, ref bool moveMade) {

            BattleSetup setup = new BattleSetup();
            setup.Attacker = character;
            setup.moveSlot = moveSlot;

            HandleAttack(setup, ref moveMade);
        }

        public static void HandleAttack(BattleSetup setup, ref bool moveMade) {
            //used when needing to check against ability to attack before attempt even goes through to execution

            if (!setup.Cancel) {
                BeforeMoveUsed(setup, ref moveMade);
            }

            if (!setup.Cancel) {
                ExecuteAttack(setup, ref moveMade);
            }
        }

        public static void BeforeMoveUsed(BattleSetup setup, ref bool moveMade) {
            #region Check against ability to use move
            if (setup.Attacker.dead) {
                setup.Cancel = true;
                return;
            }
            
            //this is the place to assign move and move index
            if (setup.moveSlot > -1) {
                setup.moveIndex = setup.Attacker.Moves[setup.moveSlot].MoveNum;
            }
            if (setup.moveIndex > -1) {
                setup.Move = new Data.MoveEntry(Data.GameData.MoveDex[setup.moveIndex]);
                setup.Move.Name = setup.Attacker.Name + " used " + setup.Move.Name +"!";
            }

            if (setup.Move == null) {
                //no move defined
                setup.Cancel = true;
                return;
            }
            #endregion


            if (!setup.BattleTags.ContainsKey("NoCost")) {
                if (setup.Attacker.PP < setup.Move.PP) {
                    Display.Screen.AddResult(new Results.BattleMsg("Not enough PP!"));
                    setup.Cancel = true;
                    return;
                }
            }

            moveMade = true;

            if (setup.Attacker.Status == Enums.StatusAilment.Freeze) {
                setup.Attacker.StatusCounter--;
                if (setup.Attacker.StatusCounter < 0) {
                    SetStatusAilment(setup.Attacker, Enums.StatusAilment.OK, 0);
                } else {
                    Display.Screen.AddResult(new Results.BattleMsg(setup.Attacker.Name + " is frozen..."));
                    setup.Cancel = true;
                    return;
                }
            }

            if (!setup.BattleTags.ContainsKey("NoCost")) {
                DeductPP(setup.Attacker, setup.Move.PP, false);
            }
        }

        public static void MockAnim(Display.CharSprite.ActionType anim, bool loop, bool inPlace)
        {
            Display.Screen.ForceReady();

            Results.CreateAction result = new Results.CreateAction(CharIndex(FocusedCharacter), FocusedCharacter, anim, loop, inPlace);
            Display.Screen.AddResult(result);
        }

        public static void MockAttack(Data.MoveEntry entry) {
            Display.Screen.ForceReady();

            BattleSetup setup = new BattleSetup();
            setup.Attacker = FocusedCharacter;
            setup.moveSlot = -1;
            setup.moveIndex = -1;
            setup.Move = new Data.MoveEntry(entry);
            setup.Move.Name = setup.Attacker.Name + " used " + setup.Move.Name + "!";


            ProcessStartAnim(setup);
            ProcessTravelingAnim(setup);

            //send copies over and hit each target that satisfies the category

            int charIndex = 0;
            int tileIndex = 0;
            int range = 0;
            while (charIndex < setup.AllTargets.Targets.Count || tileIndex < setup.AllTargets.Tiles.Count) {
                while (charIndex < setup.AllTargets.Targets.Count && setup.AllTargets.Targets[charIndex].Distance <= range) {
                    Target target = setup.AllTargets.Targets[charIndex];
                    if (target.TargetAlignment == Enums.Alignment.Foe && setup.Move.Range.HitsFoe ||
                    target.TargetAlignment == Enums.Alignment.Friend && setup.Move.Range.HitsFriend) {
                        setup.Defender = target.Character;
                        ProcessDefenderAnim(setup);
                    }
                    charIndex++;
                }
                while (tileIndex < setup.AllTargets.Tiles.Count && setup.AllTargets.Tiles[tileIndex].Distance <= range) {
                    TileTarget target = setup.AllTargets.Tiles[tileIndex];
                    setup.DefenderTile = target.TileLoc;
                    //MoveHitTile(setup);
                    
                    tileIndex++;
                }
                range++;
            }

            //always hit self last
            if (setup.Move.Range.HitsSelf) {
                foreach (Target target in setup.AllTargets.Targets) {
                    if (target.TargetAlignment == Enums.Alignment.Self) {
                        setup.Defender = target.Character;
                        ProcessDefenderAnim(setup);
                    }
                }
            }
        }

        public static void ExecuteAttack(BattleSetup setup, ref bool moveMade) {
            //invoked when attack is used without pre-checks
                        
            moveMade = true;

            AfterMoveExecuted(setup);
            
            //when we find out we're targeting someone, that's when we know we should separate this move from the concurrents
            bool attackEach = false; // set this to false to see all attack hits in sequence

            //send copies over and hit each target that satisfies the category

            int charIndex = 0;
            int tileIndex = 0;
            int range = 0;
            while (charIndex < setup.AllTargets.Targets.Count || tileIndex < setup.AllTargets.Tiles.Count) {
                //hit each character
                while (charIndex < setup.AllTargets.Targets.Count && setup.AllTargets.Targets[charIndex].Distance <= range) {
                    Target target = setup.AllTargets.Targets[charIndex];
                    if (target.TargetAlignment == Enums.Alignment.Foe && setup.Move.Range.HitsFoe ||
                    target.TargetAlignment == Enums.Alignment.Friend && setup.Move.Range.HitsFriend) {
                        setup.Defender = target.Character;
                        if (attackEach)
                        {
                            Display.Screen.SwitchConcurrentBranch();
                            int totalWait = setup.TimeForHit.Value;
                            if (setup.TotalWaves > 0)
                                totalWait += setup.TotalWaveTime * target.Distance / setup.TotalWaves;
                            Display.Screen.AddResult(new Results.Wait(RenderTime.FromMillisecs(totalWait)));
                        }
                        MoveHitCharacter(setup);
                        if (setup.Cancel) {
                            if (attackEach)
                                Display.Screen.BeginConcurrent();
                            return;
                        }
                    }
                    charIndex++;
                }

                //hit each tile
                while (tileIndex < setup.AllTargets.Tiles.Count && setup.AllTargets.Tiles[tileIndex].Distance <= range) {
                    TileTarget target = setup.AllTargets.Tiles[tileIndex];
                    setup.DefenderTile = target.TileLoc;
                    if (attackEach)
                    {
                        Display.Screen.SwitchConcurrentBranch();
                        int totalWait = setup.TimeForHit.Value;
                        if (setup.TotalWaves > 0)
                            totalWait += setup.TotalWaveTime * target.Distance / setup.TotalWaves;
                        Display.Screen.AddResult(new Results.Wait(RenderTime.FromMillisecs(totalWait)));
                    }
                    MoveHitTile(setup);
                    if (setup.Cancel)
                    {
                        if (attackEach)
                            Display.Screen.BeginConcurrent();
                        return;
                    }
                    tileIndex++;
                }
                range++;
            }

            //always hit self last
            if (setup.Move.Range.HitsSelf) {
                foreach (Target target in setup.AllTargets.Targets) {
                    if (target.TargetAlignment == Enums.Alignment.Self) {
                        setup.Defender = target.Character;
                        if (attackEach)
                        {
                            Display.Screen.SwitchConcurrentBranch();
                            int totalWait = setup.TimeForHit.Value;
                            if (setup.TotalWaves > 0)
                                totalWait += setup.TotalWaveTime * target.Distance / setup.TotalWaves;
                            Display.Screen.AddResult(new Results.Wait(RenderTime.FromMillisecs(totalWait)));
                        }
                        MoveHitCharacter(setup);
                        if (setup.Cancel)
                        {
                            if (attackEach)
                                Display.Screen.BeginConcurrent();
                            return;
                        }
                    }
                }
            }


            if (!setup.Cancel) {
                AfterActionTaken(setup);
            }
        }

        public static void MoveHitCharacter(BattleSetup userSetup) {
            BattleSetup setup = new BattleSetup(userSetup);

            BeforeMoveHits(setup);

            if (!setup.Cancel) {
                //does not need per-player effect; case statements can be made outside of this method as well
                AfterMoveHit(setup);
            }
        }

        public static void MoveHitTile(BattleSetup userSetup) {
            BattleSetup setup = new BattleSetup(userSetup);

            BeforeMoveHitsTile(setup);

            if (!setup.Cancel) {
                //does not need per-player effect; case statements can be made outside of this method as well
                AfterMoveHitTile(setup);
            }
        }

        public static void AfterMoveExecuted(BattleSetup setup) {
            
            ProcessStartAnim(setup);

            #region Attack Modifiers


            #endregion


            ProcessTravelingAnim(setup);

        }

        public static void BeforeMoveHits(BattleSetup setup) {

            #region Defender Modifiers


            #endregion


            if (!setup.Defender.dead) {
                
                ProcessDefenderAnim(setup);
            }
        }

        public static void BeforeMoveHitsTile(BattleSetup setup) {

        }

        public static void AfterMoveHit(BattleSetup setup) {

            if (setup.Move.Power > 0 && !setup.Defender.dead) {
                DamageCharacter(setup.Defender, setup.Move.Power * setup.Multiplier / 1000 + setup.Attacker.Atk);
            }
            if (setup.Cancel)
                return;

            #region MoveEffects
            switch (setup.Move.Effect) {
                case 1: {//no effect currently
                    }
                    break;
                case 2: {//jump

                    
                    }
                    break;
                case 3: {//chance of causing special status to target
                        if (Rand.Next() % 100 < setup.Move.Effect2 && !setup.Defender.dead) {
                            //if not immune to status
                            SetStatusAilment(setup.Defender, (Enums.StatusAilment)setup.Move.Effect1);
                        }
                    }
                    break;
                case 4: {//item throw
                    ProcessItemUse(setup.Defender, setup.Move.Effect1);
                    }
                    break;
                case 5: {//exploding projectile
                        TriggerExplosion(setup.Defender.CharLoc, 2);

                    }
                    break;
                case 6: {//knockback
                        if (!setup.Defender.dead) {
                            Knockback(setup.Defender, setup.Attacker.CharDir);
                            SetStatusAilment(setup.Defender, Enums.StatusAilment.OK, 0);
                        }
                    }
                    break;
            }
            #endregion

            #region Ability Effects



            #endregion
        }


        public static void AfterMoveHitTile(BattleSetup setup) {
            if (setup.Move.Power > 0) {
                DamageTile(setup.DefenderTile.Value);
            }
        }

        public static void AfterActionTaken(BattleSetup setup) {
            #region Move End Effects
            switch (setup.Move.Effect) {
                case 1: {//no effect currently
                    }
                    break;
                case 2: {//jump


                        if (setup.AllTargets.Targets.Count == 0) {
                            //check tile targets for presence of blockers

                            if (!WasAttackBlockedByObstacle(setup))
                            {
                                //otherwise, stay at the place landed
                                Operations.MoveInDirection8(ref setup.Attacker.CharLoc, setup.Attacker.CharDir, setup.Move.Range.Distance);

                                Display.Screen.AddResult(new Results.Loc(CharIndex(setup.Attacker), setup.Attacker.CharLoc));
                            }
                        }
                    }
                    break;
                case 3: {//chance of causing special status to target
                    }
                    break;
                case 4: {//item throw dropped
                    
                        if (setup.AllTargets.Targets.Count == 0) {
                            //place item on ground
                            Loc2D endLoc = (Loc2D)setup.GetBattleTag("EndLoc");
                            Loc2D? dropLoc = FindFreeTile(endLoc, 3);
                            if (dropLoc.HasValue) {
                                int itemIndex = setup.Move.Effect1;

                                if (setup.Move.MidAnim.AnimType == Display.MoveAnimationType.Throw || setup.Move.MidAnim.AnimType == Display.MoveAnimationType.ItemThrow)
                                {
                                    Display.Screen.AddResult(new Results.CreateAnim(
                                        new Display.ItemAnim(endLoc, dropLoc.Value, Data.GameData.ItemDex[itemIndex].Sprite, Display.ItemAnim.ItemAnimType.Bounce), Display.Screen.EffectPriority.None));
                                }
                                else if (WasAttackBlockedByObstacle(setup))
                                {
                                    Display.Screen.AddResult(new Results.CreateAnim(
                                        new Display.ItemAnim(endLoc, dropLoc.Value, Data.GameData.ItemDex[itemIndex].Sprite, Display.ItemAnim.ItemAnimType.Deflect), Display.Screen.EffectPriority.None));
                                }
                                else
                                {
                                    Display.Screen.AddResult(new Results.CreateAnim(
                                        new Display.ItemAnim(endLoc, dropLoc.Value, Data.GameData.ItemDex[itemIndex].Sprite, Display.ItemAnim.ItemAnimType.Drop), Display.Screen.EffectPriority.None));
                                }

                                int index = CurrentMap.AddItem(new Item(itemIndex, 1, "", false, dropLoc.Value));

                                Display.Screen.AddResult(new Results.AddItem(CurrentMap, index));
                            }
                        }
                    }
                    break;
                case 5: {//exploding projectile
                        //explode at end (if no one was hit)
                        if (setup.AllTargets.Targets.Count == 0) {
                            //explode at the end
                            Loc2D endLoc = (Loc2D)setup.GetBattleTag("EndLoc");
                            TriggerExplosion(endLoc, 2);
                        }
                    }
                    break;
            }
            #endregion
        }

        private static bool WasAttackBlockedByObstacle(BattleSetup setup)
        {
            bool blocked = false;
            if (setup.AllTargets.Tiles.Count > 0)
            {
                foreach (TileTarget target in setup.AllTargets.Tiles)
                {
                    if (IsBlocked(target.TileLoc, Enums.WalkMode.Air))
                    {
                        blocked = true;
                        break;
                    }
                }
            }
            return blocked;
        }

        private static void ProcessItemUse(ActiveChar character, int invSlot, ref bool moveMade) {
            if (!CanUse(invSlot)) {
                Display.Screen.AddResult(new Results.BattleMsg("Can't use slot " + (invSlot + 1)));
                return;
            }
            moveMade = true;
            Display.Screen.AddResult(new Results.BattleMsg(character.Name + " ate the " + Data.GameData.ItemDex[Inventory[invSlot]].Name + "."));
            Display.Screen.AddResult(new Results.SE("magic143"));
            Display.Screen.AddResult(new Results.CreateAction(CharIndex(character), character, Display.CharSprite.ActionType.Item));

            ProcessItemUse(character, Inventory[invSlot]);

            Inventory[invSlot] = -1;
        }

        private static void ProcessItemUse(ActiveChar character, int itemNum) {
            Data.ItemEntry item = Data.GameData.ItemDex[itemNum];
            switch (item.Effect) {
                case 1: {//restore HP
                    RestoreHP(character, item.Effect1);
                    }
                    break;
                case 2: {//restore PP
                        RestorePP(character, item.Effect1);
                    }
                    break;
                case 3: {//Removes status
                        //if the player is not immune to OK status,
                    SetStatusAilment(character, Enums.StatusAilment.OK, 0);
                    }
                    break;
                case 4: {//explode
                        TriggerExplosion(character.CharLoc, 2);
                    }
                    break;
                case 5: {//Speed-up
                        AddExtraStatus(character, "MovementSpeed", 5, -1, 1);
                    }
                    break;
            }
        }

        public static void TriggerExplosion(Loc2D origin, int distance) {
            Display.Screen.AddResult(new Results.SE("magic215"));
            TargetCollection targets = SimulateAttackRange(null, origin, Direction8.None, new Data.MoveRange(Enums.RangeType.Room, 0, true, distance, true, true, true));

            Display.Screen.AddResult(new Results.CreateEmitter(
            new Display.TileMoveAnimation(origin, 21, RenderTime.FromMillisecs(4), 1, Enums.RangeType.Room, Direction8.None, distance, RenderTime.Zero)));

            foreach (Target target in targets.Targets) {
                DamageCharacter(target.Character, (distance+1)*2 - target.Distance*2);
            }
        }

        public static void Knockback(ActiveChar character, Direction8 dir) {
            //face direction
            character.CharDir = Operations.ReverseDir(dir);
            Display.Screen.AddResult(new Results.Dir(CharIndex(character), character.CharDir));
            //find location of blocking
            int distance = 10;
            bool blockedOff = false;
            int distanceTraveled = 0;
            for (int i = 1; i <= distance; i++) {
                if (DirBlocked(dir, character, false, i)) {
                    distanceTraveled = i;
                    blockedOff = true;
                    break;
                }
            }

            //animate knockback to that location

            //set at that location
            for (int i = 0; i < distanceTraveled; i++)
            {
                Operations.MoveInDirection8(ref character.CharLoc, dir, 1);
                Display.Screen.AddResult(new Results.CreateAction(CharIndex(character), character, Display.CharSprite.ActionType.Knockback));
                //send location
                Display.Screen.AddResult(new Results.Loc(CharIndex(character), character.CharLoc));
            }


            //if blocked off, the one sent flying hits whatever blocked
            if (blockedOff) {

                //if someone was there, hit the character for set damage
                TargetCollection targets = GetTargetsInRange(character, character.CharLoc, Direction8.None, new Data.MoveRange(Enums.RangeType.Room, 0, false, 0, false, true, true));
                foreach (Target target in targets.Targets) {
                    DamageCharacter(target.Character, 4);
                }

                //animate deflection to tile before
                Display.Screen.AddResult(new Results.CreateAction(CharIndex(character), character, Display.CharSprite.ActionType.Deflect));

                //set at that location
                Operations.MoveInDirection8(ref character.CharLoc, character.CharDir, 1);

                //send location
                Display.Screen.AddResult(new Results.Loc(CharIndex(character), character.CharLoc));

                //the one sent flying is hit for set damage as well
                DamageCharacter(character, 4);
            }

        }

        public static bool CanThrow(ActiveChar character, int invSlot) {
            if (Inventory[invSlot] == -1) return false;
            return true;
        }

        public static void ProcessThrow(ActiveChar character, int invSlot, ref bool moveMade) {

            //check against factors preventing an attack attempt
            if (!CanThrow(character, invSlot)) {
                Display.Screen.AddResult(new Results.BattleMsg("Can't throw slot " + invSlot + "!"));
                return;
            }

            int itemIndex = Inventory[invSlot];
            Data.ItemEntry item = new Data.ItemEntry(Data.GameData.ItemDex[itemIndex]);
            Inventory[invSlot] = -1;
            moveMade = true;

            switch (item.ThrowEffect) {
                case 1: {//hits for damage

                        BattleSetup setup = new BattleSetup();
                        setup.Attacker = character;
                        setup.Move = new Data.MoveEntry();
                        setup.Move.Name = character.Name + " threw the " + item.Name + "!";
                        //setup.Move.Power = 0;
                        setup.Move.Effect = 4;
                        setup.Move.Effect1 = itemIndex;
                        setup.Move.Range.HitsFoe = true;
                        setup.Move.Range.RangeType = Enums.RangeType.FrontUntil;
                        setup.Move.Range.CutsCorners = true;
                        setup.Move.Range.Distance = 10;
                        setup.Move.StartSound = 102;
                        setup.Move.MidSound = 140;
                        setup.Move.EndSound = 122;
                        setup.Move.StartUserAnim.ActionType = Display.CharSprite.ActionType.Throw;
                        setup.Move.MidAnim.AnimType = Display.MoveAnimationType.ItemArrow;
                        setup.Move.MidAnim.AnimIndex = Data.GameData.ItemDex[itemIndex].Sprite;
                        setup.Move.MidAnim.FrameLength = RenderTime.FromMillisecs(4);
                        setup.Move.MidAnim.Anim1 = 6;
                        setup.Move.EndAnim.AnimType = Logic.Display.MoveAnimationType.Normal;
                        setup.Move.EndAnim.AnimIndex = 30;
                        setup.Move.EndAnim.FrameLength = RenderTime.FromMillisecs(8);
                        setup.Move.EndAnim.Anim1 = 1;
                        
                        ExecuteAttack(setup, ref moveMade);
                    }
                    break;
                case 2: {//explosive splash damage

                        BattleSetup setup = new BattleSetup();
                        setup.Attacker = character;
                        setup.Move = new Data.MoveEntry();
                        setup.Move.Name = character.Name + " threw the " + item.Name + "!";
                        setup.Move.Effect = 5;
                        setup.Move.Effect1 = itemIndex;
                        setup.Move.Range.HitsFoe = true;
                        setup.Move.Range.RangeType = Enums.RangeType.FrontUntil;
                        setup.Move.Range.Distance = 10;
                        setup.Move.StartSound = 102;
                        setup.Move.MidSound = 140;
                        setup.Move.StartUserAnim.ActionType = Display.CharSprite.ActionType.Throw;
                        setup.Move.MidAnim.AnimType = Display.MoveAnimationType.ItemArrow;
                        setup.Move.MidAnim.AnimIndex = Data.GameData.ItemDex[itemIndex].Sprite;
                        setup.Move.MidAnim.FrameLength = RenderTime.FromMillisecs(4);
                        setup.Move.MidAnim.Anim1 = 6;

                        ExecuteAttack(setup, ref moveMade);
                    }
                    break;
            }
        }

        public static Loc2D? FindFreeTile(Loc2D origin, int range) {
            if (CanItemLand(origin))
                return origin;

            Loc2D downLoc = origin;
            Loc2D leftLoc = origin;
            Loc2D upLoc = origin;
            Loc2D rightLoc = origin;

            for (int i = 1; i <= range; i++) {
                //check direct side
                Operations.MoveInDirection8(ref downLoc, Direction8.Down, 1);
                if (CanItemLand(downLoc))
                    return downLoc;
                Operations.MoveInDirection8(ref leftLoc, Direction8.Left, 1);
                if (CanItemLand(leftLoc))
                    return leftLoc;
                Operations.MoveInDirection8(ref upLoc, Direction8.Up, 1);
                if (CanItemLand(upLoc))
                    return upLoc;
                Operations.MoveInDirection8(ref rightLoc, Direction8.Right, 1);
                if (CanItemLand(rightLoc))
                    return rightLoc;

                Loc2D downSideLoc = downLoc;
                Loc2D leftSideLoc = leftLoc;
                Loc2D upSideLoc = upLoc;
                Loc2D rightSideLoc = rightLoc;

                for (int j = 0; j < i; j++) {
                    //check side-sides
                    Operations.MoveInDirection8(ref downSideLoc, Direction8.Right, j * 2 + 1);
                    if (CanItemLand(downSideLoc))
                        return downSideLoc;
                    Operations.MoveInDirection8(ref leftSideLoc, Direction8.Down, j * 2 + 1);
                    if (CanItemLand(leftSideLoc))
                        return leftSideLoc;
                    Operations.MoveInDirection8(ref upSideLoc, Direction8.Left, j * 2 + 1);
                    if (CanItemLand(upSideLoc))
                        return upSideLoc;
                    Operations.MoveInDirection8(ref rightSideLoc, Direction8.Up, j * 2 + 1);
                    if (CanItemLand(rightSideLoc))
                        return rightSideLoc;

                    if (j < i - 1) {
                        Operations.MoveInDirection8(ref downSideLoc, Direction8.Left, (j+1) * 2);
                        if (CanItemLand(downSideLoc))
                            return downSideLoc;
                        Operations.MoveInDirection8(ref leftSideLoc, Direction8.Up, (j + 1) * 2);
                        if (CanItemLand(leftSideLoc))
                            return leftSideLoc;
                        Operations.MoveInDirection8(ref upSideLoc, Direction8.Right, (j + 1) * 2);
                        if (CanItemLand(upSideLoc))
                            return upSideLoc;
                        Operations.MoveInDirection8(ref rightSideLoc, Direction8.Down, (j + 1) * 2);
                        if (CanItemLand(rightSideLoc))
                            return rightSideLoc;
                    }
                }
            }

            return null;
        }

        public static void RestoreHP(ActiveChar character, int hp) {
            character.HP += hp;
            if (character.HP > character.MaxHP) character.HP = character.MaxHP;
            Display.Screen.AddResult(new Results.MeterChanged(CharIndex(character), false, hp));
            Display.Screen.AddResult(new Results.BattleMsg(character.Name + " recovered " + hp + " HP."));

            if (character == FocusedCharacter) 
                Display.Screen.AddResult(new Results.SetStats(FocusedCharacter.HP, FocusedCharacter.MaxHP, FocusedCharacter.PP, FocusedCharacter.MaxPP, money));
            
        }

        public static void RestorePP(ActiveChar character, int pp) {
            character.PP += pp;
            if (character.PP > character.MaxPP) character.PP = character.MaxPP;
            Display.Screen.AddResult(new Results.MeterChanged(CharIndex(character), true, pp));
            Display.Screen.AddResult(new Results.BattleMsg(character.Name + " recovered " + pp + " PP."));

            if (character == FocusedCharacter)
                Display.Screen.AddResult(new Results.SetStats(FocusedCharacter.HP, FocusedCharacter.MaxHP, FocusedCharacter.PP, FocusedCharacter.MaxPP, money));
            
        }

        public static void DeductPP(ActiveChar character, int pp) {
            DeductPP(character, pp, true);
        }

        public static void DeductPP(ActiveChar character, int pp, bool declare) {
            character.PP -= pp;
            if (character.PP < 0) character.PP = 0;
            if (declare) {
                Display.Screen.AddResult(new Results.MeterChanged(CharIndex(character), true, -pp));
                Display.Screen.AddResult(new Results.BattleMsg(character.Name + " lost " + pp + " PP."));
            }
            if (character == FocusedCharacter) 
                Display.Screen.AddResult(new Results.SetStats(FocusedCharacter.HP, FocusedCharacter.MaxHP, FocusedCharacter.PP, FocusedCharacter.MaxPP, money));
            
        }

        public static void DamageCharacter(ActiveChar character, int hp, bool hurt = true) {
            character.HP -= hp;
            if (character.HP < 0) character.HP = 0;
            Display.Screen.AddResult(new Results.MeterChanged(CharIndex(character), false, -hp));
            Display.Screen.AddResult(new Results.BattleMsg(character.Name + " took " + hp + " damage!"));

            if (hurt && character.HP > 0) {
                Display.Screen.AddResult(new Results.CreateAction(CharIndex(character), character, Display.CharSprite.ActionType.Hurt));
            }


            if (character == FocusedCharacter) 
                Display.Screen.AddResult(new Results.SetStats(FocusedCharacter.HP, FocusedCharacter.MaxHP, FocusedCharacter.PP, FocusedCharacter.MaxPP, money));
            
            //enqueue HP lost
            if (character.HP == 0) {
                character.dead = true;
                int ind = CharIndex(character);
                if (ind < 0) {
                    Display.Screen.EndConcurrent();
                    Display.Screen.AddResult(new Results.Defeated(ind, character));
                    Display.Screen.AddResult(new Results.BattleMsg("Oh no! " + character.Name + " fainted!"));

                    Display.Screen.BeginConcurrent();
                } else {
                    Display.Screen.AddResult(new Results.Defeated(ind, character));
                    Display.Screen.AddResult(new Results.BattleMsg(character.Name + " was defeated!"));
                    money += 1;
                    Display.Screen.AddResult(new Results.SE("magic99"));
                    //Display.Screen.AddResult(new Results.CreateAnim(new Display.CharacterOriginAnimation(CharIndex(character), 21, 5500, 1), Display.Screen.EffectPriority.Top));
                    //Display.Screen.AddResult(new Results.CreateEmitter(new Display.FountainEmitter(character.CharLoc * Graphics.TextureManager.TILE_SIZE,
                    //    201, 2, 2000, 50, 2000, 4, 4, 40000)));
                    Display.Screen.AddResult(new Results.SetStats(FocusedCharacter.HP, FocusedCharacter.MaxHP, FocusedCharacter.PP, FocusedCharacter.MaxPP, money));
                    Display.Screen.AddResult(new Results.RemoveCharacter(ind));
                }
            }
        }

        public static void DamageTile(Loc2D loc) {
            Tile tile = CurrentMap.MapArray[loc.X, loc.Y];

        }


        public static void ProcessStartAnim(BattleSetup setup) {
            int maxStallTime = 0;

            //declare attack
            if (!String.IsNullOrWhiteSpace(setup.Move.Name)) {
                Display.Screen.AddResult(new Results.BattleMsg(setup.Move.Name));
            }

            //play attacker sound
            Display.Screen.AddResult(new Results.SE("magic" + setup.Move.StartSound.ToString()));

            Loc2D endLoc = setup.Attacker.CharLoc;

            switch (setup.Move.Range.RangeType) {
                case Enums.RangeType.Front:
                case Enums.RangeType.FrontUntil:
                case Enums.RangeType.Room:
                case Enums.RangeType.FrontAndSides: {
                        Operations.MoveInDirection8(ref endLoc, setup.Attacker.CharDir, setup.Move.Range.Distance);
                    }
                    break;
                case Enums.RangeType.FlyInArc: {
                        //extra case for hitting no one
                        Operations.MoveInDirection8(ref endLoc, setup.Attacker.CharDir, 2);
                    }
                    break;
            }

            //draw user action
            //send attack attempt
            if (setup.Move.StartUserAnim.ActionType != Display.CharSprite.ActionType.None) {
                Results.CreateAction result = new Results.CreateAction(CharIndex(setup.Attacker), setup.Attacker, setup.Move.StartUserAnim.ActionType);
                    //setup.Move.StartUserAnim.Anim1, setup.Move.StartUserAnim.Anim2, setup.Move.StartUserAnim.Anim3);
                Display.Screen.AddResult(result);
                if (result.Delay.ToMillisecs() > maxStallTime)
                    maxStallTime = result.Delay.ToMillisecs();
                result.InstantPass = true;
            }

            //draw effect
            if (setup.Move.StartAnim.AnimIndex >= 0) {
                switch (setup.Move.StartAnim.AnimType) {
                    case Display.MoveAnimationType.Normal: {
                        //get pass time
                            Display.Screen.AddResult(new Results.CreateSpell(
                                new Display.NormalMoveAnimation(setup.Attacker.CharLoc, setup.Move.StartAnim.AnimIndex,
                                setup.Move.StartAnim.FrameLength, setup.Move.StartAnim.Anim1), Display.Screen.EffectPriority.None));
                        }
                        break;
                    case Display.MoveAnimationType.Arrow: {
                        //get pass time
                            Display.Screen.AddResult(new Results.CreateSpell(
                                    new Display.ArrowMoveAnimation(setup.Attacker.CharLoc, setup.Move.StartAnim.AnimIndex,
                                    setup.Move.StartAnim.FrameLength, setup.Attacker.CharDir, setup.Move.Range.Distance, setup.Move.StartAnim.Anim1), Display.Screen.EffectPriority.None));
                        }
                        break;
                    case Display.MoveAnimationType.Throw: {
                        //get pass time
                            Display.Screen.AddResult(new Results.CreateSpell(
                                    new Display.ThrowMoveAnimation(setup.Attacker.CharLoc, endLoc, setup.Move.StartAnim.AnimIndex,
                                        setup.Move.StartAnim.FrameLength, setup.Move.StartAnim.Anim1, true), Display.Screen.EffectPriority.None));
                        }
                        break;
                    case Display.MoveAnimationType.Beam: {
                        //get pass time
                            Display.Screen.AddResult(new Results.CreateSpell(
                                    new Display.BeamMoveAnimation(setup.Attacker.CharLoc, setup.Move.StartAnim.AnimIndex,
                                    setup.Move.StartAnim.FrameLength, setup.Attacker.CharDir, setup.Move.Range.Distance, RenderTime.FromMillisecs(setup.Move.StartAnim.Anim1)), Display.Screen.EffectPriority.Top));

                        }
                        break;
                    case Display.MoveAnimationType.Overlay: {
                        //get pass time
                            Display.Screen.AddResult(new Results.CreateSpell(
                                new Display.OverlayMoveAnimation(setup.Move.StartAnim.AnimIndex,
                                setup.Move.StartAnim.FrameLength, setup.Move.StartAnim.Anim1,
                                (byte)setup.Move.StartAnim.Anim2), Display.Screen.EffectPriority.Overlay));
                        }
                        break;
                    case Display.MoveAnimationType.Tile: {
                        //get pass time
                            Display.Screen.AddResult(new Results.CreateEmitter(
                            new Display.TileMoveAnimation(setup.Attacker.CharLoc, setup.Move.StartAnim.AnimIndex,
                            setup.Move.StartAnim.FrameLength, setup.Move.StartAnim.Anim1, setup.Move.Range.RangeType,
                            setup.Attacker.CharDir, setup.Move.Range.Distance, RenderTime.FromMillisecs(setup.Move.StartAnim.Anim2))));

                        }
                        break;
                    case Display.MoveAnimationType.ItemArrow: {
                        //get pass time
                            Display.Screen.AddResult(new Results.CreateSpell(
                                new Display.ItemArrowMoveAnimation(setup.Attacker.CharLoc, setup.Move.StartAnim.AnimIndex,
                                setup.Move.StartAnim.FrameLength, setup.Attacker.CharDir, setup.Move.Range.Distance,
                                setup.Move.StartAnim.Anim1), Display.Screen.EffectPriority.None));

                        }
                        break;
                    case Display.MoveAnimationType.ItemThrow: {
                        //get pass time
                            Display.Screen.AddResult(new Results.CreateSpell(
                                    new Display.ItemThrowMoveAnimation(setup.Attacker.CharLoc, endLoc, setup.Move.StartAnim.AnimIndex,
                                        setup.Move.StartAnim.FrameLength, setup.Move.StartAnim.Anim1, true), Display.Screen.EffectPriority.None));
                        }
                        break;
                }
            }

            setup.TimeForHit.Value += maxStallTime;
            Display.Screen.AddResult(new Results.Wait(RenderTime.FromMillisecs(maxStallTime)));
            setup.SetBattleTag("FullRange", setup.Move.Range.Distance);
        }


        public static void ProcessTravelingAnim(BattleSetup setup) {
            int maxStallTime = 0;
            int sameThreadPauseTime = 0;
            
            Loc2D endLoc = setup.Attacker.CharLoc;

            //check against foe
            setup.AllTargets = SimulateAttackRange(setup.Attacker, setup.Attacker.CharLoc, setup.Attacker.CharDir, setup.Move.Range);

            //play mid sound
            Display.Screen.AddResult(new Results.SE("magic" + setup.Move.MidSound.ToString()));

            //calculate positions
            switch (setup.Move.Range.RangeType) {
                case Enums.RangeType.Front: {
                        Operations.MoveInDirection8(ref endLoc, setup.Attacker.CharDir, setup.Move.Range.Distance);
                    }
                    break;
                case Enums.RangeType.FrontUntil: {
                        if (setup.AllTargets.Targets.Count > 0) {
                            endLoc = setup.AllTargets.Targets[0].Character.CharLoc;
                            setup.Move.Range.Distance = setup.AllTargets.Targets[0].Distance;
                        } else {
                            Operations.MoveInDirection8(ref endLoc, setup.Attacker.CharDir, setup.Move.Range.Distance);
                        }
                    }
                    break;
                case Enums.RangeType.Room: {
                        Operations.MoveInDirection8(ref endLoc, setup.Attacker.CharDir, setup.Move.Range.Distance);
                    }
                    break;
                case Enums.RangeType.FrontAndSides: {
                        Operations.MoveInDirection8(ref endLoc, setup.Attacker.CharDir, setup.Move.Range.Distance);
                    }
                    break;
                case Enums.RangeType.FlyInArc: {
                        //extra case for hitting no one
                        if (setup.AllTargets.Targets.Count == 0) {
                            Operations.MoveInDirection8(ref endLoc, setup.Attacker.CharDir, 2);
                            setup.Move.Range.Distance = 2;
                        } else {
                            endLoc = setup.AllTargets.Targets[0].Character.CharLoc;
                            setup.Move.Range.Distance = setup.AllTargets.Targets[0].Distance;
                        }
                    }
                    break;
            }

            if (setup.Move.MidUserAnim.ActionType != Display.CharSprite.ActionType.None) {
                Results.CreateAction result = new Results.CreateAction(CharIndex(setup.Attacker), setup.Attacker, setup.Move.MidUserAnim.ActionType);
                    //setup.Move.MidUserAnim.Anim1, setup.Move.MidUserAnim.Anim2, setup.Move.MidUserAnim.Anim3);
                Display.Screen.AddResult(result);
                if (result.Delay.ToMillisecs() > maxStallTime)
                    maxStallTime = result.Delay.ToMillisecs();
                result.InstantPass = true;
            }


            //draw target action
            if (setup.Move.MidTargetAnim.ActionType != Display.CharSprite.ActionType.None) {
                Results.IResult result = new Results.CreateAction(CharIndex(setup.Attacker), setup.Attacker, setup.Move.MidTargetAnim.ActionType);
                    //setup.Move.MidTargetAnim.Anim1, setup.Move.MidTargetAnim.Anim2, setup.Move.MidTargetAnim.Anim3);
                Display.Screen.AddResult(result);
                if (result.Delay.ToMillisecs() > maxStallTime)
                    maxStallTime = result.Delay.ToMillisecs();
            }

            //draw effect
            if (setup.Move.MidAnim.AnimIndex >= 0) {
                switch (setup.Move.MidAnim.AnimType) {
                    case Display.MoveAnimationType.Normal: {
                        //get pass time
                        foreach (Target target in setup.AllTargets.Targets) {
                                Display.Screen.AddResult(new Results.CreateSpell(
                                    new Display.NormalMoveAnimation(target.Character.CharLoc, setup.Move.MidAnim.AnimIndex,
                                    setup.Move.MidAnim.FrameLength, setup.Move.MidAnim.Anim1), Display.Screen.EffectPriority.None));
                            }
                        }
                        break;
                    case Display.MoveAnimationType.Arrow: {
                        //change TimePerWave
                        maxStallTime = 0;
                        Display.ArrowMoveAnimation anim = new Display.ArrowMoveAnimation(setup.Attacker.CharLoc, setup.Move.MidAnim.AnimIndex,
                                    setup.Move.MidAnim.FrameLength, setup.Attacker.CharDir, setup.Move.Range.Distance, setup.Move.MidAnim.Anim1);
                        Display.Screen.AddResult(new Results.CreateSpell(anim, Display.Screen.EffectPriority.None));
                        setup.TotalWaves = anim.TotalWaves;
                        setup.TotalWaveTime = anim.TotalTime;
                        }
                        break;
                    case Display.MoveAnimationType.Throw: {
                        //get pass time
                            if (setup.AllTargets.Targets.Count > 0) {
                                foreach (Target target in setup.AllTargets.Targets) {
                                    Display.Screen.AddResult(new Results.CreateSpell(
                                            new Display.ThrowMoveAnimation(setup.Attacker.CharLoc, target.Character.CharLoc, setup.Move.MidAnim.AnimIndex,
                                                setup.Move.MidAnim.FrameLength, setup.Move.MidAnim.Anim1, false), Display.Screen.EffectPriority.None));
                                }
                            } else {
                                Display.Screen.AddResult(new Results.CreateSpell(
                                        new Display.ThrowMoveAnimation(setup.Attacker.CharLoc, endLoc, setup.Move.MidAnim.AnimIndex,
                                            setup.Move.MidAnim.FrameLength, setup.Move.MidAnim.Anim1, true), Display.Screen.EffectPriority.None));
                            }
                        }
                        break;
                    case Display.MoveAnimationType.Beam: {
                        maxStallTime = 0;
                        //change TimePerWave
                        Display.BeamMoveAnimation anim = new Display.BeamMoveAnimation(setup.Attacker.CharLoc, setup.Move.MidAnim.AnimIndex,
                                    setup.Move.MidAnim.FrameLength, setup.Attacker.CharDir, setup.Move.Range.Distance,
                                    RenderTime.FromMillisecs(setup.Move.MidAnim.Anim1));
                            Display.Screen.AddResult(new Results.CreateSpell( anim, Display.Screen.EffectPriority.Top));
                        setup.TotalWaves = anim.TotalDistance;
                        setup.TotalWaveTime = anim.TotalTime.ToMillisecs();
                        if (setup.TotalWaves > 0)
                            setup.TimeForHit.Value -= setup.TotalWaveTime / setup.TotalWaves;
                        }
                        break;
                    case Display.MoveAnimationType.Overlay: {
                        //get pass time
                            Display.Screen.AddResult(new Results.CreateSpell(
                                new Display.OverlayMoveAnimation(setup.Move.MidAnim.AnimIndex,
                                setup.Move.MidAnim.FrameLength, setup.Move.MidAnim.Anim1, (byte)setup.Move.MidAnim.Anim2), Display.Screen.EffectPriority.Overlay));
                        }
                        break;
                    case Display.MoveAnimationType.Tile: {
                        maxStallTime = 0;
                        //change TimePerWave
                        Display.TileMoveAnimation emitter = new Display.TileMoveAnimation(setup.Attacker.CharLoc, setup.Move.MidAnim.AnimIndex,
                            setup.Move.MidAnim.FrameLength, setup.Move.MidAnim.Anim1, setup.Move.Range.RangeType,
                            setup.Attacker.CharDir, setup.Move.Range.Distance, RenderTime.FromMillisecs(setup.Move.MidAnim.Anim2));
                            Display.Screen.AddResult(new Results.CreateEmitter(emitter));
                            setup.TotalWaves = emitter.TotalDistance;
                            setup.TotalWaveTime = emitter.TotalTime.ToMillisecs();
                            setup.TimeForHit.Value -= emitter.StallTime.ToMillisecs();
                        }
                        break;
                    case Display.MoveAnimationType.ItemArrow: {
                        maxStallTime = 0;
                        //change TimePerWave
                        Display.ItemArrowMoveAnimation anim = new Display.ItemArrowMoveAnimation(setup.Attacker.CharLoc, setup.Move.MidAnim.AnimIndex,
                                setup.Move.MidAnim.FrameLength, setup.Attacker.CharDir, setup.Move.Range.Distance, setup.Move.MidAnim.Anim1);
                            Display.Screen.AddResult(new Results.CreateSpell( anim, Display.Screen.EffectPriority.None));
                            setup.TotalWaves = anim.TotalWaves;
                            setup.TotalWaveTime = anim.TotalTime;
                        }
                        break;
                    case Display.MoveAnimationType.ItemThrow: {
                        //get pass time
                            if (setup.AllTargets.Targets.Count > 0) {
                                foreach (Target target in setup.AllTargets.Targets) {
                                    Display.Screen.AddResult(new Results.CreateSpell(
                                            new Display.ItemThrowMoveAnimation(setup.Attacker.CharLoc, target.Character.CharLoc, setup.Move.MidAnim.AnimIndex,
                                                setup.Move.MidAnim.FrameLength, setup.Move.MidAnim.Anim1, false), Display.Screen.EffectPriority.None));
                                }
                            } else {
                                Display.Screen.AddResult(new Results.CreateSpell(
                                        new Display.ItemThrowMoveAnimation(setup.Attacker.CharLoc, endLoc, setup.Move.MidAnim.AnimIndex,
                                            setup.Move.MidAnim.FrameLength, setup.Move.MidAnim.Anim1, true), Display.Screen.EffectPriority.None));
                            }
                        }
                        break;
                }
            }

            setup.TimeForHit.Value += maxStallTime;
            if (sameThreadPauseTime < maxStallTime)
                sameThreadPauseTime = maxStallTime;
            Display.Screen.AddResult(new Results.Wait(RenderTime.FromMillisecs(sameThreadPauseTime)));
            setup.SetBattleTag("EndLoc", endLoc);

        }


        public static void ProcessDefenderAnim(BattleSetup setup) {

            //play sound
            Display.Screen.AddResult(new Results.SE("magic" + setup.Move.EndSound.ToString()));

            //trigger animations of user
            if (setup.Move.EndUserAnim.ActionType != Display.CharSprite.ActionType.None) {
                Display.Screen.AddResult(new Results.CreateAction(CharIndex(setup.Attacker), setup.Attacker, setup.Move.EndUserAnim.ActionType));
                    //setup.Move.EndUserAnim.Anim1, setup.Move.EndUserAnim.Anim2, setup.Move.EndUserAnim.Anim3));
            }

            //trigger animations of target
            if (setup.Move.EndTargetAnim.ActionType != Display.CharSprite.ActionType.None) {
                Display.Screen.AddResult(new Results.CreateAction(CharIndex(setup.Attacker), setup.Attacker, setup.Move.EndTargetAnim.ActionType));
                    //setup.Move.EndTargetAnim.Anim1, setup.Move.EndTargetAnim.Anim2, setup.Move.EndTargetAnim.Anim3));
            }

            //draw effect
            if (setup.Move.EndAnim.AnimIndex >= 0) {
                switch (setup.Move.EndAnim.AnimType) {
                    case Display.MoveAnimationType.Normal: {
                            Display.Screen.AddResult(new Results.CreateSpell(
                                    new Display.NormalMoveAnimation(setup.Defender.CharLoc, setup.Move.EndAnim.AnimIndex,
                                    setup.Move.EndAnim.FrameLength, setup.Move.EndAnim.Anim1), Display.Screen.EffectPriority.None));
                        }
                        break;
                    case Display.MoveAnimationType.Arrow: {
                            Display.Screen.AddResult(new Results.CreateSpell(
                                    new Display.ArrowMoveAnimation(setup.Attacker.CharLoc, setup.Move.EndAnim.AnimIndex,
                                    setup.Move.EndAnim.FrameLength, setup.Attacker.CharDir, setup.Move.Range.Distance, setup.Move.EndAnim.Anim1), Display.Screen.EffectPriority.None));
                        }
                        break;
                    case Display.MoveAnimationType.Throw: {
                            Display.Screen.AddResult(new Results.CreateSpell(
                                new Display.ThrowMoveAnimation(setup.Attacker.CharLoc, setup.Defender.CharLoc, setup.Move.EndAnim.AnimIndex,
                                    setup.Move.EndAnim.FrameLength, setup.Move.EndAnim.Anim1, false), Display.Screen.EffectPriority.None));
                        }
                        break;
                    case Display.MoveAnimationType.Beam: {
                            Display.Screen.AddResult(new Results.CreateSpell(
                                    new Display.BeamMoveAnimation(setup.Attacker.CharLoc, setup.Move.EndAnim.AnimIndex,
                                    setup.Move.EndAnim.FrameLength, setup.Attacker.CharDir,
                                    setup.Move.Range.Distance, RenderTime.FromMillisecs(setup.Move.EndAnim.Anim1)),
                                    Display.Screen.EffectPriority.Top));

                        }
                        break;
                    case Display.MoveAnimationType.Overlay: {
                            Display.Screen.AddResult(new Results.CreateSpell(
                                new Display.OverlayMoveAnimation(setup.Move.EndAnim.AnimIndex,
                                setup.Move.EndAnim.FrameLength, setup.Move.EndAnim.Anim1, (byte)setup.Move.EndAnim.Anim2), Display.Screen.EffectPriority.Overlay));
                        }
                        break;
                    case Display.MoveAnimationType.Tile: {
                            Display.Screen.AddResult(new Results.CreateEmitter(
                            new Display.TileMoveAnimation(setup.Attacker.CharLoc, setup.Move.EndAnim.AnimIndex,
                            setup.Move.EndAnim.FrameLength, setup.Move.EndAnim.Anim1, setup.Move.Range.RangeType,
                            setup.Attacker.CharDir, setup.Move.Range.Distance, RenderTime.FromMillisecs(setup.Move.EndAnim.Anim2))));

                        }
                        break;
                    case Display.MoveAnimationType.ItemArrow: {
                            Display.Screen.AddResult(new Results.CreateSpell(
                                new Display.ItemArrowMoveAnimation(setup.Attacker.CharLoc, setup.Move.EndAnim.AnimIndex,
                                setup.Move.EndAnim.FrameLength, setup.Attacker.CharDir, setup.Move.Range.Distance, setup.Move.EndAnim.Anim1), Display.Screen.EffectPriority.None));

                        }
                        break;
                    case Display.MoveAnimationType.ItemThrow: {
                            Display.Screen.AddResult(new Results.CreateSpell(
                                    new Display.ItemThrowMoveAnimation(setup.Attacker.CharLoc, setup.Defender.CharLoc, setup.Move.EndAnim.AnimIndex,
                                        setup.Move.EndAnim.FrameLength, setup.Move.EndAnim.Anim1, false), Display.Screen.EffectPriority.None));
                        }
                        break;
                }
            }
        }

    }
}
