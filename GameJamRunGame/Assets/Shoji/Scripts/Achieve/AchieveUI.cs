using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AchieveUI : MonoBehaviour
{
    public bool ExternalStop;

    readonly string[] NameText = new string[]
    {
        "ながらタウンへようこそ",
        "よくやった！",
        "痛い目を見た",
        "チュートリアルは終わり",
        "右見て左見て...",
        "歩きスマホは終わり",
        "俺が無事なら",
        "レッドカーペット",
        "大所帯",
        "トマトパーティー！",
        "囚われの身",
        "走りスマホ",
        "熟練の技",
        "達人の域",
        "汚い花火",
        "派手な花火",
        "スパイシー！",
        "スパイシーホット！",
        "スパイシースピード！",
        "こんがり！"
    };
    [SerializeField]
    TMP_Text achieveName;
    [SerializeField]
    Animator anim;
    [SerializeField]
    AudioSource source;
    private void Awake()
    {
        AchieveManager.UI = this;

        ExternalStop = true;
        AchieveManager.LoadAchieve();
        ExternalStop = false;
    }
    public void ActiveAchieveUI(int num)
    {
        if (ExternalStop) return;
        achieveName.text = NameText[num];
        CancelInvoke();
        Invoke(nameof(StartUI), 1);
    }
    void StartUI()
    {
        anim.Play("AchieveAnim", 0, 0);
        source.Play();
    }
}
