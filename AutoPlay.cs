using System.Collections;
using UnityEngine;

namespace Assets.ScriptForTest
{
    public class AutoPlay : MonoBehaviour
    {
        private void Start()
        {
            Debug.Log("AutoPlay: enable");
            StartCoroutine(Play());
        }

        private IEnumerator Play()
        {
            foreach (var note in GameManager.Notes)
            {
                yield return new WaitUntil(()=>GameManager.CurrentTime > 0 && GameManager.CurrentTime > note.StartTime);
                Debug.Log("tap time:"+ GameManager.CurrentTime + " / note time:" + note.StartTime);
                note.FloatingLane.Attack(note.Attribute);
                if (note.GetType() == typeof(LongNote))
                {
                    StartCoroutine(Hold((LongNote)note));
                }
            }
        }

        private IEnumerator Hold(LongNote note)
        {
            note.FloatingLane.HoldSet(note.Attribute, true);
            yield return new WaitForSeconds(note.SecondDuration);
            note.FloatingLane.HoldSet(note.Attribute, false);
        }
    }
}
