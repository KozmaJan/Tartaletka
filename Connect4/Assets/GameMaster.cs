using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMaster : MonoBehaviour
{
    public transform board = gaet
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    // transform Board will have children game objects that will be collumns which will have their own gamobject that are the individual tiles
    //transform je defakto objekt, který má v sobě variably o sví pozici a rotaci, a pak si ukládá svoje děti
    //idk, I will just do a raycast and then I will check
    public bool Check(transform board, Vector2D pos, int player,int toWin = 5){
        return CheckRow(board, pos, player, toWin) || CheckCol(board, pos, player, toWin) || CheckDiag(board, pos, player, toWin){

        }
    }
    public bool CheckRow(transform board, Vector2D pos, int player,int toWin = 5){

    }
    public bool CheckCol(transform board, Vector2D pos, int player,int toWin = 5){
        
    }
    public bool CheckDiag(transform board, Vector2D pos, int player,int toWin = 5){

    }
}
