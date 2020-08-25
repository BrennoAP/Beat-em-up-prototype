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
    private int _currentHealth;
    private float _damageTimer;
    private float _damageTime =1.5f;
    private bool _isDamaged;
    private float _attackRate = 1.5f;
    private float _nextAttack ;
    
    


    [SerializeField]
    private int _maxHealth =4;
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
        _currentHealth = _maxHealth;


    }

    void Update()
    {
        _isGrounded = Physics.Linecast(transform.position, _groundCheck.position, 1 << LayerMask.NameToLayer("Ground"));
        _anim.SetBool("Grounded", _isGrounded);
        _anim.SetBool("Dead",_isDead);
        facePlayer();
        _walkTimer += Time.deltaTime;
        Attack();
        

    }

    private void FixedUpdate()
    {
        if (!_isDamaged)
        {
            EnemyMovement();
        }
            
           HitStop();
        

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

        Vector3 targetDistance = _target.position - transform.position; //forma do tutorial
        //float distance = Vector3.Distance(_target.position, transform.position); //outro forma de se fazer
       
        
       
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




        
            _rb.position = new Vector3(_rb.position.x, _rb.position.y,
                      Mathf.Clamp(_rb.position.z, _minHeight, _maxHeight));
        
            
        
        

    }


    public void TookDamage(int damage)                         //mais uma vez extraí o metodo, estava embolado no update no tutorial
    {
        if (!_isDead)
        {
            _isDamaged = true;
            _currentHealth -= damage;
            _anim.SetTrigger("HitDamage");
            if (_currentHealth <= 0)
            {
                _isDead = true;
                _rb.AddRelativeForce(new Vector3(3, 4, 0), ForceMode.Impulse);
                
            }

        }
        

    }

    //no tutorial o hitStop foi implementado dentro do update, ficou super spaghetti e eu preferi extrair o metodo 
    void HitStop() 

    {
        
        if (_isDamaged && !_isDead)    //a cada frame verifica se tomou dano e se o objeto ainda esta ativo
        {
            _currentSpeed = 0;
            _damageTimer = Time.deltaTime;   //ao tomar dano é iniciado um contador usando a média de tempo de cada frame
            if (_damageTime >= _damageTimer) // se o contador alcançar um valor maior que o tempo definido para o "congelamento" ao tomar dano (hitstop)
            {
                _isDamaged = false;       // a variavel _isDamaged volta a ser falso e o enemy pode se movimentar novamente
                _damageTimer = 0;         //reseta o timer, assim caso leve dano novamente a contagem mantém a consistência
            }
        }
    }


    //criei um metodo especifico para ataque, interresante seria definir um "attackrange" para não hardcodar direto na checagem
    //daí fica mais modular e pode ser usado caso o inimigo tenha mais de um tipo de ataque, por enquanto fica como no tutorial
    void Attack()     
    {
        float xrange = Mathf.Abs(transform.position.x - _target.transform.position.x);     //usar 2d em projeto 3d aparentemente
        float zrange = Mathf.Abs(transform.position.z - _target.transform.position.z);   //atrapalha usar Vector3.distance() ou magnitude

        Debug.Log(xrange);

        if (xrange < 1.5f && zrange <0.2f && _nextAttack < Time.time)
        {
            _anim.SetTrigger("Attack");
            _currentSpeed = 0;
            _nextAttack = Time.time + _attackRate;
        }


    }





    void DisableEnemy()
    {
        gameObject.SetActive(false);


    }

    void ResetSpeed()
    {
        _currentSpeed = _maxSpeed;
    }

}
