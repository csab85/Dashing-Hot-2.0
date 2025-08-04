using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.TextCore.Text;

public class WalkingState
{
    static bool CheckIfCanWalk(CharacterStats characterStats)
    {
        return !characterStats.IsStunned;
    }

    public class Player : IState
    {
        #region FIELDS

        //Dependencies
        readonly StateMachine _stateMachine;
        readonly PlayerStats _playerStats;
        readonly Rigidbody _rigidbody;
        readonly Transform _mainCameraTransform;
        readonly Transform _cameraFollowTransform;
        readonly Transform _richBodyTransform;

        //Configurations
        float _acceleration;
        float _turnAroundSpeed;

        //Runtime data
        Vector2 _inputDirection;
        Vector3 _direction;

        #endregion

        #region METHODS

        //constructor
        public Player(GameObject player)
        {
            //dependencies
            _rigidbody = player.GetComponent<Rigidbody>();
            _playerStats = player.GetComponent<PlayerStats>();
            _mainCameraTransform = Camera.main.transform;
            _cameraFollowTransform = GameObject.Find("Camera Follow Target").GetComponent<Transform>();
            _richBodyTransform = GameObject.Find("Rich Body").GetComponent<Transform>();

            //configurations
            _acceleration = _playerStats.Acceleration;
            _turnAroundSpeed = _playerStats.TurnAroundSpeed;
        }

        public bool Enter()
        {
            if (CheckIfCanWalk(_playerStats) && !_playerStats.isUsingSkill)
            {
                _playerStats.CallOnWalk();
                return true;
            }

            else
            {
                return false;
            }
        }

        public void Update()
        {

        }

        public void FixedUpdate()
        {
            // Calculate direction relative to camera, ignore vertical
            _direction = _inputDirection.x * _mainCameraTransform.right + _inputDirection.y * _mainCameraTransform.forward;
            _direction.y = 0;
            _direction = _direction.normalized;

            // Rotate character to face movement direction (if not in combat mode)
            if (!_playerStats.isOnCombatMode)
            {
                Quaternion targetRotation = Quaternion.LookRotation(_direction);
                _richBodyTransform.rotation = Quaternion.Slerp(_richBodyTransform.rotation, targetRotation, _turnAroundSpeed * Time.fixedDeltaTime * 10);
            }

            // Apply acceleration force on horizontal plane
            Vector3 force = _direction * _acceleration;
            _rigidbody.AddForce(force.x, 0, force.z);

            if (_playerStats.isOnCombatMode)
            {
                if (!_playerStats.isUsingSkill && !_playerStats.IsStunned)
                {
                    Vector3 forwardDirection = Vector3.Scale(_cameraFollowTransform.forward, new Vector3(1, 0, 1));
                    _richBodyTransform.rotation = Quaternion.LookRotation(forwardDirection);
                }
            }
        }

        public bool Exit()
        {
            return true;
        }

        public void SetInput(Vector2 input)
        {
            _inputDirection = input;
        }

        #endregion
    }
}
