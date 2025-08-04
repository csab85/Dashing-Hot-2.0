using System.Collections;
using UnityEngine;
using UnityEngine.Apple;
using UnityEngine.Rendering;

public class CharacterPhysics : MonoBehaviour
{
    #region VARS

    //dependencies
    Rigidbody _rigidbody;
    CharacterStats _characterStats;
    StateMachine _stateMachine;

    //PLAYER dependencies
    PlayerStats _playerStats;

    //Configuration
    [Header ("Ground")]
    [SerializeField] LayerMask _groundLayerMask;
    [SerializeField] float _groundCheckDistance;

    //Runtime Data
    //push
    bool _isUnpushable;
    bool _isUsingResistance;
    float _pushForce;
    Vector3 _pushDirection;
    Coroutine _pushSelfCoroutine;
    //ground detection
    bool _isGrounded;

    #endregion

    #region METHODS

    IEnumerator PushSelfCoroutine(Vector3 direction, float force, float duration, bool useRagdoll, bool useResistance)
    {
        //start breaking stuff it touches
        _characterStats.IsBreakingObjects = true;

        //define if it's dashing or was thrown (propelled)
        _characterStats.IsPropelled = useRagdoll;
        _characterStats.IsDashing = !useRagdoll;
        _isUsingResistance = useResistance;

        //define vars
        _pushDirection = direction.normalized;
        _pushForce = force;

        //remove friction
        _rigidbody.linearDamping = 0;

        //wait for duration
        float finalDuration;

        if (_isUsingResistance)
        {
            float finalResitance = (_characterStats.Resistance + _characterStats.ResistanceCoefficient) / _characterStats.ResistanceCoefficient;
            finalDuration = duration/ finalResitance;
        }

        else
        {
            finalDuration = duration;
        }
            yield return new WaitForSeconds(finalDuration);

        StopPushing();

        //ADD IMPULSE AND RAGDOLL HERE
    }

    //called when outter scripts wanna activate push coroutine
    public void PushSelf(Vector3 direction, float force, float duration, bool useRagdoll = true, bool useResistance = true)
    {
        if (!_isUnpushable)
        {
            _pushSelfCoroutine = StartCoroutine(PushSelfCoroutine(direction, force, duration, useRagdoll, useResistance));
        }
    }

    void StopPushing(bool hitPunch = false)
    {
        StopCoroutine(_pushSelfCoroutine);
        _characterStats.IsPropelled = false;
        _characterStats.IsDashing = false;
        StartCoroutine(PushInvencibilityCoroutine());

        //call animation
        if (hitPunch && _playerStats != null)
        {
            _playerStats.CallOnHitPunch();
        }

        else
        {
            _stateMachine.ReturnToIdleState();
        }
    }

    IEnumerator PushInvencibilityCoroutine()
    {
        _isUnpushable = true;

        yield return new WaitForEndOfFrame();

        _isUnpushable = false;
    }

    #endregion

    #region EVENTS

    private void OnCollisionEnter(Collision collision)
    {
        if (_characterStats.IsPropelled || _characterStats.IsDashing)
        {
            //check if object is character
            if (collision.gameObject.TryGetComponent<CharacterStats>(out CharacterStats colliderStats))
            {
                //push other object in the same direction
                colliderStats.GetComponent<CharacterPhysics>().PushSelf(_pushDirection, _characterStats.PushForce, _characterStats.PushDuration, true);

                //stop if it's dashing (not damage propelled) and not unstoppable
                if (_characterStats.IsDashing && !_characterStats.IsUnstoppable)
                {
                    StopPushing(true);
                }
            }
        }

    }

    #endregion

    #region UNITY LIFECYCLE

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _characterStats = GetComponent<CharacterStats>();
        _stateMachine = GetComponent<StateMachine>();

        _playerStats = _characterStats as PlayerStats;
    }

    private void FixedUpdate()
    {
        #region Ground Detection

        //check if on floor
        _isGrounded = Physics.Raycast(transform.position, -transform.up, _groundCheckDistance, _groundLayerMask);

        //activate friction if on ground and not dashing/propelled
        if (!_characterStats.IsDashing && !_characterStats.IsPropelled)
        {
            _rigidbody.linearDamping = _isGrounded ? _characterStats.GroundFriction : 0;
        }

        #endregion

        #region gravity

        //apply speed if not on floor
        if (!_isGrounded)
        {
            _rigidbody.AddForce(-transform.up * _characterStats.FallAcceleration * Time.fixedDeltaTime, ForceMode.Acceleration);
        }

        #endregion

        #region Dashing or Propelled

        if (_characterStats.IsDashing || _characterStats.IsPropelled)
        {
            float finalPushForce;

            if (_isUsingResistance)
            {
                float finalResistance = (_characterStats.Resistance + _characterStats.ResistanceCoefficient) / _characterStats.ResistanceCoefficient;
                finalPushForce = _pushForce / finalResistance;
            }

            else
            {
                finalPushForce = _pushForce;
            }

            Vector3 push = Vector3.Scale(_pushDirection.normalized, new Vector3(1, 0, 1)) * finalPushForce;
            _rigidbody.linearVelocity = new Vector3(push.x, _rigidbody.linearVelocity.y, push.z);
        }

        #endregion
    }

    #endregion
}
