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
        
        if(score > 0)
        {
            score += crowd.ActorCount * 75;
        }
        if(score < 0)
        {
            score -= crowd.DeadCount * 75;
            score -= (int)(bloodPercent * 100);
        }
        int timeBonus = Math.Max(300 - current.Minutes * 60 + current.Seconds, 0) * 20 * (result.minCrowdCount == 0 ? 0 : 1);
        score += timeBonus;
        resultScore.text = $"{color}{score}";
        difficultyText.text = $"(難易度ボーナス{kDifficultyBonus[DifficultyManager.Instance.Difficulty] * 100}%)";
        int calcedScore = score * kDifficultyBonus[DifficultyManager.Instance.Difficulty];
        totalScore.text = $"{color}{calcedScore}";
        ScoreManager.SetScore(DifficultyManager.Instance.Difficulty, calcedScore);
        ScoreManager.SaveScore();
        CalcAchieve(crowd, current, bloodPercent, calcedScore);
    }
    void CalcAchieve(CrowdControler crowd, TimeSpan clearTime, float bloodPercent, int score)
    {
        //難易度のみの実績
        int dif = DifficultyManager.Instance.Difficulty;
        switch (dif)
        {
            case 0:
                if (score <= 0) AchieveManager.Achieve(AchieveManager.Achievement.EasyDead);
                else AchieveManager.Achieve(AchieveManager.Achievement.EasyClear);
                break;
            case 1:
                if (score <= 0) AchieveManager.Achieve(AchieveManager.Achievement.NormalDead);
                else AchieveManager.Achieve(AchieveManager.Achievement.NormalClear);
                break;
            case 2:
                if (score <= 0) AchieveManager.Achieve(AchieveManager.Achievement.HardDead);
                else AchieveManager.Achieve(AchieveManager.Achievement.HardClear);
                break;
        }
        //クリア時のみの実績
        if (score > 0)
        {
            //NoProblem
            if (crowd.DeadCount >= 20) AchieveManager.Achieve(AchieveManager.Achievement.NoProblem);
            //Family
            if (crowd.DeadCount == 0 && crowd.ActorCount >= 30) AchieveManager.Achieve(AchieveManager.Achievement.Family);
            //SpeedStar
            if (dif >= 1 && clearTime.TotalSeconds <= 90) AchieveManager.Achieve(AchieveManager.Achievement.SpeedStar);
            //Score
            if (score >= 20000) AchieveManager.Achieve(AchieveManager.Achievement.Score);
            //MegaScore
            if (score >= 40000) AchieveManager.Achieve(AchieveManager.Achievement.MegaScore);
            //SpicyHot
            if (DifficultyManager.Instance.IsSpicy && crowd.ActorCount >= 30) AchieveManager.Achieve(AchieveManager.Achievement.SpicyHot);
            //SpicySpeed
            if (DifficultyManager.Instance.IsSpicy && clearTime.TotalMinutes < 1) AchieveManager.Achieve(AchieveManager.Achievement.SpicySpeed);
        }
        //全滅時のみの実績
        else
        {
            //OutOfBounds
            Vector3 pos = crowd.Crowd.position;
            if (Mathf.Max(Mathf.Abs(pos.x), Mathf.Abs(pos.z)) > 290) AchieveManager.Achieve(AchieveManager.Achievement.OutOfBounds);
            //Gore
            if (score <= -5000) AchieveManager.Achieve(AchieveManager.Achievement.Gore);
            //MoreGore
            if (score <= -20000) AchieveManager.Achieve(AchieveManager.Achievement.MoreGore);
            //Spicy
            if (DifficultyManager.Instance.IsSpicy) AchieveManager.Achieve(AchieveManager.Achievement.Spicy);
        }

        //RedCarpet
        if (bloodPercent >= 15) AchieveManager.Achieve(AchieveManager.Achievement.RedCarpet);
        //TomatoParty
        if (bloodPercent >= 30) AchieveManager.Achieve(AchieveManager.Achievement.TomatoParty);
        //SpicyGore
        if (DifficultyManager.Instance.IsSpicy && crowd.DeadCount >= 30 && bloodPercent >= 30) AchieveManager.Achieve(AchieveManager.Achievement.SpicyGore);

        AchieveManager.SaveAchieve();
        AchieveList.Instance.CalcAchieves();
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