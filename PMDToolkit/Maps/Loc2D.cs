using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PMDToolkit.Maps {
    public struct Loc2D {

        public int X;
        public int Y;

        public Loc2D(int n)
        {
            X = n;
            Y = n;
        }

        public Loc2D(int x, int y)
        {
            X = x;
            Y = y;
        }

        public Loc2D(Loc2D loc) {
            X = loc.X;
            Y = loc.Y;
        }


        public static bool operator ==(Loc2D param1, Loc2D param2) {
            return (param1.X == param2.X && param1.Y == param2.Y);
        }

        public static bool operator !=(Loc2D param1, Loc2D param2) {
            return !(param1 == param2);
        }

        public static Loc2D operator +(Loc2D param1, Loc2D param2) {
            return new Loc2D(param1.X + param2.X, param1.Y + param2.Y);
        }

        public static Loc2D operator -(Loc2D param1, Loc2D param2) {
            return new Loc2D(param1.X - param2.X, param1.Y - param2.Y);
        }

        public static Loc2D operator *(Loc2D param1, int param2) {
            return new Loc2D(param1.X * param2, param1.Y * param2);
        }

        public static Loc2D operator /(Loc2D param1, int param2) {
            return new Loc2D(param1.X / param2, param1.Y / param2);
        }

        public static int Dot(Loc2D param1, Loc2D param2)
        {
            return param1.X * param2.X + param1.Y * param2.Y;
        }
    }
}
