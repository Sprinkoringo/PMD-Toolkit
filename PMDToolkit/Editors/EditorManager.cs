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
using System.Threading;
using System.Windows.Forms;

namespace PMDToolkit.Editors {
    public class EditorManager {

        static Thread editThread;
        
        static object lockObj = new object();

        static void openEditor(Object data) {
            System.Windows.Forms.Application.Run((Form)data);
            editThread = null;
        }

        public static void OpenItemEditor() {
            lock (lockObj) {
                if (editThread == null) {
                    EditList choices = new EditList();
                    string[] entries = new string[Data.GameData.ItemDex.Length];
                    for (int i = 0; i < entries.Length; i++) {
                        entries[i] = Data.GameData.ItemDex[i].Name;
                    }
                    choices.AddEntries(entries);
                    System.Windows.Forms.Application.Run(choices);

                    if (choices.ChosenEntry > -1) {
                        currentEditor = new Editors.ItemEditor();
                        ((ItemEditor)currentEditor).LoadItem(choices.ChosenEntry);
                        editThread = new Thread(new ParameterizedThreadStart(openEditor));
                        editThread.Start(currentEditor);
                    }
                }
            }
        }

        public static void OpenSpellEditor() {
            lock (lockObj) {
                if (editThread == null) {
                    EditList choices = new EditList();
                    string[] entries = new string[Data.GameData.MoveDex.Length];
                    for (int i = 0; i < entries.Length; i++) {
                        entries[i] = Data.GameData.MoveDex[i].Name;
                    }
                    choices.AddEntries(entries);
                    System.Windows.Forms.Application.Run(choices);

                    if (choices.ChosenEntry > -1) {
                        currentEditor = new Editors.SpellEditor();
                        ((SpellEditor)currentEditor).LoadSpell(choices.ChosenEntry);
                        editThread = new Thread(new ParameterizedThreadStart(openEditor));
                        editThread.Start(currentEditor);
                    }
                }
            }
        }

        public static void OpenRDungeonEditor() {
            lock (lockObj) {
                if (editThread == null) {
                    EditList choices = new EditList();
                    string[] entries = new string[Data.GameData.RDungeonDex.Length];
                    for (int i = 0; i < entries.Length; i++) {
                        entries[i] = Data.GameData.RDungeonDex[i].Name;
                    }
                    choices.AddEntries(entries);
                    System.Windows.Forms.Application.Run(choices);

                    if (choices.ChosenEntry > -1) {
                        currentEditor = new Editors.RDungeonEditor();
                        ((RDungeonEditor)currentEditor).LoadRDungeon(choices.ChosenEntry);
                        editThread = new Thread(new ParameterizedThreadStart(openEditor));
                        editThread.Start(currentEditor);
                    }
                }
            }
        }

        static Form currentEditor;

    }
}
