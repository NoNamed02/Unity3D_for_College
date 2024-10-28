using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletCtrl : MonoBehaviour
{
    public float speed = 5.0f;
    private Rigidbody _rigidbody;
    private GameObject _player;
    private TrailRenderer _trailRenderer;
    void Start()
    {
        _player = GameObject.Find("Player");
        _rigidbody = GetComponent<Rigidbody>();
        //gameObject.transform.rotation = _player.transform.rotation;
        
        //_rigidbody.AddForce(new Vector3(1, 0, 0), ForceMode.Impulse);
        _rigidbody.AddForce(_player.transform.forward * speed, ForceMode.Impulse); // ver 1
        _rigidbody.useGravity = false;
        //StartCoroutine(DesBullet());
        Destroy(this.gameObject, 3.0f);
        
        _trailRenderer = GetComponent<TrailRenderer>();
        //_trailRenderer.startColor = new Color(0, 0, 0, 1);
        _trailRenderer.endColor = new Color(0, 0, 0, 0);
    }

    IEnumerator DesBullet() // 몇초후 총알 파괴
    {
        yield return new WaitForSeconds(3f);
        Destroy(gameObject);
    }
    private void OnCollisionEnter(Collision collision)
    {
        GameObject col = collision.gameObject;
        if (col.tag == "wall")
            Debug.Log("wall hit");
        else if (col.tag == "barrel")
        {
            RandomBarrel randomBarrel = col.GetComponent<RandomBarrel>();
            randomBarrel.hit_count++;
            Debug.Log(randomBarrel.hit_count);
            if (randomBarrel.hit_count == 3)
            {
                //Destroy(col.gameObject);
            }
            Destroy(this.gameObject);
        }
    }
}
