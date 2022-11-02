namespace domineering_game.game
{
    public class Player : IPlayer
    {
        public MoveType MoveType {  get; set; }
        public int Id { get; set; }

        public Player(int id, MoveType moveType)
        {
            Id = id;
            MoveType = moveType;
        }
    }

}