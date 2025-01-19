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
    {new(4,7),new(6,9),new(7,11)};
    static readonly Vector2[] kZombieTime = new Vector2[3]
    {new(7,7),new(5,7),new(3,8)};
    static readonly float[] kCommandTimer = new float[3]
    {-1,7,5 };

    static bool isSpicy = false;
    public bool IsSpicy { get => isSpicy; set => isSpicy = value; }
    static readonly int[] spicyMoveSpeeds = new int[3] { 0, 2, 3 };
    public static int SpicyMoveSpeed(int speed) { return isSpicy ? spicyMoveSpeeds[speed] : 0; }

    public int CommandLength => Random.Range(kDifficulty[difficulty].x, kDifficulty[difficulty].y);
    public int CommandIntendedLength => difficulty == 0 ? 0 : Random.Range(kDifficulty[difficulty - 1].x, kDifficulty[difficulty - 1].y);
    public float CommandWait => kZombieTime[difficulty].x + Random.Range(0, kZombieTime[difficulty].y);
    public float CommandTimer => kCommandTimer[difficulty];
    public float CommandMinWait => kZombieTime[difficulty].x;

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