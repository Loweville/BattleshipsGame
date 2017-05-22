using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleShipsGame
{
    class Player
    {
        int Id = -1;
        string playerName;
        int wins = 0;
        int losses = 0;
        double rat = 0.0;
        bool ai = false;

        public Player(string n)
        {
            playerName = n;
        }

        public Player(string n, int w, int l, double r)
        {
            playerName = n;
            wins = w;
            losses = l;
            rat = r;
        }

        public bool IsAI
        {
            get 
            {
                return ai;
            }
            set
            {
                ai = value;
            }

        }

        public int ID
        {
            get
            {
                return Id;
            }
            set
            {
                Id = value;
            }
        }

        public string Name()
        {
            return playerName;
        }

        public int TotalWins
        {
            get
            {
                return wins;
            }
            set
            {
                wins = value;
                rat += 1.0;
            }
        }

        public int TotalLosses
        {
            get
            {
                return losses;
            }
            set
            {
                losses = value;
                rat += 0.1;
            }
        }

        public double WinLossRatio()
        {
            return rat;
        }
    }
}
