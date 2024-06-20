using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerState{
    walk,
    attack,
    interact,
    stagger,
    idle
}

public class PlayerMovement : MonoBehaviour
{
    public float speed;
    private Rigidbody2D myRigidbody;
    private Vector3 change;
    private Animator animator;

    public PlayerState currentState;
    public FloatValue currentHealth;
    public SignalG playerHealthSignal;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        myRigidbody = GetComponent<Rigidbody2D>();
        currentState = PlayerState.walk;
        animator.SetFloat("moveX",0);
        animator.SetFloat("moveY",-1);

    }

    // Update is called once per frame
    void Update()
    {
        change = Vector3.zero;//resets how much the player changes
        change.x = Input.GetAxisRaw("Horizontal");//tracks change in horizontal axis
        change.y = Input.GetAxisRaw("Vertical");//tracks change in vertical axis
        if(Input.GetButtonDown("attack") && currentState != PlayerState.attack && currentState != PlayerState.stagger)//if attack button is pressed and currentstate is not in attacking call Attackco
        {
            StartCoroutine(AttackCo());
        }
        else if(currentState == PlayerState.walk || currentState == PlayerState.idle)
        {
            UpdateAnimationAndMove();
        }
    }
    private IEnumerator AttackCo()
    {
        animator.SetBool("attacking",true);
        currentState = PlayerState.attack;
        yield return null;//to create a delay
        animator.SetBool("attacking",false);
        yield return new WaitForSeconds(.3f);
        currentState = PlayerState.walk;
    }

    void UpdateAnimationAndMove()
    {
        if(change != Vector3.zero)
        {
            MoveCharacter();
            animator.SetFloat("moveX", change.x);//these functions set movement to movement animation 
            animator.SetFloat("moveY", change.y);
            animator.SetBool("moving", true);//since if moving is not set the player will still be moving even when the key is not pressed.
        }else{
            animator.SetBool("moving",false);
        }
    }
    void MoveCharacter()
    {
        change.Normalize();
        myRigidbody.MovePosition(
            transform.position + change.normalized * speed * Time.fixedDeltaTime//takes the position of the character adds the change in direction multiplies it to the movement speed and delta time is the amount of time since the last frame.
        ); 
    }
    public void Knock(float knockTime, float damage)
    {
        currentHealth.RuntimeValue -= damage;
        playerHealthSignal.Raise();
        if(currentHealth.RuntimeValue >0)
        {
        StartCoroutine(KnockCo(knockTime));
        }else{
            this.gameObject.SetActive(false);
        }
    }
    private IEnumerator KnockCo(float knockTime)// to stop the enemy's velocity after getting knocked back
    {
        if(myRigidbody != null)
        {
            yield return new WaitForSeconds(knockTime);
            myRigidbody.velocity = Vector2.zero;
            currentState = PlayerState.idle;
            myRigidbody.velocity = Vector2.zero;
        }else{
            this.gameObject.SetActive(false);
        }
    }
}
