using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyEMP : MonoBehaviour
{
    private float range;

    // Update is called once per frame
    void Update()
    {
        float rangeSpeed = 0.25f;
        range += rangeSpeed * Time.deltaTime;
        if (range > 1f)
        {
            range = 0;
        }
        transform.localScale = new Vector3(range, range);
    }

    
}
