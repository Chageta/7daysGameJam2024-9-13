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

    private void Awake()
    {
        foreach (var button in buttons)
        {
            button.color = inactiveColor;
        }
        buttons[buttonIndex].color = activeColor;
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
            buttonIndex = Mathf.Clamp(buttonIndex + (input == 0 ? -1 : 1), 0, buttons.Length - 1);
            buttons[buttonIndex].color = activeColor;
        }
        DifficultyManager.Instance.Difficulty = buttonIndex;
        sceneLoader.TransitionScene();
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
