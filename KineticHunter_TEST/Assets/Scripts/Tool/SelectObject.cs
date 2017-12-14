using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;

public class SelectObject : ScriptableWizard 
{

    public string searchTag = "Your tag here";
    public myComponent searchByComponent = myComponent.AlreadyMove;

    public enum myComponent { AlreadyMove, none  };

    [MenuItem ("My Tools/Select All objects...")]
    static void SelectAllOfTagWizard()
    {
        ScriptableWizard.DisplayWizard<SelectObject> ("Select All objects...", "Make Selection");
    }

    void OnWizardCreate()
    {
        GameObject[] gameObjectsTag = GameObject.FindGameObjectsWithTag (searchTag);
        List<GameObject> objectName = new List<GameObject>();

        switch (searchByComponent){

            case (myComponent.AlreadyMove) :
        //objectName = Object.fi ( BlockAlreadyMoving);
        break;

        }


        Selection.objects = gameObjectsTag;
    }
}