using UnityEngine;
using UnityEngine.InputSystem;

public class SwordArts : MonoBehaviour
{
    Hit hit;
    Move move;
    private PlayerInput playerInput;

    private int playerIndex;
    private float frames = 0f;
    private float cancelCooldown = 0f;
    //private bool vomTag;
    private bool overheadStrike;
    private bool reset;
    private bool highParry;

    public bool charge;
    public bool windup;
    public bool active;
    public bool recovery;
    public bool normalAttacking;
    public bool chargeAttacking;
    public bool idle;

    public Animator animator;

    void Awake()
    {
        hit = GetComponent<Hit>();
        move = GetComponent<Move>();
        playerInput = GetComponent<PlayerInput>();
        playerIndex = playerInput.playerIndex;
    }

    void Update()
    {
        bool attacked = move.attacked;
        bool highGuard = move.highGuard;
        bool cancelled = move.cancelled;
        bool lowGuard = move.lowGuard;

        if (!attacked && reset)
        {
            reset = false;
        }
        if (attacked && !reset/*&& vomTag*/)
        {
            idle = false;
            overheadStrike = true;
        }
        /*if (Input.GetKey(KeyCode.Space))
        {
            highParry = true;
        }*/
    }

    void FixedUpdate()
    {
        //bool hit = Hit.hit;
        bool cancelled = move.cancelled;
        if (idle)
        {
            overheadStrike = false;
            charge = false;
            highParry = false;
            recovery = false;
        }
        if (highParry)
        {
            overheadStrike = false;
            charge = false;
            idle = false;
        }
        if (cancelCooldown > 0f)
        {
            cancelCooldown--;
        }
        if (normalAttacking || chargeAttacking)
        {
            active = true;
        }
        if (overheadStrike && !charge)
        {
            Oberhau();
        }
        if (charge)
        {
            Zornhau();
        }
        if (highParry)
        {
            HighParry();            
        }
    }

    void Oberhau()
    {
        bool cancelled = move.cancelled;
        bool attacked = move.attacked;
        if (animator is null)
        {
            Debug.Log("Animator is null!");
            return;
        }
        if (highParry)
        {
            animator.SetBool("Oberhau", false);
            overheadStrike = false;
            frames = 0f;
            return;
        }
        //Normal attacks can be cancelled
        if (frames < 24f && cancelled && cancelCooldown == 0f)
        {
            animator.SetBool("Oberhau", false);
            frames = 0f;
            idle = true;
            reset = true;
            cancelCooldown = 36f;
            return;
        }
        if (frames > 0f && frames < 28f)
        {
            windup = true;
        }
        //Charge into Zornhut
        if (frames == 24f && attacked)
        {
            animator.SetBool("Oberhau", false);
            frames = 0f;
            overheadStrike = false;
            normalAttacking = false;
            charge = true;
            return;
        }
        if (frames >= 28f && frames < 48f)
        {
            windup = false;
            active = true;
            normalAttacking = true;
        }
        if (frames >= 48f && frames < 56f)
        {
            active = false;
            recovery = true;
        }
        if (frames == 56f)
        {
            animator.SetBool("Oberhau", false);
            frames = 0f;
            idle = true;
            normalAttacking = false;
            recovery = false;
            return;
        }
        else
        {
            animator.SetBool("Oberhau", true);
            frames ++;
        }
    }

    void Zornhau()
    {
        if (frames > 0f && frames < 32f)
        {
            windup = true;
        }
        if (frames >= 32f && frames < 48f)
        {
            windup = false;
            chargeAttacking = true;
            active = true;
        }
        if (frames >= 48f && frames < 64f)
        {
            active = false;
            chargeAttacking = false;
            recovery = true;
        }
        if (frames == 64f)
        {
            animator.SetBool("Zornhau", false);
            frames = 0f;
            idle = true;
            recovery = false;
            reset = true;
            return;
        }
        else
        {
            animator.SetBool("Zornhau", true);
            frames ++;
        }
    }

    void HighParry()
    {
        //Return to idle with no input
        if (frames == 40)
            {
                animator.SetBool("High Parry", false);
                animator.SetBool("Quick Charge", false);
                animator.SetBool("Oberhau", false);
                highParry = false;
                reset = true;
                idle = true;
                frames = 0f;
            }
            else
            {
                animator.SetBool("High Parry", true);
                frames ++;
            }
    }
    /*
        private void OnEnable()
    {
        playerInput.actions["MoveLeft"].started += OnMoveLeft;
        playerInput.actions["MoveLeft"].canceled += OnMoveLeft;

        playerInput.actions["MoveRight"].started += OnMoveRight;
        playerInput.actions["MoveRight"].canceled += OnMoveRight;

        playerInput.actions["Attack"].performed += OnAttack;
        playerInput.actions["Attack"].canceled += OnAttack;

        playerInput.actions["Cancel"].performed += OnCancel;
        playerInput.actions["Cancel"].canceled += OnCancel;

        playerInput.actions["HighGuard"].performed += OnHighGuard;
        playerInput.actions["HighGuard"].canceled += OnHighGuard;

        playerInput.actions["LowGuard"].performed += OnLowGuard;
        playerInput.actions["LowGuard"].canceled += OnLowGuard;
    }

    private void OnDisable()
    {
        playerInput.actions["MoveLeft"].started -= OnMoveLeft;
        playerInput.actions["MoveLeft"].canceled -= OnMoveLeft;

        playerInput.actions["MoveRight"].started -= OnMoveRight;
        playerInput.actions["MoveRight"].canceled -= OnMoveRight;

        playerInput.actions["Attack"].performed -= OnAttack;
        playerInput.actions["Attack"].canceled -= OnAttack;

        playerInput.actions["Cancel"].performed -= OnCancel;
        playerInput.actions["Cancel"].canceled -= OnCancel;

        playerInput.actions["HighGuard"].performed -= OnHighGuard;
        playerInput.actions["HighGuard"].canceled -= OnHighGuard;

        playerInput.actions["LowGuard"].performed -= OnLowGuard;
        playerInput.actions["LowGuard"].canceled -= OnLowGuard;
    }
    */
}

