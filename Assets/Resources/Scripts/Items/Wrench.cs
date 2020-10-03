using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Author: Noah Logan

public class Wrench : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        gameObject.name = "Wrench";
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
    }
}
