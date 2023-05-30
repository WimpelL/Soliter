using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum eScoreEvent
{
    draw,
    mine,
    mineGold,
    gameWin,
    gameLoss
}

public class ScoreManager : MonoBehaviour
{
    static private ScoreManager S;
    static public int Score_From_Prev_Raund = 0;
    static public int High_Score = 0; 
    [Header("Set Dynamically")]
    public int chain = 0;
    public int scoreRun = 0;
    public int score = 0;

    void Awake()
    {
        if(S == null) S = this;
        else Debug.LogError("ERROR: ScoreManager.Awake(): S is already set!");

        if(PlayerPrefs.HasKey("ProspectorHighScore"))
        {
            High_Score = PlayerPrefs.GetInt("ProspectorHighScore");
        }
        score += Score_From_Prev_Raund;
        Score_From_Prev_Raund = 0;
    }
    static public void EVENT(eScoreEvent evt)
    {
        try{S.Event(evt);}
        catch(System.NullReferenceException nre)
        {
            Debug.LogError("ERROR: ScoreManager.EVENT(): called while S = null.\n"+ nre);
        }
    }

    void Event(eScoreEvent evt)
    {
        switch (evt)
        {
            case eScoreEvent.draw:
            case eScoreEvent.gameWin:
            case eScoreEvent.gameLoss:
                chain = 0;
                score += scoreRun;
                scoreRun = 0;
                break;
            case eScoreEvent.mine:
                chain++;
                scoreRun += chain;
                break;
        }

        switch(evt)
        {
            case eScoreEvent.gameWin:
                Score_From_Prev_Raund = score;
                print("You won this round! Round score: "+score);
                break;
            case eScoreEvent.gameLoss:
                if(High_Score <= score)
                {
                    print("You got the high score! High score: "+ score);
                    High_Score = score;
                    PlayerPrefs.SetInt("ProspectorHighScore", score);
                }
                else print("Your final score for the game was: " +score);
                break;
            default:
                print("score: "+score+" scoreRun: "+scoreRun+" chain: "+chain);
                break;
        }

    }
    static public int CHAIN{get{return S.chain;}}
    static public int SCORE{get{return S.score;}}
    static public int SCORE_RUN{get{return S.scoreRun;}}

}
