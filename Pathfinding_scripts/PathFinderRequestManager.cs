using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Threading;

public class PathFinderRequestManager : MonoBehaviour
{
    //[SerializeField] Queue<PathRequest> pathReqQueue = new Queue<PathRequest>();  // OLD stuff, no longer needed for multithreading method.
    //PathRequest currentReq;
    //[SerializeField] bool isManagingPaths;

    Queue<PathResult> pathResQueue = new Queue<PathResult>();

    static PathFinderRequestManager instance;
    Pathfinding_a3 pathfinderLogic;

    

    private void Awake() {
        instance = this;
        pathfinderLogic = gameObject.GetComponent<Pathfinding_a3>();
        //Debug.Log(gameObject.name + " has awoken");
    }

    void Update() {
        if(pathResQueue.Count > 0) {
            int queueCount = pathResQueue.Count;
            lock (pathResQueue) {
                for (int i = 0; i < queueCount; i++) {
                    PathResult newResult = pathResQueue.Dequeue();
                    newResult.callback(newResult.path, newResult.success);
                }
            }
        }
    }


    public static void RequestPath(PathRequest pathRequest) {
        ThreadStart threadStart = delegate { instance.pathfinderLogic.GoToTile(pathRequest, instance.DoneProcessing);  };
        threadStart.Invoke();
        //Debug.Log("i Queue request count: " +instance.pathResQueue.Count);
        
    }    

    public void DoneProcessing(PathResult resultToSend) {
        lock (pathResQueue) {
            pathResQueue.Enqueue(resultToSend);
        }        
        //Debug.Log("iii Queue request count: " + instance.pathReqQueue.Count);
    }    
    
}

public struct PathRequest {
    public Vector3 pStart;
    public Vector3 pEnd;
    public Action<Vector3[], bool> callback;

    public PathRequest(Vector3 _pStart, Vector3 _pEnd, Action<Vector3[], bool> _callback) {
        pStart = _pStart;
        pEnd = _pEnd;
        callback = _callback;
    }
}

public struct PathResult {
    public Vector3[] path;
    public bool success;
    public Action<Vector3[], bool> callback;

    public PathResult(Vector3[] path, bool success, Action<Vector3[], bool> callback) {
        this.path = path;
        this.success = success;
        this.callback = callback;
    }
}
