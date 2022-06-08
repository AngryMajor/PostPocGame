using System;
using System.Threading;
using System.Collections.Immutable;
using System.Collections.Generic;
using PostPocModel;
using System.Linq;
using PostPocModel.CardSystem;


namespace PostPocConsoleInterface
{
    class Program
    {

        private static bool GameRunning = true;
        private static Dictionary<string, Func<string[], string>> commands = new Dictionary<string, Func<string[], string>>();

        private static HandControler handControler = new HandControler();

        private static List<IControler> controlers = new List<IControler>();

        static void Main(string[] args)
        {
            controlers.Add(handControler);

            SetupGameData();
            SubscribeToEvents();
            LoadInCommands();

            while (GameRunning) {
                GameLoop();
            }

        }

        static void GameLoop() {
            PrintGameData();

            string[] command = GetCommand();
            if (commands.ContainsKey(command[0]))
            {
                string[] commandArgs = command.Skip(1).ToArray();

                String commandReturnString = "";
                try
                {
                    commandReturnString = commands[command[0]]?.Invoke(commandArgs);
                }
                catch(Exception e) {
                    commandReturnString = e.GetType().ToString() +" :: "+ e.Message;
                }
                Console.Clear();
                Console.WriteLine(commandReturnString + "\n");
            }
            else
            {
                Console.Clear();
                Console.WriteLine("Comand Not Recognised: " + command[0] + "\n");
            }
        }

        static void SubscribeToEvents()
        {
        }

        static void SetupGameData() {
        }

        static void LoadInCommands() {
            commands.TryAdd("clear", delegate (string[] x) { Console.Clear(); return "cleared"; });
            commands.TryAdd("end", delegate (string[] x) { GameRunning = false; return ""; });
            commands.TryAdd("stop", delegate (string[] x) { GameRunning = false; return ""; });
            commands.TryAdd("quit", delegate (string[] x) { GameRunning = false; return ""; });


            foreach (IControler controler in controlers)
                controler.getCommands().ToList().ForEach(x => commands.TryAdd(x.Key, x.Value));

        }

        public static void PrintGameData()
        {
            Console.WriteLine("Current Hand:");
            Console.WriteLine(handControler.GetHandString());

        }

        public static string[] GetCommand()
        {
            return Console.ReadLine().Trim().Split(" ");
        }

        public static string GetUserInput(string message)
        {
            Console.WriteLine(message + ": ");
            return Console.ReadLine();
        }

    }

    public interface IControler {
        public Dictionary<string, Func<string[], string>> getCommands();

    }

    public class HandControler : IControler
    {
        private const int StartingHandSize = 4;

        private Deck<PostPocCard>.Hand myHand;

        public HandControler() {
            List<PostPocCard> StartingCards = new List<PostPocCard>() {
                new PostPocCard("First","First",x=>true,true),
                new PostPocCard("Second","First",x=>true,true),
                new PostPocCard("Third","First",x=>false,true),
                new PostPocCard("Fourth","First",x=>true,true),
                new PostPocCard("Fifth","First",x=>true,true),
                new PostPocCard("Sixth","First",x=>true,true),
                new PostPocCard("Seventh","First",x=>true,true),
                new PostPocCard("Eighth","First",x=>true,true),
                new PostPocCard("Ninith","First",x=>true,true),
                new PostPocCard("tenith","First",x=>true,true)
            };

            Deck<PostPocCard> deck = new Deck<PostPocCard>(StartingCards,new Random().Next());
            myHand = deck.CreateHand(StartingHandSize);
        }

        public Dictionary<string, Func<string[], string>> getCommands()
        {
            Dictionary<string, Func<string[], string>> commands = new Dictionary<string, Func<string[], string>>();

            commands.TryAdd("draw", delegate(string[] x) { myHand.DrawCard(); return ""; });
            commands.TryAdd("play", delegate (string[] x) { myHand.TryPlay(int.Parse(x[0])); return "Played: " + x[0]; });



            return commands;
        }

        public string GetHandString() {
            string returnString = "";

            foreach(PostPocCard card in myHand.Cards)
            {
                returnString += card.ToString() + "\n";
            }

            return returnString;
        }

    }

}
