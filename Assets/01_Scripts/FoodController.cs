using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodController : MonoBehaviour
{
    public float energyAmmount = 2f;
    public AudioClip eatsSound;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Player p = other.GetComponent<Player>();
            if (p != null)
            {
                p.ApplyEnergy(energyAmmount);
                AudioManager.instance.PlaySong(eatsSound);
                Destroy(gameObject);
            }
        }
    }

}
