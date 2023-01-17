using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestFeatures : MonoBehaviour
{
    [SerializeField] private GameObject prefabProjectile;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.P))
        {
            var position = transform.position;
            GameObject g = Instantiate(prefabProjectile, position, new Quaternion(0,0,0,0));
            g.GetComponent<ThrowObjectCurve>().Setup(position, new Vector2(position.x + 2,position.y), 3, 1);
        }
    }
}
