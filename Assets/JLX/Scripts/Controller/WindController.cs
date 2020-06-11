using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class WindController : MonoBehaviour
{
    [Range(0.5f, 1f)]
    public float maxScaler;
    [Range(0.2f, 0.8f)]
    public float minScaler;
    [Range(0.1f, 1f)]
    public float scalerChangeTime=1f;
    [Range(0.1f, 5f)]
    public float scalerUpdateTime = 1f;

    PlayerController pc;
    [SerializeField]
    float speedScaler = 1f;
    // Start is called before the first frame update
    void Start()
    {
        pc = FindObjectOfType<PlayerController>();
        var seq = DOTween.Sequence().SetDelay(scalerUpdateTime).SetLoops(-1);
        seq.onStepComplete += UpdateSpeedScaler;
    }

    // Update is called once per frame
    void Update()
    {
        pc.windScaler = speedScaler;
    }

    void UpdateSpeedScaler()
    {
        //Debug.Log("UpdateSpeedScaler");
        var newScaler = Random.Range(minScaler, maxScaler); ;
        DOTween.To(() => speedScaler, (c) => speedScaler = c, newScaler, scalerChangeTime);
    }
}
