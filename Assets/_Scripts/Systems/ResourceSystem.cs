using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ResourceSystem : Singleton<ResourceSystem>
{
    public List<CharacterScriptable> Characters { get; private set; }
    private Dictionary<CharacterType, CharacterScriptable> _charactersDict;

    public List<HexTileScriptable> Tiles { get; private set; }
    private Dictionary<TileType, HexTileScriptable> _tilesDict;

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

    }

    //We can probably move this out as well
    public CharacterScriptable GetCharacter(CharacterType type) => _charactersDict[type];
    public CharacterScriptable GetRandomCharacter() => _charactersDict[(CharacterType)Random.Range(0, Characters.Count)];


    public HexTileScriptable GetTile(TileType type) => _tilesDict[type];
    public HexTileScriptable GetRandomTile() => _tilesDict[(TileType)Random.Range(0, Tiles.Count)];

    //Expand
}
