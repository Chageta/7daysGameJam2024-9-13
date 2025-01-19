using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapParticle : MonoBehaviour
{
    [SerializeField]
    BoxCollider plane;
    static MapParticle instance;
    public static Transform Plane => instance.plane.transform;
    private void Awake()
    {
        instance = this;
    }
}
