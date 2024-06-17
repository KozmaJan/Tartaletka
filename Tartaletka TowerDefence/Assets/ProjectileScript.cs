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
    // Start is called before the first frame update
     void Start(){
        ownRb = this.gameObject.GetComponent<Rigidbody2D>();
     }
    // Update is called once per frame
    void Update()
    {
        if (target != null){
        lastPos = target.position;
        }
        Dir = lastPos - this.gameObject.transform.position;
        Dir.Normalize();
        ownRb.MovePosition(ownRb.position + new Vector2(Dir.x, Dir.y) * moveSpeed * Time.deltaTime);
        if(Vector3.Distance(lastPos, gameObject.transform.position)< 0.1f){
            if (target != null){
            target.gameObject.GetComponent<EnemyScript>().TakeHit(damage);
            }
            Destroy(gameObject);
        }
        
       // }
    }
}
