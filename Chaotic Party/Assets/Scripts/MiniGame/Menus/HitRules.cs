using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitRules : HitController
{
    public List<GameObject> objectsToHit;
    public Vector2 hitForce;
    private Collider2D _collider2D;
    private bool _wasHit;

    protected new void Awake()
    {
        _collider2D = GetComponent<Collider2D>();
    }

    public override void Hited(GameObject hitter)
    {
        if(_wasHit) return;
        _wasHit = true;
        
        float xScale = hitter.transform.localScale.x;
        //Lancement de l'anim de hit
        foreach (GameObject objectToHit in objectsToHit)
        {
            float rnd = Random.Range(hitForce.x, hitForce.x * 2);
            Rigidbody2D objectToHitRb = objectToHit.AddComponent<Rigidbody2D>();
            objectToHitRb.AddForce( new Vector3(
                rnd * xScale,
                rnd));
            objectToHitRb.AddTorque(rnd);
        }
        
        /*myRigidbody.AddForce(new Vector3(
            Random.Range(hitForce.x, hitForce.x * 2) * hitter.transform.localScale.x,
            Random.Range(hitForce.y, hitForce.y * 2))); //Propulsion*/
        //Reactivation des valeurs du gamepad
    }
}