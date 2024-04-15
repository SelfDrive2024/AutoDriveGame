using UnityEngine;

public class CameraSwipeMovement : MonoBehaviour
{
    public float swipeSpeed = 1f; // Adjust this to control the speed of camera movement
    private Vector2 lastPosition;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            lastPosition = Input.mousePosition;
        }
        else if (Input.GetMouseButton(0))
        {
            Vector2 delta = (Vector2)Input.mousePosition - lastPosition;
            transform.Translate(-delta.x * swipeSpeed, -delta.y * swipeSpeed, 0);
            lastPosition = Input.mousePosition;
        }
    }
}
