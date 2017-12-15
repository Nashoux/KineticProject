using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(BlockAlreadyMoving))]
class Tool : Editor
{

    BlockAlreadyMoving myBlock;
   

   void OnEnable() {
       myBlock = (BlockAlreadyMoving)target;
   }
    
    void OnSceneGUI()
    {
        if (myBlock.specificPos1 == new Vector3(0,0,0))
            return;
       
           

                Handles.Label(myBlock.transform.position + new Vector3(0,50,0), "start = "+ myBlock.transform.position +  "\n end = " + myBlock.specificPos2 + "\n speed = " + myBlock.speed);
                myBlock.specificPos1 = Handles.FreeMoveHandle( myBlock.specificPos1, Quaternion.identity, 5, new Vector3(5,5,5),Handles.RectangleHandleCap );
                myBlock.specificPos2 = Handles.FreeMoveHandle( myBlock.specificPos2, Quaternion.identity, 5, new Vector3(5,5,5),Handles.RectangleHandleCap );
                myBlock.speed = Handles.ScaleValueHandle(myBlock.speed, myBlock.transform.position, Quaternion.identity,myBlock.speed*10, Handles.ArrowCap,1);
              
           
        }
}