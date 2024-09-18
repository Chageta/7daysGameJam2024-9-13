using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectiveManager : MonoBehaviour
{
    [SerializeField]
    ObjectiveEvent[] objectives;
    int completeCount;

    [SerializeField]
    AudioSource source;
    [SerializeField]
    AudioClip mainObjectiveSE, subObjectiveSE;

    private void Awake()
    {
        if (DifficultyManager.Instance.Difficulty == 0)
        {
            objectives[1].KillObjective();
            return;
        }
        objectives[0].SetEnable(false);
    }
    public void CompleteObjective()
    {
        source.PlayOneShot(subObjectiveSE);
        completeCount++;
        if (completeCount < objectives.Length - 1) return;
        objectives[0].SetEnable(true);
    }
    public void CompleteMainObjective()
    {
        source.PlayOneShot(mainObjectiveSE);
    }
}
