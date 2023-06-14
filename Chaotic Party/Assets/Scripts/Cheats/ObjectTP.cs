using UnityEngine;

public class ObjectTP : MonoBehaviour
{
    public Vector2 coordonées;
    public KeyCode inputActivation;

    private void Update()
    {
        if (Input.GetKeyDown(inputActivation) || Hinput.anyGamepad.Y.justPressed)
        {
            transform.position = coordonées;
            if (TryGetComponent(out Rigidbody2D rigidbody2D))
            {
                rigidbody2D.velocity = Vector2.zero;
                rigidbody2D.Sleep();
                rigidbody2D.WakeUp();
            }
        }
    }
}
