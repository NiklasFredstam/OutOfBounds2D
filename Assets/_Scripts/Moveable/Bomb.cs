using System.Collections.Generic;
using UnityEngine;

public class Bomb : Moveable
{
    [SerializeField]
    private int _timer = 2;

    private int _range = 1;

    private int _pushPower = 4;
    private string _name;

    protected override void Awake()
    {
        base.Awake();
    }

    public override void SetSourceBasedProperties(Unit unitSource)
    {
        _sourceUnit = unitSource;
        _pushPower = unitSource.GetPower();
    }


    public void DecrementTimer(GameState state)
    {
        if (state == GameState.GameTurnEnd) {
            if (_timer <= 1)
            {
                Explode();
            }
            else
            {
                _timer--;
            }
        }
    }

    private void Explode()
    {
        Debug.Log("Bomb explodes");
        _moveableStats.IsOnGrid = false;
        MoveableManager.instance.RemoveMoveable(this);

        List<Vector3Int> positionsAffected = GridManager.instance.GetPositionsWithinRangeExcludingCenter(_currentPosition, _range);

        List<Moveable> moveablesAffected = MoveableManager.instance.GetMoveablesAtPositions(positionsAffected);
        foreach (Moveable moveable in moveablesAffected)
        {

            HexDirection direction = GridManager.instance.GetDirectionTo(_currentPosition, moveable.GetCurrentPosition());

            EventManager.instance.GE_GetPushed.Invoke(new GetPushedArg(_sourceUnit, moveable, _pushPower, direction));
        }
        gameObject.SetActive(false);
    }
}