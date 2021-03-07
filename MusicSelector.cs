using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicSelector : MonoBehaviour
{
    private GetMusics getMusics;

    // Start is called before the first frame update
    void Start()
    {
        getMusics = GetComponent<GetMusics>();
        getMusics.camera.backgroundColor = new Color(188/255f, 255/255f, 188/255f);
    }

    // Update is called once per frame
    void Update()
    {
        MoveSelector();
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartMusic();
        }else if(Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene("Title");
        }
        #if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.R))
        {
            ResetJson();
        }
        #endif
    }
    
    /// <summary>
    /// 矢印キーによるセレクターの移動とそれに伴うメインジャケット画像の差し替え
    /// </summary>
    void MoveSelector()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow) && getMusics.inputEnable)
        {
            getMusics.MoveJacket(false, false);
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow) && getMusics.inputEnable)
        {
            getMusics.MoveJacket(true, false);
        }

        //以下長押しへの対応
        if (Input.GetKey(KeyCode.RightArrow) && getMusics.inputEnable)
        {
            getMusics.MoveJacket(false, true);
        }
        else if (Input.GetKey(KeyCode.LeftArrow) && getMusics.inputEnable)
        {
            getMusics.MoveJacket(true, true);
        }
        
        //難易度変更
        if (Input.GetKeyDown(KeyCode.UpArrow) && getMusics.inputEnable)
        {
            ChangeDiff(1);
        }else if (Input.GetKeyDown(KeyCode.DownArrow) && getMusics.inputEnable)
        {
            ChangeDiff(-1);
        }
    }

    public void ChangeDiff(int count)
    {
        getMusics.selectedDifficulty = (Difficulty)Mathf.Clamp((int)getMusics.selectedDifficulty + count, 0, 2);
        // getMusics.ChangeText(getMusics.musics[getMusics.selector], getMusics.selectedDifficulty);
        var tmp = getMusics.selector;
        getMusics.DestroyAll();
        getMusics.obj.Clear();
        getMusics.selector = tmp;
        Debug.Log(tmp);
        getMusics.GetMusicsList();
        getMusics.SetImages(tmp-2, tmp + 3, 0);
        switch (getMusics.selectedDifficulty)
        {
            case Difficulty.HALL:
            {
                getMusics.camera.backgroundColor = new Color(188/255f, 255/255f, 188/255f);
                break;
            }
            case Difficulty.ARENA:
            {
                getMusics.camera.backgroundColor = new Color(178/255f, 178/255f, 255/255f);
                break;
            }
            case Difficulty.STUDIO:
            {
                getMusics.camera.backgroundColor = new Color(188/255f, 255/255f, 255/255f);
                break;
            }
            
        }
    }
    
    public void StartMusic()
    {
        string title = getMusics.musics[getMusics.selector];
        MusicData data = getMusics.musicDatas.First(x => x.musicParam.musicName == title);
        if (JsonClass.Instance.Loads(data.musicParam.id + "-" + getMusics.selectedDifficulty).isopen)
        {
            MusicManager.Instance.Id = data.musicParam.id;
            MusicManager.Instance.TrackName = title;
            MusicManager.Instance.ArtistName = data.musicParam.artist;
            MusicManager.Instance.Jacket = data.musicParam.jacket;
            MusicManager.Instance.Diff = getMusics.selectedDifficulty;
            MusicManager.Instance.IsSelected = true;
            SceneManager.LoadScene("UITestScene");
        }
    }
    
    #if UNITY_EDITOR

    public static void ResetJson()
    {
        List<MusicData> musicDatas = new List<MusicData>(Resources.LoadAll<MusicData>("MusicDatas"));
        foreach (MusicData musicData in musicDatas)
        {
            foreach (Difficulty difficulty in Enum.GetValues(typeof(Difficulty)))
            {
                JsonClass.Instance.Saves(musicData.musicParam.id + "-" + difficulty , 0, difficulty==Difficulty.HALL, false, Rank.B);
            }
        }
    }
    #endif
}
