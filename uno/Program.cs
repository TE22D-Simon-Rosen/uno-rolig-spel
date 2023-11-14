using System.Text.RegularExpressions;
using System.Text;
using System.Globalization;
Console.OutputEncoding = Encoding.Unicode;

Random random = new Random();

int botCount = 3;

List<Player> listOfPlayers = new List<Player>(); //Is a list because the amount of players may vary
List<string> names = new List<string>() { "fotsvamp", "Wiggo", "Bullen", "Hoffman", "Djivan <3", "Micke", "spånga monstret", "ostmannen", "Pedro", "Gringbert" };
//Is a list for the ability to remove used names so multiple bots don't get the same one


int whosTurn = 1; //Whos turn it is  1 = player1, 2 = player2 etc...
bool cardsShowed = false; //Just so it doesn't keep repeating the DisplayCards method
bool gameEnd = false;


string[] colors = { "Red", "Green", "Blue", "Yellow" };
string[] scenes = { "start", "game", "end" };
int currentScene = 0;


List<Card> playedCards = new List<Card>();


void SelectBots()
{
    Console.WriteLine("Minimum: 1, Maximum: 3\n");
    string input = Console.ReadLine();

    if (int.TryParse(input, out int result))
    {
        if (result < 1 || result > 3)
        {
            Console.WriteLine("Amount not allowed, try again\n");
            SelectBots();
        }
        else
        {
            botCount = result;

            for (int i = 1; i <= botCount; i++)
            {
                Player player = new Player();

                int randomName = random.Next(0, names.Count() - 1);
                player.name = names[randomName];
                names.RemoveAt(randomName);
                listOfPlayers.Add(player);
            }
        }
    }
    else
    {
        Console.WriteLine("Not a number, try again");
        SelectBots();
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

    card.color = random.Next(0, 4);
    card.number = random.Next(0, 14); //0-9 = normal, 10 = reverse card, 11 = +2, 12 = +4, 13 = change color 

    player.hand.Add(card);



    if (card.number <= 9)
    {
        card.name = $"{colors[card.color]} {card.number}";
    }
    else if (card.number == 10)
    {
        card.name = $"{colors[card.color]} Reverse Card";
    }
    else if (card.number == 11)
    {
        card.name = $"{colors[card.color]} +2 Card";
    }
    else if (card.number == 12)
    {
        card.name = "+4 Card";
    }
    else
    {
        card.name = "Wild Card";
    }


    if (currentScene == 1)
    {
        if (player == listOfPlayers[0])
        {
            Console.WriteLine($"You drew a {card.name}\n");
            player.numOfCards += 1;
        }
        else
        {
            Console.WriteLine(player.name + " drew a card\n");
            player.numOfCards = player.hand.Count();
        }
    }
}


void DisplayCards(Player player)
{
    if (player.numOfCards == 2)
    {
        Console.WriteLine($"\nYou have TWO cards remaining!! Don't forget to type UNO before you play your card!\n");
    }
    else
    {
        Console.WriteLine($"\nNumber of cards: {player.numOfCards}");
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
        Console.WriteLine($"Card {i + 1} = {player.hand[i].name}");
    }

    Console.WriteLine($"Last played card: {playedCards.Last().name}");

}


void PlayCard(Player player, Card card)
{
    if (whosTurn != 1)
    {
        foreach (Card botCard in player.hand)
        {
            if (botCard.color == playedCards.Last().color || botCard.number == playedCards.Last().number)
            {
                if (player.numOfCards == 2)
                {
                    if (random.Next(1, 11) > 3)
                    { //70% chance for bot to call UNO
                        Console.WriteLine($"{player.name} called UNO!");
                        Console.WriteLine($"{player.name} played a {botCard.name}\n");
                        playedCards.Add(botCard);
                        player.hand.Remove(botCard);
                        player.numOfCards = player.hand.Count();
                        player.uno = true;
                        break;
                    }
                    else
                    { //If it "forgets" to call UNO
                        Console.WriteLine($"{player.name} forgot to call UNO!\n");
                        for (int i = 0; i < 3; i++)
                        {
                            DrawCard(player);
                        }
                    }
                }
                else
                {
                    Console.WriteLine($"{player.name} played a {botCard.name}\n");
                    playedCards.Add(botCard);
                    player.hand.Remove(botCard);
                    player.numOfCards = player.hand.Count();
                    break;
                }
            }
            else
            {
                DrawCard(player);
                player.uno = false;
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
        Console.WriteLine($"You played a {card.name}\n");
        playedCards.Add(card);
        player.hand.Remove(card);
        player.numOfCards = player.hand.Count();
        whosTurn += 1;
    }
}


void PlayOrDraw(Player player)
{
    Console.WriteLine("\nPlay a card by typing the corresponding number, draw a card by typing \"Draw\"\n");
    string input = null;

    input = Console.ReadLine().Trim().ToUpper();
    if (input == "DRAW")
    {
        DrawCard(listOfPlayers[0]);
        whosTurn += 1;
        player.uno = false;
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
            //Checks if the selected card is the same color or number as the card at the top of the pile
            if (player.hand[result - 1].color == playedCards.Last().color || player.hand[result - 1].number == playedCards.Last().number)
            {
                if (player.numOfCards == 2 && !player.uno) //Gives the player 3 cards if they forget to call uno when they have to
                {
                    Console.WriteLine("You forgot to call UNO! +3 cards");
                    for (int i = 0; i < 3; i++)
                    {
                        DrawCard(player);
                        Console.ReadLine();
                    }
                }

                PlayCard(listOfPlayers[0], listOfPlayers[0].hand[result - 1]);
            }
            else
            {
                Console.WriteLine("Cannot play that card, try again with a different number or draw a card \n");
                PlayOrDraw(player);
            }
        }
    }
    else if (input == "UNO")
    {
        if (player.numOfCards == 2)
        {
            player.uno = true;
            Console.WriteLine("You called UNO!\n");
            PlayOrDraw(player);
        }
        else
        {
            Console.WriteLine("You need to have 2 cards to be able to call UNO! \n");
            PlayOrDraw(player);
        }
    }
    else
    {
        PlayOrDraw(player);
    }
}


//Game loop
while (!gameEnd)
{
    if (currentScene == 0)
    {
        Player player = new Player();
        listOfPlayers.Add(player);

        Console.WriteLine("---UNO---");
        Console.WriteLine("Press enter to start");
        Console.ReadLine();
        Console.WriteLine("Input a name");
        listOfPlayers[0].name = Console.ReadLine().Trim();

        Console.WriteLine("Input the amount of bots to play against");
        SelectBots();

        for (int i = 0; i < listOfPlayers.Count(); i++)
        {
            ResetHand(listOfPlayers[i]);
        }

        //Automatically play a random card at start
        Card card = new Card();
        card.color = random.Next(0, 4);
        card.number = random.Next(0, 10);
        playedCards.Add(card);

        card.name = colors[card.color] + " " + card.number;

        currentScene = 1;
    }
    else if (currentScene == 1)
    {
        if (whosTurn == 1)
        {
            if (!cardsShowed)
            {
                Console.WriteLine($"\n---Your Turn!--- \n");
                DisplayCards(listOfPlayers[0]);
                cardsShowed = true;
            }

            PlayOrDraw(listOfPlayers[0]);
            Console.WriteLine();
            cardsShowed = false;
        }
        else
        {
            Console.WriteLine($"\n{listOfPlayers[whosTurn - 1].name}'s Turn! \nLast played card: {playedCards.Last().name}");
            PlayCard(listOfPlayers[whosTurn - 1], null);
            Console.ReadLine();
        }

        foreach (Player player in listOfPlayers)
        {
            if (player.numOfCards == 0)
            {
                currentScene = 2;
            }
        }
    }
    else
    {
        var sortedList = listOfPlayers.OrderBy(player => player.numOfCards).ToList();

        foreach (Player player in sortedList)
        {
            if (player == sortedList[0])
            {
                Console.WriteLine(listOfPlayers[0].name + " won the match! Congratulations!\n");
            }
            else if (player == sortedList[1])
            {
                Console.WriteLine($"{sortedList[1].name} came 2nd with {sortedList[1].numOfCards} cards\n");
            }
            else if (player == sortedList[2])
            {
                Console.WriteLine($"{sortedList[2].name} came 3rd with {sortedList[2].numOfCards} cards\n");
            }
            else
            {
                Console.WriteLine($"{sortedList[3].name} came last with {sortedList[3].numOfCards} cards\n");
            }
        }

        Console.ReadLine();
    }
}


public class Card
{
    public int number = 0;
    public int color = 0;
    public string name;
}


public class Player
{
    public string name;
    public int numOfCards = 7;
    public bool uno = false;
    public List<Card> hand = new List<Card>();
}