public class MovedToArg
{
    public Moveable MOVEABLE_MOVED_HERE;
    public HexTile TARGET;
    public MovedToArg(Moveable moveableMovedHere, HexTile targetTile)
    {
        MOVEABLE_MOVED_HERE = moveableMovedHere;
        TARGET = targetTile;
    }
}