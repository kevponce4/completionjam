﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostBuster : MonoBehaviour
{
    #region movment vars
    [SerializeField]
    private float speed;
    public float distance;

    [SerializeField] 
    private FurnitureManager fm;
    private Rigidbody2D gbRB;
    #endregion

    public Health health;
    
    #region attack_vars

    [SerializeField] 
    private float attack_timer;
    public float cooldownTime;
    private float lastFireTime;
    public LaserController laser;
    #endregion

    #region unity_functions
    // Start is called before the first frame update
    void Start()
    {
        //get the transform of the ghost
        gbRB = GetComponent<Rigidbody2D>();
        attack_timer = 0;
        fm = FindObjectOfType<FurnitureManager>();
        laser = GetComponent<LaserController>();
        health = GetComponent<Health>();
    }

    // Update is called once per frame
    void Update()
    {
        if(attack_timer <= 0)
        {
            move();
            if (Vector3.Distance(transform.position, fm.tracking.position) <= distance && 
                Time.time - lastFireTime >= cooldownTime)
            {
                Debug.Log("hi");
                laser.Fire();
                lastFireTime = Time.time;
                attack_timer = laser.attackLength;
            }
        }
        else
        {
            gbRB.velocity = Vector2.zero;
            attack_timer -= Time.deltaTime;
        }
    }

    #endregion

    #region movement_func

    private void move()
    {
        Vector2 my_pos = new Vector2(this.transform.position.x, this.transform.position.y);
        Vector2 ghost_pos = new Vector2(fm.tracking.position.x, fm.tracking.position.y);
        Vector2 distance = ghost_pos - my_pos;
        gbRB.velocity = distance.normalized * speed;
        float angle = Mathf.Atan2(distance.y,distance.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);
    }
    #endregion

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.collider.CompareTag("Furniture"))
        {
            health.LoseHealth((int) other.rigidbody.velocity.magnitude);
        }
    }
    
}
