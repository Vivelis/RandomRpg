using UnityEngine;
using System.Collections;
using UnityEngine.UIElements;

[System.Serializable]
public class TransformData1
{
    public Vector3 position;
    public Quaternion rotation;
    public Vector3 scale;

    public TransformData1(Vector3 pos, Quaternion rot, Vector3 scl)
    {
        position = pos;
        rotation = rot;
        scale = scl;
    }
}

public class BasicController2 : MonoBehaviour
{
    [Header("Param�tres de mouvement")]
    public float moveSpeed = 5f;
    public float rotationSpeed = 10f;
    public float gravity = -9.81f;

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

    public float DeadHeight;
    public Vector3 DeadCenter;

    private TransformData1 savedTransform;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();

        originalHeight = controller.height;
        originalCenter = controller.center;

        Transform currentTransform = transform;
        savedTransform = new TransformData1(
            currentTransform.position,
            currentTransform.rotation,
            currentTransform.localScale
        );

        Debug.Log(savedTransform.position);
        Debug.Log(savedTransform.rotation);

        SetupAxesRelativeToPlayer();
    }

    void Update()
    {
        isGrounded = Physics.CheckSphere(transform.position + Vector3.down * (controller.height / 2), 0.1f, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        if (!anim)
        {
            if (savedTransform != null)
            {
                transform.position = savedTransform.position;
                transform.rotation = savedTransform.rotation;
                transform.localScale = savedTransform.scale;
            }
        }

        velocity.y += gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);

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
            StartCoroutine(PlayAnimationFast("Attack1"));
        }
        else if (Input.GetKeyDown(KeyCode.W))
        {
            StartCoroutine(PlayAnimationFast("Attack2"));
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            StartCoroutine(PlayAnimationDamage("Damage"));
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            animator.SetBool("Dead", true);
            AdjustCharacterController(DeadHeight, DeadCenter);
        }
    }

    private IEnumerator PlayAnimationFast(string animationName)
    {
        anim = true;
        animator.SetBool(animationName, true);
        Debug.Log(animationName);
        yield return new WaitForSeconds(1.45f);
        animator.SetBool(animationName, false);
        anim = false;
    }

    private IEnumerator PlayAnimationDamage(string animationName)
    {
        animator.SetBool(animationName, true);
        Debug.Log(animationName);
        yield return new WaitForSeconds(1.0f);
        animator.SetBool(animationName, false);

        if (savedTransform != null)
        {
            controller.enabled = false; // Désactive temporairement le CharacterController
            transform.position = savedTransform.position; // Restaurer uniquement la position sauvegardée
            transform.rotation = savedTransform.rotation; // (optionnel si nécessaire)
            controller.enabled = true; // Réactive le CharacterController

            Debug.Log("Position et rotation restaurées après Damage !");
        }
    }
}