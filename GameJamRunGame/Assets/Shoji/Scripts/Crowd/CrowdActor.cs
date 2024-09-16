using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrowdActor : MonoBehaviour
{
    event System.Action<CrowdActor> OnDead;

    [SerializeField]
    Rigidbody rb;

    CrowdControler crowd;

    Vector3 positionInCrowd;

    [SerializeField]
    Animator anim;

    Coroutine lookDirection;
    Coroutine moveUntilDead;

    const float kZombieTimeMin = 5, kZombieTimeRange = 5;

    [SerializeField]
    Transform skinRoot;
    [SerializeField]
    ParticleSystem blood;

    [SerializeField]
    GameObject phoneIcon;
    private void Awake()
    {
        skinRoot.GetChild(Random.Range(0, skinRoot.childCount)).gameObject.SetActive(true);
        positionInCrowd = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f));
        
        anim.SetLayerWeight(1, 0);
        anim.SetFloat("CycleOffset", Random.Range(0f, 1f));
    }
    public void Initialize(CrowdControler crowd, int moveSpeed)
    {
        this.crowd = crowd;
        SetMoveSpeed(moveSpeed);

        float waitTime = DifficultyManager.Instance.CommandWait + 5;
        Invoke("TurnIntoZombie", waitTime);

        OnDead += crowd.OnActorDead;
    }
    void TurnIntoZombie()
    {
        crowd.TurnIntoZombie(this);
        anim.SetBool("isTexting", true);
        anim.SetLayerWeight(1, 1);
        phoneIcon.SetActive(true);
    }
    public void Revive()
    {
        CancelInvoke();
        float waitTime = DifficultyManager.Instance.CommandWait;
        Invoke("TurnIntoZombie", waitTime);
        anim.SetBool("isTexting", false);
        anim.SetLayerWeight(1, 0);
        phoneIcon.SetActive(false);

        if (moveUntilDead == null) return;
        StopCoroutine(moveUntilDead);
        moveUntilDead = null;
    }
    public void SetMoveSpeed(int moveSpeed)
    {
        anim.SetInteger("MoveSpeed", moveSpeed);
    }
    public void LookDirection(Vector3 direction)
    {
        if (IsTexting) return;
        if (lookDirection != null)
        {
            StopCoroutine(lookDirection);
            lookDirection = null;
        }
        lookDirection = StartCoroutine(LookDirection(Quaternion.LookRotation(direction, Vector3.up)));
    }
    IEnumerator LookDirection(Quaternion rotateTarget)
    {
        while (true)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, rotateTarget, 180 * Time.deltaTime);
            yield return null;
        }
    }
    private void Update()
    {
        if (IsDead) return;
        rb.angularVelocity = Vector3.zero;
        Vector3 rot = transform.eulerAngles;
        rot.x = rot.z = 0;
        transform.eulerAngles = rot;
        if (crowd != null) if (Vector3.SqrMagnitude(crowd.transform.position - transform.position) > 2500) Die();
        if (IsTexting) return;

        Vector3 moveVector = (transform.parent.TransformPoint(positionInCrowd) - transform.position) * 2;
        rb.velocity = Vector3.Lerp(rb.velocity, moveVector, Time.deltaTime * 5);
    }
    public void MoveUntilDead(Vector3 moveVector)
    {
        if (!IsTexting) return;
        if (moveUntilDead != null) return;
        moveUntilDead = StartCoroutine(MoveUntilDeadC(moveVector));
    }
    IEnumerator MoveUntilDeadC(Vector3 moveVector)
    {
        Vector3 pos = transform.position;
        while (true)
        {
            rb.velocity = Vector3.zero;
            pos += moveVector * Time.deltaTime;
            transform.position = pos;
            yield return null;
        }
    }
    private void OnCollisionEnter(Collision hit)
    {
        if (hit.gameObject.layer != 6) return;
        if (IsDead) return;
        Vector3 velocity = hit.gameObject.GetComponent<Vehicle>().Velocity;
        rb.AddForce(velocity, ForceMode.Impulse);
        Die();
    }
    void Die()
    {
        if (IsDead) return;
        StopAllCoroutines();
        CancelInvoke();
        transform.SetParent(null);
        anim.SetBool("isDead", true);
        Invoke("HideMesh", 1);
        blood.Play();

        OnDead?.Invoke(this);
    }
    void HideMesh()
    {
        skinRoot.gameObject.SetActive(false);
    }
    public bool IsDead => anim.GetBool("isDead");
    public bool IsTexting => anim.GetBool("isTexting");
}
