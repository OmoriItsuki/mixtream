using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TapNew : NotesBehaviour
{
    protected override IEnumerator Behave()
    {
        for (float time = 0; time < GameManager.FloatingTime; time += Time.deltaTime)
        {
            transform.position = Vector3.Lerp(StartPoint.position, EndPoint.position, time / GameManager.FloatingTime);
            yield return null;
        }

        transform.position = Vector3.Lerp(StartPoint.position, EndPoint.position, 1f);
        yield return null;
        NotesGenerator.DeleteObject(gameObject);
    }

    public override void SetDestination(Transform startPoint, Transform endPoint, float timeLength = 0)
    {
        // this.StartPoint = startPoint;
        // this.EndPoint = endPoint;
        // StartCoroutine(Behave());
        base.SetDestination(startPoint, endPoint, timeLength);
        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles + new Vector3(30, 180, 0));
    }
}
