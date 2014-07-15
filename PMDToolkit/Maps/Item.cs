using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PMDToolkit.Maps;
using PMDToolkit.Graphics;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace PMDToolkit.Maps {
    public class Item {

        public int ItemIndex { get; set; }
        public int Amount { get; set; }
        public string Tag { get; set; }
        public bool Disabled { get; set; }
        public Loc2D ItemLoc { get; set; }

        public Item()
        {
            ItemIndex = -1;
            Amount = 0;
            Tag = "";
            Disabled = false;
            ItemLoc = new Loc2D();
        }

        public Item(int itemIndex, int amount, string tag, bool disabled, Loc2D loc) {
            ItemIndex = itemIndex;
            Amount = amount;
            Tag = tag;
            Disabled = disabled;
            ItemLoc = loc;
        }

        public void Draw() {
            if (ItemIndex > -1) {

                Graphics.AnimSheet sheet = Graphics.TextureManager.GetItemSheet(Data.GameData.ItemDex[ItemIndex].Sprite);

                TextureManager.TextureProgram.PushModelView();
                TextureManager.TextureProgram.LeftMultModelView(Matrix4.CreateTranslation((Graphics.TextureManager.TILE_SIZE - sheet.TileWidth) / 2, (Graphics.TextureManager.TILE_SIZE - sheet.TileHeight) / 2, 0));
                TextureManager.TextureProgram.UpdateModelView();

                sheet.RenderTile(0, 0);

                TextureManager.TextureProgram.PopModelView();
            }
        }

    }
}
