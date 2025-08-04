using UnityEngine;

public class IdleState
{
    public class Player : IState
    {
        #region FIELDS

        //Dependencies
        readonly PlayerStats _playerStats;
        readonly Transform _cameraFollowTransform;
        readonly Transform _richBodyTransform;

        //Configurations

        //Runtime data

        #endregion

        #region METHODS

        //constructor
        public Player(GameObject player)
        {
            _playerStats = player.GetComponent<PlayerStats>();
            _cameraFollowTransform = GameObject.Find("Camera Follow Target").GetComponent<Transform>();
            _richBodyTransform = GameObject.Find("Rich Body").GetComponent<Transform>();
        }

        public bool Enter()
        {
            _playerStats.CallOnIdle();
            return true;
        }

        public void Update()
        {

        }

        public void FixedUpdate()
        {
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

        #endregion
    }

    public class Enemy : IState
    {
        //dependencies
        readonly EnemyStats _enemyStats;

        //constructor
        public Enemy(GameObject enemy)
        {
            _enemyStats = enemy.GetComponent<EnemyStats>();
        }

        public bool Enter()
        {
             _enemyStats.CallOnIdle();
            return true;
        }

        public void Update()
        {

        }

        public void FixedUpdate()
        {

        }

        public bool Exit()
        {
            return true;
        }
    }
}
