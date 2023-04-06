using System;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

namespace StarterAssets
{
    [RequireComponent(typeof(CharacterController))]
    public class ThirdPersonController : MonoBehaviour, IPauseable, IRunnable
    {
        private const float CameraShakeIntensity = 5f;
        private const float CameraShakeTime = 0.3f;
        private const int JumpEffectTimeMilliseconds = 5000;
        private const float ExplosionEffectYOffset = 1f;
        private const int ExplosionEffectTimeMilliseconds = 5000;
        private const int PlayerChestIndex = 3;
        private const float ClothesScale = 1;
        
        [SerializeField] private RunProgress _runProgress;
        [SerializeField] private Map _map;
        [SerializeField] private ActiveItemsUI _activeItemsUI;
        [SerializeField] private Run _run;
        [SerializeField] private Follower _follower;
        [SerializeField] private Factories _factories;
        [SerializeField] private Transform _playerBones;
        [SerializeField] private GameObject _playerMesh;
        [SerializeField] private InputActionAsset _inputActionAsset;
        [SerializeField] private CinemachineVirtualCamera _runCamera;
        [SerializeField] private CinemachineVirtualCamera _idleCamera;
        [SerializeField] private Camera _playerRunCamera;
        [SerializeField] private Transform _playerRootBone;

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
        private GameObject _mainCamera;

        private IHittable _hittable;
        private Dictionary<Type, IHittable> _hittables;
        private Board _board;

        private IGravitable _gravitable;
        private IRollable _rollable;
        private Dictionary<Type, IGravitable> _gravitables;

        private HorizontalMoveRestriction _horizontalMoveRestriction;

        private bool _isPause;
        private bool _isFall;
        private Vector3 _startPosition;

        private PlayerRunInput _playerRunInput;
        private PlayerCamera _playerCamera;

        private PlayerClothes _playerClothes;

        public Effects Effects { get; private set; }

        public Dictionary<Type, IHittable> Hittables => _hittables;

        public Dictionary<Type, IGravitable> Gravitables => _gravitables;
        
        public Transform PlayerBones => _playerBones;

        public CharacterController Controller { get; private set; }

        public Dictionary<Type, HorizontalMoveRestriction> HorizontalMoveRestrictions { get; private set; }

        public PlayerAnimator PlayerAnimator { get; private set; }
        
        public PlayerStateMachine PlayerStateMachine { get; private set; }
        
        public bool IsDie { get; private set; }

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
            //_animator.enabled = false;
        }

        public void UnPause()
        {
            _isPause = false;
            //_animator.enabled = true;
        }

        public void Resurrect()
        {
            PlayerAnimator.ChangeAnimationTrigger(AnimationType.Resurrect);
            Effects.ActivateEffect(
                EffectType.Explosion, 
                transform.position + ExplosionEffectYOffset * Vector3.up,
                ExplosionEffectTimeMilliseconds);
            
            Sounds.Instance.PlayRandomSounds(2, "Resurrect");

            IsDie = false;
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
            int currentLine = _horizontalMoveRestriction?.CurrentMovingLine ?? 0;
            _horizontalMoveRestriction = newRestriction;
            _horizontalMoveRestriction.Init(currentLine);
        }

        public void StartRun()
        {
            transform.position = _startPosition;
            Controller.enabled = true;

            PlayerStateMachine.StartRun();
            _playerRunInput.StartRun();
            _playerCamera.StartRun();

            _isPause = false;
            _isFall = false;
            IsDie = false;
            ChangeHittable(_hittables[typeof(PlayerHittable)]);
            ChangeHorizontalMoveRestriction(HorizontalMoveRestrictions[typeof(HorizontalMoveRestriction)]);
            _horizontalMoveRestriction.Init(0);
            ChangeGravitable(_gravitables[typeof(DefaultGravity)]);
            
            _playerMesh.SetActive(true);
            
            PlayerAnimator.ChangeAnimationTrigger(AnimationType.Reset);
            
            _playerClothes.ChangeClothes(ClothesStorage.PlayerClothes.Value, false);
            
            Sounds.Instance.DisableAudioForTime(1, 2f);
        }

        public void EndRun()
        {
            PlayerStateMachine.EndRun();
            _playerRunInput.EndRun();
            _playerCamera.EndRun();

            Controller.enabled = false;
            //Controller.Move(_startPosition - transform.localPosition);
            _isPause = true;
            _isMovingX = false;
            IsDie = false;
            _movingXDir = 0;
            _previousMovingDestination = 0;
            _movingDestination = 0; 
            _movingXQueue = 0;
            CinemachineCameraTarget.transform.localRotation = Quaternion.identity;

            _playerMesh.SetActive(false);
        }

        public void SetStartRunPosition(
            Vector3 playerStartPosition,
            Vector3 cameraStartPosition,
            Quaternion cameraStartRotation)
        {
            /*playerStartPosition = new Vector3(_startPosition.x, _startPosition.y, playerStartPosition.z);
            Controller.Move(playerStartPosition - transform.localPosition);
            Transform cameraTransform = _playerRunCamera.transform;
            cameraTransform.position = cameraStartPosition;
            cameraTransform.rotation = cameraStartRotation;*/
        }
        
        public void StopRecover()
        {
            (_hittable as PlayerHittable)?.StopRecover();
        }

        public void Lose()
        {
            PlayerAnimator.ChangeAnimationTrigger(AnimationType.Lose);
            _follower.Lose(this);
            
            Sounds.Instance.PlayRandomSounds(2, "Die");
        }

        private void Awake()
        {
            Effects = new Effects(AsyncUtils.Instance);
            
            InputActionMap inputActionMap = _inputActionAsset.FindActionMap("Player", true);
            _playerRunInput = new PlayerRunInput(inputActionMap, AsyncUtils.Instance);
            inputActionMap.Enable();

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
            
            _playerClothes = new PlayerClothes(
                _playerRootBone,
                _playerBones.parent.gameObject,
                PlayerChestIndex,
                ClothesScale);
        }

        private void Start()
        {
            _cinemachineTargetYaw = CinemachineCameraTarget.transform.rotation.eulerAngles.y;
            
            TryGetComponent(out _animator);
            _animator.keepAnimatorControllerStateOnDisable = true;
            Controller = GetComponent<CharacterController>();

            PlayerAnimator = new PlayerAnimator(_animator, this);
            _playerCamera = new PlayerCamera(_playerRunCamera, _runCamera, _idleCamera, _run);

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
                        PlayerAnimator)
                },
                {
                    typeof(FlyGravity),
                    new FlyGravity(
                        Gravity,
                        JumpHeight * 5,
                        SprintSpeed,
                        this,
                        _map,
                        _runProgress,
                        _run,
                        PlayerAnimator)
                },
                {
                    typeof(SpringGravity),
                    new SpringGravity(
                        Gravity / 2,
                        JumpHeight * 7,
                        SprintSpeed,
                        this,
                        _playerRunInput,
                        _map,
                        _runProgress,
                        _run,
                        PlayerAnimator)
                },
            };
            
            _board = new Board(this, _map, _run, Effects);
            _hittables = new Dictionary<Type, IHittable>
            {
                { typeof(Board), _board },
                { typeof(PlayerHittable), new PlayerHittable(this, _follower, _run) },
                { typeof(ImmuneHittable), new ImmuneHittable(_map, Effects, transform) },
            };

            PlayerStateMachine = new PlayerStateMachine(this, _activeItemsUI, _follower, Effects);
        }

        private void FixedUpdate()
        {
            GroundedCheck();
            
            if (_isFall)
            {
                Fall();
            }

            if (_isPause)
            {
                return;
            }
            
            BoardActive();
            Roll();
            JumpAndGravity();
            Move();
            CameraRotation();
        }

        private void BoardActive()
        {
            if (_playerRunInput.IsBoardPressed && Stats.BoardCount.Value > 0)
            {
                if (PlayerStateMachine.ChangeStateSafely(typeof(RunState), typeof(BoardState)))
                {
                    Stats.BoardCount.Value -= 1;
                }
            }
        }

        private void GroundedCheck()
        {
            Vector3 position = transform.position;
            Vector3 spherePosition = new (position.x, position.y - GroundedOffset, position.z);
            bool isGrounded = Physics.CheckSphere(
                spherePosition, GroundedRadius,
                GroundLayers,
                QueryTriggerInteraction.Ignore);

            if (isGrounded && !Grounded)
            {
                Effects.ActivateEffect(EffectType.Jump, spherePosition, JumpEffectTimeMilliseconds);
                Sounds.Instance.PlaySound(1, "Landing");
            }
            
            Grounded = isGrounded;
            
            PlayerAnimator.ChangeAnimationBool(AnimationType.Land, Grounded);
        }

        private void CameraRotation()
        {
            CinemachineCameraTarget.transform.localRotation = Quaternion.Euler(
                0,
                -transform.rotation.eulerAngles.y,
                0.0f);
        }

        private void Move()
        {
            int dir = CheckForMovingX();
            
            Vector3 inputMove = new(dir, 0, _moveZ);

            float targetSpeed = SprintSpeed;

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

            PlayerAnimator.ChangeAnimationFloat(AnimationType.HorizontalRun, inputDirection.x);

            Controller.Move(
                new Vector3(inputMove.x,0,0) * 
                (_speed * _runProgress.HalfSpeedMultiplier * Time.fixedDeltaTime) +
                new Vector3(0.0f, _verticalVelocity, 0.0f) * Time.fixedDeltaTime +
                Vector3.forward * (_speed * _runProgress.SpeedMultiplier * Time.fixedDeltaTime));
            
            PlayerAnimator.ChangeAnimationFloat(AnimationType.Speed, _runProgress.HalfSpeedMultiplier);

            if (PlayerAnimator.CurrentAnimatorType == typeof(PlayerDefaultAnimator))
            {
                PlayerAnimator.ChangeAnimationFloat(AnimationType.HitSpeed, _runProgress.HalfSpeedMultiplier + 1);
            }
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
                    Die(hit.normal);
                }
                else if (hitType == HitType.Soft)
                {
                    _movingDestination = _previousMovingDestination;
                    _movingXDir *= -1;
                    _horizontalMoveRestriction.CheckHorizontalMoveRestriction(_movingXDir);
                    _playerCamera.ShakeCamera(CameraShakeIntensity, CameraShakeTime);
                    
                    PlayerAnimator.ChangeAnimationTrigger(
                        hit.normal.x < 0 ? AnimationType.SoftHitLeft : AnimationType.SoftHitRight);
                }
            }
            else if (hit.gameObject.TryGetComponent(out Item item))
            {
                item.PickupItem(this);
            }
        }

        private void Die(Vector3 hitNormal)
        {
            _playerCamera.StopShake();
            _run.Lose();
            StopRecover();
            AnimationType dieAnimationType = 
                Mathf.Abs(hitNormal.x) < 0.5f ? AnimationType.Die :
                hitNormal.x < 0 ? AnimationType.DieLeft : 
                AnimationType.DieRight;
            PlayerAnimator.ChangeAnimator(typeof(PlayerDefaultAnimator));
            PlayerAnimator.ChangeAnimationTrigger(dieAnimationType);
            _isFall = true;
            IsDie = true;
        }

        private void Fall()
        {
            if (Grounded)
            {
                return;
            }

            JumpAndGravity();
            GroundedCheck();

            Controller.Move(new Vector3(0.0f, _verticalVelocity, 0.0f) * Time.fixedDeltaTime);
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