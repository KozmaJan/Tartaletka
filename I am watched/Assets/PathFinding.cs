using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Milestone{
    public float G, H, F;
    public Vector3 position;
    public Milestone parent;
    public Vector3 target;
    
    public void PathFinder(float g, float h, Vector3 self_position, Milestone self_parent, Vector3 self_target){
        G = g; //cost to get somwhere
        H = h; //distance from target
        F = g + h;
        position = self_position;
        parent = self_parent;
    }
}
public class PathFinding : MonoBehaviour
{
    public float step;
    public List<Milestone> Milestone = new List<Milestone>();
    
    void FindNextStep(){
        
    }
}
