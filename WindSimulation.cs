using UnityEngine;

public class WindSimulation : MonoBehaviour
{
    public float windForce = 10.0f;
    public float windVariability = 0.1f;
    public float maxWindForce = 35.0f;

    private System.Random rand;

    private void Start()
    {
        rand = new System.Random();
    }

    private Vector3 GenerateRandomWindForce()
    {
        Vector3 windDirection = new Vector3((float)rand.NextDouble() * 2.0f - 1.0f,
                                            (float)rand.NextDouble() * 2.0f - 1.0f,
                                            (float)rand.NextDouble() * 2.0f - 1.0f);
        windDirection = windDirection.normalized * windForce;
        windDirection = new Vector3(windDirection.x + ((float)rand.NextDouble() * 2.0f - 1.0f) * windVariability,
                                    windDirection.y + ((float)rand.NextDouble() * 2.0f - 1.0f) * windVariability,
                                    windDirection.z + ((float)rand.NextDouble() * 2.0f - 1.0f) * windVariability);
        windDirection = windDirection.normalized * Mathf.Min(maxWindForce, windDirection.magnitude);
        return windDirection;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.GetComponent<Rigidbody>() != null)
        {
            Vector3 windDirection = GenerateRandomWindForce();
            other.GetComponent<Rigidbody>().AddForce(windDirection, ForceMode.Force);
        }
    }

    public void EndEpisode()
    {
        rand = new System.Random();
    }
}
