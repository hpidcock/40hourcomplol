using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Killing : MonoBehaviour
{
    void Start()
    {
    }

    void Update()
    {
    }

    void OnTriggerEnter(Collider obj)
    {
        Debug.Log("omg omg omg");

        Player player = obj.GetComponent<Player>() ;

        if (player != null)
        {
            player.Kill();
        }
    }
}
