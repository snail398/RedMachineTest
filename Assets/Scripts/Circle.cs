using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Circle 
{
    [HideInInspector] public Vector3 direction;
    [HideInInspector] public float baseSpeed;
    [HideInInspector] public float currentSpeed = 0f;
    [HideInInspector] public Team team;
    [HideInInspector] public Vector3 position;
    [HideInInspector] public Vector3 scale;
}
