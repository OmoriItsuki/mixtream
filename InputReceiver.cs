using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// テスト用
public class InputReceiver : SingletonMonoBehaviour<InputReceiver>
{
    [SerializeField] private Lane leftLane,rightLane;
    [SerializeField] private Joycon.Button lBtn,rBtn;

    private NotesAttribute Pad, Fader, knob;
    private Vector3 accelL, accelR, gyroL, gyroR;

    //private readonly Dictionary<KeyCorrespond, Lane> lanes = new Dictionary<KeyCorrespond, Lane>();
    List<Joycon> joycons;
    private Joycon joyconL;
    private Joycon joyconR;

    private void Start()
    {
        joycons = JoyconManager.Instance.j;
        joyconL = joycons.Find(c => c.isLeft);                                              
        joyconR = joycons.Find(c => !c.isLeft);
        Debug.Log(joycons);
    }

    // Update is called once per frame
    void Update()
    {
        //if (joyconL.GetButtonDown(lBtn)) leftLane.Attack(NotesAttribute.Pad);
        //leftLane.HoldSet(NotesAttribute.Pad, joyconL.GetButton(lBtn));

        if (joyconR.GetButtonDown(rBtn)) rightLane.Attack(NotesAttribute.Pad);
        rightLane.HoldSet(NotesAttribute.Pad, joyconR.GetButton(rBtn));

        //accelL = joyconL.GetAccel();
        //Debug.Log(accelL);
        //if (accelL.z > 0.5 && joyconL.GetButton(lBtn)) leftLane.Attack(NotesAttribute.Fader);
        //leftLane.HoldSet(NotesAttribute.Fader, joyconL.GetButton(lBtn));

        accelR = joyconR.GetAccel();
        //Debug.Log(accelR);
        if (joyconR.GetButton(rBtn) && accelR.z!=-1.0)
        {
            Debug.Log(accelR.z);
            rightLane.Attack(NotesAttribute.Fader);
        }
        leftLane.HoldSet(NotesAttribute.Fader, joyconR.GetButton(lBtn));

        //gyroL = joyconL.GetGyro();
        //gyroR = joyconL.GetGyro();
    }

    private void Check(Joycon j, Joycon.Button jB,Lane l, NotesAttribute n)
    {
        if (j.GetButtonDown(jB)) l.Attack(n);
        l.HoldSet(n, j.GetButton(jB));
    }
}