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
    
    [NonSerialized] public UnityEvent startPressed = new ();
    
    [NonSerialized] public UnityEvent aJustPressed = new ();
    [NonSerialized] public UnityEvent bJustPressed = new ();
    [NonSerialized] public UnityEvent xJustPressed = new ();
    [NonSerialized] public UnityEvent yJustPressed = new ();

    [NonSerialized] public UnityEvent<float> aLongPressed = new ();
    [NonSerialized] public UnityEvent<float> bLongPressed = new ();
    [NonSerialized] public UnityEvent<float> xLongPressed = new ();
    [NonSerialized] public UnityEvent<float> yLongPressed = new ();
    
    [NonSerialized] public UnityEvent<float, float> rightStickMoved = new ();
    [NonSerialized] public UnityEvent<float, float> rightStickMovedUp = new ();
    [NonSerialized] public UnityEvent<float, float> rightStickMovedDown = new ();
    [NonSerialized] public UnityEvent<float, float> rightStickMovedLeft = new ();
    [NonSerialized] public UnityEvent<float, float> rightStickMovedRight = new ();
    [NonSerialized] public UnityEvent<float, float> rightStickJustMovedUp = new ();
    [NonSerialized] public UnityEvent<float, float> rightStickJustMovedDown = new ();
    [NonSerialized] public UnityEvent<float, float> rightStickJustMovedLeft = new ();
    [NonSerialized] public UnityEvent<float, float> rightStickJustMovedRight = new ();
    [NonSerialized] private bool isRightStickMovedUp;
    [NonSerialized] private bool isRightStickMovedDown;
    [NonSerialized] private bool isRightStickMovedLeft;
    [NonSerialized] private bool isRightStickMovedRight;
    [NonSerialized] public UnityEvent<float, float> rightStickJustMoved = new ();
    [NonSerialized] private bool isRightStickMoved;
    [NonSerialized] public UnityEvent rightStickPressed = new ();
    [NonSerialized] public UnityEvent<float> rightStickLongPressed = new ();
    
    [NonSerialized] public UnityEvent<float, float> leftStickMoved = new (); //Déplacement avec joystick gauche
    [NonSerialized] public UnityEvent<float, float> leftStickMovedUp = new ();
    [NonSerialized] public UnityEvent<float, float> leftStickMovedDown = new ();
    [NonSerialized] public UnityEvent<float, float> leftStickMovedLeft = new ();
    [NonSerialized] public UnityEvent<float, float> leftStickMovedRight = new ();
    [NonSerialized] public UnityEvent<float, float> leftStickJustMovedUp = new ();
    [NonSerialized] public UnityEvent<float, float> leftStickJustMovedDown = new ();
    [NonSerialized] public UnityEvent<float, float> leftStickJustMovedLeft = new ();
    [NonSerialized] public UnityEvent<float, float> leftStickJustMovedRight = new ();
    [NonSerialized] private bool isLeftStickMovedUp;
    [NonSerialized] private bool isLeftStickMovedDown;
    [NonSerialized] private bool isLeftStickMovedLeft;
    [NonSerialized] private bool isLeftStickMovedRight;
    [NonSerialized] public UnityEvent<float, float> leftStickJustMoved = new ();
    [NonSerialized] private bool isLeftStickMoved;
    [NonSerialized] public UnityEvent leftStickPressed = new ();
    [NonSerialized] public UnityEvent<float> leftStickLongPressed = new ();

    #region PlayerStateBooleans

    public bool isInTheAir;
    public bool isTackling;
    public bool isHit;
    public bool isPausing;

    #endregion

    private void Start()
    {
        if(nameObject) nameObject.text = nameText + (index + 1);
        miniGameManager ??= FindObjectOfType<MiniGameManager>();
    }

    private void Update()
    {
        if (gamepad == null || miniGameManager.isGameDone) return;

        if (gamepad.start.justPressed)
        {
            startPressed.Invoke();
        }
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
            if (!isRightStickMoved)
            {
                rightStickJustMoved.Invoke(gamepad.rightStick.horizontal, gamepad.rightStick.vertical);
                isRightStickMoved = true;
            }
            if (gamepad.rightStick.up)
            {
                if (!isRightStickMovedUp)
                {
                    rightStickJustMovedUp.Invoke(gamepad.rightStick.horizontal, gamepad.rightStick.vertical);
                    isRightStickMovedUp = true;
                }
                rightStickMovedUp.Invoke(gamepad.rightStick.horizontal, gamepad.rightStick.vertical);
            }
            else
            {
                isRightStickMovedUp = false;
            }
            if (gamepad.rightStick.down)
            {
                if (!isRightStickMovedDown)
                {
                    rightStickJustMovedDown.Invoke(gamepad.rightStick.horizontal, gamepad.rightStick.vertical);
                    isRightStickMovedDown = true;
                }
                rightStickMovedDown.Invoke(gamepad.rightStick.horizontal, gamepad.rightStick.vertical);
            }
            else
            {
                isRightStickMovedDown = false;
            }
            if (gamepad.rightStick.left)
            {
                if (!isRightStickMovedLeft)
                {
                    rightStickJustMovedLeft.Invoke(gamepad.rightStick.horizontal, gamepad.rightStick.vertical);
                    isRightStickMovedLeft = true;
                }
                rightStickMovedLeft.Invoke(gamepad.rightStick.horizontal, gamepad.rightStick.vertical);
            }
            else
            {
                isRightStickMovedLeft = false;
            }
            if (gamepad.rightStick.right)
            {
                if (!isRightStickMovedRight)
                {
                    rightStickJustMovedRight.Invoke(gamepad.rightStick.horizontal, gamepad.rightStick.vertical);
                    isRightStickMovedRight = true;
                }
                rightStickMovedRight.Invoke(gamepad.rightStick.horizontal, gamepad.rightStick.vertical);
            }
            else
            {
                isRightStickMovedRight = false;
            }
            rightStickMoved.Invoke(gamepad.rightStick.horizontal, gamepad.rightStick.vertical);
        }
        else
        {
            isRightStickMoved = false;
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
            if (!isLeftStickMoved)
            {
                leftStickJustMoved.Invoke(gamepad.leftStick.horizontal, gamepad.leftStick.vertical);
                isLeftStickMoved = true;
            }
            if (gamepad.leftStick.up)
            {
                if (!isLeftStickMovedUp)
                {
                    leftStickJustMovedUp.Invoke(gamepad.rightStick.horizontal, gamepad.rightStick.vertical);
                    isLeftStickMovedUp = true;
                }
                leftStickMovedUp.Invoke(gamepad.rightStick.horizontal, gamepad.rightStick.vertical);
            }
            else
            {
                isLeftStickMovedUp = false;
            }
            if (gamepad.leftStick.down)
            {
                if (!isLeftStickMovedDown)
                {
                    leftStickJustMovedDown.Invoke(gamepad.rightStick.horizontal, gamepad.rightStick.vertical);
                    isLeftStickMovedDown = true;
                }
                leftStickMovedDown.Invoke(gamepad.rightStick.horizontal, gamepad.rightStick.vertical);
            }
            else
            {
                isLeftStickMovedDown = false;
            }
            if (gamepad.leftStick.left)
            {
                if (!isLeftStickMovedLeft)
                {
                    leftStickJustMovedLeft.Invoke(gamepad.rightStick.horizontal, gamepad.rightStick.vertical);
                    isLeftStickMovedLeft = true;
                }
                leftStickMovedLeft.Invoke(gamepad.rightStick.horizontal, gamepad.rightStick.vertical);
            }
            else
            {
                isLeftStickMovedLeft = false;
            }
            if (gamepad.leftStick.right)
            {
                if (!isLeftStickMovedRight)
                {
                    leftStickJustMovedRight.Invoke(gamepad.rightStick.horizontal, gamepad.rightStick.vertical);
                    isLeftStickMovedRight = true;
                }
                leftStickMovedRight.Invoke(gamepad.rightStick.horizontal, gamepad.rightStick.vertical);
            }
            else
            {
                isLeftStickMovedRight = false;
            }
            leftStickMoved.Invoke(gamepad.leftStick.horizontal, gamepad.leftStick.vertical);
        }
        else
        {
            isLeftStickMoved = false;
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
        startPressed.RemoveAllListeners();
        aJustPressed.RemoveAllListeners();
        bJustPressed.RemoveAllListeners();
        xJustPressed.RemoveAllListeners();
        yJustPressed.RemoveAllListeners();
        aLongPressed.RemoveAllListeners();
        bLongPressed.RemoveAllListeners();
        xLongPressed.RemoveAllListeners();
        yLongPressed.RemoveAllListeners();
        rightStickMoved.RemoveAllListeners();
        rightStickJustMoved.RemoveAllListeners();
        rightStickMovedDown.RemoveAllListeners();
        rightStickMovedLeft.RemoveAllListeners();
        rightStickMovedRight.RemoveAllListeners();
        rightStickMovedUp.RemoveAllListeners();
        rightStickJustMovedDown.RemoveAllListeners();
        rightStickJustMovedLeft.RemoveAllListeners();
        rightStickJustMovedRight.RemoveAllListeners();
        rightStickJustMovedUp.RemoveAllListeners();
        rightStickPressed.RemoveAllListeners();
        rightStickLongPressed.RemoveAllListeners();
        leftStickMoved.RemoveAllListeners();
        leftStickJustMoved.RemoveAllListeners();
        leftStickMovedDown.RemoveAllListeners();
        leftStickMovedLeft.RemoveAllListeners();
        leftStickMovedRight.RemoveAllListeners();
        leftStickMovedUp.RemoveAllListeners();
        leftStickJustMovedDown.RemoveAllListeners();
        leftStickJustMovedLeft.RemoveAllListeners();
        leftStickJustMovedRight.RemoveAllListeners();
        leftStickJustMovedUp.RemoveAllListeners();
        leftStickPressed.RemoveAllListeners();
        leftStickLongPressed.RemoveAllListeners();
    }

    public bool IsDoingSomething()
    {
        return isInTheAir && isTackling && isHit && isPausing;
    }

    public bool CanAct()
    {
        return !(isInTheAir && isTackling && isHit);
    }
}
