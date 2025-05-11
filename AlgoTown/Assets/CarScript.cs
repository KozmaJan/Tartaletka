using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CarScript : MonoBehaviour
{
    public Vector3 target;
    public int type; //police, firefighter, ambulance
    public float moveSpeed = 20f;
    public float speed = 1; //modifier multiplied by movespeed (depending on the road type)
    public Animator animator;
    //public List<Transform> waypoints = new List<Transform>();
    public Vector3 Dir = new Vector3(0f,0f,0f);
    public Vector3Int Autocheck = new Vector3Int(0,0,0);
    public Rigidbody2D ownRb;
    public float distanceTraveled;
    public float fuel;
    private GameMaster gameMaster;
    public Tilemap tilemap;
    

    void Start()
    {
        tilemap = GameObject.Find("Terrain").GetComponent<Tilemap>();
        ownRb = gameObject.GetComponent<Rigidbody2D>();
        TileBase tile = tilemap.GetTile(tilemap.WorldToCell(this.gameObject.transform.position));
        if (tile is VariableTile roadtile)
        {
           gameObject.transform.position = roadtile.leftlane + tilemap.CellToWorld(tilemap.WorldToCell(this.gameObject.transform.position));
           NextWaypointAuto();
        }
       else{
        Destroy(gameObject);
       }
        //Get the tile you are on
        //Get the information which direction you are facing and on which lane you are
        //Go to the next tile in that direction
    }
    // Update is called once per frame
    void Update()
    {
        if (target != null){
        Dir = target - this.gameObject.transform.position;
        Dir.Normalize();
        ownRb.MovePosition(ownRb.position + new Vector2(Dir.x, Dir.y) * moveSpeed * Time.deltaTime);
        distanceTraveled += (new Vector2(Dir.x, Dir.y) * moveSpeed * Time.deltaTime * speed).magnitude;
        if((Vector3.Distance(target, gameObject.transform.position)<=Vector3.Distance(new Vector3(0, 0, 0), Dir*moveSpeed *Time.deltaTime))){
            NextWaypointAuto();
        }
        transform.rotation = Quaternion.Euler(0, (Dir.x > 0) ? 0 : 180, 0);
        }
    }
    //void NextWaypoint(){
    //    bool selected = false;
    //    foreach(Transform waypoint in waypoints){
    //            target = waypoint;
    //            selected = true;
    //        }
    //    if (!selected){
    //        target = null;
    //    }
    //}
    void NextWaypointAuto(){ //automatically follow the direction of the road it's on, choses randomly at interjections
        TileBase tile = tilemap.GetTile(tilemap.WorldToCell(this.gameObject.transform.position + new Vector3(-0.5f, -0.25f, 0f)));//leftdown (-0.5, -0.25)
        Debug.Log("Next");
        if (tile is VariableTile roadtile)
        {
            target = roadtile.leftlane + tilemap.CellToWorld(tilemap.WorldToCell(this.gameObject.transform.position)+new Vector3Int(-1, 0, 0));
        }
        else{
            Destroy(gameObject);
        }
     }
    public void Dock(){ //docks at the police staion/hospital, whatever
            Destroy(gameObject);
    }
}