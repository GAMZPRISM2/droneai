using UnityEngine;

public class JoystickTarget : MonoBehaviour
{
    public float speed = 10f; // Scaling factor for joystick values
    public float rotationSpeed = 100f;
    public float maxAltitude = 120f;
    public float minAltitude = 0f;

    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        // Get joystick input values
        float leftJoystickY = -Input.GetAxis("Vertical");
        float leftJoystickX = -Input.GetAxis("Horizontal");
        float rightJoystickY = -Input.GetAxis("RightVertical");
        float rightJoystickX = -Input.GetAxis("RightHorizontal");

        // Map joystick values to control movements
        float altitude = Mathf.Clamp(transform.position.y + leftJoystickY * speed, minAltitude, maxAltitude);
        float rotation = leftJoystickX * rotationSpeed;
        Vector3 movement = new Vector3(rightJoystickX * speed, 0, rightJoystickY * speed);

        // Apply control movements to the drone
        rb.velocity = Quaternion.Euler(0, rotation, 0) * movement;
        transform.position = new Vector3(transform.position.x, altitude, transform.position.z);
    }
}
