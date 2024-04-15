using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class follow : MonoBehaviour
{

    public GameObject Target;
    public GameObject T;
    public float speed = 1.5f;
    public Text buttonpressed;
    public GameObject minimap_camera ;
    Vector3 minimap_campos;
    public Camera topDownCamera; // Assign your top-down camera here
    public RawImage minimapImage; // Assign your minimap RawImage UI element here
    public GameObject targetObject; // Assign your target object here

    void FixedUpdate()
    {
        this.transform.LookAt(Target.transform);
        float car_Move = Mathf.Abs(Vector3.Distance(this.transform.position, T.transform.position) * speed);
        this.transform.position = Vector3.MoveTowards(this.transform.position, T.transform.position, car_Move * Time.deltaTime);
    }
    private void Update()
    {
        if (Input.GetKey(KeyCode.A))
        {
            buttonpressed.text = "A";
        }
        else if (Input.GetKey(KeyCode.W))
        {
            buttonpressed.text = "W";
        }
        else if (Input.GetKey(KeyCode.S))
        {
            buttonpressed.text = "S";
        }
        else if (Input.GetKey(KeyCode.D))
        {
            buttonpressed.text = "D";
        }

    }
    public void onClickmini()
    {
        minimap_campos = minimap_camera.transform.position;
        Time.timeScale = 0;
    }
    public void offClickmini()
    {
        minimap_camera.transform.position = minimap_campos;
        Time.timeScale = 1;
    }
}

