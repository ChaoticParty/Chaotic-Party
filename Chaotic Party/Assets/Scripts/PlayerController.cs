using System;using HinputClasses;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class PlayerController : MonoBehaviour
{
    public MiniGameManager miniGameManager;
    public Gamepad gamepad;
    public int index;
    [SerializeField] private TextMeshProUGUI nameObject;
    [SerializeField] private string nameText;
    
    [NonSerialized] public UnityEvent aJustPressed = new ();
    [NonSerialized] public UnityEvent bJustPressed = new ();
    [NonSerialized] public UnityEvent xJustPressed = new ();
    [NonSerialized] public UnityEvent yJustPressed = new ();

    [NonSerialized] public UnityEvent<float> aLongPressed = new ();
    [NonSerialized] public UnityEvent<float> bLongPressed = new ();
    [NonSerialized] public UnityEvent<float> xLongPressed = new ();
    [NonSerialized] public UnityEvent<float> yLongPressed = new ();
    
    [NonSerialized] public UnityEvent<float, float> rightStickMoved = new ();
    [NonSerialized] public UnityEvent rightStickPressed = new ();
    [NonSerialized] public UnityEvent<float> rightStickLongPressed = new ();
    
    [NonSerialized] public UnityEvent<float, float> leftStickMoved = new (); //Déplacement avec joystick gauche
    [NonSerialized] public UnityEvent leftStickPressed = new ();
    [NonSerialized] public UnityEvent<float> leftStickLongPressed = new ();

    private void Start()
    {
        if(nameObject) nameObject.text = nameText + (index + 1);
    }

    private void Update()
    {
        if (gamepad == null) return;
        
        if(gamepad.A.justPressed)
        {
            aJustPressed.Invoke();
        }
        if(gamepad.B.justPressed)
        {
            bJustPressed.Invoke();
        }
        if(gamepad.X.justPressed)
        {
            xJustPressed.Invoke();
        }
        if(gamepad.Y.justPressed)
        {
            yJustPressed.Invoke();
        }
        
        if(gamepad.A.longPress.pressed)
        {
            aLongPressed.Invoke(gamepad.A.pressDuration);
        }
        if(gamepad.B.longPress.pressed)
        {
            bLongPressed.Invoke(gamepad.B.pressDuration);
        }
        if(gamepad.X.longPress.pressed)
        {
            xLongPressed.Invoke(gamepad.X.pressDuration);
        }
        if(gamepad.Y.longPress.pressed)
        {
            yLongPressed.Invoke(gamepad.Y.pressDuration);
        }
        
        if(gamepad.rightStick.distance != 0)
        {
            rightStickMoved.Invoke(gamepad.rightStick.horizontal, gamepad.rightStick.vertical);
        }
        if(gamepad.rightStickClick.pressed)
        {
            rightStickPressed.Invoke();
        }
        if(gamepad.rightStickClick.longPress.pressed)
        {
            rightStickLongPressed.Invoke(gamepad.rightStickClick.pressDuration);
        }
        
        if(gamepad.leftStick.distance != 0)
        {
            leftStickMoved.Invoke(gamepad.leftStick.horizontal, gamepad.leftStick.vertical);
        }
        if(gamepad.leftStickClick.pressed)
        {
            leftStickPressed.Invoke();
        }
        if(gamepad.leftStickClick.longPress.pressed)
        {
            leftStickLongPressed.Invoke(gamepad.leftStickClick.pressDuration);
        }
    }

    private void OnDisable()
    {
        aJustPressed.RemoveAllListeners();
        bJustPressed.RemoveAllListeners();
        xJustPressed.RemoveAllListeners();
        yJustPressed.RemoveAllListeners();
        aLongPressed.RemoveAllListeners();
        bLongPressed.RemoveAllListeners();
        xLongPressed.RemoveAllListeners();
        yLongPressed.RemoveAllListeners();
        rightStickMoved.RemoveAllListeners();
        rightStickPressed.RemoveAllListeners();
        rightStickLongPressed.RemoveAllListeners();
        leftStickMoved.RemoveAllListeners();
        leftStickPressed.RemoveAllListeners();
        leftStickLongPressed.RemoveAllListeners();
    }
}
