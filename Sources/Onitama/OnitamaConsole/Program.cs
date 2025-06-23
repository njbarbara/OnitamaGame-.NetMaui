 using Onitama.Persistance.Stub;
using OnitamaLib.Events;
using OnitamaLib.Implementations;
using OnitamaLib.Managers;
using OnitamaLib.Models;
using OnitamaLib.PersistanceManagers;
using OnitamaPersistanceJson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json.Serialization.Metadata;
using System.Threading.Tasks;
using System.Collections;
using System.Collections.ObjectModel;
using OnitamaLib.Exceptions;



namespace OnitamaConsole
{
    public static class Program
    {
        public static int ReadCoordinate(int max)
        {
            int value;
            while (!int.TryParse(Console.ReadLine(), out value) || value < 0 || value > max)
                Console.Write("Valeur invalide, retentez : ");
            return value;
        }

        public static Position? ChooseRandomPawnPositionByColor(Board board, Color color, Random rand)
        {
            List<Position> pawnPositions = [];

            for (int x = 0; x < board.Width; x++)
            {
                for (int y = 0; y < board.Height; y++)
                {
                    Position pos = new(x, y);
                    if (board.DoIhavePawn(pos) && board.GetPawnAt(pos)?.Color == color)
                    {
                        pawnPositions.Add(pos);
                    }
                }
            }

            if (pawnPositions.Count == 0)
                return null;

            return pawnPositions[rand.Next(pawnPositions.Count)];
        }

        public static string DisplayPawn(Pawn? pawn)
        {
            if (pawn == null) return "   ";
            else if (pawn.IsSensei)
            {
                if (pawn.Color == Color.WHITE)
                    return " W ";
                else
                    return " B ";
            }
            else
            {
                if (pawn.Color == Color.WHITE)
                    return " w ";
                else
                    return " b ";
            }
        }

        public static string DisplayPawnOnCard(Pawn? pawn) => pawn == null ? "   " : " p ";

        private static string[,] CreateArrayBoard(Board board, string[,] board2display)
        {
            for (int i = 0; i < board.Width; i++)
            {
                for (int j = 0; j < board.Height; j++)
                    board2display[i, j] = DisplayPawn(board.GetPawnAt(i, j));
            }
            return board2display;
        }

        private static string[,] CreateApocalypseBoard(Board board, OnitamaMovement ApocalypseMovements, string[,] board2display)
        {
            board2display = CreateArrayBoard(board, board2display);
            foreach (Position? pos in ApocalypseMovements.Positions)
                board2display[pos.PositionX, pos.PositionY] = " # ";
            return board2display;
        }

        private static string[,] CreateArrayMovement(Board board, OnitamaMovement cardMovement, string[,] board2display)
        {
            foreach (Position pos in cardMovement.Positions)
            {
                if (board.DoIhavePawn(pos.PositionX, pos.PositionY)) board2display[pos.PositionX, pos.PositionY] = " x ";
                else board2display[pos.PositionX, pos.PositionY] = " + ";
            }
            return board2display;
        }

        private static string DisplayCard(OnitamaCard card)
        {
            Board board = new();
            board.PlacePawn(new Pawn(Color.WHITE), new Position(2, 2));
            OnitamaMovement moves = new ClassicBoardMoveManager().GetAvailableMove(card, board, new Position(2, 2));
            string[,] moveArray = new string[board.Width, board.Height];
            moveArray = CreateArrayMovement(board, moves, CreateArrayBoard(board, moveArray));
            return DisplayArray(moveArray);
        }

        private static string DisplayArray(string[,] array)
        {
            StringBuilder builder = new();
            int width = array.GetLength(0), height = array.GetLength(1);

            for (int i = 0; i < height; i++)
            {
                builder.Append($"    {i}");
            }
            builder.AppendLine();
            for (int j = 0; j < width; j++)
            {
                builder.Append($"{j} ");
                for (int i = 0; i < height; i++)
                {
                    builder.Append('|');
                    builder.Append(array[j, i]);
                    builder.Append('|');
                }
                builder.AppendLine();
            }

            return builder.ToString();
        }

        private static void DisplayMenu()
        {
            Console.Clear();
            Console.WriteLine("=============================================");
            Console.WriteLine("          BIENVENUE DANS ONITAMA            ");
            Console.WriteLine("=============================================");
            Console.WriteLine("   Un jeu de stratégie inspiré du Shogi      ");
            Console.WriteLine("---------------------------------------------");
            Console.WriteLine("Choisissez une option :");
            Console.WriteLine("  1. Démarrer une partie");
            Console.WriteLine("  2. Quitter");
            Console.WriteLine("---------------------------------------------");
            Console.Write("Votre choix (1-2) : ");
        }

        private static OnitamaCard GetChosenCard(string index, Game game)
        {
            if (index == "1")
            {
                return game.GetDeckCurrentPlayer().First();
            }
            else if (index == "2")
            {
                return game.GetDeckCurrentPlayer().Last();
            }
            throw new InvalidOperationException("Index invalide");
        }

        public static string ChooseNbPlayers()
        {
            string nbJoueurs = Console.ReadLine()?.Trim()?.ToLower() ?? "";
            while (nbJoueurs != "1" && nbJoueurs != "2")
            {
                Console.WriteLine("Entrée invalide. Entrez 1 ou 2 : ");
                nbJoueurs = Console.ReadLine()?.Trim()?.ToLower() ?? "";
            }
            return nbJoueurs;
        }

        public static string ChoosePlayer(string index)
        {
            Console.WriteLine("Nom du joueur  : " + index + " :");
            string p1name = Console.ReadLine()?.Trim()?.ToLower() ?? "";
            while (string.IsNullOrEmpty(p1name))
            {
                Console.WriteLine("Le nom ne peut pas être vide. Nom du joueur " + index + " :");
                p1name = Console.ReadLine()?.Trim()?.ToLower() ?? "";
            }
            return p1name;
        }

        public static string ChooseGameMod()
        {
            string stringGameMode = Console.ReadLine()?.Trim()?.ToLower() ?? "";
            while (stringGameMode != "classique" && stringGameMode != "apocalypse")
            {
                Console.WriteLine("Mode invalide. Entrez 'classique' ou 'apocalypse' : ");
                stringGameMode = Console.ReadLine()?.Trim()?.ToLower() ?? "";
            }
            return stringGameMode;
        }

        private static void SubscribeToEvents(GameManager gm, Game game, Board board, string[,] board2Display)
        {
            game.GameOver += (sender, args) =>
            {
                var opponent = game.Players.First(p => p != game.CurrentPlayer);
                Console.WriteLine("=== FIN DE PARTIE ===\nVictoire des " + opponent.Color + " par " + opponent.Name);
            };

            game.CardInStackChanged += (sender, args) =>
            {
                Console.WriteLine("\nCARTE DANS LA PILE : " + args.CardInStack.Name);
                Console.WriteLine(DisplayCard(args.CardInStack));
            };

            board.BoardChanged += (sender, args) =>
            {
                Console.WriteLine("=== TOUR DE " + game.CurrentPlayer.Name.ToUpper() + " (" + game.CurrentPlayer.Color + ") ===");
                if (game.Gamemode == GameMod.Apocalypse)
                {
                    board2Display = CreateApocalypseBoard(board, gm.MoveManager.GetForbiddenPositions(), board2Display);
                }
                else
                    board2Display = CreateArrayBoard(board, board2Display);
                Console.WriteLine("\nPLATEAU : ");
                Console.WriteLine(DisplayArray(board2Display));
            };


            gm.InvalidMove += (sender, args) =>
            {
                if (!args.ResMove)
                    Console.WriteLine("Mouvement invalide ! Destination non autorisée !");
            };
        }


        public static void PlayerTurn(ref string? cardChosen, ref OnitamaCard cardPlayed, ref Position positionChosen, ref Position destinationChosen, GameManager gm, Game game, Board board)
        {
            Console.WriteLine("Choisissez une carte (1 ou 2) : ");
            cardChosen = Console.ReadLine()?.Trim()?.ToLower();

            while (string.IsNullOrEmpty(cardChosen) || cardChosen != "1" && cardChosen != "2")
            {
                Console.WriteLine("Choix invalide. Choisissez 1 ou 2 : ");
                cardChosen = Console.ReadLine()?.Trim()?.ToLower() ?? "";
            }
            cardPlayed = GetChosenCard(cardChosen, game);

            bool validPawn = false;
            while (!validPawn)
            {
                Console.WriteLine("Colonne du pion : ");
                positionChosen.PositionY = ReadCoordinate(board.Width);
                Console.WriteLine("Ligne du pion : ");
                positionChosen.PositionX = ReadCoordinate(board.Height);
                Pawn? pawn = board.GetPawnAt(positionChosen);
                if (pawn != null && pawn.Color == game.CurrentPlayer.Color)
                    validPawn = true;
                else
                    Console.WriteLine("Pion invalide. Choisissez une couleur : " + game.CurrentPlayer.Color);
            }

            Console.WriteLine("Colonne de destination : ");
            destinationChosen.PositionY = ReadCoordinate(board.Width);
            Console.WriteLine("Ligne de destination : ");
            destinationChosen.PositionX = ReadCoordinate(board.Height);
            gm.PlayTurn(cardPlayed, positionChosen, destinationChosen);
        }

        public static void ConfigMenu(ref GameMod gamemode, ref string? nbJoueurs, ref string? p1name, ref string? p2name)
        {
            DisplayMenu();
            string? choice = Console.ReadLine()?.Trim();
            string stringGameMode;

            switch (choice)
            {
                case "1":
                    break;
                case "2":
                    Console.WriteLine("Merci d'avoir joué à Onitama ! À bientôt !");
                    return;
                default:
                    Console.WriteLine("Choix invalide. Appuyez sur une touche...");
                    Console.ReadKey();
                    return;
            }


            Console.Clear();
            Console.WriteLine("=== CONFIGURATION DE LA PARTIE ===");
            Console.WriteLine("Nombre de joueurs (1 ou 2) : ");
            nbJoueurs = ChooseNbPlayers();
            p1name = ChoosePlayer("1");

            if (nbJoueurs == "2")
            {
                p2name = ChoosePlayer("2");
            }

            Console.WriteLine("Mode de jeu (classique ou apocalypse) : ");
            stringGameMode = ChooseGameMod();
            gamemode = stringGameMode == "classique" ? GameMod.Classique : GameMod.Apocalypse;
        }

        public static void Main()
        {
            StubLoadManager loadManager = new();
            List<OnitamaCard> originalCards = loadManager.InitializeGameCards();
            List<OnitamaCard> listCards = [.. originalCards.OrderBy(x => Guid.NewGuid()).Take(5)];

            string? p1name = null, p2name = null, nbJoueurs = null, cardChosen = "";
            GameMod gamemode = GameMod.Classique;
            Position? positionChosen = new(), destinationChosen = new();
            OnitamaCard cardPlayed = new("NewCard", []);

            Game game;

            bool exit = false;

            ConfigMenu(ref gamemode, ref nbJoueurs, ref p1name, ref p2name);

            StubSaveManager saveManager = new();
            GameManager gm = new(gamemode, loadManager, saveManager);

            p1name ??= "Joueur 1";

            if (p2name == null)
            {
                game = gm.CreateGame(p1name, Color.WHITE, gamemode, listCards);
            }
            else
            {
                game = gm.CreateGame(p1name, Color.WHITE, p2name, Color.BLACK, gamemode, listCards);
            }
            

                Board? board = game.Board;
                string[,] board2Display = new string[board.Width, board.Height];

                Console.WriteLine("=== TOUR DE " + game.CurrentPlayer.Name.ToUpper() + " (" + game.CurrentPlayer.Color + ") ===");
                Console.WriteLine("\nPLATEAU :");
                board = game.Board;

                if (game.Gamemode == GameMod.Apocalypse)
                {
                    board2Display = CreateApocalypseBoard(board, gm.MoveManager.GetForbiddenPositions(), board2Display);
                }
                else
                    board2Display = CreateArrayBoard(board, board2Display);

                SubscribeToEvents(gm, game, board, board2Display);

                Console.WriteLine(DisplayArray(board2Display));
                Console.WriteLine("\nCARTE DANS LA PILE : " + game.CardInStack.Name);
                Console.WriteLine(DisplayCard(game.CardInStack));

                while (!exit)
                {


                    while (!game.IsOver)
                    {
                        Console.WriteLine("CARTE 1 : " + GetChosenCard("1", game).Name);
                        Console.WriteLine(DisplayCard(GetChosenCard("1", game)));
                        Console.WriteLine("CARTE 2 : " + GetChosenCard("2", game).Name);
                        Console.WriteLine(DisplayCard(GetChosenCard("2", game)));

                        if (!game.CurrentPlayer.IsBot)
                        {
                            PlayerTurn(ref cardChosen, ref cardPlayed, ref positionChosen, ref destinationChosen, gm, game, board);

                        }
                        else
                        {
                            Console.WriteLine("LE BOT JOUE...");

                            gm.PlayTurn();
                        }
                    }
                }
            }
        }
    }




