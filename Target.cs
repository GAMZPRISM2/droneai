using UnityEngine;
using System.Collections;


public class Target : MonoBehaviour
{
    public float moveSpeed = 2.0f;
    public float rotateSpeed = 90.0f;
    public float minHeight = 1.0f;
    public float maxHeight = 10.0f;
    public float randomizeHeightInterval = 120.0f;

    private Rigidbody rb;

    private IEnumerator Start()
    {
        // Get the Rigidbody component
        rb = GetComponent<Rigidbody>();

        // Start the coroutine to randomize the height
        yield return StartCoroutine(RandomizeHeight());
    }

    private IEnumerator RandomizeHeight()
    {
        while (true)
        {
            // Wait for 2 minutes
            yield return new WaitForSeconds(randomizeHeightInterval);

            // Randomize the height
            float height = Random.Range(minHeight, maxHeight);
            transform.position = new Vector3(transform.position.x, height, transform.position.z);
        }
    }

    void FixedUpdate()
    {
        // Get the joystick input values
        float leftJoystickHorizontal = Input.GetAxis("Horizontal");
        float leftJoystickVertical = Input.GetAxis("Vertical");
        float rightJoystickHorizontal = Input.GetAxis("Horizontal 2");
        float rightJoystickVertical = Input.GetAxis("Vertical 2");

        // Calculate the movement vector based on joystick inputs
        Vector3 movement = new Vector3(rightJoystickHorizontal, 0.0f, rightJoystickVertical) * moveSpeed;
        movement += Vector3.up * leftJoystickVertical * moveSpeed;

        // Apply the movement to the rigidbody velocity
        rb.velocity = movement;

        // Calculate the rotation vector based on joystick inputs
        Vector3 rotation = new Vector3(-leftJoystickVertical, leftJoystickHorizontal, -rightJoystickHorizontal) * rotateSpeed;

        // Apply the rotation to the rigidbody rotation
        rb.rotation *= Quaternion.Euler(rotation * Time.fixedDeltaTime);
    }
}
