using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// Author: Noah Logan

public class GameUI : MonoBehaviour
{
    public Image Player1Item;
    public Image Player2Item;
    public Text MoveText;
    public Sprite Drill;
    public Sprite Wires;
    public Sprite Wrench;

    public string RestartScene { get; private set; }

    private GameObject player1;
    private GameObject player2;
    private int maxMoves;
    private int remainingMoves;

    // Start is called before the first frame update
    void Start()
    {
        player1 = GameObject.Find("Player1");
        player2 = GameObject.Find("Player2");
        Player1Item.enabled = false;
        Player2Item.enabled = false;
        MoveText.text = "Remaining Moves: 0";
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangePlayer1Item(string itemName, int itemCount)
    {
        if (itemCount > 0)
        {
            switch (itemName)
            {
                case "Drill":
                    Player1Item.sprite = Drill;
                    Player1Item.enabled = true;
                    break;
                case "Wires":
                    Player1Item.sprite = Wires;
                    Player1Item.enabled = true;
                    break;
                case "Wrench":
                    Player1Item.sprite = Wrench;
                    Player1Item.enabled = true;
                    break;
                default:
                    break;
            }
        }
        else
        {
            Player1Item.enabled = false;
        }
    }

    public void ChangePlayer2Item(string itemName, int itemCount)
    {
        if (itemCount > 0)
        {
            switch (itemName)
            {
                case "Drill":
                    Player2Item.sprite = Drill;
                    Player2Item.enabled = true;
                    break;
                case "Wires":
                    Player2Item.sprite = Wires;
                    Player2Item.enabled = true;
                    break;
                case "Wrench":
                    Player2Item.sprite = Wrench;
                    Player2Item.enabled = true;
                    break;
                default:
                    break;
            }
        }
        else
        {
            Player2Item.enabled = false;
        }
    }

    public void ChangeMaxMoves(int amount)
    {
        maxMoves = amount;
        remainingMoves = maxMoves;
        MoveText.text = "Remaining Moves: " + remainingMoves;
    }

    public void ChangeRemainingMoves(int amount)
    {
        remainingMoves = Mathf.Clamp(remainingMoves - amount, 0, maxMoves);

        if (remainingMoves == 0)
        {
            SceneManager.LoadScene(RestartScene, LoadSceneMode.Single);

            /*
            remainingMoves = maxMoves;
            player1.transform.position = player1.GetComponent<Player1>().SavedPoint;
            player2.transform.position = player2.GetComponent<Player2>().SavedPoint;
            */
        }

        MoveText.text = "Remaining Moves: " + remainingMoves;
    }

    public void ChangeSceneNameToRestart(string sceneName)
    {
        RestartScene = sceneName;
    }
}
