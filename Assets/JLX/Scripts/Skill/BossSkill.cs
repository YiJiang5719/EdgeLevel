using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSkill : MonoBehaviour
{
    public GameObject wind;

    public float minScale = 1f;
    public float maxScale = 5f;

    public float lastTime = 5f;
    public float waitTime = 5f;

    Timer lastTimer = new Timer();
    Timer waitTimer = new Timer();

    void Awake()
    {
        lastTimer.OnTimerStart += Show;
        lastTimer.OnTimerUpdate += Scale;
        lastTimer.OnTimerEnd += Hide;
        waitTimer.OnTimerEnd += WaitFinish;
    }

    private void OnEnable()
    {
        lastTimer.StartTimer(lastTime);
    }

    private void Update()
    {
        lastTimer.Update();
        waitTimer.Update();
    }

    // Update is called once per frame
    void Scale(float time, float maxTime)
    {
        var scaler = time / maxTime;
        var scale = minScale + (maxScale - minScale) * scaler;
        wind.transform.localScale = Vector3.one * scale;
    }

    void Show()
    {
        wind.transform.localScale = Vector3.one;
        wind.gameObject.SetActive(true);
    }

    void Hide()
    {
        wind.gameObject.SetActive(false);
        waitTimer.StartTimer(waitTime, false);
    }

    void WaitFinish()
    {
        lastTimer.StartTimer(lastTime);
    }
}
