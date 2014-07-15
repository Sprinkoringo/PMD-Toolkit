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
    public class ItemAnim : ISprite {

        public enum ItemAnimType {
            None = 0,
            Drop = 1,
            Bounce = 2,
            Deflect = 3
        };

        public static RenderTime[] ITEM_ACTION_TIME = new RenderTime[4]{
		    RenderTime.FromMillisecs(0),	//none
		    RenderTime.FromMillisecs(16000),//drop
		    RenderTime.FromMillisecs(16000),//bounce
		    RenderTime.FromMillisecs(16000)//deflect
	    };

        public ItemAnim()
        {
            ActionDone = true;
        }

        public ItemAnim(Loc2D startLoc, Loc2D endLoc, int sprite, ItemAnimType action) {
            StartLoc = startLoc;
            EndLoc = endLoc;
            Sprite = sprite;
            Action = action;
            Loc2D diffLoc = startLoc - endLoc;
            TotalDistance = (int)(TextureManager.TILE_SIZE * Math.Sqrt(Math.Pow(diffLoc.X, 2) + Math.Pow(diffLoc.Y, 2)));
        }

        public int Frame {
            get;
            set;
        }

        public int Sprite { get; set; }

        public ItemAnimType Action { get; set; }

        public Loc2D StartLoc { get; set; }
        public Loc2D EndLoc { get; set; }


        public int TotalDistance { get; set; }

        public Loc2D MapLoc { get; set; }
        public int MapHeight { get; set; }

        public RenderTime ActionTime { get; set; }
        public bool ActionDone { get; set; }

        public virtual void Begin()
        {

        }

        public void Process(RenderTime elapsedTime) {
            ActionTime += elapsedTime;

            RenderTime totalTime = ITEM_ACTION_TIME[(int)Action];

            if (ActionTime >= totalTime && Action != ItemAnimType.None)
            {
                ActionDone = true;
            } else {
                switch (Action) {
                    case ItemAnimType.None:
                        {
                            MapHeight = 0;
                            MapLoc = StartLoc;
                            break;
                        }
                    case ItemAnimType.Drop: {
                        MapHeight = DrawHelper.GetArc(TextureManager.TILE_SIZE / 4, totalTime.Ticks, ActionTime.Ticks);
                            MapHeight += TextureManager.TILE_SIZE * (totalTime - ActionTime).Ticks / 2 / totalTime.Ticks;
                            Loc2D mapDiff = (EndLoc - StartLoc) * TextureManager.TILE_SIZE;
                            mapDiff = new Loc2D(mapDiff.X * ActionTime.Ticks / totalTime.Ticks, mapDiff.Y * ActionTime.Ticks / totalTime.Ticks);
                            MapLoc = mapDiff + StartLoc * TextureManager.TILE_SIZE;
                        }
                        break;
                    case ItemAnimType.Bounce: {
                        MapHeight = DrawHelper.GetArc(TextureManager.TILE_SIZE / 2, totalTime.Ticks, ActionTime.Ticks);
                            Loc2D mapDiff = (EndLoc - StartLoc) * TextureManager.TILE_SIZE;
                            mapDiff = new Loc2D(mapDiff.X * ActionTime.Ticks / totalTime.Ticks, mapDiff.Y * ActionTime.Ticks / totalTime.Ticks);
                            MapLoc = mapDiff + StartLoc * TextureManager.TILE_SIZE;
                        }
                        break;
                    case ItemAnimType.Deflect: {
                        MapHeight = DrawHelper.GetArc(TextureManager.TILE_SIZE / 2, totalTime.Ticks, ActionTime.Ticks);
                        MapHeight += TextureManager.TILE_SIZE * (totalTime.Ticks - ActionTime.Ticks) / 2 / totalTime.Ticks;
                            Loc2D mapDiff = (EndLoc - StartLoc) * TextureManager.TILE_SIZE;
                            mapDiff = new Loc2D(mapDiff.X * ActionTime.Ticks / totalTime.Ticks, mapDiff.Y * ActionTime.Ticks / totalTime.Ticks);
                            MapLoc = mapDiff + StartLoc * TextureManager.TILE_SIZE;
                        }
                        break;
                }
            }
        }

        public void Draw() {
            if (!ActionDone) {
                TextureManager.TextureProgram.PushModelView();
                Loc2D drawLoc = GetStart();

                Graphics.TextureManager.TextureProgram.LeftMultModelView(Matrix4.CreateTranslation(drawLoc.X, drawLoc.Y, 0));
                Graphics.TextureManager.TextureProgram.UpdateModelView();

                AnimSheet sheet = TextureManager.GetItemSheet(Sprite);
                sheet.RenderAnim(Frame, 0, 0);

                TextureManager.TextureProgram.PopModelView();
            }
        }

        public Loc2D GetStart() {
            return new Loc2D(MapLoc.X + TextureManager.TILE_SIZE / 2 - Graphics.TextureManager.GetItemSheet(Sprite).TileWidth / 2,
                MapLoc.Y + TextureManager.TILE_SIZE / 2 - Graphics.TextureManager.GetItemSheet(Sprite).TileHeight / 2 - MapHeight);
        }

        public Loc2D GetEnd() {
            return new Loc2D(MapLoc.X + TextureManager.TILE_SIZE / 2 + Graphics.TextureManager.GetItemSheet(Sprite).TileWidth / 2,
                MapLoc.Y + TextureManager.TILE_SIZE / 2 + Graphics.TextureManager.GetItemSheet(Sprite).TileHeight / 2 - MapHeight);
        }

    }
}
