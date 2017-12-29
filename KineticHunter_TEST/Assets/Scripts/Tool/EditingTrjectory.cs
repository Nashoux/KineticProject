using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(BlockAlreadyMoving))]
class EditingTrjectory  : Editor
{

    BlockAlreadyMoving myBlock;
   

   void OnEnable() {
       myBlock = (BlockAlreadyMoving)target;
   }
    
    void OnSceneGUI()
    {
        
                Handles.Label(myBlock.transform.position + new Vector3(0,50,0), "start = "+ myBlock.transform.position +  "\n end = " + myBlock.specificPos2 + "\n speed = " + myBlock.speed); //to know the speed/position start and end
                myBlock.specificPos1 = Handles.FreeMoveHandle( myBlock.specificPos1, Quaternion.identity, 5, new Vector3(5,5,5),Handles.RectangleHandleCap ); //to move the start pos
                myBlock.specificPos2 = Handles.FreeMoveHandle( myBlock.specificPos2, Quaternion.identity, 5, new Vector3(5,5,5),Handles.RectangleHandleCap ); //to move the second pos
                myBlock.speed = Handles.ScaleValueHandle(myBlock.speed, myBlock.transform.position, Quaternion.identity,myBlock.speed*10, Handles.ArrowCap,1); //to change the speed
              
           
        }
}