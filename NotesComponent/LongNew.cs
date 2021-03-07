using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LongNew : NotesBehaviour
{
    [SerializeField] private Transform start;
    [SerializeField] private Transform end;
    [SerializeField] private Transform fill;
    private float timeLength;
    private float defaultZDiff;
    private float defaultZScale;

    void Awake()
    {
        defaultZDiff = Mathf.Abs(end.position.z - start.position.z);
        defaultZScale = fill.localScale.z;
    }

    IEnumerator MovePart(Transform tf)
    {
        for (float time = 0; time < GameManager.FloatingTime; time += Time.deltaTime)
        {
            //Debug.Log(tf.name+" : "+Vector3.Lerp(StartPoint.position, EndPoint.position, time / GameManager.FloatingTime));
            tf.position = Vector3.Lerp(StartPoint.position, EndPoint.position, time / GameManager.FloatingTime);
            yield return null;
        }

        tf.position = EndPoint.position;
    }

    void FillRefresh()
    {
        var curDiff = Mathf.Abs(end.position.z - start.position.z);
        var fillScale = fill.localScale;
        fillScale.z = defaultZScale * (curDiff / defaultZDiff);

        fill.position = (start.position + end.position) / 2;
        fill.localScale = fillScale;
    }

    protected override IEnumerator Behave()
    {
        end.position = StartPoint.position;
        StartCoroutine(MovePart(start));
        yield return new WaitForSeconds(timeLength);
        yield return MovePart(end);
        yield return null;
        NotesGenerator.DeleteObject(gameObject);
    }

    public override void SetDestination(Transform startPoint, Transform endPoint, float timeLength = 0)
    {
        this.StartPoint = startPoint;
        this.EndPoint = endPoint;
        start.position = startPoint.position;
        end.position = startPoint.position;
        this.timeLength = timeLength;
        start.rotation = startPoint.rotation;
        end.rotation = startPoint.rotation;
        fill.rotation = startPoint.rotation;
        StartCoroutine(Behave());
    }

    void Update()
    {
        FillRefresh();
    }
}
