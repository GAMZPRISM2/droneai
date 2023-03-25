using UnityEngine;
using Unity.MLAgents;

public class GroundPlane : MonoBehaviour
{
    public void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Drone"))
        {
            // End the episode
            other.gameObject.GetComponent<DroneAgent>().EndEpisode();
        }
    }
}
