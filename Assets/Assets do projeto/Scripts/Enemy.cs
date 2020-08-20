using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private Rigidbody _rb;
    private Animator _anim;
    private Transform _groundCheck;
    private bool _isGrounded;
    private bool _isDead;
    private bool _facingRight;
    private Transform _target;
    private float _walkTimer;
    private float _zForce;
    private float _currentSpeed;

    [SerializeField]
    private float _minHeight;
    [SerializeField]
    private float _maxHeight;
    [SerializeField]
    private float _maxSpeed =4;
    

    void Start()
    {

        _anim = GetComponent<Animator>();
        _rb = GetComponent<Rigidbody>();
        _groundCheck = transform.Find("GroundCheck");
        _target = FindObjectOfType<PlayerMovement>().transform;


    }

    void Update()
    {
        _isGrounded = Physics.Linecast(transform.position, _groundCheck.position, 1 << LayerMask.NameToLayer("Ground"));
        _anim.SetBool("Grounded", _isGrounded);
        facePlayer();
        _walkTimer += Time.deltaTime;

    }

    private void FixedUpdate()
    {
        EnemyMovement();
    }

    //no tutorial a checagem da facingRight era dentro do update, extrai o metodo

    void facePlayer()
    {
        _facingRight = (transform.position.x < _target.position.x) ? true : false; //eu inverti os paramentros em parenteses, so para pensar mais na logica

        if (_facingRight)
        {
            transform.eulerAngles = new Vector3(0, 180, 0);

        }
        else
        {
            transform.eulerAngles = new Vector3(0, 0, 0);

        }

    }

    void EnemyMovement()
    {

        Vector3 targetDistance = _target.position - transform.position; //jeito do tutorial
        //float distance = Vector3.Distance(_target.position, transform.position); //outro jeito de fazer
       // Debug.Log("distance é " +distance);
        
       
        float hforce = targetDistance.x / Mathf.Abs(targetDistance.x); // vai retornar um valor entre 0 e 1 que vai ser multiplicado pela
                                                                         //pela velocidade até chegar no player
        if (_walkTimer >= Random.Range(1f,2f))                             
        {
            _zForce = Random.Range(-1, 2);
            _walkTimer = 0;
            
        }
       
        if (Mathf.Abs(targetDistance.x) < 1.5f)
        {
            hforce = 0;
        }
        _rb.velocity = new Vector3(hforce * _currentSpeed, 0,  _zForce * _currentSpeed);

        _anim.SetFloat("Speed", Mathf.Abs(_currentSpeed));


        // Debug.Log("hforce é " + hforce);
        Debug.Log("timer" + _walkTimer);


        _rb.position = new Vector3(_rb.position.x, _rb.position.y,
                      Mathf.Clamp(_rb.position.z, _minHeight, _maxHeight));

    }
    void ResetSpeed()
    {
        _currentSpeed = _maxSpeed;
    }

}
