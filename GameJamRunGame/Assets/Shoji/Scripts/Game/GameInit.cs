using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInit : MonoBehaviour
{
    private void Awake()
    {
        ScoreManager.LoadScore();
    }
}
