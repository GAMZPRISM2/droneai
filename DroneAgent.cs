using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

public class DroneAgent : Agent
{
    public Rigidbody[] motors;
    public float[] motorForces;

    public Transform spawnPoint;

    private const int BUFFER_SIZE = 10;
    private readonly Queue<float> altitudeBuffer = new Queue<float>(BUFFER_SIZE);
    private float lastAltitude;

    private Rigidbody rb;
    private float accelerationX, accelerationY, accelerationZ;
    private float gyroX, gyroY, gyroZ;
    private Transform droneTransform;

    public float HoverReward;
    public float FlipReward;

    public float altitudeThreshold;
    

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        droneTransform = transform;
    }

    public override void OnEpisodeBegin()
    {
        // Reset the drone's position and rotation
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        droneTransform.position = spawnPoint.position;
        droneTransform.rotation = Quaternion.identity;

        // Clear the altitude buffer
        altitudeBuffer.Clear();
        // Make collisions faster
        Time.fixedDeltaTime = 0.005f;
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        // Add the sensor data to the observation space
        sensor.AddObservation(accelerationX);
        sensor.AddObservation(accelerationY);
        sensor.AddObservation(accelerationZ);
        sensor.AddObservation(gyroX);
        sensor.AddObservation(gyroY);
        sensor.AddObservation(gyroZ);
        sensor.AddObservation(droneTransform.position.y);
        Vector3 currentPosition = droneTransform.position;
        Vector3 targetPosition = GameObject.Find("Sphere").transform.position;
        float distance = Mathf.Abs((currentPosition - targetPosition).magnitude);
        sensor.AddObservation(distance);
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("WindZone"))
        {
            Debug.Log("Drone has left the WindZone collider");
            SetReward(-1f);
            EndEpisode();
        }

        if (other.gameObject.CompareTag("Ground"))
        {
            Debug.Log("Drone has collided with the Ground");
            SetReward(-1f);
            EndEpisode();
        }
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        // Get the motor forces from the action space
        var continuousActions = actions.ContinuousActions;
        for (int i = 0; i < motors.Length; i++)
        {
            motorForces[i] = Mathf.Clamp(continuousActions[i], -1f, 1f);
        }

        // Apply the motor forces to the drone
        for (int i = 0; i < motors.Length; i++)
        {
            motors[i].AddRelativeForce(Vector3.up * motorForces[i]);
        }

        // Update the sensor data
        accelerationX = rb.velocity.x / Time.deltaTime;
        accelerationY = rb.velocity.y / Time.deltaTime;
        accelerationZ = rb.velocity.z / Time.deltaTime;

        accelerationX = accelerationX / 9.8f;
        accelerationY = accelerationY / 9.8f;
        accelerationZ = accelerationZ / 9.8f;

        gyroX = droneTransform.rotation.eulerAngles.x;
        gyroY = droneTransform.rotation.eulerAngles.y;
        gyroZ = droneTransform.rotation.eulerAngles.z;
        
        // Define altitude
        float altitude = droneTransform.position.y;

        // Read button inputs
        bool xButton = Input.GetButtonDown("X Button");
        bool aButton = Input.GetButtonDown("A Button");
        bool yButton = Input.GetButtonDown("Y Button");
        bool bButton = Input.GetButtonDown("B Button");

        // Read joystick inputs
        float leftJoystickHorizontal = Input.GetAxis("Horizontal");
        //float leftJoystickVertical = Input.GetAxis("Vertical");
        float leftJoystickVertical = 1f;
        float rightJoystickHorizontal = Input.GetAxis("Horizontal 2");
        float rightJoystickVertical = Input.GetAxis("Vertical 2");

        // Add altitude data to buffer
        altitudeBuffer.Enqueue(altitude);
        if (altitudeBuffer.Count > BUFFER_SIZE)
        {
            altitudeBuffer.Dequeue();
        }

        // Calculate the average altitude in the buffer
        float averageAltitude = altitudeBuffer.Average();

        // Calculate the deviation from the average altitude
        float altitudeDeviation = Mathf.Abs(altitude - averageAltitude);

        // Joystick Rewards
        
         //Weird the drone flys stable EndEpisode();
        if (leftJoystickVertical > 0 )
        {
            Vector3 currentPosition = droneTransform.position;
            Vector3 targetPosition = GameObject.Find("Sphere").transform.position;
            float distance = Mathf.Abs((currentPosition - targetPosition).magnitude);
            float reward = 1.0f / (1.0f + distance);
            SetReward(reward);
        }

        //else if (leftJoystickVertical < 0)
        {
            //float downreward = Mathf.Abs(-altitudeDeviation * leftJoystickVertical);
            //downreward = Mathf.Clamp(downreward, 0.0f, 1.0f);
            //SetReward(downreward);
        }

    
        // Hovering Rewards
        //if (altitudeDeviation <= altitudeThreshold)
        {
            //SetReward(HoverReward);
        }


        //if (aButton)
        {
            //Debug.Log("Do a Flip");
            //SetReward(FlipReward);
        }
}
}
