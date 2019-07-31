using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Visuals")]
    [SerializeField] private Camera playerCamera;

    [Header("Gameplay")]
    [SerializeField] private int initialHealth = 100;
    [SerializeField] private int initialAmmo = 12;
    [SerializeField] private float knockbackForce = 10f; // knockback force after player collides with an enemy
    [SerializeField] private float hurtDuration = 0.5f; // how long the player is invulnerable after being hit


    private int health;
    public int Health { get { return health; } }

    private int ammo;
    public int Ammo { get { return ammo; } }

    private bool killed;
    public bool Killed { get { return killed; } }

    private bool isHurt;

    // Start is called before the first frame update
    private void Start()
    {
        health = initialHealth;
        ammo = initialAmmo;
    }

    // Update is called once per frame
    private void Update()
    {
        shootGun();
    }

    private void shootGun()
    {
        // If mouse button is pressed down, shoot a bullet if there is ammo
        if (Input.GetMouseButtonDown(0))
        {
            if (ammo > 0 && !killed)
            {
                ammo--;

                GameObject bulletObject = PoolingManager.Instance.GetBullet(true);
                bulletObject.transform.position = playerCamera.transform.position + playerCamera.transform.forward;
                bulletObject.transform.forward = playerCamera.transform.forward;
            }
        }
    }

    // Check for collisions and control player's damage taken logic
    private void OnTriggerEnter(Collider otherCollider)
    {
        if (otherCollider.GetComponent<AmmoCrate>() != null)
        {
            // Collect ammo crate
            AmmoCrate ammoCrate = otherCollider.GetComponent<AmmoCrate>();
            ammo += ammoCrate.ammo;
            Destroy(ammoCrate.gameObject);
        }
        else if (otherCollider.GetComponent<HealthCrate>() != null)
        {
            // Collect health crate
            HealthCrate healthCrate = otherCollider.GetComponent<HealthCrate>();
            health += healthCrate.health;
            Destroy(healthCrate.gameObject);
        }
        if (isHurt == false)
        {
            GameObject hazard = null; // Keeps track of which object hit the player

            // If player collides with enemy, decrease player health and set hazard to enemy gameObject
            if (otherCollider.GetComponent<Enemy>() != null)
            {
                Enemy enemy = otherCollider.GetComponent<Enemy>();
                if (enemy.Killed == false)
                {
                    hazard = enemy.gameObject;
                    health -= enemy.damage;
                }

            // If player collides with enemy bullet
            }
            else if (otherCollider.GetComponent<Bullet>() != null)
            {
                Bullet bullet = otherCollider.GetComponent<Bullet>();
                if (!bullet.ShotByPlayer)
                {
                    hazard = bullet.gameObject;
                    health -= bullet.damage;
                    bullet.gameObject.SetActive(false);
                }
            }
            // If player has been hit
            if (hazard != null)
            {
                isHurt = true;

                // Perform knockback effect
                Vector3 hurtDirection = (transform.position - hazard.transform.position).normalized;
                Vector3 knockbackDirection = (hurtDirection + Vector3.up).normalized;
                GetComponent<ForceReceiver>().AddForce(knockbackDirection, knockbackForce);

                StartCoroutine(HurtRoutine());
            }

            if (health <= 0)
            {
                if (!killed)
                {
                    killed = true;

                    OnKill();
                }
            }
        }
    }

        IEnumerator HurtRoutine()
        {
            // Make player invulnerable to more damage for hurtDuration amount of time after being hit
            yield return new WaitForSeconds(hurtDuration);
            isHurt = false;
        }

        private void OnKill()
        {
            GetComponent<CharacterController>().enabled = false;
            GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>().enabled = false;
        }
    }
