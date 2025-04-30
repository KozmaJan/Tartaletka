using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;


public class CameraScript : MonoBehaviour
{
    [SerializeField] Material GLDraw;
    GameObject Player;
    float camHeight, camWidth;
    Camera cam;
    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        cam = Camera.main;
        camHeight = 2 *cam.orthographicSize;
        camWidth = camHeight * cam.aspect;
        RenderPipelineManager.endCameraRendering += OnEndCameraRendering;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 pos = transform.position;
        pos.x = Player.transform.position.x;
        pos.y = Player.transform.position.y;
        transform.position = pos;
    }
    private void OnEndCameraRendering(ScriptableRenderContext context, Camera camera)
    {
        float cameraLeft = transform.position.x - camWidth/2;
        float cameraBottom = transform.position.y - camHeight/2;    

        GL.PushMatrix();
        GLDraw.SetPass(0);
        GL.LoadOrtho();

        Collider2D[] collisions = Physics2D.OverlapBoxAll(transform.position, new Vector2(camWidth, camHeight), LayerMask.GetMask("Wall"));
        foreach(Collider2D col in collisions){
            if (col.gameObject.GetComponent<Shadows>() == null){
                continue;
            }
            Shadows shadows = col.gameObject.GetComponent<Shadows>();
            float left = (shadows.left  -cameraLeft)/camWidth;
            float top = (shadows.top - cameraBottom)/camHeight;
            float right = (shadows.right-cameraLeft)/camWidth;
            float bottom = (shadows.bottom-cameraBottom)/camHeight;

            if (Player.transform.position.x <= shadows.centerX && Player.transform.position.y <= shadows.centerY ){
                DrawShadows(left, bottom, right, top);
            }
             if (Player.transform.position.x <= shadows.centerX && Player.transform.position.y >= shadows.centerY ){
                DrawShadows(left, top, right, bottom);
            }
             if (Player.transform.position.x >= shadows.centerX && Player.transform.position.y >= shadows.centerY ){
                DrawShadows(right, top, left, bottom);
            }
             if (Player.transform.position.x >= shadows.centerX && Player.transform.position.y <= shadows.centerY ){
                DrawShadows(right, bottom, left, top);
            }
        }
        GL.PopMatrix();
    }
    void DrawShadows(float x1, float y1, float x2, float y2){
        float x = 0.5f, y = 0.5f;
        int projectedLenght = 100;
        float projx1 = x2+ (x2-x) * projectedLenght;
        float projy1 = y1+ (y1-y) * projectedLenght;
        float projx2 = x1+ (x1-x) * projectedLenght;
        float projy2 = y2+ (y2-y) * projectedLenght;

        GL.Begin(GL.TRIANGLES);
        GL.Color(Color.black);

        GL.Vertex(new Vector3(x1, y1, 0));
        GL.Vertex(new Vector3(x2, y1, 0));
        GL.Vertex(new Vector3(projx1, projy1, 0));

        GL.Vertex(new Vector3(x1, y1, 0));
        GL.Vertex(new Vector3(projx2, projy1, 0));
        GL.Vertex(new Vector3(projx1, projy1, 0));

        GL.Vertex(new Vector3(x1, y1, 0));
        GL.Vertex(new Vector3(x1, y2, 0));
        GL.Vertex(new Vector3(projx2, projy2, 0));

        GL.Vertex(new Vector3(x1, y1, 0));
        GL.Vertex(new Vector3(projx2, projy1, 0));
        GL.Vertex(new Vector3(projx2, projy2, 0));

        GL.End();
    }
    public void OnDestroy()
    {
        RenderPipelineManager.endCameraRendering -= OnEndCameraRendering;
    }
}
