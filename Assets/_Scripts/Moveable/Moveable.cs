using Microsoft.Unity.VisualStudio.Editor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Moveable : MonoBehaviour, IInspectable
{
    protected Color _originalColor;
    protected SpriteRenderer _spriteRenderer;
    [SerializeField]
    protected Vector3Int _currentPosition;

    [SerializeField]
    protected MoveableStats _moveableStats;

    protected string _description;
    protected SelectDarker _selectDarker;
    protected BlinkDarker _blinkDarker;
    protected HoverDarker _hoverDarker;
    protected Unit _sourceUnit;

    protected virtual void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _originalColor = _spriteRenderer.color;
        _selectDarker = GetComponent<SelectDarker>();
        _hoverDarker = GetComponent<HoverDarker>();
        _blinkDarker = GetComponent<BlinkDarker>(); 
    }

    public virtual void SetSourceBasedProperties(Unit unitSource)
    {
        _sourceUnit = unitSource;
    }

    public bool IsOnGrid()
    {
        return _moveableStats.IsOnGrid;
    }

    public virtual void SetCurrentPosition(Vector3Int currentPosition)
    {
        _currentPosition = currentPosition;
    }

    public virtual Vector3Int GetCurrentPosition()
    {
        return _currentPosition;
    }

    public virtual void SetMoveableStats(MoveableStats moveableStats)
    {
        _moveableStats = moveableStats;
    }

    public int GetWeight() {
        return _moveableStats.Weight;
    }


    public virtual void Move(MoveArg moveArg)
    {
        if (moveArg.TARGET == this && moveArg.TO_POSITION != null)
        {
            _currentPosition = moveArg.TO_POSITION;

            if (moveArg.IS_PUSH)
            {
                AnimationQueue.instance.EnqueueAnimation(MoveToPosition(moveArg.TO_POSITION));
            }
            else
            {
                AnimationQueue.instance.EnqueueAnimation(MoveToPosition(moveArg.TO_POSITION));
            }
        }
    }

    private IEnumerator MoveToPosition(Vector3Int newPosition)
    {
        Vector3 worldPos = GridManager.instance.GetWorldPositionToPlaceMoveableOnInGrid(newPosition);
        yield return AnimateMove(transform, worldPos, 0.2f);
    }

    private IEnumerator PushedAlongPath(List<Vector3Int> path)
    {
        int totalSteps = path.Count;
        for (int i = 0; i < totalSteps; i++)
        {
            _currentPosition = path[i];
            Vector3 worldPos = GridManager.instance.GetWorldPositionToPlaceMoveableOnInGrid(path[i]);

            // Optional: execute other per-tile logic here

            float t = (float)i / totalSteps;
            float curve = 1f - Mathf.Pow(1f - t, 4f); // easing out (you can tweak)
            float moveDuration = Mathf.Lerp(0.1f, 1.2f, curve); // moves start fast, end slower

            yield return AnimateMove(transform, worldPos, moveDuration);
        }
    }

    public void CheckFall(BreakArg breakArg)
    {
        if(breakArg.POSITION == _currentPosition)
        {
            EventManager.instance.GE_Fall.Invoke(new FallArg(breakArg.SOURCE, this));
        }
    }

    public void CheckFall(MoveArg moveArg)
    {
        if (!GridManager.instance.HexTileExistsAtPosition(_currentPosition))
        {
            EventManager.instance.GE_Fall.Invoke(new FallArg(moveArg.SOURCE, this));
        }
    }

    public virtual void Fall(FallArg fallArg)
    {
        if (fallArg.FALLING == this && _moveableStats.IsOnGrid)
        {
            _moveableStats.IsOnGrid = false;
            Debug.Log(gameObject.name + " fell out of bounds.");
            MoveableManager.instance.RemoveMoveable(this);
            AnimationQueue.instance.EnqueueAnimation(AnimateFall(transform, new Vector3(transform.position.x, transform.position.y - 10, 10), 2f));
        }
    }

    protected IEnumerator AnimateMove(Transform transformToMove, Vector3 targetPosition, float duration)
    {
        Vector3 startPosition = transformToMove.position;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;

            t = Mathf.SmoothStep(0, 1, t);

            transformToMove.position = Vector3.Lerp(startPosition, targetPosition, t);
            yield return null;
        }

        transformToMove.position = targetPosition;
    }



    protected IEnumerator AnimateFall(Transform transformToMove, Vector3 targetPosition, float duration)
    {
        Vector3 startPosition = transformToMove.position;
        float elapsedTime = 0f;
        int sortingPrio = Helper.GetSortingPriorityFromHexPosition(_currentPosition);
        _spriteRenderer.sortingOrder = sortingPrio;
        //We set this so that the object is behind the row in front of it and in front of the row behind it!
        _spriteRenderer.sortingLayerName = "Grid";
        while (elapsedTime < duration)
        {
            transformToMove.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        gameObject.SetActive(false);
    }


    public void OnHover(HoverArg hoverArg)
    {
        if (hoverArg.HOVERED_OBJECT == gameObject)
        {
            _hoverDarker.SetDarker();
            UIManager.instance.SetDetailsWindowInfo(this);
        }
        else
        {
            _hoverDarker.ResetColor();
        }
    }

    public void OnSelect(SelectArg selectArg)
    {
        if(selectArg.SELECTED_OBJECT == gameObject)
        {
            _selectDarker.SetDarker();

        } else
        {
            _selectDarker.ResetColor();
        }
    }

    public void OnTargettable(TargettableArg targettableArg)
    {
        if(Helper.IsMoveableSelectable(targettableArg.TARGETTABLE) && targettableArg.TARGETTABLE_POSITIONS.Contains(_currentPosition))
        {
            _blinkDarker.StartFadeLoop();
        } else
        {
            _blinkDarker.StopFadeLoop();
        }
    }

    public void GetPushed(GetPushedArg getPushedArg)
    {
        if(getPushedArg.TARGET == this)
        {
            int lengthofPush = Mathf.Max(getPushedArg.POWER - GetWeight(), 0);
            List<Vector3Int> pushPath = GridManager.instance.GetLineForPush(_currentPosition, getPushedArg.DIRECTION, lengthofPush);
            foreach (Vector3Int position in pushPath)
            {
                EventManager.instance.GE_Move.Invoke(new MoveArg(getPushedArg.SOURCE, this, position, true));
            }
        }
    }

    public virtual List<StatDetail> GetDetails()
    {
        StatDetail detail = new StatDetail("Weight", GetWeight(), GetWeight() > _moveableStats.Weight, GetWeight() < _moveableStats.Weight, DetailStatType.MoveableWeight);
        return new() { detail };
    }

    public virtual string GetDescription()
    {
        return "Moveable description";
    }

    public Sprite GetObjectSprite()
    {
        return _spriteRenderer.sprite;
    }
}
