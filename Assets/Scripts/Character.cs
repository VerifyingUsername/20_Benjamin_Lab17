using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Character : MonoBehaviour
{
    public float moveSpeed;
    public float jumpForce;
    public int jumpCount;

    public GameObject HealthText;
    public GameObject CoinText;

    public int healthCount;
    public int coinCount;

    private Rigidbody2D rb;
    private Animator animator;

    private AudioSource audioSource;
    public AudioClip CoinClip;
    public AudioClip JumpClip;
    public AudioClip HitClip;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        audioSource = GetComponent<AudioSource>();

        HealthText.GetComponent<Text>().text = "Health: " + healthCount;
        CoinText.GetComponent<Text>().text = "Coin: " + coinCount;
    }

    // Update is called once per frame
    void Update()
    {
        float hVelocity = 0; //For left right movement
        float vVelocity = 0; //For jump

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            hVelocity = -moveSpeed;
            animator.SetFloat("xVelocity", Mathf.Abs(hVelocity));
            transform.localScale = new Vector3(-1, 1, 1);
        }
        else if (Input.GetKeyUp(KeyCode.LeftArrow))
        {
            animator.SetFloat("xVelocity", 0);
        }

        if (Input.GetKey(KeyCode.RightArrow))
        {
            hVelocity = moveSpeed;
            animator.SetFloat("xVelocity", Mathf.Abs(hVelocity));
            transform.localScale = new Vector3(1, 1, 1);
        }
        else if (Input.GetKeyUp(KeyCode.RightArrow))
        {
            animator.SetFloat("xVelocity", 0);
        }

        if (Input.GetKeyDown(KeyCode.Space) && jumpCount < 2)
        {
            jumpCount += 1;
            audioSource.clip = JumpClip;
            audioSource.Play();
            vVelocity = jumpForce;
            animator.SetTrigger("JumpTrigger");
        }

        // WIn COndition
        if (coinCount == 3)
        {
            SceneManager.LoadScene("WinScene");
        }

        // Lose COndition
        if (healthCount <= 0)
        {
            SceneManager.LoadScene("LoseScene");
        }

        hVelocity = Mathf.Clamp(rb.velocity.x + hVelocity, -5, 5);

        rb.velocity = new Vector2(hVelocity, rb.velocity.y + vVelocity);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            jumpCount = 0;
        }

        if (collision.gameObject.tag == "Enemy")
        {
            healthCount -= 50;
            audioSource.clip = HitClip;
            audioSource.Play();
            HealthText.GetComponent<Text>().text = "Health: " + healthCount;
        }

        if (collision.gameObject.tag == "Coin")
        {
            coinCount += 1;
            audioSource.clip = CoinClip;
            audioSource.Play();
            Destroy(collision.gameObject);
            CoinText.GetComponent<Text>().text = "Coin: " + coinCount;
        }
    }
}
