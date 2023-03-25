using UnityEngine;

public class DroneMotorControl : MonoBehaviour
{
    public Rigidbody[] motors; // Array to store references to each of the motors
    public float[] motorForces; // Array to store the motor forces for each motor

    // Drag each motor GameObject into the Inspector view to populate the array of motors
    // Set the desired force value for each motor in the array of motorForces

    private void FixedUpdate()
    {
        // Loop through each motor Rigidbody component in the array
        for (int i = 0; i < motors.Length; i++)
        {
            // Call the AddRelativeForce method on the Rigidbody component with the corresponding force value
            motors[i].AddRelativeForce(Vector3.up * motorForces[i]);
        }
    }
}
