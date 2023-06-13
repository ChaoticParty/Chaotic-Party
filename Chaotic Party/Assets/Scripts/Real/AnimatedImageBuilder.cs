using UnityEngine;

[CreateAssetMenu(fileName = "AnimatedImagebuilder", menuName = "ScriptableObjects/FX/AnimatedImageBuilder")]
public class AnimatedImageBuilder : FXBuilder
{
    public GameObject prefab;

    public override void Spawn()
    {
        Debug.Log(prefab.name);
        FXSpawner.Spawn(prefab, this, startPosition);
        Debug.Log(prefab.name);
    }

    public override void Spawn(Vector2 position, GameObject prefab)
    {
        this.prefab = prefab;
        FXSpawner.Spawn(prefab, this, startPosition + position);
    }

    public override void Spawn(Vector2 position, Sprite image)
    {
        FXSpawner.Spawn(prefab, this, startPosition + position);
    }

    public override void BaseSetup(GameObject gameObject)
    {
        
    }

    private void SetupPrefab(GameObject go = null)
    {
        prefab = go;
    }

    public void ChangePrefab(GameObject go)
    {
        prefab = go;
    }
}
