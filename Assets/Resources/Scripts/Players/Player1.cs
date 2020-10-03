using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// Author: Noah Logan

public class Player1 : MonoBehaviour, IPlayer
{
    public Animator animator;
    public LayerMask blockMovement;
    public LayerMask Default;

    public bool IsOnSavePoint { get; private set; }
    public GameObject SavePoint { get; private set; }
    public Vector3 SavedPoint { get; set; }

    private GameObject player2;
    private GameObject canvas;
    private GameObject wireCabinet;
    private AudioSource itemWorks;
    private AudioSource itemDoesNotWork;
    private Vector3 movePoint;
    private Vector3 checkPlayer2Position;
    private Dictionary<string, int> items;
    private float speed;
    private string currentItem;
    private bool isNearWireCabinet;

    // Start is called before the first frame update
    void Start()
    {
        animator.SetBool("IsMoving", false);
        animator.SetFloat("Vertical", 0);
        animator.SetFloat("Horizontal", 0);

        player2 = GameObject.Find("Player2");
        canvas = GameObject.Find("Canvas");
        itemWorks = GameObject.Find("Player1ItemWorksAudio").GetComponent<AudioSource>();
        itemDoesNotWork = GameObject.Find("Player1ItemDoesNotWorkAudio").GetComponent<AudioSource>();
        movePoint = transform.position;
        checkPlayer2Position = transform.position + new Vector3(2, 0, 0);
        speed = 3f;
        IsOnSavePoint = false;
        isNearWireCabinet = false;
        SavedPoint = transform.position;
        currentItem = "";

        items = new Dictionary<string, int>
        {
            {"Drill", 0},
            {"Wires", 0},
            {"Wrench", 0}
        };
    }

    // Update is called once per frame
    void Update()
    {
        Move();

        if (Input.GetKeyDown(KeyCode.F) && items.ContainsKey(currentItem))
        {
            UseItem();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("Quit");
            Application.Quit();
        }

        if (Input.GetKeyDown(KeyCode.Y))
        {
            SceneManager.LoadScene(canvas.GetComponent<GameUI>().RestartScene, LoadSceneMode.Single);
        }
    }

    void FixedUpdate()
    {
        
    }

    public void Move()
    {
        transform.position = Vector3.MoveTowards(transform.position, movePoint, speed * Time.deltaTime);
        checkPlayer2Position = transform.position + new Vector3(2, 0, 0);

        if (Vector3.Distance(transform.position, movePoint) == 0)
        {
            animator.SetBool("IsMoving", false);

            if (Input.GetKeyDown(KeyCode.W))
            {
                movePoint = transform.position + new Vector3(0, 1, 0);
                animator.SetFloat("Vertical", 1);
                animator.SetFloat("Horizontal", 0);
            }
            else if (Input.GetKeyDown(KeyCode.S))
            {
                movePoint = transform.position + new Vector3(0, -1, 0);
                animator.SetFloat("Vertical", -1);
                animator.SetFloat("Horizontal", 0);
            }
            else if (Input.GetKeyDown(KeyCode.A))
            {
                movePoint = transform.position + new Vector3(-1, 0, 0);
                animator.SetFloat("Vertical", 0);
                animator.SetFloat("Horizontal", -1);
            }
            else if (Input.GetKeyDown(KeyCode.D))
            {
                movePoint = transform.position + new Vector3(1, 0, 0);
                animator.SetFloat("Vertical", 0);
                animator.SetFloat("Horizontal", 1);
            }

            Collider2D movableBlockCollider = Physics2D.OverlapCircle(movePoint, 0.2f, blockMovement);
            Collider2D terrainCollider = Physics2D.OverlapCircle(movePoint + (movePoint - transform.position), 0.2f, blockMovement);

            if ((terrainCollider == null) && (movableBlockCollider != null) && movableBlockCollider.gameObject.CompareTag("MovableBlock"))
            {
                animator.SetBool("IsMoving", true);
                canvas.GetComponent<GameUI>().ChangeRemainingMoves(1);
                movableBlockCollider.transform.position += (movePoint - transform.position);
            }
            else if (Physics2D.OverlapCircle(movePoint, 0.2f, blockMovement))
            {
                movePoint = transform.position;
                animator.SetBool("IsMoving", false);
            }
            else if (movePoint != transform.position)
            {
                canvas.GetComponent<GameUI>().ChangeRemainingMoves(1);
                animator.SetBool("IsMoving", true);
            }
            else
            {
                movePoint = transform.position;
                animator.SetBool("IsMoving", false);
            }
        }
    }

    public void PickupItem(string itemName)
    {
        if (items.ContainsKey(itemName)) 
        {
            items[itemName] += 1;
            currentItem = itemName;
            canvas.GetComponent<GameUI>().ChangePlayer1Item(itemName, items[itemName]);
        }
    }

    public void UseItem()
    {
        Collider2D otherCollider = Physics2D.OverlapCircle(checkPlayer2Position, 0.2f, Default);

        if ((otherCollider != null) && otherCollider.gameObject.CompareTag("Player"))
        {
            itemWorks.Play();
            player2.GetComponent<Player2>().PickupItem(currentItem);
            items[currentItem] -= 1;
            canvas.GetComponent<GameUI>().ChangePlayer1Item(currentItem, items[currentItem]);
            currentItem = "";
        }
        else if (isNearWireCabinet && (currentItem.Equals("Drill") || currentItem.Equals("Wires")))
        {
            itemWorks.Play();
            wireCabinet.GetComponent<WireCabinet>().TakeItem(currentItem);
            items[currentItem] -= 1;
            canvas.GetComponent<GameUI>().ChangePlayer1Item(currentItem, items[currentItem]);
            currentItem = "";
        }
        else
        {
            itemDoesNotWork.Play();
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.otherCollider.gameObject.CompareTag("Player"))
        {
            SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        switch(other.gameObject.tag)
        {
            case "SavePoint":
                IsOnSavePoint = true;
                SavePoint = other.gameObject;

                if (IsOnSavePoint && player2.GetComponent<Player2>().IsOnSavePoint)
                {
                    SavedPoint = SavePoint.transform.position;
                    SavePoint.GetComponent<SavePoint>().Save();
                    player2.GetComponent<Player2>().SavedPoint = player2.GetComponent<Player2>().SavePoint.transform.position;
                    player2.GetComponent<Player2>().SavePoint.GetComponent<SavePoint>().Save();
                }
                break;
            case "WireCabinet":
                isNearWireCabinet = true;
                wireCabinet = other.gameObject.transform.parent.gameObject;
                break;
            case "IItem":
                PickupItem(other.gameObject.name);
                break;
            default:
                break;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("SavePoint"))
        {
            IsOnSavePoint = false;
        }
        else if (other.gameObject.CompareTag("WireCabinet"))
        {
            isNearWireCabinet = false;
        }
    }


}
