namespace domineering_game.game
{
    public interface IPlayer
    {
        MoveType MoveType { get; set; }
        int Id { get; set; }

    }
    public enum MoveType
    {
        Horizontal = 0,
        Vertical = 1,
    }
}