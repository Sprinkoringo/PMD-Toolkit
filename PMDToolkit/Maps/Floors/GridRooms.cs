using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PMDToolkit;
using PMDToolkit.Maps;
using PMDToolkit.Data;
using PMDToolkit.Logic.Gameplay;

namespace PMDToolkit.Maps.Floors {
    public class GridRooms : RandomMap {

        enum GridType
        {
            Blocked,
            Walkable,
            Hall,
            Door,
            End,
            Water
        };

        int minNpcsPerRoom = 0;
        int maxNpcsPerRoom = 4;
        
        private Loc2D StartRoom { get; set; }
        private Loc2D EndRoom { get; set; }
        private Loc2D StartPoint { get { return BorderPoints[0]; } }

        private GridType[,] GridArray { get; set; }

        private DungeonArrayRoom[,] Rooms { get; set; }
        private DungeonArrayHall[,] VHalls { get; set; }
        private DungeonArrayHall[,] HHalls { get; set; }

        public GridRooms() {
            
        }

        //an initial create-map method
        public override void Generate(int seed, RDungeonFloor entry, List<FloorBorder> floorBorders, Dictionary<int, List<int>> borderLinks) {
            //TODO: make sure that this algorithm follows floorBorders and borderLinks constraints

            this.seed = seed;
            this.entry = entry;
            FloorBorders = floorBorders;
            BorderLinks = borderLinks;

            BorderPoints = new Loc2D[floorBorders.Count];

            rand = new Random(seed);

            MapArray = new Tile[entry.FloorSettings["CellX"] * entry.FloorSettings["CellWidth"] + 2, entry.FloorSettings["CellY"] * entry.FloorSettings["CellHeight"] + 2];
            GridArray = new GridType[entry.FloorSettings["CellX"] * entry.FloorSettings["CellWidth"] + 2, entry.FloorSettings["CellY"] * entry.FloorSettings["CellHeight"] + 2];

            Rooms = new DungeonArrayRoom[entry.FloorSettings["CellX"], entry.FloorSettings["CellY"]]; //array of all rooms
            VHalls = new DungeonArrayHall[entry.FloorSettings["CellX"], entry.FloorSettings["CellY"] - 1]; //vertical halls
            HHalls = new DungeonArrayHall[entry.FloorSettings["CellX"] - 1, entry.FloorSettings["CellY"]]; //horizontal halls
            StartRoom = new Loc2D(-1, -1);     //marks spawn point

            bool isDone;   // bool used for various purposes


            //initialize map array to empty
            for (int y = 0; y < Height; y++) {
                for (int x = 0; x < Width; x++) {
                    GridArray[x, y] = GridType.Blocked;
                }
            }

            //initialize all rooms+halls to closed by default
            for (int x = 0; x < entry.FloorSettings["CellX"]; x++) {
                for (int y = 0; y < entry.FloorSettings["CellY"]; y++) {
                    Rooms[x, y] = new DungeonArrayRoom();
                }
            }



            for (int x = 0; x < entry.FloorSettings["CellX"]; x++) {
                for (int y = 0; y < entry.FloorSettings["CellY"] - 1; y++) {
                    VHalls[x, y] = new DungeonArrayHall();
                }
            }

            for (int x = 0; x < entry.FloorSettings["CellX"] - 1; x++) {
                for (int y = 0; y < entry.FloorSettings["CellY"]; y++) {
                    HHalls[x, y] = new DungeonArrayHall();
                }
            }

            // path generation algorithm
            StartRoom = new Loc2D(rand.Next(0, entry.FloorSettings["CellX"]), rand.Next(0, entry.FloorSettings["CellY"])); // randomly determine start room
            Loc2D wanderer = StartRoom;

            int pathsMade = 0;
            int pathsNeeded = rand.Next(0, 6) + 5; // magic numbers, determine what the dungeon looks like (in general, paths)
            Direction4 prevDir = Direction4.None; // direction of movement
            do {
                if (rand.Next(0, (2 + pathsMade)) == 0) {//will end the current path and start a new one from the start
                    if (rand.Next(0, 2) == 0) {//determine if the room should be open or a hall
                        Rooms[wanderer.X, wanderer.Y].Opened = DungeonArrayRoom.RoomState.Open;
                    } else {
                        Rooms[wanderer.X, wanderer.Y].Opened = DungeonArrayRoom.RoomState.Hall;
                    }
                    pathsMade++;
                    wanderer = StartRoom;
                    prevDir = Direction4.None;
                } else {
                    bool working = true;
                    do {
                        Loc2D sample = wanderer;
                        Direction4 nextDir = (Direction4)rand.Next(0, 4);
                        if (nextDir != prevDir) {//makes sure there is no backtracking
                            Operations.MoveInDirection4(ref sample, nextDir, 1);
                            prevDir = Operations.ReverseDir(nextDir);
                            if (sample.X >= 0 && sample.X < entry.FloorSettings["CellX"] && sample.Y >= 0 && sample.Y < entry.FloorSettings["CellY"]) {// a is the room to be checked after making a move between rooms
                                openHallBetween(wanderer, sample);
                                wanderer = sample;
                                working = false;
                            }
                        } else {
                            prevDir = Direction4.None;
                        }
                    } while (working);

                    if (rand.Next(0, 2) == 0) {//determine if the room should be open or a hall
                        Rooms[wanderer.X, wanderer.Y].Opened = DungeonArrayRoom.RoomState.Open;
                    } else {
                        Rooms[wanderer.X, wanderer.Y].Opened = DungeonArrayRoom.RoomState.Hall;
                    }
                }
            } while (pathsMade < pathsNeeded);

            Rooms[StartRoom.X, StartRoom.Y].Opened = DungeonArrayRoom.RoomState.Open;

            //Determine key rooms
            isDone = false;
            do { //determine ending room randomly
                int x = rand.Next(0, Rooms.GetLength(0));
                int y = rand.Next(0, Rooms.GetLength(1));
                if (Rooms[x, y].Opened == DungeonArrayRoom.RoomState.Open) {
                    EndRoom = new Loc2D(x, y);
                    isDone = true;
                }
            } while (!isDone);

            StartRoom = new Loc2D(-1, -1);

            isDone = false;
            do { //determine starting room randomly
                int x = rand.Next(0, Rooms.GetLength(0));
                int y = rand.Next(0, Rooms.GetLength(1));
                if (Rooms[x, y].Opened == DungeonArrayRoom.RoomState.Open) {
                    StartRoom = new Loc2D(x, y);
                    isDone = true;
                }
            } while (!isDone);



            // begin part 2, creating ASCII map
            //create rooms

            for (int i = 0; i < Rooms.GetLength(0); i++) {
                for (int j = 0; j < Rooms.GetLength(1); j++) {
                    if (Rooms[i, j].Opened != DungeonArrayRoom.RoomState.Closed) {
                        createRoom(i, j);
                    }
                }
            }

            for (int i = 0; i < Rooms.GetLength(0); i++) {
                for (int j = 0; j < Rooms.GetLength(1); j++) {
                    if (Rooms[i, j].Opened != DungeonArrayRoom.RoomState.Closed) {
                        drawRoom(i, j);
                    }
                }
            }

            for (int i = 0; i < Rooms.GetLength(0); i++) {
                for (int j = 0; j < Rooms.GetLength(1); j++) {
                    if (Rooms[i, j].Opened != DungeonArrayRoom.RoomState.Closed) {
                        padSingleRoom(i, j);
                    }
                }
            }

            for (int i = 0; i < VHalls.GetLength(0); i++) {
                for (int j = 0; j < VHalls.GetLength(1); j++) {
                    if (VHalls[i, j].Open) {
                        createVHall(i, j);
                    }
                }
            }

            for (int i = 0; i < HHalls.GetLength(0); i++) {
                for (int j = 0; j < HHalls.GetLength(1); j++) {
                    if (HHalls[i, j].Open) {
                        createHHall(i, j);
                    }
                }
            }


            for (int i = 0; i < VHalls.GetLength(0); i++) {
                for (int j = 0; j < VHalls.GetLength(1); j++) {
                    if (VHalls[i, j].Open) {
                        DrawHalls(VHalls[i, j]);
                    }
                }
            }

            for (int i = 0; i < HHalls.GetLength(0); i++) {
                for (int j = 0; j < HHalls.GetLength(1); j++) {
                    if (HHalls[i, j].Open) {
                        DrawHalls(HHalls[i, j]);
                    }
                }
            }

            addSEpos(StartRoom, true);
            addSEpos(EndRoom, false);

            //texturing
            MapLayer ground = new MapLayer(Width, Height);
            GroundLayers.Add(ground);
            for (int y = 0; y < Height; y++) {
                for (int x = 0; x < Width; x++) {
                    if (GridArray[x, y] == GridType.End) {
                        MapArray[x, y] = new Tile(PMDToolkit.Enums.TileType.ChangeFloor, 1, 0, 0);
                        GroundLayers[0].Tiles[x, y] = new TileAnim(new Loc2D(47, 0), 0);
                    } else if (GridArray[x, y] == GridType.Blocked) {
                        MapArray[x, y] = new Tile(PMDToolkit.Enums.TileType.Blocked, 0, 0, 0);

                        bool[] blockedDirs = new bool[8];
                        for (int n = 0; n < 8; n++) {
                            blockedDirs[n] = IsBlocked(x, y, (Direction8)n);
                        }
                        if (blockedDirs[(int)Direction8.Down] && blockedDirs[(int)Direction8.Left] && blockedDirs[(int)Direction8.Up] && blockedDirs[(int)Direction8.Right]) {
                            int layer = 0;
                            if (!blockedDirs[(int)Direction8.DownLeft])
                                layer += 8 * 2;
                            
                            if (!blockedDirs[(int)Direction8.UpLeft])
                                layer += 1;
                            
                            if (!blockedDirs[(int)Direction8.UpRight])
                                layer += 8;
                            
                            if (!blockedDirs[(int)Direction8.DownRight])
                                layer += 2;

                            GroundLayers[0].Tiles[x, y] = new TileAnim(new Loc2D(), 0);
                        } else if (!blockedDirs[(int)Direction8.Down] && blockedDirs[(int)Direction8.Left] && blockedDirs[(int)Direction8.Up] && blockedDirs[(int)Direction8.Right]) {
                            int layer = 6;
                            if (blockedDirs[(int)Direction8.UpRight])
                                layer += 1 * 8;
                            
                            if (blockedDirs[(int)Direction8.UpLeft])
                                layer += 2 * 8;

                            GroundLayers[0].Tiles[x, y] = new TileAnim(new Loc2D(), 0);
                        } else if (blockedDirs[(int)Direction8.Down] && !blockedDirs[(int)Direction8.Left] && blockedDirs[(int)Direction8.Up] && blockedDirs[(int)Direction8.Right]) {
                            int layer = 7;
                            if (blockedDirs[(int)Direction8.DownRight])
                                layer += 1 * 8;
                            
                            if (blockedDirs[(int)Direction8.UpRight])
                                layer += 2 * 8;

                            GroundLayers[0].Tiles[x, y] = new TileAnim(new Loc2D(), 0);
                        } else if (blockedDirs[(int)Direction8.Down] && blockedDirs[(int)Direction8.Left] && !blockedDirs[(int)Direction8.Up] && blockedDirs[(int)Direction8.Right]) {
                            int layer = 4;
                            if (blockedDirs[(int)Direction8.DownLeft])
                                layer += 1 * 8;
                            
                            if (blockedDirs[(int)Direction8.DownRight])
                                layer += 2 * 8;

                            GroundLayers[0].Tiles[x, y] = new TileAnim(new Loc2D(), 0);
                        } else if (blockedDirs[(int)Direction8.Down] && blockedDirs[(int)Direction8.Left] && blockedDirs[(int)Direction8.Up] && !blockedDirs[(int)Direction8.Right]) {
                            int layer = 5;
                            if (blockedDirs[(int)Direction8.UpLeft])
                                layer += 1 * 8;
                            
                            if (blockedDirs[(int)Direction8.DownLeft])
                                layer += 2 * 8;

                            GroundLayers[0].Tiles[x, y] = new TileAnim(new Loc2D(), 0);
                        } else if (!blockedDirs[(int)Direction8.Down] && !blockedDirs[(int)Direction8.Left] && blockedDirs[(int)Direction8.Up] && blockedDirs[(int)Direction8.Right]) {
                            int layer = 34;
                            if (blockedDirs[(int)Direction8.UpRight])
                                layer += 8;

                            GroundLayers[0].Tiles[x, y] = new TileAnim(new Loc2D(), 0);
                        } else if (blockedDirs[(int)Direction8.Down] && !blockedDirs[(int)Direction8.Left] && !blockedDirs[(int)Direction8.Up] && blockedDirs[(int)Direction8.Right]) {
                            int layer = 35;
                            if (blockedDirs[(int)Direction8.DownRight])
                                layer += 8;

                            GroundLayers[0].Tiles[x, y] = new TileAnim(new Loc2D(), 0);
                        } else if (blockedDirs[(int)Direction8.Down] && blockedDirs[(int)Direction8.Left] && !blockedDirs[(int)Direction8.Up] && !blockedDirs[(int)Direction8.Right]) {
                            int layer = 32;
                            if (blockedDirs[(int)Direction8.DownLeft])
                                layer += 8;

                            GroundLayers[0].Tiles[x, y] = new TileAnim(new Loc2D(), 0);
                        } else if (!blockedDirs[(int)Direction8.Down] && blockedDirs[(int)Direction8.Left] && blockedDirs[(int)Direction8.Up] && !blockedDirs[(int)Direction8.Right]) {
                            int layer = 33;
                            if (blockedDirs[(int)Direction8.UpLeft])
                                layer += 8;

                            GroundLayers[0].Tiles[x, y] = new TileAnim(new Loc2D(), 0);
                        } else if (blockedDirs[(int)Direction8.Down] && !blockedDirs[(int)Direction8.Left] && !blockedDirs[(int)Direction8.Up] && !blockedDirs[(int)Direction8.Right])
                            GroundLayers[0].Tiles[x, y] = new TileAnim(new Loc2D(36, 0), 0);
                        else if (!blockedDirs[(int)Direction8.Down] && blockedDirs[(int)Direction8.Left] && !blockedDirs[(int)Direction8.Up] && !blockedDirs[(int)Direction8.Right])
                            GroundLayers[0].Tiles[x, y] = new TileAnim(new Loc2D(37, 0), 0);
                        else if (!blockedDirs[(int)Direction8.Down] && !blockedDirs[(int)Direction8.Left] && blockedDirs[(int)Direction8.Up] && !blockedDirs[(int)Direction8.Right])
                            GroundLayers[0].Tiles[x, y] = new TileAnim(new Loc2D(38, 0), 0);
                        else if (!blockedDirs[(int)Direction8.Down] && !blockedDirs[(int)Direction8.Left] && !blockedDirs[(int)Direction8.Up] && blockedDirs[(int)Direction8.Right])
                            GroundLayers[0].Tiles[x, y] = new TileAnim(new Loc2D(39, 0), 0);
                        else if (!blockedDirs[(int)Direction8.Down] && blockedDirs[(int)Direction8.Left] && !blockedDirs[(int)Direction8.Up] && blockedDirs[(int)Direction8.Right])
                            GroundLayers[0].Tiles[x, y] = new TileAnim(new Loc2D(44, 0), 0);
                        else if (blockedDirs[(int)Direction8.Down] && !blockedDirs[(int)Direction8.Left] && blockedDirs[(int)Direction8.Up] && !blockedDirs[(int)Direction8.Right])
                            GroundLayers[0].Tiles[x, y] = new TileAnim(new Loc2D(45, 0), 0);
                        else if (!blockedDirs[(int)Direction8.Down] && !blockedDirs[(int)Direction8.Left] && !blockedDirs[(int)Direction8.Up] && !blockedDirs[(int)Direction8.Right])
                            GroundLayers[0].Tiles[x, y] = new TileAnim(new Loc2D(46, 0), 0);
                        
                    } else {
                        MapArray[x, y] = new Tile(PMDToolkit.Enums.TileType.Walkable, 0, 0, 0);
                        GroundLayers[0].Tiles[x, y] = new TileAnim(new Loc2D(47, 0), 0);
                    }
                }
            }

            GenItems();
            SpawnNpcs();
        }


        void addSEpos(Loc2D point, bool isStart) {

            int x = 0, y = 0, u = 0, v = 0;
            bool end = !isStart;

            x = Rooms[point.X, point.Y].StartX;
            y = Rooms[point.X, point.Y].StartY;
            u = Rooms[point.X, point.Y].EndX;
            v = Rooms[point.X, point.Y].EndY;

            int randx = 0, randy = 0;
            for (int i = 0; i < 200; i++) {
                randx = rand.Next(x, u + 1);
                randy = rand.Next(y, v + 1);

                if (GridArray[randx, randy] == GridType.Walkable) {
                    if (end) {
                        BorderPoints[1] = new Loc2D(randx, randy);
                        GridArray[randx, randy] = GridType.End;
                    } else {
                        BorderPoints[0] = new Loc2D(randx, randy);
                    }
                    return;
                }
            }
            while (true) {//backup plan in case rooms are so small that all there's left is ","
                randx = rand.Next(x, u + 1);
                randy = rand.Next(y, v + 1);

                if (GridArray[randx, randy] == GridType.Hall || GridArray[randx, randy] == GridType.Walkable) {
                    if (end) {
                        BorderPoints[1] = new Loc2D(randx, randy);
                        GridArray[randx, randy] = GridType.End;
                    } else {
                        BorderPoints[0] = new Loc2D(randx, randy);
                    }
                    return;
                }
            }

        }


        void createRoom(int roomX, int roomY) {
            if (Rooms[roomX, roomY].StartX > -1) {
                return;
            }

            int x = 0, y = 0, u, v, w, l; // variables used for position
            convertRoomToXY(roomX, roomY, ref x, ref y);

            //Determine room length/width
            if (Rooms[roomX, roomY].Opened == DungeonArrayRoom.RoomState.Hall) {
                w = 1;
                l = 1;
            } else {

                w = rand.Next(entry.FloorSettings["RoomWidthMin"], entry.FloorSettings["RoomWidthMax"] + 1);
                l = rand.Next(entry.FloorSettings["RoomLengthMin"], entry.FloorSettings["RoomLengthMax"] + 1);
                if (w < 1) w = 2;
                if (l < 1) l = 2;
                if (w > entry.FloorSettings["CellWidth"]) w = entry.FloorSettings["CellWidth"];
                if (l > entry.FloorSettings["CellHeight"]) l = entry.FloorSettings["CellHeight"];
            }


            //move X and Y to a random starting point that still would include the original x/y; exceptional case for l/w under or equal to 6
            x += rand.Next(0, (entry.FloorSettings["CellWidth"] - (w - 1)));
            y += rand.Next(0, (entry.FloorSettings["CellHeight"] - (l - 1)));


            if (x < 1) x = 1;
            if ((x + (w - 1)) > Width - 2) x = (Width - 2 - (w - 1));

            if (y < 1) y = 1;
            if ((y + (l - 1)) > Height - 2) y = (Height - 2 - (l - 1));

            // once we have our room coords, render it on the map
            u = x + (w - 1);
            v = y + (l - 1);


            Rooms[roomX, roomY].StartX = x;
            Rooms[roomX, roomY].StartY = y;
            Rooms[roomX, roomY].EndX = u;
            Rooms[roomX, roomY].EndY = v;

            // done 
        }


        void drawRoom(int roomX, int roomY) {
            int x, y, u, v;

            x = Rooms[roomX, roomY].StartX;
            y = Rooms[roomX, roomY].StartY;

            u = Rooms[roomX, roomY].EndX;
            v = Rooms[roomX, roomY].EndY;

            GridType drawnTile = GridType.Walkable;

            if (x == u || y == v) drawnTile = GridType.Hall;

            for (int i = x; i <= u; i++) {
                for (int j = y; j <= v; j++) {
                    GridArray[i, j] = drawnTile;
                }
            }

        }

        void padSingleRoom(int roomX, int roomY) {
            int x, y, u, v;

            x = Rooms[roomX, roomY].StartX;
            y = Rooms[roomX, roomY].StartY;

            u = Rooms[roomX, roomY].EndX;
            v = Rooms[roomX, roomY].EndY;


            if (x == u) {
                if (y - 1 >= 0 && GridArray[x, y - 1] == GridType.Walkable) {
                    GridArray[x, y - 1] = GridType.Door;
                }
                if (v + 1 < Height - 1 && GridArray[x, v + 1] == GridType.Walkable) {
                    GridArray[x, v + 1] = GridType.Door;
                }
            }

            if (y == v) {
                if (x - 1 >= 0 && GridArray[x - 1, y] == GridType.Walkable) {
                    GridArray[x - 1, y] = GridType.Door;
                }
                if (u + 1 < Width - 1 && GridArray[u + 1, y] == GridType.Walkable) {
                    GridArray[u + 1, y] = GridType.Door;
                }
            }

        }

        void createHHall(int hallX, int hallY) {
            DungeonArrayRoom startRoom = Rooms[hallX, hallY];
            DungeonArrayRoom endRoom = Rooms[hallX + 1, hallY];

            int distance = endRoom.StartX - startRoom.EndX;

            if (distance < 1) {
                if (startRoom.StartY > endRoom.EndY || startRoom.EndY < endRoom.StartY) {
                    distance++;
                } else {
                    return;
                }
            }
            distance--;

            int x, y, m, h, r = 0, var;

            x = startRoom.EndX;

            m = rand.Next(entry.FloorSettings["HallTurnMin"], entry.FloorSettings["HallTurnMax"] + 1); // the number of vertical pieces in the hall


            if (m > ((distance - 1) / 2)) m = (distance - 1) / 2; //reduces the number of hall turns to something the length of the hall can accept


            y = rand.Next(startRoom.StartY, startRoom.EndY + 1); //picks a Y coordinate to start at


            HHalls[hallX, hallY].TurnPoints.Add(new Loc2D(x, y));

            if (m <= 0 && (y > endRoom.EndY || y < endRoom.StartY)) {//checks if at least one turn is needed to make rooms meet
                m = 1;
            }


            for (int i = 0; i <= m; i++) {
                if (i == m) {
                    var = rand.Next(endRoom.StartY, endRoom.EndY + 1) - y;
                } else {
                    var = rand.Next(entry.FloorSettings["HallVarMin"], entry.FloorSettings["HallVarMax"] + 1);
                    if (rand.Next(0, 2) == 0) {
                        var = -var;
                    }
                }
                if (i != 0) {
                    if ((y + var) < 1) var = 1 - y;
                    if ((y + var) > Height - 2) var = Height - 2 - y;
                    
                    y += var;
                    HHalls[hallX, hallY].TurnPoints.Add(new Loc2D(x, y));
                }

                if (distance >= 0) {
                    h = (r + distance) / (m + 1);
                    r = (r + distance) % (m + 1);
                } else {
                    h = -(r - distance) / (m + 1);
                    r = (r - distance) % (m + 1);
                }
                //addHorizHall(x, y, h, mapArray);

                x += h;
                HHalls[hallX, hallY].TurnPoints.Add(new Loc2D(x, y));
            }

            HHalls[hallX, hallY].TurnPoints[HHalls[hallX, hallY].TurnPoints.Count - 1] = new Loc2D(x + 1, y);
            //mapArray[y, x + 1] = ',';


            //int x, y, h, j;
            //h = hall % 3;
            //j = hall / 3;

            //x = 8 + 13 * h;
            //y = 3 + 12 * j;

            //addHorizHall(x, y, 6, mapArray);
        }

        void createVHall(int hallX, int hallY) {
            DungeonArrayRoom startRoom = Rooms[hallX, hallY];
            DungeonArrayRoom endRoom = Rooms[hallX, hallY + 1];

            int n = endRoom.StartY - startRoom.EndY; //distance between rooms

            if (n < 1) {
                if (startRoom.StartX > endRoom.EndX || startRoom.EndX < endRoom.StartX) {
                    n++;
                } else {
                    return;
                }
            }
            n--;

            int x, y, m,/* n,*/ h, r = 0, var;

            y = startRoom.EndY;

            m = rand.Next(entry.FloorSettings["HallTurnMin"], entry.FloorSettings["HallTurnMax"] + 1); // the number of horizontal pieces in the hall


            if (m > ((n - 1) / 2)) m = (n - 1) / 2; //reduces the number of hall turns to something the length of the hall can accept


            x = rand.Next(startRoom.StartX, startRoom.EndX + 1); //picks a X coordinate to start at


            VHalls[hallX, hallY].TurnPoints.Add(new Loc2D(x, y));


            if (m <= 0 && (x > endRoom.EndX || x < endRoom.StartX)) {//checks if at least one turn is needed to make rooms meet
                m = 1;
            }


            for (int i = 0; i <= m; i++) {
                if (i == m) {
                    var = rand.Next(endRoom.StartX, endRoom.EndX + 1) - x;
                } else {
                    var = rand.Next(entry.FloorSettings["HallVarMin"], entry.FloorSettings["HallVarMax"] + 1);
                    if (rand.Next(0, 2) == 0) {
                        var = -var;
                    }
                }

                if (i != 0) {
                    if ((x + var) < 1) var = 1 - x;
                    if ((x + var) > Width - 2) var = Width - 2 - x;
                    //addHorizHall(x, y, var, mapArray);

                    x += var;
                    VHalls[hallX, hallY].TurnPoints.Add(new Loc2D(x, y));
                }// else {
                //mapArray[y,x] = ',';
                //}



                if (n >= 0) {
                    h = (r + n) / (m + 1);
                    r = (r + n) % (m + 1);
                } else {
                    h = -(r - n) / (m + 1);
                    r = (r - n) % (m + 1);
                }
                //addVertHall(x, y, h, mapArray);

                y += h;
                VHalls[hallX, hallY].TurnPoints.Add(new Loc2D(x, y));
            }

            VHalls[hallX, hallY].TurnPoints[VHalls[hallX, hallY].TurnPoints.Count - 1] = new Loc2D(x, y + 1);
            //mapArray[y + 1, x] = ',';


            //int x, y, h, j; // variables for position
            //h = hall % 4;
            //j = hall / 4;

            //x = 3 + Convert.ToInt32(13.5 * h);
            //y = 7 + 12 * j;

            //addVertHall(x, y, mapArray);
        }

        void DrawHalls(DungeonArrayHall hall) {
            if (hall.TurnPoints.Count > 0) {
                bool addedEntrance = false;
                DrawHallTile(hall.TurnPoints[0].X, hall.TurnPoints[0].Y, ref addedEntrance);
                for (int i = 0; i < hall.TurnPoints.Count - 1; i++) {
                    DrawHall(hall.TurnPoints[i], hall.TurnPoints[i + 1], ref addedEntrance);
                }
                for (int i = hall.TurnPoints.Count - 1; i > 0; i--) {
                    DrawHall(hall.TurnPoints[i], hall.TurnPoints[i - 1], ref addedEntrance);
                }
            }

        }

        void DrawHall(Loc2D point1, Loc2D point2, ref bool addedEntrance) {
            if (point1.X == point2.X) {
                if (point2.Y > point1.Y) {
                    for (int i = point1.Y; i <= point2.Y; i++) {
                        DrawHallTile(point1.X, i, ref addedEntrance);
                    }
                } else if (point2.Y < point1.Y) {
                    for (int i = point1.Y; i >= point2.Y; i--) {
                        DrawHallTile(point1.X, i, ref addedEntrance);
                    }
                }
            } else if (point1.Y == point2.Y) {
                if (point2.X > point1.X) {
                    for (int i = point1.X; i <= point2.X; i++) {
                        DrawHallTile(i, point1.Y, ref addedEntrance);
                    }
                } else if (point2.X < point1.X) {
                    for (int i = point1.X; i >= point2.X; i--) {
                        DrawHallTile(i, point1.Y, ref addedEntrance);
                    }
                }
            }
        }

        void DrawHallTile(int x, int y, ref bool addedEntrance) {
            if (GridArray[x, y] == GridType.Walkable || GridArray[x, y] == GridType.Door) {
                if (!addedEntrance) {
                    GridArray[x, y] = GridType.Door;
                    addedEntrance = true;
                }

            } else {
                if (GridArray[x, y] != GridType.Hall && GridArray[x, y] != GridType.Door) {
                    GridArray[x, y] = GridType.Hall;
                }
                addedEntrance = false;
            }
        }



        void convertRoomToXY(int roomX, int roomY, ref int x, ref int y) {

            x = 1 + roomX * entry.FloorSettings["CellWidth"];
            y = 1 + roomY * entry.FloorSettings["CellHeight"];
        }


        void openHallBetween(Loc2D room1, Loc2D room2) {

            if (room1.X == room2.X) {
                int d = room2.Y - room1.Y;
                if (d == 1) {
                    VHalls[room1.X, room1.Y].Open = true;
                } else if (d == -1) {
                    VHalls[room2.X, room2.Y].Open = true;
                }
            } else if (room1.Y == room2.Y) {
                int d = room2.X - room1.X;
                if (d == 1) {
                    HHalls[room1.X, room1.Y].Open = true;
                } else if (d == -1) {
                    HHalls[room2.X, room2.Y].Open = true;
                }
            }

            //if (room2 < room1) {
            //    int temp = room1;
            //    room1 = room2;
            //    room2 = temp;
            //}
            //if (room2 - room1 == 1) { //horizontal
            //    x = room2 / 4;
            //    hhall[room1 - x] = DungeonArrayRoom.RoomState.Open;
            //} else { //vertical
            //    vhall[room1] = DungeonArrayRoom.RoomState.Open;
            //}
        }

        static int getRoomUp(int room) {
            if (room < 4)
                return -1;
            return room - 4;
        }

        static int getRoomDown(int room) {
            if (room > 11)
                return -1;
            return room + 4;
        }

        static int getRoomLeft(int room) {
            if (room % 4 == 0)
                return -1;
            return room - 1;
        }

        static int getRoomRight(int room) {
            if (room % 4 == 3)
                return -1;
            return room + 1;
        }


        bool IsBlocked(int x, int y, Direction8 dir) {

            Operations.MoveInDirection8(ref x, ref y, dir, 1);

            if (x < 0 || x > Width - 1 || y < 0 || y > Height - 1) {
                return true;
            }

            if (GridArray[x, y] == GridType.Blocked) {
                return true;
            }

            return false;
        }
        
        public void GenItems() {

            for (int n = 0; n < 10; n++) {
                GenItem();
            }
        }

        public void GenItem() {

            //List<Loc2D> possiblePoints = new List<Loc2D>();
            //for (int i = 0; i < Width; i++) {
            //    for (int j = 0; j < Height; j++) {
            //        if (MapArray[i, j].Data.Type != PMDToolkit.Enums.TileType.Blocked && MapArray[i, j].Data.Type != PMDToolkit.Enums.TileType.Hall && MapArray[i, j].Item.ItemIndex == -1 && MapArray[i, j].TileFoliage == null) {
            //            possiblePoints.Add(new Loc2D(i, j));
            //        }
            //    }
            //}
            //if (possiblePoints.Count > 0) {
            //    Loc2D loc = possiblePoints[rand.Next(possiblePoints.Count)];
            //    MapArray[loc.X, loc.Y].Item = new Item(rand.Next(5), 1);
            //}

        }


        public void SpawnNpcs() {
            //spawn NPCs

            for (int i = 0; i < BasicMap.MAX_NPC_SLOTS; i++) {
                Npcs[i] = new Npc();
            }

            for (int i = 0; i < Rooms.GetLength(0); i++) {
                for (int j = 0; j < Rooms.GetLength(1); j++) {
                    if (Rooms[i, j].Opened == DungeonArrayRoom.RoomState.Open) {
                        int npcsPerRoom = rand.Next(minNpcsPerRoom, maxNpcsPerRoom + 1);
                        for (int n = 0; n < npcsPerRoom; n++) {
                            SpawnNpc(Rooms[i, j]);
                        }
                    }
                }
            }
        }


        public bool SpawnNpc(DungeonArrayRoom room) {

            List<Loc2D> possiblePoints = new List<Loc2D>();
            for (int y = room.StartY; y <= room.EndY; y++) {
                for (int x = room.StartX; x <= room.EndX; x++) {
                    if (MapArray[x, y].Data.Type == PMDToolkit.Enums.TileType.Walkable && StartPoint != new Loc2D(x, y)) {
                        bool placeHere = true;
                        for (int n = 0; n < BasicMap.MAX_NPC_SLOTS; n++) {
                            if (!Npcs[n].dead && Npcs[n].CharLoc == new Loc2D(x, y)) {
                                placeHere = false;
                                break;
                            }
                        }
                        if (placeHere) {
                            possiblePoints.Add(new Loc2D(x, y));
                        }
                    }
                }
            }

            if (possiblePoints.Count > 0) {
                int npcIndex = rand.Next(1, 3);
                Npc npc = new Npc(possiblePoints[rand.Next(possiblePoints.Count)], (Direction8)rand.Next(8), npcIndex);
                AddNpc(npc);
                return true;
            }
            return false;
        }

    }
}
