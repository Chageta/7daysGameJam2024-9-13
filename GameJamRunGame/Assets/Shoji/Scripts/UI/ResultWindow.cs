using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ResultWindow : MonoBehaviour
{
    [SerializeField]
    TMP_Text clearTime, crowdCount, deadCount, bloodAmount;
    [SerializeField]
    Animator anim;

    [SerializeField]
    FadeSceneLoader sceneLoader;

    public void Begin()
    {
        anim.SetTrigger("Play");
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
