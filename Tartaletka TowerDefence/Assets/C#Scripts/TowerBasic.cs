using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;

public class TowerBasic : MonoBehaviour
{
    public GameObject projectile;
    public float range = 2.5f;
     private static RaycastHit2D[] enemies;
     private Vector3 position;
     public GameObject target = null;
     public string id = "00"; //tower id for upgrades
     private float maxDistance; 
     private LayerMask enemyLayer;
     public bool canShoot = true;
     public float cooldown= 0.75f;
    private Vector3 castDir;
    public int price = 10;
    private SpriteRenderer rangeIndicator;
    public List<string> upgrades = new List<string>();
    public List<int> prices = new List<int>();
    public GameObject upgradeUI;

    // Start is called before the first frame update
    void Awake()
    {
        rangeIndicator = gameObject.transform.Find("TowerUI").gameObject.GetComponent<SpriteRenderer>();
        rangeIndicator.gameObject.transform.localScale = new Vector3(range * 1.55f, range * 1.55f, 0f);
        rangeIndicator.color = new Color(1f, 1f, 1f, 0.25f);
    }
    void Start()
    {
        upgradeUI = gameObject.transform.Find("UpgradeUI").gameObject;
        upgradeUI.SetActive(false);
        enemyLayer = LayerMask.GetMask("Enemy");
        position = this.gameObject.transform.position;
        GetUpgrades();
        towerDeselect();
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
                child.gameObject.SetActive(true);
                child.parent = null;
            }
            }
            }
    IEnumerator fireCooldown(){
  yield return new WaitForSeconds(cooldown * (2 - Time.timeScale));
  canShoot = true;
}
void OnDrawGizmos(){
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(this.gameObject.transform.position, range);
    }
public void towerSelect(){
    rangeIndicator.gameObject.SetActive(true);
    upgradeUI.SetActive(true);
    rangeIndicator.gameObject.transform.localScale = new Vector3(range * 1.55f, range * 1.55f, 0f);
    rangeIndicator.color = new Color(1f, 1f, 1f, 0.25f);
    //Upgrade();
}
public void towerDeselect(){
    rangeIndicator.gameObject.transform.localScale = new Vector3(range * 1.55f, range * 1.55f, 0f);
    rangeIndicator.color = new Color(1f, 1f, 1f, 0f);
    upgradeUI.SetActive(false);
}
public void Upgrade(int index = 1){
    if (GameObject.Find("GameManager").GetComponent<GameMaster>().money - prices[index-1] >= 0 ){
    GameObject.Find("GameManager").GetComponent<GameMaster>().money -= prices[index-1];
    string[] upgradeData = upgrades[index-1].Split();
    id = upgradeData[0];
    range = float.Parse(upgradeData[2]);
    cooldown = float.Parse(upgradeData[3]);
    Instantiate(projectile, new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y, -100), Quaternion.Euler(0,0,Mathf.Atan2(castDir.y, castDir.x) * Mathf.Rad2Deg + 90f), this.gameObject.transform);
    Destroy(projectile);
    foreach (Transform child in gameObject.transform){
            if(child.gameObject.GetComponent<ProjectileScript>()){
                projectile = child.gameObject;
                projectile.SetActive(false);
                projectile.transform.parent = null;
            }
    }
    ProjectileScript projectileData = projectile.GetComponent<ProjectileScript>();
    projectileData.damage = System.Int32.Parse(upgradeData[4]);
    projectileData.slow = float.Parse(upgradeData[6]);
    projectileData.slowTime = float.Parse(upgradeData[7]);
    projectileData.drain = float.Parse(upgradeData[8]);
    projectileData.drainTime = float.Parse(upgradeData[9]);
    projectileData.freezeTime = float.Parse(upgradeData[11]);
    projectileData.moveSpeed = float.Parse(upgradeData[12]);
    projectileData.range = float.Parse(upgradeData[13]);
    projectileData.splash = false;
    projectileData.aoe = false;
    if(upgradeData[5] == "t"){
        projectileData.splash = true;
    }
    if(upgradeData[10] == "t"){
        projectileData.aoe = true;
    }
    GetUpgrades();
    towerDeselect();
    }
}
    public void GetUpgrades(){
        upgrades.Clear();
        prices.Clear();
        StreamReader f = new StreamReader("Assets/Resources/Upgrades.txt");
        List<string> lines = f.ReadToEnd().Split("\n").ToList();
        f.Close();
        List<string> upgradesId = new List<string>();
        foreach (string line in lines){
            if (line.StartsWith(id+":")){
                upgradesId = line.Split(" ").ToList();
                Debug.Log(upgradesId[1]);
                upgradesId.RemoveAt(0);
            }
            if (upgradesId.Contains(line.Split()[0])){
                upgrades.Add(line);
                prices.Add(System.Int32.Parse(line.Split(" ")[1]));
            }
        }
    }
}
