using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour {
    public float JumpForce = 300;
    public float MaxVelocity = 10;
    public float Proportional = 1000;
    public float Integral;
    public float Derivative = 10;
    public float RightPoint = 1;
    public float LeftPoint = -1;
    public float NeutralPoint = 0;

    private GameObject _child;
    private Rigidbody _rb;
    private bool _canJump;
    private Animator _anim;
    private readonly int _jumpHash = Animator.StringToHash("Jump");
    private readonly int _bendHash = Animator.StringToHash("Bend");
    private Vector3 _previousError = Vector3.zero;
    private bool _musicOn;

    // Use this for initialization
    void Start() {
        _rb = GetComponent<Rigidbody>();
        _anim = GetComponent<Animator>();
    }

    void Update() {
        if (!_musicOn & transform.position.z >= 0) {
            GetComponent<AudioSource>().Play();
            _musicOn = true;
        }
    }

    // Update is called once per frame
    void FixedUpdate() {
        float horizontal = Input.GetAxis("Horizontal");
        float targetX;
        Vector3 error;
        float delta = Time.deltaTime;

        _rb.AddForce(0, 0, 1000 * delta);
        _rb.velocity = Vector3.ClampMagnitude(_rb.velocity, MaxVelocity);

        if (_canJump & Input.GetKey(KeyCode.Space)) {
            Jump();
            _canJump = false;
        }

        if (horizontal > 1.0 / 3) {
            targetX = RightPoint;
        }
        else if (horizontal < -1.0 / 3) {
            targetX = LeftPoint;
        }
        else {
            targetX = NeutralPoint;
        }

        error = new Vector3(targetX - transform.position.x, 0, 0);

        _rb.AddForce(PID(error, _previousError, delta, Proportional, Integral, Derivative) * delta);

        _previousError = error;

        _anim.SetBool(_bendHash, Input.GetKey(KeyCode.S));
    }

    void Jump() {
        _rb.AddForce(0, JumpForce, 0);
        _anim.SetTrigger(_jumpHash);
    }

    void OnCollisionEnter(Collision other) {
        if (other.gameObject.CompareTag("Platform")) {
            _canJump = true;
        }
    }

    void OnCollisionExit(Collision other) {
        if (other.gameObject.CompareTag("Platform")) {
            _canJump = false;
        }
    }

    Vector3 PID(Vector3 error, Vector3 previousError, float delta, float p, float i, float d) {
        return (error * p + (error - previousError) / delta * d);
    }
}