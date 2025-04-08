using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShellCollision : MonoBehaviour
{
    public GameObject explosionParticlesPrefab;

    private void OnCollisionEnter(Collision collision)
    {
        // Destroy the shell if it hits something (e.g. rock, ground, enemytank, etc.). 
        // If the shell hits enemy tank, the enemy tank will also be destroyed.

        // Your code here.
        if (collision.gameObject.CompareTag("enemytank"))
        {
            Debug.Log("enemytank");
            GameObject explosionParticles = Instantiate(explosionParticlesPrefab);
            explosionParticles.transform.SetParent(collision.gameObject.transform);
            explosionParticles.transform.localPosition = Vector3.zero;
            explosionParticles.transform.localRotation = Quaternion.identity;
            ParticleSystem ps = explosionParticles.GetComponent<ParticleSystem>();
            if (ps != null)
            {
                Debug.Log("Play");
                ps.Play();
            }
            
            Destroy(collision.gameObject, 0.1f);
        }
        Destroy(this.gameObject);
    }

}
