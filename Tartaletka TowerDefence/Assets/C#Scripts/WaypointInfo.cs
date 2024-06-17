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
   }
}
