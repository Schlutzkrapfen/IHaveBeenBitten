using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public static CameraShake Instance { get; private set; }
	private float cameraShakeDelta;
	private float shakeTimerTotal;
	private float startingIntensity;
	private CinemachineVirtualCamera cinemachineVirtualCamera;
    // Start is called before the first frame update
    private void Awake()
    {
	    Instance = this;
	    cinemachineVirtualCamera = GetComponent<CinemachineVirtualCamera>();

    }
    // Update is called once per frame
    void Update()
    {
        	if (cameraShakeDelta > 0)
	        {
		        cameraShakeDelta -= Time.deltaTime;
		        if (cameraShakeDelta <= 0)
		        {
			        CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelPerlin =
				        cinemachineVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
			        cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = 
			        Mathf.Lerp(startingIntensity, 0f, cameraShakeDelta / shakeTimerTotal);

		        }
	        }
    }  
    public void ShakeCamera(float intensity, float time)
     	            {
     		            CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelPerlin = cinemachineVirtualCamera
     			            .GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
     		            cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = intensity;
     		            cameraShakeDelta = time;
	                    startingIntensity = intensity;
                    }
}
