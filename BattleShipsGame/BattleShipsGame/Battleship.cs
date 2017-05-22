using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace BattleShipsGame
{
    class Battleship
    {
        Random r = new Random(DateTime.Now.Millisecond);
        int[] ships = new int[] { 5, 4, 3, 3, 2 };

        char[,] grid;
        char[,] gridT;
        char[,] gridAtt;
        char[,] gridOne;
        char[,] gridOTrack;
        char[,] gridTwo;
        char[,] gridTTrack;

        char[] c = { 'A', 'B', 'C', 'D', 'E', 
                       'F', 'G', 'H', 'I', 'J'}; 
        const int x = 10;
        const int y = 10;
        bool gameOver = false;
        Player pTurn;
        List<Player> p;
        string res;

        string path2 = @"Logs\Log0.txt";
        string file;

        private void Initialise(List<Player> p)
        {   // initializes the grid
            gridOne = new char[y, x];
            gridOTrack = new char[y, x];
            gridTwo = new char[y, x];
            gridTTrack = new char[y, x];
            this.p = p;
        }

        public Battleship(List<Player> p)
        {   //Starts game
            Initialise(p);
            file = "SavedGame.txt";

            Console.ForegroundColor = ConsoleColor.Red;
            Start(0, p);
        }

        public Battleship(List<Player> p, string filename)
        {   //Starts saved game
            Initialise(p);
            file = filename;

            Console.ForegroundColor = ConsoleColor.Red;
            Start(1, p);
        }

        private void Start(int v, List<Player> p) 
        {   // sets initial game solution
            Console.WriteLine("Press any key to begin");
            ConsoleKeyInfo info = Console.ReadKey();
            int i = 0;
            string file2 = "Log";

            while (File.Exists(path2))
            {
                i++;
                path2 = @"Logs\" + file2 + i + ".txt";
            }
            
            switch (v) 
            { 
                case 0:
                    Begin(p);
                    break;
                default:
                    BeginSaved(p);
                    break;
            }
        }

        public void Begin(List<Player> p)
        {   // creates and plays the game
            BuildGrid(p.First());
            BuildGrid(p.Last());

            Run();
        }

        public void BeginSaved(List<Player> p)
        {   // gets and continues saved game
            RebuildGrid(p.First(), file);
            RebuildGrid(p.Last(), file);

            Run();
        }

        private void Run()
        {
            while (!gameOver)
            {
                First();

                Last();
            }
        }

        private void First()
        {
            ShowBoard(p.First());
            Console.Beep(700, 500);

            Input(p.First());
        }

        private void Last()
        {   // only shows the grid if the 
            //player is human
            if (!p.Last().IsAI)
            {
                ShowBoard(p.Last());
            }
            Console.Beep(900, 500);

            Input(p.Last());
        }

        private void BuildGrid(Player player)
        {
            grid = new char[y, x];
            gridT = new char[y, x];
            gridAtt = new char[y, x];
            PlotShips();
            for (int val1 = 0; val1 < y; val1++)
            {
                for (int val2 = 0; val2 < x; val2++)
                {   // sets grid area at y and x location
                    // with an 'x' if it's not already 'o'
                    if (grid[val1, val2] != 'o')
                    {
                        grid[val1, val2] = 'x';
                    }
                }

                for (int val2 = 0; val2 < x; val2++)
                {   // Creates Target screen
                    gridT[val1, val2] = '_';
                }
            }

            // sets grid to the players corresponding grid
            if (gridOne[1, 1] == '\0')
            {
                gridOne = grid;
                gridOTrack = gridT;

            }
            else
            {
                gridTwo = grid;
                gridTTrack = gridT;
            }

            ShowBoard(player);
        }

        private void RebuildGrid(Player p, string file)
        {
            grid = new char[y, x];
            gridT = new char[y, x];
            gridAtt = new char[y, x];

            // reads the grid in the text file and writes it to the
            // in-game board

            int num = 0;
            if (p.ID != 0)
            {
                num = 20;
            }

            for (int i = 0; i < y; i++)
            {
                string[] sGrid = File.ReadAllLines(file);
                if (sGrid.Count() != 0)
                {
                    if (sGrid[num] != null)
                    {
                        for (int j = 0; j < x; j++)
                        {
                            grid[i, j] = sGrid[num][j];
                        }

                        for (int j = 0; j < x; j++)
                        {   // Creates Target screen
                            gridT[i, j] = sGrid[num + 10][j];
                        }
                    }
                    num++;
                }
                else
                {
                    ErrCall(4, p);
                }
            }

            // sets grid to the players corresponding grid
            if (gridOne[1, 1] == '\0')
            {
                gridOne = grid;
                gridOTrack = gridT;
            }
            else
            {
                gridTwo = grid;
                gridTTrack = gridT;
            }

            ShowBoard(p);
        }

        private void WriteGrid(Player p)
        {
            Console.Write("   1  2  3  4  5  6  7  8  9  10\t");
            Console.WriteLine("   1  2  3  4  5  6  7  8  9  10\n");

            if (p.ID == 0)
            {
                grid = gridOne;
                gridT = gridOTrack;
                gridAtt = gridTTrack;
            }
            else
            {
                grid = gridTwo;
                gridT = gridTTrack;
                gridAtt = gridOTrack;
            }

            for (int val1 = 0; val1 < y; val1++)
            {
                PlotCoords(val1);
                for (int val2 = 0; val2 < x; val2++)
                {   // writes grid area at y and x location
                    if (grid[val1, val2] != gridAtt[val1, val2])
                    {
                        if (grid[val1, val2] != 'o')
                        {
                            Console.ForegroundColor = ConsoleColor.Blue;
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.DarkGray;
                        }
                    }
                    else
                    {
                        if (grid[val1, val2] != 'x')
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.White;
                        }
                    }
                    Console.Write(grid[val1, val2] + "  ");
                }
                Console.Write("\t");

                PlotCoords(val1);
                for (int val2 = 0; val2 < x; val2++)
                {   // writes Target screen
                    Console.ForegroundColor = ConsoleColor.DarkGreen;
                    if (gridT[val1, val2] == 'o')
                    {
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                    }
                    else if (gridT[val1, val2] == 'x')
                    {
                        Console.ForegroundColor = ConsoleColor.DarkRed;
                    }
                    Console.Write(gridT[val1, val2] + "  ");
                }
                Console.WriteLine("\n");
            }

        }

        private void PlotCoords(int coord)
        {   // sets y axis plots to be displayed
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write(c[coord] + "  ");
        }

        public void PlotShips()
        {
            bool or = IsX();
            int xPlot = r.Next(y - 5);
            int yPlot = r.Next(y - 5);

            foreach (int i in ships)
            {   // sets 5 ships on the grid at random 
                // coordinates to 'o'

                while (!CheckPlot(or, yPlot, xPlot, i))
                {
                    if (IsX())
                    {
                        or = true;
                    }
                    else
                    {
                        or = false;
                    }

                    if (or)
                    {
                        xPlot = r.Next(x - i);
                        yPlot = r.Next(y - 1);
                    }
                    else
                    {
                        xPlot = r.Next(x - 1);
                        yPlot = r.Next(y - i);
                    }

                }
                Plot(i, yPlot, xPlot, or);

                if (IsX())
                {
                    or = true;
                }
                else
                {
                    or = false;
                }

            }
        }

        public void Plot(int i, int yPlot, int xPlot, bool isX)
        {   // Plots the Ship
            for (int v = 0; v < i; v++)
            {
                grid[yPlot, xPlot] = 'o';


                if (isX)
                {
                    xPlot++;
                }
                else
                {
                    yPlot++;
                }

            }
        }

        public bool CheckPlot(bool isX, int yPlot, int xPlot, int i)
        {   // checks plot is valid
            bool value = false;
            for (int a = 0; a < i; a++)
            {
                if (isX && grid[yPlot, xPlot + a] != 'o' ||
                    !isX && grid[yPlot + a, xPlot] != 'o')
                {
                    value = true;
                }
                else
                {
                    return false;
                }
            }
            return value;
        }

        private bool IsX()
        {   // dictates orientation of any input element
            int b = r.Next(2);
            switch (b)
            {
                case 0:
                    return true;
                default:
                    return false;
            }
        }

        private void ShowBoard(Player p)
        {   // Displays information about board for the 
            // current player and the board itself
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Input \'x\' or \'X\' to Export Game");
            Console.WriteLine("Player: {0}.\n", p.Name());
            WriteGrid(p);
        }

        private void Input(Player p)
        {   // handles input by the user
            string coord;
            pTurn = p;
            if (!p.IsAI)
            {
                // if the player is human, allows player 
                // to input attack coordinates
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write("{0}, please input coordinate to attack: ", p.Name());

                coord = Console.ReadLine();
                if (coord != string.Empty)
                {
                    if (coord.ToUpper() == "X")
                    {
                        Export();
                        Input(p);
                    }
                    else
                    {
                        PlayMove(pTurn, coord);
                    }
                }
                else
                {
                    ErrCall(3, p);
                }
            }
            else 
            {
                PlayMove(pTurn, PlayAI());
            }
        }

        private string PlayAI()
        {   // if the player is AI, maps out
            // the AI's turn
            string coord;
            Console.Clear();
            for (int yPlot = 0; yPlot < y; yPlot++)
            {
                for (int xPlot = 0; xPlot < x; xPlot++)
                {   // sets grid area at y and x location
                    // with an 'x' if it's not already 'o'
                    if (gridTTrack[yPlot, xPlot] == 'o')
                    {
                        bool or = IsX();
                        if (or)
                        {
                            or = IsX();
                            if (or && xPlot < 10)
                            {
                                return coord = c[yPlot + 1] + "" + (xPlot + 1);
                            }
                            else if (xPlot > 0)
                            {
                                return coord = c[yPlot - 1] + "" + (xPlot + 1);
                            }
                            else
                            {
                                return coord = c[yPlot] + "" + (xPlot + 1);
                            }
                        }
                        else
                        {
                            or = IsX();
                            if (or && yPlot < 10)
                            {
                                return coord = c[yPlot + 1] + "" + (xPlot + 1);
                            }
                            else if (yPlot > 0)
                            {
                                return coord = c[yPlot + 1] + "" + (xPlot - 1);
                            }
                            else
                            {
                                return coord = c[yPlot + 1] + "" + (xPlot);
                            }
                        }
                    }
                    else if (gridTTrack[yPlot, xPlot] == 'x')
                    {
                        return RandomCoord();
                    }
                }
            }
            return RandomCoord();
        }

        string coord;
        private string RandomCoord()
        {
            int plotX;
            int plotY;
            do
            {
                plotX = r.Next(9);
                plotY = r.Next(9);
            } 
            while (!Plotted(plotX, plotY));

            return coord = c[plotY] + "" + (plotX);
        }

        private bool Plotted(int xp, int yp)
        {   // checks if the selected coordinate has been 
            // plotted as a ship
            if (gridTTrack[yp, xp] == '_')
            {
                return true;
            }
            return false;
        }

        private void Export()
        {   // control method for exporting current game
            ExportGame();
            Console.WriteLine("Game Exported!");
            Console.Beep(300, 500);
        }

        private void ExportGame()
        {   // writes the players grids to the
            // 'SavedGame.txt' file
            string[] sGrid = new string[40];
            using (StreamWriter writer = new StreamWriter(file))
            {
                for (int i = 0; i < y * 4; i++)
                {
                    for (int j = 0; j < x; j++)
                    {   // if 'i' is <10 then it should
                        // write Player 1's grid otherwise 
                        // it should write Player 2's grid
                        if (i < y)
                        {
                            sGrid[i] += gridOne[i, j];
                        }
                        else if (i < y + 10)
                        {
                            sGrid[i] += gridOTrack[i - 10, j];
                        }
                        else if (i < y + 20)
                        {
                            sGrid[i] += gridTwo[i - 20, j];
                        }
                        else
                        {
                            sGrid[i] += gridTTrack[i - 30, j];
                        }
                    }
                    writer.WriteLine(sGrid[i]);
                }
                foreach (Player i in p)
                {
                    writer.WriteLine("a={0}/b={1}/c={2}/d={3}/e={4}/-", i.Name(),
                        i.TotalWins, i.TotalLosses, i.WinLossRatio(), i.IsAI);
                }
            }
        }

        public bool PlayMove(Player p, string coord)
        {   // plays the moves set out by the players
            if (!gameOver)
            {
                if (p == pTurn)
                {   // gets input coodinates to attack
                    if (coord.Length < 2 || !Contained(coord.Substring(1), true) ||
                        !Contained(Convert.ToString(coord[0]), false))
                    {
                        ErrCall(2, p);
                        return false;
                    }
                    char pre = coord[0];
                    int preInt = CheckDim(pre);
                    int conv = Convert.ToInt32(coord.Substring(1));
                    int post = conv - 1;

                    if (preInt > 9 || post > 9 || preInt <= -1 || post <= -1)
                    {
                        ErrCall(1, p);
                        return false;
                    }

                    if (p.ID == 0)
                    {
                        grid = gridTwo;
                        gridT = gridOTrack;
                    }
                    else
                    {
                        grid = gridOne;
                        gridT = gridTTrack;
                    }
                    
                    if (p.IsAI)
                    {
                        Console.WriteLine(coord);
                    }

                    if (gridT[preInt, post] != 'o' &&
                        gridT[preInt, post] != 'x')
                    {
                        if (grid[preInt, post] == 'o')
                        {
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine("\nHIT!\n");
                            Console.Beep(1200, 500);
                            gridT[preInt, post] = 'o';
                            res = "Hit";
                            Console.ForegroundColor = ConsoleColor.Yellow;
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("\nMISS!\n");
                            Console.Beep(200, 500);
                            gridT[preInt, post] = 'x';
                            res = "Miss";
                            Console.ForegroundColor = ConsoleColor.Yellow;
                        }

                        Console.WriteLine("Press any key to continue");
                        Console.ReadKey();
                        
                        CheckGameFin(p);
                        Log(p, coord, res);
                        return true;
                    }
                    else
                    {
                        ErrCall(0, p);
                        return false;
                    }
                }
            }
            return false;
        }

        private void CheckGameFin(Player p)
        {
            if (GameOver(p))
            {
                if (p.ID == 0)
                {
                    this.p.First().TotalWins++;
                    this.p.Last().TotalLosses++;
                }
                else
                {
                    this.p.Last().TotalWins++;
                    this.p.First().TotalLosses++;
                }

                Console.WriteLine("Congrats {0}! You've won!",
                    p.Name());
                Console.WriteLine("Press \'y\' for a new game," +
                        " or \'n\' to finish.");
                char read = Console.ReadKey().KeyChar;
                if (read == 'y' || read == 'Y')
                {
                    NewGame();
                }
                else if (read == 'n' || read == 'N')
                {
                    Console.Clear();
                    Console.WriteLine("Good Game all!");

                    Console.WriteLine("Statisitics:");

                    foreach (Player pl in this.p)
                    {
                        Console.WriteLine("Player 1:{0}" +
                            "\n\tWins: {1}\n\tLosses: {2}" +
                            "\n\tRatio: {3}\n", pl.Name(),
                            pl.TotalWins, pl.TotalLosses,
                            Math.Round(pl.WinLossRatio()), 2);
                    }
                    Console.WriteLine("\n Press any key to return to menu");
                    Console.ReadKey();

                    Console.Clear();
                    Driver d = new Driver();
                    Console.ForegroundColor = ConsoleColor.Gray;
                    Console.WindowHeight = 22;
                    d.StartScreen();
                }
            }
        }

        private void Log(Player p, string coord, string res)
        {
            string line = p.Name() + ", " + coord.ToUpper() + ", " + res;

            GC.Collect();
            File.AppendAllText(path2, line + Environment.NewLine);
        }

         private bool Contained(string check, bool isNum)
         {
             if (!isNum)
             {
                 char cc = Convert.ToChar(check.ToUpper());
                 return c.Contains(cc);
             }
             else
             {
                 return char.IsNumber(check,0);
             }
         }

        private int CheckDim(char dim)
        {   //changes the alphabtical coord to a numerical
            // value
            string s = Convert.ToString(dim).ToUpper();
            int d1 = Encoding.Default.GetBytes(s)[0];

            int val = d1 - 65;
            return val;
        }

        private void ErrCall(int num, Player p)
        {   // All error calls are handled here
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.Write("ERROR: ");

            switch (num){
                case 0:
                    Console.WriteLine("Already Played, try another.");
                    Console.Beep(400, 500);
                    break;

                case 1:
                    Console.WriteLine("Out of Bounds, Must be between a-j " +
                        "and 1-10.");
                    Console.Beep(425, 500);
                    break;

                case 2:
                    Console.WriteLine("Invalid value; Please enter " +
                        "an alpha then numeric value.");
                    Console.Beep(450, 500);
                    break;

                case 3:
                    Console.WriteLine("Please Enter a value!");
                    Console.Beep(475, 500);
                    break;

                case 4:
                    Console.WriteLine("Unable to Import Empty Value");
                    Console.Beep(510, 500);
                    Environment.Exit(0);
                    break;

                case 5:
                    Console.WriteLine("Failed to Export Game, It doesn't like you!");
                    Console.Beep(520, 500);
                    break;

                case 6:
                    Console.WriteLine("Loop ended and no result... That's not possible!");
                    Console.Beep(550, 500);
                    break;

                default:
                    Console.WriteLine("Something unforseen has gone wrong! :O");
                    Console.Beep(500, 500);
                    break;
            }
            Console.ForegroundColor = ConsoleColor.Gray;
            Input(p);
        }

        public bool GameOver(Player p)
        {   // checks to see if the game is over
            if (p.ID == 0)
            {
                grid = gridTwo;
                gridT = gridOTrack;
            }
            else
            {
                grid = gridOne;
                gridT = gridTTrack;
            }

            int val=0;
            for (int val1 = 0; val1 < y; val1++)
            {
                for (int val2 = 0; val2 < x; val2++)
                {   // writes grid area at y and x location
                    if (grid[val1, val2] == 'o' && gridT[val1, val2] == 'o')
                    {
                        val++;
                    }
                }
            }

            if (val == 17)
            {
                gameOver = true;
                return true;
            }
            gameOver = false;
            return false;
        }

        public void NewGame()
        {   // creates a new game instance
            Console.Clear();
            Battleship Battle = new Battleship(p);
        }
    }
}