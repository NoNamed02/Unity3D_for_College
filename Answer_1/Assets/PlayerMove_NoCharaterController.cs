using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMove_NoCharaterController : MonoBehaviour
{
    public int speed = 10;
    public int view_speed = 100;
    public float cameraSpeed = 0.3f;
    public Transform cameraTransform;


    private float _horizontal; // 좌우
    private float _vertical; // 위아래
    private float _mouseX; // 마우스 좌표값
    private bool _mouse0; // 마우스 좌클릭 감지
    private Animator _animator;

    private Vector3 cameraVelocity = Vector3.zero;
    private GameObject _firePos;

    private GameObject _bullet;
    private MeshRenderer _muzzleFlash;
    [SerializeField]
    private bool _canMove = true;

    void Start()
    {
        Init();
    }

    void Update()
    {
        Get_Input();
        Move_Object();
        Set_Animation();
        Move_Camera();
        FireBullet();
    }


    private void Init()
    {
        _mouseX = 0f;
        _animator = GetComponent<Animator>();
        _firePos = GameObject.Find("FirePos");

        _bullet = Resources.Load<GameObject>("Bullet");
        
        _muzzleFlash = GameObject.Find("Muzzle").GetComponent<MeshRenderer>();
        _muzzleFlash.enabled = false;
    }

    private void Get_Input()
    {
        if(_canMove)
        {
            _horizontal = Input.GetAxis("Horizontal");
            _vertical = Input.GetAxis("Vertical");

            _mouseX = Input.GetAxis("Mouse X") * view_speed * Time.deltaTime;

            _mouse0 = Input.GetMouseButtonDown(0);
        }
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
            _animator.SetFloat("Speed", 1f);
        else
            _animator.SetFloat("Speed", 0f);

        _animator.SetFloat("X", _horizontal);
        _animator.SetFloat("Y", _vertical);
    }

    private void FireBullet()
    {
        if(_mouse0)
        {
            Instantiate(_bullet, _firePos.transform.position, Quaternion.identity); // ver 1
            //Instantiate(_bullet, _firePos.transform.position, _muzzleFlash.gameObject.transform.rotation);
            StartCoroutine(ShowMuzzleFlash());
            //ShowMuzzleFlash_2rd();
        }
    }

    private void Move_Camera()
    {
        Vector3 offset = new Vector3(0f, 3f, -8f);
        
        Quaternion cameraRotation = Quaternion.Euler(0f, gameObject.transform.eulerAngles.y, 0f); // 플레이어의 Y축 회전 반영

        Vector3 desiredPosition = gameObject.transform.position + cameraRotation * offset;

        Camera.main.transform.position = Vector3.SmoothDamp(Camera.main.transform.position, desiredPosition, ref cameraVelocity, cameraSpeed);

        cameraTransform.LookAt(gameObject.transform.position + new Vector3(0f, 1.5f, 0f));
    }

    // 코루틴을 많이 사용하면 유니티가 죽는다
    IEnumerator ShowMuzzleFlash()
    {
        _muzzleFlash.enabled = true;
        _muzzleFlash.gameObject.transform.localScale = Vector3.one * Random.Range(0.3f, 0.7f);
        _muzzleFlash.gameObject.transform.rotation = Quaternion.Euler(0, 0, Random.Range(0, 90));
        yield return new WaitForSeconds(Random.Range(0.1f, 0.3f));
        _muzzleFlash.enabled = false;
    }

    private void ShowMuzzleFlash_2rd() // 코루틴 일반 함수로 구현
    {
        _muzzleFlash.enabled = true;
        _muzzleFlash.gameObject.transform.localScale = Vector3.one * Random.Range(0.3f, 0.7f);
        _muzzleFlash.gameObject.transform.rotation = Quaternion.Euler(0, 0, Random.Range(0, 90));

        float waitTime = Random.Range(0.1f, 0.3f);
        Invoke(nameof(DisableMuzzleFlash), waitTime);
    }

    private void DisableMuzzleFlash()
    {
        _muzzleFlash.enabled = false;
    }

    /*
    private void OnTriggerEnter(Collider other)
    {
        //if (other == gameObject.tag("Wall"));
    }
    */

    private void OnCollisionStay(Collision collision)
    {
        GameObject col = collision.gameObject;
        if (col.tag == "jumpWall")
            StartCoroutine(JumpWall());
    }

    private IEnumerator JumpWall()
    {
        _canMove = false;
        _animator.Play("UMATOBI00");
        yield return new WaitForSeconds(1f);
        _canMove = true;
    }
}