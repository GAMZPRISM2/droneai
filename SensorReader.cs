using UnityEngine;

public class SensorReader : MonoBehaviour
{
    private Rigidbody rb;
    public float accelerationX, accelerationY, accelerationZ;
    public float gyroX, gyroY, gyroZ;
    public Transform droneTransform;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        accelerationX = rb.velocity.x / Time.deltaTime;
        accelerationY = rb.velocity.y / Time.deltaTime;
        accelerationZ = rb.velocity.z / Time.deltaTime;

        accelerationX = accelerationX / 9.8f;
        accelerationY = accelerationY / 9.8f;
        accelerationZ = accelerationZ / 9.8f;

        gyroX = transform.rotation.eulerAngles.x;
        gyroY = transform.rotation.eulerAngles.y;
        gyroZ = transform.rotation.eulerAngles.z;

        float altitude = droneTransform.position.y;

        //Debug.Log("Acceleration X: " + accelerationX + "G");
        //Debug.Log("Acceleration Y: " + accelerationY + "G");
        //Debug.Log("Acceleration Z: " + accelerationZ + "G");
        //Debug.Log("Gyro X: " + gyroX + "°/s");
        //Debug.Log("Gyro Y: " + gyroY + "°/s");
        //Debug.Log("Gyro Z: " + gyroZ + "°/s");
        //Debug.Log("Altitude: " + altitude + " meters");
    }
}
