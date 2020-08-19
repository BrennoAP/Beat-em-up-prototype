using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    #region Variaveis da classe


    private Rigidbody _rb;
    private Animator _anim;
    private Transform _groundCheck;
    private bool _isGrounded;
    private bool _isDead;
    private SpriteRenderer _sprite;
    private bool _facingRigth = true;
    private bool _canJump;
    private float _jumpForce = 5;
    private float _MovHorizontal;
    private float _MovVertical;
    private bool _JumpPress;
    private float _currentVelocity;

    [SerializeField]
    private float _currentSpeed;
    [SerializeField]               // current e max speed vao auxiliar a parar o personagem ao atacar
    private float _maxSpeed = 4;   // e depois do ataque voltar a poder se mover
    [SerializeField]
    private float _minHeight =1;
    [SerializeField]
    private float _maxHeight;


    #endregion




    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _anim = GetComponent<Animator>();
        _groundCheck = gameObject.transform.Find("GroundCheck");
        _currentSpeed = _maxSpeed;
        _sprite = GetComponent<SpriteRenderer>();
    }

    //Metodos do monobehaviour:
    void Update()
    {
        SetAnimsState();
        _isGrounded = Physics.Linecast(transform.position, _groundCheck.position, 1 << LayerMask.NameToLayer("Ground"));
        _canJump = _isGrounded ? true : false;
        GetInputs(); //optei por pegar os inputs no update para ser mais responsivel, porem a fisica e tratada no fixed update
        JumpAction(); //jump é muito irresponsivel no fixedupdate, ainda precisa ser ajustado de toda forma
        
        

        
    }

     void FixedUpdate()
    {
        Movement();
        MoveClamps();


    }

    //Metodos da classe:

    void GetInputs()
    {
        _MovHorizontal = Input.GetAxis("Horizontal");
        _MovVertical = Input.GetAxis("Vertical");
        _JumpPress = Input.GetButtonDown("Jump");


    }


    void Movement()
    {
        if (!_isDead)
        {
            

            if (_isGrounded)
            {


                
                _rb.velocity = new Vector3(_MovHorizontal * _currentSpeed, _rb.velocity.y, _MovVertical* _currentSpeed);
                _currentVelocity = _rb.velocity.magnitude;
                

                //o tutorial origial implementava a mudança do eixo x 
                //com um metodo para alterar o player scale diretamente 
                //usei o metodo do spriterender da unity por achar mais simples 
                //não extrai o metodo pois não achei necessario

                if (_MovHorizontal < 0 && _facingRigth)
                {
                    _sprite.flipX = true;
                    _facingRigth = false;
                }
                else if(_MovHorizontal > 0 && !_facingRigth)
                {
                    _sprite.flipX = false;
                    _facingRigth = true;
                }

               
            }
           
        }
    }
    void JumpAction() 
    { 
            if (_canJump)
            {
                if (_JumpPress)
                {
                    
                    _rb.velocity = (Vector3.up* _jumpForce);
                    
                }
            }
    }

    /**O correto seria criar um scrip especifico para lidar com as animações
    porem vou seguir os scripts do tutorial, comentando para caso eu vá refatorar no futuro
    **/

    void SetAnimsState()
    {
        _anim.SetBool("OnGround", _isGrounded);
        _anim.SetBool("Dead", _isDead);

        if (Input.GetButton("Fire1"))
        {
            _anim.SetTrigger("Attack");

        }

        if (_isGrounded)
        {
            _anim.SetFloat("Speed", Mathf.Abs(_currentVelocity));

        }

    }

    //metodo para limitar o movimento na tela, no tutorial estava tudo 
    //contido no fixedupdate, basicamente extrai o metodo para ficar mais legivel e tal

    void MoveClamps()
    {
        //meio tricky mas a ideia e obter os valores do eixo x da tela baseado na posicao da camera e usar eles para 
        //limitar a posicao do rigidibody no mundo de jogo, oh mais pq não o Gameobject? porque a area usa colliders 
        //para limitar o movimento da camera,porem concerteza tem outros meios de implementar isso 
        //clamp do z vai ser setado manualmente

        float minWidth = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 10)).x;
        float maxWidth = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, 0, 10)).x;

        _rb.position = new Vector3(Mathf.Clamp(_rb.position.x, minWidth +1, maxWidth -1), _rb.position.y,
                       Mathf.Clamp(_rb.position.z, _minHeight,_maxHeight )); // o +1 e -1 e para não clipar fora da tela

    }

    //metodo para parar o personagem quando atacar, chamado no animator em attack

    void ZeroSpeed()
    {
        
        _currentSpeed = 0f;

    }

    //metodo para voltar a mover apos ataque, chamado no animator em idle

    void ResetSpeed()
    {
        
        _currentSpeed = _maxSpeed;
    }

}
