using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIColorControler : MonoBehaviour
{
    [SerializeField] Color colorCode;                     //インスペクタでカラーコードを指定
    [SerializeField] List<GameObject> colorChangeUI;      //再生したいAnimatorを持つオブジェクトを入れる


    // Start is called before the first frame update
    void Start()
    {
        SetUIColor();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void SetUIColor()
    {
        //リストに入れておいたオブジェクトのimage内のカラーを変更
        colorChangeUI.ForEach(ui =>
        {
            Image uiImage = ui.GetComponent<Image>();

            uiImage.color = colorCode;

            Debug.Log(ui.name + "の色変更完了" + ui.name);
        });
    }

}
