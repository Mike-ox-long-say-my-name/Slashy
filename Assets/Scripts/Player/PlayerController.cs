using System.Collections;
using UnityEngine;

[RequireComponent(typeof(PlayerCharacter))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private float gravity = -9.81f;
    [SerializeField, Min(0)] private float horizontalMoveSpeed = 5;
    [SerializeField, Min(0)] private float verticalMoveSpeed = 2;

    [SerializeField, Min(0)] private float jumpStartVelocity = 5;

    [SerializeField] private Camera followingCamera;
    [SerializeField, Range(0, 1)] private float followSmoothTime = 0.3f;

    [SerializeField] private CharacterController characterController;
    [SerializeField] private PlayerCharacter player;

    [SerializeField, Min(0)] private float dashRecovery;
    [SerializeField, Min(0)] private float dashDistance;
    [SerializeField, Min(0)] private float dashTime;

    [SerializeField, Range(0, 1)] private float dashInvincibilityStart;
    [SerializeField, Range(0, 1)] private float dashInvincibilityEnd;

    public bool IsDashing { get; private set; }
    public bool IsInvincible { get; private set; }
    private Vector3 _velocity;
    private float _cameraFollowVelocity;

    private void Awake()
    {
        if (followingCamera == null)
        {
            followingCamera = Camera.main;
        }
    }

    private void Update()
    {
        HandleInput();
        characterController.Move(_velocity * Time.deltaTime);
        MoveCamera();
        _velocity.y += gravity * Time.deltaTime;
    }

    private void HandleInput()
    {
        var hor = Input.GetAxis("Horizontal");
        var ver = Input.GetAxis("Vertical");

        if (characterController.isGrounded)
        {
            _velocity.y = 0;
            if (Input.GetButton("Jump"))
            {
                _velocity.y = jumpStartVelocity;
            }
        }
        
        _velocity.x = hor * horizontalMoveSpeed;
        _velocity.z = ver * verticalMoveSpeed;

        if (Input.GetKeyDown(KeyCode.LeftShift) && !IsDashing)
        {
            var direction = new Vector3(hor, 0, ver);
            Dash(direction.normalized);
        }
    }

    private void Dash(Vector3 direction)
    {
        var step = direction * (dashDistance / dashTime);
        IsDashing = true;
        IEnumerator DashCoroutine()
        {
            var passedTime = 0f;
            while (passedTime < dashTime)
            {
                passedTime += Time.deltaTime;
                var fraction = passedTime / dashTime;
                if (!IsInvincible && fraction >= dashInvincibilityStart && fraction < dashInvincibilityEnd)
                {
                    IsInvincible = true;
                    player.Hurtbox.Disable();
                }
                else if (IsInvincible && fraction >= dashInvincibilityEnd)
                {
                    IsInvincible = false;
                    player.Hurtbox.Enable();
                }

                characterController.Move(step);
                yield return new WaitForEndOfFrame();
            }

            yield return new WaitForSeconds(dashRecovery);
            IsDashing = false;
        }

        StartCoroutine(DashCoroutine());
    }

    private void MoveCamera()
    {
        var cameraPosition = followingCamera.transform.position;
        var newX = Mathf.SmoothDamp(cameraPosition.x, transform.position.x,
            ref _cameraFollowVelocity, followSmoothTime);
        followingCamera.transform.position = new Vector3(newX, cameraPosition.y, cameraPosition.z);
    }
}
