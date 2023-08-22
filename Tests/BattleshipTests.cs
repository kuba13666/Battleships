using ConsoleApp.Enums;
using ConsoleApp.GameElements;
using System.Data.Common;

namespace Tests
{
    public class BattleShipTests
    {
        [Fact]
        public void WhenBattlefieldIsCreatedThereIsABattlefieldWithHundreadFieldsAndThreeShips()
        {
            var battlefield = Battlefield.Create();
            Assert.True(battlefield.Fields.Count == 100);
            Assert.True(battlefield.Ships.Count == 3);
        }
        [Fact]
        public void WhenShotAttemptedAtOccupiedFieldThenShipGetsDamaged()
        {
            var battlefield = Battlefield.Create();
            var ship = battlefield.Ships.First();
            var fieldOccupied = ship.FieldsOccupied.First();
            battlefield.AttemptShot(fieldOccupied.Coordinate);
            Assert.True(fieldOccupied.Hit);
        }
        [Fact]
        public void WhenLastShipsPresenceGetsShotAtItSinksAndAllFieldsSurroundingAreMarked()
        {
            var battlefield = Battlefield.Create();
            var ship = battlefield.Ships.First();
            foreach (var fieldOccupied in ship.FieldsOccupied)
            {
                battlefield.AttemptShot(fieldOccupied.Coordinate);
                Assert.True(fieldOccupied.Hit);
            }
            Assert.True(ship.HasSunk);
        }

        [Fact]
        public void WhenSpanShipsAtRandomThereIsOneShipOfGivenSizeRandomlyPlaced()
        {
            var battlefield = new Battlefield();
            battlefield.InitializeFields();
            battlefield.SpawnShipAtRandom(4);
            Assert.True(battlefield.Ships.Count == 1);
        }

        [Fact]
        public void WhenTryToSpanShipOnOccupiedAreaShipWillNotGetPlaced()
        {
            var battlefield = new Battlefield();
            int shipSize = 4;
            battlefield.InitializeFields();
            battlefield.SpawnShipAtRandom(shipSize);
            var ship = battlefield.Ships.First();
            var fieldOccupied = ship.FieldsOccupied.First();
            var shipCoordinates = battlefield.FindASpotForShip(shipSize, fieldOccupied.Coordinate.Column, fieldOccupied.Coordinate.Row, Direction.Right);
            Assert.Empty(shipCoordinates);
            Assert.True(battlefield.Ships.Count == 1);
        }

        [Fact]
        public void WhenAllShipsHaveSinkedTheGameIsWon()
        {
            var battlefield = Battlefield.Create();
            foreach (var ship in battlefield.Ships)
            {
                foreach (var fieldOccupied in ship.FieldsOccupied)
                {
                    battlefield.AttemptShot(fieldOccupied.Coordinate);
                    Assert.True(fieldOccupied.Hit);
                }
            }
            Assert.True(battlefield.IsGameWon());
        }

        [Fact]
        public void GivenStartingPointAndDirectionItGivesYouNextFieldColumnAndRow()
        {
            var (currentColumn, currentRow) = Battlefield.GetRowAndColumn(Column.A, Row.zeroed, Direction.Down, 1);

            Assert.Equal(Column.A, currentColumn);
            Assert.Equal(Row.first, currentRow);
        }
    }
}