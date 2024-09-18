using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class ResultWindow : MonoBehaviour
{
    [SerializeField]
    TMP_Text clearTime, crowdCount, deadCount, bloodAmount;
    [SerializeField]
    Animator anim;

    [System.Serializable]
    struct ResultAssessment
    {
        [Min(0)] public int minCrowdCount;
        [Min(0)] public int maxDeadCount;
        [Range(0, 100)] public float maxBloodPercent;
        public int score;
        public bool IsMatch(int crowd, int dead, float blood)
        {
            if (maxDeadCount == 0) return crowd >= minCrowdCount;
            return dead >= maxDeadCount && blood >= maxBloodPercent;
        }
    }

    [SerializeField]
    TMP_Text resultScore, difficultyText, totalScore;
    [SerializeField]
    ResultAssessment[] results;
    readonly int[] kDifficultyBonus = new int[3]
    {1,2,5};

    [SerializeField]
    FadeSceneLoader sceneLoader;

    public void Begin(CrowdControler crowd)
    {
        anim.SetTrigger("Play");
        TimeSpan current = ResultManager.Time();
        clearTime.text = $"{current.Minutes}:{current.Seconds:00}";
        crowdCount.text = crowd.ActorCount.ToString();
        deadCount.text = crowd.DeadCount.ToString();
        float bloodPercent = ResultManager.BloodPercent * 100;
        bloodAmount.text = $"{bloodPercent:f1}%";

        ResultAssessment result = new();
        foreach (var resultItem in results)
        {
            if (!resultItem.IsMatch(crowd.ActorCount, crowd.DeadCount, bloodPercent)) continue;
            result = resultItem;
            Debug.Log($"deadCount:{result.maxDeadCount} score:{result.score}");
            break;
        }
        string color = result.score >= 0 ? "<#ff0>" : "<#f00>";
        int score = result.score;
        score += Math.Max(300 - current.Minutes * 60 + current.Seconds, 0) * 20 * (result.minCrowdCount == 0 ? 0 : 1);
        resultScore.text = $"{color}{score}";
        difficultyText.text = $"(難易度ボーナス{kDifficultyBonus[DifficultyManager.Instance.Difficulty] * 100}%)";
        int calcedScore = score * kDifficultyBonus[DifficultyManager.Instance.Difficulty];
        totalScore.text = $"{color}{calcedScore}";
        DifficultySelect.SetHighScore(calcedScore);
    }
    void EnableSceneTransition()
    {
        StartCoroutine(WaitForInput());
    }
    IEnumerator WaitForInput()
    {
        while (true)
        {
            yield return new WaitUntil(() => Input.anyKeyDown);
            if (!Input.GetKeyDown(KeyCode.Space)) continue;

            sceneLoader.TransitionScene();
            yield break;
        }
    }
}