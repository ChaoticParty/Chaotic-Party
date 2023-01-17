using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public void Setup(Vector2 startPos, Vector2 endPos, float durate = -1, float height = -1, Sprite sprite = null)
    {
        startPosition = startPos;
        endPosition = endPos;
        if (durate != -1) duration = durate;
        if (height != -1) maxHeight = height;

        if (sprite != null) GetComponent<SpriteRenderer>().sprite = sprite;
        transform.position = startPosition;
        isInit = true;
    }

    private void FixedUpdate()
    {
        if (!isInit) return;
        
        float linearT = timePassed / duration;
        float heightT = animationCurve.Evaluate(linearT);
        float height = Mathf.Lerp(0f, maxHeight, heightT);

        transform.position = Vector2.Lerp(startPosition, endPosition, linearT) + new Vector2(0f, height);
        timePassed += Time.deltaTime;
        if (timePassed >= duration) Destroy(gameObject);
    }
}
