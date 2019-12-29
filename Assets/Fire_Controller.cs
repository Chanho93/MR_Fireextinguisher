using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire_Controller : MonoBehaviour
{
    public bool fireButton = false;
    public bool firePin = false;

    public bool pushAllowed = false;

    public int intFireButton = 0;
    public int intFireButtonNot = 0;

    public int Gx;
    public int Gy;
    public int Gz;

    public float pitch;
    public float roll;

    public static Fire_Controller Instance
    {
        get
        {
            return instance;
        }
    }

    private static Fire_Controller instance = null;

    void Awake()
    {
        if (instance)
        {
            DestroyImmediate(gameObject);

            return;
        }

        instance = this;
    }

}
