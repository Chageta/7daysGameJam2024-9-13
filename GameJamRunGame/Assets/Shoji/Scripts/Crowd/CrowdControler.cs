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
    readonly int[] kMoveSpeeds = { 0, 4, 8 };

    readonly KeyCode[] kMoveKeys = new KeyCode[4]
    {KeyCode.W,KeyCode.S,KeyCode.A,KeyCode.D};
    readonly Vector3[] kMoveDirections = new Vector3[4]
    {Vector3.forward,Vector2.right,Vector3.back,Vector2.left};

    Vector3 determination;

    [SerializeField]
    GameObject[] moveArrows, determinationArrows;

    [SerializeField]
    ZombieCrowd zombieCrowd;

    [SerializeField]
    GameObject ActorPrefab;
    const int kInitialActorCount = 10;

    List<CrowdActor> actors = new();

    [SerializeField]
    CrowdUI crowdUI;

    public void InitializeCrowd(Vector3 position, int direction)
    {
        crowd.position = determination = position;
        currentDirection = direction;
        CalcDetermination();

        for (int i = 0; i < kInitialActorCount; i++)
        {
            Vector3 actorPosition = crowd.position;
            actorPosition += new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f));
            CrowdActor actor = Instantiate(ActorPrefab, actorPosition, Quaternion.Euler(0, 45, 0), crowd).GetComponent<CrowdActor>();
            actors.Add(actor);
            actor.Initialize(this, moveSpeed);
            actor.LookDirection(kMoveDirections[currentDirection]);
            UpdateUI();
        }
    }
    public void AddCrowdActors(CrowdActor[] newActors)
    {
        foreach (var actor in newActors)
        {
            actor.Initialize(this, moveSpeed);
            actor.LookDirection(kMoveDirections[currentDirection]);
        }
        actors.AddRange(newActors);
        UpdateUI();
    }
    public void BeginMove()
    {
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
                bool cannotStop = input == 1 && zombieCrowd.HasZombie;
                if (cannotStop) continue;

                nextDirection = 0;
                moveSpeed = Mathf.Clamp(moveSpeed + (input == 0 ? 1 : -1), 0, 2);

                actors.ForEach(a => a.SetMoveSpeed(moveSpeed));
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
        //Debug.Log("[Crowd]Input Not Valid");
        return -1;
    }
    IEnumerator Move()
    {
        while (true)
        {
            MoveUntilDead();
            CalcDetermination();
            actors.ForEach(a => a.LookDirection(kMoveDirections[currentDirection]));
            while ((determination - crowd.position).sqrMagnitude > 0.01f)
            {
                crowd.position = Vector3.MoveTowards(crowd.position, determination, kMoveSpeeds[moveSpeed] * Time.deltaTime);
                yield return null;
            }
        }
    }
    void MoveUntilDead()
    {
        actors.ForEach(a => a.MoveUntilDead(kMoveDirections[currentDirection] * kMoveSpeeds[moveSpeed]));
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
    public void TurnIntoZombie(CrowdActor actor)
    {
        actor.transform.SetParent(zombieCrowd.transform, true);
        UpdateUI();
    }
    public void ReviveAllZombies()
    {
        actors.ForEach(a => { a.Revive(); a.LookDirection(kMoveDirections[currentDirection]); a.transform.SetParent(crowd, true); });
        UpdateUI();
    }
    public void OnActorDead(CrowdActor actor)
    {
        actors.Remove(actor);
    }
    void UpdateUI()
    {
        crowdUI.SetCount(actors.Count - zombieCrowd.ZombieCount, zombieCrowd.ZombieCount);
    }
    public void Stop()
    {
        StopAllCoroutines();
        actors.ForEach(a => a.SetMoveSpeed(0));
    }
    public bool HasZombie => zombieCrowd.HasZombie;
}