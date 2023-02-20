using System.Collections.Generic;

public class BaseTrainingRoomManager : MiniGameManager
{
    public override void FinishTimer() { }

    protected override int GetWinner() { return default;}
    protected override Dictionary<PlayerController, int> GetRanking()
    {
        return null;
    }

    protected override void OnMinigameEnd() { }
}
