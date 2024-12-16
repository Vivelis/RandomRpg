using UnityEngine;
using System.Collections;
using UnityEngine.UIElements;

[System.Serializable]
public class TransformData
{
    public Vector3 position;
    public Quaternion rotation;
    public Vector3 scale;

    public TransformData(Vector3 pos, Quaternion rot, Vector3 scl)
    {
        position = pos;
        rotation = rot;
        scale = scl;
    }
}

public class BasicController1 : MonoBehaviour
{
    [Header("Param�tres de mouvement")]
    public float moveSpeed = 5f;
    public float rotationSpeed = 10f;
    public float gravity = -9.81f;
    public bool canMove = true;

    [Header("Param�tres de collision")]
    public LayerMask groundMask;

    private CharacterController controller;
    private Animator animator;
    private Vector3 velocity;
    private bool isGrounded;

    private bool anim = false;

    private Vector3 forwardAxis;
    private Vector3 rightAxis;

    private float originalHeight;
    private Vector3 originalCenter;

    public float Attack1Height;
    public Vector3 Attack1Center;

    public float DeadHeight;
    public Vector3 DeadCenter;

    private TransformData savedTransform;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();

        originalHeight = controller.height;
        originalCenter = controller.center;

        Transform currentTransform = transform;
        savedTransform = new TransformData(
            currentTransform.position,
            currentTransform.rotation,
            currentTransform.localScale
        );

        animator.SetBool("Fight", true);

        SetupAxesRelativeToPlayer();
    }

    void Update()
    {
        if (!anim)
        {
            if (savedTransform != null)
            {
                transform.position = savedTransform.position;
                transform.rotation = savedTransform.rotation;
                transform.localScale = savedTransform.scale;
            }
        }
        TestAnimations();
    }

    private void AdjustCharacterController(float height, Vector3 center)
    {
        controller.height = height;
        controller.center = center;
    }

    private void SetupAxesRelativeToPlayer()
    {
        forwardAxis = transform.forward.normalized;
        rightAxis = transform.right.normalized;
    }

    private void TestAnimations()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            StartCoroutine(PlayAnimation("Attack1"));
            AdjustCharacterController(Attack1Height, Attack1Center);
        }
        else if (Input.GetKeyDown(KeyCode.W))
        {
            StartCoroutine(PlayAnimationFast("Attack2"));
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            StartCoroutine(PlayAnimationFast("Attack3"));
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            StartCoroutine(PlayAnimationDamage("Damage"));
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            StartCoroutine(PlayAnimationDead("Dead"));
            AdjustCharacterController(DeadHeight, DeadCenter);
        }
    }

    private IEnumerator PlayAnimation(string animationName)
    {
        anim = true;
        animator.SetBool(animationName, true);
        Debug.Log(animationName);
        yield return new WaitForSeconds(2.7f);
        animator.SetBool(animationName, false);
        AdjustCharacterController(originalHeight, originalCenter);
        anim = false;
    }

    private IEnumerator PlayAnimationFast(string animationName)
    {
        anim = true;
        animator.SetBool(animationName, true);
        Debug.Log(animationName);
        yield return new WaitForSeconds(1.45f);
        animator.SetBool(animationName, false);
        AdjustCharacterController(originalHeight, originalCenter);
        anim = false;
    }

    private IEnumerator PlayAnimationDamage(string animationName)
    {
        anim = true;
        animator.SetBool(animationName, true);
        Debug.Log(animationName);
        yield return new WaitForSeconds(0.7f);
        animator.SetBool(animationName, false);
        AdjustCharacterController(originalHeight, originalCenter);
        anim = false;
    }

    private IEnumerator PlayAnimationDead(string animationName)
    {
        animator.SetBool(animationName, true);
        Debug.Log(animationName);
        yield return new WaitUntil(() => !animator.GetBool("Fight"));
        animator.SetBool(animationName, false);
        AdjustCharacterController(originalHeight, originalCenter);
    }
}