using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreWriter : MonoBehaviour
{
    [SerializeField] private string countEvaluation;

    private Text text;
    
    private void Start()
    {
        text = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        text.text = ScoreManager.Instance.scoreCounter[(Evaluation)Enum.Parse(typeof(Evaluation), countEvaluation)].ToString();
    }
}
