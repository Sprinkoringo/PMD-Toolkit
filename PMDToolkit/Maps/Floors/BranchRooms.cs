using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PMDToolkit;
using PMDToolkit.Maps;
using PMDToolkit.Data;
using PMDToolkit.Logic.Gameplay;
using System.Drawing;

namespace PMDToolkit.Maps.Floors
{

    public class BranchRooms : RandomMap
    {

        int minLineSize = 8;
        int maxLineSize = 16;
        int minPassageSize = 0;
        int maxPassageSize = 3;
        int minSizeChangePeriod = 3;
        int maxSizeChangePeriod = 7;
        double mainFreedom = 0.5;

        private Loc2D StartPoint { get { return BorderPoints[0]; } }

        private int[,] AgeArray;
        private Loc2D TopLeft;

        private BranchLine startLine;
        private int maxDepth;
        private int branches;

        public BranchRooms()
        {

        }

        //an initial create-map method
        public override void Generate(int seed, RDungeonFloor entry, List<FloorBorder> floorBorders, Dictionary<int, List<int>> borderLinks)
        {
            //TODO: make sure that this algorithm follows floorBorders and borderLinks constraints

            this.seed = seed;
            this.entry = entry;
            FloorBorders = floorBorders;
            BorderLinks = borderLinks;

            BorderPoints = new Loc2D[2];

            AgeArray = new int[20, 20];
            TopLeft = new Loc2D(-AgeArray.GetLength(0) / 2, -AgeArray.GetLength(1) / 2);

            rand = new Random(seed);

            maxDepth = 8;
            branches = 5;

            startLine = new BranchLine(new PointF(0, 0), new PointF(0, 0));

            CreatePath(startLine, 0);

            AddBranches(branches);

            AddBlobs(3, 500, 3, 6, 1, 2, 0, 3);

            if (rand.Next() % 100 < 30)
                AddBlob(startLine, 100, 3, 6, 1, 4, 0, 3);

            BorderPoints[0] = TopLeft * -1;
            BorderPoints[1] = TopLeft * -1;

            ConvertToMapArray();
        }

        private void ConvertToMapArray()
        {
            MapArray = new Tile[AgeArray.GetLength(0), AgeArray.GetLength(1)];
            MapLayer ground = new MapLayer(AgeArray.GetLength(0), AgeArray.GetLength(1));
            GroundLayers.Add(ground);
            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    if (AgeArray[x, y] != -1)
                    {
                        MapArray[x, y] = new Tile(PMDToolkit.Enums.TileType.Blocked, 0, 0, 0);

                        bool[] blockedDirs = new bool[8];
                        for (int n = 0; n < 8; n++)
                        {
                            blockedDirs[n] = IsBlocked(x, y, (Direction8)n);
                        }
                        if (blockedDirs[(int)Direction8.Down] && blockedDirs[(int)Direction8.Left] && blockedDirs[(int)Direction8.Up] && blockedDirs[(int)Direction8.Right])
                        {
                            int layer = 0;
                            if (!blockedDirs[(int)Direction8.DownLeft])
                                layer += 8 * 2;

                            if (!blockedDirs[(int)Direction8.UpLeft])
                                layer += 1;

                            if (!blockedDirs[(int)Direction8.UpRight])
                                layer += 8;

                            if (!blockedDirs[(int)Direction8.DownRight])
                                layer += 2;

                            GroundLayers[0].Tiles[x, y] = new TileAnim(new Loc2D(), 0);
                        }
                        else if (!blockedDirs[(int)Direction8.Down] && blockedDirs[(int)Direction8.Left] && blockedDirs[(int)Direction8.Up] && blockedDirs[(int)Direction8.Right])
                        {
                            int layer = 6;
                            if (blockedDirs[(int)Direction8.UpRight])
                                layer += 1 * 8;

                            if (blockedDirs[(int)Direction8.UpLeft])
                                layer += 2 * 8;

                            GroundLayers[0].Tiles[x, y] = new TileAnim(new Loc2D(), 0);
                        }
                        else if (blockedDirs[(int)Direction8.Down] && !blockedDirs[(int)Direction8.Left] && blockedDirs[(int)Direction8.Up] && blockedDirs[(int)Direction8.Right])
                        {
                            int layer = 7;
                            if (blockedDirs[(int)Direction8.DownRight])
                                layer += 1 * 8;

                            if (blockedDirs[(int)Direction8.UpRight])
                                layer += 2 * 8;

                            GroundLayers[0].Tiles[x, y] = new TileAnim(new Loc2D(), 0);
                        }
                        else if (blockedDirs[(int)Direction8.Down] && blockedDirs[(int)Direction8.Left] && !blockedDirs[(int)Direction8.Up] && blockedDirs[(int)Direction8.Right])
                        {
                            int layer = 4;
                            if (blockedDirs[(int)Direction8.DownLeft])
                                layer += 1 * 8;

                            if (blockedDirs[(int)Direction8.DownRight])
                                layer += 2 * 8;

                            GroundLayers[0].Tiles[x, y] = new TileAnim(new Loc2D(), 0);
                        }
                        else if (blockedDirs[(int)Direction8.Down] && blockedDirs[(int)Direction8.Left] && blockedDirs[(int)Direction8.Up] && !blockedDirs[(int)Direction8.Right])
                        {
                            int layer = 5;
                            if (blockedDirs[(int)Direction8.UpLeft])
                                layer += 1 * 8;

                            if (blockedDirs[(int)Direction8.DownLeft])
                                layer += 2 * 8;

                            GroundLayers[0].Tiles[x, y] = new TileAnim(new Loc2D(), 0);
                        }
                        else if (!blockedDirs[(int)Direction8.Down] && !blockedDirs[(int)Direction8.Left] && blockedDirs[(int)Direction8.Up] && blockedDirs[(int)Direction8.Right])
                        {
                            int layer = 34;
                            if (blockedDirs[(int)Direction8.UpRight])
                                layer += 8;

                            GroundLayers[0].Tiles[x, y] = new TileAnim(new Loc2D(), 0);
                        }
                        else if (blockedDirs[(int)Direction8.Down] && !blockedDirs[(int)Direction8.Left] && !blockedDirs[(int)Direction8.Up] && blockedDirs[(int)Direction8.Right])
                        {
                            int layer = 35;
                            if (blockedDirs[(int)Direction8.DownRight])
                                layer += 8;

                            GroundLayers[0].Tiles[x, y] = new TileAnim(new Loc2D(), 0);
                        }
                        else if (blockedDirs[(int)Direction8.Down] && blockedDirs[(int)Direction8.Left] && !blockedDirs[(int)Direction8.Up] && !blockedDirs[(int)Direction8.Right])
                        {
                            int layer = 32;
                            if (blockedDirs[(int)Direction8.DownLeft])
                                layer += 8;

                            GroundLayers[0].Tiles[x, y] = new TileAnim(new Loc2D(), 0);
                        }
                        else if (!blockedDirs[(int)Direction8.Down] && blockedDirs[(int)Direction8.Left] && blockedDirs[(int)Direction8.Up] && !blockedDirs[(int)Direction8.Right])
                        {
                            int layer = 33;
                            if (blockedDirs[(int)Direction8.UpLeft])
                                layer += 8;

                            GroundLayers[0].Tiles[x, y] = new TileAnim(new Loc2D(), 0);
                        }
                        else if (blockedDirs[(int)Direction8.Down] && !blockedDirs[(int)Direction8.Left] && !blockedDirs[(int)Direction8.Up] && !blockedDirs[(int)Direction8.Right])
                            GroundLayers[0].Tiles[x, y] = new TileAnim(new Loc2D(36, 0), 0);
                        else if (!blockedDirs[(int)Direction8.Down] && blockedDirs[(int)Direction8.Left] && !blockedDirs[(int)Direction8.Up] && !blockedDirs[(int)Direction8.Right])
                            GroundLayers[0].Tiles[x, y] = new TileAnim(new Loc2D(37, 0), 0);
                        else if (!blockedDirs[(int)Direction8.Down] && !blockedDirs[(int)Direction8.Left] && blockedDirs[(int)Direction8.Up] && !blockedDirs[(int)Direction8.Right])
                            GroundLayers[0].Tiles[x, y] = new TileAnim(new Loc2D(38, 0), 0);
                        else if (!blockedDirs[(int)Direction8.Down] && !blockedDirs[(int)Direction8.Left] && !blockedDirs[(int)Direction8.Up] && blockedDirs[(int)Direction8.Right])
                            GroundLayers[0].Tiles[x, y] = new TileAnim(new Loc2D(39, 0), 0);
                        else if (!blockedDirs[(int)Direction8.Down] && blockedDirs[(int)Direction8.Left] && !blockedDirs[(int)Direction8.Up] && blockedDirs[(int)Direction8.Right])
                            GroundLayers[0].Tiles[x, y] = new TileAnim(new Loc2D(44, 0), 0);
                        else if (blockedDirs[(int)Direction8.Down] && !blockedDirs[(int)Direction8.Left] && blockedDirs[(int)Direction8.Up] && !blockedDirs[(int)Direction8.Right])
                            GroundLayers[0].Tiles[x, y] = new TileAnim(new Loc2D(45, 0), 0);
                        else if (!blockedDirs[(int)Direction8.Down] && !blockedDirs[(int)Direction8.Left] && !blockedDirs[(int)Direction8.Up] && !blockedDirs[(int)Direction8.Right])
                            GroundLayers[0].Tiles[x, y] = new TileAnim(new Loc2D(46, 0), 0);

                    }
                    else
                    {
                        MapArray[x, y] = new Tile(PMDToolkit.Enums.TileType.Walkable, 0, 0, 0);
                        GroundLayers[0].Tiles[x, y] = new TileAnim(new Loc2D(47, 0), 0);
                    }
                }
            }
        }

        private void AddBlobs(int circles, int hollowChance, int minSize, int maxSize, int minVar, int maxVar, int minWobble, int maxWobble)
        {
            for (int i = 0; i < circles; i++)
            {
                BranchLine line = ChooseRandomMainBranch();

                AddBlob(line, hollowChance, minSize, maxSize, minVar, maxVar, minWobble, maxWobble);
            }
        }

        private void AddBlob(BranchLine line, int hollowChance, int minSize, int maxSize, int minVar, int maxVar, int minWobble, int maxWobble)
        {
            bool hollow = rand.Next() % 100 < hollowChance ? true : false;
            int size = rand.Next(minSize, maxSize + 1);
            int diff = rand.Next(minVar, maxVar + 1);
            int wobble = rand.Next(minWobble, maxWobble + 1);
            float progress = (float)rand.NextDouble();
            Loc2D center = new Loc2D((int)(line.End.X * progress + line.Start.X * (1 - progress)), (int)(line.End.Y * progress + line.Start.Y * (1 - progress)));

            //TODO: check for forbiddance

            //TODO: draw blob
            DrawBlob(center, size, diff, wobble, hollow);
        }

        private BranchLine ChooseRandomMainBranch()
        {
            BranchLine chosenBranch = null;
            if (startLine.Children.Count > 0)
            {
                chosenBranch = startLine.Children[0];
                BranchLine currentBranch = chosenBranch;
                int total = 1;
                while (currentBranch.Children.Count > 0)
                {
                    if (rand.Next(0, total + 1) == 0)
                        chosenBranch = currentBranch.Children[0];
                    currentBranch = currentBranch.Children[0];
                    total++;
                }
            }
            return chosenBranch;
        }

        private bool CreatePath(BranchLine parent, int depth)
        {
            if (depth >= maxDepth)
                return false;

            BranchLine branch = null;
            for (int i = 0; i < 50; i++)
            {
                //choose the end of this branch to branch from previous line

                //choose a random length
                int length = rand.Next(minLineSize, maxLineSize);

                double parentAngle = rand.NextDouble() * 2 * Math.PI;
                if (parent.Start != parent.End)
                    parentAngle = Math.Atan2(parent.End.Y - parent.Start.Y, parent.End.X - parent.Start.X);

                //choose a random direction
                double angle = (rand.NextDouble() * 2 - 1) * mainFreedom * Math.PI + parentAngle;

                //create branch with end point
                PointF end = new PointF((float)Math.Cos(angle) * length + parent.End.X, (float)Math.Sin(angle) * length + parent.End.Y);
                BranchLine tempBranch = new BranchLine(parent.End, end);

                if (!IsLineForbidden(tempBranch))
                {
                    branch = tempBranch;
                    break;
                }
            }

            if (branch != null)
            {
                //TODO: draw branch with forbidden values
                Loc2D start = new Loc2D((int)Math.Round(branch.Start.X), (int)Math.Round(branch.Start.Y));
                Loc2D end = new Loc2D((int)Math.Round(branch.End.X), (int)Math.Round(branch.End.Y));
                DrawStraightPassage(start, end);

                //for now, just add the branch
                parent.Children.Add(branch);

                //call recusrively
                CreatePath(branch, depth + 1);
            }
            return true;
        }

        private void AddBranches(int total)
        {
            for (int i = 0; i < total; i++)
            {
                BranchLine line = ChooseRandomMainBranch();
                bool success = AddChild(line);
            }
        }

        private bool AddChild(BranchLine line)
        {
            //choose a random place to branch from previous line, as a dead end
            BranchLine branch = null;
            for (int i = 0; i < 50; i++)
            {
                //choose the end of this branch to branch from previous line

                //choose a random length
                int length = rand.Next(minLineSize, maxLineSize);

                double parentAngle = rand.NextDouble() * 2 * Math.PI;
                if (line.Start != line.End)
                    parentAngle = Math.Atan2(line.End.Y - line.Start.Y, line.End.X - line.Start.X);

                //choose a random direction
                double angle = (rand.NextDouble() * 2 - 1) * mainFreedom * Math.PI + parentAngle;

                //choose a part of the line
                float phase = (float)rand.NextDouble();

                //create branch with end point
                PointF start = new PointF(line.Start.X * (1 - phase) + line.End.X * phase, line.Start.Y * (1 - phase) + line.End.Y * phase);
                PointF end = new PointF((float)Math.Cos(angle) * length + start.X, (float)Math.Sin(angle) * length + start.Y);
                BranchLine tempBranch = new BranchLine(start, end);

                if (!IsLineForbidden(tempBranch))
                {
                    branch = tempBranch;
                    break;
                }
            }

            if (branch != null)
            {
                //TODO: draw branch with forbidden values
                Loc2D start = new Loc2D((int)Math.Round(branch.Start.X), (int)Math.Round(branch.Start.Y));
                Loc2D end = new Loc2D((int)Math.Round(branch.End.X), (int)Math.Round(branch.End.Y));
                DrawStraightPassage(start, end);

                //for now, just add the branch
                line.Children.Add(branch);
            }

            return false;
        }

        //TODO
        private bool IsLineForbidden(BranchLine line)
        {
            //"draw" pixels of the line, checking to see if any part of the line's crossing is "forbidden"
            
            return false;
        }

        private bool IsCurveForbidden()
        {

            return false;
        }

        private bool IsTileForbidden(Loc2D loc, int min, int max)
        {
            //if out of range, return false
            Loc2D destLoc = loc - TopLeft;
            if (AgeArray[destLoc.X, destLoc.Y] < min || AgeArray[destLoc.X, destLoc.Y] > max)
                return true;

            //if tile value is out of range of min and max, return true
            return false;
        }
        
        private void ExpandArray(Direction8 dir)
        {
            int expansion = 10;

            if (dir == Direction8.None)
                return;

            //expand by an arbitrary amount: 10 tiles
            Loc2D size = new Loc2D(AgeArray.GetLength(0), AgeArray.GetLength(1));

            if (Operations.GetOrientation8(dir) == Orientation8.Horiz)
                size.X += expansion;
            else if (Operations.GetOrientation8(dir) == Orientation8.Vert)
                size.Y += expansion;
            else
            {
                size.X += expansion;
                size.Y += expansion;
            }

            Loc2D diff = Operations.GetResizeOffset(AgeArray.GetLength(0), AgeArray.GetLength(1), size.X, size.Y, dir);
            Operations.ResizeArray<int>(ref AgeArray, size.X, size.Y, dir);

            TopLeft -= diff;
        }

        public void DrawPath(Loc2D loc, int size)
        {
            double radius = (double)size / 2;
            double angle = Math.PI / 4;
            double centerX = (loc.X + Math.Cos(angle) * radius);
            double centerY = (loc.Y + Math.Sin(angle) * radius);
            Loc2D centerInt = new Loc2D((int)centerX, (int)centerY);
            int radiusInt = (int)radius + 1;
            //int draws = 0;
            //double minDistance = 0;
            for (int x = centerInt.X - radiusInt - 1; x <= centerInt.X + radiusInt + 1; x++)
            {
                for (int y = centerInt.Y - radiusInt; y <= centerInt.Y + radiusInt + 1; y++)
                {
                    //calculate distance and angle
                    double disSquared = Math.Pow(x - centerX, 2) + Math.Pow(y - centerY, 2);
                    //if (disSquared - radius * radius < minDistance)
                    //    minDistance = disSquared - radius * radius;

                    if (disSquared <= radius * radius)
                    {
                        //draw
                        DrawTile(new Loc2D(x, y));
                        //draws++;
                    }
                }
            }
            DrawTile(loc);
        }

        public void DrawTile(Loc2D loc)
        {
            //invisibly resize the AgeArray if asked to draw beyond what it's asked
            Loc2D destLoc = loc - TopLeft;
            Direction8 outDir = Operations.GetOuterBound8(1, 1, AgeArray.GetLength(0) - 2, AgeArray.GetLength(1) - 2, destLoc.X, destLoc.Y);
            while (outDir != Direction8.None)
            {
                ExpandArray(outDir);

                destLoc = loc - TopLeft;
                outDir = Operations.GetOuterBound8(0, 0, AgeArray.GetLength(0), AgeArray.GetLength(1), destLoc.X, destLoc.Y);
            }


            AgeArray[destLoc.X, destLoc.Y] = -1;
        }

        public void DrawStraightPassage(Loc2D loc1, Loc2D loc2)
        {
            Loc2D diff = loc2 - loc1;
            Direction4 dir = (Math.Abs(diff.Y) > Math.Abs(diff.X)) ? Direction4.Down : Direction4.Right;
            Direction4 subDir = (dir == Direction4.Down) ? Direction4.Right : Direction4.Down;
            if (Operations.DimOfDir(diff.X, diff.Y, dir) < 0)
                dir = Operations.ReverseDir(dir);
            if (Operations.DimOfDir(diff.X, diff.Y, subDir) < 0)
                subDir = Operations.ReverseDir(subDir);
            Loc2D increment = new Loc2D();
            Loc2D subIncrement = new Loc2D();
            Operations.MoveInDirection4(ref increment, dir, 1);
            Operations.MoveInDirection4(ref subIncrement, subDir, 1);

            int change = Math.Abs(Operations.DimOfDir(diff.X, diff.Y, dir));
            int subChange = Math.Abs(Operations.DimOfDir(diff.X, diff.Y, subDir));

            int err = change / 2;


            Loc2D curLoc = loc1;
            int tileSize = rand.Next(minPassageSize, maxPassageSize + 1);
            int tileLimit = rand.Next(minSizeChangePeriod, maxSizeChangePeriod + 1);
            int tilesMoved = 0;
            while (Operations.DimOfDir(curLoc.X, curLoc.Y, dir) != Operations.DimOfDir(loc2.X, loc2.Y, dir))
            {
                DrawPath(curLoc, tileSize);
                curLoc += increment;
                err -= subChange;
                if (err < 0)
                {
                    curLoc += subIncrement;
                    err += change;
                }
                tilesMoved++;
                if (tilesMoved >= tileLimit)
                {
                    tileSize = rand.Next(minPassageSize, maxPassageSize + 1);
                    tileLimit = rand.Next(minSizeChangePeriod, maxSizeChangePeriod + 1);
                    tilesMoved = 0;
                }
            }
            DrawPath(loc2, tileSize);
        }


        public void DrawStraightLine(Loc2D loc1, Loc2D loc2)
        {
            Loc2D diff = loc2 - loc1;
            Direction4 dir = (Math.Abs(diff.Y) > Math.Abs(diff.X)) ? Direction4.Down : Direction4.Right;
            Direction4 subDir = (dir == Direction4.Down) ? Direction4.Right : Direction4.Down;
            if (Operations.DimOfDir(diff.X, diff.Y, dir) < 0)
                dir = Operations.ReverseDir(dir);
            if (Operations.DimOfDir(diff.X, diff.Y, subDir) < 0)
                subDir = Operations.ReverseDir(subDir);
            Loc2D increment = new Loc2D();
            Loc2D subIncrement = new Loc2D();
            Operations.MoveInDirection4(ref increment, dir, 1);
            Operations.MoveInDirection4(ref subIncrement, subDir, 1);

            int change = Math.Abs(Operations.DimOfDir(diff.X, diff.Y, dir));
            int subChange = Math.Abs(Operations.DimOfDir(diff.X, diff.Y, subDir));

            int err = change / 2;


            Loc2D curLoc = loc1;

            while (Operations.DimOfDir(curLoc.X, curLoc.Y, dir) != Operations.DimOfDir(loc2.X, loc2.Y, dir))
            {
                DrawTile(curLoc);
                curLoc += increment;
                err -= subChange;
                if (err < 0)
                {
                    curLoc += subIncrement;
                    err += change;
                }
            }
            DrawTile(loc2);
        }

        public List<System.Drawing.PointF> CreateSpline(List<Loc2D> controlPoints)
        {
            List<System.Drawing.PointF> pointfs = new List<System.Drawing.PointF>();

            pointfs.Add(new System.Drawing.PointF(controlPoints[0].X, controlPoints[0].Y));
            for (int i = 1; i < controlPoints.Count - 1; i++)
            {
                //create the vector of the direction of the surrounding points
                Loc2D diff = controlPoints[i + 1] - controlPoints[i - 1];
                float length = (float)Math.Sqrt(diff.X * diff.X + diff.Y * diff.Y);
                System.Drawing.PointF unitDiff = new System.Drawing.PointF((float)diff.X / length, (float)diff.Y / length);

                //create vectors for each control point
                Loc2D prevDiff = controlPoints[i - 1] - controlPoints[i];
                Loc2D postDiff = controlPoints[i + 1] - controlPoints[i];

                float prevDiffDot = prevDiff.X * unitDiff.X + prevDiff.Y * unitDiff.Y;
                float postDiffDot = postDiff.X * unitDiff.X + postDiff.Y * unitDiff.Y;

                System.Drawing.PointF point = new System.Drawing.PointF(controlPoints[i].X, controlPoints[i].Y);

                System.Drawing.PointF prevPoint = new System.Drawing.PointF(point.X + unitDiff.X * prevDiffDot, point.Y + unitDiff.Y * prevDiffDot);
                System.Drawing.PointF postPoint = new System.Drawing.PointF(point.X + unitDiff.X * postDiffDot, point.Y + unitDiff.Y * postDiffDot);

                pointfs.Add(prevPoint);
                pointfs.Add(point);
                pointfs.Add(postPoint);
            }

            pointfs.Add(new System.Drawing.PointF(controlPoints[controlPoints.Count - 1].X, controlPoints[controlPoints.Count - 1].Y));

            pointfs.Insert(1, pointfs[1]);
            pointfs.Insert(pointfs.Count - 1, pointfs[pointfs.Count - 1]);

            return pointfs;
        }

        public void DrawCurvedLine(List<Loc2D> locs)
        {
            //int res = 20;
            //int tMax = (locs.Count - 1) * res;
            //List<System.Drawing.PointF> allPoints = CreateSpline(locs);
            ////every third node, starting from the first, is a control point
            ////1, 4, 7, 10, 13, etc.

            //for (int i = 0; i < tMax; i++)
            //{
            //    int segment = i / res;
            //    System.Drawing.PointF p0 = allPoints[segment];
            //    System.Drawing.PointF p1 = allPoints[segment + 1];
            //    System.Drawing.PointF p2 = allPoints[segment + 2];
            //    System.Drawing.PointF p3 = allPoints[segment + 3];
            //    float t0 = (float)i / res - segment;
            //    float t1 = 1 - t0;

            //    float pX = t1 * (t1 * (t1 * p0.X + t0 * p1.X) + t0 * (t1 * p1.X + t0 * p2.X)) + t0 * (t1 * (t1 * p1.X + t0 * p2.X) + t0 * (t1 * p2.X + t0 * p3.X));
            //    float pY = t1 * (t1 * (t1 * p0.Y + t0 * p1.Y) + t0 * (t1 * p1.Y + t0 * p2.Y)) + t0 * (t1 * (t1 * p1.Y + t0 * p2.Y) + t0 * (t1 * p2.Y + t0 * p3.Y));

            //    System.Drawing.PointF point = new System.Drawing.PointF(pX, pY);
            //}
        }


        public void DrawBlob(Loc2D center, int avgSize, int diffSize, int wobble, bool hollow)
        {
            //wobble of 1 = egg
            //wobble of 2 = oval
            double offset = rand.NextDouble() * Math.PI * 2;
            int maxSize = avgSize + diffSize;

            for (int x = -maxSize; x <= maxSize; x++)
            {
                for (int y = -maxSize; y <= maxSize; y++)
                {
                    //calculate distance and angle
                    int disSquared = x * x + y * y;
                    double angle = Math.Atan2(y, x);
                    double r = Math.Sin(angle * wobble + offset) * diffSize + avgSize;
                    if (disSquared < r*r)
                    {
                        //draw
                        DrawTile(new Loc2D(x,y) + center);
                    }
                }
            }
        }


        bool IsBlocked(int x, int y, Direction8 dir)
        {

            Operations.MoveInDirection8(ref x, ref y, dir, 1);

            if (x < 0 || x > Width - 1 || y < 0 || y > Height - 1)
                return true;

            if (AgeArray[x, y] != -1)
                return true;
            
            return false;
        }


        public void GenItems()
        {

            for (int n = 0; n < 10; n++)
            {
                GenItem();
            }
        }

        public void GenItem()
        {

            //List<Loc2D> possiblePoints = new List<Loc2D>();
            //for (int i = 0; i < Width; i++) {
            //    for (int j = 0; j < Height; j++) {
            //        if (MapArray[i, j].Data.Type != PMDToolkit.Enums.TileType.Blocked && MapArray[i, j].Data.Type != PMDToolkit.Enums.TileType.Hall && MapArray[i, j].Item.ItemIndex == -1 && MapArray[i, j].TileFoliage == null) {
            //            possiblePoints.Add(new Loc2D(i, j));
            //        }
            //    }
            //}
            //if (possiblePoints.Count > 0) {
            //    Loc2D loc = possiblePoints[rand.Next(possiblePoints.Count)];
            //    MapArray[loc.X, loc.Y].Item = new Item(rand.Next(5), 1);
            //}

        }


        public void SpawnNpcs()
        {
            //spawn NPCs

        }


        public bool SpawnNpc(DungeonArrayRoom room)
        {

            List<Loc2D> possiblePoints = new List<Loc2D>();
            for (int y = room.StartY; y <= room.EndY; y++)
            {
                for (int x = room.StartX; x <= room.EndX; x++)
                {
                    if (MapArray[x, y].Data.Type == PMDToolkit.Enums.TileType.Walkable && StartPoint != new Loc2D(x, y))
                    {
                        bool placeHere = true;
                        for (int n = 0; n < BasicMap.MAX_NPC_SLOTS; n++)
                        {
                            if (!Npcs[n].dead && Npcs[n].CharLoc == new Loc2D(x, y))
                            {
                                placeHere = false;
                                break;
                            }
                        }
                        if (placeHere)
                        {
                            possiblePoints.Add(new Loc2D(x, y));
                        }
                    }
                }
            }

            if (possiblePoints.Count > 0)
            {
                SpawnNpc(possiblePoints[rand.Next(possiblePoints.Count)]);
                return true;
            }
            return false;
        }

        public void SpawnNpc(Loc2D loc)
        {
            for (int i = 0; i < BasicMap.MAX_NPC_SLOTS; i++)
            {
                if (Npcs[i].dead)
                {
                    int npcIndex = rand.Next(1, 3);
                    Npcs[i] = new Npc(loc, (Direction8)rand.Next(8), npcIndex);
                    break;
                }
            }
        }
    }
}
