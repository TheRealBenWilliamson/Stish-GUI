﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stish_GUI
{
    class GameMaster
    {
        //the gamemaster controls the rules of stish and is what the players can call to make stuff happen
        //singleton class-object
        private static GameMaster instance;       

        public static GameMaster Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new GameMaster();
                }
                return instance;
            }
        }                     

        private GameMaster()
        {

        }

        //coordinates are uints so anything less than 0 will overflow
        public bool OnBoard(Coordinate num, BoardState board)
        {
            try
            {
                Square Check = board.getSquare(num);
                if (Check != null)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            }
            catch
            {
                return false;
            }
        }          

        public bool BuyBarracks(Coordinate Pur, Player ConPlayer, BoardState board)
        {
            bool bought = false;
            if ((board.getSquare(Pur) != null) && (board.getSquare(Pur).Dep.DepType == "Empty") && (board.getSquare(Pur).Owner == ConPlayer))
            {
                //check how many barracks the player already has and multiply it buy the cost of one barracks
                uint multiply = 0;
                Coordinate See = new Coordinate();

                for (uint y = 0; y < board.BoardSizeY; y++)
                {
                    for (uint x = 0; x < board.BoardSizeX; x++)
                    {
                        See.X = x;
                        See.Y = y;
                        if ((board.getSquare(See).Dep.DepType == "Barracks" || board.getSquare(See).Dep.DepType == "Base") && board.getSquare(See).Dep.OwnedBy == ConPlayer)
                        {
                            multiply++;
                        }
                    }
                }

                if (ConPlayer.Balance >= 3 * multiply)
                {
                    board.getSquare(Pur).Dep = new Barracks(board.getSquare(Pur).Owner, board.getSquare(Pur), 5);
                    ConPlayer.Balance -= 3 * multiply;
                    bought = true;
                }
            }
            return bought;
        }

        public bool BuyUnit(Coordinate Pur, Player ConPlayer, BoardState board)
        {
            bool bought = false;
            if ((board.getSquare(Pur) != null) && (board.getSquare(Pur).Dep.DepType == "Empty") && (board.getSquare(Pur).Owner == ConPlayer))                
            {
                //spend the entire player balance 
                if (ConPlayer.Balance > 0)
                {
                    board.getSquare(Pur).Dep = new Unit(board.getSquare(Pur).Owner, board.getSquare(Pur), ConPlayer.Balance);
                    ConPlayer.Balance = 0;
                    bought = true;
                }                 
            }
            return bought;
        }

        private void Drag(Coordinate FromCo, Coordinate ToCo, Player MyPlayer, BoardState board)
        {
            Square From = board.getSquare(FromCo);
            Square To = board.getSquare(ToCo);

            From.Owner = MyPlayer;
            To.Owner = MyPlayer;

            To.Dep = From.Dep;
            From.Dep = new Empty();
        }

        public bool Attack(Coordinate From, Coordinate Check, Player MyPlayer, BoardState board)
        {
            //returns true if this unit dies
            bool Died = false;

            //attack
            //adjust health and then if the attacking unit won, use the drag function
            //i dont know if i want to use the drag function on an attack. i will wait until i test it to decide
            //i dont have to redefine these squares but i think it helps the code read better
            Square Attacker = board.getSquare(From);
            Square Defender = board.getSquare(Check);
            String CheckDep = board.getSquare(Check).Dep.DepType;
            Player Owner = board.getSquare(Check).Dep.OwnedBy;

            //attacker must be more than 1 turn old in order to attack
            //attacker must be able to spend a mp to attack
            if (Attacker.Dep.MP > 0)
            {
                Attacker.Dep.MP--;
                //checks who wins the combat: attacker or defender
                //attacker wins
                if (Attacker.Dep.Health > Defender.Dep.Health)
                {
                    Attacker.Dep.Health -= Defender.Dep.Health;
                    //Barracks are a special case
                    if (CheckDep == "Barracks")
                    {
                        board.getSquare(Check).Dep = new Barracks(MyPlayer, board.getSquare(Check), 5);
                        board.getSquare(Check).Owner = MyPlayer;
                    }
                    else
                    {
                        Defender.Dep = new Empty();
                    }

                }
                //defender wins
                else if (Attacker.Dep.Health < Defender.Dep.Health)
                {
                    Defender.Dep.Health -= Attacker.Dep.Health;
                    Attacker.Dep = new Empty();
                    //Cursor.Instance.CursorMode = Cursor.Mode.free;
                    Died = true;
                }
                //both have equal health
                else
                {
                    if (CheckDep == "Barracks")
                    {
                        board.getSquare(Check).Dep = new Barracks(MyPlayer, board.getSquare(Check), 5);
                        board.getSquare(Check).Owner = MyPlayer;
                    }
                    else
                    {
                        Defender.Dep = new Empty();
                    }
                    Attacker.Dep = new Empty();
                    //Cursor.Instance.CursorMode = Cursor.Mode.free;
                    Died = true;
                }
            }
            return Died;
        }

        public bool Action(Coordinate From, Coordinate Check, Player MyPlayer, BoardState board)
        {
            //the bool output lets the caller know if the unit moved
            bool Moved = false;
            String CheckDep = board.getSquare(Check).Dep.DepType;
            Player Owner = board.getSquare(Check).Dep.OwnedBy;
            if (((CheckDep == "Empty") || (CheckDep == "Barracks") && Owner == MyPlayer))
            {
                //drag or destroys a friendly barracks
                if (board.getSquare(From).Dep.MP > 0)
                {
                    board.getSquare(From).Dep.MP--;
                    Drag(new Coordinate(From.X, From.Y), new Coordinate(Check.X, Check.Y), MyPlayer, board);
                    Moved = true;
                }

            }
            else if ((CheckDep == "Unit" || CheckDep == "Barracks" || CheckDep == "Base") && (Owner != MyPlayer))
            {
                Attack(From, Check, MyPlayer, board);
            }

            //TerritoryDeaths();
            //BaseSquares();
            return Moved;
        }


    }
}
