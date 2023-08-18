namespace ConsoleApp.GameElements
{
    public class Ship
    {
        public List<Presence> Presences => _presences;
        private List<Presence> _presences { get; set; }
        public bool HasSinked => _hasSinked;
        private bool _hasSinked { get; set; }
        private Ship(List<Coordinate> coordinates)
        {
            _presences = new List<Presence>();
            foreach (Coordinate coord in coordinates)
            {
                _presences.Add(Presence.Create(coord));
            }
        }
        public static Ship Create(List<Coordinate> coordinates)
        {
            return new Ship(coordinates);
        }
        public void Hit(Coordinate coord)
        {
            var presence = Presences.FirstOrDefault(presence => presence.Equals(Presence.Create(coord)));
            if (presence != null)
            {
                presence.DamageShip();
                if (Presences.All(presence => presence.IsDamaged))
                {
                    _hasSinked = true;
                }
            }
        }
    }
}
