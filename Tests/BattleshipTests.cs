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
            var presence =ship.Presences.First();
            battlefield.AttemptShot(presence.Coordinate);
            Assert.True(presence.IsDamaged);
        }
        [Fact]
        public void WhenLastShipsPresenceGetsShotAtItSinksAndAllFieldsSurroundingAreMarked()
        {
            var battlefield = Battlefield.Create();
            var ship = battlefield.Ships.First();
            foreach (var presence in ship.Presences)
            {
                battlefield.AttemptShot(presence.Coordinate);
                Assert.True(presence.IsDamaged);
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
            var presence = ship.Presences.First();
            var shipCoordinates = battlefield.FindASpotForShip(shipSize, presence.Coordinate.Column, presence.Coordinate.Row, Direction.Right);
            Assert.Empty(shipCoordinates);
            Assert.True(battlefield.Ships.Count == 1);
        }

        [Fact]
        public void WhenAllShipsHaveSinkedTheGameIsWon()
        {
            var battlefield = Battlefield.Create();
            foreach (var ship in battlefield.Ships)
            {
                foreach (var presence in ship.Presences)
                {
                    battlefield.AttemptShot(presence.Coordinate);
                    Assert.True(presence.IsDamaged);
                }
            }
            Assert.True(battlefield.IsGameWon());
        }
    }
}