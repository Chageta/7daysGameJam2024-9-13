using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InputCommand : MonoBehaviour
{
    [SerializeField]
    UnityEvent OnCommandSuccess, OnCommandFailure;

    [SerializeField]
    CommandUI ui;

    int currentCommandIndex;
    int[] commands;
    bool commandFailureWait = false;
    readonly KeyCode[] kCommandKeys = new KeyCode[4]
    {KeyCode.W,KeyCode.S,KeyCode.A,KeyCode.D};

    [SerializeField]
    AudioSource commandSource;
    [SerializeField]
    AudioClip[] commandSE;

    [SerializeField]
    AudioClip successSE, failureSE;

    public void BeginCommand()
    {
        commands = new int[DifficultyManager.Instance.CommandLength];
        for (int i = 0; i < commands.Length; i++)
        {
            commands[i] = Random.Range(0, 4);
        }
        currentCommandIndex = 0;
        ui.SetCommand(ref commands);
        ui.SetActive(true);

        StopAllCoroutines();
        StartCoroutine("CommandInput");
        commandSource.PlayOneShot(successSE);
    }
    public void ForceEndCommand()
    {
        StopAllCoroutines();
        ui.CommandEnd();
        ui.SetActive(false);
    }
    IEnumerator CommandInput()
    {
        yield return new WaitWhile(() => commandFailureWait);
        while (currentCommandIndex < commands.Length)
        {
            yield return new WaitUntil(() => Input.anyKeyDown);
            if (!IsValidInput()) continue;

            if (Input.GetKeyDown(kCommandKeys[commands[currentCommandIndex]]))
            {
                ui.CommandHit(++currentCommandIndex);
                PlayCommandSE();
            }
            else
            {
                FailureCommand();
                yield break;
            }
        }
        ui.CommandEnd();
        ui.SetActive(false);
        OnCommandSuccess.Invoke();
        commandSource.PlayOneShot(successSE);
    }
    public void EndCommandFailureWait()
    {
        commandFailureWait = false;
    }
    bool IsValidInput()
    {
        foreach (var key in kCommandKeys)
        {
            if (Input.GetKeyDown(key)) return true;
        }
        //Debug.Log("[Command]Input Not Valid");
        return false;
    }
    void PlayCommandSE()
    {
        commandSource.PlayOneShot(commandSE[Random.Range(0, commandSE.Length)]);
    }
    public void ForceFailure()
    {
        FailureCommand();
        StopAllCoroutines();
    }
    void FailureCommand()
    {
        OnCommandFailure.Invoke();
        commandFailureWait = true;
        commandSource.PlayOneShot(failureSE);
    }
}