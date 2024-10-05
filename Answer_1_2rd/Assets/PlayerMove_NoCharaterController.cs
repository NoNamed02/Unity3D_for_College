using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMove_NoCharaterController : MonoBehaviour
{
    enum animation
    {
        run_F,
        run_L,
        run_R,
        walk_B
    }

    [SerializeField]
    private float _horizontal; // 좌우
    [SerializeField]
    private float _vertical; // 위아래
    [SerializeField]
    private float _mouseX; // 마우스 좌표값
    private Animator animator;
    private Vector3 cameraVelocity = Vector3.zero;

    public Transform cameraTransform;
    public int speed = 10;
    public int view_speed = 100;
    public float cameraSpeed = 0.3f;
    GameObject Player;

    private GameObject bullet;

    //public float x, y, z; //임시
    void Start()
    {
        _mouseX = 0f;
        animator = GetComponent<Animator>();
        Player = GameObject.Find("Player");

        bullet = Resources.Load<GameObject>("Bullet");
    }

    void Update()
    {
        Get_Input();
        Move_Object();
        Set_Animation();
        Move_Camera();

        if(Input.GetMouseButtonDown(0))
            Instantiate(bullet, gameObject.transform.position + new Vector3(0f, 1f, 0), Quaternion.identity);
    }

    private void Get_Input()
    {
        _horizontal = Input.GetAxis("Horizontal");
        _vertical = Input.GetAxis("Vertical");

        _mouseX = Input.GetAxis("Mouse X") * view_speed * Time.deltaTime;
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
    }

    private void Set_Animation()
    {
        if (new Vector3(_horizontal, 0, _vertical).magnitude > 0.5f)
            animator.SetFloat("Speed", 1f);
        else
            animator.SetFloat("Speed", 0f);

        //if (_vertical > 0.4) _vertical = 0.2f; // animation clip
        animator.SetFloat("X", _horizontal);
        animator.SetFloat("Y", _vertical);
    }

    private void Move_Camera()
    {
        /*
        Vector3 offset = new Vector3(0f, 3f, -8f);
        Quaternion camRotation = Quaternion.Euler(0f, _mouseX * view_speed, 0f);
        Vector3 desiredPosition = Player.transform.position + camRotation * offset;
        cameraTransform.position = Vector3.SmoothDamp(cameraTransform.position, desiredPosition, ref cameraVelocity, cameraSpeed);
        cameraTransform.LookAt(Player.transform.position + new Vector3(0f, 1.5f, 0f));
        Camera.main.transform.position = Vector3.SmoothDamp(gameObject.transform.position + new Vector3(0, 3f, -8f), );
        */
    
        //Camera.main.transform.position = Vector3.SmoothDamp(gameObject.transform.position, gameObject.transform.position + Quaternion.Euler(0f, _mouseX * view_speed, 0f) * new Vector3(0f, 3f, -8f), ref cameraVelocity, cameraSpeed);
        //cameraTransform.LookAt(gameObject.transform.position + new Vector3(0f, 1.5f, 0f));
        

        Vector3 offset = new Vector3(0f, 3f, -8f);
        //offset = new Vector3(0,0,0);
        
        Quaternion cameraRotation = Quaternion.Euler(0f, gameObject.transform.eulerAngles.y, 0f); // 플레이어의 Y축 회전 반영

        Vector3 desiredPosition = gameObject.transform.position + cameraRotation * offset;

        Camera.main.transform.position = Vector3.SmoothDamp(Camera.main.transform.position, desiredPosition, ref cameraVelocity, cameraSpeed);

        cameraTransform.LookAt(gameObject.transform.position + new Vector3(0f, 1.5f, 0f)); 

        /*
        Camera.main.transform.position = 
        Vector3.Lerp(transform.position, transform.position - transform.forward * 8f + Vector3.up * 3f, Time.deltaTime * 20f);
        */

        //transform.position = Vector3.Lerp(transform.position, targetTr.position - targetTr.forward * Distance + Vector3.up * height, Time.deltaTime * dampingTrace);
    }
}