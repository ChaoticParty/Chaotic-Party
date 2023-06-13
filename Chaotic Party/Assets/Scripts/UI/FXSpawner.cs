using UnityEngine;

public class FXSpawner : MonoBehaviour
{
    public static FXObject Spawn(FXBuilder fxSo, Vector3 position)
    {
        GameObject fxObject = new() { transform = { position = position } };
        FXObject fxScript = fxObject.AddComponent<FXObject>();
        fxScript.SetFXObject(fxSo);
        return fxScript;
    }
    
    public static FXObject Spawn(GameObject fxGo, FXBuilder fxSo, Vector3 position)
    {
        GameObject fxObject = Instantiate(fxGo);
        FXObject fxScript = fxObject.AddComponent<FXObject>();
        fxScript.SetFXObject(fxSo);
        return fxScript;
    }
}
