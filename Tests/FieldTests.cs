using ConsoleApp.Enums;
using ConsoleApp.GameElements;

namespace Tests
{
    public class FieldTests
    {
        [Fact]
        public void WhenOccupiedFieldShotThanFieldMarkedAsHit()
        {
            var field = Field.Create(Column.A, Row.zeroed);
            field.OccupyField();
            field.Shoot();
            Assert.True(field.Hit);
            Assert.False(field.Missed);
        }
        [Fact]
        public void WhenUnoccupiedFieldShotThanFieldMarkedAsMissed()
        {
            var field = Field.Create(Column.A, Row.zeroed);
            field.Shoot();
            Assert.False(field.Hit);
            Assert.True(field.Missed);
        }
    }
}