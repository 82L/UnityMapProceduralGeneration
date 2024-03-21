using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    private CharacterController _characterController;
    private Vector2 _moveInput;
    private Vector3 _direction;
    private float _rotationVelocity;
    private float _yVelocity;
    private float gravity = -9.81f;

    [SerializeField] private GameObject playerModel;
    [SerializeField, Range(0.1f, 30f)] private float speed = 5.0f;
    [SerializeField, Range(0.1f, 1f)] private float rotationTime = 0.05f;
    [SerializeField, Range(0f, 10f)] private float gravityMultiplier = 1.0f;

    private void Awake()
    {
        _characterController = this.GetComponent<CharacterController>();
    }

    public void Move(InputAction.CallbackContext context)
    {
        _moveInput = context.ReadValue<Vector2>();

        _direction = new Vector3(_moveInput.x, 0, _moveInput.y);
    }

    private void Update()
    {
        ApplyRotation();
        // ApplyGravity();
        ApplyMovement();
    }

    private void ApplyRotation()
    {
        if (_moveInput != Vector2.zero)
        {
            float targetAngle = Mathf.Atan2(_direction.x, _direction.z) * Mathf.Rad2Deg;
            float angle = Mathf.SmoothDampAngle(playerModel.transform.eulerAngles.y, targetAngle, ref _rotationVelocity, rotationTime);
            playerModel.transform.rotation = Quaternion.Euler(0, angle, 0);
        }
    }

    
    private void ApplyMovement()
    {
        _direction.y = 0;

        _characterController.Move(_direction * speed * Time.deltaTime);
    }

    private void ApplyGravity()
    {

        if (_characterController.isGrounded && _yVelocity < 0f)
        {
            _yVelocity = 0f;
        }
        else
        {
            _yVelocity += gravity * gravityMultiplier * Time.deltaTime;
            _direction.y = _yVelocity;
        }
    }
}
