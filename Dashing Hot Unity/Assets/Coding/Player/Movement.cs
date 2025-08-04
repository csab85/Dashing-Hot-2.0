//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.InputSystem;

//public class Movement : MonoBehaviour
//{
//    #region VARIABLES

//    // Components on self
//    private Rigidbody _rb;
//    private PlayerStats _playerStats;

//    // External/transforms to track
//    private Transform _mainCameraTransform;
//    private Transform _followTransform;
//    private Transform _richBodyTransform;

//    // Player stats
//    [SerializeField] private float _acceleration;

//    // Input and movement data
//    private Vector2 _inputDirection;
//    private Vector3 _direction;

//    #endregion

//    #region METHODS

//    /// <summary>
//    /// Handles movement input and updates player state accordingly.
//    /// </summary>
//    /// <param name="context">Input callback context</param>
//    public void Walk(InputAction.CallbackContext context)
//    {
//        if (!_playerStats.usingSkill && !_playerStats.IsStunned)
//        {
//            if (context.started || context.performed)
//            {
//                _inputDirection = context.ReadValue<Vector2>();
//                _playerStats.SetState(_stateWalking);
//            }
//            else if (context.canceled)
//            {
//                _inputDirection = Vector2.zero;
//                _playerStats.SetState(_stateIdle);
//            }
//        }
//    }

//    #endregion

//    #region UNITY_LIFECYCLE

//    private void Start()
//    {
//        // Cache components
//        _rb = GetComponent<Rigidbody>();
//        _playerStats = GetComponent<PlayerStats>();

//        _mainCameraTransform = GameObject.Find("Main Camera").transform;
//        _followTransform = GameObject.Find("Follow Target").transform;
//        _richBodyTransform = transform.Find("Rich Body").transform;
//    }

//    private void FixedUpdate()
//    {
//        if (_playerStats.state == _stateWalking)
//        {
//            // Calculate direction relative to camera, ignore vertical
//            _direction = _inputDirection.x * _mainCameraTransform.right + _inputDirection.y * _mainCameraTransform.forward;
//            _direction.y = 0;
//            _direction = _direction.normalized;

//            // Rotate character to face movement direction (if not in combat mode)
//            if (!_playerStats.combatMode)
//            {
//                _richBodyTransform.rotation = Quaternion.LookRotation(_direction);
//            }

//            // Apply acceleration force on horizontal plane
//            Vector3 force = _direction * _acceleration;
//            _rb.AddForce(force.x, 0, force.z);
//        }

//        if (_playerStats.combatMode)
//        {
//            if (!_playerStats.usingSkill && !_playerStats.IsStunned)
//            {
//                Vector3 forwardDirection = new Vector3(_followTransform.forward.x, 0, _followTransform.forward.z);
//                _richBodyTransform.rotation = Quaternion.LookRotation(forwardDirection);
//            }
//        }
//    }

//    #endregion
//}
