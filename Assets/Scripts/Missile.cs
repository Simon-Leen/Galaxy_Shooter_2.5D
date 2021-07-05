using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : MonoBehaviour
{
    [SerializeField]
    private float _missileSpeed = 5f;
    private GameObject[] _enemies;
    private Transform _target;
    float distanceToo, closestEnemy;
    Quaternion targetRot;


    void Update()
    {
        _enemies = GameObject.FindGameObjectsWithTag("Enemy");
        
        StartCoroutine("TimeToLive");
        SetTarget();
    }
    void SetTarget()
    {
        Vector3 missilePos = transform.position;
        Vector3 enemyPos;
        
        foreach (GameObject enemy in _enemies)
        {
            if (_target == null)
            {
                _target = enemy.transform;
                enemyPos = _target.position;
                targetRot = Quaternion.LookRotation(transform.forward, (enemyPos - missilePos));
                distanceToo = Vector3.Distance(missilePos, enemyPos);
                closestEnemy = distanceToo;
            }
            else
            {
                enemyPos = enemy.transform.position;
                targetRot = Quaternion.LookRotation(transform.forward, (enemyPos - missilePos));
                distanceToo = Vector3.Distance(missilePos, enemyPos);
                if (distanceToo < closestEnemy)
                {
                    _target = enemy.transform;
                    closestEnemy = distanceToo;
                }
            }
        }
        transform.position = Vector3.MoveTowards(transform.position, _target.position,
            _missileSpeed * Time.deltaTime);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, _missileSpeed * Time.deltaTime);
    }
    IEnumerator TimeToLive()
    {
        yield return new WaitForSeconds(2f);
        Destroy(this.gameObject);
    }
}
