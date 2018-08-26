using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GetPath : MonoBehaviour {

    PathfindingGrid grid;
      
	// Use this for initialization
	void Awake () {
        grid = GetComponent<PathfindingGrid>();
    }
	
    // Contains the lists required to determine if a node is in the open list (walkable)
    // or the closed list (unwalkable)
    //
    public void DrawPath(PathRequest request, Action<PathResult> result)
    {
        Vector3[] waypoints = new Vector3[0];
        bool success = false;

        Nodes startingNode = grid.PillPosition(request.start);
        Nodes goalNode = grid.PillPosition(request.end);

        if (startingNode.walkableAreas && goalNode.walkableAreas)
        {
            List<Nodes> openList = new List<Nodes>();
            HashSet<Nodes> closedList = new HashSet<Nodes>();

            openList.Add(startingNode);

            while (openList.Count > 0)
            {
                Nodes currentNode = openList[0];

                for (int i = 1; i < openList.Count; i++)
                {
                    if (openList[i].fCost < currentNode.fCost || openList[i].fCost == currentNode.fCost && openList[i].hCost < currentNode.hCost)
                    {
                        currentNode = openList[i];
                    }
                }

                openList.Remove(currentNode);
                closedList.Add(currentNode);

                if (currentNode == goalNode)
                {
                    success = true;
                    break;
                }

                foreach (Nodes neighbour in grid.Neighbours(currentNode))
                {
                    if (!neighbour.walkableAreas || closedList.Contains(neighbour))
                    {
                        continue;
                    }

                    int moveToNeighbour = currentNode.gCost + GetDistance(currentNode, neighbour);

                    if (moveToNeighbour < neighbour.gCost || !openList.Contains(neighbour))
                    {
                        neighbour.gCost = moveToNeighbour;
                        neighbour.hCost = GetDistance(neighbour, goalNode);
                        neighbour.parent = currentNode;

                        if (!openList.Contains(neighbour))
                        {
                            openList.Add(neighbour);
                        }
                    }

                }
            }
        }

        if (success)
        {
            waypoints = Path(startingNode, goalNode);
        }
        result(new PathResult(waypoints, success, request.callback));
    }

    // The path to be drawn
    Vector3[] Path(Nodes start, Nodes end)
    {
        List<Nodes> pathway = new List<Nodes>();
        Nodes current = end;

        while (current != start)
        {
            pathway.Add(current);
            current = current.parent;
        }
        Vector3[] waypoints = Waypoint(pathway);
        Array.Reverse(waypoints);
        return waypoints;
    }

    // the waypoints to be drawn (only visible using gizmos within pill unit)
    Vector3[] Waypoint(List<Nodes> path)
    {
        List<Vector3> waypoints = new List<Vector3>();
        Vector2 directionOld = Vector2.zero;
        //waypoints.Add(path[0].worldPositions);
        for(int i=1; i<path.Count; i++)
        {
            Vector2 dirNew = new Vector2(path[i - 1].positionx - path[i].positionx, path[i - 1].positionz - path[i].positionz);
            if(dirNew != directionOld)
            {
                waypoints.Add(path[i].worldPositions);
            }
            directionOld = dirNew;
        }

        return waypoints.ToArray();
    }

    // The distance between one node and its neighbour
    // This uses pythagoras theorem to find the hypotenus (the distance)
    int GetDistance(Nodes nodeA, Nodes nodeB)
    {
        int distanceX = Mathf.Abs(nodeA.positionx - nodeB.positionx);
        int distanceZ = Mathf.Abs(nodeA.positionz - nodeB.positionz);

        if (distanceX > distanceZ)
        {
            return 14 * distanceZ + 10 * (distanceX - distanceZ);
        }
        else
        return 14 * distanceX + 10 * (distanceZ - distanceX);

    }
}
