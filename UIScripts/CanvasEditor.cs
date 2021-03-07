using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasEditor : MonoBehaviour
{
    [SerializeField]public GameObject mainCanvas,PauseCanvas,ResultBaseCanvas,ResultCanvas;
    private bool isMainOn = false, isPauseOn = false, isResultOn = false;
        // Start is called before the first frame update
    void Start()
    {
        var main = mainCanvas.GetComponent<CanvasGroup>();
        var pause = PauseCanvas.GetComponent<CanvasGroup>();
        var resultBase = ResultBaseCanvas.GetComponent<CanvasGroup>();
        var result = ResultCanvas.GetComponent<CanvasGroup>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseCanvas.GetComponent<CanvasGroup>().alpha = 1;
            isPauseOn = true;
        }
    }

    public IEnumerator Easing()
    {
        yield return null;
    }
    
    public static float CubicInOut(float t, float totaltime, float min, float max)
    {
        max -= min;
        t /= totaltime/2;
        if (t < 1) return max / 2 * t * t * t + min;

        t = t - 2;
        return max / 2 * (t * t * t + 2) + min;
    }
}
