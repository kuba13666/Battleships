using ConsoleApp.GameElements;

var battlefield = Battlefield.Create();
battlefield.PrintBattlefield();

battlefield.PlayTheGame();
Console.WriteLine("You have won");


Console.ReadLine();

