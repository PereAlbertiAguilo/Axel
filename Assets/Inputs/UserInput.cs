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
    public bool viewInputDown {  get; private set; }
    public bool attackInput { get; private set; }
    public bool attackInputUp { get; private set; }
    public bool changeAttackInput { get; private set; }
    public bool lockEnemyInput { get; private set; }
    public bool dashInput { get; private set; }
    public bool pauseInput { get; private set; }
    public bool interactInput { get; private set; }
    public bool statsInputDown { get; private set; }
    public bool statsInputUp { get; private set; }

    public bool isKeyboard;

    PlayerInput _playerInput;

    InputAction _moveAction;
    InputAction _viewAction;
    InputAction _attackAction;
    InputAction _changeAttackAction;
    InputAction _lockEnemyAction;
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
        _viewAction = _playerInput.actions["ViewMove"];
        _attackAction = _playerInput.actions["Attack"];
        _changeAttackAction = _playerInput.actions["Change Attack"];
        _lockEnemyAction = _playerInput.actions["Lock Enemy"];
        _dashAction = _playerInput.actions["Dash"];
        _pauseAction = _playerInput.actions["Pause"];
        _interactAction = _playerInput.actions["Interact"];
        _statsAction = _playerInput.actions["Stats"];
    }

    void UpdateInputs()
    {
        attackDirInput = _viewAction.ReadValue<Vector2>();
        viewInputDown = _viewAction.WasPressedThisFrame();
        attackInput = _attackAction.IsPressed();
        attackInputUp = _attackAction.WasReleasedThisFrame();
        changeAttackInput = _changeAttackAction.WasPressedThisFrame();
        lockEnemyInput = _lockEnemyAction.WasPressedThisFrame();
        dashInput = _dashAction.IsPressed();
        pauseInput = _pauseAction.WasPressedThisFrame();
        interactInput = _interactAction.WasPressedThisFrame();
        //isKeyboard = Input.GetJoystickNames()[Input.GetJoystickNames().Length].Length < 1;
        moveInput = _moveAction.ReadValue<Vector2>();
        statsInputDown = _statsAction.WasPressedThisFrame();
        statsInputUp = _statsAction.WasReleasedThisFrame();
    }
}
