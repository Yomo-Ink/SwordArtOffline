using UnityEngine;
using UnityEngine.InputSystem;

public class Move : MonoBehaviour
{
    
    SwordArts swordArts;
    private PlayerInput playerInput;
    public Animator animator;

    [SerializeField, Range(0f, 100f)] private float maxSpeed = 4f;
    [SerializeField, Range(0f, 1000f)] private float maxAcceleration = 100f;
    [SerializeField, Range(0f, 100f)] private float maxAirAcceleration = 20f;

    private Vector2 direction = Vector2.zero;
    private Vector2 desiredVelocity;
    private Vector2 velocity;
    private Rigidbody2D body;
    private Ground ground;

    private float maxSpeedChange;
    private float acceleration;
    private bool onGround;
    private bool movingLeft = false;
    private bool movingRight = false;

    public bool attacked = false;
    public bool cancelled = false;
    public bool highGuard = false;
    public bool lowGuard = false;
    public GameObject Alpha;
    public GameObject Beta;

    void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        ground = GetComponent<Ground>();
        swordArts = GetComponent<SwordArts>();
        playerInput = GetComponent<PlayerInput>();
    }

    /*void Start()
    {
        Vector2 alphaPosition = new Vector2(-5f, -3.058552f);
        Vector2 betaPosition = new Vector2(5f, -3.058552f);

        var alpha = PlayerInput.Instantiate(Alpha, 0, controlScheme: "Keyboard1", -1, pairWithDevice: Keyboard.current);
        var beta = PlayerInput.Instantiate(Beta, 1, controlScheme: "Keyboard2", -1, pairWithDevice: Keyboard.current);
        
        alpha.transform.position = alphaPosition;
        beta.transform.position = betaPosition;

        var alphaPlayerInput = alpha.GetComponent<PlayerInput>();
        var betaPlayerInput = beta.GetComponent<PlayerInput>();
    }*/

    void Start()
    {
        Vector2 alphaPosition = new Vector2(-5f, -3.058552f);
        Vector2 betaPosition = new Vector2(5f, -3.058552f);

        var alpha = Instantiate(Alpha, alphaPosition, Quaternion.identity);
        var beta = Instantiate(Beta, betaPosition, Quaternion.identity);

        var alphaPlayerInput = alpha.GetComponent<PlayerInput>();
        var betaPlayerInput = beta.GetComponent<PlayerInput>();
    }


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

    public void OnMoveLeft(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            Debug.Log("MoveLeft action started.");
            movingLeft = true;
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            Debug.Log("MoveLeft action cancelled.");
            movingLeft = false;
        }
        UpdateDirection();
    }

    public void OnMoveRight(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            Debug.Log("MoveRight action started.");
            movingRight = true;
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            Debug.Log("MoveRight action cancelled.");
            movingRight = false;
        }
        UpdateDirection();
    }

    public void UpdateDirection()
    {
        if (movingRight && !movingLeft)
        {
            direction = new Vector2(1, 0);
        }
        else if (movingLeft && !movingRight)
        {
            direction = new Vector2(-1, 0);
        }
        else
        {
            direction = new Vector2(0, direction.y);
        }
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        Debug.Log("Attack action detected.");
        attacked = context.ReadValue<float>() > 0;
    }

    public void OnCancel(InputAction.CallbackContext context)
    {
        Debug.Log("Attack action detected.");
        cancelled = context.ReadValue<float>() > 0;
    }

    public void OnHighGuard(InputAction.CallbackContext context)
    {
        highGuard = context.ReadValue<float>() > 0;
    }

    public void OnLowGuard(InputAction.CallbackContext context)
    {
        lowGuard = context.ReadValue<float>() > 0;
    }

    void Update()
    {
        bool normalAttacking = swordArts.normalAttacking;
        bool chargeAttacking = swordArts.chargeAttacking;
        bool recovery = swordArts.recovery;
        if (normalAttacking)
        {
            desiredVelocity = new Vector2(3f, 0f);
        }
        if (chargeAttacking)
        {
            desiredVelocity = new Vector2(7f, 0f);
        }
        else if (recovery)
        {
            desiredVelocity = new Vector2(0f, 0f);
        }
        else
        {
            Vector2 move = new Vector2(direction.x, 0f) * Mathf.Max(maxSpeed - ground.GetFriction(), 0f);
            desiredVelocity = move;
        }
    }

    private void FixedUpdate()
    {
        onGround = ground.GetOnGround();
        velocity = body.velocity;

        acceleration = onGround ? maxAcceleration : maxAirAcceleration;
        maxSpeedChange = acceleration * Time.deltaTime;
        velocity.x = Mathf.MoveTowards(velocity.x, desiredVelocity.x, maxSpeedChange);

        body.velocity = velocity;
    }
}