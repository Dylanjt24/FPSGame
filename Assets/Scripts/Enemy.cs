using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int health = 5;
    public int damage = 5;

    private bool killed = false;
    public bool Killed { get { return killed; } }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Bullet>() != null)
        {
            Bullet bullet = other.GetComponent<Bullet>();
            // Check if bullet was shot by the player
            if (bullet.ShotByPlayer)
            {
                health -= bullet.damage;
                bullet.gameObject.SetActive(false);

                // If enemy health reaches 0, destroy enemy
                if (health <= 0)
                {
                    if (killed == false)
                    {
                        killed = true;
                        OnKill();
                    }
                }

            }
        }
    }

    protected virtual void OnKill()
    {

    }
}
