using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Stish_GUI
{
    public partial class Form1 : Form
    {
        private Button[,] m_Buttons;
        private List<Button> Highlighted = new List<Button>();
        private Button CentreButton = new Button();
        private Label m_Label;
        private Label m_LabelP1;
        private Label m_LabelP2;
        private Label m_LabelTurn;
        private int ButtonSize;
        public enum PlayersTurn { Player1, Player2 };

        //This is a very bad and temporary fix to not knowing how to attach more information to a menu selection
        private Button BuyingFrom = new Button();

        public Form1()
        {
            m_Buttons = new Button[StishBoard.Instance.BoardSizeX, StishBoard.Instance.BoardSizeY];
            StishBoard.Instance.Player1 = Player.PlayerFactory(Player.PlayerNumber.Player1, Player.PlayerType.Human, StishBoard.Instance);
            StishBoard.Instance.Player2 = Player.PlayerFactory(Player.PlayerNumber.Player2, Player.PlayerType.Computer, StishBoard.Instance);



            InitializeComponent();
            Shown += new EventHandler(Form1_Shown);
            
        }

        private void Form1_Shown(Object sender, EventArgs e)
        {
            ButtonSize = 75;
            this.Size = new Size(((int)StishBoard.Instance.BoardSizeX * (ButtonSize)) + 515, ((ButtonSize + 5) * (int)StishBoard.Instance.BoardSizeY));         

            for (int x = 0; x < StishBoard.Instance.BoardSizeX; x++)
            {
                for (int y = 0; y < StishBoard.Instance.BoardSizeY; y++)
                {
                    Button dynamicButton = new Button();
                    dynamicButton.Location = new Point(ButtonSize * x, ButtonSize * y);

                    dynamicButton.Height = ButtonSize;
                    dynamicButton.Width = ButtonSize;
                    dynamicButton.BackColor = System.Drawing.Color.FromArgb(240, 240, 240);
                    dynamicButton.ForeColor = Color.White;

                    dynamicButton.Text = "";
                    dynamicButton.Name = "DynamicButton";
                    dynamicButton.Font = new Font("Ariel", 10);

                    dynamicButton.Click += new EventHandler(BasicButtonOnClick);
                    dynamicButton.MouseEnter += new EventHandler(ButtonOnMouseEnter);
                    dynamicButton.MouseLeave += new EventHandler(ButtonOnMouseLeave);
                    Controls.Add(dynamicButton);
                    m_Buttons[x, y] = dynamicButton;
                   
                }
            }

            Button EndTurnButton = new Button();
            EndTurnButton.Height = ButtonSize;
            EndTurnButton.Width = ButtonSize * (int)StishBoard.Instance.BoardSizeX;

            EndTurnButton.BackColor = Color.HotPink;
            EndTurnButton.ForeColor = Color.Black;
            EndTurnButton.Location = new Point(ButtonSize * (int)StishBoard.Instance.BoardSizeX, ButtonSize * (((int)StishBoard.Instance.BoardSizeY) - 1));
            EndTurnButton.Text = "End Turn";
            EndTurnButton.Name = "EndTurnButton";
            EndTurnButton.Font = new Font("Georgia", 20);
            EndTurnButton.Click += new EventHandler(EndTurnButtonClick);
            Controls.Add(EndTurnButton);

            m_LabelP1 = new Label();
            m_LabelP1.Location = new Point(ButtonSize * ((int)StishBoard.Instance.BoardSizeX + 1), 0);
            m_LabelP1.Height = ButtonSize * 2;
            m_LabelP1.Width = 400;
            m_LabelP1.Font = new Font("Georgia", 18);
            m_LabelP1.BackColor = System.Drawing.Color.FromArgb(240, 240, 240);
            m_LabelP1.ForeColor = Color.Red;
            m_LabelP1.Show();
            Controls.Add(m_LabelP1);

            m_LabelP2 = new Label();
            m_LabelP2.Location = new Point(ButtonSize * ((int)StishBoard.Instance.BoardSizeX + 1), ButtonSize * 2);
            m_LabelP2.Height = ButtonSize * 2;
            m_LabelP2.Width = 400;
            m_LabelP2.Font = new Font("Georgia", 18);
            m_LabelP2.BackColor = System.Drawing.Color.FromArgb(240, 240, 240);
            m_LabelP2.ForeColor = System.Drawing.Color.FromArgb(86, 111, 192);
            m_LabelP2.Show();
            Controls.Add(m_LabelP2);

            m_LabelTurn = new Label();
            m_LabelTurn.Location = new Point(ButtonSize * ((int)StishBoard.Instance.BoardSizeX + 1), ButtonSize * 4);
            m_LabelTurn.Height = ButtonSize * 2;
            m_LabelTurn.Width = 400;
            m_LabelTurn.Font = new Font("Georgia", 18);
            m_LabelTurn.BackColor = System.Drawing.Color.FromArgb(240, 240, 240);
            m_LabelTurn.ForeColor = Color.Black;
            m_LabelTurn.Show();
            Controls.Add(m_LabelTurn);
           
            m_Label = new Label();
            m_Label.Location = new Point(ButtonSize * ((int)StishBoard.Instance.BoardSizeX + 1), ButtonSize * 6);
            m_Label.Height = (int)StishBoard.Instance.BoardSizeY * ButtonSize;
            m_Label.Width = 400;
            m_Label.Font = new Font("Georgia", 18);
            m_Label.BackColor = System.Drawing.Color.FromArgb(240, 240, 240);
            m_Label.Show();
            Controls.Add(m_Label);          

            StishBoard.Instance.Player1.TurnBalance(StishBoard.Instance);

        }

        private void GameOver(Player Winner)
        {
            DisableBoard();
            m_Label.Text = Winner.GetPlayerNum + " has Won";
            m_LabelP1.Text = Winner.GetPlayerNum + " has Won";
            m_LabelP2.Text = Winner.GetPlayerNum + "  has Won";
            m_LabelTurn.Text = Winner.GetPlayerNum + "  has Won";
            Task.Delay(10000);
        }

        private void EndTurn()
        {         
            Coordinate P1Base = new Coordinate(StishBoard.Instance.Player1.BaseX, StishBoard.Instance.Player1.BaseY);
            Coordinate P2Base = new Coordinate(StishBoard.Instance.Player2.BaseX, StishBoard.Instance.Player2.BaseY);
            if (StishBoard.Instance.getSquare(P2Base).Dep.Health < 1)
            {
                //Player1 has won
                DisableBoard();
                m_Label.Text = "Player 1 has Won";
                m_LabelP1.Text = "Player 1 has Won";
                m_LabelP2.Text = "Player 1 has Won";
                m_LabelTurn.Text = "Player 1 has Won";
                GameOver(StishBoard.Instance.Player1);
            }
            else if (StishBoard.Instance.getSquare(P1Base).Dep.Health < 1)
            {
                //Player2 has won
                DisableBoard();
                m_Label.Text = "Player 2 has Won";
                m_LabelP1.Text = "Player 2 has Won";
                m_LabelP2.Text = "Player 2 has Won";
                m_LabelTurn.Text = "Player 2 has Won";
                GameOver(StishBoard.Instance.Player2);
            }

            m_LabelTurn.ForeColor = System.Drawing.Color.FromArgb(86, 111, 192);
            m_LabelTurn.Text = "Player 2's Turn";

            DisableBoard();
            StishBoard.Instance.GamePlayersTurn = StishBoard.PlayersTurn.Player2;
            RefreshBoard();

            //StishBoard.Instance.Player2.TurnBalance(StishBoard.Instance);
            StishBoard.Instance.Player2.MaxMP(StishBoard.Instance);

            StishBoard.Instance.Player2.MakeMove();

            StishBoard.Instance.Player1.TurnBalance(StishBoard.Instance);
            StishBoard.Instance.Player1.MaxMP(StishBoard.Instance);

            StishBoard.Instance.GamePlayersTurn = StishBoard.PlayersTurn.Player1;
            m_LabelTurn.ForeColor = Color.Red;
            m_LabelTurn.Text = "Player 1's Turn";

            RefreshBoard();
            EnableBoard();
        }

        private void EndTurnButtonClick(object sender, System.EventArgs e)
        {
            EndTurn();
        }

        private void RefreshBoard()
        {
            if(StishBoard.Instance.GamePlayersTurn == StishBoard.PlayersTurn.Player1)
            {
                m_LabelTurn.ForeColor = Color.Red;
                m_LabelTurn.Text = "Player 1's Turn";
            }
            else if (StishBoard.Instance.GamePlayersTurn == StishBoard.PlayersTurn.Player2)
            {
                m_LabelTurn.ForeColor = System.Drawing.Color.FromArgb(86, 111, 192);
                m_LabelTurn.Text = "Player 2's Turn";
            }

            m_LabelP1.Text = "Player 1's Balance: " + StishBoard.Instance.Player1.Balance + "\r\nTheir next barracks will cost: " + StishBoard.Instance.BarracksCost(StishBoard.Instance, StishBoard.Instance.Player1);
            m_LabelP2.Text = "Player 2's Balance: " + StishBoard.Instance.Player2.Balance + "\r\nTheir next barracks will cost: " + StishBoard.Instance.BarracksCost(StishBoard.Instance, StishBoard.Instance.Player2);                  

            Coordinate Inspect = new Coordinate(StishBoard.Instance.Player1.BaseX, StishBoard.Instance.Player1.BaseY);
            if (StishBoard.Instance.getSquare(Inspect).Dep.Health < 1)
            {
                GameOver(StishBoard.Instance.Player2);
            }
            Inspect.X = StishBoard.Instance.Player2.BaseX;
            Inspect.Y = StishBoard.Instance.Player2.BaseY;
            if (StishBoard.Instance.getSquare(Inspect).Dep.Health < 1)
            {
                GameOver(StishBoard.Instance.Player1);
            }

            for (uint x = 0; x < StishBoard.Instance.BoardSizeX; x++)
            {
                for (uint y = 0; y < StishBoard.Instance.BoardSizeY; y++)
                {
                    Inspect.X = x;
                    Inspect.Y = y;
                    Button dynamicButton = m_Buttons[x, y];

                    dynamicButton.ForeColor = Color.Black;
                    try
                    {
                        if (StishBoard.Instance.getSquare(Inspect).Owner.GetPlayerNum == "Player1")
                        {
                            dynamicButton.BackColor = Color.Red;
                        }
                        else if (StishBoard.Instance.getSquare(Inspect).Owner.GetPlayerNum == "Player2")
                        {
                            dynamicButton.BackColor = System.Drawing.Color.FromArgb(86, 111, 192);
                        }
                    }
                    catch
                    {
                        dynamicButton.BackColor = Color.LightGray;
                    }

                    if(IsHighlighted(dynamicButton) == true)
                    {
                        dynamicButton.BackColor = Color.HotPink;
                    }

                    if (StishBoard.Instance.getSquare(Inspect).Dep.DepType == "Unit")
                    {
                        dynamicButton.Text = "HP: " + StishBoard.Instance.getSquare(Inspect).Dep.Health + "\r\n" + StishBoard.Instance.getSquare(Inspect).Dep.DepType + "\r\n MP: " + StishBoard.Instance.getSquare(Inspect).Dep.MP;
                    }
                    else if(StishBoard.Instance.getSquare(Inspect).Dep.DepType != "Empty")
                    {
                        dynamicButton.Text = "HP: " + StishBoard.Instance.getSquare(Inspect).Dep.Health + "\r\n" + StishBoard.Instance.getSquare(Inspect).Dep.DepType + "\r\n";
                    }
                    else
                    {
                        dynamicButton.Text = StishBoard.Instance.getSquare(Inspect).Dep.DepType;
                    }
                    
                    dynamicButton.Name = "DynamicButton";
                    dynamicButton.Font = new Font("Georgia", 10);
                }
            }
        }

        private Button FindButtonArray(Button Seek)
        {

            for(uint x = 0; x < StishBoard.Instance.BoardSizeX; x++)
            {
                for (uint y = 0; y < StishBoard.Instance.BoardSizeY; y++)
                {
                    if(m_Buttons[x,y] == Seek)
                    {
                        return m_Buttons[x, y];
                    }
                }
            }
            return null;
        }

        private Coordinate FindButtonPosition(Button Seek)
        {
            Coordinate Position = new Coordinate();

            for (uint x = 0; x < StishBoard.Instance.BoardSizeX; x++)
            {
                for (uint y = 0; y < StishBoard.Instance.BoardSizeY; y++)
                {
                    if (m_Buttons[x, y] == Seek)
                    {
                        Position.X = x;
                        Position.Y = y;
                        return Position;
                    }
                }
            }
            return null;
        }

        private void DisableBasic()
        {
            Coordinate Position = new Coordinate();

            for (uint x = 0; x < StishBoard.Instance.BoardSizeX; x++)
            {
                for (uint y = 0; y < StishBoard.Instance.BoardSizeY; y++)
                {
                    Position.X = x;
                    Position.Y = y;
                    

                    if (!(IsHighlighted(m_Buttons[x,y]) == true || m_Buttons[x,y] == CentreButton))
                    {
                        //basic button
                        m_Buttons[x, y].Enabled = false;
                    }
                }
            }
        }

        private void DisableBoard()
        {
            Coordinate Position = new Coordinate();

            for (uint x = 0; x < StishBoard.Instance.BoardSizeX; x++)
            {
                for (uint y = 0; y < StishBoard.Instance.BoardSizeY; y++)
                {
                    Position.X = x;
                    Position.Y = y;

                    m_Buttons[x, y].Enabled = false;
                }
            }
        }

        private void EnableBoard()
        {
            Coordinate Position = new Coordinate();

            for (uint x = 0; x < StishBoard.Instance.BoardSizeX; x++)
            {
                for (uint y = 0; y < StishBoard.Instance.BoardSizeY; y++)
                {
                    Position.X = x;
                    Position.Y = y;

                    m_Buttons[x, y].Enabled = true;
                }
            }
        }        

        private bool IsHighlighted(Button Seek)
        {
            try
            {
                for (int index = 0; index < StishBoard.Instance.BoardSizeX; index++)
                {

                    if (Highlighted[index] == Seek)
                    {
                        return true;
                    }

                }
                return false;
            }
            catch
            {
                return false;
            }
                     
        }

        private void RedressEvents()
        {
            Coordinate Position = new Coordinate();

            for (uint x = 0; x < StishBoard.Instance.BoardSizeX; x++)
            {
                for (uint y = 0; y < StishBoard.Instance.BoardSizeY; y++)
                {
                    Position.X = x;
                    Position.Y = y;

                    m_Buttons[x, y].Click -= new EventHandler(SpecialButtonOnClick);
                    m_Buttons[x, y].Click += new EventHandler(BasicButtonOnClick);
                    if (IsHighlighted(m_Buttons[x,y]) == true || m_Buttons[x,y] == CentreButton)
                    {
                        m_Buttons[x, y].Click += new EventHandler(SpecialButtonOnClick);
                        m_Buttons[x, y].Click -= new EventHandler(BasicButtonOnClick);
                    }
                }
            }
        }

        private void BasicButtonOnClick(object sender, System.EventArgs e)
        {
            RefreshBoard();

            Coordinate ThisCoordinate = FindButtonPosition((Button)sender);
            Button ThisButton = FindButtonArray((Button)sender);
            if (StishBoard.Instance.getSquare(ThisCoordinate).Owner != null)
            {
                if (StishBoard.Instance.getSquare(ThisCoordinate).Dep.DepType == "Unit" && StishBoard.Instance.getSquare(ThisCoordinate).Owner.GetPlayerNum == "Player1" && ThisButton != CentreButton && IsHighlighted(ThisButton) == false)
                {
                    //we can make the assumption that player1 is clicking the button as the AI cannot
                    //the player wants to select a new unit for the first time
                    TwitchChange(ThisCoordinate);
                    CentreButton = ThisButton;
                    DisableBasic();
                    RedressEvents();
                    RefreshBoard();
                }
                if (StishBoard.Instance.getSquare(ThisCoordinate).Dep.DepType == "Empty" && StishBoard.Instance.getSquare(ThisCoordinate).Owner.GetPlayerNum == "Player1" && IsHighlighted(ThisButton) == false)
                {
                    //we can make the assumption that player1 is clicking the button as the AI cannot
                    ContextMenu menu = new ContextMenu();
                    menu.MenuItems.Add("&Unit", new EventHandler(MenuUnitOnClick));
                    menu.MenuItems.Add("&Barracks", new EventHandler(MenuBarracksOnClick));
                    menu.Show(this, new Point(((int)ThisCoordinate.X + 1) * ButtonSize, (int)ThisCoordinate.Y * ButtonSize));
                    BuyingFrom = ThisButton;
                }
            }
            


            /*
            Button ThisButton = FindButtonArray((Button)sender);
            ThisButton.Text = "Unit";
            ThisButton.BackColor = System.Drawing.Color.FromArgb(255, 255, 153);
            
            Coordinate ThisCoordinate = FindButtonPosition((Button)sender);
            TwitchChange(ThisCoordinate);

            if(ThisCoordinate.Y == 2)
            {
                ContextMenu menu = new ContextMenu();
                menu.MenuItems.Add("&Unit", new EventHandler(MenuUnitOnClick));
                menu.MenuItems.Add("&Barracks", new EventHandler(MenuBarracksOnClick));
                menu.Show(this, new Point((int)ThisCoordinate.X * 60, (int)ThisCoordinate.Y * 60));
            }

            Button DisabledButton = m_Buttons[1, 1];
            DisabledButton.Enabled = !DisabledButton.Enabled;
            */


        }

        private void SpecialButtonOnClick(object sender, System.EventArgs e)
        {
            //is used by highlighted and centre buttons
            Button ThisButton = FindButtonArray((Button)sender);
            Coordinate ThisCoordinate = FindButtonPosition((Button)sender);
            Coordinate CentreCoordinate = FindButtonPosition(CentreButton);

            if (IsHighlighted(ThisButton) == true)
            {
                //move unit to this position and refresh the highlighted buttons
                bool ActuallyMoved = GameMaster.Instance.Action(CentreCoordinate, ThisCoordinate, StishBoard.Instance.Player1, StishBoard.Instance);
                Coordinate Where = new Coordinate();
                if(ActuallyMoved == true)
                {
                    Where = ThisCoordinate;
                    CentreButton = ThisButton;
                }
                else
                {
                    Where = CentreCoordinate;
                    CentreButton = m_Buttons[CentreCoordinate.X, CentreCoordinate.Y];
                }

                Highlighted.Clear();
                if (StishBoard.Instance.getSquare(Where).Dep.MP > 0 && StishBoard.Instance.getSquare(Where).Dep.Health > 0)
                {
                    //carries on movement process
                    // no change to CentreButton
                    TwitchChange(Where);                
                    EnableBoard();
                    DisableBasic();
                    RedressEvents();
                    RefreshBoard();
                }
                else
                {
                    //auto ends turn for running out of MP or HP
                    CentreButton = null;
                    RedressEvents();
                    RefreshBoard();
                    EndTurn();
                }

            }
            else if (ThisButton == CentreButton)
            {
                //a unit that has already been selected. must find out if already moved or not
                if(StishBoard.Instance.getSquare(ThisCoordinate).Dep.MP == StishBoard.Instance.GameMP)
                {
                    //unit hasn't moved yet
                    Highlighted.Clear();
                    CentreButton = null;
                    RedressEvents();
                    EnableBoard();
                    RefreshBoard();
                }
                else if (StishBoard.Instance.getSquare(ThisCoordinate).Dep.MP < StishBoard.Instance.GameMP)
                {
                    //unit has already moved
                    Highlighted.Clear();
                    CentreButton = null;
                    RedressEvents();
                    RefreshBoard();
                    EndTurn();
                }
            }
            RefreshBoard();
        }

        private void ButtonOnMouseEnter(object sender, System.EventArgs e)
        {

            Button ThisButton = FindButtonArray((Button)sender);
            Coordinate ThisCoordinate = FindButtonPosition((Button)sender);
            m_Label.Text = "Button Coordinate: " + ThisCoordinate.X + "," + ThisCoordinate.Y;
        }

        private void ButtonOnMouseLeave(object sender, System.EventArgs e)
        {

            Button ThisButton = FindButtonArray((Button)sender);
            Coordinate ThisCoordinate = FindButtonPosition((Button)sender);
            m_Label.Text = "";
        }

        public void TwitchChange(Coordinate ThisCoordinate)
        {
            Coordinate Twitch = new Coordinate();

            for (int dir = 0; dir < 4; dir++)
            {

                Twitch.X = ThisCoordinate.X;
                Twitch.Y = ThisCoordinate.Y;

                if (dir == 0)
                {
                    //up
                    Twitch.MoveUp();
                }
                else if (dir == 1)
                {
                    //right
                    Twitch.MoveRight();
                }
                else if (dir == 2)
                {
                    //down
                    Twitch.MoveDown();
                }
                else if (dir == 3)
                {
                    //left
                    Twitch.MoveLeft();
                }

                try
                {
                    Button TwitchButton = m_Buttons[Twitch.X, Twitch.Y];
                    Highlighted.Add(TwitchButton);
                    TwitchButton.Click += new EventHandler(SpecialButtonOnClick);
                    TwitchButton.Click -= new EventHandler(BasicButtonOnClick);
                }
                catch
                {

                }

            }
        }
      

        private void MenuUnitOnClick(object sender, System.EventArgs e)
        {
            Task.Delay(3000);
            Coordinate ThisCoordinate = FindButtonPosition(BuyingFrom);
            Button ThisButton = FindButtonArray(BuyingFrom);
            bool ActuallyBought = GameMaster.Instance.BuyUnit(ThisCoordinate, StishBoard.Instance.Player1, StishBoard.Instance);
            RefreshBoard();

            if (ActuallyBought == true)
            {
                EndTurn();
            }         
        }

        private void MenuBarracksOnClick(object sender, System.EventArgs e)
        {
            Coordinate ThisCoordinate = FindButtonPosition(BuyingFrom);
            Button ThisButton = FindButtonArray(BuyingFrom);
            bool ActuallyBought = GameMaster.Instance.BuyBarracks(ThisCoordinate, StishBoard.Instance.Player1, StishBoard.Instance);
            RefreshBoard();

            if (ActuallyBought == true)
            {
                EndTurn();
            }
        }



    }
}
