using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuSelector : MonoBehaviour
{
    public Color colorWhenSelected, defaultColor;
    public GameObject[] menus;
    public int[] scenesindex;
    private int menuSize, selectedIndex;
    // Start is called before the first frame update
    void Start()
    {
        ResetColor();
        menus.First().GetComponent<Image>().color = colorWhenSelected;
        menuSize = menus.Length;
        selectedIndex = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.isPause)
        {
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                ChangeArrow(1);
            }else if(Input.GetKeyDown(KeyCode.UpArrow))
            {
                ChangeArrow(-1);
            }else if (Input.GetKeyDown(KeyCode.Space))
            {
                switch (scenesindex[selectedIndex])
                {
                    case -2:
                        break;
                    case -1:
                        GameManager.ChangeResumePause();
                        break;
                    default:
                        GameManager.ChangeResumePause();
                        SceneManager.LoadScene(scenesindex[selectedIndex]);
                        break;
                }
    
                ChangeArrow(-selectedIndex);
            }
        }
        
    }

    void ChangeArrow(int diffValue)
    {
        selectedIndex = (selectedIndex + diffValue + menuSize) % menuSize;
        Debug.Log(selectedIndex);
        ResetColor();   
        menus[selectedIndex].GetComponent<Image>().color = colorWhenSelected;
    }

    void ResetColor()
    {
        foreach (GameObject menu in menus)
        {
            menu.GetComponent<Image>().color = defaultColor;
        }
    }
}
