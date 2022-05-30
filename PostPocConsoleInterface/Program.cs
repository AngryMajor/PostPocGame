﻿using System;
using System.Threading;
using System.Collections.Immutable;
using System.Collections.Generic;
using PostPocModel;
using System.Linq;

namespace PostPocConsoleInterface
{
    class Program
    {

        private static bool GameRunning = true;
        private static Dictionary<string, Func<string[], string>> commands = new Dictionary<string, Func<string[], string>>();
        private static int intpuCount;

        private static CardControler cardControler;

        private static GameDataLoader dataLoader = new HardCodeGameDataLoader();
        private static GameWorld world;
        private static GameContext currContext;

        static void Main(string[] args)
        {
            cardControler = new CardControler();

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

            cardControler.AddCards(dataLoader.GetGameCards());
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

    public class CardControler {

        private CardDeck<GameCard> deck = new CardDeck<GameCard>();
        private List<GameCard> _hand = new List<GameCard>();
        public IEnumerable<GameCard> Hand { get { return _hand; } }


        public CardControler() {

        }

        public void AddCards(IEnumerable<IGameCard> cardEnumerable) {
            foreach (IGameCard card in cardEnumerable)
                deck.AddCard((GameCard)card);
        }

        public Dictionary<string, Func<String[], string>> getCommands() {
            Dictionary<string, Func<String[],string>> commands = new Dictionary<string, Func<String[], string>>();
           
            commands.TryAdd("draw", delegate (string[] x) { _hand.Add(deck.DrawCard()); return _hand[0].ToString(); });


            return commands;
        }

    }
}
