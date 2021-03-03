using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // allows us to modify the UI so that we can change the coin count

public class PlayerController : Phys
{
    protected bool feather; // Feather item 
    public float jumpVelocity = 7;
    public float maxSpeed = 7;
    private Animator animator; // Player animator 
    private bool right;  // Player facing right at beginning 
    public int coinCount; // Number of Coins
    public Text coinText;
    public GameObject DeathUI;
    public GameObject WinUI;
    public GameObject PauseUI;
    private bool paused = false;

    // Awake gets called immediately the first time a GameObject is enabled.
    void Awake()
    {
        animator = GetComponent<Animator>(); 
        right = true;
    }
    

    protected override void CalculateVelocity()
    {
        Vector2 move = Vector2.zero;
        move.x = Input.GetAxis("Horizontal");
        if(Input.GetButtonDown ("Jump") && grounded) // If player is grounded they can jump 
        {
            velocity.y = jumpVelocity;
            featherUse += 1;
        }
        else if(Input.GetButtonUp("Jump")) // If player lets go of jump key early their jump velocity gets halved 
        {
          if (velocity.y > 0)
            {
                velocity.y = velocity.y * 0.5f;
            }
        } 
        else if(Input.GetButtonDown("Jump") && feather && featherUse < 1) // If player picks up feather item they can jump twice 
        {
            velocity.y = jumpVelocity;
            featherUse += 1;
        }

        if(move.x > 0 && !right || move.x < 0 && right) // Changes the direction the character is facing 
        {
            right = !right;
            Vector3 newScale = transform.localScale;
            newScale.x *= -1;
            transform.localScale = newScale;
        }

        animator.SetBool("grounded", grounded);  // Tells animator whether or not player is grounded
        animator.SetFloat("velocityX", Mathf.Abs(velocity.x)); // Tells animator whether or not player is moving on X

        realVelocity = move * maxSpeed;

        if (Input.GetButtonDown("Pause"))
        {
            paused = !paused;
        }

        if (paused == true)
        {
            PauseUI.SetActive(true);
            Time.timeScale = 0; // Time is Paused
        }

        if (paused == false)
        {
            PauseUI.SetActive(false);
            Time.timeScale = 1;
        }
    }

    public void Resume()
    {
        paused = false;
    }

    protected void OnCollisionEnter2D(Collision2D collider) // OnCollision works when two colliders hit each other (coin and player or feather and player)
    {
        if(collider.gameObject.tag == "Coin")
        {
            coinCount += 1;
            Destroy(collider.gameObject);
            coinText.text = "" + coinCount;
        }

        if(collider.gameObject.tag == "Feather")
        {
            Destroy(collider.gameObject);
            feather = true;
        }

        if(collider.gameObject.tag == "Spikes")
        {
            animator.SetBool("hurt", true); // Tells animator to play death animation when a spike is touched
            DeathUI.SetActive(true);
            Time.timeScale = 0;
        }

        if(collider.gameObject.tag == "Trophy")
        {
            WinUI.SetActive(true);
            Time.timeScale = 0;
        }

    }

    public void Respawn()
    {
       int tempCoins = coinCount - 5;
        if (coinCount >= 5)
        {
            Application.LoadLevel(Application.loadedLevel);
            coinCount = tempCoins;
        }
    }

    public void Save()
    {
        if (coinCount > PlayerPrefs.GetInt("highscore"))
        {
            PlayerPrefs.SetInt("highscore", coinCount);
        }
    }
}
