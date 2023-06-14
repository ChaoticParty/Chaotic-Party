using UnityEngine;

public class FXObject : MonoBehaviour
{
    private FXBuilder _builder;
    private float _currentTime;
    private Vector2 _startPost;
    private Vector2 _startScale;

    public void SetFXObject(FXBuilder builder)
    {
        _builder = builder;
        
        Transform transform1 = transform;
        _startPost = transform1.position;
        _startScale = transform1.localScale;
        
        GameObject go = gameObject;
        builder.fxObject = go;
        builder.BaseSetup(go);
        
        DoEveryFrame(0);
    }

    private void Update()
    {
        _currentTime += Time.deltaTime;
        DoEveryFrame(_currentTime);
        if(_currentTime > _builder.time) Destroy(gameObject);
    }
    
    private void DoEveryFrame(float currentTime)
    {
        transform.rotation = Quaternion.Euler(0, 0, _builder.rotationCurve.Evaluate(currentTime) * _builder.rotationMultiplier);

        float currentScaleValue = _builder.scaleCurve.Evaluate(currentTime) * _builder.scaleMultiplier;
        transform.localScale = _startScale + new Vector2(currentScaleValue, currentScaleValue);

        float x = _builder.xPositionCurve.Evaluate(currentTime) * _builder.xMultiplier;
        float y = _builder.yPositionCurve.Evaluate(currentTime) * _builder.yMultiplier;

        transform.position = _startPost + new Vector2(x, y);
    }
}
