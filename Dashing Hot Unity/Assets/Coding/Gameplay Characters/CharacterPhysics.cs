using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.Apple;
using UnityEngine.Rendering;

public class CharacterPhysics : MonoBehaviour
{
    #region VARS

    //dependencies
    Rigidbody _rigidbody;
    Rigidbody[] _ragdollRigidbodies;
    CharacterStats _characterStats;
    StateMachine _stateMachine;
    [SerializeField] Transform _HipBoneTransform;

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

    #region Push
    IEnumerator PushSelfCoroutine(Vector3 direction, float force, float duration, bool useRagdoll, bool useResistance)
    {
        if (useRagdoll)
        {
            _characterStats.CallOnPropelled();
        }

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

        //check if needs to enable ragdoll
        if (useRagdoll)
        {
            EnableRagdoll();
        }

        StopPushing(false, useRagdoll);

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

    void StopPushing(bool hitPunch = false, bool resetRagdoll = true)
    {
        if (resetRagdoll)
        {
            _HipBoneTransform.gameObject.GetComponent<Rigidbody>().AddForce((_pushDirection + (transform.up/5)) * _pushForce * 10, ForceMode.Impulse);
            StartCoroutine(WaitAndResetRagdoll());
        }

        StopCoroutine(_pushSelfCoroutine);
        _characterStats.IsPropelled = false;
        _characterStats.IsDashing = false;
        _characterStats.IsBreakingObjects = false;
        StartCoroutine(PushInvencibilityCoroutine());

        //call result
        if (hitPunch && _playerStats != null)
        {
            _playerStats.CallOnHitPunch();
        }

        else
        {
            _stateMachine.ReturnToIdleState();
        }
    }

    IEnumerator WaitAndResetRagdoll()
    {
        yield return new WaitForSeconds(_characterStats.FallDuration);

        transform.position = _HipBoneTransform.position;
        print("bo" + gameObject);
        DisableRagdoll();
    }

    IEnumerator PushInvencibilityCoroutine()
    {
        _isUnpushable = true;

        yield return new WaitForEndOfFrame();

        _isUnpushable = false;
    }
    #endregion

    #region Ragdoll

    void EnableRagdoll()
    {
        foreach (Rigidbody rigidbody in _ragdollRigidbodies)
        {
            rigidbody.isKinematic = false;
            _characterStats.CallOnEnableRagdoll();
        }
    }

    void DisableRagdoll()
    {
        foreach (Rigidbody rigidbody in _ragdollRigidbodies)
        {
            rigidbody.isKinematic = true;
            _characterStats.CallOnDisableRagdoll();
        }
    }

    #endregion

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
                    StopPushing(true, false);
                }
            }
        }

    }

    #endregion

    #region UNITY LIFECYCLE

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _ragdollRigidbodies = GetComponentsInChildren<Rigidbody>().Where<Rigidbody>(rb => rb.gameObject != this.gameObject).ToArray(); //ignore rb if it's in a game object equal to this one
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
