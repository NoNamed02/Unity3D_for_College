using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy_2rd : MonoBehaviour
{
    [SerializeField]
    List<Material> color;
    private NavMeshAgent agent;
    private Transform target; // 목표가 되는 가장 가까운 오브젝트

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        Debug.Log($"{GetInstanceID()}, {gameObject.GetInstanceID()}");
    }

    void Update()
    {
        FindClosestTargetWithTag();

        if (target != null)
        {
            agent.SetDestination(target.position);
        }
    }

    void FindClosestTargetWithTag()
    {
        GameObject[] taggedObjects = GameObject.FindGameObjectsWithTag(gameObject.tag);
        GameObject player = GameObject.FindWithTag("Player");
        float closestDistance = Mathf.Infinity;
        target = null;

        foreach (GameObject obj in taggedObjects)
        {
            if (obj == gameObject) continue;

            float distance = Vector3.Distance(transform.position, obj.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                target = obj.transform;
            }
        }

        float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
        if (distanceToPlayer < closestDistance)
        {
            target = player.transform;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag(gameObject.tag))
        {
            if (collision.gameObject.GetInstanceID() < gameObject.GetInstanceID())
            {
                transform.localScale += new Vector3(0.3f, 0.3f, 0.3f);
                Debug.Log($"collision object = {collision.gameObject.GetInstanceID()} this object = {gameObject.GetInstanceID()}");
                Destroy(collision.gameObject);
            }
            else
            {
                collision.transform.localScale += new Vector3(0.3f, 0.3f, 0.3f);
                Debug.Log($"collision object = {collision.gameObject.GetInstanceID()} this object = {gameObject.GetInstanceID()}");
                Destroy(gameObject);
            }
        }
    }
}
