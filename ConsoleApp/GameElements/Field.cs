using ConsoleApp.Enums;
using CSharpFunctionalExtensions;
using System.Security.Cryptography.X509Certificates;

namespace ConsoleApp.GameElements
{
    public class Field : ValueObject<Field>
    {
        public Coordinate Coordinate => _coordinate;
        private Coordinate _coordinate { get; set; }
        public bool IsOccupied => _isOccupied;
        private bool _isOccupied { get; set; }
        public bool Missed => _missed;
        private bool _missed { get; set; }
        public bool Hit => _hit;
        private bool _hit { get; set; }
        public void OccupyField()
        {
            _isOccupied = true;
        }

        private Field(Coordinate coodrinate)
        {
            _coordinate = coodrinate;
            _isOccupied = false;
        }

        public static Field Create(Column column, Row row)
        {
            var coordinates = Coordinate.Create(column, row);
            return new Field(coordinates);
        }

        public void Shoot()
        {
            if (IsOccupied)
                _hit = true;
            else
                _missed = true;
        }
        public void MarkField()
        {
            _missed = true;
        }

        protected override bool EqualsCore(Field other)
        {
            return Coordinate.Equals(other._coordinate);
        }

        protected override int GetHashCodeCore()
        {
            throw new NotImplementedException();
        }
    }
}
