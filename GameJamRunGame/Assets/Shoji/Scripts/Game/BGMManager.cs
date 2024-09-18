using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMManager : MonoBehaviour
{
    [SerializeField]
    AudioSource mainBGM, hardBGM;

    static float hardAmount;
    private void Awake()
    {
        StartCoroutine(InitialVolume());
        StartCoroutine(AutoVolume());
    }
    IEnumerator AutoVolume()
    {
        while (true)
        {
            yield return new WaitWhile(() => hardAmount == 0);
            hardAmount = Mathf.Clamp(hardAmount-=Time.deltaTime, 0, 5);
            hardBGM.volume = hardAmount / 2;
            mainBGM.volume = 1 - hardAmount / 2;
            yield return null;
        }
    }
    IEnumerator InitialVolume()
    {
        float initialMute = 0;
        while (initialMute < 1)
        {
            initialMute = Mathf.Clamp01(Time.time / 3);
            hardBGM.volume = 0;
            mainBGM.volume = initialMute;
            yield return null;
        }
    }
    public static void AddHard()
    {
        hardAmount += 1;
    }
}
