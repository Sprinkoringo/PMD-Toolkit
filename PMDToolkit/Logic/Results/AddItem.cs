/*The MIT License (MIT)

Copyright (c) 2014 Sprinkoringo

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
