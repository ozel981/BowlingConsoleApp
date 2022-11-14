// See https://aka.ms/new-console-template for more information
using Kregle;

Console.WriteLine("Hello, World!");
Game game = new Game();
while(true)
{
    var option = 0;
    Console.Write("Action [1.Add user|2.Start game|Else.End game]:");
    option = Console.Read();
    if (option == 1)
    {
        Console.Write("Nick name:");
        var nickName = Console.ReadLine();
        new User()
        {
            NickName = nickName ?? ""
        };
    }
    else if (option == 2)
    {
        while(true)
        {
            Console.WriteLine(game.ToString()); 
            var gameOption = 0;
            Console.Write($"{} [1.Add|2.Start game|Else.End game]:");
            gameOption = Console.Read();

        }
    }
    else
    {
        break;
    }
}