using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class UIAnimStarter : MonoBehaviour
{
    [SerializeField] List<GameObject> StartUpUI;      //再生したいAnimatorを持つオブジェクトを入れる

    // Start is called before the first frame update
    void Start()
    {

    }

    //アニメーション発火用の処理
    //Inspectorでリストに発火させたいUIをアタッチ
    //再生用のキーを渡して発火

    //キー一覧
    // "IN" →表示時のアニメーション
    // "OUT" →非表示時のアニメーション
    // "Highscore_IN" →ハイスコア時のアニメーション

    //利用例
    // launchUI.GetComponent<UIAnimStarter>().launchAnimation("IN", 0.2f);

    public void launchAnimationAll(string key, float delayTime)
    {
        Debug.Log(gameObject.name + "に設定されたアニメーションを再生します。");
        //リストに入れておいたオブジェクトのAnimatorコントローラーの該当キーを再生させる
        StartUpUI.ForEach(ui =>
        {
            Animator anim = ui.GetComponent<Animator>();

            StartCoroutine(Delay(delayTime, () =>
            {
                anim.SetTrigger(key);
            }, anim));

            Debug.Log("アニメーションを再生しました:" + anim.name);
        });
    }
    
    /// <summary>
    /// アニメーション再生用の関数
    /// </summary>
    /// <param name="animName">再生するアニメーションの名前</param>
    /// <param name="key">再生キー</param>
    /// <param name="delayTime">遅延(秒)</param>
    public void launchAnimation(string animName,string key, float delayTime)
    {
        Debug.Log(gameObject.name + "に設定されたアニメーションを再生します。");
        //リストに入れておいたオブジェクトのAnimatorコントローラーの該当キーを再生させる
        var anim = StartUpUI.Find(x => x.name == animName).GetComponent<Animator>();
        StartCoroutine(Delay(delayTime, () =>
        {
            anim.SetTrigger(key);
        }, anim));
        
    }


    //かんたん遅延処理
    //発火遅延時間と実行処理(Action)を引数で渡す
    public IEnumerator Delay(float waitTime, UnityAction Action, Animator animator)
    {
        yield return new WaitForSecondsRealtime(waitTime);
        Action();
    }
}
