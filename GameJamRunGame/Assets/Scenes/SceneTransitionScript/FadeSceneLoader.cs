using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class FadeSceneLoader : MonoBehaviour
{
    [SerializeField] private string sceneName;
    [SerializeField] private Image fadePanel;             // フェード用のUIパネル（Image）
    [SerializeField] private float fadeDuration = 1.0f;   // フェードの完了にかかる時間
    // Update is called once per frame

    static private bool m_IsFadeIn = false; //フェードインフラグ

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
        fadePanel.enabled = true;                 // パネルを有効化
        float elapsedTime = 0.0f;                 // 経過時間を初期化
        Canvas canvas = fadePanel.GetComponentInParent<Canvas>();
        if (canvas != null)
        {
            canvas.sortingOrder = 100; // 高い値に設定する
        }

        Color PanelColor = fadePanel.color;       // フェードパネルの開始色を取得
        Color startColor = new Color(PanelColor.r, PanelColor.g, PanelColor.b, 0.0f);      // フェードパネルの開始色を取得
        Color endColor   = new Color(PanelColor.r, PanelColor.g, PanelColor.b, 1.0f); // フェードパネルの最終色を設定

        // フェードアウトアニメーションを実行
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;                        // 経過時間を増やす
            float t = Mathf.Clamp01(elapsedTime / fadeDuration);  // フェードの進行度を計算
            fadePanel.color = Color.Lerp(startColor, endColor, t); // パネルの色を変更してフェードアウト
            yield return null;                                     // 1フレーム待機
        }

        fadePanel.color = endColor;  // フェードが完了したら最終色に設定
        m_IsFadeIn = true;
        SceneManager.LoadScene(sceneName); // シーンをロードしてメニューシーンに遷移

    }

    private IEnumerator FadeInAndLoadScene()
    {
        fadePanel.enabled = true;                 // パネルを有効化
        float elapsedTime = 0.0f;                 // 経過時間を初期化
        Canvas canvas = fadePanel.GetComponentInParent<Canvas>();
        if (canvas != null)
        {
            canvas.sortingOrder = 100; // 高い値に設定する
        }
        Color PanelColor = fadePanel.color;       // フェードパネルの開始色を取得
        Color startColor = new Color(PanelColor.r, PanelColor.g, PanelColor.b, 1.0f);      // フェードパネルの開始色を取得
        Color endColor = new Color(PanelColor.r, PanelColor.g, PanelColor.b, 0.0f); // フェードパネルの最終色を設定

        // フェードアウトアニメーションを実行
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;                        // 経過時間を増やす
            float t = Mathf.Clamp01(elapsedTime / fadeDuration);  // フェードの進行度を計算
            fadePanel.color = Color.Lerp(startColor, endColor, t); // パネルの色を変更してフェードアウト
            yield return null;                                     // 1フレーム待機
        }

        fadePanel.color = endColor;  // フェードが完了したら最終色に設定
        m_IsFadeIn = false;
    }
}
