using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace PMDToolkit.Logic.Gameplay {
    public class ReplayMenu : SingleStripMenu {


        public ReplayMenu() {
            string[] directories = Directory.GetFiles("Logs/Journey/");

            Initialize(new Maps.Loc2D(100, 100), 512, directories, 0);
        }

        public override void Process(Input input, ActiveChar character, ref bool moveMade) {
            base.Process(input, character, ref moveMade);
        }

        protected override void Choose(ActiveChar character, ref bool moveMade) {
            MenuManager.Menus.Clear();
            Processor.StartReplay(Choices[CurrentChoice].Text);
        }
    }
}
