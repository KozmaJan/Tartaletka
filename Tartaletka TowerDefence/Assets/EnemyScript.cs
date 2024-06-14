using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    public WaypointInfo target;
    public int group = 1;
    public int currentWP = 0;
    public float moveSpeed = 20f;
    public float health = 20f;
    public Animator animator;
    public List<WaypointInfo> waypoints = new List<WaypointInfo>();
    public Vector3 Dir = new Vector3(0f,0f,0f);
    public int damage = 1;
    public Rigidbody2D ownRb;
    public float distanceTraveled;
    void Start()
    {
        ownRb = gameObject.GetComponent<Rigidbody2D>();
        GameObject[] waypointsObjects = GameObject.FindGameObjectsWithTag("waypoint");
        foreach(GameObject waypoint in waypointsObjects){
            if (waypoint.GetComponent<WaypointInfo>()){
                waypoints.Add(waypoint.GetComponent<WaypointInfo>());
            }
        }
        NextWaypoint();
    }
    // Update is called once per frame
    void Update()
    {
        if (target != null){
        Dir = target.position.position - this.gameObject.transform.position;
        Dir.Normalize();
        ownRb.MovePosition(ownRb.position + new Vector2(Dir.x, Dir.y) * moveSpeed * Time.deltaTime);
        distanceTraveled += (new Vector2(Dir.x, Dir.y) * moveSpeed * Time.deltaTime).magnitude;
        if((Vector3.Distance(target.position.position, gameObject.transform.position)<Vector3.Distance(new Vector3(0, 0, 0), Dir*moveSpeed *Time.deltaTime))&& target.order == currentWP){
            currentWP += 1;
            NextWaypoint();
        }
        }
    }
    void NextWaypoint(){
        bool selected = false;
        foreach(WaypointInfo waypoint in waypoints){
            if(waypoint.group == group && waypoint.order == currentWP){
                target = waypoint;
                selected = true;
            }
        }
        if (!selected){
            target = null;
        }
    }
    public void TakeHit(float damage){
        health -= damage;
        if(health <= 0){
            Destroy(gameObject);
        }
    }
}
