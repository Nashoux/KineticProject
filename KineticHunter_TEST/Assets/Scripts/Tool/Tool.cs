using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(BlockAlreadyMoving))]
class Tool : Editor
{

    
    void Start()
    {
        
    }
    
    void OnSceneGUI()
    {
        BlockAlreadyMoving connectedObjects = target as BlockAlreadyMoving;
        if (connectedObjects.specificPos1 == new Vector3(0,0,0))
            return;
       
           
                Handles.DrawLine(connectedObjects.specificPos1, connectedObjects.specificPos2);          
                Handles.CubeCap(1,connectedObjects.specificPos1,Quaternion.identity,2);
                Handles.CubeCap(1,connectedObjects.specificPos2,Quaternion.identity,2);
           
        }
}