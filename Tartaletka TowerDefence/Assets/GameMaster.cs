using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMaster : MonoBehaviour
{ 
    public int lives = 15;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
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
    }
   public void loseLives(int lost){
    lives -= lost;
    if(lives <= 0){
        Time.timeScale = 0;
    }
   }
}
