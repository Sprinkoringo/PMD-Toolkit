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
