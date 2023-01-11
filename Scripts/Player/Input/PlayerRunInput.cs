using System.Threading.Tasks;
using UnityEngine.InputSystem;

public sealed class PlayerRunInput : IRunnable
{
    private const float JumpTimeoutLength = 0.5f;
    
    private bool _isLeftPressed;
    private bool _isRightPressed;
    private bool _isRollPressed;
    private bool _isJumpPressed;
    private bool _isBoardPressed;

    private bool _isJumpTimeout;

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
            return true;
        }
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

        if (!_isRollPressed)
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

        await Task.Delay((int)(1000 * JumpTimeoutLength), GlobalCancellationToken.Instance.CancellationToken);

        _isJumpTimeout = false;
    }
}