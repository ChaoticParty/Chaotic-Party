using System;
using UnityEngine;
using UnityEngine.Events;

public abstract class FXBuilder : ScriptableObject
{
    public AudioClip clip;
    public float time;
    public AnimationCurve scaleCurve;
    public float scaleMultiplier = 1;
    public AnimationCurve rotationCurve;
    public float rotationMultiplier = 1;
    public Vector2 startPosition;
    public AnimationCurve xPositionCurve;
    public float xMultiplier = 1;
    public AnimationCurve yPositionCurve;
    public float yMultiplier = 1;
    [NonSerialized] public GameObject fxObject;
    public UnityEvent onEndEvent;

    public virtual void Spawn(Vector2 position)
    {
        FXSpawner.Spawn(this, startPosition + position);
    }

    public virtual void Spawn()
    {
        FXSpawner.Spawn(this, startPosition);
    }

    public virtual void Spawn(Vector2 position, int value)
    {
        FXSpawner.Spawn(this, startPosition + position);
    }

    public virtual void Spawn(Vector2 position, string value)
    {
        FXSpawner.Spawn(this, startPosition + position);
    }

    public virtual void Spawn(Vector2 position, Sprite sprite)
    {
        FXSpawner.Spawn(this, startPosition + position);
    }

    public virtual void Spawn(Vector2 position, GameObject prefab)
    {
        FXSpawner.Spawn(this, startPosition + position);
    }

    public abstract void BaseSetup(GameObject gameObject);

    #region SetFXBuilder

    /*public void SetFXBuilder(float time, AnimationCurve scaleCurve, AnimationCurve rotationCurve)
    {
        this.time = time;
        this.scaleCurve = scaleCurve;
        this.rotationCurve = rotationCurve;
    }
    
    public void SetFXBuilder(float time, Vector2 endScale, float endRotationAngle)
    {
        this.time = time;
        this.endScale = endScale;
        this.endRotationAngle = endRotationAngle;
        rotationCurve = null;
        scaleCurve = null;
    }

    public void SetFXBuilder(float time)
    {
        this.time = time;
        endScale = Vector2.one;
        endRotationAngle = 0;
        rotationCurve = null;
        scaleCurve = null;
    }

    public void SetFXBuilder()
    {
        time = 1;
        endScale = Vector2.one;
        endRotationAngle = 0;
        rotationCurve = null;
        scaleCurve = null;
    }*/

    #endregion

    public void PlaySound()
    {
        //audioSource.Play(_clip);
    }
}