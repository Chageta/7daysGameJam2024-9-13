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
        Debug.Log($"time:{startTime}   {DateTime.Now}   elapse:{DateTime.Now - startTime}");
        return DateTime.Now - startTime;
    }
}
