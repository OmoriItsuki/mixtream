using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MusicImage : MonoBehaviour
{
    public Image img;
    private Sprite spt;

    [SerializeField] public GameObject obj;
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < 1000; i++)
        {
            Instantiate(obj);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChanegeImage(int t)
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            spt = Resources.Load<Sprite>("Jacket/PUPA");
            img = this.GetComponent<Image>();
            img.sprite = spt;
        }
    }
}
