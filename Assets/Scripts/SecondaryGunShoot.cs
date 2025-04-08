using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecondaryGunShoot : MonoBehaviour
{
    public int damagePerShot = 20;                  // The damage inflicted by each bullet.
    public float timeBetweenBullets = 0.15f;        // The time between each shot.
    public float range = 100f;                      // The distance the gun can fire.

    float timer;                                    // A timer to determine when to fire.
    Ray shootRay = new Ray();                       // A ray from the gun end forwards.
    RaycastHit shootHit;                            // A raycast hit to get information about what was hit.
    ParticleSystem gunParticles;                    // Reference to the particle system.
    LineRenderer gunLine;                           // Reference to the line renderer.
    float effectsDisplayTime = 2.5f;                // The proportion of the timeBetweenBullets that the effects will display for.

    private int shootableMask;

    public GameObject lightningPrefab;

    void Awake()
    {
        // Set up the references.
        gunParticles = GetComponent<ParticleSystem>();
        gunLine = GetComponent<LineRenderer>();
        
        shootableMask = LayerMask.GetMask("shootable");
    }


    void Update()
    {
        // Add the time since Update was last called to the timer.
        timer += Time.deltaTime;

        // If the Fire1 button is being press and it's time to fire...
        if (Input.GetMouseButton(1) && timer >= timeBetweenBullets && Time.timeScale != 0)
        {
            // ... shoot the gun.
            Shoot();
        }

        // If the timer has exceeded the proportion of timeBetweenBullets that the effects should be displayed for...
        if (timer >= timeBetweenBullets * effectsDisplayTime)
        {
            // ... disable the effects.
            DisableEffects();
        }
    }


    public void DisableEffects()
    {
        // Disable the line renderer and the light.
        gunLine.enabled = false;
    }


    void Shoot()
    {
        // Reset the timer.
        timer = 0f;

        // Stop the particles from playing if they were, then start the particles.
        gunParticles.Stop();
        gunParticles.Play();

        // Enable the line renderer and set it's first position to be the end of the gun.
        gunLine.enabled = true;
        gunLine.SetPosition(0, transform.position);

        // Set the shootRay so that it starts at the end of the gun and points forward from the barrel.
        shootRay.origin = transform.position;
        shootRay.direction = transform.forward;

        // Perform the raycast against gameobjects on the shootable layer and if it hits something...
        // If it hits something, set the second position of the line renderer to the point the raycast hit, otherwise, 
        // set the second position of the line renderer to the maximal raycast range.

        // Your code here.
        RaycastHit hit;
        
        Vector3 shootDirection = transform.forward + new Vector3(0f, -0.05f, 0f);
        shootDirection.Normalize();

        if (Physics.Raycast(transform.position, shootDirection, out hit, range, shootableMask))
        {
            // Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.red);
            Debug.Log("Hit");
            
            gunLine.SetPosition(1, hit.point);
            
            GameObject hitGameObject = hit.collider.gameObject;
            Debug.Log(hitGameObject.name);

            if (hitGameObject.CompareTag("enemytank"))
            {
                Debug.Log("Hit enemytank");
                GameObject lightning = Instantiate(lightningPrefab, hit.point, Quaternion.Euler(-90f, 0f, 0f));
                lightning.gameObject.transform.SetParent(hitGameObject.transform);
                AudioSource audioSource = lightning.GetComponent<AudioSource>();
                audioSource.time = 1.0f;
                audioSource.Play();
                ParticleSystem ps = lightning.GetComponent<ParticleSystem>();
                ps.Play();
                Destroy(hitGameObject, effectsDisplayTime);
            }
        }
        else
        {
            // Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * 100f, Color.blue);
            Debug.Log("not hit");
            
            gunLine.SetPosition(1, transform.position + shootDirection * range);
        }
    }
}
