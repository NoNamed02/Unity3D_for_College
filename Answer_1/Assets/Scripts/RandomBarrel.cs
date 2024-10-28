using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomBarrel : MonoBehaviour
{
    public List<Material> materials;
    private MeshRenderer _meshRenderer;
    public int hit_count = 0;
    private Rigidbody _rigidbody;

    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        
        _meshRenderer = GetComponent<MeshRenderer>();
        _meshRenderer.material = materials[Random.Range(0, materials.Count)];
    }
    void Update()
    {


        if (hit_count == 3)
        {
            Collider[] colls = Physics.OverlapSphere(transform.position, 10f);
            foreach(Collider coll in colls)
            {
                Rigidbody rb = coll.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.AddExplosionForce(1000f, transform.position, 10f, 300f);
                }
            }
            Destroy(gameObject);
        }
    }
}
