using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : SingletonMonoBehaviour<MusicManager>
{
    private string ID;
    private string TRACK_NAME;
    private string ARTIST_NAME;
    private Sprite JACKET;
    private Difficulty DIFF;
    private string LEVEL;
    private int THEORY_COMBO = 0;
    private int THEORY_SCORE;
    private bool isSelected = false;

    public string Id
    {
        get => Instance.ID;
        set => Instance.ID = value;
    }

    public string TrackName
    {
        get => Instance.TRACK_NAME;
        set => Instance.TRACK_NAME = value;
    }

    public string ArtistName
    {
        get => Instance.ARTIST_NAME;
        set => Instance.ARTIST_NAME = value;
    }

    public Sprite Jacket
    {
        get => Instance.JACKET;
        set => Instance.JACKET = value;
    }

    public Difficulty Diff
    {
        get => Instance.DIFF;
        set => Instance.DIFF = value;
    }

    public string Level
    {
        get => LEVEL;
        set => LEVEL = value;
    }

    public int TheoryCombo
    {
        get => THEORY_COMBO;
        set => THEORY_COMBO = value;
    }

    public int TheoryScore
    {
        get => THEORY_SCORE;
        set => THEORY_SCORE = value;
    }

    public bool IsSelected
    {
        get => Instance.isSelected;
        set => Instance.isSelected = value;
    }

    public void Start()
    {
        DontDestroyOnLoad(this);
    }
}
