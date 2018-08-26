using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nodes {

    // this is each of the cubes that are in the Pathfinding grid
    public bool walkableAreas;
    public Vector3 worldPositions;
    public int positionx, positionz;

    // how far away if this node from the starting node
    public int gCost;
    // how far away is this node from the target node
    public int hCost;

    public int fCost
    {
        get
        {
            return gCost + hCost;
        }
    }


    public Nodes parent;

    public Nodes(bool walkable, Vector3 position, int x, int z)
    {
        walkableAreas = walkable;
        worldPositions = position;
        positionx = x;
        positionz = z;
    }
	
}
