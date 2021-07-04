using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserDetector : MonoBehaviour
{
    [SerializeField]
    private GameObject _Enemy;


    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Laser")
        {
            float laserX = other.transform.position.x;
            float enemyX = transform.position.x;
            int side;
            if (laserX < enemyX)
            {
                side = -1;
            }
            else
            {
                side = 1;
            }
            Enemy e = _Enemy.GetComponent<Enemy>();
            e.LaserSide(true, side);
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if(other.tag == "Laser")
        {
            Enemy e = _Enemy.GetComponent<Enemy>();
            e.LaserSide(false, 0);
        }
    }
}
