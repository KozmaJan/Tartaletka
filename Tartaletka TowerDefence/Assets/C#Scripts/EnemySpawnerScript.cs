using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnerScript : MonoBehaviour
{
    public int index = 1;
    public Vector3 position= new Vector3(0, 0,0);
    public List<GameObject> enemies = new List<GameObject>(); //list jmen nepřátel nepřátel, které čekají na spawnutí
    public List<float> spawnTime = new List<float>(); //jaká doba musí uplynout v sekundách před jejich spawnutím
    public List<int> count = new List<int>();
    // Start is called before the first frame update
    void Start()
    {
        position = gameObject.transform.position;
    }
    public void StartSummoning(){
       if (enemies.Count != 0){
        count[0] -= 1; 
        StartCoroutine("SummonEnemy");
       }
    }
    IEnumerator SummonEnemy()
    {
        yield return new WaitForSeconds(spawnTime[0]);
        Instantiate(enemies[0], position, Quaternion.Euler(0, 0, 0), gameObject.transform);
        foreach(Transform child in gameObject.transform){
            child.gameObject.GetComponent<EnemyScript>().group = index;
            child.parent = null;
        }
        if(count[0] <= 0){
            enemies.RemoveAt(0);
            count.RemoveAt(0);
            spawnTime.RemoveAt(0);
        }
        StartSummoning();
    }
}
