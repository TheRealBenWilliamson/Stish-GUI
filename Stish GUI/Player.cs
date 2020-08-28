﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stish_GUI
{
    public abstract class Player
    {

        //private static Player Player1Instance;
        //private static Player Player2Instance;
        //dont know where to use these

        //by now a StishBoard.Instance should already have been created. StishBoard.Instance allows us to get a reference to the existing StishBoard.Instance.

        //using enums because it is an easy way to show exclusive possible properties of something. eg an enum for the day of the week would be appropriate because there are seven set possibilities yet the day could only be one of them at once.
        //why didnt we use enums for deployemnt?
        public enum PlayerNumber { Player1, Player2};
        public enum PlayerType { Human, Computer};

        protected PlayerNumber playerNumber;
        protected PlayerType playerType;
        protected uint balance;

        public uint CursorX = 1;
        public uint CursorY = 1;
        private uint baseX = 0;
        private uint baseY = 0;        

        public uint BaseX
        {
            get
            {
                return baseX;
            }
            set
            {
                baseX = value;
            }
        }

        public uint BaseY
        {
            get
            {
                return baseY;
            }
            set
            {
                baseY = value;
            }
        }

        protected Base homeBase;

        protected Player(PlayerNumber PN, PlayerType PT, BoardState Board)
        {
            playerNumber = PN;
            playerType = PT;
            //balance can be changed for testing and balancing
            balance = 4;

            //homeBase = new Base();
            if (playerNumber == PlayerNumber.Player1)
            {
                BaseX = (Board.BoardSizeX) / 2;
                BaseY = Board.BoardSizeY - 2;                                      
            }
            else
            {
                BaseX = (Board.BoardSizeX) / 2;
                BaseY = 1;                          
            }
            Coordinate ThisCo = new Coordinate(BaseX, BaseY);
            new Base(this, Board.getSquare(ThisCo), 20);
            for (uint y = BaseY - 1; y < BaseY + 2; y++)
            {
                for (uint x = BaseX - 1; x < BaseX + 2; x++)
                {
                    ThisCo.X = x;
                    ThisCo.Y = y;
                    Board.getSquare(ThisCo).Owner = this;
                }
            }
        }      
        
        public Player(Player CopyFrom)
        {
            playerNumber = CopyFrom.playerNumber;
            playerType = CopyFrom.playerType;
            balance = CopyFrom.balance;
            CursorX = CopyFrom.CursorX;
            CursorY = CopyFrom.CursorY;
            baseX = CopyFrom.baseX;
            baseY = CopyFrom.baseY;
        }

        public string GetPlayerNum
        {
            get
            {
                return playerNumber.ToString();
            }
        }

        public string GetPlayerType
        {
            get
            {
                return playerType.ToString();
            }
        }


        public ConsoleColor GetRenderColour()
        {
            ConsoleColor retval;

            switch (playerNumber)
            {
                case PlayerNumber.Player1:
                    retval = ConsoleColor.DarkRed;
                    break;
                case PlayerNumber.Player2:
                    retval = ConsoleColor.Blue;
                    break;
                default:
                    retval = ConsoleColor.White;
                    break;
            }

            return retval;
        }

        //a function that can be called upon to create a player of a given type. the player is aslo assigned a number to represent them so we can distinguish between to players of the same type.
        public static Player PlayerFactory(PlayerNumber PN, PlayerType PT, BoardState Board)
        {
            Player creation = null;

            //creates either a human or computer object and tells it the which player number it has.
            if (PT == PlayerType.Human)
            {
                creation = new Human(PN, PT, Board);
            }
            if (PT == PlayerType.Computer)
            {
                creation = new Computer(PN, PT, Board);
            }

            return creation;
        }

        public uint Balance
        {
            get
            {
                return balance;
            }
            set
            {
                balance = value;
            }
        }        

        public virtual void MakeMove()
        {
            
        }

        public void TurnBalance(BoardState Board)
        {
            Coordinate ThisCo = new Coordinate();
            for (uint y = 0; y < Board.BoardSizeY; y++)
            {
                for (uint x = 0; x < Board.BoardSizeX; x++)
                {
                    ThisCo.X = x;
                    ThisCo.Y = y;
                    if((Board.getSquare(ThisCo).Dep.DepType == "Barracks" || Board.getSquare(ThisCo).Dep.DepType == "Base") && Board.getSquare(ThisCo).Owner == this)
                    {
                        this.Balance ++ ;
                    }           
                }
            }
        }        


        public void MaxMP(BoardState Board)
        {
            //this fuction is run at the start of a turn and sets all units that belong to this player to the max MP.
            Coordinate ThisCo = new Coordinate();
            for (uint y = 0; y < Board.BoardSizeY; y++)
            {
                for (uint x = 0; x < Board.BoardSizeX; x++)
                {
                    ThisCo.X = x;
                    ThisCo.Y = y;
                    Square ThisSquare = Board.getSquare(ThisCo);
                    if ((ThisSquare.Owner == this) && (ThisSquare.Dep.DepType == "Unit"))
                    {
                        //This number is subject to change throughout testing and balancing
                        ThisSquare.Dep.MP = StishBoard.Instance.GameMP;
                        ThisSquare.Dep.JustCreated = false;
                    }
                }
            }
        }


    }
}
