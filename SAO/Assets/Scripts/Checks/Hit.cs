using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hit : MonoBehaviour
{
    SwordArts swordArts;

    private Rigidbody2D rb;

    public bool hit;
    
    private void Awake ()
    {
        rb = GetComponent<Rigidbody2D>();
        swordArts = GetComponent<SwordArts>();
    }

    void Update()
    {

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        bool active = swordArts.active;
        if (active)
        {
            hit = true;
            Debug.Log("Hurt.");
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        hit = false;
        Debug.Log("Done.");
    }
}
