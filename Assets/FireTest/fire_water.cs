﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 불 끄기
/// </summary>
public class fire_water : MonoBehaviour
{

    public ParticleSystem fire_particle1;
    public ParticleSystem grow_particle;
    public ParticleSystem smoke_particle;
    private bool TF = true;
    private AudioSource audio;
    public AudioClip clip;

    // public ParticleSystem fire_particle3;

    // public ParticleSystem fire_particle4; // 불 파티클
    public GameObject fireobject;                   // 불 오브젝트

    void Start()
    {
        audio = this.GetComponent<AudioSource>();
    }

    void Update()
    {
        if (this.GetComponent<BoxCollider>().enabled && TF)
        {
            audio.Play();
            TF = false;
        }
        else if (!this.GetComponent<BoxCollider>().enabled && !TF)
        {
            audio.Stop();
            TF = true;

        }
    }

    void OnTriggerStay(Collider coll)
    {
        if (coll.CompareTag("Fire"))
        {
            fire_particle1.startSize -= 0.01f;
            grow_particle.startSize -= 0.05f;
            smoke_particle.startSize -= 0.01f;

            if (fire_particle1.startSize <= 0)
                fireobject.SetActive(false);


        }
    }

}