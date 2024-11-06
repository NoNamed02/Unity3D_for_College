using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomPos : MonoBehaviour
{
    void Start()
    {
        transform.position = new Vector3(Random.Range(-5, 5), 1, Random.Range(-5, 5));
    }
}
