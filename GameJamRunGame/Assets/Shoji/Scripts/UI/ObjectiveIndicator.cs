using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ObjectiveIndicator : MonoBehaviour
{
    [SerializeField]
    private TMP_Text marker;
    [SerializeField]
    private Image arrow;
    [SerializeField]
    private Transform objective;

    private Camera mainCamera;

    void Awake()
    {
        mainCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 pos = mainCamera.WorldToViewportPoint(objective.position);

        bool withOutCamera =
            pos.x < 0.0f || pos.x > 1.0f ||
            pos.y < 0.0f || pos.y > 1.0f ||
            pos.z < 0.0f;

        marker.gameObject.SetActive(withOutCamera);
        arrow.gameObject.SetActive(withOutCamera);

        Vector3 targetPosition = mainCamera.transform.InverseTransformPoint(objective.position);
        targetPosition.z = 0.0f;
        targetPosition = targetPosition.normalized;

        float arrowAngle = Mathf.Atan2(targetPosition.y, targetPosition.x) * Mathf.Rad2Deg;

        targetPosition /= 2;
        targetPosition += new Vector3(0.5f, 0.5f);

        targetPosition.x = Mathf.Clamp(targetPosition.x, 0.15f, 0.85f);
        targetPosition.y = Mathf.Clamp(targetPosition.y, 0.15f, 0.85f);

        marker.transform.position = mainCamera.ViewportToScreenPoint(targetPosition);
        arrow.transform.position = mainCamera.ViewportToScreenPoint(targetPosition);
        arrow.rectTransform.rotation = Quaternion.Euler(new Vector3(0.0f, 0.0f, arrowAngle));
    }
}
