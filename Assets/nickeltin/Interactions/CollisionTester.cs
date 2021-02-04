using UnityEngine;

public class CollisionTester : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other);
    }

    private void OnCollisionEnter(Collision other)
    {
         Debug.Log(other);
    }
}
