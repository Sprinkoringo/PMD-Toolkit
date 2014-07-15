using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PMDToolkit.Maps;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using PMDToolkit.Graphics;

namespace PMDToolkit.Logic.Gameplay {
    public static class MenuManager {

        public static List<MenuBase> Menus { get; set; }

        public static void Init() {
            Menus = new List<MenuBase>();
        }

        public static void ProcessMenus(Input input, ActiveChar character, ref bool moveMade) {

            //process most recent menu
            Menus[0].Process(input, character, ref moveMade);

            //send all menus to display

        }

        public static void DrawMenus() {
            for (int i = 0; i < Menus.Count; i++) {
                Menus[i].Draw();
            }
        }

    }
}
