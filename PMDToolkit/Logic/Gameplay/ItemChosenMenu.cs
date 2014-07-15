using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PMDToolkit.Logic.Gameplay {
    public class ItemChosenMenu : SingleStripMenu {

        private int invSlot;
        
        public ItemChosenMenu(int invSlot) {
            this.invSlot = invSlot;
            string[] choices = new string[3] {
                "Use",
                "Throw",
                "Drop"
            };

            Initialize(new Maps.Loc2D(356, 100), 64, choices, 0);
        
        }

        protected override void Choose(ActiveChar character, ref bool moveMade) {
            switch (CurrentChoice) {
                case 0: {//use
                        MenuManager.Menus.Clear();
                        Processor.ProcessDecision(new Command(Command.CommandType.Use, invSlot), character, ref moveMade);
                    }
                    break;
                case 1: {//throw
                        MenuManager.Menus.Clear();
                        Processor.ProcessDecision(new Command(Command.CommandType.Throw, invSlot), character, ref moveMade);
                    }
                    break;
                case 2: {//drop
                        MenuManager.Menus.Clear();
                        Processor.ProcessDecision(new Command(Command.CommandType.Drop, invSlot), character, ref moveMade);
                    }
                    break;
            }
        }

    }
}
