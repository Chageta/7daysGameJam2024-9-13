using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static string Path => Application.persistentDataPath + "/Score.txt";
    public struct Score
    {
        public bool hasSave;
        public int max, min;
    }
    static Score[] scores = new Score[3];
    public static void SetScore(int difficulty, int value)
    {
        Score cur = scores[difficulty];
        cur.hasSave = true;
        if (value < cur.min) cur.min = value;
        if (value > cur.max) cur.max = value;
        scores[difficulty] = cur;
    }
    public static Score GetScore(int difficulty)
    {
        return scores[difficulty];
    }
    public static void SaveScore()
    {
        using (var fs = new StreamWriter(Path))
        {
            fs.Write(
                $"{scores[0].min},{scores[0].max},{(scores[0].hasSave ? 1 : 0)}:" +
                $"{scores[1].min},{scores[1].max},{(scores[1].hasSave ? 1 : 0)}:" +
                $"{scores[2].min},{scores[2].max},{(scores[2].hasSave ? 1 : 0)}"
                );
        }
    }
    public static void LoadScore()
    {
        if (!File.Exists(Path)) return;

        string save;
        using (var fs = new StreamReader(Path))
        {
            save = fs.ReadToEnd();
        }
        string[] scoreStrings = save.Split(':');
        for (int i = 0; i < scoreStrings.Length; i++)
        {
            string[] datas = scoreStrings[i].Split(',');
            scores[i].min = int.Parse(datas[0]);
            scores[i].max = int.Parse(datas[1]);
            scores[i].hasSave = int.Parse(datas[2]) == 1;
        }
    }
}
