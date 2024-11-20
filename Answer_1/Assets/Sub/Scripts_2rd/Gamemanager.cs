using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class Gamemanager : MonoBehaviour
{
    public static Gamemanager Instance;
    public Transform [] spwanpoint;
    public Material [] materials;
    public GameObject monsterPrefab;

    void Awake() {
        if (Instance != null)
        {
            Destroy(Instance);
            Instance = this;
        }
        else Instance = this;
    }

    void Start()
    {
        //monsterPrefab = Resources.Load<GameObject>("TurtleShellEnemy");
        for (int i = 0; i < spwanpoint.Length; i ++)
        {
            GameObject Monster = Instantiate(monsterPrefab, spwanpoint[i].position, Quaternion.identity);
            //int materialIndex = Random.Range(0, materials.Length);
            //Monster.GetComponent<MeshRenderer>().material = materials[materialIndex];
            switch (Random.Range(0,3))
            {
                case 0:
                    Monster.tag = "E_B";
                    break;
                case 1:
                    Monster.tag = "E_G";
                    break;
                case 2:
                    Monster.tag = "E_Y";
                    break;
            }
        }
    }
}
