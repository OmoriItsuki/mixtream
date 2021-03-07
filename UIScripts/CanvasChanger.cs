using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class CanvasChanger : MonoBehaviour
{
    [SerializeField] public int maskSpeed=3;
    [SerializeField] public GameObject mainC, pauseC, resultBaseC, resultC;
    [SerializeField] public GameObject trackName, artist, jacket;
    public bool isPause = false;
    private bool isResult = false, inputEnable = true;
    private bool isResultCalled = false;

    // Start is called before the first frame update
    void Start()
    {
        if(MusicManager.Instance.IsSelected)
        {
            trackName.GetComponent<Text>().text = MusicManager.Instance.TrackName;
            artist.GetComponent<Text>().text = MusicManager.Instance.ArtistName;
            jacket.GetComponent<Image>().sprite = MusicManager.Instance.Jacket;
            Debug.Log("track data is set" + jacket.GetComponent<Image>().sprite);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!inputEnable) return;
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            GameManager.ChangeResumePause();
        }

        #if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.A))
        {
            ShowResultCanvas();
        }
        #endif

        if (!isResultCalled & GameManager.musicStatus == CriAtomSource.Status.PlayEnd)
        {
            isResultCalled = true;
            ShowResultCanvas();
        }
    }

    /// <summary>
    /// リザルト画面を表示するための関数
    /// </summary>
    public void ShowResultCanvas()
    {
        bool isHighScore = false;
        if (MusicManager.Instance.IsSelected)
        {
            Json data = JsonClass.Instance.Loads(MusicManager.Instance.Id + "-" + MusicManager.Instance.Diff);
            resultC.transform.GetChild(0).GetChild(0).GetComponent<Image>().sprite = MusicManager.Instance.Jacket;
            foreach (Difficulty diff in Enum.GetValues(typeof(Difficulty)))
            {
                resultC.transform.GetChild(0).GetChild(2).Find(diff.ToString()).gameObject
                    .SetActive(diff == MusicManager.Instance.Diff);
            }

            resultC.transform.GetChild(0).GetChild(6).GetComponent<Text>().text = MusicManager.Instance.TrackName;
            resultC.transform.GetChild(0).GetChild(7).GetComponent<Text>().text = MusicManager.Instance.ArtistName;
            resultC.transform.GetChild(0).GetChild(9).GetComponent<Text>().text = MusicManager.Instance.Level;

            ScoreManager.Instance.score = ScoreManager.calcScore();
            ScoreManager.Instance.rate = ScoreManager.Instance.score / ScoreManager.MAX_SCORE;
            int rate = ScoreManager.Instance.rate;
            ScoreManager.Instance.rank = rate > 80 ? rate > 90 ? Rank.S : Rank.A : Rank.B;
            foreach (Rank rank in Enum.GetValues(typeof(Rank)))
            {
                resultC.transform.GetChild(1).GetChild(3).Find("rankLogo_"+rank).GetComponent<CanvasGroup>().alpha = (rank == ScoreManager.Instance.rank) ? 1 : 0;
            }

            resultC.transform.GetChild(2).GetChild(0).GetComponent<Text>().text = ScoreManager.Instance.maxCombo.ToString();
            resultC.transform.GetChild(3).GetChild(0).GetComponent<Text>().text = rate + "%";
            foreach (var (score, index) in ScoreManager.Instance.attributeCounter.Select((score, index) => (score, index)))
            {
                resultC.transform.GetChild(4).GetChild(index).GetChild(1).GetComponent<Text>().text = score.Value.ToString();
            }
            resultC.transform.GetChild(6).GetChild(3).GetComponent<Text>().text = ScoreManager.Instance.score.ToString();
            isHighScore = ScoreManager.Instance.score >= data.highScore;
            if (isHighScore)
            {
                resultC.transform.GetChild(7).GetChild(3).GetComponent<Text>().text = ScoreManager.Instance.score.ToString();
                resultC.transform.GetChild(8).gameObject.SetActive(true);
                data.highScore = ScoreManager.Instance.score;
                JsonClass.Instance.Saves(data);
            }
            else
            {
                resultC.transform.GetChild(7).GetChild(3).GetComponent<Text>().text = data.highScore.ToString();
                resultC.transform.GetChild(8).gameObject.SetActive(false);
            }

        }
        mainC.GetComponent<UIAnimStarter>().launchAnimationAll("OUT",0f);
        resultBaseC.GetComponent<UIAnimStarter>().launchAnimationAll("IN",1f);
        if (isHighScore)
        {
            resultC.GetComponent<UIAnimStarter>().launchAnimationAll("Highscore_IN",1f);
        }
        else
        {
            resultC.GetComponent<UIAnimStarter>().launchAnimationAll("result_IN",1f);
        }

        StartCoroutine(WaitInput());
    }
    
    IEnumerator WaitInput()
    {
        yield return null;
        yield return new WaitUntil((() => Input.anyKeyDown));
        SceneManager.LoadScene("MusicSelect");
    }

}
