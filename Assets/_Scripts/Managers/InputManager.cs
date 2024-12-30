

public class InputManager : Singleton<InputManager>
{
    //When multiplayer maybe this class needs to have non static stuff, dono
    public SelectState InputState { get; private set; }

    //UI och Game manager lyssnar på dessa. UI kommer troligtvis lyssna på OnTileClick
    public static Event<HexTile> OnTileClick = new Event<HexTile>();
    public static Event<HexTile> OnTileHover = new Event<HexTile>();
    public static Event<Unit> OnUnitClick = new Event<Unit>();
    public static Event<Unit> OnUnitHover = new Event<Unit>();


    //Either subsribe to everything
    //And have
    //tilehover
    //tileclick
    //unitclick
    //unithover
    //moveableclick
    //moveablehover
    //UIclick


    //OR 

    //Things simply call tileHover and
    //this checks if its valid and sends input through if it is?

}
