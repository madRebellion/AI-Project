using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathfindingGrid : MonoBehaviour {

    Nodes[,] gridNodes;

    public GameObject ground;
    public LayerMask unwalkables;
    public Vector3 worldSize; // size of the A* grid
    Vector3 firstNodeInGrid; // the top left position of the first node within the grid

    public float nodeWidth; 
    int gridLength, gridHeight;

    //public bool showPathfindingGrid;      //USE WITH THE GIZMOS   
    
	// Use this for initialization
	void Awake () {

        worldSize.x = ground.transform.lossyScale.x * 10f;
        worldSize.y = ground.transform.lossyScale.z * 10f;
        gridLength = Mathf.RoundToInt(worldSize.x / nodeWidth); // Mathf.RoundToInt() is to draw a whole number of cubes
        gridHeight = Mathf.RoundToInt(worldSize.y / nodeWidth);
        firstNodeInGrid = transform.position - Vector3.right * worldSize.x / 2 - Vector3.forward * worldSize.y / 2; // the top left left cube in the grid
        DrawPathfindingGrid();
	}

    // Dimensions for the grid
    void DrawPathfindingGrid()
    {
        gridNodes = new Nodes[gridLength, gridHeight];

        for (int x = 0; x < gridLength; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                // Places the nodes in the grid 
                Vector3 nodeWorldPositions = firstNodeInGrid + Vector3.right * (x * nodeWidth) +  Vector3.forward * (y * nodeWidth);
                bool walkable = !(Physics.CheckSphere(nodeWorldPositions, (nodeWidth / 2), unwalkables)); // checks to see if a node as collided with the unwalkable regions of our world                
                gridNodes[x, y] = new Nodes(walkable, nodeWorldPositions, x, y);
            }
        }
    }

    // Finds the nodes around the currently looked at node and see if it is walkable
    public List<Nodes> Neighbours(Nodes nodes)
    {
        List<Nodes> neighbours = new List<Nodes>();

        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y<= 1; y++)
            {
                if (x == 0 && y ==0)
                {
                    continue;
                }
                int checkX = nodes.positionx + x;
                int checkZ = nodes.positionz + y;

                if (checkX >= 0 && checkX < gridLength && checkZ >= 0 && checkZ < gridHeight)
                {
                    neighbours.Add(gridNodes[checkX, checkZ]);
                }
            }
        }

        return neighbours;
    }

    // Calculate where the current agent is on the grid and return its world position
    // Used to figure out which node the pill occupies
    public Nodes PillPosition(Vector3 pos)
    {
        float x = (pos.x + worldSize.x / 2) / worldSize.x;
        float z = (pos.z + worldSize.y / 2) / worldSize.y;
        x = Mathf.Clamp01(x);
        z = Mathf.Clamp01(z);

        int pillX = Mathf.RoundToInt((gridLength - 1) * x);
        int pillZ = Mathf.RoundToInt((gridHeight - 1) * z);

        return gridNodes[pillX,pillZ];
    }

    // ONLY USE TO HAVE A VISUAL REPRESENTATION OF THE GRID
    // COMMENT OUT WHEN ACTUALLY USING IT TO MAKE UNITY RUN MUCH MORE SMOOTHLY

    //private void OnDrawGizmos()
    //{
    //    Gizmos.DrawWireCube(transform.position, new Vector3(worldSize.x, 1, worldSize.y));

    //    if (gridNodes != null && showPathfindingGrid)
    //    {
    //        foreach (Nodes n in gridNodes)
    //        {
    //            Gizmos.color = (n.walkableAreas) ? Color.white : Color.red;
    //            Gizmos.DrawCube(n.worldPositions, Vector3.one * (nodeWidth - 0.1f));
    //        }
    //    }
    //}

}
