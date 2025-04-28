using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationQueue : Singleton<AnimationQueue>
{
    private readonly Queue<IEnumerator> _animationQueue = new ();
    private bool _isAnimating = false;

    public void EnqueueAnimation(IEnumerator animation)
    {
        _animationQueue.Enqueue(animation);
        if (!_isAnimating && !GameManager.instance.IsGameOver)
            StartCoroutine(ProcessAnimationQueue());
    }

    private IEnumerator ProcessAnimationQueue()
    {
        _isAnimating = true;

        while (_animationQueue.Count > 0)
        {
            IEnumerator currentAnimation = _animationQueue.Dequeue();
            yield return StartCoroutine(currentAnimation);
        }

        _isAnimating = false;
    }

    public IEnumerator WaitForAnimationQueue()
    {
        while (_isAnimating)
        {
            yield return null;
        }
    }
}
