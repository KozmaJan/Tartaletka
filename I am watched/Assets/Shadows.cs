using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shadows : MonoBehaviour
{
    [HideInInspector] public float centerX,  centerY, left, top, right, bottom;
    BoxCollider2D boxCollider2D;
    // Start is called before the first frame update
    void Start()
    {
        boxCollider2D = this.gameObject.GetComponent<BoxCollider2D>();
        centerX = transform.position.y;
        centerY = transform.position.x;
        left = transform.position.x - boxCollider2D.bounds.extents.x;
        top = transform.position.y + boxCollider2D.bounds.extents.y;
        right = transform.position.x + boxCollider2D.bounds.extents.x;
        bottom = transform.position.y - boxCollider2D.bounds.extents.y;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
