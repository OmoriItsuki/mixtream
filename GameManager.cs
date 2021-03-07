using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;
using Assets.ScriptForTest;
using Effekseer;

public class GameManager : SingletonMonoBehaviour<GameManager>
{
    public enum InputType
    {
        JoyCon, Keyboard, AutoPlay
    }

    #region =========================== Nested Types ===================================

    [Serializable]
    public class TapEvaluationLevel
    {
        public Evaluation name;
        public float halfActiveRange;
    }

    #endregion ===========================================================================



    #region ====================== variables & properties ===========================

    [SerializeField] private InputType inputType;

    /// <summary>
    /// ノーツが流れ切るまでにかかる時間(秒)
    /// </summary>
    [SerializeField] private float floatingTime;
    public static float FloatingTime => Instance.floatingTime;

    [SerializeField] public int holdScoreIntervalTick;
    public int resolution = 480;

    /// <summary>
    /// ノーツタップの各評価の秒範囲(仮)必ず広い範囲のものから書くこと
    /// </summary>
    [SerializeField] private List<TapEvaluationLevel> evaluationLevels = new List<TapEvaluationLevel>();

    public bool isPause = false;
    private GameObject pauseC;

    /// <summary>
    /// 現在の再生時間(秒)
    /// </summary>
    //public static float CurrentTime => Instance.criAtomSource.time / 1000f;
    public static float CurrentTime
    {
        get
        {
            if (Instance.criAtomSource.time != 0)
                return (float)Instance.criAtomSource.time / 1000;
            else
                return Instance.negativeTime;
        }
    }

    public static List<Note> Notes { get; set; }

    /// <summary>
    /// 曲が流れる前の負の時間
    /// </summary>
    private float negativeTime;

    /// <summary>
    /// 各レーンにアタッチされたクラスのインスタンス
    /// </summary>
    [SerializeField] private List<Lane> lanes;

    /// <summary> オーディオソースコンポーネント </summary>
    [HideInInspector] public CriAtomSource criAtomSource;

    [HideInInspector] public CriAtom criAtom;
    
    /// <summary>
    /// エフェクト用コンポーネント変数
    /// </summary>
    [SerializeField] public EffekseerEmitter tapScript;
    public EffekseerEmitter longScript;


    static public EffekseerEmitter GetTapScript => Instance.tapScript;
    static public EffekseerEmitter GetLongScript => Instance.longScript;

     #endregion ==============================================================



    #region ======================= public methods =======================

    /// <summary> ステージの音楽データ、譜面データを読み込んでNotesGenerator, NotesAttackerを呼び出し </summary>
    /// <param name="musicName">曲名</param>
    /// <param name="difficulty">難易度</param>
    private void SetMusic(string musicName, string difficulty, InputType inputType)
    {
        //アニメーション再生
        GameObject.Find("MainCanvas").GetComponent<UIAnimStarter>().launchAnimationAll("IN",0f);
        //入力用コンポーネントを有効にする
        this.inputType = inputType;
        GetComponent<JoyconManager>().enabled = false;
        GetComponent<InputReceiver>().enabled = false;
        //GetComponent<AutoPlay>().enabled = false;
        GetComponent<KeyboardInput>().enabled = false;
        MusicData data = Resources.LoadAll<MusicData>("MusicDatas").First(x => x.musicParam.musicName == musicName);
        switch (inputType)
        {
            case InputType.JoyCon:
                GetComponent<JoyconManager>().enabled = true;
                GetComponent<InputReceiver>().enabled = true;
                break;
            case InputType.Keyboard:
                GetComponent<KeyboardInput>().enabled = true;
                break;
            case InputType.AutoPlay:
                GetComponent<AutoPlay>().enabled = true;
                break;
            default:
                Debug.LogError("Augment:inputType is invalid");
                return;
        }

        // 再生用コンポーネント取得
        criAtomSource = GetComponent<CriAtomSource>();

        // 音楽データ準備
        criAtomSource.cueName = data.musicParam.cueName;
        criAtomSource.cueSheet = data.musicParam.acbPath;

        // CSV譜面データ準備
        string line;
        List<Note> notes = new List<Note>();
        StringReader reader = new StringReader(((TextAsset) Resources.Load(data.musicParam.csvPath + "/" + difficulty)).text);
        
        // 1行目はTickPerBeat(一拍分のTick数)
        resolution = int.Parse(reader.ReadLine().Split(':')[1]);

        MusicManager.Instance.TheoryCombo = 0;
        
        // CSVを1行ずつ読み込んでNote型のリストに格納する
        while ((line = reader.ReadLine()) != null)
        {
            notes.Add(ConvertStringToNote(line));
        }

        // 昇順整列
        notes.Sort((a, b) => a.StartTime - b.StartTime > 0 ? 1 : -1);

        Notes = notes;
        

        // ノーツを流す処理の呼び出し
        StartCoroutine(NotesFall(notes));

        // タイマー開始、曲の開始
        StartCoroutine(PlayMusic());

        NowPlaying = true;
    }

    public static void ResumeMusic()
    {
        if(NowPlaying) return;

        NowPlaying = true;
        Time.timeScale = 1.0f;
        Instance.criAtomSource.Pause(false);
    }

    public static void PauseMusic()
    {
        if (!NowPlaying) return;

        NowPlaying = false;
        Time.timeScale = 0f;
        Instance.criAtomSource.Pause(true);
    }

    public static bool NowPlaying { get; private set; }
    public static CriAtomSource.Status musicStatus
    {
        get { return Instance.criAtomSource.status; }
    }
    
    /// <summary>
    /// ノーツからアクセスするためのプロパティ
    /// </summary>
    public static IReadOnlyList<TapEvaluationLevel> EvaluationRanges => Instance.evaluationLevels;

    public static void ChangeResumePause()
    {
        Instance.isPause = !Instance.isPause;
        if (Instance.isPause)
        {
            Instance.pauseC.GetComponent<UIAnimStarter>().launchAnimation("PauseUI","IN",0f);
            PauseMusic();
        }
        else
        {
            Instance.pauseC.GetComponent<UIAnimStarter>().launchAnimation("PauseUI","OUT",0f);
            ResumeMusic();
        }
    }

    #endregion ===========================================================
    
    
    
    #region ======================= private methods ===========================

    /// <summary>
    /// CSVの一行分を一つのNote型オブジェクトに変換
    /// </summary>
    private Note ConvertStringToNote(string parametersLine)
    {
        //[開始時間(秒), レーン(0~5), 種類(0~2), ノーツのtick長(int), ノーツのSec長(decimal)]
        string[] notesParameters = parametersLine.Split(',');
        
        var startTime = float.Parse(notesParameters[0]);
        var lane = lanes[(int)float.Parse(notesParameters[1])];
        var attribute = (NotesAttribute) (int)float.Parse(notesParameters[2]);
        var tickDuration = (int) float.Parse(notesParameters[3]);
        var secondDuration = float.Parse(notesParameters[4]);

        if (inputType == InputType.Keyboard)
        {
            attribute = NotesAttribute.Pad;
        }

        if (tickDuration != 0)
        {
            MusicManager.Instance.TheoryCombo += 1;
            return new LongNote(startTime, lane, attribute, tickDuration, secondDuration);
        }
        else
        {
            MusicManager.Instance.TheoryCombo += tickDuration / Instance.holdScoreIntervalTick;
            return new TapNote(startTime, lane, attribute);
        }
    }

    /// <summary>
    /// タイマーを起動し、 floatingTime経過後、曲を開始
    /// </summary>
    /// <returns></returns>
    private IEnumerator PlayMusic()
    {
        negativeTime = -FloatingTime;
        while (negativeTime < 0)
        {
            yield return null;
            negativeTime += Time.deltaTime;
        }

        criAtomSource.Play();
    }

    /// <summary>
    /// 譜面に従ってノーツが落ちてくる処理
    /// </summary>
    private IEnumerator NotesFall(IReadOnlyList<Note> notes)
    {
        yield return new WaitUntil(( )=>CurrentTime >= 0);
        for (int i = 0; i < notes.Count(); i++)
        {
            yield return new WaitUntil(()=>CurrentTime >= notes[i].StartTime - FloatingTime);
            notes[i].Generate();
        }
    }

    private void Start()
    {

        if (!MusicManager.Instance.IsSelected)
        {
            SetMusic("Twinkle Tone", "HALL", inputType);
        }
        else
        {
            SetMusic(MusicManager.Instance.TrackName, MusicManager.Instance.Diff.ToString(), inputType);
        }

        pauseC = GetComponent<CanvasChanger>().pauseC;

    }
    // private void Start() => SetMusic(GetMusics.name, "HALL", inputType);

    // テストプレイ用
    [SerializeField] private float ttt;
    private void Update()
    {
        ttt = CurrentTime;
        
    }
    
    
    #endregion ===============================================================
}