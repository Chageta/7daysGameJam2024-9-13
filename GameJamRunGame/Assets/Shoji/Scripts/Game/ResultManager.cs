using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ResultManager : MonoBehaviour
{
    static DateTime startTime;
    public static void StartTimer()
    {
        startTime = DateTime.Now;
    }
    public static TimeSpan Time()
    {
        return DateTime.Now - startTime;
    }
}
