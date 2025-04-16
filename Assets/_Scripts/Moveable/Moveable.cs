using System.Collections;
using UnityEngine;

public abstract class Moveable : MonoBehaviour, IHovereable, ISelectable
{
    protected Color _originalColor;
    protected SpriteRenderer _spriteRenderer;
    protected Vector3Int _currentPosition;

    void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _originalColor = _spriteRenderer.color;
    }

    [SerializeField]
    protected MoveableStats _moveableStats;

    public virtual void SetCurrentPosition(Vector3Int currentPosition)
    {
        _currentPosition = currentPosition;
    }

    public virtual void SetMoveableStats(MoveableStats moveableStats)
    {
        _moveableStats = moveableStats;
    }


    public virtual void Move(MoveArg moveArg)
    {
        if (moveArg.SOURCE == this)
        {
            StartCoroutine(MoveToPosition(moveArg));
            Debug.Log("Moved");
        }
    }

    public virtual IEnumerator MoveToPosition(MoveArg moveArg)
    {
        if(moveArg.SOURCE == this)
        {
            _currentPosition = moveArg.TARGET.GetCurrentGridPosition();
            yield return Helper.MoveToPosition(transform, GridManager.instance.GetWorldPositionToPlaceMoveableOnInGrid(moveArg.TARGET.GetCurrentGridPosition()), 0.5f);
        }
    }

    public virtual void Push(int direction, int distance)
    {
    }

    public virtual void Pull(int direction, int distance)
    {
    }

    public void CheckFall(BreakArg breakArg)
    {
        if(_currentPosition == breakArg.POSITION)
        {
            EventManager.instance.GE_Fall.Invoke(new FallArg(breakArg.SOURCE, this));
        }
    }

    public virtual void Fall(FallArg fallArg)
    {
        if (this == fallArg.FALLING)
        {
            FallAndDisable();
        }
    }

    public virtual IEnumerator FallAndDisable()
    {
        yield return StartCoroutine(Helper.MoveToPosition(transform, new Vector3(transform.position.x, -20, -10), 1f));
        gameObject.SetActive(false);
    }


    public void OnHoverLeave()
    {
        RestoreColor();
    }

    public void OnHoverEnter()
    {
        DarkenColor();
    }

    private void DarkenColor()
    {
        Color darkerColor = _originalColor * 0.5f; // Reduce brightness (50% darker)
        darkerColor.a = _originalColor.a; // Preserve original alpha
        _spriteRenderer.color = darkerColor;
    }

    private void RestoreColor()
    {
        if (_originalColor != null)
        {
            _spriteRenderer.color = _originalColor;
        }
    }

    public void OnSelect()
    {
    }

    public void OnDeselect()
    {
    }

    public void GetPushed(GameObject pushedBy, int tilesPushed)
    {
        HexTile currentTile = GridManager.instance.GetHexTileForGameObject(gameObject);
        HexTile pushedFromTile = GridManager.instance.GetHexTileForGameObject(gameObject);
        Vector3Int currentPos = currentTile.GetCurrentGridPosition();
        Vector3Int direction = Helper.GetTargetDirection(currentPos, pushedFromTile.GetCurrentGridPosition());
        for (int i = 0; i < tilesPushed; i++) 
        {
        }
    }

}
