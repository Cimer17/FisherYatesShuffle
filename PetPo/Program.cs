using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Linq;

namespace PetPo
{
    /// <summary>
    /// Игрок
    /// </summary>
    public class Player
    {

        private List<string> _arm;

        /// <summary>
        /// Карты в руке игрока
        /// </summary>
        public List<string> Arm
        {
            get
            {
                return _arm;
            }
            set
            {
                _arm = value;
            }
        }

        public Player()
        {
            _arm = new List<string>();
        }

        /// <summary>
        /// Добавить картув рукук
        /// </summary>
        public void AddCard(string card)
        {
            Arm.Add(card);
        }

        /// <summary>
        /// Очистить руку
        /// </summary>
        public void Clear()
        {
            Arm.Clear();
        }

        /// <summary>
        /// Изображения карт
        /// </summary>
        public void CardImage()
        {
            Console.WriteLine(String.Concat(Enumerable.Repeat("* *** *  ", Arm.Count)));
            for (int i = 0; i < 4; i++)
            {
                if (i == 1)
                {
                    for (int j = 0; j < Arm.Count; j++)
                    {
                        if (_arm[j] == "10")
                        {
                            Console.Write($"* {Arm[j]}  *  ");
                        }
                        else
                        {
                            Console.Write($"*  {Arm[j]}  *  ");
                        }
                    }
                    Console.WriteLine();
                    continue;
                }
                Console.WriteLine(String.Concat(Enumerable.Repeat("*     *  ", Arm.Count)));
            }
            Console.WriteLine(String.Concat(Enumerable.Repeat("* *** *  ", Arm.Count)));
        }

        /// <summary>
        /// Подсчёт очков в руке
        /// </summary>
        public int CheckDeck()
        {
            int result = 0;
            int t = 0;
            foreach (string card in Arm)
            {
                if (card == "J" || card == "Q" || card == "K")
                {
                    result += 10;
                }
                else
                {
                    if (card == "A")
                    {
                        t++;
                        continue;
                    }
                    result += Convert.ToInt32(card);
                }
            }
            if (t > 0)
            {
                for (int j = 0; j < t; j++)
                {
                    if (result + 11 > 21)
                    {
                        result += 1;
                    }
                    else
                    {
                        result += 11;
                    }
                }
                
            }
            return result;
        }
    }

    /// <summary>
    /// Колода
    /// </summary>
    public class Deсs
    {
        private List<string> _deck;
        private int _countDeck;

        public List<string> Deck
        {
            get
            {
                return _deck;
            }
            set
            {
                _deck = value;
            }
        }

        public int CountDeck {

            get
            {
                return _countDeck;
            }
            set
            {
                if (value >= 1 && value <= 8)
                {
                    _countDeck = value;
                    GetDeck(_countDeck);
                }
                else
                {
                    throw new ArgumentOutOfRangeException(nameof(value), "Количество колод должно быть от 1 до 8.");
                }
            }
        }

        public Deсs(int count = 1)
        {
            Deck = new List<string>();
            CountDeck = count;
        }

        /// <summary>
        /// Механика получения карты
        /// </summary>
        public string GetCard()
        {
            CheckCountCard();
            string card = Deck[0];
            DeleteCard(1);
            return card;
        }

        /// <summary>
        /// Проверка количества в колоде
        /// </summary>
        public void CheckCountCard()
        {
            if (Deck.Count == 0)
            {
                Console.WriteLine("________________________________");
                Console.WriteLine("The deck is over! Mixed it up...");
                Console.WriteLine("________________________________");
                GetDeck(CountDeck);
            }
        }

        /// <summary>
        /// Механика генерации колоды
        /// </summary>
        /// <param name="countDeck">Колличество колод в игре</param>
        private List<string> GetDeck(int countDeck)
        {
            for (int i = 2; i <= 9; i++)
            {
                for (int j = 0; j < 4 * countDeck; j++)
                {
                    Deck.Add(i.ToString());
                }
            }
            for (int i = 0; i < 4 * countDeck; i++)
            {
                Deck.Add("10");
                Deck.Add("J");
                Deck.Add("Q");
                Deck.Add("K");
                Deck.Add("A");
            }
            return Shuffle();
        }

        /// <summary>
        /// Fisher-Yates shuffle
        /// </summary>
        private List<string> Shuffle()
        {
            int n = Deck.Count;
            
            // Представление стандартного алгоритма
            //Random rng = new Random();
            //while (n > 1)
            //{
            //    int k = rng.Next(n--);
            //    var temp = Deck[n];
            //    Deck[n] = Deck[k];
            //    Deck[k] = temp;
            //}

            using (var rng = RandomNumberGenerator.Create())
            {
                while (n > 1)
                {
                    byte[] randomNumber = new byte[4];
                    rng.GetBytes(randomNumber);
                    uint uintK = BitConverter.ToUInt32(randomNumber, 0);
                    int k = (int)(uintK % (uint)n);
                    var temp = Deck[n - 1];
                    Deck[n - 1] = Deck[k];
                    Deck[k] = temp;
                    n--;
                }
            }

            return Deck;
        }

        /// <summary>
        /// Удаление карт
        /// </summary>
        /// <param name="count">Колличество</param>
        private void DeleteCard(int count)
        {
            for (int i = 0; i < count; i++)
            {
                Deck.RemoveAt(i);
            }
        }
    }

    public class Program
    {
        public int GameStart = 0;
        public int WinGame = 0;
        public int LoseGame = 0;

        public void Game(Deсs deck, Player player, Player enemy)
        {
            for (int i = 0; i < 2; i++)
            {
                player.AddCard(deck.GetCard());
                enemy.AddCard(deck.GetCard());
            }

            while (true)
            {
                DisplayStatistics();
                DisplayPlayerInfo(player);
                if (player.CheckDeck() == 21)
                {
                    Console.Clear();
                    GameStart++;
                    WinGame++;
                    DisplayResult("|!EASY WIN!|", 21);
                    Console.WriteLine("________________________________");
                    Console.WriteLine("     21 at once!     ");
                    Console.WriteLine("________________________________");
                    player.Clear();
                    enemy.Clear();
                    break;
                }
                if (AskForMoreCards())
                {
                    player.AddCard(deck.GetCard());
                    int playerScore = player.CheckDeck();

                    if (playerScore == 21)
                    {
                        Console.Clear();
                        GameStart++;
                        WinGame++;
                        DisplayResult("|!EASY WIN!|", playerScore);
                        player.Clear();
                        enemy.Clear();
                        break;
                    }
                    else if (playerScore > 21)
                    {
                        Console.Clear();
                        GameStart++;
                        LoseGame++;
                        DisplayResult("You Lose!", playerScore);
                        player.Clear();
                        enemy.Clear();
                        break;
                    }
                    Console.Clear();
                }
                else
                {
                    int playerScore = player.CheckDeck();
                    int enemyScore = enemy.CheckDeck();
                    Console.Clear();
                    DetermineOutcome(playerScore, enemyScore);
                    player.Clear();
                    enemy.Clear();
                    break;
                }
            }
        }

        private void DisplayPlayerInfo(Player playerDeck)
        {
            Console.WriteLine("**********************");
            Console.WriteLine("      Live Game       ");
            Console.WriteLine("Your cards: ");
            playerDeck.CardImage();
            Console.WriteLine($"Your score: {playerDeck.CheckDeck()}");
            Console.WriteLine("More? 1 - Yes, 2 - No");
            Console.WriteLine("**********************");
        }

        private bool AskForMoreCards()
        {
            return Console.ReadLine() == "1";
        }

        private void DisplayResult(string message, int score)
        {
            Console.WriteLine("========+++++=========");
            Console.WriteLine("      Last Game       ");
            Console.WriteLine($"      {message}      ");
            Console.WriteLine($"Score: {score}");
            Console.WriteLine("========+++++=========");

        }

        private void DisplayStatistics()
        {
            Console.WriteLine("      Statistics      ");
            Console.WriteLine($"You games:   {GameStart}");
            Console.WriteLine($"You wins:    {WinGame}");
            Console.WriteLine($"You loses:   {LoseGame}");
            Console.WriteLine("========+++++=========");
        }

        private void DetermineOutcome(int playerScore, int enemyScore)
        {
            if (playerScore > enemyScore)
            {
                GameStart++;
                WinGame++;
                DisplayResult("You Win!", playerScore);
            }
            else
            {
                GameStart++;
                LoseGame++;
                DisplayResult("You Lose!", playerScore);
            }
        }

        static void Main(string[] args)
        {
            bool validInput = false;
            Program program = new Program();
            Deсs deck = null;
            
            Console.WriteLine("How many decks are there? (1 - 8)");
            while (!validInput)
            {
                string input = Console.ReadLine();
                if (int.TryParse(input, out int value))
                {
                    try
                    {
                        validInput = true;
                        deck = new Deсs(Convert.ToInt32(input));
                    }
                    catch (ArgumentOutOfRangeException ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
                else
                {
                    Console.WriteLine("Please enter number 1 - 8");
                }

            }

            Player player = new Player();
            Player enemy = new Player();
            Console.Clear();

            while (true)
            {
                program.Game(deck, player, enemy);
            }
        }
    }
}
