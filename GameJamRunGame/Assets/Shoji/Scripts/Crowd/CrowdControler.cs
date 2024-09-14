using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Cinemachine;

public class CrowdControler : MonoBehaviour
{
    [SerializeField]
    Transform crowd;
    [SerializeField]
    CinemachineVirtualCamera[] cameras;
    int currentDirection, nextDirection;
    int moveSpeed = 1;
    readonly int[] kMoveSpeeds = { 0, 3, 7 };

    readonly KeyCode[] kMoveKeys = new KeyCode[4]
    {KeyCode.W,KeyCode.S,KeyCode.A,KeyCode.D};
    readonly Vector3[] kMoveDirections = new Vector3[4]
    {Vector3.forward,Vector2.right,Vector3.back,Vector2.left};

    Vector3 determination;

    [SerializeField]
    GameObject[] moveArrows, determinationArrows;

    private void Awake()
    {
        determination = crowd.position;
        CalcDetermination();
        StartCoroutine("Move");
        BeginControl();
    }
    public void BeginControl()
    {
        EndControl();
        StartCoroutine("ControlInput");
    }
    public void EndControl()
    {
        StopCoroutine("ControlInput");
    }
    IEnumerator ControlInput()
    {
        while (true)
        {
            yield return new WaitUntil(() => Input.anyKeyDown);
            int input = IsValidInput();
            if (input == -1) continue;

            if (input < 2)
            {
                nextDirection = 0;
                moveSpeed = Mathf.Clamp(moveSpeed + (input == 0 ? 1 : -1), 0, 2);
            }
            else
            {
                nextDirection = input == 2 ? -1 : 1;
            }
        }
    }
    int IsValidInput()
    {
        for (int i = 0; i < kMoveKeys.Length; i++)
        {
            if (Input.GetKeyDown(kMoveKeys[i])) return i;
        }
        Debug.Log("[Crowd]Input Not Valid");
        return -1;
    }
    IEnumerator Move()
    {
        while (true)
        {
            while ((determination - crowd.position).sqrMagnitude > 0.01f)
            {
                crowd.position = Vector3.MoveTowards(crowd.position, determination, kMoveSpeeds[moveSpeed] * Time.deltaTime);
                yield return null;
            }
            CalcDetermination();
        }
    }
    void CalcDetermination()
    {
        Debug.Log("calc dest");
        crowd.position = determination;
        cameras[currentDirection].Priority = 0;
        currentDirection = (currentDirection + nextDirection + 4) % 4;
        nextDirection = 0;
        cameras[currentDirection].Priority = 10;

        Vector3 currentIntersection = new Vector3(Mathf.Round(determination.x / 80), 0, Mathf.Round(determination.z / 80));
        Vector3 nextIntersection = determination + kMoveDirections[currentDirection] * 40;
        nextIntersection = new Vector3(Mathf.Round(nextIntersection.x / 80), 0, Mathf.Round(nextIntersection.z / 80));
        //êMçÜÇ‹ÇΩÇÆÇ©éüÇÃåç∑ì_Ç©
        float multiply = Vector3.SqrMagnitude(currentIntersection - nextIntersection) < 1 ? 16 : 64;
        determination += kMoveDirections[currentDirection] * multiply;
    }
}
