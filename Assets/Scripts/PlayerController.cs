using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public float moveSpeed = 10;
    public float rotateSpeed = 100;
    Rigidbody playerRB;
    public float jumpForce = 500;
    // Start is called before the first frame update
    void Start()
    {
        playerRB = GetComponent<Rigidbody>();
        
    }

    // Update is called once per frame
    void Update()
    {
        float xMovement = Input.GetAxis("Horizontal") * moveSpeed * Time.deltaTime;
        float zMovement = Input.GetAxis("Vertical") * moveSpeed * Time.deltaTime;
        if (Input.GetButtonDown("Jump"))
        {
            playerRB.velocity = new Vector3(0, 0, 0);
            playerRB.AddForce(Vector3.up * jumpForce);
        }
        transform.Translate(xMovement, 0, zMovement);

        float xMouse = Input.GetAxis("Mouse X") * rotateSpeed * Time.deltaTime;
        Vector3 lookAt = new Vector3(0, xMouse, 0);
        transform.Rotate(lookAt);
    }
}
