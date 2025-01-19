using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SpicyChickenSelect : MonoBehaviour
{
    [SerializeField]
    CanvasGroup spicyGroup;
    [SerializeField]
    Image[] spicyImages;
    [SerializeField]
    TMP_Text spicySwitchText;
    [SerializeField]
    TMP_Text spicyActiveText;

    private void Start()
    {
        if(!AchieveList.Instance.CanSpicyChicken)
        {
            spicyGroup.alpha = 0;
            return;
        }
        spicyGroup.alpha = 1;
        spicyActiveText.enabled = false;
        SetSpicy();
        StartCoroutine(nameof(WaitForInput));
    }
    IEnumerator WaitForInput()
    {
        while (true)
        {
            yield return new WaitUntil(() => Input.anyKeyDown);
            if (!Input.GetKeyDown(KeyCode.LeftShift) && (!Input.GetKeyDown(KeyCode.RightShift))) continue;

            spicyActiveText.enabled ^= true;
            SetSpicy();
        }
    }
    void SetSpicy()
    {
        foreach (var image in spicyImages)
        {
            image.color = ActiveSpicy ? new Color(1, 0.5f, 0, 1) : Color.gray;
        }
        spicySwitchText.text = ActiveSpicy ? "Shiftキーでスパイシーチキンモード無効" : "Shiftキーでスパイシーチキンモード有効";
        DifficultyManager.Instance.IsSpicy = ActiveSpicy;
    }
    bool ActiveSpicy => spicyActiveText.enabled;
}
