using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slow : MonoBehaviour
{
    public new Collider2D collider;
    public PhysicsMaterial2D frictionlessMaterial;
    public PhysicsMaterial2D stickyMaterial;

    SwordArts swordArts;

    void Start()
    {
        GameObject obj = GameObject.Find("Alpha");
        swordArts = obj.GetComponent<SwordArts>();
        collider = GetComponent<Collider2D>();
        collider.sharedMaterial = frictionlessMaterial; // start with the frictionless material
    }

    void Update()
    {
        bool idle = swordArts.idle;
        if (!idle) // Left mouse button was clicked
        {
            collider.sharedMaterial = stickyMaterial; // change to the sticky material
        }

        if (idle) // Left mouse button was released
        {
            collider.sharedMaterial = frictionlessMaterial; // change back to the frictionless material
        }
    }
}