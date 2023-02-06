using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class HitButton : HitController
{
    public UnityEvent onHitEvent;
    public float hitCooldown = -1;
    private bool _wasHit;

    public override void Hited(GameObject hitter)
    {
        if(_wasHit) return;
        _wasHit = true;
        
        onHitEvent.Invoke();

        if (hitCooldown >= 0)
        {
            StartCoroutine(CanHitAgain());
        }
    }

    private IEnumerator CanHitAgain()
    {
        yield return new WaitForSeconds(hitCooldown);
        _wasHit = false;
    }
}