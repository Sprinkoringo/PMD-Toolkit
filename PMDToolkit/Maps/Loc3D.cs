using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PMDToolkit.Maps {
    public struct Loc3D {

        public int X;
        public int Y;
        public int Z;

        public Loc3D(int x, int y, int z) {
            X = x;
            Y = y;
            Z = z;
        }

        public Loc3D(Loc3D loc) {
            X = loc.X;
            Y = loc.Y;
            Z = loc.Z;
        }

        public Loc2D To2D() {
            return new Loc2D(X, Y);
        }

        public static bool operator ==(Loc3D param1, Loc3D param2) {
            return (param1.X == param2.X && param1.Y == param2.Y && param1.Z == param2.Z);
        }

        public static bool operator !=(Loc3D param1, Loc3D param2) {
            return !(param1 == param2);
        }


        public static Loc3D operator +(Loc3D param1, Loc3D param2) {
            return new Loc3D(param1.X + param2.X, param1.Y + param2.Y, param1.Z + param2.Z);
        }

        public static Loc3D operator -(Loc3D param1, Loc3D param2) {
            return new Loc3D(param1.X - param2.X, param1.Y - param2.Y, param1.Z - param2.Z);
        }
    }
}
