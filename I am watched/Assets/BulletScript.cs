using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    public float damage = 20;
    public float stunTime = 2;
    public float speed = 50;
    public float maxDistance = 50;
    public Vector3 destination; 
    public Vector3 direction;
    public Vector3 originPosition;
    public Rigidbody2D ownRb;
    // Start is called before the first frame update
    void Start()
    {
        ownRb = this.gameObject.GetComponent<Rigidbody2D>();
        originPosition = this.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
            ownRb.MovePosition(ownRb.position + new Vector2(transform.right.x, transform.right.y) * speed * Time.deltaTime);
    }
    void FixedUpdate(){
        if (Vector3.Distance(ownRb.position, originPosition) > maxDistance && maxDistance != 0) {
            Destroy(gameObject);
        }
    }
    void OnCollisionEnter2D(Collision2D other){
        //if(other.gameObject.layer == targetLM){
        if( other.gameObject.GetComponent<EnemyScript>() != null){
            other.gameObject.GetComponent<EnemyScript>().isDamaged(damage);
        }
       // }
        Destroy(gameObject);
    }
}
