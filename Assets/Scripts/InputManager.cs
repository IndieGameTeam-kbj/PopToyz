using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    private InputActions _inputActions;
    private InputAction _pointAction;
    private InputAction _shootAction;
    private InputAction _backAction;

    public bool IsBackPressed => _backAction.WasPressedThisFrame();

    public static InputManager Instance { get; private set; }
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        _inputActions = new InputActions();
        _pointAction = _inputActions.Game.Point;
        _shootAction = _inputActions.Game.Shoot;
        _backAction = _inputActions.Game.Back;
    }

    public bool TryGetShootPosition(out Vector2 screenPosition)
    {
        if (!_shootAction.WasPressedThisFrame())
        {
            screenPosition = default;
            return false;
        }

        screenPosition = _pointAction.ReadValue<Vector2>();
        return true;
    }

    private void OnEnable()
    {
        _inputActions.Enable();
    }

    private void OnDisable()
    {
        _inputActions.Disable();
    }

}
