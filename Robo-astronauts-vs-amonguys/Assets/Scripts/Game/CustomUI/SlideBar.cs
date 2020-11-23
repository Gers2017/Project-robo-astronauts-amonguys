using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;

namespace CustomUI
{
    public class SlideBar : MonoBehaviour
    {
        [SerializeField] Gradient barGradient;
        [SerializeField] Image barImage;
        [SerializeField] float slideTime = 5f;
        public OnValueChangeEvent OnValueChange;
        void SetBarFillAmount(float value)
        {
            value = Mathf.Clamp(value, 0, 1f);
            barImage.fillAmount = value;
            OnValueChange.Invoke(value);
        }
        public void SetStartValue(float startPercentage)
        {
            SetBarFillAmount(startPercentage);
        }
        public void SetBarValue(float percentage)
        {
            StartCoroutine(UpdateBarValue(percentage));
        }

        IEnumerator UpdateBarValue(float percentage)
        {
            float startAmount = barImage.fillAmount;
            float timeElapsed = 0f;

            while (timeElapsed < slideTime)
            {
                timeElapsed += Time.deltaTime;
                float time = timeElapsed / slideTime;
                barImage.fillAmount = Mathf.Lerp(startAmount, percentage, time);
                yield return null;
            }
            barImage.color = barGradient.Evaluate(percentage);
            barImage.fillAmount = percentage;
        }
        void OnDisable()
        {
            StopAllCoroutines();
        }
    }
    [System.Serializable]
    public class OnValueChangeEvent : UnityEvent<float>{}

}
