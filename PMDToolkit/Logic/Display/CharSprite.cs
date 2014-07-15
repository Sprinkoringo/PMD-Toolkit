using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PMDToolkit.Maps;
using PMDToolkit.Graphics;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace PMDToolkit.Logic.Display {
    public class CharSprite : ISprite {

        static readonly RenderTime STATUS_FRAME_LENGTH = RenderTime.FromMillisecs(4000);
        static readonly RenderTime IDLE_FRAME_LENGTH = RenderTime.FromMillisecs(90);
        static readonly RenderTime WALK_FRAME_LENGTH = RenderTime.FromMillisecs(140);

        public enum ActionType {
            None = 0,
            Idle,
            Walk,
            Attack,
            AttackArm,
            AltAttack,
            SpAttack,
            SpAttackShoot,
            SpAttackCharge,
            Sleeping,
            Hurt,
            Defeated,
            Throw,
            Item,
            Jump,
            JumpHit,
            Deflect,
            Knockback
        };

        public static RenderTime GetActionTime(Gameplay.FormData charData, Direction8 dir, ActionType actionType)
        {
            switch (actionType)
            {
                case ActionType.None:
                    return RenderTime.FromMillisecs(1000);
                case ActionType.Idle:
                    {
                        int frames = TextureManager.GetSpriteSheet(charData.Species, charData.Form, charData.Shiny, charData.Gender).FrameData.GetFrameCount(FrameType.Idle, dir);
                        
                        return IDLE_FRAME_LENGTH * frames;
                    }
                case ActionType.Walk:
                    {
                        RenderTime passTime = GetPassTime(charData, dir, actionType);
                        int frames = TextureManager.GetSpriteSheet(charData.Species, charData.Form, charData.Shiny, charData.Gender).FrameData.GetFrameCount(FrameType.Walk, dir);
                        
                        if (frames == 0)
                            return GetPassTime(charData, dir, actionType);

                        int totalFrames = frames;
                        while (WALK_FRAME_LENGTH * frames < passTime)
                        {
                            totalFrames += frames;
                        }
                        return WALK_FRAME_LENGTH * totalFrames;
                    }
                case ActionType.Attack:
                case ActionType.AttackArm:
                case ActionType.AltAttack:
                    return RenderTime.FromMillisecs(400);
                case ActionType.SpAttack:
                case ActionType.SpAttackShoot:
                    return RenderTime.FromMillisecs(480);
                case ActionType.SpAttackCharge:
                    return RenderTime.FromMillisecs(320);
                case ActionType.Sleeping:
                    return RenderTime.FromMillisecs(1000);
                case ActionType.Hurt:
                    return RenderTime.FromMillisecs(360);
                case ActionType.Defeated:
                    return RenderTime.FromMillisecs(720);
                case ActionType.Throw:
                    return RenderTime.FromMillisecs(200);
                case ActionType.Item:
                    return RenderTime.FromMillisecs(200);
                case ActionType.Jump:
                    return RenderTime.FromMillisecs(300);
                case ActionType.JumpHit:
                    return RenderTime.FromMillisecs(600);
                case ActionType.Deflect:
                    return RenderTime.FromMillisecs(600);
                case ActionType.Knockback:
                    return RenderTime.FromMillisecs(80);
                default:
                    return RenderTime.Zero;
            }
        }

        public static RenderTime GetPassTime(Gameplay.FormData charData, Direction8 dir, ActionType actionType)
        {

            switch (actionType)
            {
                case ActionType.None:
                    return RenderTime.FromMillisecs(1000);
                case ActionType.Idle:
                    return RenderTime.FromMillisecs(0);
                case ActionType.Walk:
                    return RenderTime.FromMillisecs(200);
                case ActionType.Attack:
                case ActionType.AltAttack:
                    return RenderTime.FromMillisecs(200);
                case ActionType.SpAttack:
                case ActionType.SpAttackShoot:
                    return RenderTime.FromMillisecs(200);
                case ActionType.SpAttackCharge:
                    return RenderTime.FromMillisecs(280);
                case ActionType.Sleeping:
                case ActionType.Hurt:
                case ActionType.Defeated:
                case ActionType.Throw:
                case ActionType.Item:
                case ActionType.Jump:
                case ActionType.JumpHit:
                case ActionType.Deflect:
                case ActionType.Knockback:
                    return GetActionTime(charData, dir, actionType);
                default:
                    return RenderTime.Zero;
            }
        }

        public Gameplay.FormData CharData;

        public Enums.StatusAilment StatusAilment { get; set; }

        //determining position
        private Loc2D charLoc;
        public Loc2D CharLoc { get { return charLoc; } set { charLoc = value; } }
        
        public Loc2D TileOffset;
        private Loc2D drawOffset;
        private byte opacity;
        public Loc2D MapLoc { get { return new Loc2D(charLoc.X * TextureManager.TILE_SIZE + TileOffset.X + drawOffset.X, charLoc.Y * TextureManager.TILE_SIZE + TileOffset.Y + drawOffset.Y); } }
        public int MapHeight { get; set; }

        public bool Dead { get; set; }

        public FrameType CharFrameType { get; set; }
        public int CharFrame { get; set; }

        public int MovementSpeed { get; set; }

        //logic based + used for determining animation
        public ActionType CurrentAction { get; set; }

        //logic-based
        public RenderTime ActionTime { get; set; }
        public RenderTime PrevActionTime { get; set; }
        public bool ActionDone { get; set; }
        public bool ActionLoop { get; set; }
        public bool MoveInPlace;

        //determining animation
        public Direction8 CharDir { get; set; }

        public CharSprite()
        {
            charLoc = new Loc2D();
            ActionDone = true;
            Dead = true;
        }

        //draw offset, height offset, and frame are determined on runtime based on
        //-CurrentAction
        //-ActionTime
        //-CharDir
        public CharSprite(Loc2D charLoc, Direction8 charDir) {
            this.charLoc = charLoc;
            this.CharDir = charDir;
            this.ActionDone = true;
            //everything else at default values
            //CharData.Species, CharData.Form, CharData.Shiny, CharData.Gender is 0 for now
            CharData = new Gameplay.FormData();
        }

        public virtual void Begin()
        {

        }

        public void Process(RenderTime elapsedTime)
        {
            if (Dead)
                return;

            if (CurrentAction == ActionType.Idle)
            {
                if (MovementSpeed < 0)
                {
                    elapsedTime /= 2;
                }
                else if (MovementSpeed > 0)
                {
                    elapsedTime *= 2;
                }
            }
            ActionTime += elapsedTime;

            RenderTime totalActionTime = GetActionTime(CharData, CharDir, CurrentAction);
            RenderTime totalPassTime = GetPassTime(CharData, CharDir, CurrentAction);
            
            if (ActionTime >= totalPassTime)
                ActionDone = true;

            if (ActionTime >= totalActionTime)
            {
                if (ActionLoop)
                {
                    ActionTime = ActionTime % totalActionTime;
                }
                else
                {
                    switch (CurrentAction)
                    {
                        case ActionType.None:
                            {
                                ActionTime = RenderTime.Zero;
                                CurrentAction = ActionType.Idle;
                            }
                            break;
                        case ActionType.Idle:
                            {
                                if (totalActionTime > RenderTime.Zero)
                                    ActionTime = ActionTime % totalActionTime;
                                else
                                    ActionTime = RenderTime.Zero;
                            }
                            break;
                        default:
                            {
                                ActionTime = RenderTime.Zero;
                                CurrentAction = ActionType.None;
                            }
                            break;
                    }
                }
            }

            CharFrameType = FrameType.Idle;
            CharFrame = 0;
            TileOffset = new Loc2D();
            drawOffset = new Loc2D();
            opacity = 255;
            MapHeight = 0;
            if (CurrentAction == ActionType.Idle)
            {
                CharFrameType = FrameType.Idle;
                int totalFrames = TextureManager.GetSpriteSheet(CharData.Species, CharData.Form, CharData.Shiny, CharData.Gender).FrameData.GetFrameCount(CharFrameType, CharDir);
                if (totalFrames > 0)
                    CharFrame = (ActionTime.Ticks / IDLE_FRAME_LENGTH.Ticks) % totalFrames;

                TileOffset = new Loc2D();
                MapHeight = 0;
            }
            else if (CurrentAction == ActionType.Walk)
            {
                CharFrameType = FrameType.Walk;
                int totalFrames = TextureManager.GetSpriteSheet(CharData.Species, CharData.Form, CharData.Shiny, CharData.Gender).FrameData.GetFrameCount(CharFrameType, CharDir);
                
                if (totalFrames > 0)
                    CharFrame = ((ActionTime + PrevActionTime).Ticks / WALK_FRAME_LENGTH.Ticks) % totalFrames;

                if (!MoveInPlace)
                {
                    if (ActionTime.Ticks <= totalPassTime.Ticks)
                        Operations.MoveInDirection8(ref TileOffset, CharDir, ActionTime.Ticks * Graphics.TextureManager.TILE_SIZE / totalPassTime.Ticks);
                }
            }
            else if (CurrentAction == ActionType.Attack)
            {
                CharFrameType = FrameType.Attack;
                int totalFrames = TextureManager.GetSpriteSheet(CharData.Species, CharData.Form, CharData.Shiny, CharData.Gender).FrameData.GetFrameCount(CharFrameType, CharDir);
                if (totalFrames > 0)
                    CharFrame = (ActionTime.Ticks * totalFrames / totalActionTime.Ticks);

                if (!MoveInPlace)
                {
                    int pullback_distance = Graphics.TextureManager.TILE_SIZE / 8;
                    int farthest_distance = Graphics.TextureManager.TILE_SIZE * 3 / 4;
                    int hold_point = totalActionTime.Ticks / 8;
                    int rush_point = totalActionTime.Ticks * 2 / 8;
                    int hit_point = totalActionTime.Ticks * 4 / 8;
                    int return_point = totalActionTime.Ticks * 6 / 8;
                    if (ActionTime.Ticks <= hold_point)
                        Operations.MoveInDirection8(ref drawOffset, CharDir, -ActionTime.Ticks * pullback_distance / rush_point);
                    else if (ActionTime.Ticks <= rush_point)
                        Operations.MoveInDirection8(ref drawOffset, CharDir, -pullback_distance);
                    else if (ActionTime.Ticks <= hit_point)
                        Operations.MoveInDirection8(ref drawOffset, CharDir, (ActionTime.Ticks - rush_point) * (farthest_distance + pullback_distance) / (hit_point - rush_point) - pullback_distance);
                    else if (ActionTime.Ticks <= return_point)
                        Operations.MoveInDirection8(ref drawOffset, CharDir, farthest_distance);
                    else
                        Operations.MoveInDirection8(ref drawOffset, CharDir, ((totalActionTime.Ticks - hit_point) - (ActionTime.Ticks - hit_point)) * farthest_distance / (totalActionTime.Ticks - hit_point));
                }
            }
            else if (CurrentAction == ActionType.AttackArm)
            {
                CharFrameType = FrameType.AttackArm;
                int totalFrames = TextureManager.GetSpriteSheet(CharData.Species, CharData.Form, CharData.Shiny, CharData.Gender).FrameData.GetFrameCount(CharFrameType, CharDir);
                if (totalFrames > 0)
                    CharFrame = (ActionTime.Ticks * totalFrames / totalActionTime.Ticks);

                if (!MoveInPlace)
                {
                    int pullback_distance = Graphics.TextureManager.TILE_SIZE / 8;
                    int farthest_distance = Graphics.TextureManager.TILE_SIZE * 3 / 4;
                    int hold_point = totalActionTime.Ticks / 8;
                    int rush_point = totalActionTime.Ticks * 2 / 8;
                    int hit_point = totalActionTime.Ticks * 4 / 8;
                    int return_point = totalActionTime.Ticks * 6 / 8;
                    if (ActionTime.Ticks <= hold_point)
                        Operations.MoveInDirection8(ref drawOffset, CharDir, -ActionTime.Ticks * pullback_distance / rush_point);
                    else if (ActionTime.Ticks <= rush_point)
                        Operations.MoveInDirection8(ref drawOffset, CharDir, -pullback_distance);
                    else if (ActionTime.Ticks <= hit_point)
                        Operations.MoveInDirection8(ref drawOffset, CharDir, (ActionTime.Ticks - rush_point) * (farthest_distance + pullback_distance) / (hit_point - rush_point) - pullback_distance);
                    else if (ActionTime.Ticks <= return_point)
                        Operations.MoveInDirection8(ref drawOffset, CharDir, farthest_distance);
                    else
                        Operations.MoveInDirection8(ref drawOffset, CharDir, ((totalActionTime.Ticks - hit_point) - (ActionTime.Ticks - hit_point)) * farthest_distance / (totalActionTime.Ticks - hit_point));
                }
            }
            else if (CurrentAction == ActionType.AltAttack)
            {
                CharFrameType = FrameType.AltAttack;
                int totalFrames = TextureManager.GetSpriteSheet(CharData.Species, CharData.Form, CharData.Shiny, CharData.Gender).FrameData.GetFrameCount(CharFrameType, CharDir);
                CharFrame = (ActionTime.Ticks * totalFrames / totalActionTime.Ticks);

                if (!MoveInPlace)
                {
                    int pullback_distance = Graphics.TextureManager.TILE_SIZE / 8;
                    int farthest_distance = Graphics.TextureManager.TILE_SIZE * 3 / 4;
                    int hold_point = totalActionTime.Ticks / 8;
                    int rush_point = totalActionTime.Ticks * 2 / 8;
                    int hit_point = totalActionTime.Ticks * 4 / 8;
                    int return_point = totalActionTime.Ticks * 6 / 8;
                    if (ActionTime.Ticks <= hold_point)
                        Operations.MoveInDirection8(ref drawOffset, CharDir, -ActionTime.Ticks * pullback_distance / rush_point);
                    else if (ActionTime.Ticks <= rush_point)
                        Operations.MoveInDirection8(ref drawOffset, CharDir, -pullback_distance);
                    else if (ActionTime.Ticks <= hit_point)
                        Operations.MoveInDirection8(ref drawOffset, CharDir, (ActionTime.Ticks - rush_point) * (farthest_distance + pullback_distance) / (hit_point - rush_point) - pullback_distance);
                    else if (ActionTime.Ticks <= return_point)
                        Operations.MoveInDirection8(ref drawOffset, CharDir, farthest_distance);
                    else
                        Operations.MoveInDirection8(ref drawOffset, CharDir, ((totalActionTime.Ticks - hit_point) - (ActionTime.Ticks - hit_point)) * farthest_distance / (totalActionTime.Ticks - hit_point));
                }
            }
            else if (CurrentAction == ActionType.SpAttack)
            {
                CharFrameType = FrameType.SpAttack;
                int totalFrames = TextureManager.GetSpriteSheet(CharData.Species, CharData.Form, CharData.Shiny, CharData.Gender).FrameData.GetFrameCount(CharFrameType, CharDir);
                CharFrame = (ActionTime.Ticks * totalFrames / totalActionTime.Ticks);
            }
            else if (CurrentAction == ActionType.SpAttackShoot)
            {
                CharFrameType = FrameType.SpAttackShoot;
                int totalFrames = TextureManager.GetSpriteSheet(CharData.Species, CharData.Form, CharData.Shiny, CharData.Gender).FrameData.GetFrameCount(CharFrameType, CharDir);
                CharFrame = (ActionTime.Ticks * totalFrames / totalActionTime.Ticks);

                if (!MoveInPlace)
                {
                    int pullback_distance = Graphics.TextureManager.TILE_SIZE / 8;
                    int farthest_distance = Graphics.TextureManager.TILE_SIZE / 8;
                    int hold_point = totalActionTime.Ticks / 8;
                    int rush_point = totalActionTime.Ticks * 3 / 8;
                    int hit_point = totalActionTime.Ticks * 4 / 8;
                    int return_point = totalActionTime.Ticks * 7 / 8;
                    if (ActionTime.Ticks <= hold_point)
                        Operations.MoveInDirection8(ref drawOffset, CharDir, -ActionTime.Ticks * pullback_distance / rush_point);
                    else if (ActionTime.Ticks <= rush_point)
                        Operations.MoveInDirection8(ref drawOffset, CharDir, -pullback_distance);
                    else if (ActionTime.Ticks <= hit_point)
                        Operations.MoveInDirection8(ref drawOffset, CharDir, (ActionTime.Ticks - rush_point) * (farthest_distance + pullback_distance) / (hit_point - rush_point) - pullback_distance);
                    else if (ActionTime.Ticks <= return_point)
                        Operations.MoveInDirection8(ref drawOffset, CharDir, farthest_distance);
                    else
                        Operations.MoveInDirection8(ref drawOffset, CharDir, ((totalActionTime.Ticks - hit_point) - (ActionTime.Ticks - hit_point)) * farthest_distance / (totalActionTime.Ticks - hit_point));
                }
            }
            else if (CurrentAction == ActionType.SpAttackCharge)
            {
                CharFrameType = FrameType.SpAttackCharge;
                int totalFrames = TextureManager.GetSpriteSheet(CharData.Species, CharData.Form, CharData.Shiny, CharData.Gender).FrameData.GetFrameCount(CharFrameType, CharDir);
                if (totalFrames > 0)
                    CharFrame = totalFrames - 1;

                if (!MoveInPlace)
                {
                    if (ActionTime.Ticks / 40 % 2 == 0)
                        Operations.MoveInDirection8(ref drawOffset, Operations.AddDir(CharDir, Direction8.Left), 1);
                }
            }
            else if (CurrentAction == ActionType.Sleeping)
            {
                CharFrameType = FrameType.Sleep;
                int frameCount = TextureManager.GetSpriteSheet(CharData.Species, CharData.Form, CharData.Shiny, CharData.Gender).FrameData.GetFrameCount(CharFrameType, Direction8.Down);
                CharFrame = (ActionTime.Ticks * frameCount / totalActionTime.Ticks);
            }
            else if (CurrentAction == ActionType.Hurt)
            {
                CharFrameType = FrameType.Hurt;
                CharFrame = 0;
                if (!MoveInPlace)
                {
                    if ((ActionTime.Ticks * 3 / totalActionTime.Ticks) % 2 == 0)
                        Operations.MoveInDirection8(ref drawOffset, Operations.ReverseDir(CharDir), 1);
                }
            }
            else if (CurrentAction == ActionType.Defeated)
            {
                CharFrameType = FrameType.Hurt;
                CharFrame = 0;
                if ((ActionTime.Ticks * 6 / totalActionTime.Ticks) % 2 == 0)
                    Operations.MoveInDirection8(ref drawOffset, Operations.ReverseDir(CharDir), 1);
                if ((ActionTime.Ticks * 2 / totalActionTime.Ticks) > 0)
                    opacity = 128;
            }
            //else if (CurrentAction == ActionType.Jump)
            //{
            //    CharFrame = (ActionTime * TextureManager.GetSpriteSheet(CharData.Species, CharData.Form, CharData.Shiny, CharData.Gender).FrameData.GetFrameCount(FrameType.AltAttack, CharDir) / FrameLength);
            //    if (ActionData1 > 1 || ActionData1 == 0)
            //    {
            //        Operations.MoveInDirection8(ref tileOffset, CharDir, ActionTime * ActionData1 * Graphics.TextureManager.TILE_SIZE / FrameLength);
            //    }
            //    else
            //    {
            //        int moveDist = ActionTime * 2 * Graphics.TextureManager.TILE_SIZE / FrameLength;
            //        if (moveDist > Graphics.TextureManager.TILE_SIZE) moveDist = Graphics.TextureManager.TILE_SIZE;
            //        Operations.MoveInDirection8(ref tileOffset, CharDir, moveDist);
            //    }
            //}
            //else if (CurrentAction == ActionType.JumpHit)
            //{
            //    int phaseTotal = (FrameLength / 2);
            //    int phaseTime = ActionTime % phaseTotal;
            //    if (ActionTime < FrameLength / 2)
            //    {
            //        CharFrame = (phaseTime * TextureManager.GetSpriteSheet(CharData.Species, CharData.Form, CharData.Shiny, CharData.Gender).FrameData.GetFrameCount(FrameType.AltAttack, CharDir) / phaseTotal);
            //        if (ActionData1 > 1 || ActionData1 == 0)
            //        {
            //            Operations.MoveInDirection8(ref drawOffset, CharDir, phaseTime * ActionData1 * Graphics.TextureManager.TILE_SIZE / phaseTotal);
            //        }
            //        else
            //        {
            //            int moveDist = phaseTime * 2 * Graphics.TextureManager.TILE_SIZE / phaseTotal;
            //            if (moveDist > Graphics.TextureManager.TILE_SIZE) moveDist = Graphics.TextureManager.TILE_SIZE;
            //            Operations.MoveInDirection8(ref drawOffset, CharDir, moveDist);
            //        }
            //        MapHeight = phaseTime * Graphics.TextureManager.TILE_SIZE / phaseTotal;
            //    }
            //    else
            //    {
            //        CharFrame = (phaseTime * TextureManager.GetSpriteSheet(CharData.Species, CharData.Form, CharData.Shiny, CharData.Gender).FrameData.GetFrameCount(FrameType.AltAttack, CharDir) / phaseTotal);
            //        Operations.MoveInDirection8(ref drawOffset, CharDir, ActionData1 * Graphics.TextureManager.TILE_SIZE - phaseTime * ActionData1 * Graphics.TextureManager.TILE_SIZE / phaseTotal);
            //        MapHeight = (phaseTotal - phaseTime) * Graphics.TextureManager.TILE_SIZE / phaseTotal;
            //    }
            //}
            else if (CurrentAction == ActionType.Deflect)
            {
                CharFrameType = FrameType.Hurt;
                CharFrame = 0;
                TileOffset = new Loc2D();
                Operations.MoveInDirection8(ref TileOffset, CharDir, ActionTime.Ticks * Graphics.TextureManager.TILE_SIZE / totalActionTime.Ticks);
                MapHeight = DrawHelper.GetArc(TextureManager.TILE_SIZE / 2, totalActionTime.Ticks, ActionTime.Ticks);
            }
            else if (CurrentAction == ActionType.Knockback)
            {
                CharFrameType = FrameType.Hurt;
                CharFrame = 0;
                TileOffset = new Loc2D();
                Operations.MoveInDirection8(ref TileOffset, Operations.ReverseDir(CharDir), ActionTime.Ticks * Graphics.TextureManager.TILE_SIZE / totalActionTime.Ticks);
                MapHeight = 0;
            }
            else
            {
                CharFrameType = FrameType.Idle;
                CharFrame = 0;
                TileOffset = new Loc2D();
                MapHeight = 0;
            }

        }

        public void Draw() {

            //draw back status

            TextureManager.TextureProgram.PushModelView();


            Loc2D drawLoc = GetStart();
            Graphics.TextureManager.TextureProgram.LeftMultModelView(Matrix4.CreateTranslation(drawLoc.X, drawLoc.Y, 0));
            Graphics.TextureManager.TextureProgram.UpdateModelView();

            int curFrame = CharFrame;

            if (MovementSpeed < 0)
                Graphics.TextureManager.TextureProgram.SetTextureColor(new Color4(128, 128, 255, opacity));
            else if (MovementSpeed > 0)
                Graphics.TextureManager.TextureProgram.SetTextureColor(new Color4(255, 128, 128, opacity));
            else
                Graphics.TextureManager.TextureProgram.SetTextureColor(new Color4(255, 255, 255, opacity));

            //draw sprite at current frame
            if (StatusAilment == Enums.StatusAilment.Freeze)
                Graphics.TextureManager.GetSpriteSheet(CharData.Species, CharData.Form, CharData.Shiny, CharData.Gender).GetSheet(Graphics.FrameType.Hurt, CharDir).RenderTile(0, 0);
            else
                Graphics.TextureManager.GetSpriteSheet(CharData.Species, CharData.Form, CharData.Shiny, CharData.Gender).GetSheet(CharFrameType, CharDir).RenderTile(CharFrame, 0);


            Graphics.TextureManager.TextureProgram.SetTextureColor(new Color4(255, 255, 255, 255));

            TextureManager.TextureProgram.PopModelView();

            //draw front status
            if (StatusAilment == Enums.StatusAilment.Burn) {
                TextureManager.TextureProgram.PushModelView();
                
                //draw front status; use global time
                Loc2D frontDraw = new Loc2D(charLoc.X * Graphics.TextureManager.TILE_SIZE + TileOffset.X + Graphics.TextureManager.TILE_SIZE / 2 - Graphics.TextureManager.GetStatusSheet(1).TileWidth / 2,
                    charLoc.Y * Graphics.TextureManager.TILE_SIZE + TileOffset.Y - MapHeight + Graphics.TextureManager.TILE_SIZE - Graphics.TextureManager.GetStatusSheet(1).TileHeight);

                Graphics.TextureManager.TextureProgram.LeftMultModelView(Matrix4.CreateTranslation(frontDraw.X, frontDraw.Y, 0));
                Graphics.TextureManager.TextureProgram.UpdateModelView();

                int frame = (int)(Display.Screen.TotalTick / (ulong)STATUS_FRAME_LENGTH.Ticks % (ulong)Graphics.TextureManager.GetStatusSheet(1).MaxX);

                Graphics.TextureManager.GetStatusSheet(1).RenderTile(frame, 0);

                TextureManager.TextureProgram.PopModelView();
            } else if (StatusAilment == Enums.StatusAilment.Freeze) {
                TextureManager.TextureProgram.PushModelView();

                //draw front status; use global time
                Loc2D frontDraw = new Loc2D(charLoc.X * Graphics.TextureManager.TILE_SIZE + TileOffset.X + Graphics.TextureManager.TILE_SIZE / 2 - Graphics.TextureManager.GetStatusSheet(0).TileWidth / 2,
                    charLoc.Y * Graphics.TextureManager.TILE_SIZE + TileOffset.Y - MapHeight + Graphics.TextureManager.TILE_SIZE - Graphics.TextureManager.GetStatusSheet(0).TileHeight);

                Graphics.TextureManager.TextureProgram.LeftMultModelView(Matrix4.CreateTranslation(frontDraw.X, frontDraw.Y, 0));
                Graphics.TextureManager.TextureProgram.UpdateModelView();

                int frame = (int)(Display.Screen.TotalTick / (ulong)STATUS_FRAME_LENGTH.Ticks % (ulong)Graphics.TextureManager.GetStatusSheet(0).MaxX);

                Graphics.TextureManager.GetStatusSheet(0).RenderTile(frame, 0);

                TextureManager.TextureProgram.PopModelView();
            } else if (StatusAilment == Enums.StatusAilment.Poison) {
                TextureManager.TextureProgram.PushModelView();

                //draw front status; use global time
                Loc2D frontDraw = new Loc2D(charLoc.X * Graphics.TextureManager.TILE_SIZE + TileOffset.X + Graphics.TextureManager.TILE_SIZE / 2 - Graphics.TextureManager.GetStatusSheet(2).TileWidth / 2,
                    charLoc.Y * Graphics.TextureManager.TILE_SIZE + TileOffset.Y - MapHeight + Graphics.TextureManager.TILE_SIZE - Graphics.TextureManager.GetStatusSheet(2).TileHeight);

                Graphics.TextureManager.TextureProgram.LeftMultModelView(Matrix4.CreateTranslation(frontDraw.X, frontDraw.Y, 0));
                Graphics.TextureManager.TextureProgram.UpdateModelView();

                int frame = (int)(Display.Screen.TotalTick / (ulong)STATUS_FRAME_LENGTH.Ticks % (ulong)Graphics.TextureManager.GetStatusSheet(2).MaxX);

                Graphics.TextureManager.GetStatusSheet(2).RenderTile(frame, 0);

                TextureManager.TextureProgram.PopModelView();
            }
        }

        public Loc2D GetStart()
        {
            Loc2D mapLoc = MapLoc;
            return new Loc2D(mapLoc.X + Graphics.TextureManager.TILE_SIZE / 2 - Graphics.TextureManager.GetSpriteSheet(CharData.Species, CharData.Form, CharData.Shiny, CharData.Gender).FrameData.FrameWidth / 2,
                mapLoc.Y - MapHeight + Graphics.TextureManager.TILE_SIZE - Graphics.TextureManager.GetSpriteSheet(CharData.Species, CharData.Form, CharData.Shiny, CharData.Gender).FrameData.FrameHeight);
        }

        public Loc2D GetEnd()
        {
            Loc2D mapLoc = MapLoc;
            return new Loc2D(mapLoc.X + Graphics.TextureManager.TILE_SIZE / 2 + Graphics.TextureManager.GetSpriteSheet(CharData.Species, CharData.Form, CharData.Shiny, CharData.Gender).FrameData.FrameWidth / 2,
                mapLoc.Y - MapHeight + Graphics.TextureManager.TILE_SIZE);
        }

    }
}
