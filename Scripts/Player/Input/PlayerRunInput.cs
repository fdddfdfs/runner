using System.Threading.Tasks;
using UnityEngine.InputSystem;

public sealed class PlayerRunInput : IRunnable
{
    private const float JumpTimeoutLength = 0.5f;
    private const float RollTimeoutLength = 0.5f;

    private readonly ICancellationTokenProvider _cancellationTokenProvider;
    
    private bool _isLeftPressed;
    private bool _isRightPressed;
    private bool _isRollPressed;
    private bool _isJumpPressed;
    private bool _isBoardPressed;

    private bool _isJumpTimeout;
    private bool _isRollTimeout;
    
    private bool _isRun;

    public bool IsBoardPressed
    {
        get
        {
            if (!_isBoardPressed) return false;

            _isBoardPressed = false;
            return true;
        }
    }
    
    public bool IsLeftPressed
    {
        get
        {
            if (!_isLeftPressed) return false;
            
            _isLeftPressed = false;
            return true;
        }
    }
    
    public bool IsRightPressed
    {
        get
        {
            if (!_isRightPressed) return false;
            
            _isRightPressed = false;
            return true;
        }
    }
    
    public bool IsJumpPressed
    {
        get
        {
            if (!_isJumpPressed) return false;
            
            _isJumpPressed = false;
            JumpTimeout();
            return true;
        }
    }
    
    public bool IsRollPressed
    {
        get
        {
            if (!_isRollPressed) return false;
            
            _isRollPressed = false;
            RollTimeout();
            return true;
        }
    }
    
    public PlayerRunInput(InputActionMap inputActionMap, ICancellationTokenProvider cancellationTokenProvider)
    {
        inputActionMap["Left"].started += (_) => _isLeftPressed = true;
        inputActionMap["Left"].canceled += (_) => _isLeftPressed = false;
        inputActionMap["Right"].started += (_) => _isRightPressed = true;
        inputActionMap["Right"].canceled += (_) => _isRightPressed = false;
        inputActionMap["Jump"].started += (_) => _isJumpPressed = true;
        inputActionMap["Jump"].canceled += (_) => _isJumpPressed = false;
        inputActionMap["Roll"].started += (_) => _isRollPressed = true;
        inputActionMap["Roll"].canceled += (_) => _isRollPressed = false;
        inputActionMap["ActivateBoard"].started += (_) => _isBoardPressed = true;
        inputActionMap["ActivateBoard"].canceled += (_) => _isBoardPressed = false;

        _cancellationTokenProvider = cancellationTokenProvider;
    }
    
    public void Update()
    {
        if (!_isRun) return;
        
        if (!_isJumpPressed && !_isJumpTimeout)
        {
            _isJumpPressed = Keyboard.current.spaceKey.wasPressedThisFrame;
        }

        if (!_isLeftPressed)
        {
            _isLeftPressed = Keyboard.current.aKey.wasPressedThisFrame;
        }

        if (!_isRightPressed)
        {
            _isRightPressed = Keyboard.current.dKey.wasPressedThisFrame;
        }

        if (!_isRollPressed && !_isRollTimeout)
        {
            _isRollPressed = Keyboard.current.sKey.wasPressedThisFrame;
        }

        if (!_isBoardPressed)
        {
            _isBoardPressed = Keyboard.current.eKey.wasPressedThisFrame;
        }
    }

    public void StartRun()
    {
        _isRun = true;
    }

    public void EndRun()
    {
        _isRun = false;
        
        _isJumpPressed = false;
        _isLeftPressed = false;
        _isRightPressed = false;
        _isRollPressed = false;
        _isBoardPressed = false;
    }

    private async void JumpTimeout()
    {
        _isJumpTimeout = true;

        await Task.Delay((int)(1000 * JumpTimeoutLength), _cancellationTokenProvider.GetCancellationToken())
            .ContinueWith(AsyncUtils.EmptyTask);

        _isJumpTimeout = false;
    }

    private async void RollTimeout()
    {
        _isRollTimeout = true;

        await Task.Delay((int)(1000 * RollTimeoutLength), _cancellationTokenProvider.GetCancellationToken())
            .ContinueWith(AsyncUtils.EmptyTask);

        _isRollTimeout = false;
    }
}