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
