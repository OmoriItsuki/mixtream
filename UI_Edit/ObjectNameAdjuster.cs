using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
public class ObjectNameAdjuster : MonoBehaviour
{
    [SerializeField] GameObject AdjustReferenceObj;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AdjustName()
    {
        Sprite targetSprite = GetComponent<Image>().sprite;
        name = targetSprite.name;
        Debug.Log("オブジェクトの名前をスプライト名で上書き");
    }

    public void AdjustSymmetry()
    {
        Vector2 AdjustPos = AdjustReferenceObj.GetComponent<RectTransform>().anchoredPosition;
        Vector2 OriginPos;

        OriginPos.x = -(AdjustPos.x);
        OriginPos.y = AdjustPos.y;

        GetComponent<RectTransform>().anchoredPosition = OriginPos;

        Debug.Log("オブジェクトを左右対称に配置");
    }



}
