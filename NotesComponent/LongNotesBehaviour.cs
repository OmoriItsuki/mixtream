using System.Collections;
using UnityEngine;

public class LongNotesBehaviour : NotesBehaviour
{
    [SerializeField] private Transform scalePivot;
    [SerializeField] private float timeLength;

    protected override IEnumerator Behave()
    {
        // 拡大開始
        Coroutine expand = StartCoroutine(Expand());
        // ノーツ流れ終わるまで待つ
        yield return Move();
        // 拡大が終わってなければ強制的に止まっているように見せる
        if (timeLength >= GameManager.FloatingTime)
        {
            StopCoroutine(expand);
            // 拡大と縮小が同時に起こっているために見かけ上のノーツは止まる
            yield return new WaitForSeconds(timeLength - GameManager.FloatingTime);
        }
        // 縮小
        yield return Contract();
        // ノーツ終了
        NotesGenerator.DeleteObject(gameObject);
    }

    private IEnumerator Move()
    {
        for (float time = 0; time < GameManager.FloatingTime; time += Time.deltaTime)
        {
            transform.position = Vector3.Lerp(StartPoint.position, EndPoint.position, time / GameManager.FloatingTime);
            yield return null;
        }
        transform.position = Vector3.Lerp(StartPoint.position, EndPoint.position, 1f);
        yield return null;
    }

    private IEnumerator Expand()
    {
        for (float time = 0; time < timeLength; time += Time.deltaTime)
        {
            scalePivot.localScale = new Vector3(1, 1, Mathf.Lerp(0, 100, time / GameManager.FloatingTime));
            yield return null;
        }
    }

    private IEnumerator Contract()
    {
        float range = Mathf.Min(timeLength, GameManager.FloatingTime);
        float maxScale = scalePivot.localScale.z;
        for (float time = 0; time < range; time += Time.deltaTime)
        {
            scalePivot.localScale = new Vector3(1, 1, Mathf.Lerp(maxScale, 0, time / range));
            yield return null;
        }
    }
    
    public override void SetDestination(Transform startPoint, Transform endPoint, float timeLength = 0)
    {
        this.timeLength = timeLength;
        base.SetDestination(startPoint, endPoint, timeLength);
    }
}
