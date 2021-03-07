using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyboardInput : MonoBehaviour
{
    [SerializeField] private KeyCode leftKey;
    [SerializeField] private Lane leftLane;
    [SerializeField] private KeyCode rightKey;
    [SerializeField] private Lane rightLane;

    private void KeyCheck(KeyCode key, Lane lane)
    {
        if (Input.GetKeyDown(key))
        {
            // lane.Attack(NotesAttribute.Fader);
            // lane.Attack(NotesAttribute.Knob);
            lane.Attack(NotesAttribute.Pad);
        }
        // lane.HoldSet(NotesAttribute.Fader, Input.GetKey(key));
        // lane.HoldSet(NotesAttribute.Knob, Input.GetKey(key));
        lane.HoldSet(NotesAttribute.Pad, Input.GetKey(key));
    }

    // Update is called once per frame
    void Update()
    {
        KeyCheck(leftKey, leftLane);
        KeyCheck(rightKey, rightLane);
        /*if (Input.GetKeyDown(KeyCode.P))
        {
            GameManager.PauseMusic();
        }
        else if (Input.GetKeyDown(KeyCode.R))
        {
            GameManager.ResumeMusic();
        }*/
    }

}
