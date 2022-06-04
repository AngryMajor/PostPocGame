using System;
using System.Threading;
using System.Collections.Immutable;
using System.Collections.Generic;
using PostPocModel;
using PostPocModel.CardSystem;
using System.Linq;

namespace PostPocConsoleInterface
{
    class Program
    {

        private static bool GameRunning = true;
        private static Dictionary<string, Func<string[], string>> commands = new Dictionary<string, Func<string[], string>>();
        private static int intpuCount;

        private static CardControler cardControler;

        private static GameWorld world;
        private static GameContext currContext;
        private static ContextControler contextControler;

        static void Main(string[] args)
        {
            contextControler = new ContextControler();
            cardControler = new CardControler("", contextControler);

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
                String commandReturnString = commands[command[0]]?.Invoke(commandArgs);
                Console.Clear();
                Console.WriteLine(commandReturnString + "\n");
            }
            else
            {
                Console.Clear();
                Console.WriteLine("Comand Not Recognised: " + command[0] + "\n");
            }

            currContext = new GameContext(world);
            intpuCount++;
        }

        static void SubscribeToEvents()
        {
            ViewControler.InputPopupEvent += GetUserInput;
        }

        static void SetupGameData() {
            world = new GameWorld(new DefaultStettelemtnBuilder());
            currContext = new GameContext(world);
        }

        static void LoadInCommands() {
            commands.TryAdd("clear", delegate (string[] x) { Console.Clear(); return "cleared"; });
            commands.TryAdd("end", delegate (string[] x) { GameRunning = false; return ""; });
            commands.TryAdd("stop", delegate (string[] x) { GameRunning = false; return ""; });

            commands.TryAdd("test_getUserInput", delegate (string[] x) 
            {
                Dictionary<string, object> args = new Dictionary<string, object>();
                args.Add("Message", "test message");
                args.Add("InputField", "test field");
                ViewControler.GetPlayerInputAction action = new ViewControler.GetPlayerInputAction(args);

                action.GetDoable(currContext).Do(currContext);

                return (string)currContext.dict["test field"]; });


            cardControler.getCommands().ToList().ForEach(x => commands.TryAdd(x.Key, x.Value));

        }

        static void PrintGameData() {
            Console.WriteLine("Current Hand:");

            foreach (GameCard card in cardControler.Hand) {
                Console.WriteLine("     *:" + card.Name + ":*\n       " + card.Description);
            }

            Console.WriteLine("Command number: " + intpuCount);
        }

        static string[] GetCommand() {
            return Console.ReadLine().Trim().Split(" ");
        }

        static string GetUserInput(string message) {
            Console.WriteLine(message + ": ");
            return Console.ReadLine();
        }

    }

    public class ContextControler {

        protected GameWorld world;

        public GameContext CurrContext { get; private set; }

        public ContextControler(GameWorld world) {
            this.world = world;
            CurrContext = new GameContext(world);
        }

        public void resetContext() {
            CurrContext = new GameContext(world);

        }

    }

    public class CardControler {
        private ContextControler ContextC;

        private CardDeck<GameCard> deck = new CardDeck<GameCard>();
        private List<GameCard> _hand = new List<GameCard>();
        public IEnumerable<GameCard> Hand { get { return _hand; } }

        public CardDataBase<GameCard> cardDB;
        private ICardBuilder<GameCard> cardBuilder = new GameCard.Builder();

        public CardControler(string cardSourcePath, ContextControler contextC) {
            this.ContextC = contextC;
            cardDB = new CardDataBase<GameCard>(cardSourcePath, cardBuilder);
            foreach (GameCard card in cardDB.GetStartingDeck())
                deck.AddCard(card);
        }

        protected string DrawCard() {
            GameCard card = deck.DrawCard();
            if (card == null)
                return "deck empty";

            _hand.Add(card);
            return card.Name;
        }

        protected string PlayCard(int cardIndex, int activationIndex) {
            var activation = _hand[cardIndex].Activations(activationIndex).GetActivatable(ContextC.CurrContext);
            if (activation == null)
                return "could not play card";
            
            activation.Activate(ContextC.CurrContext);
            return "played";    
        }

        public Dictionary<string, Func<string[], string>> getCommands() {
            Dictionary<string, Func<string[],string>> commands = new Dictionary<string, Func<string[], string>>();
           
            commands.TryAdd("draw",  x => DrawCard() );
            commands.TryAdd("play", x => PlayCard(int.Parse(x[0]), int.Parse(x[1])));



            return commands;
        }

    }
}
