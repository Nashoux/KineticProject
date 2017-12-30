using UnityEngine;
using System.Collections;
using UnityEditor;

public class SpawnerPlateforme : ScriptableWizard 
{

    public Mesh plateformeMesh;
    public Material myMat;
    public string objectName = "Plateform";

	public BlockAlreadyMoving.mouvementType typeOfMouv = BlockAlreadyMoving.mouvementType.specific;

	public Vector3 specPos1;
	public Vector3 specPos2;
	public float speed = 5;

	public Vector3 size = new Vector3 (50,50,50);

	public string parentObject = "Islands";

	public Vector3 baseRotation;

    [MenuItem ("My Tools/CreatePlateform")]
    static void CreateWizard()
    {
        ScriptableWizard.DisplayWizard<SpawnerPlateforme> ("Create Plateform", "Create new");
    }

    void OnWizardCreate()  {
        
        //spawn my plateform and change the transform
        GameObject myPlateform = new GameObject (); 
		myPlateform.name = objectName;		
		myPlateform.transform.localRotation = Quaternion.EulerAngles(baseRotation);
		myPlateform.transform.position = specPos1;
		myPlateform.transform.localScale = size;
		if(GameObject.Find(parentObject)){
			myPlateform.transform.parent = GameObject.Find(parentObject).transform; 
		}
		

        //change the component to move the object
        BlockAlreadyMoving plateformComponent = myPlateform.AddComponent<BlockAlreadyMoving> (); 
		plateformComponent.specificPos1 = specPos1;
		plateformComponent.specificPos2 = specPos2;
		plateformComponent.speed = speed;
		plateformComponent.myNewMouv = typeOfMouv;

        //add some more graphic 
		myPlateform.AddComponent<MeshRenderer>().material = myMat;
		myPlateform.AddComponent<MeshFilter>().mesh = plateformeMesh;

		myPlateform.AddComponent<MeshCollider>().convex = true;

		
       
    }

    void OnWizardOtherButton(){ //to update a target 
        if (Selection.activeTransform != null){
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