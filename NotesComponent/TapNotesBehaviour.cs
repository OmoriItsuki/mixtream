using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TapNotesBehaviour : NotesBehaviour
{
    protected override IEnumerator Behave()
    {
        yield break;
    }

    public override void SetDestination(Transform startPoint, Transform endPoint, float timeLength = 0)
    {
        base.SetDestination(startPoint, endPoint, timeLength);
        this.GetComponent<Transform>().rotation = Quaternion.Euler(startPoint.rotation.x - 30,0,0);
    }
}
