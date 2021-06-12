﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float _speed = 4f;
    [SerializeField]
    private Player _player;
    private Animator _anim;
    private AudioSource _audioSource;

    private void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
        if (_player == null)
        {
            Debug.LogError("Player is Null");
        }
        _anim = GetComponent<Animator>();
        if (_anim == null)
        {
            Debug.LogError("Animator is Null");
        }
        _audioSource = GetComponent<AudioSource>();
        if (_audioSource == null)
        {
            Debug.LogError("Audio Source on Enemy is Null");
        }
    }

    void Update()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if(transform.position.y < -6f)
        {
            transform.position = new Vector3(transform.position.x, 7f, 0);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            Player player = other.transform.GetComponent<Player>();
            if(player != null)
            {
                player.TakeDamage();
            }
            _anim.SetTrigger("OnEnemyDeath");
            _speed = 0f;
            _audioSource.Play();
            Destroy(this.gameObject, 2.5f);
        }

        if (other.tag == "Laser")
        {
            Destroy(other.gameObject);
            if (_player != null)
            {
                _player.AddScore(Random.Range(5,11));
            }
            _anim.SetTrigger("OnEnemyDeath");
            _speed = 0f;
            _audioSource.Play();
            Destroy(this.gameObject, 2.5f);
        }
    }
}
