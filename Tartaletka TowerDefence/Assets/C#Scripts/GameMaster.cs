using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class GameMaster : MonoBehaviour
{ 
    public int lives = 15;
    public int money = 100;
    public bool placing  = false;
    public GameObject towerPlaced = null;
    public GameObject tower;
    private Vector3 mousePos;
    private bool canPlace;
    private float personalSpace;
    private RaycastHit2D obstacle;
    private int cost;
    private SpriteRenderer spriteRenderer;  
    public List<GameObject> enemies = new List<GameObject>(); //list jmen nepřátel nepřátel, které čekají na spawnutí
    public List<int> spawnPoint = new List<int>(); //Na jakém spawnpointu se mají spawnout (kdyby bylo víc spawnpointů)
    public List<float> spawnTime = new List<float>(); //jaká doba musí uplynout v sekundách před jejich spawnutím
    public List<int> count = new List<int>();
    public List<GameObject> spawners = new List<GameObject>();
    public float timeTillNextWave = 10000;
    public int wave = 0;
    private int level = 1;
    private GameObject selected;
    private Text txtLives;
    private Text txtMoney;
    private GameObject UI;
    // Start is called before the first frame update
    void Start(){
        UI = GameObject.Find("UI");
        txtLives = UI.transform.Find("Lives").GetComponent<Text>();
        txtLives.text = lives.ToString();
        txtMoney = UI.transform.Find("Money").GetComponent<Text>();
        txtMoney.text = money.ToString();
        spawners = GameObject.FindGameObjectsWithTag("Spawner").ToList(); 
        List<GameObject> tempList = GameObject.FindGameObjectsWithTag("Spawner").ToList();
        foreach(GameObject spawner in tempList){
            if (spawner.GetComponent<EnemySpawnerScript>()){
                spawners[spawner.GetComponent<EnemySpawnerScript>().index - 1] = spawner;
            }
        }
        CallNextWave();
    }
    // Update is called once per frame
    void Update()
    {
        txtMoney.text = money.ToString();
        if (wave > 0){
            if (timeTillNextWave > 0){
                timeTillNextWave -= Time.deltaTime;
            }
            else if (timeTillNextWave <= 0){
                CallNextWave();
            }
        }
        if(Input.GetKeyDown(KeyCode.Alpha1)){
            Time.timeScale = 1;
        }
        if(Input.GetKeyDown(KeyCode.Alpha2)){
            Time.timeScale = 1.5f;
        }
        if(Input.GetKeyDown(KeyCode.Alpha3)){
            Time.timeScale = 2;
        }
        if(Input.GetKeyDown(KeyCode.Alpha0)){
            if (Time.timeScale != 0){
                Time.timeScale = 0;
            }
            else{
                Time.timeScale = 1;
            }
        }
        if (Input.GetMouseButtonDown(0) && !placing) {    
			mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			RaycastHit2D hit = Physics2D.Raycast(new Vector2(mousePos.x, mousePos.y), Vector2.zero);
            if (hit.collider != null){
                selected = hit.collider.gameObject;
			if (selected.tag == "Tower") {
				selected.GetComponent<TowerBasic>().towerSelect();
			}
            else if (selected.tag == "UpgradeButton"){
                selected.transform.parent.transform.parent.GetComponent<TowerBasic>().Upgrade(System.Int32.Parse(selected.name.Replace("Upgrade", "")));
            }
            }
            else if (selected!=null){
                if (selected.tag == "Tower") {
                    selected.GetComponent<TowerBasic>().towerDeselect();
                }
                selected = null;
            } 
		}
        if(Input.GetKeyDown("p")){
            if (placing == false){
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
             Instantiate(tower, Camera.main.ScreenToWorldPoint(Input.mousePosition) , Quaternion.Euler(0, 0, 0), this.gameObject.transform);
                placing = true;
            foreach (Transform child in gameObject.transform){
            if(child.gameObject.tag == "Tower"){
                towerPlaced = child.gameObject;
                personalSpace = towerPlaced.GetComponent<CircleCollider2D>().radius;
                towerPlaced.GetComponent<CircleCollider2D>().enabled = false;
                cost = towerPlaced.GetComponent<TowerBasic>().price;
                towerPlaced.transform.parent = null;
                towerPlaced.transform.Find("TowerUI").gameObject.active = true;
                spriteRenderer = towerPlaced.transform.Find("TowerUI").gameObject.GetComponent<SpriteRenderer>();
            }
        }
    }
    }
    if (placing && towerPlaced != null){
        spriteRenderer.color = new Color(1f, 1f, 1f, 0.25f);
        canPlace = true;
        towerPlaced.transform.position = new Vector3 (mousePos.x, mousePos.y, 0);
        obstacle = Physics2D.CircleCast(towerPlaced.transform.position, personalSpace, new Vector2(0, 0), 0);
        if (obstacle.collider != null || money - cost < 0){
            canPlace = false;
            spriteRenderer.color = new Color(1f, 0f, 0f, 0.25f);
        }
       if (Input.GetMouseButtonDown(0)){
        if (canPlace == true){
            towerPlaced.GetComponent<TowerBasic>().enabled = true;
            towerPlaced.GetComponent<CircleCollider2D>().enabled = true;
            towerPlaced.GetComponent<TowerBasic>().towerDeselect();
            money -= cost;
            towerPlaced = null;
            placing = false;
            }
        }
        if(Input.GetMouseButtonDown(1) || Input.GetKeyDown(KeyCode.Escape)){
            Destroy(towerPlaced);
            placing = false;
            cost = 0;
        }
    }
    }
   public void loseLives(int lost){
    lives -= lost;
     txtLives.text = lives.ToString();
    if(lives <= 0){
        Time.timeScale = 0;
    }
   }
   public void CallNextWave(){
        wave += 1;
        bool currentWave = false;
        StreamReader f = new StreamReader("Assets/Resources/WavesInfo/Level"+level+".txt");
        Debug.Log("File read");
        List<string> lines = f.ReadToEnd().Split("\n").ToList();
        f.Close();
        foreach(string line in lines){
            if(line.StartsWith("#")){
                if(System.Int32.TryParse(line.Split()[0].Replace("#", ""), out int cWave)){
                    Debug.Log("Wave" + wave);
                    if(cWave == wave){
                        currentWave = true;
                        timeTillNextWave = float.Parse(line.Split()[1]); 
                    }
                    else{
                        currentWave = false;
                    }
                }
            }
            else if (currentWave){
                enemies.Add(Resources.Load("Enemies/" +line.Split()[0], typeof(GameObject)) as GameObject);
                count.Add(System.Int32.Parse(line.Split()[1]));
                spawnTime.Add(float.Parse(line.Split()[2]));
                spawnPoint.Add(System.Int32.Parse(line.Split()[3]));
            }
        }
        for (int i = 0; i < spawnPoint.Count; i++){
            spawners[spawnPoint[i]-1].GetComponent<EnemySpawnerScript>().enemies.Add(enemies[i]);
            spawners[spawnPoint[i]-1].GetComponent<EnemySpawnerScript>().spawnTime.Add(spawnTime[i]);
            spawners[spawnPoint[i]-1].GetComponent<EnemySpawnerScript>().count.Add(count[i]);
        }
        foreach(GameObject spawner in spawners){
            spawner.GetComponent<EnemySpawnerScript>().StartSummoning();
        }
   }
}
