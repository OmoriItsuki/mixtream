using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using Effekseer;

///<summary>
/// ノーツの種類
/// </summary>
public enum NotesAttribute
{
    Knob,
    Fader,
    Pad
}

///<summary>
/// CSVから読み込んだ譜面をこれのListとして格納する
/// </summary>
public abstract class Note
{
    #region ==================== variables ===========================

    /// <summary>
    /// ノーツのライン到達時間(秒)
    /// </summary>
    public float StartTime { get; private set; }
    
    /// <summary>
    /// レーン(0~5)
    /// </summary>
    public Lane FloatingLane { get; private set; }
    
    /// <summary>
    /// ノーツの種類
    /// </summary>
    public NotesAttribute Attribute { get; private set; }

    /// <summary>
    /// タップかホールドか
    /// </summary>
    public abstract TapStyle TapStyle { get; }

    /// <summary>
    /// ノーツのコンポーネント
    /// </summary>
    private NotesBehaviour component;

    #endregion ======================================================



    #region ===================== public methods ===========================

    /// <summary>
    /// 一つのノーツ
    /// </summary>
    /// <param name="startTime">ノーツのライン到達時間(秒)</param>
    /// <param name="lane">レーン(0~5)</param>
    /// <param name="attribute">ノーツの種類</param>
    protected Note(float startTime, Lane lane, NotesAttribute attribute)
    {
        this.StartTime = startTime;
        this.FloatingLane = lane;
        this.Attribute = attribute;
    }

    /// <summary>
    /// 
    /// </summary>
    public virtual void Generate()
    {
        var behaviour = NotesGenerator.CreateObject(TapStyle, Attribute);
        //Debug.Log(behaviour);
        var gptf = FloatingLane.generatePointTransform;
        var aptf = FloatingLane.attackPointTransform;

        behaviour.SetDestination(gptf, aptf);
        FloatingLane.AddNote(this);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns>ノーツが評価された場合(タップ失敗or成功の範囲内)ならtrue</returns>
    public virtual Evaluation Attack()
    {
        var evaluation = Evaluation.None;
        foreach (var level in GameManager.EvaluationRanges)
            if (Mathf.Abs(GameManager.CurrentTime - StartTime) < level.halfActiveRange)
                evaluation = level.name;
            else
                break;
        if (evaluation != Evaluation.None)
            ScoreManager.AddScore(evaluation, Attribute, TapStyle);
        if (evaluation == Evaluation.Good)
        {
            PlayEffect(evaluation);
        }
        return evaluation;
    }
    
    /// <summary>
    /// エフェクトの再生
    /// </summary>
    private void PlayEffect(Evaluation e)
    {
        //Debug.Log("effectAsset : " + script.effectAsset.name + " position : " + attackPointTransform.position);
        EffekseerHandle handle;
        Vector3 pos = FloatingLane.attackPointTransform.position;
        pos.y += 0.1f;
        if (TapStyle == TapStyle.Hold)
        {
            handle = EffekseerSystem.PlayEffect(GameManager.GetLongScript.effectAsset, pos);
        }
        else if(TapStyle == TapStyle.Tap)
        {
            handle = EffekseerSystem.PlayEffect(GameManager.GetTapScript.effectAsset, pos);
        }
        else
        {
            return;
        }
        //エフェクトをランダムな色に変える謎エフェクト
        handle.SetAllColor(new Color(Random.Range(0.3f,0.8f), Random.Range(0.3f,0.8f), Random.Range(0.3f,0.8f), 1f));
        handle.SetRotation(Quaternion.AngleAxis(70,new Vector3(1,0,0)));
        handle.paused = true;
        handle.speed = 5;
        handle.paused = false;
    }

    #endregion ============================================================
}
