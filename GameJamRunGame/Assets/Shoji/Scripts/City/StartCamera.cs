using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class StartCamera : MonoBehaviour
{
    [SerializeField]
    float startCameraTime = 5;
    [SerializeField]
    CinemachineVirtualCamera vCamera;
    [SerializeField]
    PlayerInput playerInput;
    private void Awake()
    {
        StartCoroutine(StartCameraWait());
    }
    IEnumerator StartCameraWait()
    {
        while (startCameraTime > 0)
        {
            startCameraTime -= Time.deltaTime;
            transform.Rotate(0, 30 * Time.deltaTime, 0);
            yield return null;
        }
        vCamera.Priority = 0;
        yield return new WaitForSeconds(2);
        playerInput.BeginInput();
        ResultManager.StartTimer();
    }
}
