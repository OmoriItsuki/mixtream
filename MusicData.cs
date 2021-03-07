using UnityEngine;

[CreateAssetMenu(fileName = "MusicData", menuName = "ScriptableObject/CreateMusicData")]
public class MusicData : ScriptableObject
{
    public MusicParam musicParam;

    [System.Serializable]
    public class MusicParam
    {
        public string id;
        public string musicName;
        public Sprite jacket;
        public string artist;
        public string acbPath;
        public string csvPath;
        public string cueName;
        public float bpm;
        public string arinaLevel;
        public string hallLevel;
        public string studioLevel;
    }

}