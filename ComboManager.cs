using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ComboManager : MonoBehaviour
{
    private static Text text;
    [HideInInspector]public static int combo;

    [HideInInspector] public static int maxCombo;
    // Start is called before the first frame update
    void Start()
    {
        combo = 0;
        maxCombo = 0;
        text = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static void ComboAdd(Evaluation e)
    {
        if ((int)e > (int) Evaluation.Miss)
        {
            combo++;
            if (combo > maxCombo) maxCombo = combo;
        }else if ((int) e == (int) Evaluation.None) {
        }else if ((int)e == (int)Evaluation.Miss) {
            combo = 0;
        }
        else
        {
            combo = 0;
            Debug.LogError("Combo Error");
        }
        ComboSave();
    }

    private static void ComboSave()
    {
        text.text = combo.ToString();
    }
}
