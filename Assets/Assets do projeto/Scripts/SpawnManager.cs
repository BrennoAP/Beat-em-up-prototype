using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private float minZ, maxZ;     //faixa minima e maxima em Z onde pode ser spwanado o enemy

    [SerializeField]
    private Enemy []enemies;      //array com os prefabs dos inimigos a serem spawnados

    private int currentEnemies;    //numero de inimigos na cena
    [SerializeField]
    private int spawnTime;         //tempo entre um spwan e outro do enemy
    [SerializeField]
    private int numberOfEnemies;   //maximo de inimigos a serem spawnados



    // Update is called once per frame
    void Update()
    {
        SpawnControl();

    }

    private void OnTriggerEnter(Collider other)  
                                                 
    {
        if (other.tag == "Player")               
        {
            GetComponent<BoxCollider>().enabled = false;                        //quando o player colidir, ira desativar o collider para não spawnar inimigos novamente
            FindObjectOfType<CameraFollow>().maxXAndY.x = transform.position.x;   // ira travar a camera
            SpawnEnemy();                                                       //instanciará o inimigos

        }
    }

    void SpawnEnemy()
    {
        bool positionX = Random.Range(0, 2) == 0 ? true : false;   //sortear se vai spawnar na esquerda ou direta
        Vector3 spawnposition;                                    //inicia um vector para receber a posicao do spawn
        spawnposition.z = Random.Range(minZ, maxZ);               //define onde no eixo Z vai spawnar o inimigo
        if (positionX)
        {
            spawnposition = new Vector3(transform.position.x + 10, 0, spawnposition.z);

        }
        else
        {
            spawnposition = new Vector3(transform.position.x - 10, 0, spawnposition.z);

        }
        Instantiate(enemies[(Random.Range(0, enemies.Length))], spawnposition, Quaternion.identity);
        currentEnemies++;
        if (currentEnemies < numberOfEnemies)
        {
            Invoke("SpawnEnemy", spawnTime);
        }
    }
        void SpawnControl()                               //chamado no update, ou seja vai ser executado a cada frame
        {
            if (currentEnemies >= numberOfEnemies)       //enquanto haver inimigos ativos na cena ira realizar a verificação abaixo
            {

                int enemies = FindObjectsOfType<Enemy>().Length;                  //verifica o numero de inimigos na cena 
                if (enemies <= 0)                                                 //se não houver inimigos...
                {
                    FindObjectOfType<CameraFollow>().maxXAndY.x = 200;           //...destrava a camera...
                    gameObject.SetActive(false);                                 //...desabilita o objeto
                }

            }

        }


    

}
