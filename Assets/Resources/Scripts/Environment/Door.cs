using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Author: Noah Logan

public class Door : MonoBehaviour
{
    public Animator animator;

    private BoxCollider2D doorCollider;

    // Start is called before the first frame update
    void Start()
    {
        animator.SetBool("IsOpen", false);
        doorCollider = gameObject.transform.GetChild(1).GetComponent<BoxCollider2D>();
        doorCollider.enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Open()
    {
        animator.SetBool("IsOpen", true);
        doorCollider.enabled = false;
    }

    public void Close()
    {
        animator.SetBool("IsOpen", false);
        doorCollider.enabled = true;
    }
}
