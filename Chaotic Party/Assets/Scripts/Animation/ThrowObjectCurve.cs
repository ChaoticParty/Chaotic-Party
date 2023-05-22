using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class ThrowObjectCurve : MonoBehaviour
{
    [Header("Curve")]
    [SerializeField] private AnimationCurve animationCurve;

    [Space] [Header("Values")] 
    private bool isInit = false;
    private float timePassed = 0;
    private Vector2 startPosition;
    private Vector2 endPosition;
    [SerializeField] private float duration = 1;
    [SerializeField] private float maxHeight = 3;
    private Action _onEnd;

    public void Setup(Vector2 startPos, Vector2 endPos, float durate = -1, float height = -1, Sprite sprite = null, Vector3 scale = default, Action onEnd = null)
    {
        startPosition = startPos;
        endPosition = endPos;
        if (durate > 0) duration = durate;
        if (height > 0) maxHeight = height;
        animationCurve = new AnimationCurve(new[] { new Keyframe(0, 0, 0, 10), 
            new Keyframe(duration / 2, maxHeight), new Keyframe(duration, 0, -10, 0) });
        _onEnd = onEnd;
        transform.localScale = scale;

        if (sprite != null)
        {
            SpriteRenderer renderer = GetComponent<SpriteRenderer>();
            renderer.sprite = sprite;
            renderer.sortingLayerName = "Foreground";
            renderer.sortingOrder = 100;
        }
        transform.position = startPosition;
        isInit = true;
    }

    private void FixedUpdate()
    {
        if (!isInit) return;
        
        float linearT = timePassed / duration;
        float height = animationCurve.Evaluate(timePassed);

        transform.position = Vector2.Lerp(startPosition, endPosition, linearT) + new Vector2(0f, height);
        timePassed += Time.deltaTime;
        if (timePassed >= duration)
        {
            _onEnd();
            Destroy(gameObject);
        }
    }
}
