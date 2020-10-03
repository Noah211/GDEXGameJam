using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// Author: Noah Logan

public class Player2 : MonoBehaviour, IPlayer
{
    public Animator animator;
    public LayerMask blockMovement;
    public LayerMask Default;

    public bool IsOnSavePoint { get; private set; }
    public GameObject SavePoint { get; private set; }
    public Vector3 SavedPoint { get; set; }

    private GameObject player1;
    private GameObject canvas;
    private GameObject wireCabinet;
    private AudioSource itemWorks;
    private AudioSource itemDoesNotWork;
    private Vector3 movePoint;
    private Vector3 checkPlayer1Position;
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

        player1 = GameObject.Find("Player1");
        canvas = GameObject.Find("Canvas");
        itemWorks = GameObject.Find("Player2ItemWorksAudio").GetComponent<AudioSource>();
        itemDoesNotWork = GameObject.Find("Player2ItemDoesNotWorkAudio").GetComponent<AudioSource>();
        movePoint = transform.position;
        checkPlayer1Position = transform.position + new Vector3(-2, 0, 0);
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

        if (Input.GetKeyDown(KeyCode.RightControl) && items.ContainsKey(currentItem))
        {
            UseItem();
        }
    }

    void FixedUpdate()
    {

    }

    public void Move()
    {
        transform.position = Vector3.MoveTowards(transform.position, movePoint, speed * Time.deltaTime);
        checkPlayer1Position = transform.position + new Vector3(-2, 0, 0);

        if (Vector3.Distance(transform.position, movePoint) == 0)
        {
            animator.SetBool("IsMoving", false);

            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                movePoint = transform.position + new Vector3(0, 1, 0);
                animator.SetFloat("Vertical", 1);
                animator.SetFloat("Horizontal", 0);
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                movePoint = transform.position + new Vector3(0, -1, 0);
                animator.SetFloat("Vertical", -1);
                animator.SetFloat("Horizontal", 0);
            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                movePoint = transform.position + new Vector3(-1, 0, 0);
                animator.SetFloat("Vertical", 0);
                animator.SetFloat("Horizontal", -1);
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow))
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
                animator.SetBool("IsMoving", true);
                canvas.GetComponent<GameUI>().ChangeRemainingMoves(1);
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
            canvas.GetComponent<GameUI>().ChangePlayer2Item(itemName, items[itemName]);
        }
    }

    public void UseItem()
    {
        Collider2D otherCollider = Physics2D.OverlapCircle(checkPlayer1Position, 0.2f, Default);

        if ((otherCollider != null) && otherCollider.gameObject.CompareTag("Player"))
        {
            itemWorks.Play();
            player1.GetComponent<Player1>().PickupItem(currentItem);
            items[currentItem] -= 1;
            canvas.GetComponent<GameUI>().ChangePlayer2Item(currentItem, items[currentItem]);
            currentItem = "";
        }
        else if (isNearWireCabinet && (currentItem.Equals("Drill") || currentItem.Equals("Wires")))
        {
            itemWorks.Play();
            wireCabinet.GetComponent<WireCabinet>().TakeItem(currentItem);
            items[currentItem] -= 1;
            canvas.GetComponent<GameUI>().ChangePlayer2Item(currentItem, items[currentItem]);
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
        switch (other.gameObject.tag)
        {
            case "SavePoint":
                IsOnSavePoint = true;
                SavePoint = other.gameObject;

                if (IsOnSavePoint && player1.GetComponent<Player1>().IsOnSavePoint)
                {
                    SavedPoint = SavePoint.transform.position;
                    SavePoint.GetComponent<SavePoint>().Save();
                    player1.GetComponent<Player1>().SavedPoint = player1.GetComponent<Player1>().SavePoint.transform.position;
                    player1.GetComponent<Player1>().SavePoint.GetComponent<SavePoint>().Save();
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
