using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;
#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
using UnityEngine.InputSystem;
#endif

/* Note: animations are called via the controller for both the character and capsule using animator null checks
 */

namespace StarterAssets
{
    [RequireComponent(typeof(CharacterController))]
    public class ThirdPersonController : MonoBehaviour, IPauseable, IRunnable
    {
        [SerializeField] private RunProgress _runProgress;
        [SerializeField] private Map _map;
        [SerializeField] private ActiveItemsUI _activeItemsUI;
        [SerializeField] private Run _run;
        [SerializeField] private Follower _follower;
        [SerializeField] private Factories _factories;
        [SerializeField] private Transform _playerMesh;

        [Header("Player")]
        [Tooltip("Move speed of the character in m/s")]
        public float MoveSpeed = 2.0f;

        [Tooltip("Sprint speed of the character in m/s")]
        public float SprintSpeed = 5.335f;

        [Tooltip("How fast the character turns to face movement direction")]
        [Range(0.0f, 0.3f)]
        public float RotationSmoothTime = 0.12f;

        [Tooltip("Acceleration and deceleration")]
        public float SpeedChangeRate = 10.0f;

        public AudioClip LandingAudioClip;
        public AudioClip[] FootstepAudioClips;
        [Range(0, 1)] public float FootstepAudioVolume = 0.5f;

        [Space(10)]
        [Tooltip("The height the player can jump")]
        public float JumpHeight = 2f;

        [Tooltip("The character uses its own gravity value. The engine default is -9.81f")]
        public float Gravity = -15.0f;

        [Space(10)]
        [Tooltip("Time required to pass before being able to jump again. Set to 0f to instantly jump again")]
        public float JumpTimeout = 0.50f;

        [Tooltip("Time required to pass before entering the fall state. Useful for walking down stairs")]
        public float FallTimeout = 0.15f;

        [Header("Player Grounded")]
        [Tooltip("If the character is grounded or not. Not part of the CharacterController built in grounded check")]
        public bool Grounded = true;

        [Tooltip("Useful for rough ground")]
        public float GroundedOffset = -0.14f;

        [Tooltip("The radius of the grounded check. Should match the radius of the CharacterController")]
        public float GroundedRadius = 0.28f;

        [Tooltip("What layers the character uses as ground")]
        public LayerMask GroundLayers;

        [Header("Cinemachine")]
        [Tooltip("The follow target set in the Cinemachine Virtual Camera that the camera will follow")]
        public GameObject CinemachineCameraTarget;

        [Tooltip("How far in degrees can you move the camera up")]
        public float TopClamp = 70.0f;

        [Tooltip("How far in degrees can you move the camera down")]
        public float BottomClamp = -30.0f;

        [Tooltip("Additional degress to override the camera. Useful for fine tuning camera position when locked")]
        public float CameraAngleOverride = 0.0f;

        [Tooltip("For locking the camera position on all axis")]
        public bool LockCameraPosition = false;

        private float _moveZ = 1;
        private bool _isMovingX;
        private int _movingXDir;
        private int _movingDestination;
        private int _movingXQueue;
        private int _previousMovingDestination;

        // cinemachine
        private float _cinemachineTargetYaw;
        private float _cinemachineTargetPitch;

        // player
        private float _speed;
        private float _animationBlend;
        private float _targetRotation;
        private float _verticalVelocity;
        private float _terminalVelocity = 53.0f;
        
        private Animator _animator;
        private StarterAssetsInputs _input;
        private GameObject _mainCamera;
        private PlayerAnimator _playerAnimator;

        private IHittable _hittable;
        private Dictionary<Type, IHittable> _hittables;
        private Board _board;

        private IGravitable _gravitable;
        private IRollable _rollable;
        private Dictionary<Type, IGravitable> _gravitables;

        private HorizontalMoveRestriction _horizontalMoveRestriction;

        private bool _isPause;
        private Vector3 _startPosition;

        private PlayerRunInput _playerRunInput;

        public Dictionary<Type, IHittable> Hittables => _hittables;

        public Dictionary<Type, IGravitable> Gravitables => _gravitables;
        
        public Transform PlayerMesh => _playerMesh;

        public CharacterController Controller { get; private set; }

        public Dictionary<Type, HorizontalMoveRestriction> HorizontalMoveRestrictions { get; private set; }

        public bool IsRoll { get; private set; }

        public void EndRoll()
        {
            _rollable?.EndRoll();
            IsRoll = false;
        }

        public void StartRoll()
        {
            IsRoll = true;
        }

        public void Pause()
        {
            _isPause = true;
            _animator.enabled = false;
        }

        public void UnPause()
        {
            _isPause = false;
            _animator.enabled = true;
        }

        public void ChangeHittable(IHittable newHittable)
        {
            _hittable = newHittable;
        }

        public void ChangeGravitable(IGravitable newGravitable)
        {
            _gravitable?.LeaveGravity();
            _gravitable = newGravitable;
            _gravitable.EnterGravity();

            _rollable = _gravitable as IRollable;
        }

        public void ChangeHorizontalMoveRestriction(HorizontalMoveRestriction newRestriction)
        {
            _horizontalMoveRestriction = _horizontalMoveRestriction?.ChangeRestriction(newRestriction) ?? newRestriction;
        }

        public void StartRun()
        {
            _playerRunInput.StartRun();

            _isPause = false;
            ChangeHittable(_hittables[typeof(PlayerHittable)]);
            ChangeHorizontalMoveRestriction(HorizontalMoveRestrictions[typeof(HorizontalMoveRestriction)]);
            ChangeGravitable(_gravitables[typeof(DefaultGravity)]);
        }

        public void EndRun()
        {
            _playerRunInput.EndRun();
            
            Controller.Move(_startPosition - transform.localPosition);
            _isPause = true;
            _isMovingX = false;
            _movingXDir = 0;
            _previousMovingDestination = 0;
            _movingDestination = 0; 
            _movingXQueue = 0;
        }

        private void Awake()
        {
            _playerRunInput = new PlayerRunInput();
            
            _startPosition = transform.position;
            _isPause = true;
            
            if (_mainCamera == null)
            {
                _mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
            }

            HorizontalMoveRestrictions = new Dictionary<Type, HorizontalMoveRestriction>
            {
                { typeof(HorizontalMoveRestriction), new HorizontalMoveRestriction() },
                { typeof(FlyHorizontalRestriction), new FlyHorizontalRestriction() },
            };
        }

        private void Start()
        {
            _cinemachineTargetYaw = CinemachineCameraTarget.transform.rotation.eulerAngles.y;
            
            TryGetComponent(out _animator);
            _animator.keepAnimatorControllerStateOnDisable = true;
            Controller = GetComponent<CharacterController>();
            _input = GetComponent<StarterAssetsInputs>();

            _playerAnimator = new PlayerAnimator(_animator, this);

            _gravitables = new Dictionary<Type, IGravitable>
            {
                {
                    typeof(DefaultGravity),
                    new DefaultGravity(
                        FallTimeout,
                        JumpTimeout,
                        _terminalVelocity,
                        Gravity,
                        _playerRunInput,
                        this,
                        _playerAnimator)
                },
                {
                    typeof(FlyGravity),
                    new FlyGravity(
                        Gravity,
                        JumpHeight * 10,
                        SprintSpeed,
                        this,
                        _factories.ItemFactories[ItemType.Money] as MoneyItemFactory<Item>,
                        _map,
                        _runProgress,
                        _playerAnimator)
                },
                {
                    typeof(SpringGravity),
                    new SpringGravity(
                        Gravity / 2,
                        JumpHeight * 20,
                        SprintSpeed,
                        this,
                        _playerRunInput,
                        _factories.ItemFactories[ItemType.Money] as MoneyItemFactory<Item>,
                        _map,
                        _runProgress,
                        _playerAnimator)
                },
            };
            
            _board = new Board(this, _map, _activeItemsUI, _playerAnimator);
            _hittables = new Dictionary<Type, IHittable>
            {
                { typeof(Board), _board },
                { typeof(PlayerHittable), new PlayerHittable(this, _follower) },
                { typeof(ImmuneHittable), new ImmuneHittable(_map) },
            };
        }

        private void FixedUpdate()
        {
            if (_isPause)
            {
                return;
            }
            
            BoardActive();
            Roll();
            JumpAndGravity();
            GroundedCheck();
            Move();
            CameraRotation();
        }

        private void Update()
        {
            CheckForMovingInput();
        }

        private void CheckForMovingInput()
        {
            _playerRunInput.Update();
        }

        private void BoardActive()
        {
            if (_playerRunInput.IsBoardPressed && _hittable.GetType() != typeof(ImmuneHittable))
            {
                _board.Activate();
            }
        }

        private void GroundedCheck()
        {
            Vector3 position = transform.position;
            Vector3 spherePosition = new Vector3(position.x, position.y - GroundedOffset, position.z);
            Grounded = Physics.CheckSphere(
                spherePosition, GroundedRadius,
                GroundLayers,
                QueryTriggerInteraction.Ignore);
            
            _playerAnimator.ChangeAnimationBool(AnimationType.Land, Grounded);
        }

        private void CameraRotation()
        {
            _cinemachineTargetYaw = ClampAngle(_cinemachineTargetYaw, float.MinValue, float.MaxValue);
            _cinemachineTargetPitch = ClampAngle(_cinemachineTargetPitch, BottomClamp, TopClamp);

            CinemachineCameraTarget.transform.rotation = Quaternion.Euler(
                _cinemachineTargetPitch + CameraAngleOverride,
                _cinemachineTargetYaw, 0.0f);
        }

        private void Move()
        {
            int dir = CheckForMovingX();
            
            Vector3 inputMove = new(dir, 0, _moveZ);
            _input.sprint = true;

            float targetSpeed = _input.sprint ? SprintSpeed : MoveSpeed;

            if (inputMove == Vector3.zero) targetSpeed = 0.0f;

            _speed = targetSpeed;

            _animationBlend = Mathf.Lerp(_animationBlend, targetSpeed, Time.fixedDeltaTime * SpeedChangeRate);
            if (_animationBlend < 0.01f) _animationBlend = 0f;
            
            Vector3 inputDirection = inputMove.normalized;

            if(!_isMovingX)
            {
                float xDelta = _movingDestination - transform.localPosition.x;
                inputDirection = new Vector3(
                    xDelta > 0.1f ? xDelta : 0,
                    inputDirection.y,
                    inputDirection.z);
            }

            _playerAnimator.ChangeAnimationFloat(AnimationType.HorizontalRun, inputDirection.x);

            Controller.Move(
                new Vector3(inputMove.x,0,0) * 
                (_speed * _runProgress.HalfSpeedMultiplier * Time.fixedDeltaTime) +
                new Vector3(0.0f, _verticalVelocity, 0.0f) * Time.fixedDeltaTime +
                Vector3.forward * (_speed * _runProgress.SpeedMultiplier * Time.fixedDeltaTime));
            
            _playerAnimator.ChangeAnimationFloat(AnimationType.Speed, _runProgress.HalfSpeedMultiplier);
        }

        private int CheckForMovingX()
        {
            int dir = 0;
            CheckForInput(_playerRunInput.IsLeftPressed, -1, ref dir);
            CheckForInput(_playerRunInput.IsRightPressed, 1, ref dir);

            float xDelta = transform.localPosition.x - _movingDestination;
            if (_isMovingX && Mathf.Abs(xDelta) > 0.1f && Math.Sign(-xDelta) == _movingXDir)
            {
                dir = _movingXDir;
            }
            else if (_isMovingX)
            {
                Vector3 position = transform.localPosition;
                Controller.Move(new Vector3(
                    _movingDestination,
                    position.y,
                    position.z) - position);
                _isMovingX = false;
            }
            else if(_movingXQueue != 0)
            {
                if (!_horizontalMoveRestriction.CheckHorizontalMoveRestriction(_movingXQueue))
                {
                    _movingXQueue = 0;
                    return dir;
                }
                
                SetupMovingX(_movingXQueue, true);
                dir = _movingXQueue;
            }

            return dir;

            void CheckForInput(bool input, int direction, ref int outputDir)
            {
                if (input)
                {
                    if (!_isMovingX && !_horizontalMoveRestriction.CheckHorizontalMoveRestriction(direction))
                        return;
                    
                    if (!_isMovingX)
                    {
                        SetupMovingX(direction);
                        outputDir = direction;
                    }
                    else
                    {
                        _movingXQueue = direction;
                    }
                }
            }
        }
        
        private void SetupMovingX(int dir, bool clearQueue = false)
        {
            _isMovingX = true;
            _movingXDir = dir;
            _previousMovingDestination = _movingDestination;
            _movingDestination = (int)Map.GetClosestColumn(transform.localPosition.x) + Map.ColumnOffset * dir;

            if (clearQueue) _movingXQueue = 0;
        }

        private void Roll()
        {
            _rollable?.Roll(Grounded);
        }

        private void JumpAndGravity()
        {
            _verticalVelocity = _gravitable.VerticalVelocity(Grounded);
        }

        private static float ClampAngle(float lfAngle, float lfMin, float lfMax)
        {
            if (lfAngle < -360f) lfAngle += 360f;
            if (lfAngle > 360f) lfAngle -= 360f;
            return Mathf.Clamp(lfAngle, lfMin, lfMax);
        }

        private void OnFootstep(AnimationEvent animationEvent)
        {
            if (animationEvent.animatorClipInfo.weight > 0.5f)
            {
                if (FootstepAudioClips.Length > 0)
                {
                    var index = Random.Range(0, FootstepAudioClips.Length);
                    AudioSource.PlayClipAtPoint(
                        FootstepAudioClips[index],
                        transform.TransformPoint(Controller.center),
                        FootstepAudioVolume);
                }
            }
        }

        private void OnLand(AnimationEvent animationEvent)
        {
            if (animationEvent.animatorClipInfo.weight > 0.5f)
            {
                AudioSource.PlayClipAtPoint(
                    LandingAudioClip,
                    transform.TransformPoint(Controller.center),
                    FootstepAudioVolume);
            }
        }

        private void OnControllerColliderHit(ControllerColliderHit hit)
        {
            if (hit.gameObject.layer == 7)
            {
                if(Mathf.Abs(hit.normal.y) > 0.5f || hit.normal.z > 0)
                    return;
                HitType hitType = Mathf.Abs(hit.normal.x) > 0.5f ? HitType.Soft : HitType.Hard;
                bool result = _hittable.Hit(hitType);

                if (result)
                {
                    Die();
                }
                else if (hitType == HitType.Soft)
                {
                    _movingDestination = _previousMovingDestination;
                    _movingXDir *= -1;
                }
            }
            else if (hit.gameObject.TryGetComponent(out Item item))
            {
                item.PickupItem(this);
            }
        }

        private void Die()
        {
            _run.Lose();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.TryGetComponent(out Item item))
            {
                item.PickupItem(this);
            }
        }
    }
}