using System.Collections;
using UnityEngine;

public abstract class Moveable : MonoBehaviour
{
    [SerializeField]
    private MoveableStats MoveableStats;

    public Event<Moveable> BeforeFall = new Event<Moveable>();
    public Event<Moveable> AfterFall = new Event<Moveable>();


    public Event<Moveable> BeforeMove = new Event<Moveable>();
    public Event<Moveable> AfterMove = new Event<Moveable>();



    public virtual void SetMoveableStats(MoveableStats moveableStats)
    {
        MoveableStats = moveableStats;
    }


    //Only ever move one gridspace?
    public virtual void Move(Vector3 newPosition)
    {
        BeforeMove.Invoke(this);
        StartCoroutine(MoveToPosition(newPosition, 0.5f));
        AfterMove.Invoke(this);
    }

    protected IEnumerator MoveToPosition(Vector3 targetPosition, float duration)
    {
        Vector3 startPosition = transform.position;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            transform.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure the position is exactly the target position at the end
        transform.position = targetPosition;
    }

    public virtual void Push(int direction, int distance)
    {
        //Move();
    }

    public virtual void Pull(int direction, int distance)
    {
        //Move();
    }

    public virtual void Fall(HexTile tile)
    {
        BeforeFall.Invoke(this);


        StartCoroutine(MoveToPosition(new Vector3(transform.position.x, -20, -10), 1f));
        Debug.Log("Is out of bounds.");
        gameObject.SetActive(false);

        AfterFall.Invoke(this);
    }
}
