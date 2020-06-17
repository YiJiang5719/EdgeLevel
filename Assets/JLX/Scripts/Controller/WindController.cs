using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindController : MonoBehaviour
{
    public float windSpeedScaler = 0.6f;
    public float cdTime = 3f;

    float originalSpeedScaler = 1f;
    PlayerController pc;
    Timer cdTimer;

    // Start is called before the first frame update
    void Start()
    {
        pc = FindObjectOfType<PlayerController>();
        cdTimer = new Timer();
    }

    private void Update()
    {
        cdTimer.Update();
    }

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("OnTriggerEnter");
        pc.windScaler = windSpeedScaler;
        if (!cdTimer.IsStart)
        {
            //Debug.Log("OnTriggerEnter cdTimer");
            pc.WindHit(transform.position);
            cdTimer.StartTimer(cdTime, false);
        }
    }

    private void OnTriggerStay(Collider other)
    {
    }

    private void OnTriggerExit(Collider other)
    {
        //Debug.Log("OnTriggerExit");
        pc.windScaler = originalSpeedScaler;
    }
}
