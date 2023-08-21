using ConsoleApp.Enums;
using ConsoleApp.GameElements;

namespace Tests
{
    public class ShipTests
    {
        [Fact]
        public void WhenShipIsHitThenPresenceGetsDamaged()
        {
            var shipCoordinates = new List<Coordinate> { Coordinate.Create(Column.A, Row.zeroed), Coordinate.Create(Column.B, Row.zeroed) };
            var ship = Ship.Create(shipCoordinates);
            ship.Hit(shipCoordinates[0]);
            var presenceHit = ship.Presences.First(presence => presence.Equals(Presence.Create(shipCoordinates[0])));
            Assert.True(presenceHit.IsDamaged);
        }
        [Fact]
        public void WhenEveryPresenceIsDamagedThenShipSinks()
        {
            var shipCoordinates = new List<Coordinate> { Coordinate.Create(Column.A, Row.zeroed), Coordinate.Create(Column.B, Row.zeroed) };
            var ship = Ship.Create(shipCoordinates);
            ship.Hit(shipCoordinates[0]);
            ship.Hit(shipCoordinates[1]);

            Assert.True(ship.Presences.All(presence => presence.IsDamaged));
            Assert.True(ship.HasSunk);
        }
    }
}