using System.Text.RegularExpressions;
using System.Text;
using System.Globalization;
Console.OutputEncoding = Encoding.UTF8;

Random random = new Random();

int botCount = 3;

List<Player> listOfPlayers = new List<Player>();

for (int i = 1; i <= botCount + 1; i++)
{
    Player player = new Player();
    listOfPlayers.Add(player);
}

int whosTurn = 1; //Whos turn it is  1 = player1, 2 = player2 etc...
bool cardsShowed = false; //Just so it doesn't keep repeating the DisplayCards method
bool gameEnd = false;

string[] scenes = { "start", "game", "end" };
int currentScene = 0;

string[] colors = { "Red", "Green", "Blue", "Yellow" };

List<Card> playedCards = new List<Card>();


void ResetHand(Player player)
{
    //Give 7 cards at start
    for (int i = 0; i < player.numOfCards; i++)
    {
        DrawCard(player);
    }
}


void DrawCard(Player player)
{
    Card card = new Card();

    card.color = random.Next(1, 4);
    card.number = random.Next(0, 9);

    player.hand.Add(card);

    if (player == listOfPlayers[0])
    {
        Console.WriteLine($"You drew a {colors[card.color]} {card.number}\n");
        whosTurn += 1;
    }
}


void DisplayCards(Player player)
{
    Console.WriteLine($"Number of cards: {player.numOfCards} \nP2: {listOfPlayers[1].numOfCards}, P3: {listOfPlayers[2].numOfCards}, P4: {listOfPlayers[3].numOfCards}\n");
    Console.WriteLine("Your cards:\n");
    for (int i = 0; i < player.hand.Count(); i++)
    {
        Console.WriteLine($"card {i + 1} = {colors[player.hand[i].color]} {player.hand[i].number}\n");
    }
}


void PlayCard(Player player, Card card)
{
    if (whosTurn != 1)
    {
        foreach (Card botCard in player.hand)
        {
            if (botCard.color == playedCards.Last().color || botCard.number == playedCards.Last().number)
            {
                Console.WriteLine($"{player} played a {colors[botCard.color]} {botCard.number}\n");
                playedCards.Add(botCard);
                player.hand.Remove(botCard);
                player.numOfCards = player.hand.Count();
                break;
            }
            else
            {
                DrawCard(player);
                Console.WriteLine(player + " drew a card\n");
                break;
            }
        }

        if (whosTurn == listOfPlayers.Count())
        {
            whosTurn = 1;
        }
        else
        {
            whosTurn += 1;
        }
    }
    else
    {
        Console.WriteLine($"You played a {colors[card.color]} {card.number}");
        playedCards.Add(card);
        player.hand.Remove(card);
        player.numOfCards = player.hand.Count();
        whosTurn += 1;
    }
}


void PlayOrDraw(Player player)
{
    Console.WriteLine("Play a card by typing the corresponding number, draw a card by typing \"Draw\"\n");
    string input = null;

    input = Console.ReadLine().Trim().ToUpper();
    if (input == "DRAW")
    {
        DrawCard(listOfPlayers[0]);
    }
    else if (int.TryParse(input, out int result))
    {
        if (result < 1 || result > player.numOfCards)
        {
            Console.WriteLine("Card does not exist, try again with a different number\n");
            PlayOrDraw(listOfPlayers[0]);
        }
        else
        {
            if (player.hand[result - 1].color == playedCards.Last().color || player.hand[result - 1].number == playedCards.Last().number)
            {
                PlayCard(listOfPlayers[0], listOfPlayers[0].hand[result - 1]);
            }
            else
            {
                Console.WriteLine("Cannot play that card, try again with a different number or draw a card");
                PlayOrDraw(player);
            }
        }
    }
    else { PlayOrDraw(player); }
}

while (!gameEnd)
{
    if (currentScene == 0)
    {
        Console.WriteLine("---UNO---");
        Console.WriteLine("Press enter to start");
        Console.ReadLine();

        for (int i = 0; i < listOfPlayers.Count(); i++)
        {
            ResetHand(listOfPlayers[i]);
        }

        //Automatically play a random card at start
        Card card = new Card();
        card.color = random.Next(1, 4);
        card.number = random.Next(0, 9);
        playedCards.Add(card);

        currentScene = 1;
    }
    else if (currentScene == 1)
    {
        if (whosTurn == 1)
        {
            if (!cardsShowed)
            {
                Console.WriteLine($"Your Turn! \nLast played card: {colors[playedCards.Last().color]} {playedCards.Last().number}\n");
                DisplayCards(listOfPlayers[0]);
                cardsShowed = true;
            }

            PlayOrDraw(listOfPlayers[0]);
            Console.WriteLine();
            cardsShowed = false;
        }
        else
        {
            Console.WriteLine($"Player {whosTurn}'s Turn! \nLast played card: {colors[playedCards.Last().color]} {playedCards.Last().number}\n");
            Console.ReadLine();
            PlayCard(listOfPlayers[whosTurn - 1], null);
            Console.ReadLine();
        }
    }
}


public class Card
{
    public int number = 0;
    public int color = 0;
}


public class Player
{
    public int numOfCards = 7;
    public bool uno = false;
    public List<Card> hand = new List<Card>();
}