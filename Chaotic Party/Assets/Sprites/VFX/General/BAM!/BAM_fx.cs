using Sirenix.OdinInspector;
using UnityEngine;

public class BAM_fx : MonoBehaviour
{
    public Animator bamAnimator;
    public CP_OnomatopeChoice script;

    private void Start()
    {
        Debug.Log(transform);
        Debug.Log(transform.parent);
        Debug.Log(transform.parent.transform);
        Debug.Log(transform.parent.transform.localScale);
        Debug.Log(transform.parent.transform.localScale.x);
        transform.localScale = new Vector3(transform.parent.transform.localScale.x * 0.5f,0.5f,0.5f);
        transform.position = new Vector3(transform.position.x,transform.position.y + 2,transform.position.z);
        BAM();
    }

    [Button("BAM")]
    public void BAM()
    {
        script.BAM();
        bamAnimator.SetTrigger("BAM");
    }

    public void Destroy()
    {
        Destroy(gameObject);
    }
}
