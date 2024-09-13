
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitions : MonoBehaviour
{
    private FadeSceneLoader m_FadeSceneLoader;

    [SerializeField] private string sceneName_ = "";

    private void Start()
    {
        m_FadeSceneLoader = GetComponent<FadeSceneLoader>();

    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            m_FadeSceneLoader.CallCoroutine(sceneName_);
        }
    }
}
