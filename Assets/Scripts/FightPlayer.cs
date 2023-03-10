using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightPlayer : MonoBehaviour
{    
    AudioSource speaker;

    [SerializeField]
    AudioClip[] hurtSounds;

    [SerializeField]
    AudioClip[] deathSounds;

    Rigidbody rb;
    Vector3 startScale;

    bool alive = true;
    bool disablePlayer = false;

    ParticleSystem splatter;

    private void Start()
    {
        startScale = transform.localScale;
        rb = GetComponent<Rigidbody>();
        splatter = GetComponentInChildren<ParticleSystem>();
        health = GameProgression.instance.YoungestMembersHealt;
        speaker = GetComponent<AudioSource>();
    }

    [SerializeField]
    float reactMagnitude = 0.05f;

    private float LookDirection(float inputDirection)
    {
        // Debug.Log($"{inputDirection} / {Mathf.Sign(transform.localScale.x)}");
        if (inputDirection < -reactMagnitude)
        {
            return -1;
        } else if (inputDirection > reactMagnitude)
        {
            return 1;
        } else
        {  
            // This negative is because the original     direction is a flipped sign
            return -Mathf.Sign(transform.localScale.x);
        }        
    }

    [SerializeField]
    AnimationCurve acceleration;

    [SerializeField]
    float forceScaler = 10;

    float lastGround = 0;
    [SerializeField]
    float groundAllowance = 0.3f;

    HashSet<Transform> groundings = new HashSet<Transform>();

    bool ActuallyGrounded
    {
        get
        {
            return groundings.Count > 0;
        }
    }

    bool Grounded
    {
        get
        {
            return ActuallyGrounded || Time.timeSinceLevelLoad - lastGround < groundAllowance;
        }

        set
        {
            if (value)
            { 
                lastGround = Time.timeSinceLevelLoad;
            } else
            {
                lastGround = 2 * -groundAllowance;
            }
        }
    }


    [SerializeField]
    float dampen = 0.5f;

    [SerializeField]
    Vector3 clampVelocities = new Vector3(5, 5, 0);

    string prevState = "";
    void LogStateChange()
    {
        var gState = Grounded ? "Grounded" : "Flying";
        var jState = Jumping ? "Jumping" : "Not jumping";
        var state = $"{gState} {jState}";

        if (state != prevState)
        {
            prevState = state;
            Debug.Log(state);
        }
        
    }
    private void Update()
    {
        if (!alive || disablePlayer) return;

        LogStateChange();

        float horizontal = Input.GetAxis("Horizontal");
        transform.localScale = new Vector3(startScale.x * LookDirection(horizontal), startScale.y, startScale.z);

        if (Grounded)
        {
            if (Input.GetButtonDown("Jump"))
            {
                Jump(horizontal);
            } else
            { 
                GroundedMotion(horizontal);
            }
        } else {
            GroundedMotion(horizontal, 0.2f, false);
        }

        if (Input.GetButton("Jump"))
        {

            // SustainedJump(horizontal);
        } else if (Input.GetButtonUp("Jump"))
        {
            Jumping = false;
            // Debug.Log("Ended jump");
        }

        ClampVelocities();
    }

    void ClampVelocities()
    {
        var current = rb.velocity;
        current = new Vector3(Mathf.Clamp(current.x, -clampVelocities.x, clampVelocities.x), Mathf.Clamp(current.y, -clampVelocities.y, clampVelocities.y), 0);
        rb.velocity = current;
    }

    [SerializeField]
    float jumpUpForce = 500f;

    [SerializeField, Range(0, 0.1f)]
    float lateralForceFactor = 0.03f;

    float jumpSustainTime = 0.25f;

    float jumpStart;
    bool Jumping
    {
        get {
            return Time.timeSinceLevelLoad - jumpStart < jumpSustainTime;
        }

        set
        {
            if (value)
            {
                jumpStart = Time.timeSinceLevelLoad;
            } else
            {
                jumpStart = 2 * -jumpSustainTime;
            }
        }
    }
    enum JumpState {  NotJumping, ApplyingForce, Flying };

    JumpState jumpState = JumpState.NotJumping;

    void SustainedJump(float xDirection)
    {
        if (Jumping)
        {
            // Debug.Log("Sustaining jump");
            rb.AddForce(Vector3.up * jumpUpForce, ForceMode.Impulse);

            if (Mathf.Sign(xDirection) == rb.velocity.x)
            {
                var velocity = rb.velocity;
                velocity.x *= (1 - 0.5f) * Time.deltaTime;
                rb.velocity = velocity;
            }
        } else if (!Grounded)
        {
            // Debug.Log("Ended jump");
            Jumping = false;
            jumpState = JumpState.Flying;
        }
    }

    void Jump(float xDirection)
    {
        if (Jumping || (jumpState != JumpState.NotJumping && Time.timeSinceLevelLoad - jumpStart < 5 * jumpSustainTime)) return;
        Debug.Log("Starting jump");
        Jumping = true;

        var lateralForce = xDirection * jumpUpForce * lateralForceFactor;
        rb.AddForce(Vector3.up * jumpUpForce +  Vector3.right * lateralForce, ForceMode.Impulse);

        StartCoroutine(SustainedForce(jumpUpForce, 0.3f));

        Grounded = false;
        jumpState = JumpState.ApplyingForce;
    }

    IEnumerator<WaitForSeconds> SustainedForce(float upForce, float duration, float decay = 0.95f)
    {
        float start = Time.timeSinceLevelLoad;
        while (Time.timeSinceLevelLoad - start < duration)
        {
            if (!Input.GetButton("Jump")) yield break;
            upForce *= decay;
                        
            rb.AddForce(Vector3.up * upForce, ForceMode.Impulse);
            yield return new WaitForSeconds(0.1f);
        }

        Jumping = false;
        jumpState = JumpState.Flying;
    }

    void GroundedMotion(float horizontal, float factor = 1, bool doDampen = true)
    {
        if (Mathf.Abs(horizontal) > reactMagnitude)
        {
            rb.AddForce(Vector3.right * acceleration.Evaluate(rb.velocity.magnitude) * horizontal * Time.deltaTime * forceScaler * factor);
        }
        else if (doDampen)
        {
            float dampening = (1 - Time.deltaTime) * dampen;
            var velocity = rb.velocity;
            velocity.x *= dampening;
            rb.velocity = velocity;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            if (!Grounded) Debug.Log($"Grounded by {collision.gameObject.name}");
            groundings.Add(collision.transform);
            jumpState = JumpState.NotJumping;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            if (Grounded) Debug.Log($"Leaving gound {collision.gameObject.name}");

            groundings.Remove(collision.transform);
            lastGround = Time.timeSinceLevelLoad - Time.deltaTime * 0.5f;
        }
    }

    public bool IsStill
    {
        get
        {
            return Grounded && rb.velocity.magnitude < 0.1f;
        }
    }

    public bool IsAlive
    {
        get
        {
            return alive;
        }
    }

    float health = 1;
    
    public float Health
    {
        get
        {
            return health;
        }
    }

    void IgnoreMonsterColllisions(bool ignore)
    {
        Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Monster"), ignore);
    }

    public void Hurt(Vector3 force, float damage, bool longDisable = true)
    {
        if (disablePlayer) return;

        disablePlayer = true;
        IgnoreMonsterColllisions(true);

        rb.velocity = Vector3.zero;
        rb.AddForce(force, ForceMode.Impulse);

        health = Mathf.Clamp01(health - damage);
        Debug.Log($"Took {damage} / health {health} / Force {force}");

        splatter.Play();

        if (health <= 0) {
            IgnoreMonsterColllisions(false);
            alive = false;
            var progress = GameProgression.instance;
            progress.SetYoungestMemberHealth(0);
            progress.NewDeath = true;


            speaker.PlayOneShot(deathSounds[Random.Range(0, deathSounds.Length)]);
            transform.Rotate(new Vector3(0, 0, 90));
        } else
        {
            speaker.PlayOneShot(hurtSounds[Random.Range(0, hurtSounds.Length)]);
            StartCoroutine(ReenableColliders(force, longDisable));
        }
    }    

    IEnumerator<WaitForSeconds> ReenableColliders(Vector3 force, bool longDisable)
    {
        for (float t = 0; t<0.5f; t+=0.02f)
        {
            rb.AddForce(force, ForceMode.Impulse);
            yield return new WaitForSeconds(0.02f);
            force *= 0.5f;
        }

        if (longDisable)
        {
            yield return new WaitForSeconds(0.5f);
        }

        IgnoreMonsterColllisions(false);
        disablePlayer = false;
    }
}
