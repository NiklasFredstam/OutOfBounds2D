using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ResourceSystem : Singleton<ResourceSystem>
{
    public List<CharacterScriptable> Characters { get; private set; }
    private Dictionary<CharacterType, CharacterScriptable> _charactersDict;

    public List<HexTileScriptable> Tiles { get; private set; }
    private Dictionary<TileType, HexTileScriptable> _tilesDict;

    public List<AbilityScriptable> Abilities { get; private set; }
    private Dictionary<AbilityType, AbilityScriptable> _abilitiesDict;

    public List<ItemScriptable> Items { get; private set; }
    private Dictionary<ItemType, ItemScriptable> _itemsDict;

    public List<MoveableScriptable> MoveableObjects { get; private set; }
    private Dictionary<MoveableObjectType, MoveableScriptable> _moveableObjects;

    protected override void Awake()
    {
        base.Awake();
        AssembleResources();
    }

    private void AssembleResources()
    {
        Characters = Resources.LoadAll<CharacterScriptable>("Characters").ToList();
        _charactersDict = Characters.ToDictionary(character => character.CharacterType, character => character);

        Tiles = Resources.LoadAll<HexTileScriptable>("Tiles").ToList();
        _tilesDict = Tiles.ToDictionary(tile => tile.TileType, tile => tile);

        Abilities = Resources.LoadAll<AbilityScriptable>("Abilities").ToList();
        _abilitiesDict = Abilities.ToDictionary(ability => ability.AbilityType, ability => ability);


        Items = Resources.LoadAll<ItemScriptable>("Items").ToList();
        _itemsDict = Items.ToDictionary(item => item.ItemType, item => item);

        MoveableObjects = Resources.LoadAll<MoveableScriptable>("Objects").ToList();
        _moveableObjects = MoveableObjects.ToDictionary(moveableObject => moveableObject.MoveableType, moveableObject => moveableObject);
    }

    public CharacterScriptable GetCharacter(CharacterType type) => _charactersDict[type];
    public CharacterScriptable GetRandomCharacter() => _charactersDict[(CharacterType)Random.Range(0, Characters.Count)];


    public HexTileScriptable GetTile(TileType type) => _tilesDict[type];
    public HexTileScriptable GetRandomTile() => _tilesDict[(TileType)Random.Range(0, Tiles.Count)];

    public AbilityScriptable GetAbility(AbilityType type) => _abilitiesDict[type];
    public AbilityScriptable GetRandomAbility() => _abilitiesDict[(AbilityType)Random.Range(0, Abilities.Count)];

    public ItemScriptable GetItem(ItemType type) => _itemsDict[type];
    public ItemScriptable GetRandomItem() => _itemsDict[(ItemType)Random.Range(0, Items.Count)];

    public MoveableScriptable GetMoveableObject(MoveableObjectType type) => _moveableObjects[type];
    public MoveableScriptable GetRandomMoveableObject() => _moveableObjects[(MoveableObjectType)Random.Range(0, MoveableObjects.Count)];
}
