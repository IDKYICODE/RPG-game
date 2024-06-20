using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knockback : MonoBehaviour
{
        public float thrust;// force with which the player knockbacks the enemy
        public float knockTime;
        public float damage;

    // Start is called before the first frame update
    
    private void OnTriggerEnter2D(Collider2D other)
    {
         if(other.gameObject.CompareTag("breakable") && this.gameObject.CompareTag("Player"))
        {
            other.GetComponent<pot>().Smash();//invokes the smash component to play the animation
        }
        if(other.gameObject.CompareTag("enemy") || other.gameObject.CompareTag("Player"))// if it is the enemy or player
        {
            Rigidbody2D hit = other.GetComponent<Rigidbody2D>();// converting kinematic to dynamic and dynamic to kinematic
            if(hit != null)
            {
                Vector2 difference = hit.transform.position - transform.position;
                difference = difference.normalized * thrust;
                hit.AddForce(difference, ForceMode2D.Impulse);
                if(other.gameObject.CompareTag("enemy") && other.isTrigger)
                {
                hit.GetComponent<Enemy>().currentState= EnemyState.stagger;
                other.GetComponent<Enemy>().Knock(hit, knockTime, damage);
                }
                if(other.gameObject.CompareTag("Player"))
                {
                    if(other.GetComponent<PlayerMovement>().currentState != PlayerState.stagger)
                    {
                    hit.GetComponent<PlayerMovement>().currentState = PlayerState.stagger;
                    other.GetComponent<PlayerMovement>().Knock(knockTime, damage);
                    }
                }
               
            }
        }
    }
}
