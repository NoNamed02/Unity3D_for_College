using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move_2rd : MonoBehaviour
{
    [SerializeField]
    float x, y;
    float speed = 5f;

    public int HP = 100;

    Vector3 offset = new Vector3(0, 6, -10);
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        x = Input.GetAxisRaw("Horizontal");
        y = Input.GetAxisRaw("Vertical");
        Vector3 moveDirection = new Vector3(x, 0, y).normalized;

        transform.position += new Vector3(x * speed * Time.deltaTime, 0, y * speed * Time.deltaTime);
        if (moveDirection != Vector3.zero)
        {
            Vector3 lookTarget = transform.position + moveDirection;
            transform.LookAt(lookTarget);
        }

        Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, transform.position + offset, 5f * Time.deltaTime);
        Camera.main.transform.LookAt(transform.position + new Vector3(0, 1.5f, 0));
    }
}
