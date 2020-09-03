using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UiManager : MonoBehaviour
{
    //player

    [SerializeField]
    private Slider healthUi;
    [SerializeField]
    private Image playerImage;
    [SerializeField]
    private Text playerName, livesText;
    private PlayerScript _player;

    //poderia ser atribuido as referencias por codigo pegando os components nos childs do UI, mas de fato é mais pratico pelo editor
    //e continua sendo reutilizavel, funcionando para criar prefabs

    //enemy
    [SerializeField]
    private GameObject enemyUi;
    [SerializeField]
    private Slider enemySlider;
    [SerializeField]
    private Text enemyName;
    [SerializeField]
    private Image enemyImage;

    private float _enemyUItime = 3f;
    private float _enemyUItimer;




    void Start()
    {
        _player = FindObjectOfType<PlayerScript>();
        healthUi.maxValue = _player.PlayerMaxHealth;
        healthUi.value = healthUi.maxValue;
        playerName.text = _player.playerName;
        playerImage.sprite = _player.playerImage;

    }

    void Update()
    {
        EnemyUITime();

    }

    public void UpdateHealthUi(int amount)
    {
        healthUi.value = amount;
    }

    public void UpdateEnemyUi(int maxhealth, int currenthealth, Sprite enemyimg, string enemyname)  //não gostei, prefiriria setEnemyUi, mas vou seguir como no tutorial
    {
        enemySlider.maxValue = maxhealth;
        enemySlider.value = currenthealth;
        enemyName.text = enemyname;
        enemyImage.sprite = enemyimg;
        _enemyUItimer = 0;
        enemyUi.SetActive(true);


    }

    void EnemyUITime()  //ativar e desativar o Ui do inimigo quanto o player atacar/para de atacar
    {
        _enemyUItimer += Time.deltaTime;
        if (_enemyUItimer >= _enemyUItime)
        {
            enemyUi.SetActive(false);
            _enemyUItimer = 0;
        }

    }


}
