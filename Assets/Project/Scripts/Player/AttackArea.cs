using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackArea : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Enemy"))
        {
            if (other.GetComponent<Character>().enabled)
            {
                other.GetComponent<Character>().OnHit(30f);
            }
        }
    }
}
