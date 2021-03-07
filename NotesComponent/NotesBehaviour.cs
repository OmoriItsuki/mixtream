using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

/// <summary>
/// ノーツにアタッチされ、ノーツの動きのみを扱う。
/// </summary>
public abstract class NotesBehaviour : MonoBehaviour
{
    protected Transform StartPoint;
    protected Transform EndPoint;

    /// <summary> ノーツの実際の動き </summary>
    protected abstract IEnumerator Behave();

    /// <summary> 生成時、ノーツの移動先を与える </summary>
    public virtual void SetDestination(Transform startPoint, Transform endPoint, float timeLength = 0)
    {
        this.StartPoint = startPoint;
        this.EndPoint = endPoint;
        this.GetComponent<Transform>().rotation = startPoint.rotation;
        StartCoroutine(Behave());
    }
}
