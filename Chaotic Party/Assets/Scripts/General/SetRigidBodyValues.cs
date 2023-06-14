using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class SetRigidBodyValues : MonoBehaviour
{
    private Rigidbody2D _rigidbody2D;

    [SerializeField] private RigidbodyConstraints2D constraints2D = RigidbodyConstraints2D.FreezeRotation;

    private void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();

        _rigidbody2D.constraints = constraints2D;
        
        Destroy(this);
    }
}
