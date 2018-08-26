using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Threading;

public class PathThreader : MonoBehaviour {

    // Holds the results of the path requester in a queue
    //This will help prevent lag and make it easier when threading
    Queue<PathResult> results = new Queue<PathResult>();

    static PathThreader PThreader;
    GetPath pathfinding;
    
    private void Awake()
    {
        // Self reference
        PThreader = this;

        pathfinding = GetComponent<GetPath>();
    }

    public void Update()
    {
        //if the results queue is not empty..
        if (results.Count > 0)
        {
            int count = results.Count;
            lock (results)
            {
                for (int i = 0; i < count; i++)
                {
                    //... remove the next request from the queue and process it
                    PathResult result = results.Dequeue();
                    result.callback(result.path, result.success);
                }
            }
        }
    }

    //Starts the threading process (Lague, 2017)
    public static void RequestPath(PathRequest pathRequest)
    {
        ThreadStart threadStart = delegate
        {
            PThreader.pathfinding.DrawPath(pathRequest, PThreader.Finished);
        };

        threadStart.Invoke();
    }

    // lock prevents other threads from access the results queue while it is already being used
    public void Finished(PathResult result)
    {
        lock (results)
        {
            results.Enqueue(result);
        }
    }

}

//Struct for the results. 
public struct PathResult
{
    public Vector3[] path;
    public bool success;
    public Action<Vector3[], bool> callback;

    public PathResult(Vector3[] path, bool success, Action<Vector3[], bool> callback)
    {
        this.path = path;
        this.success = success;
        this.callback = callback;
    }
}
//This struct holds all the info needed to draw a path
public struct PathRequest
{
    public Vector3 start, end;
    public Action<Vector3[], bool> callback;

    public PathRequest(Vector3 _start, Vector3 _end, Action<Vector3[], bool> _callback)
    {
        start = _start;
        end = _end;
        callback = _callback;
    }
}
