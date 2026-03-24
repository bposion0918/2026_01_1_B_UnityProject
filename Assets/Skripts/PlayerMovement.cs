using JetBrains.Rider.Unity.Editor;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float movespeed = 5.0f;
    public float jumpforce = 5.0f;

    public Rigidbody rb;

    public bool isGrounded = true;

    public int coinCount = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        rb.linearVelocity = new Vector3(moveHorizontal * movespeed, rb.linearVelocity.y, moveVertical * movespeed);


        if(Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpforce, ForceMode.Impulse);
            isGrounded = false;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            isGrounded = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Coin"))
        {
            coinCount++;
            Destroy(other.gameObject);
        }
    }
}
