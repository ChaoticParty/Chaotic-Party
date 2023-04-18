using System;
using System.Collections.Generic;
using HinputClasses;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[RequireComponent(typeof(CrownManager))]
public class PlayerController : MonoBehaviour
{
    public MiniGameManager miniGameManager;
    public SkinSelector skinSelector;
    public List<MiniGameController> miniGameControllers;
    public Gamepad gamepad;
    public int index;
    [SerializeField] private TextMeshProUGUI nameObject;
    [SerializeField] private string nameText;
    [SerializeField] private TextMeshProUGUI bulleText;
    [SerializeField] private Image bullSpt;
    private PlayerSO _playerSo;
    private CrownManager _crownManager;

    #region Sprites
    
    public SpriteRenderer head;
    public SpriteRenderer body;

    #endregion

    #region ControllerEvents

    [NonSerialized] public UnityEvent startPressed = new ();
    [NonSerialized] public UnityEvent selectPressed = new ();
    
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
    
    [NonSerialized] public UnityEvent dPadUp = new ();
    [NonSerialized] public UnityEvent dPadJustUp = new ();
    [NonSerialized] public UnityEvent dPadDown = new ();
    [NonSerialized] public UnityEvent dPadJustDown = new ();
    [NonSerialized] public UnityEvent dPadLeft = new ();
    [NonSerialized] public UnityEvent dPadJustLeft = new ();
    [NonSerialized] public UnityEvent dPadRight = new ();
    [NonSerialized] public UnityEvent dPadJustRight = new ();
    [NonSerialized] private bool isDPadMovedUp;
    [NonSerialized] private bool isDPadMovedDown;
    [NonSerialized] private bool isDPadMovedLeft;
    [NonSerialized] private bool isDPadMovedRight;

    [NonSerialized] public UnityEvent leftBumperClick = new();
    [NonSerialized] public UnityEvent rightBumperClick = new();
    
    [NonSerialized] public UnityEvent leftTriggerClick = new();
    [NonSerialized] public UnityEvent rightTriggerClick = new();

    #endregion

    #region PlayerStateBooleans

    public bool isInTheAir;
    public bool isTackling;
    public bool isHit;
    public bool isPausing;
    public bool isStunned;
    public bool isMoving;

    #endregion

    private void Start()
    {
        if(nameObject) nameObject.text = nameText + (index + 1);
        miniGameManager ??= FindObjectOfType<MiniGameManager>();
        _crownManager ??= GetComponent<CrownManager>();
    }

    public void AddAllListeners()
    {
        foreach (MiniGameController miniGameController in miniGameControllers)
        {
            miniGameController.AddListeners();
        }
    }

    private void Update()
    {
        if (gamepad == null /*|| miniGameManager.isGameDone*/) return;

        if (gamepad.start.justPressed)
        {
            startPressed.Invoke();
        }
        if (gamepad.back.justPressed)
        {
            selectPressed.Invoke();
        }
        if(gamepad.A.justPressed)
        {
            aJustPressed.Invoke();
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
        
        if(gamepad.rightStick.distance > 0.2f)
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
        
        if(gamepad.leftStick.distance > 0.2f)
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

        if(gamepad.dPad.up)
        {
            if (!isDPadMovedUp)
            {
                isDPadMovedUp = true;
                isDPadMovedDown = false;
                isDPadMovedLeft = false;
                isDPadMovedRight = false;
                dPadUp.Invoke();
            }
        }
        if(gamepad.dPad.down)
        {
            if (!isDPadMovedDown)
            {
                isDPadMovedDown = true;
                isDPadMovedUp = false;
                isDPadMovedLeft = false;
                isDPadMovedRight = false;
                dPadDown.Invoke();
            }
        }
        if(gamepad.dPad.left)
        {
            if (!isDPadMovedLeft)
            {
                isDPadMovedLeft = true;
                isDPadMovedUp = false;
                isDPadMovedDown = false;
                isDPadMovedRight = false;
                dPadLeft.Invoke();
            }
        }
        if(gamepad.dPad.right)
        {
            if (!isDPadMovedRight)
            {
                isDPadMovedRight = true;
                isDPadMovedUp = false;
                isDPadMovedDown = false;
                isDPadMovedLeft = false;
                dPadRight.Invoke();    
            }
        }

        if (!gamepad.dPad.inPressedZone)
        {
            isDPadMovedUp = false;
            isDPadMovedDown = false;
            isDPadMovedLeft = false;
            isDPadMovedRight = false;
        }
        
        if (gamepad.leftBumper.justPressed)
        {
            leftBumperClick.Invoke();
        }
        if (gamepad.rightBumper.justPressed)
        {
            rightBumperClick.Invoke();
        }
        
        if (gamepad.leftTrigger.justPressed)
        {
            rightTriggerClick.Invoke();
        }
        if (gamepad.rightTrigger.justPressed)
        {
            rightTriggerClick.Invoke();
        }
    }

    private void OnDisable()
    {
        RemoveAllListeners();
    }
    
    public void RemoveAllListeners()
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
        dPadUp.RemoveAllListeners();
        dPadDown.RemoveAllListeners();
        dPadLeft.RemoveAllListeners();
        dPadRight.RemoveAllListeners();
        leftBumperClick.RemoveAllListeners();
        rightBumperClick.RemoveAllListeners();
        leftTriggerClick.RemoveAllListeners();
        rightTriggerClick.RemoveAllListeners();
    }

    public bool IsDoingSomething()
    {
        return isInTheAir || isTackling || isHit || isPausing || isStunned || isMoving;
    }

    public bool CanAct()
    {
        return !(isInTheAir || isTackling || isHit || isStunned);
    }

    public bool CanMove()
    {
        return !(isTackling || isHit || isStunned);
    }

    public bool CanBeStunned()
    {
        return !(isStunned || isHit);
    }

    public bool IsStandingStill()
    {
        return !(isStunned || isMoving || isInTheAir || isTackling || isHit);
    }

    public void SetupSprite(PlayerSO playerSo)
    {
        _playerSo = playerSo;
        
        skinSelector.SetupSkin(_playerSo.head, _playerSo.body, _playerSo.color);
    }

    public void ChangeColor()
    {
        ChangeColor(_playerSo.color);
    }

    public void ChangeColor(Color color)
    {
        head.color = color;
        body.color = color;
    }

    #region Methodes Bulle

    public void ActivateBulle(bool activate)
    {
        bullSpt.GameObject().SetActive(activate);
    }

    public void ChangeBulleText(string text)
    {
        bulleText.text = text;
    }

    public void ChangeBulleImage(Sprite image)
    {
        bullSpt.sprite = image;
    }

    #endregion
}
