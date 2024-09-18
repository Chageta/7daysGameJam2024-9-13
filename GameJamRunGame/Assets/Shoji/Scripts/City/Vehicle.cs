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
        Destroy(gameObject);
    }
    public void Collide()
    {
        blood.Play();

        Vector2Int pos = new(Mathf.RoundToInt(transform.position.x / 80) + 3, Mathf.RoundToInt(transform.position.z / 80) + 3);
        ResultManager.DieAt(pos.x * 7 + pos.y);
    }
    public Vector3 Velocity => transform.TransformVector(0, 0, speed);
}
