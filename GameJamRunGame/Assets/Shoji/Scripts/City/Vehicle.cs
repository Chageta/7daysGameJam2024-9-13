using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vehicle : MonoBehaviour
{
    [SerializeField]
    float speed;
    Vector3 localPos;

    void Start()
    {
        localPos = transform.localPosition;
        localPos.z *= -1;
        transform.GetChild(Random.Range(0, transform.childCount)).gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(0, 0, speed * Time.deltaTime);
        if (Vector3.Dot(transform.localPosition - localPos, localPos) < 0) return;
        Destroy(gameObject);
    }
    public Vector3 Velocity => transform.TransformVector(0, 0, speed);
}
