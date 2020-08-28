﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stish_GUI
{
    public class Computer : Player
    {
        public Computer(PlayerNumber PN, PlayerType PT, BoardState Board) : base(PN, PT, Board)
        {

        }     

        public Computer(Computer Cpu) : base(Cpu)
        {

        }

        public override void MakeMove()
        {
            /*
            StishMiniMaxNode GameNode = StishBoard.Instance.CurrentGameNode;
            GameNode.Allegiance = StishBoard.Instance.Player1;
            GameNode.NodeBoardState = new BoardState(StishBoard.Instance);
            */
            StishMiniMaxNode GameNode = new StishMiniMaxNode(null, StishBoard.Instance.Player1);
            GameNode.NodeBoardState = new BoardState(StishBoard.Instance);

            //double evaluation = MiniMaxMind.Instance.BuildABTree(GameNode, 4, int.MinValue, int.MaxValue, 1);
            MiniMaxMind.Instance.RecBuildMMTree(GameNode, 4);
            double evaluation = MiniMaxMind.Instance.TraverseTree(GameNode, 4, -1);
            //colour = 1 for odd depths. -1 for even

            //ForeSight.Instance.PredctionCount();
            try
            {
                StishBoard.Instance.ReplaceState(GameNode.BestChild.NodeBoardState);
            }
            catch
            {
                StishMiniMaxNode AIWillLose = (StishMiniMaxNode)GameNode.GetChild(0);
                StishBoard.Instance.ReplaceState(AIWillLose.NodeBoardState);
            }
            

        }


        public void DetermineMove()
        {

        }
    }
}
