using System.Text.RegularExpressions;
using System.Text;
using System.Globalization;
Console.OutputEncoding = Encoding.UTF8;

Random random = new Random();

int botCount = 3;

List<Player> listOfPlayers = new List<Player>(); //Is a list because the amount of players may vary
List<string> names = new List<string>() { "Josh", "Wiggo", "Bullen", "Hoffman", "Djivan <3", "Micke", "Ruben", "ostmannen", "Pedro", "Gringbert" };
//Is a list for the ability to remove used names so multiple bots don't get the same one


int whosTurn = 1; //Whos turn it is  1 = player1, 2 = player2 etc...
bool cardsShowed = false; //Just so it doesn't keep repeating the DisplayCards method
bool gameEnd = false;


string[] colors = { "Red", "Green", "Blue", "Yellow" };
string[] scenes = { "start", "game", "end" };
int currentScene = 0;


List<Card> playedCards = new List<Card>();


void selectBots()
{
    Console.WriteLine("Minimum: 1, Maximum: 3\n");
    string input = Console.ReadLine();

    if (int.TryParse(input, out int result))
    {
        if (result < 1 || result > 3)
        {
            Console.WriteLine("Amount not allowed, try again\n");
            selectBots();
        }
        else
        {
            botCount = result;

            for (int i = 1; i <= botCount + 1; i++)
            {
                Player player = new Player();

                int randomName = random.Next(0, names.Count() - 1);
                player.name = names[randomName];
                names.RemoveAt(randomName);
                listOfPlayers.Add(player);
            }
        }
    }
    else{
        Console.WriteLine("Not a number, try again");
        selectBots();
    }
}


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

    if (currentScene == 1)
    {
        if (player == listOfPlayers[0])
        {
            Console.WriteLine($"You drew a {colors[card.color]} {card.number}\n");
            player.numOfCards += 1;
            whosTurn += 1;
        }
    }
}


void DisplayCards(Player player)
{
    if (listOfPlayers[0].numOfCards == 1)
    {
        Console.WriteLine($"You have ONE card remaining!! Don't forget to type UNO before you play your card!\n");
    }
    else
    {
        Console.WriteLine($"Number of cards: {player.numOfCards}");
    }

    Console.WriteLine(""); //Line for the foreach loop to contine writing on
    foreach (Player bot in listOfPlayers)
    {
        if (bot != listOfPlayers[0]) //exclude the actual player
        {
            if (listOfPlayers.Count() > 2)
            {
                if (bot == listOfPlayers.Last())
                {
                    Console.Write($"{bot.name}: {bot.numOfCards}\n");
                }
                else
                {
                    Console.Write($"{bot.name}: {bot.numOfCards}, ");
                }
            }
            else
            {
                Console.Write($"{bot.name}: {bot.numOfCards}");
            }
        }
    }

    Console.WriteLine("Your cards:\n");

    for (int i = 0; i < player.hand.Count(); i++)
    {
        Console.WriteLine($"Card {i + 1} = {colors[player.hand[i].color]} {player.hand[i].number}\n");
    }

    Console.WriteLine($"Last played card: {colors[playedCards.Last().color]} {playedCards.Last().number}\n");
}


void PlayCard(Player player, Card card)
{
    if (whosTurn != 1)
    {
        foreach (Card botCard in player.hand)
        {
            if (botCard.color == playedCards.Last().color || botCard.number == playedCards.Last().number)
            {
                Console.WriteLine($"{player.name} played a {colors[botCard.color]} {botCard.number}\n");
                playedCards.Add(botCard);
                player.hand.Remove(botCard);
                player.numOfCards = player.hand.Count();
                break;
            }
            else
            {
                DrawCard(player);
                Console.WriteLine(player.name + " drew a card\n");
                player.numOfCards = player.hand.Count();
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


//Game loop
while (!gameEnd)
{
    if (currentScene == 0)
    {
        Console.WriteLine("---UNO---");
        Console.WriteLine("Press enter to start");
        Console.ReadLine();
        Console.WriteLine("Input the amount of bots to play against");
        selectBots();

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
                Console.WriteLine($"---Your Turn!--- \n");
                DisplayCards(listOfPlayers[0]);
                cardsShowed = true;
            }

            PlayOrDraw(listOfPlayers[0]);
            Console.WriteLine();
            cardsShowed = false;
        }
        else
        {
            Console.WriteLine($"{listOfPlayers[whosTurn - 1].name}'s Turn! \nLast played card: {colors[playedCards.Last().color]} {playedCards.Last().number}\n");
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
    public string name;
    public int numOfCards = 7;
    public bool uno = false;
    public List<Card> hand = new List<Card>();
}