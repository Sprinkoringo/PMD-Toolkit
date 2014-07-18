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
using PMDToolkit.Maps;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using PMDToolkit.Graphics;
using PMDToolkit.Core;
using System.IO;

namespace PMDToolkit.Logic.Gameplay {
    public static partial class Processor {

        public const int MAX_INV_SLOTS = 8;
        public const int MAX_MOVE_SLOTS = 4;
        public const int MAX_TEAM_SLOTS = 4;

        public static bool Intangible { get; set; }

        public static Player[] Players { get; set; }
        public static Npc[] Npcs { get { return CurrentMap.Npcs; } }
        public static string CurrentMapID { get; set; }
        public static ActiveMap CurrentMap { get { return CurrentMapGroup[CurrentMapID]; } }
        public static Random Rand { get; set; }

        //a distinction must be made between a game command and an interface command
        //key commands: Up, Down, Left, Right, A(Attack), B(Jump), X(Pickup?), Y(Moves?), MouseDown, MouseUp
        public static Input PrevInput { get; set; }
        public static Input CurrentInput { get; set; }
        public static RenderTime InputTime { get; set; }

        static MapGroup CurrentMapGroup;

        public static int Seed { get; set; }
        public static MoveState[] Moves
        {
            get
            {
                return FocusedCharacter.Moves;
            }
        }

        static int money;
        public static int[] Inventory { get; set; }

        public static ActiveChar FocusedCharacter
        {
            get
            {
                return CharOfIndex(FocusedCharIndex);
            }
        }
        public static int FocusedCharIndex;
        static int currentCharIndex;
        static int turnsTaken;
        static int turnCount;
        static bool someoneMoved;

        static List<Command> replayInputs;
        static bool isLogging;

        static bool print;
        static bool allowPrint;

        public static void Init() {
            //clean map pointer
            CurrentMapGroup = null;

            //clean player
            Players = new Player[MAX_TEAM_SLOTS];
            for (int i = 0; i < MAX_TEAM_SLOTS; i++)
            {
                Players[i] = new Player();
            }
            Inventory = new int[MAX_INV_SLOTS];
            for (int i = 0; i < MAX_INV_SLOTS; i++) {
                Inventory[i] = -1;
            }

            PrevInput = new Input();
            CurrentInput = new Input();
            InputTime = RenderTime.Zero;
            replayInputs = new List<Command>();
            Rand = new Random();
            isLogging = true;
            currentCharIndex = -MAX_TEAM_SLOTS;
            CurrentMapID = "";
        }


        public static void Restart() {
            isLogging = true;
            StartMap("", Rand.Next());
        }

        public static void StartDungeon(int seed) {
            //BeginSeed(seed);
            //CurrentMapGroup = Data.GameData.DungeonAlgorithmDex[Data.GameData.RDungeonDex[0].Algorithm].CreateDungeon();
            //CurrentMapGroup.Generate(seed, Data.GameData.RDungeonDex[0]);

            //ResetGameState();
            //Display.Screen.AddResult(new Results.Fade(Display.Screen.FadeType.FadeIn));
        }

        public static void StartMap(string mapName, int seed)
        {
            BeginSeed(seed);
            MapGroup map_group = new BasicMapGroup();
            map_group.Initialize();
            CurrentMapGroup = map_group;
            CurrentMapID = mapName;

            ResetGameState();
            Display.Screen.AddResult(new Results.Fade(Display.Screen.FadeType.FadeIn));
        }

        public static void BeginSeed(int seed)
        {
            Seed = seed;
            Logs.Logger.LogDebug("Seed: " + seed);
            if (isLogging)
                Logs.Logger.BeginJourney(seed);
            Rand = new Random(seed);
        }

        public static void StartReplay(string path) {
            try {
                Display.Screen.AddResult(new Results.Fade(Display.Screen.FadeType.FadeOut));
                isLogging = false;
                replayInputs.Clear();
                using (StreamReader reader = new StreamReader(path, true)) {
                    int seed = reader.ReadLine().ToInt();
                    StartDungeon(seed);
                    while(!reader.EndOfStream){
                        string[] commandString = reader.ReadLine().Split(' ');
                        Command command = new Command((Command.CommandType)commandString[0].ToInt());
                        for (int i = 1; i < commandString.Length; i++) {
                            command.AddArg(commandString[i].ToInt());
                        }
                        replayInputs.Add(command);
                    }
                }
                Display.Screen.AddResult(new Results.Fade(Display.Screen.FadeType.FadeIn));
            } catch (Exception ex) {
                Logs.Logger.LogError(new Exception("Replay Error with " + path + "\n", ex));
            }
            
        }

        public static void MoveToFloor(int exitPoint) {
            //FloorLink landing = CurrentMapGroup.GetFloorLink(CurrentFloor, exitPoint);
            //int floor = landing.FloorNum;
            //int entrance = landing.EntranceIndex;

            //RandomMap map = CurrentMapGroup[floor];

            ////place characters around in order
            //Players[0].CharLoc = map.BorderPoints[entrance];
            //for (int i = 1; i < MAX_TEAM_SLOTS; i++)
            //{
            //    Players[i] = new Player();
            //}
            //CurrentFloor = floor;

            //BeginFloor();
        }

        private static void ResetGameState() {
            //reset player
            for (int i = 0; i < MAX_TEAM_SLOTS; i++)
            {
                Players[i] = new Player();
            }
            Players[0] = new Player(new Loc2D(), Direction8.Down);
            Players[0].CharData.Species = 1;
            //Players[0].Moves[0].MoveNum = 2;
            //Players[0].Moves[1].MoveNum = 3;
            //Players[0].Moves[2].MoveNum = 4;

            //Inventory[0] = 0;
            //Inventory[1] = 1;
            //Inventory[2] = 2;
            //Inventory[3] = 3;
            //Inventory[4] = 4;
            money = 0;

            BeginFloor();
        }

        public static void BeginFloor() {
            Display.Screen.AddResult(new Results.SetMap(CurrentMap, 0));

            for (int i = 0; i < MAX_TEAM_SLOTS; i++)
            {
                Display.Screen.AddResult(new Results.RemoveCharacter(i-MAX_TEAM_SLOTS));
                ActiveChar character = CharOfIndex(i-MAX_TEAM_SLOTS);
                if (!character.dead)
                    Display.Screen.AddResult(new Results.SpawnCharacter(character, i-MAX_TEAM_SLOTS));
            }
            currentCharIndex = -MAX_TEAM_SLOTS;
            SwitchFocus(-MAX_TEAM_SLOTS);

            Display.Screen.AddResult(new Results.BGM(CurrentMap.Music, true));

            //resync NPCs
            for (int i = 0; i < BasicMap.MAX_NPC_SLOTS; i++) {
                Display.Screen.AddResult(new Results.RemoveCharacter(i));
                if (!Npcs[i].dead) {
                    Display.Screen.AddResult(new Results.SpawnCharacter(Npcs[i], i));
                }
            }
            turnCount = 0;
            print = true;
        }


        public static void SwitchFocus(int focus)
        {
            FocusedCharIndex = focus;
            Display.Screen.AddResult(new Results.FocusCharacter(FocusedCharIndex));
            Display.Screen.AddResult(new Results.SetStats(FocusedCharacter.HP, FocusedCharacter.MaxHP, FocusedCharacter.PP, FocusedCharacter.MaxPP, money));
            Display.Screen.AddResult(new Results.CurrentMoves(FocusedCharacter.Moves));
        }

        public static void SetFrameInput(Input input, RenderTime elapsedTime, int ups)
        {
            if (input == CurrentInput)
            {
                InputTime += elapsedTime;
            }
            else
            {
                InputTime = RenderTime.FromMillisecs(0);
            }
            PrevInput = CurrentInput;
            CurrentInput = input;

            Display.Screen.UpdatesPerSecond = ups;
            ProcessMeta();
        }

        public static void Process() {

            if (print && allowPrint) {
                Print();
                print = false;
            }

            bool moveMade = false;

            Display.Screen.BeginConcurrent();
            ProcessPlayerInput(CharOfIndex(currentCharIndex), ref moveMade);
            //if a move was made, everyone else gets a turn
            if (moveMade) {
                someoneMoved = true;

                PassToNextTurn();

                while (true) {
                    //all other characters get a turn, if they have one.
                    ActiveChar character = CharOfIndex(currentCharIndex);
                    if (!character.dead) {
                        if (turnsTaken <= character.TurnCounter) {
                            if (IsPlayerTurn()) {
                                SwitchFocus(currentCharIndex);
                                break;
                            } else {
                                someoneMoved = true;
                                ProcessAIDecision(character);
                            }
                        }
                    }

                    PassToNextTurn();
                }

                print = true;
            }
            Display.Screen.EndConcurrent();
        }


        private static void ProcessMeta() {
            if (CurrentInput.Intangible && !PrevInput.Intangible)
            {
                Intangible = !Intangible;
                Display.Screen.Intangible = Intangible;
            }

            if (CurrentInput.SpeedDown && !PrevInput.SpeedDown)
            {
                if (Display.Screen.DebugSpeed > Display.Screen.GameSpeed.Pause)
                    Display.Screen.DebugSpeed--;
            }

            if (CurrentInput.SpeedUp && !PrevInput.SpeedUp)
            {
                if (Display.Screen.DebugSpeed < Display.Screen.GameSpeed.Instant)
                    Display.Screen.DebugSpeed++;
            }

            if (CurrentInput.ShowDebug && !PrevInput.ShowDebug)
            {
                Display.Screen.ShowDebug = !Display.Screen.ShowDebug;
            }

            if (CurrentInput.Print && !PrevInput.Print)
            {
                allowPrint = !allowPrint;
                Display.Screen.Print = allowPrint;
                if (allowPrint)
                    Print();
                else
                    Console.Clear();
            }

            if (CurrentInput.Restart && !PrevInput.Restart)
            {
                Restart();
            }

            if (CurrentInput.MouseWheel != PrevInput.MouseWheel)
            {
                int diff = CurrentInput.MouseWheel - PrevInput.MouseWheel;
                if (diff > Int32.MaxValue / 2)
                    diff = (PrevInput.MouseWheel - Int32.MinValue) + (Int32.MaxValue - CurrentInput.MouseWheel);
                else if (diff < Int32.MinValue / 2)
                    diff = (CurrentInput.MouseWheel - Int32.MinValue) + (Int32.MaxValue - PrevInput.MouseWheel);

                Display.Screen.Zoom -= diff;
                if (Display.Screen.Zoom < Display.Screen.GameZoom.x8Near)
                    Display.Screen.Zoom = Display.Screen.GameZoom.x8Near;
                if (Display.Screen.Zoom > Display.Screen.GameZoom.x16Far)
                    Display.Screen.Zoom = Display.Screen.GameZoom.x16Far;
            }

            if (Editors.MapEditor.mapEditing)
            {

                if (!CurrentInput.RightMouse && PrevInput.RightMouse && CurrentInput.Shift)
                {
                    Loc2D coords = Display.Screen.ScreenCoordsToMapCoords(CurrentInput.MouseLoc);
                    if (Operations.IsInBound(CurrentMap.Width, CurrentMap.Height, coords.X, coords.Y))
                    {
                        FocusedCharacter.CharLoc = coords;
                        Display.Screen.AddResult(new Results.Loc(FocusedCharIndex, FocusedCharacter.CharLoc));
                    }
                }
                else if (Editors.MapEditor.chosenEditMode == Editors.MapEditor.TileEditMode.Draw)
                {
                    if (CurrentInput.LeftMouse)
                    {
                        Editors.MapEditor.PaintTile(Display.Screen.ScreenCoordsToMapCoords(CurrentInput.MouseLoc), Editors.MapEditor.GetBrush());
                    }
                    else if (CurrentInput.RightMouse)
                    {
                        Editors.MapEditor.PaintTile(Display.Screen.ScreenCoordsToMapCoords(CurrentInput.MouseLoc), new TileAnim());
                    }
                }
                else if (Editors.MapEditor.chosenEditMode == Editors.MapEditor.TileEditMode.Eyedrop)
                {
                    if (CurrentInput.LeftMouse)
                    {
                        Editors.MapEditor.EyedropTile(Display.Screen.ScreenCoordsToMapCoords(CurrentInput.MouseLoc));
                    }
                    else if (PrevInput.LeftMouse)
                    {

                    }
                }
                else if (Editors.MapEditor.chosenEditMode == Editors.MapEditor.TileEditMode.Fill)
                {
                    if (!CurrentInput.LeftMouse && PrevInput.LeftMouse)
                    {
                        Editors.MapEditor.FillTile(Display.Screen.ScreenCoordsToMapCoords(CurrentInput.MouseLoc), Editors.MapEditor.GetBrush());
                    }
                    else if (!CurrentInput.RightMouse && PrevInput.RightMouse)
                    {
                        Editors.MapEditor.FillTile(Display.Screen.ScreenCoordsToMapCoords(CurrentInput.MouseLoc), new TileAnim());
                    }
                }
            }
        }


        private static bool IsPlayerTurn() {
            if (currentCharIndex < 0) return true;
            //if (directCommand && charIndex < 0) return true;
            return false;
        }

        public static bool IsGameOver()
        {
            //leader is dead
            if (Players[0].dead) return true;
            return false;
        }

        private static void ProcessPlayerInput(ActiveChar character, ref bool moveMade) {
            if (MenuManager.Menus.Count > 0) {
                MenuManager.ProcessMenus(CurrentInput, character, ref moveMade);
                return;
            } else if (replayInputs.Count > 0 && !isLogging) {
                ProcessDecision(replayInputs[0], character, ref moveMade);
                replayInputs.RemoveAt(0);
                return;
            }
            
            Command command = new Command(Command.CommandType.None);

            bool jump = false;
            bool spell = false;
            bool turn = false;
            bool diagonal = false;
            
#if GAME_MODE
            //menu button presses
            if (CurrentInput[Input.InputType.Enter] && !PrevInput[Input.InputType.Enter]) {
                MenuManager.Menus.Insert(0, new MainMenu());
            } else if (CurrentInput[Input.InputType.Q] && !PrevInput[Input.InputType.Q]) {
                MenuManager.Menus.Insert(0, new ItemMenu());
            } else if (CurrentInput[Input.InputType.W] && !PrevInput[Input.InputType.W]) {
                MenuManager.Menus.Insert(0, new SpellMenu());
            } else
#endif
                //multi-button presses
            if (CurrentInput[Input.InputType.A]) {
                if (CurrentInput[Input.InputType.S] && !PrevInput[Input.InputType.S]) {
                    command = new Logic.Gameplay.Command(Logic.Gameplay.Command.CommandType.Spell, 0);
                } else if (CurrentInput[Input.InputType.D] && !PrevInput[Input.InputType.D]) {
                    command = new Logic.Gameplay.Command(Logic.Gameplay.Command.CommandType.Spell, 1);
                } else if (CurrentInput[Input.InputType.X] && !PrevInput[Input.InputType.X]) {
                    command = new Logic.Gameplay.Command(Logic.Gameplay.Command.CommandType.Spell, 2);
                } else if (CurrentInput[Input.InputType.C] && !PrevInput[Input.InputType.C]) {
                    command = new Logic.Gameplay.Command(Logic.Gameplay.Command.CommandType.Spell, 3);
                } else {
                    //keep move display
                    spell = true;
                    if (CurrentInput.Direction != Direction8.None) {
                        command = new Command(Command.CommandType.Dir);
                        command.AddArg((int)CurrentInput.Direction);
                    }
                }
            } else {
                if (CurrentInput[Input.InputType.Z]) {
                    jump = true;
                }
                if (CurrentInput[Input.InputType.S]) {
                    turn = true;
                }
                if (CurrentInput[Input.InputType.D]) {
                    diagonal = true;
                }

                //single button presses
                if (CurrentInput[Input.InputType.X] && !PrevInput[Input.InputType.X]) {
                    if (jump) {
                        command = new Command(Command.CommandType.AltAttack);
                        command.AddArg((int)character.CharDir);
                        command.AddArg(1);
                    } else {
                        command = new Command(Command.CommandType.Attack);
                    }
                    jump = false;
                    turn = false;
                    diagonal = false;
                } else if (CurrentInput[Input.InputType.C] && !PrevInput[Input.InputType.C]) {
                    if (jump) {
                        command = new Command(Command.CommandType.Wait);
                    } else {
                        command = new Command(Command.CommandType.Pickup);
                    }
                    jump = false;
                    turn = false;
                    diagonal = false;
                }//directions
                else if (CurrentInput.Direction != Direction8.None) {
                    if (Display.Screen.DebugSpeed != Display.Screen.GameSpeed.Instant || PrevInput.Direction == Direction8.None)
                    {
                        Command.CommandType cmdType = Command.CommandType.None;
                        if (Operations.IsDiagonal(CurrentInput.Direction))
                            cmdType = Command.CommandType.Dir;
                        else if (InputTime > RenderTime.FromMillisecs(20) || PrevInput.Direction == Direction8.None)
                            cmdType = Command.CommandType.Dir;
                        if (InputTime > RenderTime.FromMillisecs(60) || Display.Screen.DebugSpeed == Display.Screen.GameSpeed.Instant) cmdType = Command.CommandType.Move;
                        if (jump) cmdType = Command.CommandType.AltAttack;
                        if (turn) cmdType = Command.CommandType.Dir;

                        if (!diagonal || Operations.IsDiagonal(CurrentInput.Direction))
                        {
                            command = new Command(cmdType);
                            command.AddArg((int)CurrentInput.Direction);
                        }
                        if (!turn)
                        {
                            jump = false;
                            diagonal = false;
                        }
                    }
                }
            }

#if GAME_MODE
            Display.Screen.Jump = jump;
            Display.Screen.Spell = spell;
            Display.Screen.Turn = turn;
            Display.Screen.Diagonal = diagonal;
#endif
            ProcessDecision(command, character, ref moveMade);
        }

        //the intention, and its result to that frame
        //"choose the action to partake in"
        public static void ProcessDecision(Command command, ActiveChar character, ref bool moveMade) {
            //translates commands into actions
            if (character == FocusedCharacter && command.Type != Command.CommandType.None && isLogging) {
                Logs.Logger.LogJourney(command);
            }

            switch (command.Type) {
                case Command.CommandType.Dir: {
                        ProcessDir((Direction8)command[0], character, ref moveMade);
                    }
                    break;
                case Command.CommandType.Move:
                    {
                        Display.Screen.BeginRunModeConcurrent(CharIndex(character));
                        //takes a dir argument
                        //Display.Screen.ResultList.Add(new Results.StartTag("Move"));
                        if (command.ArgCount > 0) {
                            ProcessDir((Direction8)command[0], character, ref moveMade);
                        }
                        ProcessWalk(character, ref moveMade);
                        //Display.Screen.ResultList.Add(new Results.EndTag());
                    }
                    break;
                    #if GAME_MODE
                    case Command.CommandType.Attack: {
                        Display.Screen.BeginConcurrent();
                        //takes a dir argument
                        if (command.ArgCount > 0) {
                            ProcessDir((Direction8)command[0], character, ref moveMade);
                        }
                        Attack(character, ref moveMade);
                    }
                    break;
                    case Command.CommandType.Pickup:
                    {
                        Display.Screen.BeginConcurrent();
                        //takes an index argument
                        ProcessPickup(character, ref moveMade);
                    }
                    break;
                    case Command.CommandType.Use:
                    {
                        Display.Screen.BeginConcurrent();
                        //takes an index argument
                        ProcessItemUse(character, command[0], ref moveMade);
                    }
                    break;
                    case Command.CommandType.Throw:
                    {
                        Display.Screen.BeginConcurrent();
                        //takes an index argument
                        ProcessThrow(character, command[0], ref moveMade);
                    }
                    break;
                    case Command.CommandType.Drop:
                    {
                        Display.Screen.BeginConcurrent();
                        //takes an index argument
                        ProcessDrop(character, command[0], ref moveMade);
                    }
                    break;
                    case Command.CommandType.Spell:
                    {
                        Display.Screen.BeginConcurrent();
                        //takes an index argument
                        UseMove(character, command[0], ref moveMade);
                    }
                    break;
#endif
                    case Command.CommandType.Wait:
                    {
                        moveMade = true;
                    }
                    break;
                    default: break;
            }
            if (moveMade) {
                ProcessTurnEnd(character);
            }
        }

        private static void ProcessAIDecision(ActiveChar character) {
            //picks an action
            bool moveMade = false;

            Command command = character.Tactic.Think();

            ProcessDecision(command, character, ref moveMade);

        }
        
        private static void PassToNextTurn() {
            if (IsGameOver())
            {
                Display.Screen.EndConcurrent();
                Display.Screen.AddResult(new Results.Fade(Display.Screen.FadeType.FadeOut));
                Restart();
                return;
            }

            bool lastTurn = (currentCharIndex == BasicMap.MAX_NPC_SLOTS - 1);
            currentCharIndex = (currentCharIndex + MAX_TEAM_SLOTS + 1) % (BasicMap.MAX_NPC_SLOTS + MAX_TEAM_SLOTS) - MAX_TEAM_SLOTS;
            Display.Screen.SwitchConcurrentBranch(currentCharIndex);
            if (lastTurn) {

                if (someoneMoved) {
                    turnsTaken++;
                    someoneMoved = false;
                } else {
                    turnsTaken = 0;
                    ProcessMapTurnEnd();
                }

                ProcessMapRoundEnd();
            }
        }

        private static void ProcessMapRoundEnd() {

            foreach (Player player in Players)
            {
                ProcessRoundEnd(player);
            }

            foreach (Npc npc in Npcs) {
                ProcessRoundEnd(npc);
            }
        }

        private static void ProcessRoundEnd(ActiveChar character) {
            if (!character.dead) {
                if (character.MovementSpeed >= 0) {
                    character.TurnCounter = character.MovementSpeed;
                }
            }
        }

        private static void ProcessMapTurnEnd() {
            foreach (Player player in Players)
            {
                ProcessMapTurnEnd(player);
            }

            foreach (Npc npc in Npcs) {
                ProcessMapTurnEnd(npc);
            }

            if (turnCount % 10 == 0) {
                //Display.Screen.ResultList.Add(new Results.BattleMsg("Hail continues to fall."));
            }


            turnCount++;
        }

        private static void ProcessMapTurnEnd(ActiveChar character) {

            if (!character.dead) {
                if (character.VolatileStatus.ContainsKey("MovementSpeed")) {
                    character.VolatileStatus["MovementSpeed"].Counter--;
                    if (character.VolatileStatus["MovementSpeed"].Counter <= 0) {
                        AddExtraStatus(character, "MovementSpeed", 0, -1, 0);
                    }
                }

                if (character.MovementSpeed < 0) {
                    character.TurnCounter++;
                    if (character.TurnCounter > 0) character.TurnCounter = character.MovementSpeed;
                }

                if (character.Status == Enums.StatusAilment.Burn) {
                    Display.Screen.BeginConcurrent();
                    DamageCharacter(character, 1, false);
                    character.StatusCounter--;
                    if (character.StatusCounter <= 0) {
                        SetStatusAilment(character, Enums.StatusAilment.OK, 0);
                    }
                }
            }
        }

        private static void ProcessTurnEnd(ActiveChar character) {

            if (!character.dead) {

                if (character.Status == Enums.StatusAilment.Poison) {
                    Display.Screen.BeginConcurrent();
                    DeductPP(character, 1);
                    character.StatusCounter--;
                    if (character.StatusCounter <= 0) {
                        SetStatusAilment(character, Enums.StatusAilment.OK, 0);
                    }
                }
            }
        }

        private static void ProcessDir(Direction8 dir, ActiveChar character, ref bool moveMade) {

            if (character.Status == Enums.StatusAilment.Freeze) {
                return;
            }

            character.CharDir = dir;
            Display.Screen.AddResult(new Results.Dir(CharIndex(character), character.CharDir));
        }

        private static void ProcessWalk(ActiveChar character, ref bool moveMade) {

            if (character.dead) return;

            Loc2D loc = character.CharLoc;
            Operations.MoveInDirection8(ref loc, character.CharDir, 1);

            //check for blocking
            if (DirBlocked(character.CharDir, character)) {
                return;
            }



            moveMade = true;

            if (character.Status == Enums.StatusAilment.Freeze) {
                return;
            }

            character.CharLoc = loc;

            Display.Screen.AddResult(new Results.CreateAction(CharIndex(character), character, Display.CharSprite.ActionType.Walk));
            
            Display.Screen.AddResult(new Results.Loc(CharIndex(character), loc));
            
            moveMade = true;

            //if void, add restart
            if (!Operations.IsInBound(CurrentMap.Width, CurrentMap.Height, character.CharLoc.X, character.CharLoc.Y)) {
                //Lose();
                throw new Exception("Player out of bounds");
            } else {
                //god mode check
                if (Intangible)
                    return;
                
                Tile tile = CurrentMap.MapArray[character.CharLoc.X, character.CharLoc.Y];

                //if landed on certain tiles, present effects
                if (tile.Data.Type == Enums.TileType.Slippery)
                {
                    //if on ice, continue to slide
                    //but only if we're not blocked
                    if (!DirBlocked(character.CharDir, character))
                    {
                        ProcessWalk(character, ref moveMade);
                    }
                }
                
            }
        }

        private static void ProcessPickup(ActiveChar character, ref bool moveMade) {
            Tile tile = CurrentMap.MapArray[character.CharLoc.X, character.CharLoc.Y];

            int freeSlot = FindInvSlot();

            if (tile.Data.Type == Enums.TileType.ChangeFloor) {
                //if we're at the stairs, we go on
                //a case for changing floor; leader only!
                if (character == Players[0]) {
                    Display.Screen.AddResult(new Results.SE("magic135"));
                    //send changefloor result
                    Display.Screen.AddResult(new Results.Fade(Display.Screen.FadeType.FadeOut));
                    Display.Screen.AddResult(new Results.BattleMsg("Advanced to the next floor!"));
                    //character.CharLoc = new Loc2D(tile.Data2, tile.Data3);
                    //CurrentFloor = tile.Data1;
                    MoveToFloor(tile.Data.Data1);
                    Display.Screen.AddResult(new Results.Fade(Display.Screen.FadeType.FadeIn));
                }
            }

            int itemSlot = CurrentMap.GetItem(character.CharLoc);
            if (itemSlot == -1)
            {
                Display.Screen.AddResult(new Results.BattleMsg("Nothing there."));
                return;
            }
            
            if (freeSlot == -1) {
                Display.Screen.AddResult(new Results.BattleMsg("Inv full"));
                return;
            }
            moveMade = true;
            int itemIndex = CurrentMap.Items[itemSlot].ItemIndex;
            Inventory[freeSlot] = itemIndex;
            CurrentMap.Items[itemSlot] = new Item();
            
            Display.Screen.AddResult(new Results.SE("magic130"));
            Display.Screen.AddResult(new Results.RemoveItem(itemSlot));

            Display.Screen.AddResult(new Results.BattleMsg(character.Name + " picked up a " + Data.GameData.ItemDex[itemIndex].Name + "."));
            
        }

        private static void ProcessDrop(ActiveChar character, int invSlot, ref bool moveMade) {
            if (!CanDrop(invSlot)) {
                Display.Screen.AddResult(new Results.BattleMsg("Can't drop slot " + (invSlot + 1)));
                return;
            }
            Loc2D loc = character.CharLoc;
            if (!CanItemLand(loc)) {
                Display.Screen.AddResult(new Results.BattleMsg("Can't drop here!"));
                return;
            }
            moveMade = true;
            int itemIndex = Inventory[invSlot];


            int mapSlot = CurrentMap.AddItem(new Item(itemIndex, 1, "", false, loc));

            Inventory[invSlot] = -1;
            
            Display.Screen.AddResult(new Results.SE("magic693"));
            Display.Screen.AddResult(new Results.AddItem(CurrentMap, mapSlot));

            Display.Screen.AddResult(new Results.BattleMsg(character.Name + " dropped a " + Data.GameData.ItemDex[itemIndex].Name + "."));
        }

        private static int FindInvSlot() {
            for (int i = 0; i < MAX_INV_SLOTS; i++) {
                if (Inventory[i] == -1) return i;
            }

            return -1;
        }

        private static bool CanDrop(int invSlot) {
            if (Inventory[invSlot] == -1) return false;
            return true;
        }

        private static bool CanUse(int invSlot) {
            if (Inventory[invSlot] == -1) return false;
            return true;
        }

        private static bool CanThrow(int invSlot) {
            if (Inventory[invSlot] == -1) return false;
            return true;
        }

        public static bool DirBlocked(Direction8 dir, ActiveChar character) {
            return DirBlocked(dir, character, false);
        }

        public static bool DirBlocked(Direction8 dir, ActiveChar character, bool inAir) {
            return DirBlocked(dir, character, inAir, 1);
        }

        public static bool DirBlocked(Direction8 dir, ActiveChar character, bool inAir, int distance) {
            return DirBlocked(dir, character, character.CharLoc, inAir, distance);
        }

        public static bool DirBlocked(Direction8 dir, ActiveChar character, Loc2D loc, bool inAir, int distance) {
            if (character == FocusedCharacter && Intangible)
            {
                Operations.MoveInDirection8(ref loc, dir, 1);
                if (!Operations.IsInBound(CurrentMap.Width, CurrentMap.Height, loc.X, loc.Y))
                    return true;

                return false;
            }

            Enums.WalkMode walkMode = Enums.WalkMode.Normal;
            
            if (inAir)
                walkMode = Enums.WalkMode.Air;

            for (int i = 0; i < distance; i++) {

                Operations.MoveInDirection8(ref loc, dir, 1);

                if (IsBlocked(loc, walkMode))
                    return true;
            }

            return false;

        }

        public static bool IsBlocked(Loc2D loc) {
            return IsBlocked(loc, Enums.WalkMode.Normal);
        }

        public static bool IsBlocked(Loc2D loc, Enums.WalkMode walkMode) {
            //jumping ignores all short obstacles

            if (TileBlocked(loc, walkMode)) return true;

            if (walkMode < Enums.WalkMode.Air) {
                foreach (Player player in Players)
                {
                    if (!player.dead && player.CharLoc == loc)
                        return true;
                }
                foreach (Npc npc in Npcs) {
                    if (!npc.dead && npc.CharLoc == loc)
                        return true;
                }
            }
            
            //map object blocking

            return false;
        }



        public static bool MoveBlocked(Loc2D loc, Direction8 dir) {
            Enums.WalkMode walkMode = Enums.WalkMode.Air;

            Operations.MoveInDirection8(ref loc, dir, 1);

            if (TileBlocked(loc, walkMode))
                return true;

            return false;

        }

        public static bool CanItemLand(Loc2D loc) {
            if (TileBlocked(loc))
                return false;

            return (CurrentMap.GetItem(loc) == -1);
        }
        
        public static bool TileBlocked(Loc2D loc) {
            return TileBlocked(loc, Enums.WalkMode.Normal);
        }
        
        public static bool TileBlocked(Loc2D loc, Enums.WalkMode walkMode) {
            if (!Operations.IsInBound(CurrentMap.Width, CurrentMap.Height, loc.X, loc.Y)) {
                return true;
            }
            if (CurrentMap.MapArray[loc.X, loc.Y].Data.Type == Enums.TileType.Blocked) {
                return true;
            }
            if (CurrentMap.MapArray[loc.X, loc.Y].Data.Type == Enums.TileType.Water) {
                return (walkMode > 0);
            }
            return false;
        }

        public static int CharIndex(ActiveChar character) {
            if (character is Player) return (Array.IndexOf(Players, character) - MAX_TEAM_SLOTS);
            return Array.IndexOf(Npcs, character);
        }

        public static ActiveChar CharOfIndex(int charIndex) {
            if (charIndex < 0)
                return Players[charIndex + MAX_TEAM_SLOTS];
            return Npcs[charIndex];
        }

        public static void ChangeAppearance(ActiveChar character, FormData data)
        {
            character.CharData = data;
            Display.Screen.AddResult(new Results.SpawnCharacter(character, CharIndex(character)));
        }


        public static void Print()
        {
            if (CurrentMapGroup != null)
            {
                int oldLeft = Console.CursorLeft;
                int oldTop = Console.CursorTop;
                Console.SetCursorPosition(0, 0);
                string topString = "";
                string turnString = "Turn #" + (turnCount + 1);
                topString += String.Format("{0,-82}", turnString);
                topString += '\n';
                for (int i = 0; i < 32 + CurrentMap.Width + 1; i++)
                {
                    topString += "=";
                }
                topString += '\n';
                string statString = "";
                statString += "                                \n";
                statString += "Inventory:                      \n";
                statString += "--------------------------------\n";
                for (int i = 0; i < MAX_INV_SLOTS; i++)
                {
                    string invString = (i + 1).ToString("D2") + ": ";
                    if (Inventory[i] > -1)
                    {
                        invString += "#" + Inventory[i] + ") " + Data.GameData.ItemDex[Inventory[i]].Name;
                    }
                    else
                    {
                        invString += "Empty";
                    }
                    statString += String.Format("{0,-32}", invString);
                    statString += '\n';
                }
                statString += "--------------------------------\n";
                statString += "                                \n";
                statString += "Specials:                       \n";
                statString += "--------------------------------\n";
                for (int i = 0; i < MAX_MOVE_SLOTS; i++)
                {
                    string moveString = (i + 1).ToString("D2") + ": ";
                    if (i >= MAX_MOVE_SLOTS)
                    {
                        moveString = "";
                    }
                    else if (Moves[i].MoveNum > -1)
                    {
                        moveString += "#" + Moves[i] + ") " + Data.GameData.MoveDex[Moves[i].MoveNum].Name;
                    }
                    else
                    {
                        moveString += "Empty";
                    }
                    statString += String.Format("{0,-32}", moveString);
                    statString += '\n';
                }

                string mapString = "";
                for (int y = 0; y < CurrentMap.Height; y++)
                {
                    for (int x = 0; x < CurrentMap.Width; x++)
                    {
                        Loc2D loc = new Loc2D(x, y);
                        int ind = -1;
                        for (int i = 0; i < Npcs.Length; i++)
                        {
                            if (!Npcs[i].dead && Npcs[i].CharLoc == loc)
                            {
                                ind = i;
                                break;
                            }
                        }
                        bool containsPlayer = false;
                        foreach(Player player in Players)
                        {
                            if (player.CharLoc == loc)
                            {
                                containsPlayer = true;
                                break;
                            }
                        }
                        if (containsPlayer)
                        {
                            mapString += '@';
                        }
                        else if (ind > -1)
                        {
                            char npcChar = (char)((int)'A' + ind);
                            mapString += npcChar;
                        }
                        else
                        {
                            if (TileBlocked(new Loc2D(x, y)))
                            {
                                mapString += ' ';
                            }
                        }
                    }
                    mapString += "|\n";
                }
                string finalString = topString + CombineStrings(mapString, statString);
                List<string> recentMsgs = Logs.Logger.GetRecentBattleLog(6);
                for (int i = 0; i < recentMsgs.Count; i++)
                {
                    finalString += String.Format("{0,-64}", recentMsgs[i]);
                    finalString += '\n';
                }
                Console.Write(finalString);
                Console.SetCursorPosition(oldLeft, oldTop);
            }
        }

        private static string CombineStrings(string leftString, string rightString)
        {
            string[] stringArray1 = leftString.Split('\n');
            string[] stringArray2 = rightString.Split('\n');
            string returnString = "";
            for (int i = 0; i < stringArray1.Length; i++)
            {
                returnString += stringArray1[i];
                if (i < stringArray2.Length)
                {
                    returnString += stringArray2[i];
                }
                returnString += '\n';
            }
            return returnString;
        }


    }
}
