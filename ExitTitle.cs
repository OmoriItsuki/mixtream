using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ExitTitle : MonoBehaviour
{
    public GameObject t;
    // Start is called before the first frame update

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
            #elif UNITY_STANDALONE
                UnityEngine.Application.Quit();
            #endif
        }else if(Input.anyKey)
        {
            SceneManager.LoadScene("MusicSelect");
        }
    }

    IEnumerator WaitInput()
    {
        yield return new WaitUntil((() => Input.anyKeyDown && !Input.GetKeyDown(KeyCode.Escape)));
        SceneManager.LoadScene("MusicSelect");
    }

    /*// Update is called once per frame
    void Update()
    {
        if (Input.anyKey)
        {
            SceneManager.LoadScene("MusicSelect");
        }
    }*/
}
