using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Stish_GUI
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            StishBoard.Instance.Player1 = Player.PlayerFactory(Player.PlayerNumber.Player1, Player.PlayerType.Human, StishBoard.Instance); ;
            StishBoard.Instance.Player2 = Player.PlayerFactory(Player.PlayerNumber.Player2, Player.PlayerType.Computer, StishBoard.Instance); ;

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}
