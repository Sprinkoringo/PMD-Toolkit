using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PMDToolkit.Logic.Gameplay {
    public class ItemMenu : SingleStripMenu {
        
        
        public ItemMenu() {
            string[] moves = new string[Processor.MAX_INV_SLOTS];
            for (int i = 0; i < Processor.MAX_INV_SLOTS; i++) {
                if (Processor.Inventory[i] >= 0) {
                    moves[i] = Data.GameData.ItemDex[Processor.Inventory[i]].Name;
                } else {
                    moves[i] = "Empty";
                }
            }

            Initialize(new Maps.Loc2D(100, 100), 256, moves, 0);
        
        }

        public override void Process(Input input, ActiveChar character, ref bool moveMade) {
            if (input[Input.InputType.ItemMenu] && !Processor.PrevInput[Input.InputType.ItemMenu]) {
                MenuManager.Menus.RemoveAt(0);
            } else {
                base.Process(input, character, ref moveMade);
            }
        }

        protected override void Choose(ActiveChar character, ref bool moveMade) {
            if (Processor.Inventory[CurrentChoice] >= 0) {
                MenuManager.Menus.Insert(0, new ItemChosenMenu(CurrentChoice));
            }
        }
    }
}
