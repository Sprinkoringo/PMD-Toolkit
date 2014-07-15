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
    public abstract class SingleStripMenu : MenuBase {

        private int currentChoice;
        public int CurrentChoice { get { return currentChoice; } }

        public override Loc2D PickerPos {
            get { return new Loc2D(Start.X + TextureManager.MenuBack.TileWidth*2 - TextureManager.Picker.ImageWidth,
                Start.Y + TextureManager.MenuBack.TileHeight + currentChoice * 10); }
        }

        protected void Initialize(Loc2D start, int width, string[] choices, int defaultChoice) {
            Start = start;
            int horizSpace = width;
            int vertSpace = 10;
            for(int i = 0; i < choices.Length; i++) {
                Choices.Add(new MenuText(choices[i], start + new Loc2D(TextureManager.MenuBack.TileWidth * 2, TextureManager.MenuBack.TileHeight + vertSpace * i)));
            }
            End = Start + new Loc2D(horizSpace, choices.Length * vertSpace + TextureManager.MenuBack.TileHeight * 2);
            currentChoice = defaultChoice;
        }

        public override void Process(Input input, ActiveChar character, ref bool moveMade) {
            if (input[Input.InputType.Menu] && !Processor.PrevInput[Input.InputType.Menu]) {
                Choose(character, ref moveMade);
            } else if (input[Input.InputType.Confirm] && !Processor.PrevInput[Input.InputType.Confirm]) {
                Choose(character, ref moveMade);
            } else if (input[Input.InputType.Cancel] && !Processor.PrevInput[Input.InputType.Cancel]) {
                MenuManager.Menus.RemoveAt(0);
            } else {
                bool chooseDown = (input.Direction == Direction8.Down || input.Direction == Direction8.DownLeft || input.Direction == Direction8.DownRight);
                bool prevDown = (Processor.PrevInput.Direction == Direction8.Down || Processor.PrevInput.Direction == Direction8.DownLeft || Processor.PrevInput.Direction == Direction8.DownRight);
                bool chooseUp = (input.Direction == Direction8.Up || input.Direction == Direction8.UpLeft || input.Direction == Direction8.UpRight);
                bool prevUp = (Processor.PrevInput.Direction == Direction8.Up || Processor.PrevInput.Direction == Direction8.UpLeft || Processor.PrevInput.Direction == Direction8.UpRight);
                if (chooseDown && (!prevDown || Processor.InputTime >= RenderTime.FromMillisecs(40)))
                {
                    currentChoice = (currentChoice + 1) % Choices.Count;
                } else if (chooseUp && (!prevUp || Processor.InputTime >= RenderTime.FromMillisecs(40))) {
                    currentChoice = (currentChoice + Choices.Count - 1) % Choices.Count;
                }
            }
        }

        protected abstract void Choose(ActiveChar character, ref bool moveMade);

    }
}
