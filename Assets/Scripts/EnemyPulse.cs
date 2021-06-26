using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPulse : MonoBehaviour
{
    private float range;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        float rangeSpeed = 1f;
        range += rangeSpeed * Time.deltaTime;
        if (range > 0.75f)
        {
            Destroy(this.gameObject);
        }
        transform.localScale = new Vector3(range, -range);
    }
}
