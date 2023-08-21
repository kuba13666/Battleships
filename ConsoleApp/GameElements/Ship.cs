namespace ConsoleApp.GameElements
{
    public class Ship
    {
        public List<Field> FieldsOccupied => _fieldsOccupied;
        private List<Field> _fieldsOccupied { get; set; }
        public bool HasSunk => FieldsOccupied.All(field => field.Hit);
        private Ship(List<Field> fields)
        {
            _fieldsOccupied = fields;
            foreach (var field in FieldsOccupied)
            {
                field.OccupyField();
            }
        }
        public static Ship Create(List<Field> fieldsOccupied)
        {
            return new Ship(fieldsOccupied);
        }

    }
}
