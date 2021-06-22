using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollBackground : MonoBehaviour
{
    private Rigidbody2D _rigidbody;
    private BoxCollider2D _boxCollider;
    private float _backgroundYLength;

    void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _rigidbody.velocity = new Vector3(0, -1f, 0);

        _boxCollider = GetComponent<BoxCollider2D>();
        _backgroundYLength = _boxCollider.size.y;
    }

    // Update is called once per frame
    void Update()
    {
        if(transform.position.y < -_backgroundYLength)
        {
            transform.position = new Vector3(transform.position.x, _backgroundYLength, transform.position.z);
        }
    }
}
