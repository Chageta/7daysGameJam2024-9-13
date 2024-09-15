using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DifficultyManager : MonoBehaviour
{
    static DifficultyManager instance;
    public static DifficultyManager Instance => instance;

    static int difficulty;
    public int Difficulty { get => difficulty; set => difficulty = value; }

    static readonly Vector2Int[] kDifficulty = new Vector2Int[3]
    {new(4,7),new(5,8),new(6,9)};

    public int CommandLength => Random.Range(kDifficulty[difficulty].x, kDifficulty[difficulty].y);

    private void Awake()
    {
        if (instance != null)
        {
            DestroyImmediate(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }
}