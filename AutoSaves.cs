using System;
using System.IO;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace ScriptForTest
{
    public class AutoSaves : MonoBehaviour
    {
        
        /*void start()
        {
            string path = System.IO.Directory.GetCurrentDirectory() + "/Resources/CSV/";
            string[] folders = Directory.GetFiles (path, "*.meta",SearchOption.TopDirectoryOnly );
            if (folders.Length == 0)
            {
                Debug.LogError("譜面データが存在しません。");
            }
            string oldFolderName = "CSV/";
            foreach (var folder in folders)
            {
                string folderName = Path.GetFileNameWithoutExtension(folder);
                //Debug.Log(path);
                path = path.Replace(oldFolderName, "CSV/"+folderName+"/");
                //Debug.Log(oldFolderName+ " "+ folderName+ " "+ path);
                string[] files = Directory.GetFiles (path, "*.csv",SearchOption.TopDirectoryOnly );
                foreach (var file in files)
                {
                    //Debug.Log(file);
                    string fileName = Path.GetFileNameWithoutExtension(file);
                    if (!File.Exists(System.IO.Directory.GetCurrentDirectory() + "/JsonData/" + folderName + "-" + fileName + ".json"))
                    {
                        JsonClass.Instance.Saves(folderName + "-" + fileName, 0, false, false, Rank.B);
                    }
                }
                oldFolderName = "CSV/"+folderName+"/";
            }
        }*/


        /// <summary>
        /// もし譜面データは存在しているがハイスコア等のクライアントデータが存在しない場合に自動で生成する
        /// </summary>
        public void Awake()
        {
            //登録されている譜面データの取得
            List<MusicData> musicDatas = new List<MusicData>(Resources.LoadAll<MusicData>("MusicDatas"));
            foreach (MusicData musicData in musicDatas)
            {
                foreach (Difficulty difficulty in Enum.GetValues(typeof(Difficulty)))
                {
                    // Debug.Log(System.IO.Directory.GetCurrentDirectory() + "/JsonData/" + musicData.musicParam.id + "-" + difficulty + ".json");
                    // if (!File.Exists(System.IO.Directory.GetCurrentDirectory() + "/Resources/JsonData/" + musicData.musicParam.id + "-" + difficulty + ".json"))
                    //{
                        JsonClass.Instance.Saves(musicData.musicParam.id + "-" + difficulty , 0, true, false, Rank.B);
                    //}
                }
            }
            
        }
    }
}