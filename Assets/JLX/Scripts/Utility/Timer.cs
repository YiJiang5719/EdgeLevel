using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public interface ITimer
{
    void Update();
    void FixedUpdate();
}

public class Timer : ITimer
{
    public Action OnTimerStart;
    public Action<float, float> OnTimerUpdate;
    public Action<float, float> OnTimerFixedUpdate;
    public Action OnTimerEnd;
    public Action OnTimerInterrupt;
    public Action OnTimerStop;
    public Action OnTimerResume;

    [SerializeField]
    float maxTime = 0f;
    [SerializeField]
    float time = 0f;
    [SerializeField]
    bool isStart = false;
    [SerializeField]
    bool isStop = false;
    [SerializeField]
    bool loop = false;

    public bool IsStart
    {
        get
        {
            return isStart;
        }
    }

    public void Update()
    {
        if (IsStart && !isStop)
        {
            time -= Time.deltaTime;
            if (time > 0)
            {
                if (OnTimerUpdate!=null)
                    OnTimerUpdate(time, Time.deltaTime);
            }
            else
            {
                if (OnTimerEnd != null)
                    OnTimerEnd();
                isStart = false;
                if (loop)
                    StartTimer(maxTime, loop);
            }
        }
    }
    public void FixedUpdate()
    {
        if (IsStart && !isStop)
        {
            time -= Time.fixedDeltaTime;
            if (time > 0)
            {
                if (OnTimerFixedUpdate != null)
                    OnTimerFixedUpdate(time, Time.fixedDeltaTime);
            }
            else
            {
                if (OnTimerEnd != null)
                    OnTimerEnd();
                isStart = false;
                if (loop)
                    StartTimer(maxTime, loop);
            }
        }
    }

    public void StartTimer(float maxTime, bool loop)
    {
        if (IsStart)
            return;
        this.maxTime = maxTime;
        this.loop = loop;
        time = maxTime;
        isStart = true;
        isStop = false;
        if (OnTimerStart != null)
            OnTimerStart();
    }

    public void InterruptTimer()
    {
        if (IsStart)
        {
            isStart = false;
            if (OnTimerInterrupt != null)
                OnTimerInterrupt();
        }
    }

    public void ResumeTimer()
    {
        if (IsStart)
        {
            if (isStop)
            {
                isStop = false;
                if (OnTimerResume != null)
                    OnTimerResume();
            }
        }
    }

    public void StopTimer()
    {
        if (IsStart)
        {
            if (!isStop)
            {
                isStop = true;
                if (OnTimerStop != null)
                    OnTimerStop();
            }
        }
    }

    public void Reset()
    {
        isStart = false;
        isStop = false;
    }
}

