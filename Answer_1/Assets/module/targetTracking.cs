using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.AI;

public class TargetTracking : MonoBehaviour
{
    public GameObject _target;
    [SerializeField]
    private float _rayDistance = 10f;
    [SerializeField]
    public string layerName;
    [SerializeField]
    private string targetTag;
    private NavMeshAgent _agent;
    private LayerMask _obstacleLayer;
    private bool _isFind = false;
    private float _iter = 0;
    [SerializeField]
    private float angle = 30f;
    
    void Start()
    {
        Setup();
        StartCoroutine(FindTarget());
    }

    void Setup()
    {
        if (GetComponent<NavMeshAgent>() == null)
        {
            Debug.LogWarning("NavAgent is Null");
            _agent = gameObject.AddComponent<NavMeshAgent>();
        }
        else
            _agent = GetComponent<NavMeshAgent>();
        
        if (LayerMask.GetMask(layerName) == 0)
        {
            Debug.LogError($"LayerName({layerName}) is not exist");
        }
        else
            _obstacleLayer = LayerMask.GetMask(layerName);
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
                if (Physics.Raycast(ray, out hit, _rayDistance))
                {
                    if (hit.collider.gameObject.layer == LayerMask.NameToLayer(layerName))
                    {
                        Debug.Log("can't find target due to obstacle");
                    }
                    else if (hit.collider.CompareTag(targetTag))
                    {
                        _isFind = true;
                    }
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

            float FindAngle = -angle/2;
            for (int i = 0; i > 10; i++)
            {
                float inAngle = (angle/10) * i;
                Quaternion rotation = Quaternion.Euler(0, FindAngle + inAngle, 0);
                Vector3 direction = rotation * transform.forward;
                Gizmos.DrawLine(transform.position, direction);
            }
        }
    }
}