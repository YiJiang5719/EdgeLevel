using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public float ShakeDuration = 0.3f;          // Time the Camera Shake effect will last
    public float ShakeAmplitude = 1.2f;         // Cinemachine Noise Profile Parameter
    public float ShakeFrequency = 2.0f;         // Cinemachine Noise Profile Parameter

    // Cinemachine Shake
    public CinemachineVirtualCamera VirtualCamera;
    private CinemachineBasicMultiChannelPerlin virtualCameraNoise;
    private Timer shakeTimer = new Timer();

    // Use this for initialization
    void Start()
    {
        // Get Virtual Camera Noise Profile
        if (VirtualCamera != null)
            virtualCameraNoise = VirtualCamera.GetCinemachineComponent<Cinemachine.CinemachineBasicMultiChannelPerlin>();
        shakeTimer.OnTimerStart += ShakeStart;
        shakeTimer.OnTimerEnd += ShakeEnd;
    }

    void Update()
    {
        shakeTimer.Update();
    }

    public void Shake()
    {
        shakeTimer.StartTimer(ShakeDuration);
    }


    void ShakeStart()
    {
        virtualCameraNoise.m_AmplitudeGain = ShakeAmplitude;
        virtualCameraNoise.m_FrequencyGain = ShakeFrequency;
    }

    void ShakeEnd()
    {
        virtualCameraNoise.m_AmplitudeGain = 0f;
        virtualCameraNoise.m_FrequencyGain = 0f;
    }
}
