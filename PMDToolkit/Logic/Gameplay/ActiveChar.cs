using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PMDToolkit.Maps;

namespace PMDToolkit.Logic.Gameplay {

    public class ActiveChar : Character {

        public const int MAX_SPEED = 6;
        public const int MIN_SPEED = 0;
        public const int MAX_BUFF = 6;
        public const int MIN_BUFF = -6;

        public Enums.Element Type1 { get; set; }
        public Enums.Element Type2 { get; set; }
        
        public string Ability1 { get; set; }
        public string Ability2 { get; set; }
        public string Ability3 { get; set; }

        public string Name { get; set; }

        public FormData CharData;

        public MoveState[] Moves { get; set; }

        //final stats (without buffing)
        public int MaxHP { get; set; }
        public int Atk { get; set; }
        public int Def { get; set; }
        public int SpAtk { get; set; }
        public int SpDef { get; set; }
        public int Speed { get; set; }

        //stat buffs
        int attackBuff;
        public int AttackBuff
        {
            get { return attackBuff; }
            set { attackBuff = Math.Min(Math.Max(value, MIN_SPEED), MAX_SPEED); }
        }
        int defenseBuff;
        public int DefenseBuff
        {
            get { return defenseBuff; }
            set { defenseBuff = Math.Min(Math.Max(value, MIN_SPEED), MAX_SPEED); }
        }
        int spAtkBuff;
        public int SpAtkBuff
        {
            get { return spAtkBuff; }
            set { spAtkBuff = Math.Min(Math.Max(value, MIN_SPEED), MAX_SPEED); }
        }
        int spDefBuff;
        public int SpDefBuff
        {
            get { return spDefBuff; }
            set { spDefBuff = Math.Min(Math.Max(value, MIN_SPEED), MAX_SPEED); }
        }
        int speedBuff;
        public int SpeedBuff
        {
            get { return speedBuff; }
            set { speedBuff = Math.Min(Math.Max(value, MIN_SPEED), MAX_SPEED); }
        }
        int accuracyBuff;
        public int AccuracyBuff
        {
            get { return accuracyBuff; }
            set { accuracyBuff = Math.Min(Math.Max(value, MIN_SPEED), MAX_SPEED); }
        }
        int evasionBuff;
        public int EvasionBuff
        {
            get { return evasionBuff; }
            set { evasionBuff = Math.Min(Math.Max(value, MIN_SPEED), MAX_SPEED); }
        }

        int movementSpeed;
        public int MovementSpeed {
            get { return movementSpeed; }
            set { movementSpeed = Math.Min(Math.Max(value, MIN_SPEED), MAX_SPEED); }
        }

        //determining position
        public Loc2D CharLoc;
        //determining direction
        public Direction8 CharDir { get; set; }

        public int HP { get; set; }

        public int PP { get; set; }
        public int MaxPP { get; set; }

        public bool dead;

        public Enums.StatusAilment Status { get; set; }
        public int StatusCounter { get; set; }
        public Dictionary<string, ExtraStatus> VolatileStatus { get; set; }

        public int TurnCounter { get; set; }

        public AI Tactic { get; set; }

        public ActiveChar() {
            dead = true;

            VolatileStatus = new Dictionary<string, ExtraStatus>();
        }
        
        public ActiveChar(Loc2D newLoc, Direction8 charDir) {
            //clean variables
            CharLoc = newLoc;
            CharDir = charDir;
            HP = MaxHP;
            Status = Enums.StatusAilment.OK;
            StatusCounter = 0;
            Moves = new MoveState[Processor.MAX_MOVE_SLOTS];
            for (int i = 0; i < Processor.MAX_MOVE_SLOTS; i++)
            {
                Moves[i] = new MoveState();
            }

            VolatileStatus = new Dictionary<string, ExtraStatus>();
        }

        public void Initialize() {
            dead = false;
            //clean variables
            HP = MaxHP;
            Status = Enums.StatusAilment.OK;
            StatusCounter = 0;
            Moves = new MoveState[Processor.MAX_MOVE_SLOTS];
            for (int i = 0; i < Processor.MAX_MOVE_SLOTS; i++)
            {
                Moves[i] = new MoveState();
            }

            VolatileStatus = new Dictionary<string, ExtraStatus>();
        }


        public bool HasAbility(string ability) {
            return (ability == Ability1 || ability == Ability2 || ability == Ability3);
        }

    }
}
