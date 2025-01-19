using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Cinemachine;
using UnityEngine.UI;

public class CrowdControler : MonoBehaviour
{
    [SerializeField]
    UnityEvent OnAllActorDead;

    [SerializeField]
    Transform crowd;
    public Transform Crowd => crowd;
    [SerializeField]
    CinemachineVirtualCamera[] cameras;
    int currentDirection, nextDirection;
    int moveSpeed = 1;
    readonly int[] kMoveSpeeds = { 0, 4, 8 };
    int MoveSpeed(int speed) { return kMoveSpeeds[speed] + DifficultyManager.SpicyMoveSpeed(speed); }

    readonly Vector3[] kMoveDirections = new Vector3[4]
    {Vector3.forward,Vector2.right,Vector3.back,Vector2.left};

    Vector3 determination;

    [SerializeField]
    GameObject[] speedArrows, directionArrows, determinationArrows;
    [SerializeField]
    Transform arrowRoot, determinationArrowRoot;
    int arrowEmissionShaderProperty;
    Coroutine arrowEmissionCoroutine;

    [SerializeField]
    ZombieCrowd zombieCrowd;

    [SerializeField]
    GameObject actorPrefab;
    const int kInitialActorCount = 10;

    List<CrowdActor> actors = new();
    int deadCount;

    [SerializeField]
    CrowdUI crowdUI;

    [SerializeField]
    ResultWindow resultWindow;

    float cameraShakeAmount;
    Coroutine cameraShake;
    [SerializeField]
    Image screenRed;

    [SerializeField]
    AudioSource source;
    [SerializeField]
    AudioClip[] speedSE, directionSE;

    public void InitializeCrowd(Vector3 position, int direction)
    {
        crowd.position = determination = position;
        currentDirection = direction;

        //CalcDeterminationの初期化部分だけ
        cameras[currentDirection].Priority = 10;

        for (int i = 0; i < kInitialActorCount; i++)
        {
            Vector3 actorPosition = crowd.position;
            actorPosition += new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f));
            CrowdActor actor = Instantiate(actorPrefab, actorPosition, Quaternion.Euler(0, 45, 0), crowd).GetComponent<CrowdActor>();
            actors.Add(actor);
            actor.Initialize(this, 0);
            actor.LookDirection(kMoveDirections[currentDirection]);
            UpdateUI();
        }
        arrowEmissionShaderProperty = Shader.PropertyToID("_EmissionAmount");
    }
    public void AddCrowdActors(CrowdActor[] newActors)
    {
        foreach (var actor in newActors)
        {
            actor.transform.SetParent(crowd);
            actor.Initialize(this, moveSpeed);
            actor.LookDirection(kMoveDirections[currentDirection]);
        }
        crowdUI.AddCrowd(newActors.Length);
        actors.AddRange(newActors);
        UpdateUI();
    }
    public void BeginMove()
    {
        StartCoroutine("Move");
        BeginControl();
        actors.ForEach(a => a.SetMoveSpeed(moveSpeed));
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

            bool wsInput = input < 2;
            if (wsInput)
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
            SetArrow(wsInput);
        }
    }
    void SetArrow(bool wsInput)
    {
        HideArrows();
        if (wsInput)
        {
            speedArrows[moveSpeed].SetActive(true);
            source.PlayOneShot(speedSE[moveSpeed]);
        }
        else
        {
            directionArrows[nextDirection + 1].SetActive(true);
            source.PlayOneShot(directionSE[nextDirection + 1]);
        }

        determinationArrows[nextDirection + 1].SetActive(true);

        if (arrowEmissionCoroutine != null)
        {
            StopCoroutine(arrowEmissionCoroutine);
            arrowEmissionCoroutine = null;
        }
        arrowEmissionCoroutine = StartCoroutine(ArrowEmission());
    }
    void HideArrows()
    {
        foreach (var arrow in directionArrows)
            arrow.SetActive(false);
        foreach (var arrow in speedArrows)
            arrow.SetActive(false);
        foreach (var arrow in determinationArrows)
            arrow.SetActive(false);
    }
    IEnumerator ArrowEmission()
    {
        float emission = 1;
        while (emission > 0)
        {
            emission = Mathf.Clamp01(emission - Time.deltaTime);
            Shader.SetGlobalFloat(arrowEmissionShaderProperty, emission);
            yield return null;
        }
    }
    int IsValidInput()
    {
        KeyCode[] moveKeys = PlayerInput.Keys;
        for (int i = 0; i < moveKeys.Length; i++)
        {
            if (Input.GetKeyDown(moveKeys[i])) return i;
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

            arrowRoot.SetLocalPositionAndRotation(kMoveDirections[currentDirection] * 3, Quaternion.LookRotation(-kMoveDirections[currentDirection]));
            determinationArrowRoot.SetPositionAndRotation(determination + kMoveDirections[currentDirection] * 3, arrowRoot.rotation);
            SetArrow(true);

            while ((determination - crowd.position).sqrMagnitude > 0.01f)
            {
                crowd.position = Vector3.MoveTowards(crowd.position, determination, MoveSpeed(moveSpeed) * Time.deltaTime);
                yield return null;
            }
        }
    }
    void MoveUntilDead()
    {
        actors.ForEach(a => a.MoveUntilDead(kMoveDirections[currentDirection] * MoveSpeed(moveSpeed)));
    }
    void CalcDetermination()
    {
        Debug.Log("calc dest");
        crowd.position = determination;
        cameras[currentDirection].Priority = 0;

        if (actors.Count == zombieCrowd.ZombieCount) nextDirection = 0;
        currentDirection = (currentDirection + nextDirection + 4) % 4;
        nextDirection = 0;
        cameras[currentDirection].Priority = 10;

        Vector3 currentIntersection = new Vector3(Mathf.Round(determination.x / 80), 0, Mathf.Round(determination.z / 80));
        Vector3 nextIntersection = determination + kMoveDirections[currentDirection] * 40;
        nextIntersection = new Vector3(Mathf.Round(nextIntersection.x / 80), 0, Mathf.Round(nextIntersection.z / 80));
        //信号またぐか次の交差点か
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
        deadCount++;
        UpdateUI();
        if (cameraShake != null)
        {
            StopCoroutine(cameraShake);
            cameraShake = null;
        }
        cameraShake = StartCoroutine(CameraShake());
        BGMManager.AddHard();

        ForceEnd();
        if (actors.Count > 0) return;
        Stop();
        OnAllActorDead.Invoke();
        resultWindow.Begin(this);
    }
    IEnumerator CameraShake()
    {
        cameraShakeAmount += 1;
        float startCameraShakeAmount = cameraShakeAmount;
        float timer = 0;
        yield return new WaitForFixedUpdate();
        while (cameraShakeAmount > 0)
        {
            cameraShakeAmount = Mathf.Clamp(Mathf.Lerp(startCameraShakeAmount, 0, timer), 0, 4);
            timer += Time.fixedDeltaTime * 0.5f;
            foreach (var camera in cameras)
            {
                camera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain = cameraShakeAmount;
            }
            Color redColor = screenRed.color;
            redColor.a = cameraShakeAmount;
            screenRed.color = redColor;

            yield return new WaitForFixedUpdate();
        }
    }
    void UpdateUI()
    {
        crowdUI.SetCount(actors.Count - zombieCrowd.ZombieCount, zombieCrowd.ZombieCount);
    }
    public void Stop()
    {
        EndControl();
        StopCoroutine("Move");
        actors.ForEach(a => a.SetMoveSpeed(0));
        HideArrows();
    }
    private void Update()
    {
        //マップ外強制死亡
        if (Mathf.Max(Mathf.Abs(crowd.position.x), Mathf.Abs(crowd.position.z)) < 320) return;

        Stop();
        resultWindow.Begin(this);
    }
    void ForceEnd()
    {
        bool uncontrol = actors.TrueForAll(a => a.IsTexting && Vector3.SqrMagnitude(crowd.position - a.transform.position) > 100);
        if (!uncontrol) return;
        Stop();
        resultWindow.Begin(this);
    }
    public int ActorCount => actors.Count;
    public int DeadCount => deadCount;
    public bool HasZombie => zombieCrowd.HasZombie;
}