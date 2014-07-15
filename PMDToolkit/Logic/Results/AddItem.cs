using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PMDToolkit.Maps;
using PMDToolkit.Logic.Gameplay;
using PMDToolkit.Logic.Display;
using PMDToolkit.Graphics;
using PMDToolkit.Graphics;

namespace PMDToolkit.Logic.Results {
    public class AddItem : IResult {

        //public ResultType Type { get { return ResultType.SpawnNpc; } }
        public RenderTime Delay { get { return RenderTime.Zero; } }

        int itemSlot;
        int sprite;
        Loc2D itemLoc;

        public AddItem(ActiveMap map, int itemSlot) {
            this.itemSlot = itemSlot;
            Item item = map.Items[itemSlot];
            sprite = Data.GameData.ItemDex[item.ItemIndex].Sprite;
            itemLoc = item.ItemLoc;
        }

        public void Execute()
        {
            Screen.Items[itemSlot] = new ItemAnim(itemLoc * Graphics.TextureManager.TILE_SIZE, itemLoc * Graphics.TextureManager.TILE_SIZE, sprite, ItemAnim.ItemAnimType.None);
        }

    }
}
