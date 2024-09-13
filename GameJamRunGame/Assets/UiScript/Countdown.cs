using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Countdown : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI m_TimerText; // プレイ時間を表示するテキスト

    private float m_CountdownTime = 3;

    private bool m_IsStart = false;

    public bool IsStart
    {
        get { return m_IsStart; }
        set { m_IsStart = value; }
    }
    // Start is called before the first frame update
    void Start()
    {
        m_IsStart = false;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateTimerText();
    }

    // タイマー表示用テキストを更新するメソッド
    private void UpdateTimerText()
    {
        //Debug.Log(m_CountdownTime);
        int Timerasint = Mathf.CeilToInt(m_CountdownTime);
        if (m_CountdownTime >= 0)
        {
            m_CountdownTime -= Time.deltaTime;
            m_TimerText.text = Timerasint.ToString();
        }
        else
        {
            Debug.Log(m_IsStart);

            m_IsStart = true;
            m_TimerText.text = "START!!!";
        }
    }
}
