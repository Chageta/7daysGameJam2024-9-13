using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

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

    [SerializeField]
    TMP_Text[] highScoresTexts;
    static int[] highScores = new int[3] { int.MinValue, int.MinValue, int.MinValue };

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
        for (int i = 0; i < 3; i++)
        {
            string text = highScores[i] == int.MinValue ? "" : $"ハイスコア:{highScores[i]}";
            highScoresTexts[i].text = text;
        }
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
        for (int i = 0; i < kSelectKeys.Length; i++)
        {
            if (Input.GetKeyDown(kSelectKeys[i])) return i;
        }
        return -1;
    }
    public static void SetHighScore(int score)
    {
        highScores[DifficultyManager.Instance.Difficulty] = Mathf.Max(highScores[DifficultyManager.Instance.Difficulty], score);
    }
}