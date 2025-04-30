using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    public float acc = 1;
    public Rigidbody2D rb;
    public GameObject gun;
    public Transform gunpoint;
    public GameObject bullet;
    public float MoveSpeed = 20f;
    public SpriteRenderer flashLight;

    bool key_Flashlight = false;
    // Start is called before the first frame update
    void Start()
    {
        rb = this.gameObject.GetComponent<Rigidbody2D>();
        flashLight = this.gameObject.transform.Find("Hands").Find("Flashlight").GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
         key_Flashlight = Input.GetKeyDown("f");

         Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);
            Vector3 castDir = mousePos - this.gameObject.transform.position;
            castDir.Normalize();
            castDir*=180;
            //gun.transform.rotation = Quaternion.Euler(0, 0,Mathf.Atan2(castDir.y, castDir.x) * Mathf.Rad2Deg);
            gun.transform.rotation = Quaternion.Euler((castDir.x > 0) ? 0 : 180, 0,Mathf.Atan2(castDir.y, castDir.x) * Mathf.Rad2Deg*((castDir.x > 0) ? 1 : -1));
            if (Input.GetMouseButtonDown(1)){
            Instantiate(bullet, gunpoint.position,Quaternion.Euler((castDir.x > 0) ? 0 : 180, 0,Mathf.Atan2(castDir.y, castDir.x) * Mathf.Rad2Deg*((castDir.x > 0) ? 1 : -1)));
        }
        Vector2 Dir = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        Dir.Normalize();
        rb.MovePosition(rb.position +Dir*Time.deltaTime*MoveSpeed);
        
        if (key_Flashlight){
            flashLight.enabled = !flashLight.enabled;
        }

        }

}
