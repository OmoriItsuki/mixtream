using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Evaluation
{
    None, Miss, Good, Hold
}

public enum Rank
{
    S=0, A, B
}

public class ScoreManager : SingletonMonoBehaviour<ScoreManager>
{
    public const int MAX_SCORE = 100000;
    public Dictionary<Evaluation, int> scoreCounter;
    public Dictionary<(NotesAttribute, TapStyle), int> attributeCounter;
    public int maxCombo;
    public int score;
    public int rate;
    public Rank rank;

    public static void AddScore(Evaluation evaluation, NotesAttribute na, TapStyle ts)
    {
        Instance.scoreCounter[evaluation]++;
        if(evaluation == Evaluation.Good || evaluation == Evaluation.Hold)
        {
            Instance.attributeCounter[(na, ts)]++;
        }
        ComboManager.ComboAdd(evaluation);
        Instance.maxCombo = ComboManager.maxCombo;
        
        //デバッグ用のスコアログ出力
        Debug.Log("----------------------------------------------------------------------");
        foreach (Evaluation scoreCounterKey in Instance.scoreCounter.Keys)
        { 
            Debug.Log(scoreCounterKey+" "+Instance.scoreCounter[scoreCounterKey]);  
        }

        foreach ((NotesAttribute, TapStyle) attributeCounterKey in Instance.attributeCounter.Keys)
        {
            Debug.Log(attributeCounterKey+" "+Instance.attributeCounter[attributeCounterKey]);
        }
        Debug.Log(calcScore() +" / "+ MAX_SCORE);
    }

    public static int calcScore()
    {
        double noteScore = MAX_SCORE * 0.8 * (Instance.scoreCounter[Evaluation.Good] + Instance.scoreCounter[Evaluation.Hold])/MusicManager.Instance.TheoryCombo;
        double comboScore = MAX_SCORE * 0.2 * Instance.maxCombo / MusicManager.Instance.TheoryCombo;
        return (int) (noteScore + comboScore);
    }

    // Start is called before the first frame update
    void Start()
    {
        scoreCounter = new Dictionary<Evaluation, int>()
        {
            {Evaluation.Good, 0},
            {Evaluation.Hold, 0},
            {Evaluation.Miss, 0},
            {Evaluation.None, 0}
        };
        attributeCounter = new Dictionary<(NotesAttribute, TapStyle), int>();
        foreach (NotesAttribute notesAttribute in Enum.GetValues(typeof(NotesAttribute)))
        {
            foreach (TapStyle tapStyle in Enum.GetValues(typeof(TapStyle)))
            {
                attributeCounter[(notesAttribute, tapStyle)] = 0;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
