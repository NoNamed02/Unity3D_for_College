using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove_NoCharaterController : MonoBehaviour
{
    private float _horizontal; // 좌우
    private float _vertical; // 위아래
    private float _mouseX; // 마우스 좌표값
    private Animator animator;
    private Vector3 cameraVelocity = Vector3.zero;

    public Transform cameraTransform;
    public int speed = 10;
    public int view_speed = 100;
    public float cameraSpeed = 0.3f;


    void Start()
    {
        _mouseX = 0f;
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        _horizontal = Input.GetAxis("Horizontal");
        _vertical = Input.GetAxis("Vertical");

        _mouseX = Input.GetAxis("Mouse X") * view_speed * Time.deltaTime;

        Move_Object();
        Move_Camera();
    }

    private void Move_Object()
    {
        float x = _horizontal; 
        float y = _vertical;

        float mappedX = x * Mathf.Sqrt(1 - (y * y) / 2);
        float mappedY = y * Mathf.Sqrt(1 - (x * x) / 2);

        float hori = mappedX * speed * Time.deltaTime;
        float vert = mappedY * speed * Time.deltaTime;

        gameObject.transform.Translate(hori, 0, vert);

        gameObject.transform.Rotate(0, _mouseX * view_speed, 0);

        if (new Vector3(x, 0, y).magnitude > 0.5f)
            animator.SetFloat("Speed", 1f);
        else
            animator.SetFloat("Speed", 0f);
    }

    private void Move_Camera()
    {
        Vector3 target = transform.position + new Vector3(0f, 3f, -8f);
        cameraTransform.position =
        Vector3.SmoothDamp(cameraTransform.position, target, ref cameraVelocity, cameraSpeed);
    }
}