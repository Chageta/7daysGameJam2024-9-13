using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreDisplay : MonoBehaviour
{
    [SerializeField]
    TMP_Text[] highScoresTexts;
    [SerializeField]
    TMP_Text[] lowScoresTexts;

    private void Start()
    {
        for (int i = 0; i < 3; i++)
        {
            ScoreManager.Score cur = ScoreManager.GetScore(i);
            string high = "", low = "";
            if (cur.hasSave)
            {
                high = $"最高スコア:{cur.max}";
                if (cur.min < 0) low = $"最低スコア:{cur.min}";
            }
            highScoresTexts[i].text = high;
            lowScoresTexts[i].text = low;
        }
    }
}
