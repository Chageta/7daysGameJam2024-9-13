using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ResultManager : MonoBehaviour
{
    static DateTime startTime;
    static ulong dieArray;
    static int bloodPercent;
    public static void StartTimer()
    {
        startTime = DateTime.Now;
        dieArray = 0;
        bloodPercent = 0;
    }
    public static TimeSpan Time()
    {
        return DateTime.Now - startTime;
    }
    public static void DieAt(int index)
    {
        bloodPercent += (dieArray & (ulong)(1 << index)) == 0 ? 1 : 0;
        dieArray |= (uint)1 << index;
        Debug.Log($"[RM] index:{index} blood:{bloodPercent} dieArray:{dieArray}");
    }
    public static float BloodPercent => bloodPercent * 2 / 49f;
}
