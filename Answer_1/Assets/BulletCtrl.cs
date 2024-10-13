using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletCtrl : MonoBehaviour
{
    public float speed = 5.0f;
    private Rigidbody _rigidbody;
    private GameObject _player;
    void Start()
    {
        _player = GameObject.Find("Player");
        _rigidbody = GetComponent<Rigidbody>();
        //gameObject.transform.rotation = _player.transform.rotation;
        
        //_rigidbody.AddForce(new Vector3(1, 0, 0), ForceMode.Impulse);
        _rigidbody.AddForce(_player.transform.forward * speed, ForceMode.Impulse); // ver 1
        _rigidbody.useGravity = false;
    }
}
