using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Author: Noah Logan

public class DoorPressurePlate : MonoBehaviour
{
    public Animator animator;
    public GameObject Door;

    // Start is called before the first frame update
    void Start()
    {
        animator.SetBool("IsPressed", false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player") || other.gameObject.CompareTag("MovableBlock"))
        {
            animator.SetBool("IsPressed", true);
            Door.GetComponent<Door>().Open();
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player") || other.gameObject.CompareTag("MovableBlock"))
        {
            animator.SetBool("IsPressed", false);
            Door.GetComponent<Door>().Close();
        }
    }
}
