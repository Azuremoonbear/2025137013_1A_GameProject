using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllor : MonoBehaviour
{
    [Header("�̵� ����")]
    public float walkSpeed = 3f;
    public float runSpeed = 6f;
    public float rotationSpeed = 10;

    [Header("���� ����")]
    public float jumpHeight = 2f;
    public float gravity = -9.81f;
    public float landingDuration = 0.3f;

    [Header("���� ����")]
    public float attackDuration = 0.8f;
    public bool canMoveWhileAttacking = false;

    [Header("������Ʈ")]
    private Animator animator;

    private CharacterController controller;
    private Camera playerCamera;

    //�������
    private float currentSpeed;
    private bool isAttacking = false;
    private bool isLanding = false;
    private float landingTimer;

    private Vector3 velocity;
    private bool isGrounded;
    private bool wasGrounded;
    private float attackTimer;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        playerCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        CheckGrounded();
        HandleLanding();
        HandleMovement();
        UpdateAnimator();
        HandleJump();
        HandleAttack();
        UpdateAnimator();
    }

    void UpdateAnimator()
    {
        if (animator != null)
        {
            float animatorSpeed = Mathf.Clamp01(currentSpeed / runSpeed);
            animator.SetFloat("speed", animatorSpeed);
            animator.SetBool("isGrounded", isGrounded);

            bool isFalling = !isGrounded && velocity.y < -0.1f;
            animator.SetBool("isFalling", isFalling);
            animator.SetBool("isLanding", isLanding);
        }
    }

    void HandleAttack()
    {
        if (isAttacking)
        {
            attackTimer -= Time.deltaTime;
            if(attackTimer < 0)
            {
                isAttacking = false;
            }
        }
        if(Input.GetKeyDown(KeyCode.Alpha1) && !isAttacking)
        {
            isAttacking = true;
            attackTimer = attackDuration;

            if(animator != null)
            {
                animator.SetTrigger("attackTrigger");
            }
        }
    }

    void CheckGrounded()
    {
        wasGrounded = isGrounded;
        isGrounded = controller.isGrounded;

        if (isGrounded && wasGrounded)
        {
            Debug.Log("�������� ����");
        }

        if (!isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;

            //���� ��� Ʈ���� �� ���� ���� ����
            if (isGrounded && !wasGrounded)
            {
                //animator.SetTrigger("landTrigger")
                animator.SetTrigger("landTrigger");
                isLanding = true;
                landingTimer = landingDuration;
                Debug.Log("����");
            }
        }
    }

    void HandleLanding()
    {
        if(isLanding)
        {
            landingTimer -= Time.deltaTime;
            if(landingTimer <= 0)
            {
                isLanding = false;
            }
        }
    }

    void HandleJump()
    {
        if(Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);

            if (animator != null)
            {
                animator.SetTrigger("jumpTrigger");
            }
        }

        if(!isGrounded)
        {
            velocity.y += gravity * Time.deltaTime;
        }

        controller.Move(velocity * Time.deltaTime);
    }

    void HandleMovement()
    {
        if((isAttacking && !canMoveWhileAttacking) || isLanding)
        {
            currentSpeed = 0;
            return;
        }

        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        if (horizontal != 0 || vertical != 0)
        {
            //ī�޶� ���� ������ �������� �ǰ� ����
            Vector3 cameraForward = Camera.main.transform.forward;
            Vector3 cameraRight = Camera.main.transform.right;
            cameraForward.y = 0;
            cameraRight.y = 0;
            cameraForward.Normalize();
            cameraRight.Normalize();

            Vector3 moveDirection = cameraForward * vertical + cameraRight * horizontal;

            if (Input.GetKey(KeyCode.LeftShift))
            {
                currentSpeed = runSpeed;
            }
            else
            {
                currentSpeed = walkSpeed;
            }

            controller.Move(moveDirection * currentSpeed * Time.deltaTime);

            //�̵� ���� ������ �ٶ󺸸鼭 �̵�
            Quaternion tragetRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, tragetRotation, Time.deltaTime);
        }
        else
        {
            currentSpeed = 0;
        }
    }
}
