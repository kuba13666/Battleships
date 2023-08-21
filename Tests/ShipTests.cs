using ConsoleApp.Enums;
using ConsoleApp.GameElements;

namespace Tests
{
    public class ShipTests
    {

        [Fact]
        public void WhenEveryPresenceIsDamagedThenShipSinks()
        {
            var shipFields = new List<Field> { Field.Create(Column.A, Row.zeroed), Field.Create(Column.B, Row.zeroed) };
            var ship = Ship.Create(shipFields);
            foreach (var field in ship.FieldsOccupied)
            {
                field.Shoot();
            }

            Assert.True(ship.FieldsOccupied.All(field => field.Hit));
            Assert.True(ship.HasSunk);
        }
    }
}