using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CharacterController2D : MonoBehaviour
{
    [SerializeField] private float m_JumpForce = 400f;                          // Amount of force added when the player jumps.
    [Range(0, 1)] [SerializeField] private float m_CrouchSpeed = .36f;          // Amount of maxSpeed applied to crouching movement. 1 = 100%
    [Range(0, .3f)] [SerializeField] private float m_MovementSmoothing = .05f;  // How much to smooth out the movement
    [SerializeField] private bool m_AirControl = false;                         // Whether or not a player can steer while jumping;
    [SerializeField] private LayerMask m_WhatIsGround;                          // A mask determining what is ground to the character
    [SerializeField] private Transform m_GroundCheck;                           // A position marking where to check if the player is grounded.
    [SerializeField] private Transform m_CeilingCheck;                          // A position marking where to check for ceilings
    [SerializeField] private Collider2D m_CrouchDisableCollider;                // A collider that will be disabled when crouching

    const float k_GroundedRadius = 0.40f; // Radius of the overlap circle to determine if grounded
    private bool m_Grounded;            // Whether or not the player is grounded.
    const float k_CeilingRadius = .2f; // Radius of the overlap circle to determine if the player can stand up
    private Rigidbody2D m_Rigidbody2D;
    private bool m_FacingRight = true;  // For determining which way the player is currently facing.
    private Vector3 m_Velocity = Vector3.zero;

    private Animator anim;
    public PlayerMovement pm;
    public float flySpeed;
    public float groundSpeed;
    private float moveSpeed = 10f;
    private AudioSource sound;

    [SerializeField] public Sprite[] Feet;
    [SerializeField] public Sprite[] Hand;
    [SerializeField] public SpriteRenderer FootL;
    [SerializeField] public SpriteRenderer FootR;
    [SerializeField] public SpriteRenderer HandR;
    [SerializeField] public AimObject weapon;
    [SerializeField] public SpriteRenderer head;
    [SerializeField] public Sprite[] heads;
    
    public AudioClip step;
    public AudioClip land;

    [Header("Events")]
    [Space]

    public UnityEvent OnLandEvent;

    [System.Serializable]
    public class BoolEvent : UnityEvent<bool> { }

    public BoolEvent OnCrouchEvent;
    private bool m_wasCrouching = false;

    private float air_MovementSmoothing;
    private float ground_MovementSmoothing;

    private void Start()
    {
        sound = GetComponent<AudioSource>();
        
        Physics2D.IgnoreLayerCollision(8, 9);

        anim = GetComponent<Animator>();
        air_MovementSmoothing = 3*m_MovementSmoothing;
        ground_MovementSmoothing = 1.5f*m_MovementSmoothing;
    }

    private void Awake()
    {
        m_Rigidbody2D = GetComponent<Rigidbody2D>();

        if (OnLandEvent == null)
            OnLandEvent = new UnityEvent();

        if (OnCrouchEvent == null)
            OnCrouchEvent = new BoolEvent();
    }

    private void FixedUpdate()
    {
        anim.SetBool("Ground", m_Grounded);
        anim.SetBool("Crouch", m_wasCrouching || Input.GetButton("Crouch"));
        bool wasGrounded = m_Grounded;
        m_Grounded = false;

        if (weapon != null) weapon.Wobble(m_Rigidbody2D.velocity.y);

        
        Collider2D[] colliders = Physics2D.OverlapCircleAll(m_GroundCheck.position, k_GroundedRadius, m_WhatIsGround);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject != gameObject)
            {
                m_Grounded = true;
                if (!wasGrounded) {
                    m_MovementSmoothing = ground_MovementSmoothing;
                    OnLandEvent.Invoke();
                    moveSpeed = groundSpeed;
                }
            }
        }
        if (!m_Grounded)
        {
            m_MovementSmoothing = air_MovementSmoothing;
            moveSpeed = flySpeed;
        }
        else
        {
            anim.SetBool("Dash", false);
        }
    }


    public void Move(float move, bool crouch, bool jump)
    {
        // If crouching, check to see if the character can stand up
        if (!crouch)
        {
            // If the character has a ceiling preventing them from standing up, keep them crouching
            if (Physics2D.OverlapCircle(m_CeilingCheck.position, k_CeilingRadius, m_WhatIsGround))
            {
                crouch = true;
            }
        }

        //only control the player if grounded or airControl is turned on
        if (m_Grounded || m_AirControl)
        {

            // If crouching
            if (crouch)
            {
                if (!m_wasCrouching)
                {
                    m_wasCrouching = true;
                    OnCrouchEvent.Invoke(true);
                }

                // Reduce the speed by the crouchSpeed multiplier
                move *= m_CrouchSpeed;

                // Disable one of the colliders when crouching
                if (m_CrouchDisableCollider != null)
                    m_CrouchDisableCollider.enabled = false;
            }
            else
            {
                // Enable the collider when not crouching
                if (m_CrouchDisableCollider != null)
                    m_CrouchDisableCollider.enabled = true;

                if (m_wasCrouching)
                {
                    m_wasCrouching = false;
                    OnCrouchEvent.Invoke(false);
                }
            }

            // Move the character by finding the target velocity
            Vector3 targetVelocity = new Vector2(move * moveSpeed, m_Rigidbody2D.velocity.y);
            // And then smoothing it out and applying it to the character
            m_Rigidbody2D.velocity = Vector3.SmoothDamp(m_Rigidbody2D.velocity, targetVelocity, ref m_Velocity, m_MovementSmoothing);

            // If the input is moving the player right and the player is facing left...
            if (move > 0 && !m_FacingRight)
            {
                // ... flip the player.
                Flip();
            }
            // Otherwise if the input is moving the player left and the player is facing right...
            else if (move < 0 && m_FacingRight)
            {
                // ... flip the player.
                Flip();
            }
        }
        // If the player should jump...
        if (jump)
        {
            // Add a vertical force to the player.
            m_Grounded = false;
            m_Rigidbody2D.AddForce(new Vector2(0f, m_JumpForce));
        }
    }


    private void Flip()
    {
        // Switch the way the player is labelled as facing.
        m_FacingRight = !m_FacingRight;
        // Multiply the player's x local scale by -1.
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;

        weapon.Flip();

        if (FootL.sprite.Equals(Feet[0])) FootL.sprite = Feet[1];
        else FootL.sprite = Feet[0];
        if (FootR.sprite.Equals(Feet[0])) FootR.sprite = Feet[1];
        else FootR.sprite = Feet[0];
        if (HandR.sprite.Equals(Hand[0])) HandR.sprite = Hand[1];
        else HandR.sprite = Hand[0];
        FootL.sortingOrder = FootL.sortingOrder * -1;
        FootR.sortingOrder = FootR.sortingOrder * -1;
        HandR.sortingOrder = HandR.sortingOrder * -1;

    }

    private void playSound(int index)
    {

        if (index == 0) sound.PlayOneShot(step);
        if (index == 1) sound.PlayOneShot(land);
    }
}