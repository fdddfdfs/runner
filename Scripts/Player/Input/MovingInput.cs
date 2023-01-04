using UnityEngine.InputSystem;

public class MovingInput : IRunnable
{
    private bool _isLeftPressed;
    private bool _isRightPressed;
    private bool _isRollPressed;
    private bool _isJumpPressed;

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
        if (!_isJumpPressed)
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
    }
}