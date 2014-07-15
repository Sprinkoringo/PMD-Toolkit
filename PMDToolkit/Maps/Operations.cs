using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PMDToolkit.Maps
{

    public enum Direction4
    {
        All = -3,
        Any = -2,
        None = -1,
        Down = 0,
        Left = 1,
        Up = 2,
        Right = 3
    };

    public enum Direction8
    {
        All = -3,
        Any = -2,
        None = -1,
        Down = 0,
        Left = 1,
        Up = 2,
        Right = 3,
        DownLeft = 4,
        UpLeft = 5,
        UpRight = 6,
        DownRight = 7
    };

    public enum Orientation4
    {
        None = -1,
        Vert = 0,
        Horiz = 1
    };

    public enum Orientation8
    {
        None = -1,
        Vert = 0,
        Horiz = 1,
        DiagForth = 2,
        DiagBack = 3
    };


    public enum Direction3D
    {
        All = -3,
        Any = -2,
        None = -1,
        Down = 0,
        Left = 1,
        Up = 2,
        Right = 3,
        Forth = 4,
        Back = 5
    };

    public enum Orientation3D
    {
        None = -1,
        Vert = 0,
        Horiz = 1,
        Depth = 2
    };

    public static class Operations
    {

        public static void MoveInDirection4(ref Loc2D loc, Direction4 dir, int length)
        {
            MoveInDirection4(ref loc.X, ref loc.Y, dir, length);
        }

        public static void MoveInDirection4(ref int x, ref int y, Direction4 dir, int length)
        {
            switch (dir)
            {
                case Direction4.Down:
                    {
                        y += length;
                    }
                    break;
                case Direction4.Up:
                    {
                        y -= length;
                    }
                    break;
                default: break;
            }

            switch (dir)
            {
                case Direction4.Left:
                    {
                        x -= length;
                    }
                    break;
                case Direction4.Right:
                    {
                        x += length;
                    }
                    break;
                default: break;
            }

        }


        public static void MoveInDirection8(ref Loc2D loc, Direction8 dir, int length)
        {
            MoveInDirection8(ref loc.X, ref loc.Y, dir, length);
        }

        public static void MoveInDirection8(ref int x, ref int y, Direction8 dir, int length)
        {
            switch (dir)
            {
                case Direction8.Down:
                case Direction8.DownLeft:
                case Direction8.DownRight:
                    {
                        y += length;
                    }
                    break;
                case Direction8.Up:
                case Direction8.UpLeft:
                case Direction8.UpRight:
                    {
                        y -= length;
                    }
                    break;
                default: break;
            }

            switch (dir)
            {
                case Direction8.Left:
                case Direction8.UpLeft:
                case Direction8.DownLeft:
                    {
                        x -= length;
                    }
                    break;
                case Direction8.Right:
                case Direction8.UpRight:
                case Direction8.DownRight:
                    {
                        x += length;
                    }
                    break;
                default: break;
            }

        }

        public static void MoveInDirection3D(ref Loc3D loc, Direction3D dir, int length)
        {
            MoveInDirection3D(ref loc.X, ref loc.Y, loc.Z, dir, length);
        }

        public static void MoveInDirection3D(ref int x, ref int y, int z, Direction3D dir, int length)
        {
            switch (dir)
            {
                case Direction3D.Down:
                    {
                        y += length;
                    }
                    break;
                case Direction3D.Up:
                    {
                        y -= length;
                    }
                    break;
                default: break;
            }

            switch (dir)
            {
                case Direction3D.Left:
                    {
                        x -= length;
                    }
                    break;
                case Direction3D.Right:
                    {
                        x += length;
                    }
                    break;
                default: break;
            }

            switch (dir)
            {
                case Direction3D.Forth:
                    {
                        z -= length;
                    }
                    break;
                case Direction3D.Back:
                    {
                        z += length;
                    }
                    break;
                default: break;
            }

        }

        public static bool CheckCollide(int x1, int y1, int w1, int h1,
            int x2, int y2, int w2, int h2)
        {

            return (x1 + w1 >= x2 && x1 <= x2 + w2 &&
                y1 + h1 >= y2 && y1 <= y2 + h2);

        }

        public static Direction4 GetDirection4(Loc2D loc1, Loc2D loc2)
        {
            return GetDirection4(loc1.X, loc1.Y, loc2.X, loc2.Y);
        }

        public static Direction4 GetDirection4(int x1, int y1, int x2, int y2)
        {
            if (y2 > y1)
            {
                if (x2 > x1)
                {
                    throw new Exception("Invalid coordinates");
                }
                else if (x2 == x1)
                {
                    return Direction4.Down;
                }
                else
                {
                    throw new Exception("Invalid coordinates");
                }
            }
            else if (y2 == y1)
            {
                if (x2 > x1)
                {
                    return Direction4.Right;
                }
                else if (x2 == x1)
                {
                    return Direction4.None;
                }
                else
                {
                    return Direction4.Left;
                }
            }
            else
            {
                if (x2 > x1)
                {
                    throw new Exception("Invalid coordinates");
                }
                else if (x2 == x1)
                {
                    return Direction4.Up;
                }
                else
                {
                    throw new Exception("Invalid coordinates");
                }
            }
        }

        public static Direction8 GetDirection8(Loc2D loc1, Loc2D loc2)
        {
            return GetDirection8(loc1.X, loc1.Y, loc2.X, loc2.Y);
        }

        public static Direction8 GetDirection8(int x1, int y1, int x2, int y2)
        {
            if (y2 > y1)
            {
                if (x2 > x1)
                {
                    return Direction8.DownRight;
                }
                else if (x2 == x1)
                {
                    return Direction8.Down;
                }
                else
                {
                    return Direction8.DownLeft;
                }
            }
            else if (y2 == y1)
            {
                if (x2 > x1)
                {
                    return Direction8.Right;
                }
                else if (x2 == x1)
                {
                    return Direction8.None;
                }
                else
                {
                    return Direction8.Left;
                }
            }
            else
            {
                if (x2 > x1)
                {
                    return Direction8.UpRight;
                }
                else if (x2 == x1)
                {
                    return Direction8.Up;
                }
                else
                {
                    return Direction8.UpLeft;
                }
            }
        }

        public static Direction3D GetDirection3D(Loc3D loc1, Loc3D loc2)
        {
            return GetDirection3D(loc1.X, loc1.Y, loc1.Z, loc2.X, loc2.Y, loc2.Z);
        }

        public static Direction3D GetDirection3D(int x1, int y1, int z1, int x2, int y2, int z2)
        {
            int cmpx = 0;
            if (x1 > x2)
            {
                cmpx = 1;
            }
            else if (x1 < x2)
            {
                cmpx = -1;
            }
            int cmpy = 0;
            if (y1 > y2)
            {
                cmpy = 1;
            }
            else if (y1 < y2)
            {
                cmpy = -1;
            }
            int cmpz = 0;
            if (z1 > z2)
            {
                cmpz = 1;
            }
            else if (z1 < z2)
            {
                cmpz = -1;
            }

            if (cmpx != 0 && cmpy == 0 && cmpz == 0)
            {
                if (cmpx > 0)
                {
                    return Direction3D.Left;
                }
                else
                {
                    return Direction3D.Right;
                }
            }
            else if (cmpx == 0 && cmpy != 0 && cmpz == 0)
            {
                if (cmpy > 0)
                {
                    return Direction3D.Up;
                }
                else
                {
                    return Direction3D.Down;
                }
            }
            else if (cmpx == 0 && cmpy == 0 && cmpz != 0)
            {
                if (cmpz > 0)
                {
                    return Direction3D.Forth;
                }
                else
                {
                    return Direction3D.Back;
                }
            }
            else
            {
                throw new Exception("Invalid coordinates");
            }

        }

        public static Orientation4 GetOrientation4(Direction4 dir)
        {
            switch (dir)
            {
                case Direction4.Down:
                case Direction4.Up:
                    return Orientation4.Vert;
                case Direction4.Left:
                case Direction4.Right:
                    return Orientation4.Horiz;
                default:
                    return Orientation4.None;
            }
        }

        public static bool IsDiagonal(Direction8 dir)
        {
            Orientation8 ori = GetOrientation8(dir);
            return (ori == Orientation8.DiagBack || ori == Orientation8.DiagForth);
        }

        public static Orientation8 GetOrientation8(Direction8 dir)
        {
            switch (dir)
            {
                case Direction8.Down:
                case Direction8.Up:
                    return Orientation8.Vert;
                case Direction8.Left:
                case Direction8.Right:
                    return Orientation8.Horiz;
                case Direction8.DownLeft:
                case Direction8.UpRight:
                    return Orientation8.DiagForth;
                case Direction8.UpLeft:
                case Direction8.DownRight:
                    return Orientation8.DiagBack;
                default:
                    return Orientation8.None;
            }
        }

        public static Orientation3D GetOrientation3D(Direction3D dir)
        {
            switch (dir)
            {
                case Direction3D.Down:
                case Direction3D.Up:
                    return Orientation3D.Vert;
                case Direction3D.Left:
                case Direction3D.Right:
                    return Orientation3D.Horiz;
                case Direction3D.Forth:
                case Direction3D.Back:
                    return Orientation3D.Depth;
                default:
                    return Orientation3D.None;
            }
        }

        public static Direction4 ReverseDir(Direction4 dir)
        {
            if ((int)dir <= (int)Direction4.None) return dir;
            return (Direction4)(((int)dir + 2) % 4);
        }

        public static Direction4 AddDir(Direction4 dir1, Direction4 dir2)
        {
            if ((int)dir1 <= (int)Direction4.None) return dir2;
            if ((int)dir2 <= (int)Direction4.None) return dir1;
            return (Direction4)(((int)dir1 + (int)dir2) % 4);
        }

        public static Direction8 ReverseDir(Direction8 dir)
        {
            if ((int)dir <= (int)Direction8.None) return dir;
            return (Direction8)(((int)dir + 2) % 4 + (int)dir / 4 * 4);
        }

        public static Direction8 AddDir(Direction8 dir1, Direction8 dir2)
        {
            if ((int)dir1 <= (int)Direction8.None) return dir2;
            if ((int)dir2 <= (int)Direction8.None) return dir1;
            bool dir1Diag = IsDiagonal(dir1);
            bool dir2Diag = IsDiagonal(dir2);
            if (!dir1Diag && !dir2Diag)
            {
                return (Direction8)(((int)dir1 + (int)dir2) % 4);
            }
            else if (dir1Diag && dir2Diag)
            {
                return (Direction8)(((int)dir1 + (int)dir2 - 3) % 4);
            }
            else
            {
                return (Direction8)(((int)dir1 + (int)dir2) % 8 + ((int)dir1 + (int)dir2) / 8 * 4);
            }

        }

        public static Direction3D ReverseDir3D(Direction3D dir)
        {
            switch (dir)
            {
                case Direction3D.Down: return Direction3D.Up;
                case Direction3D.Up: return Direction3D.Down;
                case Direction3D.Left: return Direction3D.Right;
                case Direction3D.Right: return Direction3D.Left;
                case Direction3D.Forth: return Direction3D.Back;
                case Direction3D.Back: return Direction3D.Forth;
                default:
                    return Direction3D.None;
            }
        }

        //returns the direction in which the point is in relation to the rectangle
        public static Direction8 GetOuterBound8(int x, int y, int w, int h, int px, int py)
        {
            if (py < y)
            {
                if (px < x)
                {
                    return Direction8.UpLeft;
                }
                else if (px < x + w)
                {
                    return Direction8.Up;
                }
                else
                {
                    return Direction8.UpRight;
                }
            }
            else if (py < y + h)
            {
                if (px < x)
                {
                    return Direction8.Left;
                }
                else if (px < x + w)
                {
                    return Direction8.None;
                }
                else
                {
                    return Direction8.Right;
                }
            }
            else
            {
                if (px < x)
                {
                    return Direction8.DownLeft;
                }
                else if (px < x + w)
                {
                    return Direction8.Down;
                }
                else
                {
                    return Direction8.DownRight;
                }
            }
        }

        //returns the direction in which the point is in relation to the rectangle
        public static Direction4 GetOuterBound4(int x, int y, int w, int h, int px, int py)
        {
            if (py < y)
            {
                if (px < x)
                {
                    throw new Exception("Invalid coordinates");
                }
                else if (px < x + w)
                {
                    return Direction4.Up;
                }
                else
                {
                    throw new Exception("Invalid coordinates");
                }
            }
            else if (py < y + h)
            {
                if (px < x)
                {
                    return Direction4.Left;
                }
                else if (px < x + w)
                {
                    return Direction4.None;
                }
                else
                {
                    return Direction4.Right;
                }
            }
            else
            {
                if (px < x)
                {
                    throw new Exception("Invalid coordinates");
                }
                else if (px < x + w)
                {
                    return Direction4.Down;
                }
                else
                {
                    throw new Exception("Invalid coordinates");
                }
            }
        }

        public static int GetFarthestDistance(int x, int y, int w, int h, int px, int py, Direction4 dir)
        {
            int dy = 0;
            int dx = 0;
            bool moveX = false;
            bool moveY = false;

            switch (dir)
            {
                case Direction4.Down:
                    {
                        dy = (y + h) - py - 1;
                        moveY = true;
                    }
                    break;
                case Direction4.Up:
                    {
                        dy = py - y;
                        moveY = true;
                    }
                    break;
                default: break;
            }

            switch (dir)
            {
                case Direction4.Left:
                    {
                        dx = px - x;
                        moveX = true;
                    }
                    break;
                case Direction4.Right:
                    {
                        dx = (x + w) - px - 1;
                        moveX = true;
                    }
                    break;
                default: break;
            }

            if (moveY && moveX)
            {
                throw new Exception("Invalid coordinates");
            }
            else if (moveX)
            {
                return dx;
            }
            else if (moveY)
            {
                return dy;
            }

            return 0;
        }

        public static bool IsInBound(int w, int h, int px, int py)
        {
            return (GetOuterBound8(0, 0, w, h, px, py) == Direction8.None);
        }

        public static bool IsPointBetween(Loc2D loc1, Loc2D loc2, Loc2D p, bool x1Excl, bool x2Excl)
        {
            return (IsPointBetween(loc1.X, loc2.X, p.X, x1Excl, x2Excl) && IsPointBetween(loc1.Y, loc2.Y, p.Y, x1Excl, x2Excl));
        }

        public static bool IsPointBetween(int x1, int x2, int p, bool x1Excl, bool x2Excl)
        {
            if (x1Excl && x2Excl)
            {
                return (p < x2 && p > x1 || p < x1 && p > x2);
            }
            else if (x1Excl && !x2Excl)
            {
                return (p < x2 && p >= x1 || p <= x1 && p > x2);
            }
            else if (!x1Excl && x2Excl)
            {
                return (p <= x2 && p > x1 || p < x1 && p >= x2);
            }
            else
            {
                return (p <= x2 && p >= x1 || p <= x1 && p >= x2);
            }
        }

        public static int DimOfDir(int x, int y, Direction4 dir)
        {
            return DimOfDir(x, y, GetOrientation4(dir));
        }

        public static int DimOfDir(int x, int y, Orientation4 ori)
        {
            if (ori == Orientation4.Vert)
            {
                return y;
            }
            else
            {
                return x;
            }
        }

        public static void AssignDirDim(int x, int y, int p, Direction4 dir)
        {
            if (dir == Direction4.Down || dir == Direction4.Up)
            {
                y = p;
            }
            else
            {
                x = p;
            }
        }

        public static int DimOfDir3D(int x, int y, int z, Direction3D dir)
        {
            return DimOfDir3D(x, y, z, GetOrientation3D(dir));
        }

        public static int DimOfDir3D(int x, int y, int z, Orientation3D ori)
        {
            if (ori == Orientation3D.Vert)
            {
                return y;
            }
            else if (ori == Orientation3D.Horiz)
            {
                return x;
            }
            else
            {
                return z;
            }
        }

        public static void AssignDirDim3D(int x, int y, int z, int p, Direction3D dir)
        {
            if (dir == Direction3D.Down || dir == Direction3D.Up)
            {
                y = p;
            }
            else if (dir == Direction3D.Left || dir == Direction3D.Right)
            {
                x = p;
            }
            else
            {
                z = p;
            }
        }

        public static int DistanceInFront(Loc2D loc1, Loc2D loc2, Direction4 dir)
        {
            return DistanceInFront(loc1.X, loc1.Y, loc2.X, loc2.Y, dir);
        }

        public static int DistanceInFront(int x1, int y1, int x2, int y2, Direction4 dir)
        {
            switch (dir)
            {
                case Direction4.Down:
                    {
                        return y2 - y1;
                    }
                    break;
                case Direction4.Left:
                    {
                        return x1 - x2;
                    }
                    break;
                case Direction4.Up:
                    {
                        return y1 - y2;
                    }
                    break;
                case Direction4.Right:
                    {
                        return x2 - x1;
                    }
                    break;
                default: return 0;
            }
        }

        public static int DistanceInFront3D(Loc3D loc1, Loc3D loc2, Direction3D dir)
        {
            return DistanceInFront3D(loc1.X, loc1.Y, loc1.Z, loc2.X, loc2.Y, loc2.Z, dir);
        }

        public static int DistanceInFront3D(int x1, int y1, int z1, int x2, int y2, int z2, Direction3D dir)
        {
            switch (dir)
            {
                case Direction3D.Down:
                    {
                        return y2 - y1;
                    }
                    break;
                case Direction3D.Left:
                    {
                        return x1 - x2;
                    }
                    break;
                case Direction3D.Up:
                    {
                        return y1 - y2;
                    }
                    break;
                case Direction3D.Right:
                    {
                        return x2 - x1;
                    }
                    break;
                case Direction3D.Forth:
                    {
                        return z1 - z2;
                    }
                    break;
                case Direction3D.Back:
                    {
                        return z2 - z1;
                    }
                    break;
                default: return 0;
            }
        }

        public static bool DoLinesOverlap(int x1, int x2, int y1, int y2)
        {
            if (x1 > x2) return DoLinesOverlap(x2, x1, y1, y2);
            if (y1 > y2) return DoLinesOverlap(x1, x2, y2, y1);

            return (x2 > y1 && x1 < y2);
        }

        public static void DecomposeDiagonal(Direction8 dir, ref Direction4 horiz, ref Direction4 vert)
        {
            switch (dir)
            {
                case Direction8.Down:
                case Direction8.DownLeft:
                case Direction8.DownRight:
                    {
                        vert = Direction4.Down;
                    }
                    break;
                case Direction8.Up:
                case Direction8.UpLeft:
                case Direction8.UpRight:
                    {
                        vert = Direction4.Up;
                    }
                    break;
                default: break;
            }

            switch (dir)
            {
                case Direction8.Left:
                case Direction8.UpLeft:
                case Direction8.DownLeft:
                    {
                        horiz = Direction4.Left;
                    }
                    break;
                case Direction8.Right:
                case Direction8.UpRight:
                case Direction8.DownRight:
                    {
                        horiz = Direction4.Right;
                    }
                    break;
                default: break;
            }
        }

        public static void ResizeArray<T>(ref T[,] array, int width, int height, Direction8 dir) where T : struct
        {
            Direction4 vert = Direction4.None;
            Direction4 horiz = Direction4.None;
            DecomposeDiagonal(dir, ref horiz, ref vert);

            Loc2D offset = new Loc2D();
            if (horiz == Direction4.None)
            {
                offset.X = (width - array.GetLength(0)) / 2;
            }
            else if (horiz == Direction4.Left)
            {
                offset.X = (width - array.GetLength(0));
            }

            if (vert == Direction4.None)
            {
                offset.Y = (height - array.GetLength(1)) / 2;
            }
            else if (vert == Direction4.Up)
            {
                offset.Y = (height - array.GetLength(1));
            }


            T[,] returnArray = new T[width, height];
            for (int x = Math.Max(-offset.X, 0); x < array.GetLength(0) && x + offset.X < returnArray.GetLength(0); x++)
            {
                for (int y = Math.Max(-offset.Y, 0); y < array.GetLength(1) && y + offset.Y < returnArray.GetLength(1); y++)
                {
                    returnArray[x + offset.X, y + offset.Y] = array[x, y];
                }
            }

            array = returnArray;
        }


        public static void ResizeArray<T>(ref T[,] array, int width, int height, Direction8 dir, bool initialize) where T : new()
        {
            Direction4 vert = Direction4.None;
            Direction4 horiz = Direction4.None;
            DecomposeDiagonal(dir, ref horiz, ref vert);

            Loc2D offset = new Loc2D();
            if (horiz == Direction4.None)
            {
                offset.X = (width - array.GetLength(0)) / 2;
            }
            else if (horiz == Direction4.Left)
            {
                offset.X = (width - array.GetLength(0));
            }

            if (vert == Direction4.None)
            {
                offset.Y = (height - array.GetLength(1)) / 2;
            }
            else if (vert == Direction4.Up)
            {
                offset.Y = (height - array.GetLength(1));
            }


            T[,] returnArray = new T[width, height];
            if (initialize)
            {
                for (int x = 0; x < width; x++)
                {
                    for (int y = 0; y < height; y++)
                    {
                        if (x >= offset.X && x < offset.X + array.GetLength(0) &&
                            y >= offset.Y && y < offset.Y + array.GetLength(1))
                        {
                            returnArray[x, y] = array[x - offset.X, y - offset.Y];
                        }
                        else
                        {
                            returnArray[x, y] = new T();
                        }
                    }
                }
            }
            else
            {
                for (int x = Math.Max(-offset.X, 0); x < array.GetLength(0) && x + offset.X < returnArray.GetLength(0); x++)
                {
                    for (int y = Math.Max(-offset.Y, 0); y < array.GetLength(1) && y + offset.Y < returnArray.GetLength(1); y++)
                    {
                        returnArray[x + offset.X, y + offset.Y] = array[x, y];
                    }
                }
            }

            array = returnArray;
        }

        public static Loc2D GetResizeOffset(int oldWidth, int oldHeight, int width, int height, Direction8 dir)
        {
            Direction4 vert = Direction4.None;
            Direction4 horiz = Direction4.None;
            DecomposeDiagonal(dir, ref horiz, ref vert);

            Loc2D center = new Loc2D();
            if (horiz == Direction4.None)
                center.X += (width - oldWidth)/2;
            else if (horiz == Direction4.Left)
                center.X += (width - oldWidth);

            if (vert == Direction4.None)
                center.Y += (height - oldHeight) / 2;
            else if (vert == Direction4.Up)
                center.Y += (height - oldHeight);

            return center;
        }






        public delegate bool TileCheck(int locX, int locY);
        public delegate void TileOperation(int locX, int locY);


        static void AddNextScanLine( TileCheck checkOp, TileOperation fillOp,
            int min, int max, int range_min, int range_max, int y, bool isNext, Direction4 dir, 
            Stack<Tuple<int, int, int, Direction4, bool, bool>> stack)
        {
            int rMinX = range_min;
            bool inRange = false;
            int x = range_min;
            for (; x <= range_max; x++)
            {
                //// skip testing, if testing previous line within previous range
                bool empty = (isNext || (x < min || x > max)) && checkOp(x,y);
                
                if (!inRange && empty)
                {
                    rMinX = x;
                    inRange = true;
                }
                else if (inRange && !empty)
                {
                    stack.Push(new Tuple<int, int, int, Direction4, bool, bool>(rMinX, x-1, y, dir, rMinX == range_min, false));
                    inRange = false;
                }
                
                if (inRange)
                    fillOp(x,y);
                
                if (!isNext && x == min)
                    break;
            }
            if (inRange)
                stack.Push(new Tuple<int, int, int, Direction4, bool, bool>(rMinX, x - 1, y, dir, rMinX == range_min, true));

        }


        public static void FillArray(int arrayWidth, int arrayHeight, TileCheck checkOp, TileOperation fillOp, Loc2D loc)
        {
            Stack<Tuple<int, int, int, Direction4, bool, bool>> stack = new Stack<Tuple<int, int, int, Direction4, bool, bool>>();
            stack.Push(new Tuple<int, int, int, Direction4, bool, bool>(loc.X, loc.X, loc.Y, Direction4.None, true, true));
            fillOp(loc.X, loc.Y);

            while (stack.Count > 0)
            {
                Tuple<int, int, int, Direction4, bool, bool> this_should_really_be_a_class = stack.Pop();
                Direction4 dir = this_should_really_be_a_class.Item4;
                int minX = this_should_really_be_a_class.Item1;
                int maxX = this_should_really_be_a_class.Item2;
                int y = this_should_really_be_a_class.Item3;
                bool goLeft = this_should_really_be_a_class.Item5;
                bool goRight = this_should_really_be_a_class.Item6;

                int newMinX = minX;
                if (goLeft)
                {
                    while(newMinX - 1 >= 0 && checkOp(newMinX-1, y))
                    {
                        newMinX--;
                        fillOp(newMinX, y);
                    }
                }

                int newMaxX = maxX;
                if (goRight)
                {
                    while (newMaxX + 1 < arrayWidth && checkOp(newMaxX + 1, y))
                    {
                        newMaxX++;
                        fillOp(newMaxX, y);
                    }
                }

                minX--;
                maxX++;

                if (y < arrayHeight - 1)
                    AddNextScanLine(checkOp, fillOp, minX, maxX, newMinX, newMaxX, y + 1, dir != Direction4.Up, Direction4.Down, stack);

                if (y > 0)
                    AddNextScanLine(checkOp, fillOp, minX, maxX, newMinX, newMaxX, y - 1, dir != Direction4.Down, Direction4.Up, stack);
            }
        }
    }
}
