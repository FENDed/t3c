using System;
using System.Text;
using System.Security.Cryptography;
using System.Linq;

namespace Task3
{
    class Program
    {
        static void Main(string[] args)
        {
            bool gameOver = false;
            string plMove;
            string aiMove;
            string gameKey = KeyGenerator();

            while (!gameOver)
            {
                if(args.Length % 2 == 0 || args.Length < 3 || !isCorrectMovesInsert(args))
                {
                    ErrorMessage();
                    string newArgs = Console.ReadLine();
                    args = newArgs.Split(' ');
                }else
                {
                    Console.WriteLine($"\nHMAC: {HmacGenerator(gameKey)}");
                    ShowMenu(args);
                    plMove = Console.ReadLine();
                    while (!gameOver)
                    {
                        
                        if (IsCorrectChoosen(args, plMove))
                        {
                            aiMove = Convert.ToString(AiChoice(args));
                            Console.WriteLine($"\nAI: {aiMove} PL: {plMove}");
                            AlertWinner(args, aiMove, plMove);
                            Console.WriteLine($"\nKEY: {gameKey}\n");
                            gameOver = true;
                        }
                        else
                        {
                            Console.Write("\nPlease, choose correct value: ");
                            plMove = Console.ReadLine();
                        }
                    }
                }
            }
        }

        public static void ErrorMessage()
        {
            Console.WriteLine("Arguments aren't correct. \n" +
                "Use at least 3 arguments. The number of arguments must be uneven \n" +
                "Example: \n" +
                "Rock Paper Scissors \n\n" +
                "Please, enter arguments(separated by space): \n");
        }

        public static void ShowMenu(string[] args)
        {
            Console.WriteLine("\n\n=========== CHOOSE UR MOVE ===========\n\n" +
                "Avaliable moves: \n");
            ShowMoves(args);
            Console.Write("0 - Exit\n" +
                "Ur move: ");
        }

        public static bool isCorrectMovesInsert(string[] args)
        {
            return args.Distinct().ToArray().Length == args.Length;
        }

        public static void ShowMoves(string[] args)
        {
            for(int i = 0; i < args.Length; i++) 
                Console.WriteLine($"{i + 1} - {args[i]}");
        }

        public static bool IsCorrectChoosen(string[] args, string choosenMove)
        {
            return Convert.ToInt32(choosenMove, 10) >= 1 && Convert.ToInt32(choosenMove, 10) <= args.Length;
        }

        public static int AiChoice(string[] args)
        {
            return new Random().Next(1, args.Length + 1);
        }

        public static void AlertWinner(string[] args, string aiMove, string plMove)
        {
            int ai = Convert.ToInt32(aiMove, 10);
            int pl = Convert.ToInt32(plMove, 10);
            bool result = (Math.Abs(ai - pl)) <= (args.Length / 2);
            if (aiMove == plMove)
                Console.WriteLine($"Draw.");
            else if (result)
                Console.WriteLine("Computer wins!");
            else
                Console.WriteLine("U win!");
            
        }

        private static string HmacGenerator(string key)
        {
            StringBuilder hmac = new StringBuilder();
            foreach (byte el in SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes(key.ToString())))
            {
                hmac.Append(el.ToString("x2"));
            }
            return hmac.ToString();
        }
        private static string KeyGenerator()
        {
            byte[] rand = new byte[16];
            RandomNumberGenerator.Create().GetBytes(rand);
            StringBuilder key = new StringBuilder();
            foreach (byte el in rand)
            {
                key.Append(el.ToString("x2"));
            }
            return key.ToString();
        }

    }
}
