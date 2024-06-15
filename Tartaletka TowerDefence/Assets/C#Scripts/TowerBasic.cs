using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerBasic : MonoBehaviour
{
    public GameObject projectile;
    public float range = 2.5f;
     private static RaycastHit2D[] enemies;
     private Vector3 position;
     public GameObject target = null;
     private float maxDistance; 
     private LayerMask enemyLayer;
     public bool canShoot = true;
     public float cooldown= 0.75f;
    private Vector3 castDir;
    public int price = 10;
    private SpriteRenderer rangeIndicator;

    // Start is called before the first frame update
    void Awake()
    {
        rangeIndicator = gameObject.transform.Find("TowerUI").gameObject.GetComponent<SpriteRenderer>();
        rangeIndicator.gameObject.transform.localScale = new Vector3(range * 1.55f, range * 1.55f, 0f);
        rangeIndicator.color = new Color(1f, 1f, 1f, 0.25f);
    }
    void Start()
    {
        enemyLayer = LayerMask.GetMask("Enemy");
        position = this.gameObject.transform.position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (canShoot){
        maxDistance = 0;
       enemies = Physics2D.CircleCastAll(position, range, new Vector2(0, 0), 0, enemyLayer);
       foreach (RaycastHit2D enemy in enemies){
        if (enemy.collider.transform.gameObject.GetComponent<EnemyScript>() != null){
            if (maxDistance < enemy.collider.transform.gameObject.GetComponent<EnemyScript>().distanceTraveled){
            maxDistance = enemy.collider.transform.gameObject.GetComponent<EnemyScript>().distanceTraveled;
            target = enemy.collider.transform.gameObject;
         }
        }
       }
       if (target!= null){
        Fire();
       }
       }
    }
    public void Fire(){
            //transform.rotation = Quaternion.Euler(0, (castDir.x > 0) ? 0 : 180, 0);
            castDir = target.transform.position - this.gameObject.transform.position;
            castDir.Normalize();
            Instantiate( projectile, this.gameObject.transform.position, Quaternion.Euler(0,0,Mathf.Atan2(castDir.y, castDir.x) * Mathf.Rad2Deg + 90f), this.gameObject.transform);
            canShoot = false;
            StartCoroutine("fireCooldown");
            foreach (Transform child in gameObject.transform){
            if(child.gameObject.GetComponent<ProjectileScript>()){
                child.gameObject.GetComponent<ProjectileScript>().target = target.transform;
                child.parent = null!;
            }
            }
            }
    IEnumerator fireCooldown(){
  yield return new WaitForSeconds(cooldown);
  canShoot = true;
}
void OnDrawGizmos(){
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(this.gameObject.transform.position, range);
    }
public void towerSelect(){
    rangeIndicator.gameObject.transform.localScale = new Vector3(range * 1.55f, range * 1.55f, 0f);
    rangeIndicator.color = new Color(1f, 1f, 1f, 0.25f);
}
public void towerDeselect(){
    rangeIndicator.gameObject.transform.localScale = new Vector3(range * 1.55f, range * 1.55f, 0f);
    rangeIndicator.color = new Color(1f, 1f, 1f, 0f);
}
}
