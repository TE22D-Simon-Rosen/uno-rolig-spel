Random random = new Random();

int numOfCards = 7;
int whosTurn = 1; //Whos turn it is  1 = player1, 2 = player2 etc...

string[] scenes = { "start", "game", "end" };
int currentScene = 0;

string[] colors = { "Red", "Green", "Blue", "Yellow" };
List<Card> cards = new List<Card>();


void DrawCard()
{
    Card card = new Card();

    card.color = random.Next(1, 4);
    card.number = random.Next(0, 9);

    cards.Add(card);
}


void DisplayCards()
{
    for (int i = 0; i < cards.Count(); i++)
    {
        Console.WriteLine($"\ncard {i + 1} = {colors[cards[i].color]} {cards[i].number}");
    }
}


while (numOfCards > 0)
{
    if (currentScene == 0)
    {
        Console.WriteLine("---UNO---");
        Console.WriteLine("Press enter to start");
        Console.ReadLine();

        //Give 7 cards at start
        for (int i = 0; i < numOfCards; i++)
        {
            DrawCard();
        }

        currentScene = 1;
    }
    else if (currentScene == 1)
    {
        if (whosTurn == 1){
            DisplayCards();
        }
    }
}


public class Card
{
    public int number = 0;
    public int color = 0;
}