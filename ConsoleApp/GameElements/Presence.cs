using CSharpFunctionalExtensions;

namespace ConsoleApp.GameElements
{
    public class Presence : ValueObject<Presence>
    {
        public Coordinate Coordinate => _coordinate;
        private Coordinate _coordinate { get; set; }
        public bool IsDamaged => _isDamaged;
        private bool _isDamaged { get; set; }
        private Presence(Coordinate coordinate)
        {
            _coordinate = coordinate;
            _isDamaged = false;
        }

        public static Presence Create(Coordinate coordinate)
        {
            return new Presence(coordinate);
        }
        protected override bool EqualsCore(Presence other)
        {
            return Coordinate.Equals(other._coordinate);
        }

        protected override int GetHashCodeCore()
        {
            throw new NotImplementedException();
        }

        public void DamageShip()
        {
            _isDamaged = true;
        }
    }
}
