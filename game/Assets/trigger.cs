using UnityEngine;
using UnityEngine.UI;

public class trigger : MonoBehaviour
{
    public Camera minimapCamera;
    public GameObject cubePrefab;

    private RectTransform rectTransform;

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();

    }

    public void ClickOnMinimap()
    {
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, Input.mousePosition, null, out localPoint);

        Vector2 normalizedPoint = new Vector2(
            Mathf.InverseLerp(-rectTransform.rect.width * 0.5f, rectTransform.rect.width * 0.5f, localPoint.x),
            Mathf.InverseLerp(-rectTransform.rect.height * 0.5f, rectTransform.rect.height * 0.5f, localPoint.y));

        // Calculate the world position based on normalized point
        Vector3 targetPosition = minimapCamera.ViewportToWorldPoint(new Vector3(normalizedPoint.x, normalizedPoint.y, minimapCamera.nearClipPlane));

        // Set the Y position of the cube to be at the ground level (Y = 0)
        targetPosition.y = 0;

        // Set the position of the cube without instantiating it
        cubePrefab.transform.position = targetPosition;
    }
}
