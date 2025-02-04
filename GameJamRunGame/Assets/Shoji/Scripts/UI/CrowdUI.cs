using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CrowdUI : MonoBehaviour
{
    [SerializeField]
    TMP_Text[] countTexts = new TMP_Text[2];
    [SerializeField]
    Animator[] iconAnims = new Animator[2];

    [SerializeField]
    TMP_Text addCrowdText;
    [SerializeField]
    Animator addCrowdAnim;

    int[] counts = new int[2];
    enum CountType
    {
        Crowd,
        Zombie
    }

    [SerializeField]
    AudioSource source;
    [SerializeField]
    AudioClip addSE;

    public void SetCount(int crowd, int zombie)
    {
        StopAllCoroutines();
        StartCoroutine(MoveCount(crowd, CountType.Crowd));
        StartCoroutine(MoveCount(zombie, CountType.Zombie));
    }
    IEnumerator MoveCount(int value, CountType countType)
    {
        int type = (int)countType;
        TMP_Text text = countTexts[type];

        while (counts[type] != value)
        {
            counts[type] += Mathf.Clamp(value - counts[type], -1, 1);
            text.text = counts[type].ToString();
            iconAnims[type].SetTrigger("Jump");
            yield return null;
        }
        iconAnims[type].ResetTrigger("Jump");
    }
    public void AddCrowd(int amount)
    {
        addCrowdText.text = $"+{amount}";
        addCrowdAnim.Play("AddCrowd", 0, 0);
        source.PlayOneShot(addSE);
    }
}