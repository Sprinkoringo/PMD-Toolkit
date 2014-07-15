using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PMDToolkit.Maps;

namespace PMDToolkit.Logic.Gameplay {
    public class AI {

        public ActiveChar ControlledChar { get; set; }

        public ActiveChar TargetChar { get; set; }
        public Loc2D TargetLoc { get; set; }

        public List<Loc2D> PrevLocs { get; set; }

        public AI(ActiveChar controlledChar) {
            ControlledChar = controlledChar;
        }

        #region A-Star

        private List<Direction8> FindPathToTarget(Loc2D end, bool inAir) {
            PathTile[,] tileMap = GetSeenTiles();
            
            Loc2D mapStart = new Loc2D(tileMap.GetLength(0) / 2, tileMap.GetLength(1) / 2);
            Loc2D offset = ControlledChar.CharLoc - mapStart;
            Loc2D mapEnd = end - offset;

            List<PathTile> candidates = new List<PathTile>();
            tileMap[mapStart.X, mapStart.Y].Heuristic = Math.Sqrt(Math.Pow(mapEnd.X - mapStart.X, 2) + Math.Pow(mapEnd.Y - mapStart.Y, 2));
            candidates.Add(tileMap[mapStart.X, mapStart.Y]);

            while (candidates.Count > 0) {
                PathTile currentTile = candidates[0];
                if (currentTile.Location == mapEnd) {
                    List<Direction8> directions = new List<Direction8>();
                    TraceBack(directions, currentTile);
                    return directions;
                }

                candidates.RemoveAt(0);
                currentTile.Traversed = true;

                List<PathTile> neighbors = GetValidNeighbors(currentTile.Location, tileMap, offset, mapEnd, inAir);

                //insert each neighbor at the right place
                foreach (PathTile tile in neighbors) {
                    double newCost = currentTile.Cost + 1;
                    if (tile.Traversed && newCost >= tile.Cost) {
                        //not worth it
                    } else if (!candidates.Contains(tile) || newCost < tile.Cost) {
                        tile.Heuristic = Math.Sqrt(Math.Pow(mapEnd.X - tile.Location.X, 2) + Math.Pow(mapEnd.Y - tile.Location.Y, 2));
                        tile.BackReference = currentTile;
                        bool placed = false;
                        for (int i = candidates.Count-1; i >= 0; i--) {
                            if (tile.Cost + tile.Heuristic >= candidates[i].Cost + candidates[i].Heuristic) {
                                candidates.Insert(i+1, tile);
                                placed = true;
                                break;
                            } else if (candidates[i] == tile) {
                                candidates.RemoveAt(i);
                            }
                        }
                        if (!placed) {
                            candidates.Insert(0, tile);
                        }
                    }
                }

            }

            return new List<Direction8>();
        }

        private void TraceBack(List<Direction8> directions, PathTile tile) {
            if (tile.BackReference != null) {
                directions.Insert(0, Operations.GetDirection8(tile.BackReference.Location, tile.Location));
                TraceBack(directions, tile.BackReference);
            }
        }

        private List<PathTile> GetValidNeighbors(Loc2D loc, PathTile[,] tileMap, Loc2D offset, Loc2D mapEnd, bool inAir) {
            List<PathTile> candidates = new List<PathTile>();
            //get all tiles not blocked, or out of range
            for (int i = 0; i < 8; i++) {
                Loc2D newLoc = loc;
                Operations.MoveInDirection8(ref newLoc, (Direction8)i, 1);
                if (newLoc == mapEnd || Operations.IsInBound(tileMap.GetLength(0), tileMap.GetLength(1), newLoc.X, newLoc.Y)
                    && !Processor.DirBlocked((Direction8)i, ControlledChar, loc + offset, false, 1))
                {
                    candidates.Add(tileMap[newLoc.X, newLoc.Y]);
                }
            }

            return candidates;
        }

        #endregion

        private int HorizSight() {
            return (int)Math.Ceiling((double)(Graphics.TextureManager.SCREEN_WIDTH - Graphics.TextureManager.TILE_SIZE) / 2 / Graphics.TextureManager.TILE_SIZE);
        }

        private int VertSight() {
            return (int)Math.Ceiling((double)(Graphics.TextureManager.SCREEN_HEIGHT - Graphics.TextureManager.TILE_SIZE) / 2 / Graphics.TextureManager.TILE_SIZE);
        }

        private PathTile[,] GetSeenTiles() {
            int widthSeen = HorizSight() * 2 + 1;
            int heightSeen = VertSight() * 2 + 1;
            PathTile[,] tileMap = new PathTile[widthSeen, heightSeen];
            for (int y = 0; y < heightSeen; y++) {
                for (int x = 0; x < widthSeen; x++) {
                    tileMap[x, y] = new PathTile(new Loc2D(x, y), true, false, 0, 0, null);
                }
            }
            return tileMap;
        }

        private List<ActiveChar> GetSeenCharacters(bool includeSelf, bool includeAllies, bool includeFoes) {
            List<ActiveChar> seenChars = new List<ActiveChar>();
            for (int i = -Processor.MAX_TEAM_SLOTS; i < BasicMap.MAX_NPC_SLOTS; i++) {
                ActiveChar target = Processor.CharOfIndex(i);
                if (CanSeeCharacter(target) && Processor.IsTargeted(ControlledChar, target, includeSelf, includeAllies, includeFoes)) {
                    seenChars.Add(target);
                }
            }
            return seenChars;
        }

        private bool CanSeeCharacter(ActiveChar character) {
            if (character == null) return false;
            int horizSight = HorizSight();
            int vertSight = VertSight();

            if (character.CharLoc.X < ControlledChar.CharLoc.X - horizSight || character.CharLoc.X > ControlledChar.CharLoc.X + horizSight) {
                return false;
            }
            if (character.CharLoc.Y < ControlledChar.CharLoc.Y - vertSight || character.CharLoc.Y > ControlledChar.CharLoc.Y + vertSight) {
                return false;
            }

            return true;
        }

        private List<Loc2D> GetPossibleExits() {

            return new List<Loc2D>();
        }

        public Command Think() {
            try {
                List<ActiveChar> targets = GetSeenCharacters(false, false, true);

                if (targets.Count > 0) {
                    ActiveChar target = targets[0];
                    List<Direction8> dirs = FindPathToTarget(target.CharLoc, false);
                    if (dirs.Count == 0) {
                        dirs = FindPathToTarget(target.CharLoc, true);
                    }
                    if (dirs.Count == 1) {
                        return new Command(Command.CommandType.Attack, (int)dirs[0]);
                    } else if (ControlledChar.Status == Enums.StatusAilment.Freeze || ControlledChar.Status == Enums.StatusAilment.Sleep) {
                        //attack if in front
                        return new Command(Command.CommandType.Attack);
                    } else if (dirs.Count > 1) {
                        return new Command(Command.CommandType.Move, (int)dirs[0]);//change to 8 to enable diagonal movement
                    } else {
                        //!?!?!?
                        return new Command(Command.CommandType.Wait);
                    }
                } else {
                    //walk with purpose
                    return new Command(Command.CommandType.Move, Processor.Rand.Next(8));
                }
            } catch (Exception ex) {
                Logs.Logger.LogError(new Exception("AI Error\n", ex));
                return new Command(Command.CommandType.Wait);
            }
        }

    }
}
