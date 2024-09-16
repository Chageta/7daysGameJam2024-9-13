using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class ResultWindow : MonoBehaviour
{
    [SerializeField]
    TMP_Text clearTime, crowdCount, deadCount, bloodAmount;
    [SerializeField]
    Animator anim;

    [SerializeField]
    FadeSceneLoader sceneLoader;

    public void Begin(CrowdControler crowd)
    {
        anim.SetTrigger("Play");
        TimeSpan current = ResultManager.Time();
        clearTime.text = $"{current.Minutes}:{current.Seconds.ToString("00")}";
        crowdCount.text = crowd.ActorCount.ToString();
        deadCount.text = crowd.DeadCount.ToString();
        Debug.Log(current);
    }
    void EnableSceneTransition()
    {
        StartCoroutine(WaitForInput());
    }
    IEnumerator WaitForInput()
    {
        while (true)
        {
            yield return new WaitUntil(() => Input.anyKeyDown);
            if (!Input.GetKeyDown(KeyCode.Space)) continue;

            sceneLoader.TransitionScene();
            yield break;
        }
    }
}
