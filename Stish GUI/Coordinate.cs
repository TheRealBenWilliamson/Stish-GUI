﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stish_GUI
{
    public class Coordinate
    {
        private uint Xco;
        private uint Yco;

        public uint X
        {
            get
            {
                return Xco;
            }
            set
            {
                Xco = value;
            }
        }

        public uint Y
        {
            get
            {
                return Yco;
            }
            set
            {
                Yco = value;
            }
        }

        public Coordinate()
        {
            Xco = 0;
            Yco = 0;
        }

        public Coordinate(uint CalledX, uint CalledY)
        {
            Xco = CalledX;
            Yco = CalledY;
        }

        public Coordinate(Coordinate CopyFrom)
        {
            Xco = CopyFrom.X;
            Yco = CopyFrom.Y;
        }

        public void MoveLeft()
        {
            Xco--;
        }
        public void MoveRight()
        {
            Xco++;
        }
        public void MoveUp()
        {
            Yco--;
        }
        public void MoveDown()
        {
            Yco++;
        }

        public int Get2DDistance(Coordinate Where)
        {
            int DeltaX = Math.Abs((int)Where.X - (int)X);
            int DeltaY = Math.Abs((int)Where.Y - (int)Y);

            return (DeltaX + DeltaY);
        }
    }
}