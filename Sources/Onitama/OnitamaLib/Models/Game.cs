using System;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Xml.Linq;
using OnitamaLib.Managers;
using System.Collections;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;
using OnitamaLib.Events;
using System.Runtime.Serialization;
[assembly : InternalsVisibleTo("OnitamaTests")]
namespace OnitamaLib.Models
{
    /// <summary>
    /// la classe game rassemble tout ce que le jeu a besoin pour représenter une partie d'Onitama
    /// </summary>
    [DataContract(Name ="Game")]
    public class Game : ObservableObject
    {
        /// <summary>La date et l'heure de création de la partie.</summary>
        /// 
        [DataMember(Order =1)]
        public  DateTime Date { get; private set; }
        /// <summary>Le mode de jeu (Classique ou Apocalypse) </summary>
        [DataMember(Order = 2)]
        public GameMod Gamemode { get; private set; }

        private int _scoreP1;
        private int _scoreP2;

        /// <summary>Le score du joueur 1 avec un setter privé pour éviter la triche et de se faire modifier en dehors</summary>
        [DataMember(Order = 3)]
        public int ScoreP1 {
            get => _scoreP1;
            private set { _scoreP1 = value; OnPropertyChanged(); }
        }
        /// <summary>Le score du joueur 2 avec un setter privé pour éviter la triche et de se faire modifier en dehors </summary>       
        [DataMember(Order = 4)]
        public int ScoreP2
        {
            get => _scoreP2;
            private set { _scoreP2 = value; OnPropertyChanged(); }
        }        

        /// <summary>booléen qui donne l'état de la partie true pour la partie finie et false pour la partie n'est pas fini </summary>
        /// 

        [DataMember(Order = 5)]
        public bool IsOver { get; private set; }
        /// <summary>représente le joueur qui est entrain de jouer (celui à qui c'est le tour) </summary>
        /// 

        [DataMember(Order = 6)]
        public  Player CurrentPlayer { get; private set; }
        /// <summary>représente le plateau de jeu</summary>
        /// 


        [DataMember(Order = 10)]
        public Board Board { get; internal set; }

        [DataMember(Order = 7)]
        private readonly Player[] _players;

        [DataMember(Order = 11)]
        public string P1Name { get; private set;  }

        [DataMember(Order = 12)]
        public string P2Name { get; private set; }

        [DataMember(Order = 13)]
        public string FileName { get; set; } = string.Empty;

        /// <summary>Liste des joueurs(2joueurs)  IEnumerable pour donner le moins de permission possible pour protéger le code contre les cast et la triche</summary>
        public IEnumerable<Player> Players => new ReadOnlyCollection<Player>(_players);


        [DataMember(Order = 8)]
        private readonly Dictionary<Player, List<OnitamaCard>> _decks;
        /// <summary>Liste de carte du joueur 1 en  IEnumerable pour donner le moins de permission possible pour protéger le code contre les cast et la triche</summary>    
        public IEnumerable<OnitamaCard> Player1 => new ReadOnlyCollection<OnitamaCard> (_decks[_players[0]]);


        /// <summary>Liste de carte du joueur 2 en  IEnumerable pour donner le moins de permission possible pour protéger le code contre les cast et la triche</summary>
        /// 
        public IEnumerable<OnitamaCard> Player2 => new ReadOnlyCollection<OnitamaCard>(_decks[_players[1]]);

        /// <summary>Carte dans la résèrve (la carte à coté du plateau)</summary>
        /// 


        [DataMember(Order =9)]
        public OnitamaCard CardInStack { get; private set; }

        /// <summary>événement déclenché lorsqu’un changement de tour a lieu</summary>
        public event EventHandler<TurnChangedEventArgs>? TurnChanged;

        /// <summary>Déclenche l’événement TurnChanged.</summary>
        /// <param name="args">Arguments du changement de tour.</param>
        protected virtual void OnTurnChanged(TurnChangedEventArgs args) => TurnChanged?.Invoke(this, args);

        /// <summary>événement déclenché lorsque la fin de partie est déclenché (quand la partie se termine )</summary>
        public event EventHandler<GameOverEventArgs>? GameOver;

        /// <summary>déclenche l’événement OnGameOver</summary>
        /// <param name="args">Arguments pour la fin de partie</param>
        protected virtual void OnGameOver(GameOverEventArgs args) => GameOver?.Invoke(this, args);

        public Player GetWinner()
        {
            if (!IsOver) throw new InvalidOperationException("La partie n'est pas terminé.");
            return (ScoreP1 > ScoreP2) ? _players[0] : _players[1];
        }

        public Player GetLoser() =>  _players[0] == GetWinner() ? _players[1]: _players[0];
        

        public event EventHandler<CardInStackChangedEventArgs>? CardInStackChanged;

        protected virtual void OnCardInStackChanged(CardInStackChangedEventArgs args) => CardInStackChanged?.Invoke(this, args);


        /// <summary>Ajoute une carte à un joueur.</summary>
        /// <param name="player">le joueur à qui ont veut ajouter la carte</param>
        /// <param name="card">La carte à ajouter.</param>
        /// <exception cref="ArgumentException">si le joueur n'existe pas</exception>
        public void AddCard2Player(Player player, OnitamaCard card)
        {
            if (!_decks.TryGetValue(player, out List<OnitamaCard>? value))
                throw new ArgumentException("Le joueur spécifié n'existe pas dans le dictionnaire.");
            value.Add(card);
        }

        /// <summary>retirer une carte à un joueur.</summary>
        /// <param name="player">le joueur à qui ont veut retirer la carte</param>
        /// <param name="card">La carte à retirer</param>
        /// <exception cref="ArgumentException">si le joueur n'existe pas</exception>
        public void RemoveCardFromPlayer(Player player, OnitamaCard card)
        {
            if (!_decks.TryGetValue(player, out List<OnitamaCard>? value))
                throw new ArgumentException("Le joueur spécifié n'existe pas dans le dictionnaire.");
            value.Remove(card);
        }

        /// <summary>Ajouter la carte dans la réserve(la carte à coté du plateau)</summary>
        /// <param name="card">La carte à mettre en résèrve (ca sera la carte que le joueur vient de jouer dans notre cas d'utilisation)</param>
        public void AddCardInStack(OnitamaCard card)
        {
            CardInStack = card;

            OnCardInStackChanged(new CardInStackChangedEventArgs(card));
        }

        /// <summary>constructeur d’une partie à deux joueurs
        /// elle va aussi attribué les cartes pour le joueur 1 et les cartes pour le jouer 2 et s'occuper de tout l'initialisation pour démarrer la partie
        /// </summary>
        /// <param name="player1">Joueur 1 </param>
        /// <param name="player2">Joueur 2</param>
        /// <param name="board"> le plateau</param>
        /// <param name="gamemode"> le mode de jeu (Apocalypse ou Classique)</param>
        /// <param name="gameCards">les cartes du jeu (celle pioché dans toutes celles disponible (20 cartes dispo et 5 choisi)</param>
        public Game(Player player1, Player player2, Board board, GameMod gamemode, List<OnitamaCard> gameCards)
        {
            _decks = [];
            _players = [player1, player2];
            CurrentPlayer = player1;
            Board = board;
            Date = DateTime.Now;
            Gamemode = gamemode;
            _scoreP1 = 0;
            _scoreP2 = 0;
            IsOver = false;
            CardInStack = gameCards[0];

            _decks[player1] = [gameCards[1], gameCards[2]];
            _decks[player2] = [gameCards[3], gameCards[4]];

            P1Name = player1.Name;
            P2Name = player2.Name;
        }

        /// <summary>constructeur d’une partie contre un Bot en se servant du constructeur principale de Game mais avec l'ajout du bot comme Joueur 2 </summary>
        /// <param name="player1">Joueur 1 </param>
        /// <param name="board"> le plateau</param>
        /// <param name="gamemode"> le mode de jeu (Apocalypse ou Classique)</param>
        /// <param name="gameCards">les cartes du jeu (celle pioché dans toutes celles disponible (20 cartes dispo et 5 choisi)</param>
        public Game(Player player1, Board board, GameMod gamemode, List<OnitamaCard> gameCards)
            : this(player1, new OnitamaBot(), board, gamemode, gameCards)
        { }

        public Game()
        {
            _players = new Player[2];
            _decks = new Dictionary<Player, List<OnitamaCard>>();
            Board = new Board();
            Date = DateTime.Now;
            Gamemode = GameMod.Classique;
            ScoreP1 = 0;
            ScoreP2 = 0;
            IsOver = false;
            CardInStack = new OnitamaCard("Rat",
            [
                new(-2, -1),
                new(-1, 2),
                new(1, 1),
                new(3, 0)
            ]);
            P1Name = string.Empty;
            P2Name = string.Empty;
            CurrentPlayer = _players[0];
        }



        /// <summary>retourne un joueur</summary>
        /// <param name="index">Index du joueur (0 ou 1) par rapport à la liste </param>
        /// <returns>Le joueur correspondant à l'index dans le tableau</returns>
        /// <exception cref="ArgumentOutOfRangeException">Si l’index est invalide</exception>
        public Player GetPlayer(int index) => (index == 0 || index == 1) ? _players[index] : throw new ArgumentOutOfRangeException(nameof(index));

        /// <summary>retourne un joueur en fonction de sa couleur</summary>
        /// <param name="color">la couleur du joueur</param>
        /// <returns>Le joueur avec la couleur spécifiée</returns>
        public Player GetPlayerByColor(Color color) => _players.First(p => p.Color == color);

        /// <summary>retourne le deck du joueur courant toujours en ienumerable pour donner le moins de permission possible pour protéger le code </summary>
        /// <returns>liste des cartes du joueur courant</returns>
        public IEnumerable<OnitamaCard> GetDeckCurrentPlayer() => (_decks[CurrentPlayer]);

        /// <summary>met à jour le score du joueur gagnant (elle compte le nombre de coups)</summary>
        /// <param name="winner">Joueur gagnant(il s'agit du CurrentPlayer après chaque coup le joueur voit son nombre de coups incrèmenté de 1 )</param>
        /// <exception cref="ArgumentException">Si le joueur n’est pas reconnu ou plutot pas trouvé</exception>
        public void UpdateScore(Player winner)
        {
            if (winner == _players[0])
                ScoreP1++;
            else if (winner == _players[1])
                ScoreP2++;
            else
                throw new ArgumentException("Joueur inconnu.");
        }

        /// <summary>retourne le score du joueur actuel(nb de coups)</summary>
        /// <returns>score du joueur courant(nb de coups)</returns>
        public int GetScoreCurrentPlayer() => (CurrentPlayer == _players[0]) ? ScoreP1 : ScoreP2;

        /// <summary>change le joueur qui joue (il change le CurrentPlayer) et déclenche l’événement TurnChanged.</summary>
        public void SwitchPlayer()
        {
            CurrentPlayer = (CurrentPlayer == _players[0]) ? _players[1] : _players[0];
            OnTurnChanged(new TurnChangedEventArgs(CurrentPlayer, GetScoreCurrentPlayer(), GetDeckCurrentPlayer()));
        }

        /// <summary>Change l'état de la partie est met l'état à True pour terminer la partie </summary>
        public void ChangeStateOfGame()
        {
            IsOver = true;
            OnGameOver(new GameOverEventArgs(this));
        }

        /// <summary>regarde si la carte est dans la main du joueur courant.</summary>
        /// <param name="card">carte à vérifier</param>
        /// <returns>Vrai si la carte est présente</returns>
        public bool IsCardInCurrentPlayerHand(OnitamaCard card) => (_decks[CurrentPlayer].Contains(card));

        
    }
}