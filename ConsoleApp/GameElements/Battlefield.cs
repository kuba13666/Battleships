using ConsoleApp.Enums;
using System;
using System.Data.Common;
using System.Net.Http.Headers;

namespace ConsoleApp.GameElements
{
    public class Battlefield
    {
        public List<Field> Fields => _fields;
        private List<Field> _fields = new List<Field>();
        public List<Ship> Ships => _ships;
        private List<Ship> _ships = new List<Ship>();

        public Battlefield()
        {
        }
        public static Battlefield Create()
        {
            var battlefield = new Battlefield();
            battlefield.InitializeFields();
            battlefield.SpawnShipAtRandom(4);
            battlefield.SpawnShipAtRandom(4);
            battlefield.SpawnShipAtRandom(5);
            return battlefield;
        }

        public bool IsGameWon()
        {
            return Ships.All(ship => ship.HasSunk);
        }
        public void InitializeFields()
        {
            foreach (var column in Enum.GetValues(typeof(Column)))
            {
                foreach (var row in Enum.GetValues(typeof(Row)))
                {
                    _fields.Add(Field.Create((Column)column, (Row)row));
                }
            }
        }

        public void SpawnShipAtRandom(int shipSize)
        {
            var random = new Random();
            var currentShipCoordinates = new List<Coordinate>();
            while (currentShipCoordinates.Count != shipSize)
            {
                Column startingColumn = (Column)random.Next(0, 10);
                Row startingRow = (Row)random.Next(0, 10);
                Direction direction = (Direction)random.Next(0, 2);
                currentShipCoordinates = FindASpotForShip(shipSize, startingColumn, startingRow, direction);
            }
            AddShip(currentShipCoordinates);
        }

        public List<Coordinate> FindASpotForShip(int shipSize, Column startingColumn, Row startingRow, Direction direction)
        {
            var currentShipCoordinates = new List<Coordinate>();
            try
            {
                switch (direction)
                {
                    case Direction.Up:
                        for (int i = 0; i < shipSize; i++)
                        {
                            if (Fields.First(field => field.Equals(Field.Create(startingColumn, startingRow - i))).IsOccupied)
                                break;
                            currentShipCoordinates.Add(Coordinate.Create(startingColumn, startingRow - i));
                        }
                        break;
                    case Direction.Down:
                        for (int i = 0; i < shipSize; i++)
                        {
                            if (Fields.First(field => field.Equals(Field.Create(startingColumn, startingRow + i))).IsOccupied)
                                break;
                            currentShipCoordinates.Add(Coordinate.Create(startingColumn, startingRow + i));
                        }
                        break;
                    case Direction.Left:
                        for (int i = 0; i < shipSize; i++)
                        {
                            if (Fields.First(field => field.Equals(Field.Create(startingColumn - i, startingRow))).IsOccupied)
                                break;
                            currentShipCoordinates.Add(Coordinate.Create(startingColumn - i, startingRow));
                        }
                        break;
                    case Direction.Right:
                        for (int i = 0; i < shipSize; i++)
                        {
                            if (Fields.First(field => field.Equals(Field.Create(startingColumn + i, startingRow))).IsOccupied)
                                break;
                            currentShipCoordinates.Add(Coordinate.Create(startingColumn + i, startingRow));
                        }
                        break;
                    default:
                        break;
                }
                return currentShipCoordinates;
            }
            catch(InvalidOperationException ex)
            {
                return new List<Coordinate>();
            }
            
        }

        private void AddShip(List<Coordinate> destroyerCoordinates)
        {
            _ships.Add(Ship.Create(destroyerCoordinates));
            foreach (var coord in destroyerCoordinates)
            {
                var field = _fields.SingleOrDefault(field => field.Coordinate.Equals(coord));
                field.OccupyField();
            }
        }

        public void PrintBattlefield()
        {
            Console.Write(' ');
            foreach (var column in Enum.GetValues(typeof(Column)))
            {
                Console.Write(column);
            }
            Console.WriteLine();
            foreach (var row in Enum.GetValues(typeof(Row)))
            {
                Console.Write((int)row);
                foreach (var column in Enum.GetValues(typeof(Column)))
                {
                    var field = Fields.SingleOrDefault(x => x.Coordinate.Row == (Row)row && x.Coordinate.Column == (Column)column);
                    if (field.IsOccupied)
                    {
                        Console.Write('x');
                    }
                    else if (field.Missed)
                    {
                        Console.Write('o');
                    }
                    else if (field.Hit)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write('x');
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                    else
                    {
                        Console.Write(' ');
                    }
                }
                Console.WriteLine();
            }
        }

        public void PlayTheGame()
        {
            while (!IsGameWon())
            {
                Console.WriteLine("Point Column");
                var column = Console.ReadLine();
                while (!Enum.GetNames(typeof(Column)).Contains(column.ToUpper()))
                {
                    Console.WriteLine("Column outside of range");
                    Console.WriteLine("Point Column");
                    column = Console.ReadLine();
                }

                Console.WriteLine("Point Row");
                var row = Console.ReadLine();
                bool rowInRange = false;
                while (!int.TryParse(row, out int result) || !rowInRange)
                {
                    if (!int.TryParse(row, out int resultInLoop))
                    {
                        Console.WriteLine("row should be a number");
                        Console.WriteLine("Point Row");
                        row = Console.ReadLine();
                        continue;
                    }
                    var rowName = ((Row)resultInLoop).ToString();
                    if (!Enum.GetNames(typeof(Row)).Contains(rowName))
                    {
                        rowInRange = false;
                        Console.WriteLine("row outside of range");
                        Console.WriteLine("Point Row");
                        row = Console.ReadLine();
                        continue;
                    }
                    else
                    {
                        rowInRange = true;
                    }
                }
                var parsedRow = int.Parse(row);

                var coordinates = Coordinate.Create((Column)Enum.Parse(typeof(Column), column.ToUpper()), (Row)parsedRow);
                AttemptShot(coordinates);
            }
        }

        public void AttemptShot(Coordinate coord)
        {
            var fieldAttempted = Fields.First(field => field.Coordinate.Equals(coord));
            fieldAttempted.Shoot();

            var shipHit = Ships.FirstOrDefault(ship => ship.Presences.Contains(Presence.Create(coord)));
            if (shipHit != null)
                shipHit.Hit(coord);

            if (Ships.Any(x => x.HasSunk && x.Presences.Any(y => y.Coordinate.Equals(coord))))
            {
                var sinkedShip = Ships.First(ship => ship.HasSunk && ship.Presences.Any(presence => presence.Coordinate.Equals(coord)));
                //MarkAround(sinkedShip);
            }
            PrintBattlefield();
        }
    }
}