using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarScript : MonoBehaviour
{
    public TileInfo target;
    public int type; //police, firefighter, ambulance
    public float moveSpeed = 20f;
    public float speed = 1; //modifier multiplied by movespeed (depending on the road type)
    public Animator animator;
    public List<TileInfo> waypoints = new List<TileInfo>();
    public Vector3 Dir = new Vector3(0f,0f,0f);
    public Rigidbody2D ownRb;
    public float distanceTraveled;
    public float fuel;
    private GameMaster gameMaster;
    void Start()
    {
        gameMaster = GameObject.Find("GameManager").GetComponent<GameMaster>();
        ownRb = gameObject.GetComponent<Rigidbody2D>();
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
        ownRb.MovePosition(ownRb.position + new Vector2(Dir.x, Dir.y) * moveSpeed * Time.deltaTime *(1f-slow));
        distanceTraveled += (new Vector2(Dir.x, Dir.y) * moveSpeed * Time.deltaTime * speed).magnitude;
        if((Vector3.Distance(target.position.position, gameObject.transform.position)<Vector3.Distance(new Vector3(0, 0, 0), Dir*moveSpeed *Time.deltaTime))){
            NextWaypoint();
        }
        transform.rotation = Quaternion.Euler(0, (Dir.x > 0) ? 0 : 180, 0);
        }
    }
    void NextWaypoint(){
        bool selected = false;
        foreach(TileInfo waypoint in waypoints){
                target = waypoint;
                selected = true;
            }
        if (!selected){
            target = null;
        }
    }
    void NextWaypointAuto(){ //automatically follow the direction of the road it's on, choses randomly at interjections

    }
    public void Dock(){ //docks at the police staion/hospital, whatever
            Destroy(gameObject);
    }
}