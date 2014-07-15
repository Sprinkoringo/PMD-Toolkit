using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PMDToolkit.Logic.Gameplay {
    public class SpellMenu : SingleStripMenu {

        
        public SpellMenu() {
            string[] moves = new string[Processor.MAX_MOVE_SLOTS];
            for (int i = 0; i < Processor.MAX_MOVE_SLOTS; i++) {
                if (Processor.Moves[i].MoveNum >= 0) {
                    moves[i] = "(" + Data.GameData.MoveDex[Processor.Moves[i].MoveNum].PP + "PP) " + Data.GameData.MoveDex[Processor.Moves[i].MoveNum].Name;
                } else {
                    moves[i] = "Empty";
                }
            }

            Initialize(new Maps.Loc2D(100, 100), 256, moves, 0);
        
        }

        public override void Process(Input input, ActiveChar character, ref bool moveMade) {
            if (input[Input.InputType.MoveMenu] && !Processor.PrevInput[Input.InputType.MoveMenu]) {
                MenuManager.Menus.RemoveAt(0);
            } else {
                base.Process(input, character, ref moveMade);
            }
        }

        protected override void Choose(ActiveChar character, ref bool moveMade) {
            MenuManager.Menus.Clear();
            Processor.ProcessDecision(new Command(Command.CommandType.Spell, CurrentChoice), character, ref moveMade);
        }

    }
}
