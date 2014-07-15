using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PMDToolkit.Logic.Gameplay {
    public class MainMenu : SingleStripMenu {

        public MainMenu() {
            Initialize(new Maps.Loc2D(100, 100), 128,
            new string[8] {
                "Wait",
                "Restart",
                "Regenerate",
                "Replays",
                "Item Edit",
                "Move Edit",
                "Map Edit",
                "Dungeon Edit"}, 0);
        
        }

        protected override void Choose(ActiveChar character, ref bool moveMade) {
            switch (CurrentChoice) {
                case 0: {
                    MenuManager.Menus.Clear();
                        Processor.ProcessDecision(new Command(Command.CommandType.Wait), character, ref moveMade);
                    }
                    break;
                case 1:
                    {
                        MenuManager.Menus.Clear();
                        Processor.StartDungeon(Processor.Seed);
                    }
                    break;
                case 2:
                    {
                        MenuManager.Menus.Clear();
                        Processor.StartDungeon(Processor.Rand.Next());
                    }
                    break;
                case 3: {
                        this.Visible = false;
                        MenuManager.Menus.Insert(0, new ReplayMenu());
                    }
                    break;
                case 4: {//item
                        MenuManager.Menus.RemoveAt(0);
                        Editors.EditorManager.OpenItemEditor();
                    }
                    break;
                case 5: {//move
                        MenuManager.Menus.RemoveAt(0);
                        Editors.EditorManager.OpenSpellEditor();
                    }
                    break;
                case 6: {//map
                    MenuManager.Menus.RemoveAt(0);
                    //Editors.EditorManager.OpenMapEditor();

                    break;
                }
                case 7: {//dungeon
                        MenuManager.Menus.RemoveAt(0);
                        Editors.EditorManager.OpenRDungeonEditor();
                    }
                    break;
                default: {
                    MenuManager.Menus.RemoveAt(0);
                    }
                    break;
            }
        }

    }
}
