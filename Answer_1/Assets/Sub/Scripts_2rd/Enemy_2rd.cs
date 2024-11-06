using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_2rd : MonoBehaviour
{
    [SerializeField]
    List<Material> color;
    void Start()
    {
        MeshRenderer myMaterial = GetComponent<MeshRenderer>();
        while(true)
        {
            if (GameObject.FindGameObjectsWithTag("E_B").Length <= 2)
            {
                myMaterial.material = color[0];
                gameObject.tag = "E_B";
                break;
            }
            else if (GameObject.FindGameObjectsWithTag("E_G").Length <= 2)
            {
                myMaterial.material = color[1];
                gameObject.tag = "E_G";
                break;
            }
            else if (GameObject.FindGameObjectsWithTag("E_Y").Length <= 2)
            {
                myMaterial.material = color[2];
                gameObject.tag = "E_Y";
                break;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
