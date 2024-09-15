using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneTransitions : MonoBehaviour
{
    [SerializeField]
    private FadeSceneLoader m_FadeSceneLoader;

    private void Start()
    {
        StartCoroutine(WaitForInput());
    }
    // Update is called once per frame
    IEnumerator WaitForInput()
    {
        while (true)
        {
            yield return new WaitUntil(() => Input.anyKeyDown);
            if (!Input.GetKeyDown(KeyCode.Space)) continue;
            m_FadeSceneLoader.TransitionScene();
        }
    }
}