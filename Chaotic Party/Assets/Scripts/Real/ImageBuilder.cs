using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Imagebuilder", menuName = "ScriptableObjects/FX/ImageBuilder")]
public class ImageBuilder : FXBuilder
{
    private Sprite _sprite;
    private SpriteRenderer _spriteRenderer;

    public override void Spawn(Vector2 position, Sprite image)
    {
        _sprite = image;
        FXSpawner.Spawn(this, position);
    }

    public override void BaseSetup(GameObject gameObject)
    {
        _spriteRenderer = gameObject.AddComponent<SpriteRenderer>();
        SetupSprite();
    }

    #region SetImageBuilder

    /*public void SetImageBuilder(Sprite sprite, float time, AnimationCurve scaleCurve, AnimationCurve rotationCurve)
    {
        SetupSprite(sprite);
        SetFXBuilder(time, scaleCurve, rotationCurve);
    }
    
    public void SetImageBuilder(Sprite sprite, float time, Vector2 endScale, float endRotationAngle)
    {
        SetupSprite(sprite);
        SetFXBuilder(time, endScale, endRotationAngle);
    }
    
    public void SetImageBuilder(Sprite sprite, float time)
    {
        SetupSprite(sprite);
        SetFXBuilder(time);
    }
    
    public void SetImageBuilder()
    {
        SetupSprite(null);
    }*/

    #endregion

    private void SetupSprite(Sprite sprite = null)
    {
        _spriteRenderer ??= fxObject.GetComponent<SpriteRenderer>();
        if(sprite) _sprite = sprite;
        _spriteRenderer.sprite = _sprite;
        _spriteRenderer.sortingLayerName = "FX";
    }

    public void ChangeSprite(Sprite sprite)
    {
        _sprite = sprite;
        _spriteRenderer.sprite = sprite;
    }
}
