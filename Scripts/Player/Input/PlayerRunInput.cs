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
        inputActionMap["Jump"].started += (_) =>
        {
            if (_isJumpTimeout) return;
            
            _isJumpPressed = true;
            JumpTimeout();
        };
        
        inputActionMap["Jump"].canceled += (_) => _isJumpPressed = false;
        inputActionMap["Roll"].started += (_) =>
        {
            if (_isRollTimeout) return;
            
            _isRollPressed = true;
            RollTimeout();
        };
        
        inputActionMap["Roll"].canceled += (_) => _isRollPressed = false;
        inputActionMap["ActivateBoard"].started += (_) => _isBoardPressed = true;
        inputActionMap["ActivateBoard"].canceled += (_) => _isBoardPressed = false;

        _cancellationTokenProvider = cancellationTokenProvider;
    }

    public void StartRun()
    {
    }

    public void EndRun()
    {
        _isJumpPressed = false;
        _isLeftPressed = false;
        _isRightPressed = false;
        _isRollPressed = false;
        _isBoardPressed = false;
    }

    private async void JumpTimeout()
    {
        _isJumpTimeout = true;

        await AsyncUtils.Wait(JumpTimeoutLength, _cancellationTokenProvider.GetCancellationToken());

        _isJumpTimeout = false;
    }

    private async void RollTimeout()
    {
        _isRollTimeout = true;

        await AsyncUtils.Wait(RollTimeoutLength, _cancellationTokenProvider.GetCancellationToken());

        _isRollTimeout = false;
    }
}