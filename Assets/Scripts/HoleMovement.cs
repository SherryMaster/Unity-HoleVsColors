using UnityEngine;
using System.Collections.Generic;

public class HoleMovement : MonoBehaviour
{
    [Header("Hole mesh")]
    [SerializeField] MeshFilter meshFilter;
    [SerializeField] MeshCollider meshCollider;

    [Header("Hole vertices radius")]
    [SerializeField] Vector2 moveLimitsX;
    [SerializeField] Vector2 moveLimitsY;
    //Hole vertices radius from the hole's center
    [SerializeField] float radius;
    [SerializeField] Transform holeCenter;

    [Space]
    [SerializeField] float moveSpeed;

    Mesh mesh;
    List<int> holeVertices;
    //hole vertices offsets from hole center
    List<Vector3> offsets;
    int holeVerticesCount;

    float x, y;
    Vector3 touch, targetPos;

    void Start()
    {

        Game.isHoleMoving = false;
        Game.isGameOver = false;

        //Initializing lists
        holeVertices = new List<int>();
        offsets = new List<Vector3>();

        //get the meshFilter's mesh
        mesh = meshFilter.mesh;

        //Find Hole vertices on the mesh
        FindHoleVertices();
    }


    void Update()
    {
        //Mouse
#if UNITY_EDITOR
        //isMoving=true whenever mouse is clicked 
        //isMoving=falseever mouse is released
        Game.isHoleMoving = Input.GetMouseButton(0);

        if (!Game.isGameOver && Game.isHoleMoving)
        {
            //Move hole center
            MoveHole();
            //Update hole vertices
            UpdateHoleVerticesPosition();
        }


        //Touch
#else
		//TouchPhase.Moved to prevent hole from jumping at first touch
		Game.isMoving = Input.touchCount > 0 && Input.GetTouch (0).phase == TouchPhase.Moved;

		if (!Game.isGameover && Game.isMoving) {
		//Move hole center
		MoveHole ();
		//Update hole vertices
		UpdateHoleVerticesPosition ();
		}
#endif


    }

    void MoveHole()
    {
        x = Input.GetAxis("Mouse X");
        y = Input.GetAxis("Mouse Y");

        //lerp (smooth) movement
        touch = Vector3.Lerp(
            holeCenter.position,
            holeCenter.position + new Vector3(x, y, 0f), //move hole on x & z 
            moveSpeed * Time.deltaTime
        );

        targetPos = new Vector3(
            //Clamp: to prevent hole from going outside of the ground
            Mathf.Clamp(touch.x, moveLimitsX.x, moveLimitsX.y),//limit X
            Mathf.Clamp(touch.y, moveLimitsY.x, moveLimitsY.y),
            touch.z
        );

        holeCenter.position = targetPos;
    }

    void UpdateHoleVerticesPosition()
    {
        //Move hole vertices
        Vector3[] vertices = mesh.vertices;
        for (int i = 0; i < holeVerticesCount; i++)
        {
            vertices[holeVertices[i]] = holeCenter.position + offsets[i];
        }

        //update mesh vertices
        mesh.vertices = vertices;
        //update meshFilter's mesh
        meshFilter.mesh = mesh;
        //update collider
        meshCollider.sharedMesh = mesh;
    }

    void FindHoleVertices()
    {
        for (int i = 0; i < mesh.vertices.Length; i++)
        {
            //Calculate distance between holeCenter & each Vertex
            float distance = Vector3.Distance(holeCenter.position, mesh.vertices[i]);

            if (distance < radius)
            {
                //this vertex belongs to the Hole
                holeVertices.Add(i);
                //offset: how far the Vertex from the HoleCenter
                offsets.Add(mesh.vertices[i] - holeCenter.position);
            }
        }
        //save hole vertices count
        holeVerticesCount = holeVertices.Count;
    }


    //Visualize Hole vertices Radius in the Scene view
    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(holeCenter.position, radius);

        Gizmos.color = Color.red;
        for (int i = 0; i < holeVerticesCount; i++)
        {
            Gizmos.DrawWireSphere(holeCenter.position + offsets[i], 0.1f);
        }
    }
}
