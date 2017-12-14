﻿using UnityEngine;
using System.Collections;
using UnityEditor;

public class SpawnerPlateforme : ScriptableWizard 
{

    public Mesh plateformeMesh;
    public Material myMat;
    public string objectName = "Plateform";

	public Vector3 specPos1;
	public Vector3 specPos2;
	public float speed = 5;

	public string parentObject = "Islands";

	public Vector3 baseRotation;

    [MenuItem ("My Tools/CreatePlateform")]
    static void CreateWizard()
    {
        ScriptableWizard.DisplayWizard<SpawnerPlateforme> ("Create Plateform", "Create new", "Update selected");
    }

    void OnWizardCreate()
    {
        GameObject myPlateform = new GameObject ();
		myPlateform.name = objectName;		
		myPlateform.transform.localRotation = Quaternion.EulerAngles(baseRotation);
		myPlateform.transform.position = specPos1;

        BlockAlreadyMoving plateformComponent = myPlateform.AddComponent<BlockAlreadyMoving> ();
		plateformComponent.specificPos1 = specPos1;
		plateformComponent.specificPos2 = specPos2;
		plateformComponent.speed = speed;

		myPlateform.AddComponent<MeshRenderer>().material = myMat;
		myPlateform.AddComponent<MeshFilter>().mesh = plateformeMesh;

		myPlateform.AddComponent<MeshCollider>().convex = true;

		
       
    }

    void OnWizardOtherButton()
    {
        if (Selection.activeTransform != null)
        {
            BlockAlreadyMoving plateformComponent = Selection.activeTransform.GetComponent<BlockAlreadyMoving>();

            if (plateformComponent != null)
            {
                
            }
        }
    }

   // void OnWizardUpdate()
    //{
      //  helpString = "Enter character details";
   // }

}