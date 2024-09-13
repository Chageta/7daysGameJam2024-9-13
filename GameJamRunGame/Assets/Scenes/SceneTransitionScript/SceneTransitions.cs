
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitions : MonoBehaviour
{
    private FadeSceneLoader m_FadeSceneLoader;

    private Countdown m_Countdown;

    [SerializeField] private string sceneName_ = "";

    private void Start()
    {
        m_FadeSceneLoader = GetComponent<FadeSceneLoader>();

        m_Countdown = GameObject.Find("Timer")?.GetComponent<Countdown>();

    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (m_Countdown == null || (m_Countdown != null && m_Countdown.IsStart == true))
            {
                m_FadeSceneLoader.CallCoroutine(sceneName_);
            }
        }
    }


}
