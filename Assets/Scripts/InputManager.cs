using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    private InputActions _inputActions;
    private InputAction _pointAction;
    private InputAction _shootAction;
    private InputAction _backAction;

    public Vector2 PointerScreenPosition => _pointAction.ReadValue<Vector2>();
    public bool IsShootPressed => _shootAction.WasPressedThisFrame();
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

    private void OnEnable()
    {
        _inputActions.Enable();
    }

    private void OnDisable()
    {
        _inputActions.Disable();
    }

}
