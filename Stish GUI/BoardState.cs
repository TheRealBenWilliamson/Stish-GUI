using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stish_GUI
{
    public class BoardState
    {
        protected Square[,] m_BoardState;
        //boardsize gives the very bottom right point.
        protected Coordinate boardSize = new Coordinate();
        protected Player player1;
        protected Player player2;

        public BoardState()
        {

        }

        //StishBoard
        public BoardState(StishBoard CurrentBoard)
        {
            this.boardSize.X = CurrentBoard.BoardSizeX;
            this.boardSize.Y = CurrentBoard.BoardSizeY;
            this.player1 = new Human(CurrentBoard.Player1);
            m_BoardState = new Square[CurrentBoard.BoardSizeX, CurrentBoard.BoardSizeY];
            Coordinate Here = new Coordinate();

            if (CurrentBoard.Player2.GetPlayerType == "Human")
            {
                this.player2 = new Human(CurrentBoard.Player2);
                for (uint y = 0; y < CurrentBoard.BoardSizeY; y++)
                {
                    for (uint x = 0; x < CurrentBoard.BoardSizeX; x++)
                    {
                        Here.Y = y;
                        Here.X = x;
                        m_BoardState[x, y] = new Square(CurrentBoard.getSquare(Here), (Human)this.player1, (Human)this.player2);
                    }

                }
            }
            else
            {
                this.player2 = new Computer((Computer)CurrentBoard.Player2);
                for (uint y = 0; y < CurrentBoard.BoardSizeY; y++)
                {
                    for (uint x = 0; x < CurrentBoard.BoardSizeX; x++)
                    {
                        Here.Y = y;
                        Here.X = x;
                        m_BoardState[x, y] = new Square(CurrentBoard.getSquare(Here), (Human)this.player1, (Computer)this.player2);
                    }

                }
            }        
            
            
            
        }

        //BoardState
        public BoardState(BoardState CurrentBoard)
        {
            this.boardSize.X = CurrentBoard.BoardSizeX;
            this.boardSize.Y = CurrentBoard.BoardSizeY;
            this.player1 = new Human(CurrentBoard.Player1);
            m_BoardState = new Square[CurrentBoard.BoardSizeX, CurrentBoard.BoardSizeY];
            Coordinate Here = new Coordinate();
            if (CurrentBoard.Player2.GetPlayerType == "Human")
            {
                this.player2 = new Human(CurrentBoard.Player2);
                for (uint y = 0; y < CurrentBoard.BoardSizeY; y++)
                {
                    for (uint x = 0; x < CurrentBoard.BoardSizeX; x++)
                    {
                        Here.Y = y;
                        Here.X = x;
                        m_BoardState[x, y] = new Square(CurrentBoard.getSquare(Here), (Human)this.player1, (Human)this.player2);
                    }

                }
            }
            else
            {
                this.player2 = new Computer((Computer)CurrentBoard.Player2);
                for (uint y = 0; y < CurrentBoard.BoardSizeY; y++)
                {
                    for (uint x = 0; x < CurrentBoard.BoardSizeX; x++)
                    {
                        Here.Y = y;
                        Here.X = x;
                        m_BoardState[x, y] = new Square(CurrentBoard.getSquare(Here), (Human)this.player1, (Computer)this.player2);
                    }

                }
            }                
        }

        public Player Player1
        {
            get
            {
                return player1;
            }
            set
            {
                player1 = value;
            }
        }

        public Player Player2
        {
            get
            {
                return player2;
            }
            set
            {
                player2 = value;
            }
        }

        public uint BoardSizeX
        {
            get
            {
                return boardSize.X;
            }
            set
            {
                boardSize.X = value;
            }
        }

        public uint BoardSizeY
        {
            get
            {
                return boardSize.Y;
            }
            set
            {
                boardSize.Y = value;
            }
        }

        //creates a public function called "getSquare", it will return the reference to the square object that sits in the position in "array" that is asked for in the arguments.
        public Square getSquare(Coordinate Find)
        {
            try
            {
                return m_BoardState[Find.X, Find.Y];
            }
            catch
            {
                return null;
            }

        }

        public Square[,] getBoard
        {
            get
            {
                return m_BoardState;
            }
            set
            {
                m_BoardState = value;
            }
        }

        //barracks no/health, base health, units no/health.  both players

        public uint Counting(String Type, Player ThisPlayer, bool CheckNumber)
        {
            //check number switches the function to wither count the total health of deployment or just the amount of the deployment
            uint counted = 0;
            Coordinate Here = new Coordinate();
            for (uint y = 0; y < this.BoardSizeY; y++)
            {
                for (uint x = 0; x < this.BoardSizeX; x++)
                {
                    Here.Y = y;
                    Here.X = x;
                    if((this.getSquare(Here).Dep.OwnedBy == ThisPlayer) && (this.getSquare(Here).Dep.DepType == Type))
                    {
                        if (CheckNumber == true)
                        {
                            counted++;
                        }
                        else
                        {
                            counted += this.getSquare(Here).Dep.Health;
                        }
                        
                    }
                }

            }

            return counted;
        }

        public uint ClosestDistance(Coordinate UnitPos, Player OpPlayer)
        {
            uint BestDistance = uint.MaxValue;
            Coordinate SearchPos = new Coordinate();
            for (uint y = 0; y < this.BoardSizeY; y++)
            {
                for (uint x = 0; x < this.BoardSizeX; x++)
                {
                    SearchPos.Y = y;
                    SearchPos.X = x;
                    if ((this.getSquare(SearchPos).Dep.OwnedBy == OpPlayer) && ( (this.getSquare(SearchPos).Dep.DepType == "Barracks") || this.getSquare(SearchPos).Dep.DepType == "Base") )
                    {
                        if(UnitPos.Get2DDistance(SearchPos) < BestDistance)
                        {
                            BestDistance = (uint)UnitPos.Get2DDistance(SearchPos);
                        }

                    }
                }

            }

            return BestDistance;
        }

        public uint SumOfDistances(Coordinate UnitPos, Player OpPlayer)
        {
            uint SumDistance = 0;
            Coordinate SearchPos = new Coordinate();
            for (uint y = 0; y < this.BoardSizeY; y++)
            {
                for (uint x = 0; x < this.BoardSizeX; x++)
                {
                    SearchPos.Y = y;
                    SearchPos.X = x;
                    if ((this.getSquare(SearchPos).Dep.OwnedBy == OpPlayer) && ((this.getSquare(SearchPos).Dep.DepType == "Barracks") || this.getSquare(SearchPos).Dep.DepType == "Base"))
                    {
                        if(this.getSquare(SearchPos).Dep.DepType == "Base")
                        {
                            SumDistance += 5 * (uint)UnitPos.Get2DDistance(SearchPos);
                        }
                        else
                        {
                            SumDistance += (uint)UnitPos.Get2DDistance(SearchPos);
                        }
                    }
                }

            }

            return SumDistance;
        }

        public double DistanceValuation(Player MePlayer, Player OpPlayer)
        {
            Double TotalDistanceValue = 0;
            Coordinate Here = new Coordinate();
            for (uint y = 0; y < this.BoardSizeY; y++)
            {
                for (uint x = 0; x < this.BoardSizeX; x++)
                {
                    Here.Y = y;
                    Here.X = x;
                    if ((this.getSquare(Here).Dep.OwnedBy == MePlayer) && (this.getSquare(Here).Dep.DepType == "Unit"))
                    {
                        TotalDistanceValue = TotalDistanceValue + 1 / (ClosestDistance(Here,OpPlayer));
                        //TotalDistanceValue = TotalDistanceValue + (1 / (SumOfDistances(Here, OpPlayer)));
                    }
                }

            }

            return TotalDistanceValue;
        }

        public uint BarracksCost(BoardState ThisBoard, Player Buyer)
        {
            uint Bcost = 1;
            Coordinate ThisCo = new Coordinate();
            for (uint y = 0; y < BoardSizeY; y++)
            {
                for (uint x = 0; x < BoardSizeX; x++)
                {
                    ThisCo.X = x;
                    ThisCo.Y = y;
                    if (this.getSquare(ThisCo).Dep.DepType == "Barracks"  && this.getSquare(ThisCo).Dep.OwnedBy == Buyer)
                    {
                        Bcost++;
                    }
                }
            }

            return (3 * Bcost);
        }


    }
}
