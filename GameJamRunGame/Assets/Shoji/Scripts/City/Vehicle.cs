using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vehicle : MonoBehaviour
{
    [SerializeField]
    float speed;
    Vector3 localPos;

    [SerializeField]
    ParticleSystem blood;
    const float kDestoyWait = 5;
    bool collide = false;
    [SerializeField]
    BoxCollider col;

    void Start()
    {
        localPos = transform.localPosition;
        localPos.z *= -1;
        transform.GetChild(Random.Range(0, transform.childCount - 1)).gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(0, 0, speed * Time.deltaTime);
        if (Vector3.Dot(transform.localPosition - localPos, localPos) < 0) return;

        StartDestoy();
    }
    public void Collide()
    {
        collide = true;
        blood.Play();

        ResultManager.DieAt(transform.position);
    }
    void StartDestoy()
    {
        if (!collide)
        {
            Destroy(gameObject);
            return;
        }
        col.enabled = false;
        for (int i = 0; i < 3; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }
        Destroy(gameObject, kDestoyWait);
    }
    public Vector3 Velocity => transform.TransformVector(0, 0, speed);
}
