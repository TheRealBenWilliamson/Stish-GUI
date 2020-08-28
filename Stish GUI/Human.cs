using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stish_GUI
{
    public class Human : Player
    {
        public Human(PlayerNumber PN, PlayerType PT, BoardState Board) :  base(PN,PT,Board)
        {

        }

        public Human(Player Hu) : base(Hu)
        {

        }

        //Option helps use enums for the player making number corrosponding branching choices
        public enum Action { MoveUnit, BuyUnit, BuyBarracks, EndTurn};

        //by now a StishBoard.Instance should already have been created. StishBoard.Instance allows us to get a reference to the existing StishBoard.Instance.

        //define methods for the player's move

        
        /*
        public override void MakeMove()
        {
            Console.Clear();
            StishBoard.Instance.Render();
            Cursor.Instance.Render(this);

            bool EndTurn = false;
            do
            {
                string output = "Nothing";
                System.ConsoleKey put = Console.ReadKey(true).Key;
                
                if (put == ConsoleKey.W)
                {
                    output = "W";
                }
                else if (put == ConsoleKey.A)
                {
                    output = "A";
                }
                else if (put == ConsoleKey.S)
                {
                    output = "S";
                }
                else if (put == ConsoleKey.D)
                {
                    output = "D";
                }
                else if (put == ConsoleKey.Spacebar)
                {
                    output = " ";
                }
                else if (put == ConsoleKey.Q)
                {
                    output = "Q";
                }
                else if (put == ConsoleKey.E)
                {
                    output = "E";
                }
                else if (put == ConsoleKey.Enter)
                {
                    output = "_";
                }

                EndTurn = Cursor.Instance.Move(this, output);

                if (EndTurn == true)
                {
                    Cursor.Instance.CursorMode = Cursor.Mode.free;
                    CursorX = Cursor.Instance.FindX;
                    CursorY = Cursor.Instance.FindY;
                    Cursor.Instance.SpaceEnds = false;
                }
                
            } while (EndTurn == false);
            
        }

        */
        

        

    }
}
