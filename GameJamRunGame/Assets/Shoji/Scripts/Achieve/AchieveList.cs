using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AchieveList : MonoBehaviour
{
    [System.Serializable]
    struct AchieveElement
    {
        public AchieveManager.Achievement name;
        public CanvasGroup group;
        public Image icon;
    }
    [SerializeField]
    AchieveElement[] achieves;
    [SerializeField]
    CanvasGroup achieveGroup;
    [SerializeField]
    TMP_Text achieveCountText;
    [SerializeField]
    Animator achieveListCloseText;

    int achieveCount;
    public bool CanSpicyChicken => achieveCount >= 6;

    static AchieveList instance;
    public static AchieveList Instance => instance;

    private void Start()
    {
        achieveGroup.alpha = 0;
        achieveListCloseText.SetBool("Show", true);
        instance = this;
        CalcAchieves();
        StartCoroutine(nameof(WaitForInput));
    }
    public void CalcAchieves()
    {
        achieveCount = 0;
        foreach (var achievement in achieves)
        {
            bool achieved = AchieveManager.IsAlreadyAchieved(achievement.name);
            achievement.group.alpha = achieved ? 1 : 0.375f;
            achievement.icon.color = achieved ? Color.white : Color.gray;
            if (achieved) achieveCount++;
        }
        achieveCountText.text = $"’B¬‚µ‚½ŽÀÑ@{achieveCount}/{AchieveManager.NumberOfAchieve}";
        achieveListCloseText.Play("AchieveListCloseShow", 0, 0);
    }
    IEnumerator WaitForInput()
    {
        while (true)
        {
            yield return new WaitUntil(() => Input.anyKeyDown);
            if (!Input.GetKeyDown(KeyCode.Tab)) continue;

            achieveListCloseText.SetBool("Show", IsOpen);
            achieveGroup.alpha = IsOpen ? 0 : 1;
        }
    }
    bool IsOpen => achieveGroup.alpha > 0.5f;
}
