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
using PMDToolkit.Core;
using PMDToolkit.Graphics;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using DragonOgg.MediaPlayer;

using OpenTK.Audio;
using OpenTK.Audio.OpenAL;

namespace PMDToolkit.Logic.Display {
    public static class Screen {

        public enum EffectPriority {
            Ground = -2,
            Back = -1,
            None = 0,
            Top = 1,
            Overlay = 2
        }

        public enum FadeType {
            None,
            FadeIn,
            FadeOut
        }

        public enum GameSpeed
        {
            Pause = -4,
            Eighth = -3,
            Fourth = -2,
            Half = -1,
            Normal = 0,
            Double = 1,
            Quadruple = 2,
            Octuple = 3,
            Instant = 4
        }

        public enum GameZoom
        {
            x8Near = -3,
            x4Near = -2,
            x2Near = -1,
            x1 = 0,
            x2Far = 1,
            x4Far = 2,
            x8Far = 3,
            x16Far = 4
        }

        public static ulong TotalTick { get; set; }

        public static int UpdatesPerSecond { get; set; }
        public static GameSpeed DebugSpeed { get; set; }
        public static bool ShowDebug { get; set; }
        public static bool ShowGrid { get; set; }
        public static bool ShowCoords { get; set; }
        public static bool Intangible { get; set; }
        public static bool Print { get; set; }
        public static bool ShowSprites { get; set; }
        public static GameZoom Zoom { get; set; }

        private static Results.IResultContainer currentContainer;
        private static Queue<Results.IResultContainer> resultContainers { get; set; }
        private static Results.IResultContainer outContainer;

        private static List<Results.IResult> miscResults { get; set; }

        public static readonly RenderTime TOTAL_FADE_TIME = RenderTime.FromMillisecs(1000);
        public static RenderTime FadeTime { get; set; }
        public static FadeType CurrentFade { get; set; }

        public static string Song { get; set; }
        public static string NextSong { get; set; }
        public static RenderTime MusicFadeTime;
        public static readonly RenderTime MUSIC_FADE_TOTAL = RenderTime.FromMillisecs(2000);

        public static DisplayMap Map { get; set; }
        public static int Floor { get; set; }

        //game elements
        public static CharSprite FocusedCharacter { 
            get {
                if (FocusedIndex < 0)
                    return Players[FocusedIndex + Gameplay.Processor.MAX_TEAM_SLOTS];
                else
                    return Npcs[FocusedIndex];
                }
        }
        public static PlayerSprite[] Players { get; set; }
        public static NpcSprite[] Npcs { get; set; }
        public static ItemAnim[] Items { get; set; }

        public static Dictionary<EffectPriority, List<ISprite>> Effects { get; set; }
        public static List<IEmitter> Emitters { get; set; }

        public static int FocusedIndex { get; set; }
        public static Loc2D CamOffset { get; set; }

        //UI elements
        public static int[] CurrentCharMoves;

        public static int HPMax { get; set; }
        public static int HP { get; set; }
        public static int PPMax { get; set; }
        public static int PP { get; set; }
        public static int Gold { get; set; }
                
        public static bool Diagonal { get; set; }
        public static bool Turn { get; set; }
        public static bool Jump { get; set; }
        public static bool Spell { get; set; }

        public static Random Rand { get; set; }
        
        public static void Init() {
            miscResults = new List<Logic.Results.IResult>();
            resultContainers = new Queue<Results.IResultContainer>();
            outContainer = new Results.ResultContainer();
            Map = new DisplayMap();
            Players = new PlayerSprite[Gameplay.Processor.MAX_TEAM_SLOTS];
            for (int i = 0; i < Gameplay.Processor.MAX_TEAM_SLOTS; i++)
            {
                Players[i] = new PlayerSprite();
            }
            Npcs = new NpcSprite[BasicMap.MAX_NPC_SLOTS];
            for (int i = 0; i < BasicMap.MAX_NPC_SLOTS; i++)
            {
                Npcs[i] = new NpcSprite();
            }
            Items = new ItemAnim[BasicMap.MAX_ITEM_SLOTS];
            for (int i = 0; i < BasicMap.MAX_ITEM_SLOTS; i++)
            {
                Items[i] = new ItemAnim();
            }
            Effects = new Dictionary<EffectPriority, List<ISprite>>();
            for (int n = (int)EffectPriority.Ground; n <= (int)EffectPriority.Overlay; n++) {
                Effects.Add((EffectPriority)n, new List<ISprite>());
            }
            Emitters = new List<IEmitter>();
            CurrentCharMoves = new int[Gameplay.Processor.MAX_MOVE_SLOTS];
            for (int i = 0; i < Gameplay.Processor.MAX_MOVE_SLOTS; i++) {
                CurrentCharMoves[i] = -1;
            }
            Rand = new Random();
            ShowDebug = true;
            //ShowGrid = true;
            ShowCoords = true;
            ShowSprites = true;
            Zoom = GameZoom.x1;
            //DebugSpeed = GameSpeed.Pause;
        }


        public static void AddResult(Results.IResult result)
        {
            if (currentContainer != null)
            {
                currentContainer.AddResult(result);
            }
            else
            {
                Results.ResultContainer container = new Results.ResultContainer();
                container.AddResult(result);
                resultContainers.Enqueue(container);
            }
        }

        public static void EndConcurrent()
        {
            if (currentContainer != null)
            {
                if (!currentContainer.Empty)
                {
                    resultContainers.Enqueue(currentContainer);
                }
                currentContainer = null;
            }
        }

        public static void BeginConcurrent()
        {
            EndConcurrent();
            currentContainer = new Results.ResultContainer();
        }

        public static void BeginRunModeConcurrent(int id)
        {
            //do not end concurrent if currently in run mode
            //and run mode is ordered
            if (!(currentContainer is Results.MotionResultContainer))
            {
                EndConcurrent();
                currentContainer = new Results.MotionResultContainer(id);
            }
        }

        public static void SwitchConcurrentBranch(int id)
        {
            //do this only if in run mode
            Results.MotionResultContainer container = currentContainer as Results.MotionResultContainer;
            if (container != null)
            {
                container.OpenBranch(id);
            }
        }

        public static void SwitchConcurrentBranch()
        {
            //do this only if not in run mode
            Results.ResultContainer container = currentContainer as Results.ResultContainer;
            if (container != null)
            {
                if (!container.IsBranchEmpty())
                    container.OpenNewBranch();
            }
            else
            {
                throw new Exception("Bad Concurrency switch");
            }
        }

        public static void Process(RenderTime elapsedTime)
        {

            if (DebugSpeed == GameSpeed.Pause)
            {
                return;
            }
            else if (DebugSpeed == GameSpeed.Instant)
            {
                ForceReady();
                ProcessActions(elapsedTime);
            }
            else
            {
                int speedFactor = 1000;
                
                speedFactor = (int)(speedFactor * Math.Pow(2, (int)DebugSpeed));

                RenderTime newElapsed = elapsedTime * speedFactor / 1000;
                ProcessActions(newElapsed);

            }
            
            //if actions are ready for queue, get a new result
            ProcessTaskQueue(true);

            //update actions at 0 time
            ProcessActions(new RenderTime());
        }

        public static void ForceReady()
        {

            while (!outContainer.IsFinished() || resultContainers.Count > 0)
            {
                ProcessActions(RenderTime.FromMillisecs(1000));
                ProcessTaskQueue(false);
            }
        }

        public static void ProcessActions(RenderTime elapsedTime) {

            TotalTick += (ulong)elapsedTime.Ticks;

            outContainer.ProcessDelay(elapsedTime);

            //update music
            if (NextSong != null) {
                MusicFadeTime -= elapsedTime;
                if (MusicFadeTime.Ticks <= 0) {
                    AudioManager.BGM.Stop();
                    if (System.IO.File.Exists(NextSong))
                    {
                        Song = NextSong;
                        AudioManager.BGM.SetBGM(Song);
                        AudioManager.BGM.Play();
                        NextSong = null;
                    }
                    else
                    {
                        Song = "";
                    }
                } else {
                    AudioManager.BGM.SetVolume((float)MusicFadeTime.Ticks / (float)MUSIC_FADE_TOTAL.Ticks);
                }
            }

            //update fade
            if (CurrentFade != FadeType.None) {
                FadeTime -= elapsedTime;
                if (FadeTime.Ticks <= 0) {
                    CurrentFade = FadeType.None;
                }
            }

            //update the player
            foreach (PlayerSprite player in Players)
            {
                player.Process(elapsedTime);
            }

            //update Items
            foreach (ItemAnim item in Items)
            {
                item.Process(elapsedTime);
            }

            //update Npcs
            foreach (NpcSprite npc in Npcs) {
                npc.Process(elapsedTime);
            }

            for (int n = (int)EffectPriority.Ground; n <= (int)EffectPriority.Overlay; n++) {
                for (int i = Effects[(EffectPriority)n].Count - 1; i >= 0; i--) {
                    Effects[(EffectPriority)n][i].Process(elapsedTime);
                    if (Effects[(EffectPriority)n][i].ActionDone) Effects[(EffectPriority)n].RemoveAt(i);
                }
            }

            for (int i = Emitters.Count - 1; i >= 0; i--) {
                Emitters[i].Process(elapsedTime);
                if (Emitters[i].ActionDone) Emitters.RemoveAt(i);
            }


            //update the camera, reliant on the player
            CamOffset = FocusedCharacter.TileOffset;
            
        }

        public static void ProcessTaskQueue(bool askUp)
        {
            //unqueue all loose tasks
            DelegateResultContainers();

            //if no tasks are queued, look for more
            if (outContainer.IsFinished())
            {
                //if no choices are queued
                if (resultContainers.Count == 0 && askUp)
                    Logic.Gameplay.Processor.Process();

                //then do actual choice queue processing (if choices are now queued)
                if (resultContainers.Count > 0)
                    outContainer = resultContainers.Dequeue();
            }

            DelegateResultContainers();
        }

        //continually dequeues results from outContainer
        //and dequeues containers out from resultContainers
        //until arrived at a point where it must wait
        private static void DelegateResultContainers()
        {
            if (!outContainer.IsFinished())
            {
                while (true)
                {
                    //push in as many tasks as possible
                    DelegateOutContainer();

                    if (!outContainer.IsFinished())
                    {
                        //if, after dequeueing as much as possible, there are still elements left, wait until next loop to try again
                        break;
                    }

                    //in the end, due to all elements potentially being zero-frame results,
                    //outContainer may be empty with the screen still ReadyForResults, and require another enqueue
                    if (resultContainers.Count > 0)
                        outContainer = resultContainers.Dequeue();

                    //after re-enqueue, the outContainer may or may not be empty now
                    //if so, it means the choice queue has exhausted all and must wait until the next call
                    //if not, we can try again to see if the next outContainer also contains only zero-frame results
                    if (outContainer.IsFinished())
                        break;
                }
            }
        }

        private static void DelegateOutContainer()
        {
            bool dequeuedSomething = true;
            while (dequeuedSomething)
            {
                dequeuedSomething = false;
                //then do actual task processing (if tasks are now queued)
                foreach (Results.ResultBranch branch in outContainer.GetAllBranches())
                {
                    //find the next element for each branch and process it
                    if (!branch.IsEmpty() && branch.IsReady())
                    {
                        Results.IResult result = branch.Results.Dequeue();
                        result.Execute();
                        branch.Delay = result.Delay;
                        dequeuedSomething = true;
                    }
                }
            }
        }

        public static void Draw(int fps) {

            //draw menus on top
            //if (ShowTitle) {
            //    Graphics.TextureManager.TextureProgram.SetModelView(Matrix4.Identity);
            //    Graphics.TextureManager.TextureProgram.LeftMultModelView(
            //        Matrix4.CreateTranslation(TextureManager.SCREEN_WIDTH / 2 - TextureManager.Title.ImageWidth / 2,
            //        TextureManager.SCREEN_HEIGHT / 2 - TextureManager.Title.ImageHeight / 2, 1));
            //    Graphics.TextureManager.TextureProgram.UpdateModelView();
            //    Graphics.TextureManager.Title.Render(null);

            //    Graphics.TextureManager.TextureProgram.SetModelView(Matrix4.Identity);
            //    Gameplay.MenuManager.DrawMenus();

            //}
            float scale = GetZoomScale(Zoom);
            Loc2D camCenter = new Loc2D(FocusedCharacter.CharLoc.X * TextureManager.TILE_SIZE + TextureManager.TILE_SIZE / 2,
                FocusedCharacter.CharLoc.Y * TextureManager.TILE_SIZE + TextureManager.TILE_SIZE / 2);
            camCenter = camCenter + CamOffset;

            Loc2D camStart = new Loc2D((int)(camCenter.X - TextureManager.SCREEN_WIDTH / scale / 2), (int)(camCenter.Y - TextureManager.SCREEN_HEIGHT / scale / 2));
            Loc2D camEnd = new Loc2D((int)(camCenter.X + TextureManager.SCREEN_WIDTH / scale / 2), (int)(camCenter.Y + TextureManager.SCREEN_HEIGHT / scale / 2));
            Loc2D camStartTile = new Loc2D(camStart.X / TextureManager.TILE_SIZE - 1, camStart.Y / TextureManager.TILE_SIZE - 1);
            Loc2D camEndTile = new Loc2D(camEnd.X / TextureManager.TILE_SIZE + 1, camEnd.Y / TextureManager.TILE_SIZE + 1);

            //TextureManager.TextureProgram.SetModelView(Matrix4.CreateTranslation(TextureManager.SCREEN_WIDTH / 2, TextureManager.SCREEN_HEIGHT / 2, 0));
            //TextureManager.TextureProgram.SetModelView(Matrix4.CreateTranslation(-camCenter.X * scale, -camCenter.Y * scale, 0));
            TextureManager.TextureProgram.SetModelView(Matrix4.CreateTranslation(-camCenter.X * scale + TextureManager.SCREEN_WIDTH / 2, -camCenter.Y * scale + TextureManager.SCREEN_HEIGHT / 2, 0));
            TextureManager.TextureProgram.LeftMultModelView(Matrix4.CreateScale(scale));
            //draw the background

            TextureManager.TextureProgram.PushModelView();
            TextureManager.TextureProgram.SetTextureColor(new Color4(128, 128, 128, 255));
            TextureManager.TextureProgram.LeftMultModelView(Matrix4.CreateScale(Math.Min(Map.Width, camEndTile.X) * TextureManager.TILE_SIZE, Math.Min(Map.Height, camEndTile.Y) * TextureManager.TILE_SIZE, 1));
            TextureManager.TextureProgram.UpdateModelView();
            TextureManager.BlankTexture.Render(null);
            TextureManager.TextureProgram.SetTextureColor(Color4.White);
            TextureManager.TextureProgram.PopModelView();

            for (int j = camStartTile.Y; j <= camEndTile.Y; j++) {
                for (int i = camStartTile.X; i <= camEndTile.X; i++) {
                    //set tile sprite position
                    if (i < 0 || j < 0 || i >= Map.Width || j >= Map.Height) {
                        
                    } else {
                        Map.DrawGround(i * TextureManager.TILE_SIZE, j * TextureManager.TILE_SIZE, new Loc2D(i,j));
                        if (Turn) Graphics.TextureManager.GetTile(1, new Loc2D(1, 0)).Render(null);
                    }
                }
            }
            //draw items
            foreach (ItemAnim item in Items)
            {
                if (!item.ActionDone && IsSpriteInView(camStart, camEnd, item))
                {
                    item.Draw();
                }
            }

            //draw effects laid on ground
            List<ISprite> sortedSprites = new List<ISprite>();
            foreach (ISprite effect in Effects[EffectPriority.Ground]) {
                if (IsSpriteInView(camStart, camEnd, effect)) {
                    AddInOrder(sortedSprites, effect);
                }
            }
            int charIndex = 0;
            while (charIndex < sortedSprites.Count) {
                sortedSprites[charIndex].Draw();
                charIndex++;
            }


            //draw effects in object space
            sortedSprites = new List<ISprite>();

            //get all back effects, see if they're in the screen, and put them in the list, sorted
            foreach (ISprite effect in Effects[EffectPriority.Back]) {
                if (IsSpriteInView(camStart, camEnd, effect)) {
                    AddInOrder(sortedSprites, effect);
                }
            }
            if (ShowSprites)
            {
                //check if player is in the screen, put in list
                foreach (PlayerSprite player in Players)
                {
                    if (!player.Dead && IsSpriteInView(camStart, camEnd, player))
                    {
                        AddInOrder(sortedSprites, player);
                    }
                }
                //get all enemies, see if they're in the screen, and put them in the list, sorted
                foreach (NpcSprite npc in Npcs)
                {
                    if (!npc.Dead && IsSpriteInView(camStart, camEnd, npc))
                    {
                        AddInOrder(sortedSprites, npc);
                    }
                }
            }
            //get all effects, see if they're in the screen, and put them in the list, sorted
            foreach (ISprite effect in Effects[EffectPriority.None]) {
                if (IsSpriteInView(camStart, camEnd, effect)) {
                    AddInOrder(sortedSprites, effect);
                }
            }

            //draw object
            charIndex = 0;
            for (int j = camStartTile.Y; j <= camEndTile.Y; j++) {
                //before drawing objects, draw all active effects behind objects

                for (int i = camStartTile.X; i <= camEndTile.X; i++) {
                    //set tile sprite position
                    if (i < 0 || j < 0 || i >= Map.Width || j >= Map.Height) {

                    }
                    else
                    {
                        Map.DrawPropBack(i * TextureManager.TILE_SIZE, j * TextureManager.TILE_SIZE, new Loc2D(i,j));
                    }
                }

                //after drawing objects of the row, draw characters whose locations are of between that row(inc) to the next(exc)
                while (charIndex < sortedSprites.Count)
                {
                    int charY = sortedSprites[charIndex].MapLoc.Y;
                    if (charY == j * TextureManager.TILE_SIZE)
                    {
                        sortedSprites[charIndex].Draw();
                        charIndex++;
                    }
                    else
                    {
                        break;
                    }
                }


                for (int i = camStartTile.X; i <= camEndTile.X; i++)
                {
                    //set tile sprite position
                    if (i < 0 || j < 0 || i >= Map.Width || j >= Map.Height)
                    {

                    }
                    else
                    {
                        Map.DrawPropFront(i * TextureManager.TILE_SIZE, j * TextureManager.TILE_SIZE, new Loc2D(i, j));
                    }
                }

                //after drawing objects of the row, draw characters whose locations are of between that row(inc) to the next(exc)
                while (charIndex < sortedSprites.Count)
                {
                    int charY = sortedSprites[charIndex].MapLoc.Y;
                    if (charY < (j + 1) * TextureManager.TILE_SIZE)
                    {
                        sortedSprites[charIndex].Draw();
                        charIndex++;
                    }
                    else
                    {
                        break;
                    }
                }
            }
            //draw remaining sprites
            while (charIndex < sortedSprites.Count) {
                sortedSprites[charIndex].Draw();
                charIndex++;
            }

            //draw effects in top
            sortedSprites = new List<ISprite>();
            foreach (ISprite effect in Effects[EffectPriority.Top]) {
                if (IsSpriteInView(camStart, camEnd, effect)) {
                    AddInOrder(sortedSprites, effect);
                }
            }
            charIndex = 0;
            while (charIndex < sortedSprites.Count) {
                sortedSprites[charIndex].Draw();
                charIndex++;
            }

            //draw foreground
            for (int j = camStartTile.Y; j <= camEndTile.Y; j++) {
                for (int i = camStartTile.X; i <= camEndTile.X; i++) {
                    //set tile sprite position
                    if (i < 0 || j < 0 || i >= Map.Width || j >= Map.Height) {

                    }
                    else
                    {
                        Map.DrawFringe(i * TextureManager.TILE_SIZE, j * TextureManager.TILE_SIZE, new Loc2D(i, j));


                        if (ShowGrid)
                        {
                            TextureManager.TextureProgram.PushModelView();
                            TextureManager.TextureProgram.LeftMultModelView(Matrix4.CreateTranslation(i * TextureManager.TILE_SIZE, j * TextureManager.TILE_SIZE, 0));
                            TextureManager.TextureProgram.UpdateModelView();
                            Graphics.TextureManager.GetTile(10, new Loc2D()).Render(null);
                            TextureManager.TextureProgram.PopModelView();
                        }
                    }
                }
            }

            //draw effects in foreground
            sortedSprites = new List<ISprite>();
            foreach (ISprite effect in Effects[EffectPriority.Overlay]) {
                if (IsSpriteInView(camStart, camEnd, effect)) {
                    AddInOrder(sortedSprites, effect);
                }
            }
            charIndex = 0;
            while (charIndex < sortedSprites.Count) {
                sortedSprites[charIndex].Draw();
                charIndex++;
            }


            Graphics.TextureManager.TextureProgram.SetModelView(Matrix4.Identity);
            Gameplay.MenuManager.DrawMenus();
                

            //Moves
            Graphics.TextureManager.TextureProgram.SetModelView(Matrix4.Identity);
            if (Spell) {
                string[] keys = new string[4] { "S", "D", "X", "C" };
                TextureManager.TextureProgram.LeftMultModelView(Matrix4.CreateTranslation(-240, 32, 0));
                for (int i = 0; i < Gameplay.Processor.MAX_MOVE_SLOTS; i++) {
                    TextureManager.TextureProgram.LeftMultModelView(Matrix4.CreateTranslation(0, 64, 0));
                    Graphics.TextureManager.TextureProgram.UpdateModelView();

                    if (CurrentCharMoves[i] >= 0) {
                        Data.MoveEntry moveEntry = Data.GameData.MoveDex[CurrentCharMoves[i]];
                        TextureManager.SingleFont.RenderText(244, 20, keys[i] + ": (" + moveEntry.PP + "PP) " + moveEntry.Name, null, AtlasSheet.SpriteVOrigin.Top, AtlasSheet.SpriteHOrigin.Left, 0, Color4.White);
                    } else {
                        TextureManager.SingleFont.RenderText(244, 20, keys[i] + ": Empty", null, AtlasSheet.SpriteVOrigin.Top, AtlasSheet.SpriteHOrigin.Left, 0, Color4.White);
                    }
                }
            }

            
            //draw transitions
            if (CurrentFade == FadeType.FadeIn) {
                Graphics.TextureManager.TextureProgram.SetModelView(Matrix4.Identity);
                Graphics.TextureManager.TextureProgram.SetTextureColor(new Color4(0, 0, 0, (byte)(FadeTime.Ticks * 255 / TOTAL_FADE_TIME.Ticks)));
                Graphics.TextureManager.TextureProgram.LeftMultModelView(Matrix4.Scale(TextureManager.SCREEN_WIDTH, TextureManager.SCREEN_HEIGHT, 1));
                Graphics.TextureManager.TextureProgram.UpdateModelView();
                Graphics.TextureManager.BlankTexture.Render(null);
                Graphics.TextureManager.TextureProgram.SetTextureColor(Color4.White);
            } else if (CurrentFade == FadeType.FadeOut) {
                Graphics.TextureManager.TextureProgram.SetModelView(Matrix4.Identity);
                Graphics.TextureManager.TextureProgram.SetTextureColor(new Color4(0, 0, 0, (byte)(255 - FadeTime.Ticks * 255 / TOTAL_FADE_TIME.Ticks)));
                Graphics.TextureManager.TextureProgram.LeftMultModelView(Matrix4.Scale(TextureManager.SCREEN_WIDTH, TextureManager.SCREEN_HEIGHT, 1));
                Graphics.TextureManager.TextureProgram.UpdateModelView();
                Graphics.TextureManager.BlankTexture.Render(null);
                Graphics.TextureManager.TextureProgram.SetTextureColor(Color4.White);
            }

            if (ShowDebug)
            {
                DrawDebug(fps);
            }
        }

        public static Loc2D ScreenCoordsToMapCoords(Loc2D loc)
        {
            float scale = GetZoomScale(Zoom);
            Loc2D camCenter = new Loc2D(FocusedCharacter.CharLoc.X * TextureManager.TILE_SIZE + TextureManager.TILE_SIZE / 2,
                FocusedCharacter.CharLoc.Y * TextureManager.TILE_SIZE + TextureManager.TILE_SIZE / 2);
            camCenter = camCenter + CamOffset;

            Loc2D camStart = new Loc2D((int)(camCenter.X - TextureManager.SCREEN_WIDTH / scale / 2), (int)(camCenter.Y - TextureManager.SCREEN_HEIGHT / scale / 2));
            Loc2D camEnd = new Loc2D((int)(camCenter.X + TextureManager.SCREEN_WIDTH / scale / 2), (int)(camCenter.Y + TextureManager.SCREEN_HEIGHT / scale / 2));
            
            loc.X = (int)(loc.X / scale);
            loc.Y = (int)(loc.Y / scale);
            loc += camStart;
            loc = loc - (camStart / TextureManager.TILE_SIZE * TextureManager.TILE_SIZE) + new Loc2D(TextureManager.TILE_SIZE);
            loc /= TextureManager.TILE_SIZE;
            loc = loc + (camStart / TextureManager.TILE_SIZE) - new Loc2D(1);

            return loc;
        }

        public static bool IsSpriteInView(Loc2D camStart, Loc2D camEnd, ISprite sprite) {
            if (sprite == null) return false;
            Loc2D spriteStart = sprite.GetStart();
            Loc2D spriteEnd = sprite.GetEnd();

            if (spriteStart == spriteEnd)
                return true;

            //check to see if the sprite's left is to the right of the screen's right side
            if (spriteStart.X > camEnd.X)
                return false;

            //check to see if the sprite's right is to the left of the screen's left side
            if (spriteEnd.X < camStart.X)
                return false;

            //check to see if the sprite's top is to the bottom of the screen's bottom side
            if (spriteStart.Y > camEnd.Y)
                return false;
            
            //check to see if the sprite's bottom is to the top of the screen's top side
            if (spriteEnd.Y < camStart.Y)
                return false;

            return true;
        }

        public static void AddInOrder(List<ISprite> sprites, ISprite sprite) {
            int min = 0;
            int max = sprites.Count - 1;
            int point = max;
            MathFunctions.Compare compare = MathFunctions.Compare.Less;
            while (min <= max) {
                point = (min + max) / 2;

                compare = CompareSpriteCoords(sprites[point], sprite);

                if (compare == MathFunctions.Compare.Greater) {
                    //go down
                    max = point - 1;
                } else if (compare == MathFunctions.Compare.Less) {
                    //go up
                    min = point + 1;
                } else {
                    //go past the last index of equal comparison
                    point++;
                    while (point < sprites.Count && CompareSpriteCoords(sprites[point], sprite) == MathFunctions.Compare.Equal) {
                        point++;
                    }
                    sprites.Insert(point, sprite);
                    return;
                }
            }
            //no place found
            if (compare == MathFunctions.Compare.Greater) {
                //put this one under the current point
                sprites.Insert(point, sprite);
            } else {
                //put this one above the current point
                sprites.Insert(point+1, sprite);
            }
        }

        static float GetZoomScale(GameZoom zoom)
        {
            switch (zoom)
            {
                case GameZoom.x8Near:
                    return 8f;
                case GameZoom.x4Near:
                    return 4f;
                case GameZoom.x2Near:
                    return 2f;
                case GameZoom.x2Far:
                    return 0.5f;
                case GameZoom.x4Far:
                    return 0.25f;
                case GameZoom.x8Far:
                    return 0.125f;
                case GameZoom.x16Far:
                    return 0.0625f;
                default:
                    return 1.0f;
            }
        }



        public static MathFunctions.Compare CompareSpriteCoords(ISprite sprite1, ISprite sprite2) {
            return MathFunctions.CompareValues(sprite1.MapLoc.Y, sprite2.MapLoc.Y);
        }

        private static void DrawDebug(int fps)
        {
            Graphics.TextureManager.TextureProgram.SetModelView(Matrix4.Identity);
            Graphics.TextureManager.TextureProgram.UpdateModelView();

            if (ShowDebug)
            {
                TextureManager.GetMugshot(FocusedCharacter.CharData.Species, FocusedCharacter.CharData.Form, FocusedCharacter.CharData.Shiny, FocusedCharacter.CharData.Gender).RenderTile(0,0);

                //TextureManager.SingleFont.RenderText(2, 32, "\u2640 \u2642", null, AtlasSheet.SpriteVOrigin.Top, AtlasSheet.SpriteHOrigin.Left, 0, Color4.White);


#if GAME_MODE
            TextureManager.SingleFont.RenderTextShadowed(TextureManager.SCREEN_WIDTH - 2, 22, Floor + "F", null, AtlasSheet.SpriteVOrigin.Top, AtlasSheet.SpriteHOrigin.Right, Color4.White);
#endif
                TextureManager.SingleFont.RenderText(TextureManager.SCREEN_WIDTH - 2, 32, fps + " FPS", null, AtlasSheet.SpriteVOrigin.Top, AtlasSheet.SpriteHOrigin.Right, 0, Color4.White);
                TextureManager.SingleFont.RenderText(TextureManager.SCREEN_WIDTH - 2, 42, UpdatesPerSecond + " UPS", null, AtlasSheet.SpriteVOrigin.Top, AtlasSheet.SpriteHOrigin.Right, 0, Color4.White);
                TextureManager.SingleFont.RenderText(TextureManager.SCREEN_WIDTH - 2, 52, DebugSpeed.ToString() + " Speed", null, AtlasSheet.SpriteVOrigin.Top, AtlasSheet.SpriteHOrigin.Right, 0, Color4.LightYellow);
                TextureManager.SingleFont.RenderText(TextureManager.SCREEN_WIDTH - 2, 62, "F1 = Toggle Debug Menu", null, AtlasSheet.SpriteVOrigin.Top, AtlasSheet.SpriteHOrigin.Right, 0, Color4.White);
                TextureManager.SingleFont.RenderText(TextureManager.SCREEN_WIDTH - 2, 72, "F2 = Slow Down | F3 = Speed Up", null, AtlasSheet.SpriteVOrigin.Top, AtlasSheet.SpriteHOrigin.Right, 0, Color4.White);
                TextureManager.SingleFont.RenderText(TextureManager.SCREEN_WIDTH - 2, 82, "Zoom: " + Zoom.ToString(), null, AtlasSheet.SpriteVOrigin.Top, AtlasSheet.SpriteHOrigin.Right, 0, Color4.White);



                if (Print) TextureManager.SingleFont.RenderText(TextureManager.SCREEN_WIDTH - 2, 92, "ASCII Print", null, AtlasSheet.SpriteVOrigin.Top, AtlasSheet.SpriteHOrigin.Right, 0, Color4.LightYellow);
                if (Intangible) TextureManager.SingleFont.RenderText(TextureManager.SCREEN_WIDTH - 2, 102, "Intangible", null, AtlasSheet.SpriteVOrigin.Top, AtlasSheet.SpriteHOrigin.Right, 0, Color4.LightYellow);
                if (Jump) TextureManager.SingleFont.RenderText(TextureManager.SCREEN_WIDTH - 2, 112, "Jump", null, AtlasSheet.SpriteVOrigin.Top, AtlasSheet.SpriteHOrigin.Right, 0, Color4.White);
                if (Turn) TextureManager.SingleFont.RenderText(TextureManager.SCREEN_WIDTH - 2, 122, "Turn", null, AtlasSheet.SpriteVOrigin.Top, AtlasSheet.SpriteHOrigin.Right, 0, Color4.White);
                if (Diagonal) TextureManager.SingleFont.RenderText(TextureManager.SCREEN_WIDTH - 2, 132, "Diagonal", null, AtlasSheet.SpriteVOrigin.Top, AtlasSheet.SpriteHOrigin.Right, 0, Color4.White);

            }

            if (ShowCoords)
            {
                TextureManager.SingleFont.RenderText(TextureManager.SCREEN_WIDTH - 2, 2, FocusedCharacter.CharLoc.X + "X", null, AtlasSheet.SpriteVOrigin.Top, AtlasSheet.SpriteHOrigin.Right, 0, Color4.White);
                TextureManager.SingleFont.RenderText(TextureManager.SCREEN_WIDTH - 2, 12, FocusedCharacter.CharLoc.Y + "Y", null, AtlasSheet.SpriteVOrigin.Top, AtlasSheet.SpriteHOrigin.Right, 0, Color4.White);
            }
            List<string> logs = Logs.Logger.GetRecentBattleLog(16);
            for (int i = 0; i < logs.Count; i++)
                TextureManager.SingleFont.RenderText(2, 48+10*i, logs[i], null, AtlasSheet.SpriteVOrigin.Top, AtlasSheet.SpriteHOrigin.Left, 0, Color4.White);
        }

    }
}
