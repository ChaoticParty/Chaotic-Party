using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class HitController : MiniGameController
{
    protected Rigidbody2D myRigidbody;
    protected new void Awake()
    {
        base.Awake();
        myRigidbody = gameObject.GetComponent<Rigidbody2D>();
    }
    public abstract void Hited();
}