using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class ObjectiveDiplayer : MonoBehaviour
{
    public TextMeshProUGUI objectiveText;
    public CanvasGroup canvasGroup;
    public float fadeTime = 0.3f;

    public void UpdateMessage(string text)
    {
        objectiveText.SetText(text);
    }
    public void Show()
    {
        StartCoroutine(Fade(1));
    }
    public void Hide()
    {
        StartCoroutine(Fade(0));
    }

    IEnumerator Fade(float value)
    {
        float t = 0;
        float start = canvasGroup.alpha;
        while (t < fadeTime)
        {
            t += Time.deltaTime;
            float percent = t / fadeTime;
            canvasGroup.alpha = Mathf.Lerp(start, value, percent);
            yield return null;
        }

        canvasGroup.alpha = value;
    }

}
