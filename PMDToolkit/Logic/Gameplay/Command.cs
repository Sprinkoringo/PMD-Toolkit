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
    public struct Command {

        public enum CommandType {
            None = -1,
            Dir = 0,
            Move = 4,
            Attack = 5,
            AltAttack = 6,
            Pickup = 7,
            Use = 8,
            Drop = 9,
            Throw = 10,
            Spell = 11,
            Solid = 12,
            Speed = 13,
            Print = 14,
            Restart = 15,
            Wait = 16
        };

        public CommandType Type;
        private List<int> args;
        public int this[int index] {
            get {
                return args[index];
            }
        }
        public int ArgCount { get { return args.Count; } }


        public Command(CommandType type, params int[] args) {
            Type = type;
            this.args = new List<int>();
            for (int i = 0; i < args.Length; i++) {
                this.args.Add(args[i]);
            }
        }

        public void AddArg(int arg) {
            args.Add(arg);
        }

        public static bool operator ==(Command command1, Command command2) {
            if (command1.Type != command2.Type) return false;
            if(command1.ArgCount != command2.ArgCount) return false;
            for (int i = 0; i < command1.ArgCount; i++) {
                if (command1[i] != command2[i]) return false;
            }

            return true;
        }

        public static bool operator !=(Command command1, Command command2) {
            return !(command1 == command2);
        }
    }
}
