using UnityEngine;

[CreateAssetMenu(fileName = "Imagebuilder", menuName = "ScriptableObjects/FX/ImageBuilder")]
public class ImageBuilder : FXBuilder
{
    public Sprite sprite;
    public SpriteRenderer spriteRenderer;

    public override void Spawn(Vector2 position, Sprite image)
    {
        sprite = image;
        FXSpawner.Spawn(this, startPosition + position);
        SetupSprite();
    }

    public override void BaseSetup(GameObject gameObject)
    {
        spriteRenderer = gameObject.AddComponent<SpriteRenderer>();
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
        spriteRenderer ??= fxObject.GetComponent<SpriteRenderer>();
        if(sprite) this.sprite = sprite;
        spriteRenderer.sprite = this.sprite;
        spriteRenderer.sortingLayerName = "Player";
        spriteRenderer.sortingOrder = 100;
    }

    public void ChangeSprite(Sprite sprite)
    {
        this.sprite = sprite;
        spriteRenderer.sprite = sprite;
    }
}
