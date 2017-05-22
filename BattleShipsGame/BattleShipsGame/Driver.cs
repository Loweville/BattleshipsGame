using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace BattleShipsGame
{
    class Driver
    {
        bool AI = false;
        List<Player> p;
        List<string> vals;
        bool Exp = false;
        string file = "SavedGame.txt";

        static void Main() 
        {
            Driver d = new Driver();
            Console.WindowHeight = 22;
            d.StartScreen();
        }

        public void StartScreen()
        {
            Console.WriteLine("Welcome to Battleships!");
            Console.WriteLine("                 ___       _   _   _           _     _           \n"+
                "                / __\\ __ _| |_| |_| | ___  ___| |__ (_)_ __  ___ \n" +
                "               /__\\/// _` | __| __| |/ _ \\/ __| '_ \\| | '_ \\/ __|\n" +
                "              / \\/  \\ (_| | |_| |_| |  __/\\__ \\ | | | | |_) \\__ \\\n" +
                "              \\_____/\\__,_|\\__|\\__|_|\\___||___/_| |_|_| .__/|___/\n" +
                "                                                      |_|        \n");
            Console.WriteLine("Select your Game:\n 1: SinglePlayer\n 2: Dualplayer\n 3: Play Saved Game" +
                "\n\nPress any other key to exit game.");
            char key = Console.ReadKey().KeyChar;
            Console.WriteLine("");
            Console.WindowHeight = 32;
            if (key == '1')
            {
                AI = true;
                Play();
            }
            else if (key == '2')
            {
                Play();
            }
            else if (key == '3')
            {
                SavedPlay();
            }
        }

        private void GetVals(int pos, string input, int id)
        {
            int startPlace;
            int endPlace;
            bool fin=false;
            int ind = 97;
            vals = new List<string>();

            char ch = Convert.ToChar(ind);
            string i = Convert.ToString(ch) + '=';
            while ((startPlace = input.IndexOf(i, pos)) >= 0 &&
                (endPlace = input.IndexOf("/", pos)) >= 0 && !fin)
            {
                if (startPlace < endPlace)
                {
                    vals.Add(input.Substring(startPlace + 2, endPlace - startPlace - 2));

                    string val = input.Substring(startPlace + 2, endPlace - startPlace - 2);

                    if (ind == 101)
                    {
                        fin = true;
                    }

                    ind++;
                    ch = Convert.ToChar(ind);
                    i = Convert.ToString(ch) + '=';
                    pos = endPlace + 1;
                }
            }
            if (id == 0)
            {
                Player Player1 = new Player(vals[0], Convert.ToInt32(vals[1]), Convert.ToInt32(vals[2]),
                    Convert.ToDouble(vals[3]));

                Player1.ID = id;
                Player1.IsAI = Convert.ToBoolean(vals[4]);

                p.Add(Player1);
            }
            else
            {
                Player Player2 = new Player(vals[0], Convert.ToInt32(vals[1]), Convert.ToInt32(vals[2]),
                    Convert.ToDouble(vals[3]));

                Player2.ID = id;
                Player2.IsAI = Convert.ToBoolean(vals[4]);

                p.Add(Player2);
            }
        }

        private void GetPlayers()
        {
            using (StreamReader reader = new StreamReader(file))
            {
                p = new List<Player>();
                string read;
                int start = 480;
                int id = 0;
                
                int seekStart;
                int seekEnd;

                read = reader.ReadToEnd();
                string readVal= read.Trim();
                
                while (id < 2 && (seekStart = readVal.IndexOf("a", start)) >= 0 &&
                    (seekEnd = readVal.IndexOf("-", start)) >= 0)
                {
                    if (seekStart < seekEnd)
                    {
                        seekStart = readVal.IndexOf("a", start);
                        seekEnd = readVal.IndexOf("-", start);

                        reader.BaseStream.Seek(seekStart, SeekOrigin.Begin);
                        GetVals(seekStart, readVal, id);

                        id += 1;
                    }
                    start = seekEnd + 1;
                }
            }
            Exp = true;
            Setup("", "");
        }

        public void SavedPlay()
        {
            GetPlayers();
            string fPath = "SavedGame.txt";
            Battleship Battle = new Battleship(p, fPath);
        }

        private void Setup(string play1, string play2)
        {
            if (!Exp)
            {
                Player player1 = new Player(play1);
                Player player2 = new Player(play2);
                
                player1.ID = 0;
                player2.ID = 1;

                p.Add(player1);
                p.Add(player2);
            }

            Console.Beep(600, 500);
            Console.Clear();
        }

        public void CreatePlayers()
        {
            Console.Clear();
            p = new List<Player>();
            
            Console.ForegroundColor = ConsoleColor.Yellow;
            string play;
            if (AI)
            {
                play = "Singleplayer";
            }
            else
            {
                play = "Multiplayer";
            }
            Console.WriteLine("Welcome to {0}!\n", play);

            Console.Write("Player 1; input your name: ");
            string play1 = Console.ReadLine();

            Console.Write("Player 2; input your name: ");
            string play2 = Console.ReadLine();

            Setup(play1, play2);
        } 

        public void Play()
        {
            CreatePlayers();
            if (AI)
            {
                p.Last().IsAI = true;
            }
            Battleship Battle = new Battleship(p);
        }
    }
}
