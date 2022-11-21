using UnityEngine.Events;

public class Spam : SpamController
{
    public SpamButton spamButton;
    
    protected new void Awake()
    {
        base.Awake();
        UnityEvent buttonEvent = new();
        switch (spamButton)
        {
            case SpamButton.A:
                buttonEvent = player.aJustPressed;
                break;
            case SpamButton.B:
                buttonEvent = player.bJustPressed;
                break;
            case SpamButton.X:
                buttonEvent = player.xJustPressed;
                break;
            case SpamButton.Y:
                buttonEvent = player.yJustPressed;
                break;
            case SpamButton.RightStick:
                buttonEvent = player.rightStickPressed;
                break;
            case SpamButton.LeftStick:
                buttonEvent = player.leftStickPressed;
                break;
            case SpamButton.Any:
                break;
        }
        buttonEvent.AddListener(Click);
    }

    protected override void Click()
    {
        spamManager.Click(player.index, spamManager.spamValue, spamButton);
    }

    public void Malus()
    {
        spamManager.Click(player.index, -100, SpamButton.A);
    }
}

public enum SpamButton
{
    A,
    B,
    X,
    Y,
    RightStick,
    LeftStick,
    Any
}