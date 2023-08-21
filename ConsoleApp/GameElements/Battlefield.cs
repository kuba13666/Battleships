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
            var currentShipFields = new List<Field>();
            while (currentShipFields.Count != shipSize)
            {
                Column startingColumn = (Column)random.Next(0, 10);
                Row startingRow = (Row)random.Next(0, 10);
                Direction direction = (Direction)random.Next(0, 4);
                currentShipFields = FindASpotForShip(shipSize, startingColumn, startingRow, direction);
            }
            AddShip(currentShipFields);
        }

        public List<Field> FindASpotForShip(int shipSize, Column startingColumn, Row startingRow, Direction direction)
        {
            var currentShipFields = new List<Field>();
            try
            {
                switch (direction)
                {
                    case Direction.Up:
                        for (int i = 0; i < shipSize; i++)
                        {
                            var field = Fields.FirstOrDefault(field => field.Equals(Field.Create(startingColumn, startingRow - i)));
                            if (field == null || field.IsOccupied)
                                break;
                            currentShipFields.Add(field);
                        }
                        break;
                    case Direction.Down:
                        for (int i = 0; i < shipSize; i++)
                        {
                            var field = Fields.FirstOrDefault(field => field.Equals(Field.Create(startingColumn, startingRow + i)));
                            if (field == null || field.IsOccupied)
                                break;
                            currentShipFields.Add(field);
                        }
                        break;
                    case Direction.Left:
                        for (int i = 0; i < shipSize; i++)
                        {
                            var field = Fields.FirstOrDefault(field => field.Equals(Field.Create(startingColumn - i, startingRow)));
                            if (field == null || field.IsOccupied)
                                break;
                            currentShipFields.Add(field);
                        }
                        break;
                    case Direction.Right:
                        for (int i = 0; i < shipSize; i++)
                        {
                            var field = Fields.FirstOrDefault(field => field.Equals(Field.Create(startingColumn + i, startingRow)));
                            if (field == null || field.IsOccupied)
                                break;
                            currentShipFields.Add(field);
                        }
                        break;
                    default:
                        break;
                }
                return currentShipFields;
            }
            catch(InvalidOperationException ex)
            {
                return new List<Field>();
            }
            
        }

        private void AddShip(List<Field> shipFields)
        {
            var fields = Fields.Where(field => shipFields.Contains(field)).ToList() ;
            _ships.Add(Ship.Create(fields));
            foreach (var field in fields)
            {
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
                    if (field.Missed)
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

            PrintBattlefield();
        }
    }
}