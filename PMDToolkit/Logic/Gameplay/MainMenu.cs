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
