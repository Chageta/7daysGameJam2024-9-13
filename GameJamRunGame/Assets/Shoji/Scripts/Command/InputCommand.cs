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

    [SerializeField]
    AudioSource commandSource;
    [SerializeField]
    AudioClip[] commandSE;

    [SerializeField]
    AudioClip successSE, failureSE;

    bool commandIntendedWait = false;
    int successRate = 0;
    public int SuccessRate => successRate;

    public void BeginCommand()
    {
        int commandLength = DifficultyManager.Instance.CommandLength;
        if (DifficultyManager.Instance.Difficulty == 2)
        {
            if (commandIntendedWait) commandLength += DifficultyManager.Instance.CommandIntendedLength;
            commandLength += Mathf.Min(5, (int)ResultManager.Time().TotalSeconds / 45);
        }

        commands = new int[commandLength];
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
        KeyCode[] commandKeys = PlayerInput.Keys;
        while (currentCommandIndex < commands.Length)
        {
            StartCoroutine(nameof(CalcCommandIntendedWait));
            yield return new WaitUntil(() => Input.anyKeyDown);
            if (!IsValidInput()) continue;
            StopCoroutine(nameof(CalcCommandIntendedWait));

            if (Input.GetKeyDown(commandKeys[commands[currentCommandIndex]]))
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
        if (DifficultyManager.Instance.Difficulty >= 2) successRate = Mathf.Min(successRate++, 100);
    }
    public void EndCommandFailureWait()
    {
        commandFailureWait = false;
    }
    bool IsValidInput()
    {
        KeyCode[] commandKeys = PlayerInput.Keys;
        foreach (var key in commandKeys)
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
        if (DifficultyManager.Instance.Difficulty >= 2) successRate = Mathf.Max(successRate--, 0);
    }
    IEnumerator CalcCommandIntendedWait()
    {
        commandIntendedWait = false;
        if (currentCommandIndex < 3) yield break;
        yield return new WaitForSeconds(1);
        commandIntendedWait = true;
    }
}