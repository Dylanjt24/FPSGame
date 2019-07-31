using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolingManager : MonoBehaviour
{

    private static PoolingManager instance;
    public static PoolingManager Instance { get { return instance; } }

    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private int bulletAmount = 20;

    private List<GameObject> bullets;

    // Start is called before the first frame update
    private void Awake()
    {
        instance = this;

        // Preload bullets
        bullets = new List<GameObject>(bulletAmount);

        // Create amount of bullets equal to bulletAmount and put them in a list
        for (int i = 0; i < bulletAmount; i++)
        {
            GameObject prefabInstance = Instantiate(bulletPrefab);
            prefabInstance.transform.SetParent(transform);
            prefabInstance.SetActive(false);

            bullets.Add(prefabInstance);
        }
    }

    // Logic for getting a bullet. Takes in parameter to determine if bullet came from player or not
    public GameObject GetBullet(bool shotByPlayer)
    {
        // Loop through bullets and set an inactive bullet to active
        foreach (GameObject bullet in bullets)
        {
            if (!bullet.activeInHierarchy)
            {
                bullet.SetActive(true);
                bullet.GetComponent<Bullet>().ShotByPlayer = shotByPlayer;
                return bullet;
            }
        }

        // If no inactive bullets exist, create a new bullet and add it to bullets
        GameObject prefabInstance = Instantiate(bulletPrefab);
        prefabInstance.transform.SetParent(transform);
        prefabInstance.GetComponent<Bullet>().ShotByPlayer = shotByPlayer;
        bullets.Add(prefabInstance);

        return prefabInstance;
    }
}
