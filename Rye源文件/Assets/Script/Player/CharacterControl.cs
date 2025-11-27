using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class CharacterControl : MonoBehaviour
{
    private Rigidbody2D _rigidbody2D;
    private Collider2D _collider2D;
    [SerializeField] private InputReader _inputReader;
    public InputReader InputReader => _inputReader;
    public Vector2 inputVector { get; private set; }
    public Vector2 Velocity => _rigidbody2D.velocity;
    private Vector3 _playerScale;
    public int FacingX { get; private set; } //1 向右 -1向左
    public bool CanUpdateVelocity { get; set; }
    public bool IgnoreInput { get; set; }

    [Header("移动参数")] 
    [Tooltip("最大速度")] [SerializeField]
    private float _maxMoveSpeed = 2f;
    public bool ApplyDownwardsForce { get; set; }
    [SerializeField] private float _downForce = 100f;
    [Tooltip("地面加速度")] 
    [SerializeField] private float _groundAcceleration = 5f;
    private Vector2 _currentVelocity;
    private Vector2 _desiredVelocity;

    [Header("跳跃参数")] 
    [SerializeField] private float _normalJumpHeight = 5f;
    [SerializeField] private float _doubleJumpHeight = 3f;

    [SerializeField] private float _normalGravityScale = 3f;
    [SerializeField] private float _fallGravityScale = 5f;
    // 起跳后 持续一段时间使OnGround为假 防止起跳即落地
    private bool _duringJumpCooldown;
    private readonly WaitForSeconds _jumpDisableGroundCheckTime = new WaitForSeconds(0.3f);

    public bool JumpTrigger { get; set; }

    // 二段跳
    private int _jumpCount = 0;
    public bool CanDoubleJump => _jumpCount < 2;

    [Header("地面检测")] 
    [SerializeField] private LayerMask _groundLayer;
    [SerializeField] private float _maxGroundAngle = 25f;
    private Vector3 _leftOrigin;
    private Vector3 _rightOrigin;
    private Vector3 _middleOrigin;
    [SerializeField] private Vector2 _leftOriginOffset;
    [SerializeField] private Vector2 _rightOriginOffset;
    // [SerializeField] private Transform _leftGroundCheckOrigin;
    // [SerializeField] private Transform _rightGroundCheckOrigin;
    [SerializeField] private float _groundCheckDistance = 0.5f;
    public bool OnGround { get; private set; }
    private float _minGroundDotProduct;
    private Vector2 _averageContactNormal;

    [Header("台阶检测")] 
    [SerializeField] private float _maxStepHeight = 0.2f;
    [SerializeField] private float _stepCheckDistance = 0.2f;
    [SerializeField] private Transform _bottomStepCheckOrigin;
    private Vector3 _topStepCheckOrigin;
    private bool _shouldStepUp = false;
    // 上台阶期间保证OnGround
    private bool _duringStepCooldown;
    private readonly WaitForSeconds _stepDisableGroundCheckTime = new WaitForSeconds(0.3f);


    [Header("交互检测")] 
    [SerializeField] private LayerMask _interactableLayer;
    [SerializeField] private Transform _interactablePosition;
    private InteractableBase _currentInteractable;
    public bool HasInteractable => _currentInteractable != null;

    [Header("传送功能")] 
    [SerializeField] private UnityEvent _onDoubleJump;
    private Vector2 _downDirection;

    // 地面音效
    public SurfaceType.Surface CurrentSurfaceType { get; private set; }

    private void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _collider2D = GetComponent<Collider2D>();
        OnValidate();
    }

    private void Start()
    {
        _playerScale = transform.localScale;
        FacingX = 1;
        _downDirection = Vector2.down;

        _topStepCheckOrigin = _bottomStepCheckOrigin.position + new Vector3(0f, -_downDirection.y, 0f) * _maxStepHeight;
    }

    private void OnValidate()
    {
        _minGroundDotProduct = Mathf.Cos(_maxGroundAngle * Mathf.Deg2Rad);
    }
    
    private void OnEnable()
    {
        InputReader.Moved += InputReader_OnMoved;
        _inputReader.TogglePlayerActions(true);
    }

    private void OnDisable()
    {
        InputReader.Moved -= InputReader_OnMoved;
        _inputReader.TogglePlayerActions(false);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("MovingPlatform"))
        {
            _rigidbody2D.interpolation = RigidbodyInterpolation2D.None;
            transform.SetParent(other.transform);
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("MovingPlatform"))
        {
            _rigidbody2D.interpolation = RigidbodyInterpolation2D.Interpolate;
            transform.SetParent(null);
        }
    }

    private void Update()
    {
        // 根据输入得到目标速度
        _desiredVelocity = new Vector2(inputVector.x, 0f) * _maxMoveSpeed;

        GroundCheck();
        if (OnGround && inputVector.sqrMagnitude > 0.1f)
        {
            StepCheck();
        }

        if (OnGround)
        {
            SurfaceCheck();
        }
    }

    private void FixedUpdate()
    {
        if (CanUpdateVelocity)
        {
            ApplyDownForceWhenIdle();
            UpdateVelocity();
        }
    }

    private void ApplyDownForceWhenIdle()
    {
        if (ApplyDownwardsForce)
        {
            if (inputVector.sqrMagnitude < 0.1f) return;

            _rigidbody2D.AddForce(_downForce * -_averageContactNormal);
        }
    }

    private void InputReader_OnMoved(Vector2 input)
    {
        if (IgnoreInput)
        {
            return;
        }
        
        inputVector = input;
        inputVector = Vector2.ClampMagnitude(inputVector, 1);
    }

    private void UpdateVelocity()
    {
        UpdateVelocityParams();

        _currentVelocity = _rigidbody2D.velocity;

        // 将速度投影到接触面上
        Vector2 adjustedDirX = ProjectDirectionOnPlane(Vector2.right, _averageContactNormal);
        float adjustedVelX = Vector2.Dot(_currentVelocity, adjustedDirX);

        // 根据加速度确定速度改变
        float velocityChange = _groundAcceleration * Time.deltaTime;
        float targetVelocityX = Mathf.MoveTowards(adjustedVelX, _desiredVelocity.x, velocityChange);
        _currentVelocity += adjustedDirX * (targetVelocityX - adjustedVelX);


        JumpTriggerCheck();
        StepUp();

        _rigidbody2D.velocity = _currentVelocity;

        ClearVelocityParams();
    }

    private void UpdateVelocityParams()
    {
        if (OnGround)
        {
            // 重置二段跳
            _jumpCount = 0;
        }
    }

    private void ClearVelocityParams()
    {
        _averageContactNormal = Vector2.zero;
        _currentVelocity = Vector2.zero;
    }

    public void DeathResetParams()
    {
        JumpTrigger = false;
        _shouldStepUp = false;
        
        SetNormalGravity();

        ClearVelocityParams();
    }

    private void StepUp()
    {
        if (_shouldStepUp)
        {
            StartCoroutine(StepCoolDown());
            _shouldStepUp = false;
            _rigidbody2D.position += -_downDirection * _maxStepHeight;
        }
    }

    private IEnumerator StepCoolDown()
    {
        _duringStepCooldown = true;
        yield return _stepDisableGroundCheckTime;
        _duringStepCooldown = false;
    }


    #region Jump

    private void JumpTriggerCheck()
    {
        if (JumpTrigger)
        {
            //Debug.Break();
            JumpTrigger = false;
            _jumpCount++;

            switch (_jumpCount)
            {
                //jumpCount > 2 return 
                default:
                    break;
                case > 2:
                    break;
                //jumpCount = 1 jump
                case 1:
                    Jump(_normalJumpHeight);
                    break;
                //jumpCount = 2 double jump
                case 2:
                    _onDoubleJump?.Invoke();
                    Jump(_doubleJumpHeight);
                    break;
            }
        }
    }

    public void PreventTripleJumpWhenFall()
    {
        if (!OnGround)
        {
            if (_jumpCount < 1)
            {
                _jumpCount = 1;
            }
        }
    }


    private void Jump(float targetHeight)
    {
        StartCoroutine(JumpCoolDown());
        // 跳跃决定竖直速度
        float jumpSpeed =
            Mathf.Sqrt(Mathf.Abs(2f * Physics2D.gravity.magnitude * _rigidbody2D.gravityScale * targetHeight));

        _currentVelocity.y = 0f; // 二段跳前将已有速度清零
        _currentVelocity += -_downDirection.normalized * jumpSpeed;
    }

    private IEnumerator JumpCoolDown()
    {
        _duringJumpCooldown = true;
        yield return _jumpDisableGroundCheckTime;
        _duringJumpCooldown = false;
    }

    #endregion

    #region Interact

    public void InteractableCheck()
    {
        // 从interact state退出时检测
        if (_currentInteractable != null)
        {
            return;
        }

        // 左右都检测
        RaycastHit2D hit = Physics2D.BoxCast(transform.position, Vector2.one, 0f, transform.right * FacingX, 1.5f,
            _interactableLayer);
        if (!hit.collider)
        {
            hit = Physics2D.BoxCast(transform.position, Vector2.one, 0f, transform.right * -FacingX, 1.5f,
                _interactableLayer);
        }
        
        //获取interactable
        if (hit.collider)
        {
            //_currentInteractable = hit.collider.gameObject;
            _currentInteractable = hit.collider.gameObject.GetComponent<InteractableBase>();
        }
    }

    public void EnterInteractable()
    {
        _currentInteractable.SetUpInteractable(_interactablePosition);
    }

    public void MoveInteractable()
    {
        if (_currentInteractable)
        {
            _currentInteractable.UpdateInteractable();
        }
    }

    public void DropInteractable()
    {
        _currentInteractable.DropInteractable();
        _currentInteractable = null;
    }

    #endregion


    public void TurnCheck()
    {
        if (inputVector.x == 0)
        {
            return;
        }

        Vector3 turnScale = new Vector3();

        if (inputVector.x > 0)
        {
            FacingX = 1;
            turnScale = new Vector3(_playerScale.x, transform.localScale.y, transform.localScale.z);
        }
        else if (inputVector.x < 0)
        {
            FacingX = -1;
            turnScale = new Vector3(-_playerScale.x, transform.localScale.y, transform.localScale.z);
        }

        transform.localScale = turnScale;
    }

    public void SetFallGravity()
    {
        _rigidbody2D.gravityScale = _fallGravityScale * -_downDirection.y;
    }

    public void SetNormalGravity()
    {
        _rigidbody2D.gravityScale = _normalGravityScale * -_downDirection.y;
    }

    private void GroundCheck()
    {
        if (_duringJumpCooldown)
        {
            OnGround = false;
            return;
        }

        if (_duringStepCooldown)
        {
            OnGround = true;
            return;
        }
        
        _leftOrigin = (Vector2)_collider2D.bounds.center +
                      new Vector2(-_collider2D.bounds.extents.x, -_collider2D.bounds.extents.y * -_downDirection.y)
                      + _leftOriginOffset;
        _rightOrigin = (Vector2)_collider2D.bounds.center +
                       new Vector2(_collider2D.bounds.extents.x, -_collider2D.bounds.extents.y * -_downDirection.y)
                       + _rightOriginOffset;

        _middleOrigin = (Vector2)_collider2D.bounds.center +
                        new Vector2(0f, -_collider2D.bounds.extents.y * -_downDirection.y);
        
        
        RaycastHit2D leftHit =
            Physics2D.Raycast(_leftOrigin, _downDirection, _groundCheckDistance, _groundLayer);
        RaycastHit2D rightHit =
            Physics2D.Raycast(_rightOrigin, _downDirection, _groundCheckDistance, _groundLayer);
        RaycastHit2D middleHit = 
            Physics2D.Raycast(_middleOrigin, _downDirection, _groundCheckDistance, _groundLayer);
            
        
        
        OnGround = (leftHit.collider || rightHit.collider || middleHit.collider); // 坡面移动时y>0

        // 穿过单向平台时，不能落地，防止重置二段跳
        if (leftHit.collider)
        {
            if (leftHit.collider.CompareTag("OneWay") && Mathf.Abs(_rigidbody2D.velocity.y) > 0.01f)
            {
                OnGround = false;
            } 
        }
        if (rightHit.collider)
        {
            if (rightHit.collider.CompareTag("OneWay") && Mathf.Abs(_rigidbody2D.velocity.y) > 0.01f)
            {
                OnGround = false;
            } 
        }
        if (middleHit.collider)
        {
            if (middleHit.collider.CompareTag("OneWay") && Mathf.Abs(_rigidbody2D.velocity.y) > 0.01f)
            {
                OnGround = false;
            } 
        }
        
        
        // GroundCheck在update中进行，因此需要在这里归一化
        if (OnGround)
        {
            if (Vector2.Dot(-_downDirection, leftHit.normal) > _minGroundDotProduct)
            {
                _averageContactNormal += leftHit.normal;
            }
            // else
            // {
            //     Debug.Log("Left Hit Too Steep");
            // }

            if (Vector2.Dot(-_downDirection, rightHit.normal) > _minGroundDotProduct)
            {
                _averageContactNormal += rightHit.normal;
            }
            // else
            // {
            //     Debug.Log("Right Hit Too Steep");
            // }

            if (Vector2.Dot(-_downDirection, middleHit.normal) > _minGroundDotProduct)
            {
                _averageContactNormal += middleHit.normal;
            }

            _averageContactNormal.Normalize();
        }
        else
        {
            _averageContactNormal = -_downDirection;
        }

        Debug.DrawLine(transform.position, transform.position + (Vector3)_averageContactNormal);


        //DRAW LINE
        Debug.DrawLine(_leftOrigin,
            _leftOrigin + (Vector3)_downDirection * _groundCheckDistance,
            leftHit.collider ? Color.red : Color.green);
        Debug.DrawLine(_rightOrigin,
            _rightOrigin + (Vector3)_downDirection * _groundCheckDistance,
            rightHit.collider ? Color.red : Color.green);
        Debug.DrawLine(_middleOrigin,
            _middleOrigin + (Vector3)_downDirection * _groundCheckDistance,
            middleHit.collider ? Color.red : Color.green);
    }

    private void StepCheck()
    {
        _topStepCheckOrigin = _bottomStepCheckOrigin.position + new Vector3(0f, -_downDirection.y, 0f) * _maxStepHeight;
        Vector3 facingDir = FacingX * Vector3.right;

        // 下方检测到碰撞体 且上方没有检测到
        RaycastHit2D bottomHit =
            Physics2D.Raycast(_bottomStepCheckOrigin.position, facingDir, _stepCheckDistance, _groundLayer);
        if (bottomHit.collider)
        {
            RaycastHit2D topHit =
                Physics2D.Raycast(_topStepCheckOrigin, facingDir, _stepCheckDistance, _groundLayer);

            if (!topHit.collider)
            {
                _shouldStepUp = true;
            }
        }


        Debug.DrawLine(_bottomStepCheckOrigin.position,
            _bottomStepCheckOrigin.position + _stepCheckDistance * facingDir,
            _shouldStepUp ? Color.red : Color.green);
        Debug.DrawLine(_topStepCheckOrigin, _topStepCheckOrigin + _stepCheckDistance * facingDir,
            _shouldStepUp ? Color.red : Color.green);
    }

    private void SurfaceCheck()
    {
        RaycastHit2D downRay =
            Physics2D.Raycast(transform.position, _downDirection, 1f, _groundLayer);

        if (downRay.collider)
        {
            if (downRay.collider.TryGetComponent(out SurfaceType surface))
            {
                CurrentSurfaceType = surface.CurrentSurface;
            }
        }
    }


    private Vector2 ProjectDirectionOnPlane(Vector2 direction, Vector2 normal)
    {
        return (direction - normal * Vector2.Dot(direction, normal)).normalized;
    }

    public void FlipGravity()
    {
        // 反转sprite
        transform.localScale = new Vector3(transform.localScale.x, -transform.localScale.y, transform.localScale.z);

        _rigidbody2D.gravityScale = -_rigidbody2D.gravityScale;
        _downDirection = new Vector2(_downDirection.x, -_downDirection.y);
    }

    public void FreezePosition()
    {
        _rigidbody2D.bodyType = RigidbodyType2D.Static;
    }

    public void UnfreezePosition()
    {
        _rigidbody2D.bodyType = RigidbodyType2D.Dynamic;
    }

    public void ManualControlMoveInput(float horizontal)
    {
        IgnoreInput = true;
        inputVector = new Vector2(horizontal, 0f);
    }
}