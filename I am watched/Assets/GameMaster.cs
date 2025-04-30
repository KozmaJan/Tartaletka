using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMaster : MonoBehaviour
{
    public static GameMaster Settings;
    public SpriteRenderer Darkness;
    void Avake(){
        Settings = this;
        //in another script, you can now reference this script as GameMaster.Settings
        //e.g. GameMaster.Settings.SetDarkness(0.25f);
    }
    public void SetDarkness (float alpha, float r = 0, float g = 0, float b = 0){
        MaterialPropertyBlock mpb = new MaterialPropertyBlock();
        Darkness.GetPropertyBlock(mpb);
        mpb.SetColor("_Color", new Color(r,g,b, alpha));
        Darkness.SetPropertyBlock(mpb);
    }
}
