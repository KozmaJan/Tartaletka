using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointInfo : MonoBehaviour
{
   public int order = 0;
   public int group = 1;
   public Transform position;
   void Awake(){
      position = this.gameObject.transform;
      if(System.Int32.TryParse(gameObject.name.Replace("Waypoint (","").Replace(")", ""), out int ord)){
         order = ord;
      }
      if(System.Int32.TryParse(gameObject.transform.parent.gameObject.name.Replace("Path",""), out int grp)){
         group = grp;
      }
   }
}
