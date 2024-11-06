using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Nav_enemy : MonoBehaviour
{
    private NavMeshAgent _agent;
    private GameObject _target;
    [SerializeField]
    private float _rayDistance = 10f;
    private LayerMask _obstacleLayer;

    [SerializeField]
    private bool _isFind = false;
    private float _iter = 0;
    void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        _target = GameObject.FindWithTag("Player");
        _obstacleLayer = LayerMask.GetMask("Obstacle");
        StartCoroutine(FindTarget());
    }
    void GoToTarget()
    {
        _iter += Time.deltaTime;
        if (_iter <= 120 && _target != null)
        {
            _agent.destination = _target.transform.position;
            _iter = 0f;
        }
    }
    IEnumerator FindTarget()
    {
        while (true)
        {
            if (_target != null)
            {
                Ray ray = new Ray(transform.position, transform.forward);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit, _rayDistance, _obstacleLayer))
                {
                    Debug.Log("hit wall");
                }
                else if (Physics.Raycast(ray, out hit, _rayDistance)
                && hit.collider.CompareTag("Player")) 
                {
                    _isFind = true;
                }
            }
            yield return new WaitForSeconds(0.5f);
            if (_isFind) GoToTarget();
        }
    }


    private void OnDrawGizmos()
    {
        if (_target != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawRay(transform.position, transform.forward * _rayDistance);
        }
    }
}