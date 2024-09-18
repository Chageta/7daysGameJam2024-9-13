using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class FadeSceneLoader : MonoBehaviour
{
    [SerializeField] private string sceneName;
    [SerializeField] private Image fadePanel;             // �t�F�[�h�p��UI�p�l���iImage�j
    [SerializeField] private float fadeDuration = 1.0f;   // �t�F�[�h�̊����ɂ����鎞��
    // Update is called once per frame

    static private bool m_IsFadeIn = false; //�t�F�[�h�C���t���O

    private void Start()
    {
        if (m_IsFadeIn)
        {
            StartCoroutine(FadeInAndLoadScene());
        }
    }

    public void TransitionScene()
    {
        StartCoroutine(FadeOutAndLoadScene());
    }
    public void TransitionScene(string sceneName)
    {
        this.sceneName = sceneName;
        TransitionScene();
    }

    private IEnumerator FadeOutAndLoadScene()
    {
        fadePanel.enabled = true;                 // �p�l����L����
        float elapsedTime = 0.0f;                 // �o�ߎ��Ԃ�������
        Canvas canvas = fadePanel.GetComponentInParent<Canvas>();
        if (canvas != null)
        {
            canvas.sortingOrder = 100; // �����l�ɐݒ肷��
        }

        Color PanelColor = fadePanel.color;       // �t�F�[�h�p�l���̊J�n�F���擾
        Color startColor = new Color(PanelColor.r, PanelColor.g, PanelColor.b, 0.0f);      // �t�F�[�h�p�l���̊J�n�F���擾
        Color endColor   = new Color(PanelColor.r, PanelColor.g, PanelColor.b, 1.0f); // �t�F�[�h�p�l���̍ŏI�F��ݒ�

        // �t�F�[�h�A�E�g�A�j���[�V���������s
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;                        // �o�ߎ��Ԃ𑝂₷
            float t = Mathf.Clamp01(elapsedTime / fadeDuration);  // �t�F�[�h�̐i�s�x���v�Z
            fadePanel.color = Color.Lerp(startColor, endColor, t); // �p�l���̐F��ύX���ăt�F�[�h�A�E�g
            yield return null;                                     // 1�t���[���ҋ@
        }

        fadePanel.color = endColor;  // �t�F�[�h������������ŏI�F�ɐݒ�
        m_IsFadeIn = true;
        SceneManager.LoadScene(sceneName); // �V�[�������[�h���ă��j���[�V�[���ɑJ��

    }

    private IEnumerator FadeInAndLoadScene()
    {
        fadePanel.enabled = true;                 // �p�l����L����
        float elapsedTime = 0.0f;                 // �o�ߎ��Ԃ�������
        Canvas canvas = fadePanel.GetComponentInParent<Canvas>();
        if (canvas != null)
        {
            canvas.sortingOrder = 100; // �����l�ɐݒ肷��
        }
        Color PanelColor = fadePanel.color;       // �t�F�[�h�p�l���̊J�n�F���擾
        Color startColor = new Color(PanelColor.r, PanelColor.g, PanelColor.b, 1.0f);      // �t�F�[�h�p�l���̊J�n�F���擾
        Color endColor = new Color(PanelColor.r, PanelColor.g, PanelColor.b, 0.0f); // �t�F�[�h�p�l���̍ŏI�F��ݒ�

        // �t�F�[�h�A�E�g�A�j���[�V���������s
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;                        // �o�ߎ��Ԃ𑝂₷
            float t = Mathf.Clamp01(elapsedTime / fadeDuration);  // �t�F�[�h�̐i�s�x���v�Z
            fadePanel.color = Color.Lerp(startColor, endColor, t); // �p�l���̐F��ύX���ăt�F�[�h�A�E�g
            yield return null;                                     // 1�t���[���ҋ@
        }

        fadePanel.color = endColor;  // �t�F�[�h������������ŏI�F�ɐݒ�
        m_IsFadeIn = false;
    }
}
