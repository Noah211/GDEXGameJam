using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Author: Noah Logan

public class SavePoint : MonoBehaviour
{
    public Animator animator;
    public string SceneNameToRestart;
    public int maxMoves;

    private GameObject canvas;
    private bool isSaved;

    // Start is called before the first frame update
    void Start()
    {
        canvas = canvas = GameObject.Find("Canvas");
        animator.SetBool("IsSaved", false);
        isSaved = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Save()
    {
        if (!isSaved)
        {
            canvas.GetComponent<GameUI>().ChangeSceneNameToRestart(SceneNameToRestart);
            canvas.GetComponent<GameUI>().ChangeMaxMoves(maxMoves);
            animator.SetBool("IsSaved", true);
            isSaved = true;
        }
    }
}
