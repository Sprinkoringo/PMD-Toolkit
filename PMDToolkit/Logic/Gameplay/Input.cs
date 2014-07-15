using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK.Graphics;
using OpenTK.Input;
using OpenTK.Platform;
using PMDToolkit.Maps;

namespace PMDToolkit.Logic.Gameplay {
    public class Input {

        public enum InputType {
            Cancel,
            Confirm,
            Pickup,
            Moves,
            Move1,
            Move2,
            Move3,
            Move4,
            Turn,
            Diagonal,
            ItemMenu,
            MoveMenu,
            Menu
        }

        private bool[] inputStates = new bool[13];

        private Direction8 dir = Maps.Direction8.None;


        public bool this[InputType i] {
            get {
                return inputStates[(int)i];
            }
        }

        public int TotalInputs { get { return inputStates.Length; } }

        public Direction8 Direction {
            get {
                return dir;
            }
        }

        public bool LeftMouse { get; set; }
        public bool RightMouse { get; set; }
        public Loc2D MouseLoc { get; set; }
        public int MouseWheel { get; set; }

        public bool Shift { get; set; }

        public bool ShowDebug { get; set; }
        public bool SpeedDown { get; set; }
        public bool SpeedUp { get; set; }
        public bool Intangible { get; set; }
        public bool Print { get; set; }
        public bool Restart { get; set; }

        public Input() {
            dir = Direction8.None;
        }

        public Input(KeyboardDevice keyboard, MouseDevice mouse) {
            Loc2D dirLoc = new Loc2D();


            if (keyboard[Key.Down]) {
                Operations.MoveInDirection8(ref dirLoc, Direction8.Down, 1);
            }
            if (keyboard[Key.Left]) {
                Operations.MoveInDirection8(ref dirLoc, Direction8.Left, 1);
            }
            if (keyboard[Key.Up]) {
                Operations.MoveInDirection8(ref dirLoc, Direction8.Up, 1);
            }
            if (keyboard[Key.Right]) {
                Operations.MoveInDirection8(ref dirLoc, Direction8.Right, 1);
            }

            dir = Operations.GetDirection8(new Loc2D(), dirLoc);

            inputStates[(int)InputType.Confirm] = keyboard[Key.F];
            inputStates[(int)InputType.Cancel] = keyboard[Key.BackSpace];
            inputStates[(int)InputType.Pickup] = keyboard[Key.Enter];
            inputStates[(int)InputType.Moves] = keyboard[Key.Q];
            inputStates[(int)InputType.Move1] = keyboard[Key.W];
            inputStates[(int)InputType.Move2] = keyboard[Key.A];
            inputStates[(int)InputType.Move3] = keyboard[Key.S];
            inputStates[(int)InputType.Move4] = keyboard[Key.D];
            inputStates[(int)InputType.Turn] = keyboard[Key.T];
            inputStates[(int)InputType.Diagonal] = keyboard[Key.R];

            inputStates[(int)InputType.ItemMenu] = keyboard[Key.Number1];
            inputStates[(int)InputType.MoveMenu] = keyboard[Key.Number2];

            inputStates[(int)InputType.Menu] = (keyboard[Key.Escape]);

            LeftMouse = mouse[MouseButton.Left];
            RightMouse = mouse[MouseButton.Right];

            MouseWheel = mouse.Wheel;

            MouseLoc = new Loc2D(mouse.X, mouse.Y);

            Shift = keyboard[Key.ShiftLeft] || keyboard[Key.ShiftRight];

            ShowDebug = keyboard[Key.F1];
            SpeedDown = keyboard[Key.F2];
            SpeedUp = keyboard[Key.F3];
#if GAME_MODE
            Intangible = keyboard[Key.F4];
            Print = keyboard[Key.F5];
            Restart = keyboard[Key.F12];
#endif
        }

        public static bool operator ==(Input input1, Input input2) {
            if (input1.Direction != input2.Direction) return false;

            for (int i = 0; i < 9; i++) {
                if (input1[(InputType)i] != input2[(InputType)i]) return false;
            }

            return true;
        }

        public static bool operator !=(Input input1, Input input2) {
            return !(input1 == input2);
        }

    }
}
