using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Finnish : MonoBehaviour
{  
    public float range = 2.5f;
     private static RaycastHit2D enemy;
     private Vector3 position;
     private LayerMask enemyLayer;
    private GameMaster gameMaster;
    // Start is called before the first frame update
    void Start()
    {  
        gameMaster = GameObject.Find("GameManager").GetComponent<GameMaster>();
        enemyLayer = LayerMask.GetMask("Enemy");
        position = this.gameObject.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
       enemy = Physics2D.CircleCast(position, range, new Vector2(0, 0), 0, enemyLayer);
       if (enemy){
        Debug.Log(enemy);
        if (enemy.collider.transform.gameObject.GetComponent<EnemyScript>() != null){
            gameMaster.loseLives(enemy.collider.transform.gameObject.GetComponent<EnemyScript>().damage);
            enemy.collider.transform.gameObject.GetComponent<EnemyScript>().Die();
       }
       }
    }
    void OnDrawGizmos(){
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(this.gameObject.transform.position, range);
    }
    }
