using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealthController : MonoBehaviour
{
    public int enemyMaxHealth = 1;
    public int enemyCurrentHealth;

    public int enemyDamage = 1;

    void Start()
    {
        enemyCurrentHealth = enemyMaxHealth;
    }

    // Update is called once per frame
    void Update()
    {
            
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            PlayerHealthController.Instance.TakeDamage(enemyDamage);
            Debug.Log("Player Hit");
        }
        else if(other.transform.parent!=null && other.transform.parent.gameObject.GetComponent<Arrow>()!=null)
        {
            enemyCurrentHealth -= other.transform.parent.gameObject.GetComponent<Arrow>().arrowDamage;
            Destroy(other.transform.parent.gameObject);
            if(enemyCurrentHealth<=0)
            {
                Destroy(gameObject);
            }
        }
    }
}
