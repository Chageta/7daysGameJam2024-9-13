using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DifficultySelect : MonoBehaviour
{
    [SerializeField]
    Image[] buttons;
    int buttonIndex;

    readonly KeyCode[] kSelectKeys = new KeyCode[2]
    {KeyCode.A,KeyCode.D};

    [SerializeField]
    Color inactiveColor, activeColor;

    [SerializeField]
    FadeSceneLoader sceneLoader;

    [SerializeField]
    GameObject[] infoTexts;

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
            int input = IsValidInput();
            if (input == -1) continue;

            buttons[buttonIndex].color = inactiveColor;
            SetButtonActive(false);
            buttonIndex = Mathf.Clamp(buttonIndex + (input == 0 ? -1 : 1), 0, buttons.Length - 1);
            SetButtonActive(true);
        }
        DifficultyManager.Instance.Difficulty = buttonIndex;
        sceneLoader.TransitionScene();
    }
    void SetButtonActive(bool active)
    {
        buttons[buttonIndex].color = active ? activeColor : inactiveColor;
        infoTexts[buttonIndex].SetActive(active);
    }
    int IsValidInput()
    {
        for (int i = 0; i < kSelectKeys.Length; i++)
        {
            if (Input.GetKeyDown(kSelectKeys[i])) return i;
        }
        return -1;
    }
}
