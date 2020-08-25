using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    private int _damage = 1;

  

    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject != null)
        {
           Enemy enemy= other.GetComponent<Enemy>();
            enemy.TookDamage(_damage);

        }



    }

}
