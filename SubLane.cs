using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 判定の隠蔽、アタックの処理は基底クラスのコンポーネントに委託
/// ノーツの流し込み処理と移動先を与える処理、基底クラスコンポーネントとの中継を行う
/// </summary>
public class SubLane : Lane
{
    [SerializeField] private Lane parentLane;

    public override void HoldSet(NotesAttribute attribute, bool isHold) => parentLane.HoldSet(attribute, isHold);

    public override bool IsHoldActive(NotesAttribute attribute) => parentLane.IsHoldActive(attribute);

    public override void Attack(NotesAttribute attribute) { }

    public override void AddNote(Note note)
    {
        //Debug.Log(parentLane);
        parentLane.AddNote(note);
    }


    public override void Start()
    {
        base.Start();
    }
}
