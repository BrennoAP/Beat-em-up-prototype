using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    private int _damage = 1;

  

    
    private void OnTriggerEnter(Collider other)
    {
    

        if (other.gameObject != null && other.CompareTag("Enemy"))
        {
            Enemy enemy = other.GetComponent<Enemy>();   
            enemy.TookDamage(_damage);

        }

        if (other.gameObject != null && other.CompareTag("Player"))
        {
            PlayerScript player = other.GetComponent<PlayerScript>();

            player.TookDamage(_damage);

        }

        //não sei se é melhor cachear os componentes e ja alocar memoria mesmo não usando,
        //ou fazer chamadas de busca do componente so quando preciso...

    }

}
