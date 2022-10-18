using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;


public class Timer : MonoBehaviour
{

    [Header("Time UI reference :")]
    [SerializeField] private Image UiFillImage;
    [SerializeField] private TMP_Text UiText;
    float baseTime = 60 * 5;


    public float Duration;

    private float remaningDuration;


    private void Awake()
    {
        resetTimer();

    }

    private void resetTimer()
    {
        UiText.text = "00:00";
        UiFillImage.fillAmount = 0f;

        Duration = remaningDuration = baseTime;
    }

    public Timer SetDuration(int secounds)
    {
        Duration = remaningDuration = secounds;
        return this;
    }

    public void Begin()
    {
        StopAllCoroutines();
        StartCoroutine(UpdateTimer());
    }

    private IEnumerator UpdateTimer()
    {
        while(remaningDuration > 0)
        {
            UpdateUI(remaningDuration);
            remaningDuration--;
            yield return new WaitForSeconds(1f);
        }
        End();

    }

    private void UpdateUI(float secounds)
    {
        UiText.text = string.Format("{0:D2}:{1:D2}", secounds / baseTime, secounds % baseTime);
        UiFillImage.fillAmount = Mathf.InverseLerp(0, Duration, secounds);
    }

    public void End()
    {
        resetTimer();
    }

    public void OnDestroy()
    {
        StopAllCoroutines();
    }


}
