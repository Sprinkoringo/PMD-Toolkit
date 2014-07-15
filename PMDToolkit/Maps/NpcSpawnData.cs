using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PMDToolkit.Graphics;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using PMDToolkit.Logic.Gameplay;

namespace PMDToolkit.Maps {
    public class NpcSpawnData {


        public FormData Data;

        public int MinLevel;
        public int MaxLevel;

        public int AppearanceRate;

        public Loc2D Loc;

        public int[] Moves;

        public NpcSpawnData()
        {
            Data.Gender = Enums.Gender.Unknown;
            Data.Shiny = Enums.Coloration.Unknown;
            Moves = new int[Processor.MAX_MOVE_SLOTS];
            for (int i = 0; i < Processor.MAX_MOVE_SLOTS; i++)
            {
                Moves[i] = -1;
            }
        }

    }
}
