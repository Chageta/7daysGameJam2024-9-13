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

    static bool firstTime = true;
    [SerializeField]
    GameObject[] tutorialWindows;

    [SerializeField]
    AudioSource source;
    [SerializeField]
    AudioClip confirmSE;

    [SerializeField]
    CanvasGroup[] startHideCanvas;

    private void Awake()
    {
        if (firstTime) StartCoroutine(FirstTimeTutorial());
        StartCoroutine(StartCameraWait());
        foreach (var canvas in startHideCanvas)
            canvas.alpha = 0;
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
        yield return new WaitForSeconds(1);
        float timer = 0;
        while (timer < 1)
        {
            timer += Time.deltaTime;
            foreach (var canvas in startHideCanvas)
                canvas.alpha = timer;
            yield return null;
        }
        playerInput.BeginInput();
        ResultManager.StartTimer();
    }
    IEnumerator FirstTimeTutorial()
    {
        firstTime = false;
        yield return new WaitForSeconds(1.2f);
        int index = 0;
        Time.timeScale = 0;
        while (index < tutorialWindows.Length)
        {
            tutorialWindows[index].SetActive(true);

            yield return new WaitForSecondsRealtime(0.25f);
            yield return new WaitUntil(() => Input.anyKeyDown);
            if (!Input.GetKeyDown(KeyCode.Space)) continue;
            tutorialWindows[index].SetActive(false);
            source.PlayOneShot(confirmSE);
            index++;
        }
        Time.timeScale = 1;
    }
}
