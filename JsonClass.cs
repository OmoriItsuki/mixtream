using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[Serializable]
public class Json
{
    public string id;
    public int highScore;
    public bool isopen;
    public bool clear;
    public Rank rank;

    public Json(string id, int highScore, bool isopen, bool clear,Rank rank)
    {
        this.id = id;
        this.highScore = highScore;
        this.isopen = isopen;
        this.clear = clear;
        this.rank = rank;
    }

    public Json() {}
}

public class JsonClass : SingletonMonoBehaviour<JsonClass>
{
    
    /// <summary>
    /// Json書き込み用関数
    /// </summary>
    /// <param name="id">それぞれのメンバーを引数に必要</param>
    /// <param name="highScore"></param>
    /// <param name="isopen"></param>
    /// <param name="clear"></param>
    public void Saves(string id, int highScore, bool isopen, bool clear,Rank rank)
    {
        Json data = new Json(id, highScore, isopen, clear,rank);
        Debug.Log(data.id);
        SaveMethod(id+".json",data);
    }

    public void Saves(Json data)
    {
        SaveMethod(data.id+".json",data);
    }

    /// <summary>
    /// Json読み出し関数
    /// </summary>
    /// <param name="id">読み出しする曲のID</param>
    public Json Loads(string id)
    {
        Json data= new Json();
        LoadMethod(id+".json",data);
        return data;
    }
    

    /// <summary>
    /// 実際Jsonに変換して書き込む関数
    /// </summary>
    /// <param name="localPath">書き込むファイルのパス</param>
    /// <param name="data">書き込むインスタンス</param>
    public static void SaveMethod(string localPath, object data)
    {
        var json = JsonUtility.ToJson(data);
        var path = Application.dataPath + localPath;
        var writer = new StreamWriter(path, false);
        writer.WriteLine(json);
        writer.Flush();
        writer.Close();
    }

    /// <summary>
    /// 実際の読み出し関数
    /// </summary>
    /// <param name="localPath">読み出しデータのパス</param>
    /// <param name="data">読み出したデータを格納するオブジェクト</param>
    public static void LoadMethod(string localPath, object data)
    {
        if (!File.Exists(Application.dataPath + localPath))
        {
            Debug.LogError(localPath+"にjsonデータが存在しません。");
            return;
        }
        var info = new FileInfo(Application.dataPath + localPath);
        var reader = new StreamReader(info.OpenRead());
        var json = reader.ReadToEnd();
        JsonUtility.FromJsonOverwrite(json, data);
        reader.Close();
    }
}