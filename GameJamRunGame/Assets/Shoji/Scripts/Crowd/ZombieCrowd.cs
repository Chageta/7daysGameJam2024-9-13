using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieCrowd : MonoBehaviour
{
    public bool HasZombie => transform.childCount != 0;
    public int ZombieCount => transform.childCount;
}