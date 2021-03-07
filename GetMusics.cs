using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;

public static class Define
{
    //奇数にしてください
    public static readonly int NumberOfMusicPerPage = 5;
}

public enum Difficulty
{
    STUDIO=0,
    HALL,
    ARENA
}

public class GetMusics : MonoBehaviour
{
    public string[] musics;
    private Image imgs, mainImage;
    [SerializeField] private GameObject img;

    [SerializeField] private GameObject canv;

    //[SerializeField] private GameObject mainJacket;
    [SerializeField] private GameObject locked;
    private Sprite spt;
    public List<GameObject> obj = new List<GameObject>();
    [HideInInspector] public int selector, s, e;
    [SerializeField] private Text title, score, diff, artist, level, bpm;
    [SerializeField] private GameObject rank;
    [HideInInspector] public bool inputEnable = true;
    public float jacketSpeed = 1, holdSpeed = 10, centerJacketSize = 30;
    public Difficulty selectedDifficulty = Difficulty.HALL;
    public Camera camera;
    [HideInInspector]public List<MusicData> musicDatas;

    // Start is called before the first frame update
    void Start()
    {
        selector = Define.NumberOfMusicPerPage / 2 - 1;
        GetMusicsList();
        SetImages(-1, Define.NumberOfMusicPerPage - 1, 0);
    }


    /// <summary>
    /// ジャケットフォルダにある曲名から曲リストを作成
    /// </summary>
    /// <returns></returns>
    public void GetMusicsList()
    {
        musicDatas = new List<MusicData>(Resources.LoadAll<MusicData>("MusicDatas"));
            /*musics = musicsD.Select(x =>
        {
            string[] t = x.Split('-');
            Debug.Log(t.Take(t.Length-1).ToArray());
            return Path.GetFileNameWithoutExtension(String.Join("-", t.Take(t.Length-1).ToArray()));
        }).ToArray();*/
            musics = musicDatas.Select(x => x.musicParam.musicName).ToArray();
    }

    /// <summary>
    /// ジャケットリストを表示し、メイン画面に情報を表示
    /// </summary>
    public void SetImages(int start, int end, int index)
    {
        s = (start + musics.Length) % musics.Length;
        e = (end + musics.Length) % musics.Length;
        // Debug.Log("s:"+s+" e:"+e);
        for (int i = s; i != e; i = (i + 1) % musics.Length)
        {
            ChangeImages(musics[i], index);
            index++;
        }

        ChangeText(musics[selector], selectedDifficulty);
    }

    /// <summary>
    /// 画像の枠を生成して、そこにジャケット画像を張り付ける
    /// </summary>
    /// <param name="title">曲名</param>
    /// <param name="index">何番目に配置するか(最左が0)</param>
    void ChangeImages(string title, int index, bool left = false)
    {
        obj.Insert(index,
            Instantiate(img,
                new Vector3(-(50 * (Define.NumberOfMusicPerPage / 2)) + 50 * (left ? index - 1 : index),
                    canv.transform.position.y +
                    ((index == Define.NumberOfMusicPerPage / 2 ) ? centerJacketSize : 0),
                    canv.transform.position.z),
                Quaternion.Euler(90, 0, 0), canv.transform));
        if (index >= Define.NumberOfMusicPerPage / 2)
        {
            obj[Define.NumberOfMusicPerPage / 2].transform.SetAsLastSibling();
        }
        obj[index].transform.localScale = new Vector3(2, 2, 2);
        spt = musicDatas.Where(x => x.musicParam.musicName == title).Select(x => x.musicParam.jacket).ToArray().First();
        imgs = obj[index].GetComponent<Image>();
        imgs.sprite = spt;
        LockMusic(title, selectedDifficulty, obj[index]);
    }


    /// <summary>
    /// イージングによりジャケットを移動する
    /// </summary>
    /// <param name="lr">左へ動かす:true.右へ動かす:false</param>
    /// <returns></returns>
    public IEnumerator movingJacket(bool lr, float speed = 1)
    {
        inputEnable = false;
        var stratPos = new Vector3(canv.transform.position.x, canv.transform.position.y, canv.transform.position.z);
        var endPos = new Vector3((lr ? canv.transform.position.x - 50 : canv.transform.position.x + 50),
            canv.transform.position.y, canv.transform.position.z);
        for (float time = 0; time < 1; time += Time.deltaTime * speed)
        {
            var nakano = CubicOut(time, 1, 0, 1);
            canv.transform.position = Vector3.Lerp(stratPos, endPos, nakano);
//            Debug.Log(nakano);
            yield return null;
        }

        inputEnable = true;
    }

    
    /// <summary>
    /// 中心のジャケットを大きく表示する処理
    /// </summary>
    /// <param name="beBigObj">大きくするオブジェクト</param>
    /// <param name="beSmallObj">小さくするオブジェクト(もともと大きかったオブジェクト)</param>
    /// <param name="isLeft">左に行くかどうか</param>
    /// <param name="isHold">長押しされているかどうか</param>
    /// <returns></returns>
    public IEnumerator Center(GameObject beBigObj,GameObject beSmallObj,bool isLeft,bool isHold)
    {
        var tmpPos = canv.transform.parent.transform.position;
        var bigStartPos = beBigObj.transform.position;
        var bigEndPos = new Vector3(tmpPos.x, tmpPos.y + centerJacketSize, tmpPos.z);
        var smallStartPos = beSmallObj.transform.position;
        var smallEndPos = new Vector3(bigEndPos.x + (isLeft ? 50 : -50), bigEndPos.y - centerJacketSize, bigEndPos.z);
        for (float time = 0; time < 1; time += Time.deltaTime * (isHold ? holdSpeed : jacketSpeed)) 
        {
            beBigObj.transform.SetAsLastSibling();
            var nakano = CubicOut(time, 1, 0, 1);
            beBigObj.transform.position = Vector3.Lerp(bigStartPos, bigEndPos, nakano);
            beSmallObj.transform.position = Vector3.Lerp(smallStartPos, smallEndPos, nakano);
            yield return null;
        }
    }
    

    /// <summary>
    /// ジャケットの移動に必要な処理
    /// </summary>
    /// <param name="isLeft">左への移動であるか</param>
    /// <param name="isHold">長押しの処理であるか</param>
    public void MoveJacket(bool isLeft, bool isHold)
    {
        if (isLeft)
        {
            s = (s - 1 + musics.Length) % musics.Length;
            e = (e - 1 + musics.Length) % musics.Length;
        }

        selector = isLeft
            ? ((selector - 1 + musics.Length) % musics.Length)
            : ((selector + 1 + musics.Length) % musics.Length);
        //ここにイージング処理
        StartCoroutine(movingJacket(!isLeft, isHold ? holdSpeed : jacketSpeed));
        StartCoroutine(Center(obj[(Define.NumberOfMusicPerPage / 2) + (isLeft ? -1 : 1)],
            obj[(Define.NumberOfMusicPerPage / 2)], isLeft, isHold));
        ChangeImages(musics[isLeft ? s : e], isLeft ? 0 : Define.NumberOfMusicPerPage, isLeft);
        Destroy(isLeft ? obj.Last() : obj.First());
        obj.Remove(isLeft ? obj.Last() : obj.First());
        ChangeText(musics[selector], selectedDifficulty);
        if (!isLeft)
        {
            e = (e + 1) % musics.Length;
            s = (s + 1) % musics.Length;
        }
    }

    /// <summary>
    /// メイン画面のテキスト情報書き換え
    /// </summary>
    /// <param name="id">曲名</param>
    /// <param name="diffic">難易度</param>
    public void ChangeText(string musicName, Difficulty diff)
    {
        //Debug.Log(id);
        Json data = JsonClass.Instance.Loads(musicDatas.First(x => x.musicParam.musicName == musicName).musicParam.id + "-" + diff);
        MusicData md = musicDatas.First(x => x.musicParam.musicName == musicName);
        
        //Debug.Log(id);
        name = musicName;
        title.text = musicName;
        score.text = data.highScore.ToString();
        this.diff.text = diff.ToString();
        artist.text = md.musicParam.artist;
        bpm.text = md.musicParam.bpm.ToString();
        switch (diff)
        {
            case Difficulty.HALL:
                level.text = md.musicParam.hallLevel;
                break;
            case Difficulty.ARENA:
                level.text = md.musicParam.arinaLevel;
                break;
            case Difficulty.STUDIO:
                level.text = md.musicParam.studioLevel;
                break;
        }

        foreach (Transform go in rank.transform)
        {
            go.gameObject.SetActive(false);
        }
        rank.transform.GetChild((int)data.rank).gameObject.SetActive(true);
    }
    
    

    /// <summary>
    /// 画像を暗くして、南京錠のイラストをオーバーライドする
    /// </summary>
    /// <param name="obj">対象のジャケット</param>
    public void LockMusic(string id, Difficulty diff, GameObject obj)
    {
        Debug.Log(System.IO.Directory.GetCurrentDirectory());
        if (!JsonClass.Instance.Loads(musicDatas.First(x => x.musicParam.musicName == id).musicParam.id + "-" + diff).isopen)
        {
            Image img = obj.GetComponent<Image>();
            img.color = new Color(121 / 255f, 121 / 255f, 121 / 255f);
            GameObject locking =
                Instantiate(locked, new Vector3(0, 10, -10), Quaternion.Euler(90, 0, 0), obj.transform);
            locking.GetComponent<RectTransform>().sizeDelta =
                obj.GetComponent<RectTransform>().sizeDelta * new Vector2(0.8f, 0.8f);
            locking.GetComponent<RectTransform>().position = obj.GetComponent<RectTransform>().position;
        }
    }

    public void DestroyAll()
    {
        for (int i = 0; i < obj.Count; i++)
        {
            Destroy(obj[i]);
        }
    }

    /// <summary>
    /// イージング処理用の数値を返す関数
    /// </summary>
    /// <param name="t"></param>
    /// <param name="totaltime"></param>
    /// <param name="min"></param>
    /// <param name="max"></param>
    /// <returns></returns>
    public static float CubicOut(float t, float totaltime, float min, float max)
    {
        max -= min;
        t = t / totaltime - 1;
        return max * (t * t * t + 1) + min;
    }
}