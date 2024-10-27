using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class UserInput : MonoBehaviour
{
    public static UserInput instance;

    public Vector2 moveInput {  get; private set; }
    public Vector2 attackDirInput {  get; private set; }
    public bool attackDirDown {  get; private set; }
    public bool dashInput { get; private set; }
    public bool pauseInput { get; private set; }
    public bool interactInput { get; private set; }
    public bool statsInput { get; private set; }
    public bool statsInputDown { get; private set; }
    public bool statsInputUp { get; private set; }
    public bool isKeyboard { get; private set; }
    public bool isGamepad { get; private set; }
    public bool mouseIsMoveing { get; private set; }

    [SerializeField] float timeToHideMouseWhenStatic = 2;

    Vector3 lastMousePosition;
    float currentTime = 0;

    PlayerInput _playerInput;

    InputAction _moveAction;
    InputAction _attackDirAction;
    InputAction _dashAction;
    InputAction _pauseAction;
    InputAction _interactAction;
    InputAction _statsAction;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }

        _playerInput = GetComponent<PlayerInput>();

        SetupInputActions();
    }

    private void Update()
    {
        UpdateInputs();
    }

    void SetupInputActions()
    {
        _moveAction = _playerInput.actions["Move"];
        _attackDirAction = _playerInput.actions["AttackDir"];
        _dashAction = _playerInput.actions["Dash"];
        _pauseAction = _playerInput.actions["Pause"];
        _interactAction = _playerInput.actions["Interact"];
        _statsAction = _playerInput.actions["Stats"];
    }

    void UpdateInputs()
    {
        attackDirInput = _attackDirAction.ReadValue<Vector2>();
        attackDirDown = _attackDirAction.WasPressedThisFrame();
        dashInput = _dashAction.IsPressed();
        pauseInput = _pauseAction.WasPressedThisFrame();
        interactInput = _interactAction.WasPressedThisFrame();
        moveInput = _moveAction.ReadValue<Vector2>();
        statsInput = _statsAction.IsPressed();
        statsInputDown = _statsAction.WasPressedThisFrame();
        statsInputUp = _statsAction.WasReleasedThisFrame();
        isKeyboard = _playerInput.currentControlScheme == "Keyboard";
        isGamepad = _playerInput.currentControlScheme == "Gamepad";
        mouseIsMoveing = MouseIsMoveing();

        HideCursorWhenStatic();
    }

    public void HideCursorWhenStatic()
    {
        if (!mouseIsMoveing)
        {
            if (Input.mouseScrollDelta.y != 0) currentTime = timeToHideMouseWhenStatic;
            if (Cursor.visible) currentTime -= Time.unscaledDeltaTime;
            if (currentTime <= 0) StartCoroutine(HideCursor());
        }
        else currentTime = timeToHideMouseWhenStatic;
    }

    public IEnumerator HideCursor()
    {
        Mouse.current.WarpCursorPosition(new Vector2(Screen.currentResolution.width / 2, 0));

        yield return null;

        currentTime = 0;
        Cursor.visible = false;
    }

    bool MouseIsMoveing()
    {
        if (Input.mousePosition != lastMousePosition)
        {
            lastMousePosition = Input.mousePosition;
            Cursor.visible = true;
            return true;
        }
        else return false;
    }
}
