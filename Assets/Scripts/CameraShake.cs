using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraShake : MonoBehaviour{
    
    public static CameraShake instance {get; private set;}
    private CinemachineBasicMultiChannelPerlin noise;
    private float shakeTimer = 0f;

    void Awake(){
        instance = this;
        CinemachineVirtualCamera v_cam = GetComponent<CinemachineVirtualCamera>();
        noise = v_cam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }

    public void ShakeCamera(float intensity){
        noise.m_AmplitudeGain = intensity;
        shakeTimer = 0.4f;
    }

    void Update(){
        if(shakeTimer < 0f) return;
        shakeTimer -= Time.deltaTime;
        if(shakeTimer < 0f) noise.m_AmplitudeGain = 0f;
    }
}
