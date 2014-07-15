
using System;
using System.Collections.Generic;
using System.Text;

namespace PMDToolkit.Maps {
    public class GeneratorOptions
    {
        #region Constructors

        public GeneratorOptions()
        {
            TrapMin = 5;
            TrapMax = 30;
            
            ItemMin = 5;
            ItemMax = 30;

            Intricacy = 6;

            RoomWidthMin = 3;
            RoomWidthMax = 10;
            RoomLengthMin = 3;
            RoomLengthMax = 10;

            CellX = 6;
            CellY = 4;

            CellWidth = 12;
            CellHeight = 12;

            HallTurnMin = 0;
            HallTurnMax = 3;
			
	    	HallVarMin = 0;
	        HallVarMax = 3;

            WaterFrequency = 0;

            Craters = 0;
            CraterMinLength = 3;
            CraterMaxLength = 13;
            CraterFuzzy = true;

            MinChambers = 0;
            MaxChambers = 0;
        }

        #endregion Constructors

        #region Properties

        

        /// <summary>
        /// Gets or Sets the maximum number of traps that will be generated. Adjust to number between 0 and 255 to see changes.
        /// </summary>
        public int TrapMax
        {
            get; set;
        }

        /// <summary>
        /// Gets or Sets the minimum number of traps that will be generated. Adjust to number between 0 and 255 to see changes.
        /// </summary>
        public int TrapMin
        {
            get; set;
        }
        
        /// <summary>
        /// Gets or Sets the maximum number of items that will be generated. Adjust to number between 0 and 255 to see changes.
        /// </summary>
        public int ItemMax
        {
            get; set;
        }

        /// <summary>
        /// Gets or Sets the minimum number of items that will be generated. Adjust to number between 0 and 255 to see changes.
        /// </summary>
        public int ItemMin
        {
            get; set;
        }

        /// <summary>
        /// Gets or Sets the "magic number" that is responsible for deciding how many rooms (or split hallways) the dungeon has.
        /// </summary>
        public int Intricacy
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or Sets the maximum room width that will be generated. Adjust to number between 0 and 48 to see changes.
        /// </summary>
        public int RoomWidthMax
        {
            get; set;
        }

        /// <summary>
        /// Gets or Sets the minimum room width that will be generated. Adjust to number between 0 and 48 to see changes.
        /// </summary>
        public int RoomWidthMin
        {
            get; set;
        }

        /// <summary>
        /// Gets or Sets the maximum room length that will be generated. Adjust to number between 0 and 48 to see changes.
        /// </summary>
        public int RoomLengthMax
        {
            get; set;
        }

        /// <summary>
        /// Gets or Sets the minimum room length that will be generated. Adjust to number between 0 and 48 to see changes.
        /// </summary>
        public int RoomLengthMin
        {
            get; set;
        }

        public int CellX { get; set; }
        public int CellY { get; set; }

        public int CellWidth { get; set; }
        public int CellHeight { get; set; }

        /// <summary>
        /// Gets or Sets the min number of times there is a turn in a hallway. Adjust to number 0 or above to see changes.  May be overridden if the hall is too short.
        /// </summary>
        public int HallTurnMin
        {
            get; set;
        }
		
		/// <summary>
        /// Gets or Sets the max number of times there is a turn in a hallway. Adjust to number 0 or above to see changes.  May be overridden if the hall is too short.
        /// </summary>
        public int HallTurnMax
        {
            get; set;
        }
		
		/// <summary>
        /// Gets or Sets the min amount a turn swerves in a hallway. Adjust to number 0 or above to see changes.  May be overridden if the hall is too short.
        /// </summary>
        public int HallVarMin
        {
            get; set;
        }
		
		/// <summary>
        /// Gets or Sets the max amount a turn swerves in a hallway. Adjust to number 0 or above to see changes.  May be overridden if the hall is too short.
        /// </summary>
        public int HallVarMax
        {
            get; set;
        }

        public int WaterFrequency //How much water will be in the dungeon; based on /100 chance
        {
            get; set;
        }


        //After setting water, the algorithm will "blast" several craters of water into the dungeon map, to simulate how water appears in pools in PMD

        public int Craters //How many craters of water the dungeon will have
        {
            get;
            set;
        }

        public int CraterMinLength //the smallest a crater can get (diameter)
        {
            get;
            set;
        }

        public int CraterMaxLength //the largest a crater can get (diameter)
        {
            get;
            set;
        }

        public bool CraterFuzzy //determines whether the crater is diamond-shaped (false) or has a "spray-paint" aura around it (true)
        {
            get;
            set;
        }

        public int MinChambers //the fewest possible chambers
        {
            get;
            set;
        }

        public int MaxChambers //the most possible chambers
        {
            get;
            set;
        }

        #endregion Properties
    }
}