using TMPro;
using UnityEngine;

[CreateAssetMenu(fileName = "TextBuilder", menuName = "ScriptableObjects/FX/TextBuilder")]
public class TextBuilder : FXBuilder
{
    public string text;
    public Color color = Color.white;
    private TextMeshPro _textMeshPro;

    public override void Spawn(Vector2 position, int value)
    {
        text = value.ToString();
        FXSpawner.Spawn(this, startPosition + position);
    }

    public override void Spawn(Vector2 position, string value)
    {
        text = value;
        FXSpawner.Spawn(this, startPosition + position);
    }

    public override void BaseSetup(GameObject gameObject)
    {
        _textMeshPro = gameObject.AddComponent<TextMeshPro>();
        SetupText();
    }

    private void SetupText(string text = "")
    {
        _textMeshPro ??= fxObject.GetComponent<TextMeshPro>();
        if(text == default) this.text = text;
        _textMeshPro.text = this.text;
        _textMeshPro.color = color;
        _textMeshPro.alignment = TextAlignmentOptions.Center;
        _textMeshPro.sortingLayerID = SortingLayer.NameToID("FX");
    }

    #region SetTextBuilder

    /*public void SetTextBuilder(string text, Color color, float time, AnimationCurve scaleCurve, AnimationCurve rotationCurve)
    {
        SetupText(text, color);
        SetFXBuilder(time, scaleCurve, rotationCurve);
    }
    
    public void SetTextBuilder(string text, float time, AnimationCurve scaleCurve, AnimationCurve rotationCurve)
    {
        SetupText(text, Color.white);
        SetFXBuilder(time, scaleCurve, rotationCurve);
    }
    
    public void SetTextBuilder(string text, Color color, float time, Vector2 endScale, float endRotationAngle)
    {
        SetupText(text, color);
        SetFXBuilder(time, endScale, endRotationAngle);
    }
    
    public void SetTextBuilder(string text, float time, Vector2 endScale, float endRotationAngle)
    {
        SetupText(text, Color.white);
        SetFXBuilder(time, endScale, endRotationAngle);
    }
    
    public void SetTextBuilder(string text, float time)
    {
        SetupText(text, Color.white);
        SetFXBuilder(time);
    }
    
    public void SetTextBuilder()
    {
        SetupText(null, Color.white);
    }

    public void SetTextBuilder(TextFXStruct textStruct)
    {
        _textStruct = textStruct;
    }*/

    #endregion

    public void ChangeText(string text)
    {
        this.text = text;
        _textMeshPro.text = text;
    }

    public void ChangeColor(Color color)
    {
        this.color = color;
        _textMeshPro.color = color;
    }
}
