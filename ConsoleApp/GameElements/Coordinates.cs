using ConsoleApp.Enums;
using CSharpFunctionalExtensions;

namespace ConsoleApp.GameElements
{
    public class Coordinate : ValueObject<Coordinate>
    {
        public Column Column => _column;
        private Column _column { get; set; }
        public Row Row => _row;
        private Row _row { get; set; }
        private Coordinate(Column column, Row row)
        {
            _column = column;
            _row = row;
        }
        public static Coordinate Create(Column column, Row row)
        {
            return new Coordinate(column, row);
        }

        protected override bool EqualsCore(Coordinate other)
        {
            return Column == other.Column && Row == other.Row;
        }

        protected override int GetHashCodeCore()
        {
            return Column.GetHashCode() ^ Row.GetHashCode();
        }
    }
}
