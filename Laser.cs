using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Laser : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip impactSound;
    private Ship shipScript;
    private string[] destroyTags = { "Laser1", "Laser2", "Ship1", "Ship2" };

    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, 5);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Asteroid"))
        {
            audioSource.PlayOneShot(impactSound);
        }

        if (!destroyTags.Contains(other.gameObject.tag))
        {
            Destroy(gameObject);
        }

        if ((gameObject.CompareTag("Laser1") && other.CompareTag("Ship2")) || (gameObject.CompareTag("Laser2") && other.CompareTag("Ship1")))
        {
            shipScript = other.GetComponent<Ship>();
            shipScript.TakeDamage();
            Destroy(gameObject);
        }
    }
}
