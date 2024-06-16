using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileScript : MonoBehaviour
{
    public Transform target;
    public float damage = 20;
    public Rigidbody2D ownRb;
    public float moveSpeed = 5;
    private Vector3 Dir;
    private Vector3 lastPos = new Vector3(0, 0, 0);
    [Header("Splash")]
    public bool splash = false;
    public bool aoe = false; //některé věže mají kolem sebe nějaký efekt, dělají to tak že střílí splash projektil na svoji pozici který dává nepřátelům efekty
    public float range = 1;
    [Header("Effects")]
    public float slow = 0f; //O kolik se danná jednotka zpomalí
    public float slowTime = 0;
    public float drain = 0f; //Pro efekty jako je jed a krvácení
    public float drainTime = 0;
    public float freezeTime = 0f; //Pokudd se má nepřítel zpomalit
    // Start is called before the first frame update
     void Start(){
        ownRb = this.gameObject.GetComponent<Rigidbody2D>();
        lastPos = target.position;
        if (aoe){
            Detonate();
            Destroy(gameObject);
        }
     }
    // Update is called once per frame
    void Update()
    {
        if (target != null && !splash){
        lastPos = target.position;
        }
        Dir = lastPos - this.gameObject.transform.position;
        Dir.Normalize();
        ownRb.MovePosition(ownRb.position + new Vector2(Dir.x, Dir.y) * moveSpeed * Time.deltaTime);
        if(Vector3.Distance(lastPos, gameObject.transform.position)< 0.1f){
            if (target != null && !splash){
            target.gameObject.GetComponent<EnemyScript>().TakeHit(damage);
            target.gameObject.GetComponent<EnemyScript>().ApplyEffects(slow, drain, freezeTime, slowTime, drainTime);
            }
            else if (splash){
                Detonate();
            }
            Destroy(gameObject);
        }
        
       // }
    }
    public void Detonate(){
                RaycastHit2D[] hits = Physics2D.CircleCastAll(this.gameObject.transform.position, range, this.gameObject.transform.position);
                foreach (RaycastHit2D hit in hits){
                    if (hit.collider.gameObject.GetComponent<EnemyScript>()){
                        hit.collider.gameObject.GetComponent<EnemyScript>().TakeHit(damage);
                        hit.collider.gameObject.GetComponent<EnemyScript>().ApplyEffects(slow, drain, freezeTime, slowTime, drainTime);
                    }
                }
    }
}
