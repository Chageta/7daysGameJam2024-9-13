using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DifficultySelect : MonoBehaviour
{
    [SerializeField]
    Image[] buttons;
    int buttonIndex;

    [SerializeField]
    Color inactiveColor, activeColor;

    [SerializeField]
    FadeSceneLoader sceneLoader;

    [SerializeField]
    GameObject[] infoTexts;

    [SerializeField]
    AudioSource source;
    [SerializeField]
    AudioClip[] selectSE;
    [SerializeField]
    AudioClip confirmSE;

    private void Awake()
    {
        foreach (var button in buttons)
        {
            button.color = inactiveColor;
        }
        foreach (var text in infoTexts)
        {
            text.SetActive(false);
        }
        SetButtonActive(true);
        StartCoroutine(SelectButtons());
    }
    IEnumerator SelectButtons()
    {
        bool selecting = true;
        while (selecting)
        {
            yield return new WaitUntil(() => Input.anyKeyDown);

            if (Input.GetKeyDown(KeyCode.Space))
            {
                selecting = false;
                continue;
            }
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                sceneLoader.TransitionScene("TitleScene");
                yield break;
            }
            int input = IsValidInput();
            if (input == -1) continue;

            buttons[buttonIndex].color = inactiveColor;
            SetButtonActive(false);
            int prevButton = buttonIndex;
            buttonIndex = Mathf.Clamp(buttonIndex + (input == 0 ? -1 : 1), 0, buttons.Length - 1);
            SetButtonActive(true);
            if (prevButton != buttonIndex) source.PlayOneShot(selectSE[Random.Range(0, selectSE.Length)]);
        }
        DifficultyManager.Instance.Difficulty = buttonIndex;
        sceneLoader.TransitionScene();
        source.PlayOneShot(confirmSE);
    }
    void SetButtonActive(bool active)
    {
        buttons[buttonIndex].color = active ? activeColor : inactiveColor;
        infoTexts[buttonIndex].SetActive(active);
    }
    int IsValidInput()
    {
        KeyCode[] selectKeys = new KeyCode[2];
        KeyCode[] input = PlayerInput.Keys;
        selectKeys[0] = input[2];
        selectKeys[1] = input[3];
        for (int i = 0; i < selectKeys.Length; i++)
        {
            if (Input.GetKeyDown(selectKeys[i])) return i;
        }
        return -1;
    }
}