using UnityEngine;
using System.Collections;

[AddComponentMenu("Scripts/SPGMetaStates/RoomMetaState")]
public class DungeonMetaState : MonoBehaviour
{
	public Hero Hero;
    public MetaStateManager MetaStateManager;
    public DungeonGenerator DungeonGenerator;
    public bool GoToNextFloorWhenExitIsTouched = false;

	void Start()
    {        
	}
	
	void Update()
    {
        if (MetaStateManager.GetCurrentMetaState() != MetaState.Dungeon)
        {
            return;
        }

        if (Input.GetKeyUp(KeyCode.H))
        {
            MetaStateManager.SetNewMetaState(MetaState.HomeBase);
        }

        if (Input.GetKeyUp(KeyCode.I))
        {
            MetaStateManager.SetNewMetaState(MetaState.Inventory);
        }

//        if (Input.GetKeyUp(KeyCode.Space) && !GoToNextFloorWhenExitIsTouched && DungeonGenerator.IsHeroOnStairsDown_square())
//        {
//            MetaStateManager.SetNewMetaState(MetaState.Story);
//        }

        HandleHeroMovementInput();
	    //HandleCheatInput();
    }

//    void HandleCheatInput()
//    {
//        if (Input.GetKeyUp(KeyCode.Tab))
//        {
//            DungeonGenerator.GoToNextFloor();
//        }
//    }
//	
//	bool wPressed = false;
//	bool aPressed = false;
//	bool sPressed = false;
//	bool dPressed = false;
//	
//	bool northCollision = false;
//	bool southCollision = false;
//	bool eastCollision = false;
//	bool westCollision = false;
//	
//	bool accidentalNorthCollision = false;
//	bool accidentalSouthCollision = false;
//	bool accidentalEastCollision = false;
//	bool accidentalWestCollision = false;
	
	
//	float heroSpeed = 0.06f;
//	float collisionBounds = 0.1f;
//	
    void HandleHeroMovementInput()
    {
		DungeonGenerator.SetHeroLocation();
		
//        bool moveSuccess = false;
//		
//		// North Collision
//		if (Hero.heroLocation.z - Hero.heroCellLocation.z > collisionBounds
//			&& DungeonGenerator.cellWalkableGrid[(int)Hero.heroCellLocation.x,(int)Hero.heroCellLocation.z + 1] == DungeonGenerator.CellWalkable.No)
//		{
//			northCollision = true;
//		} else 
//			northCollision = false;
//		
//		// South Collision
//		if (Hero.heroLocation.z - Hero.heroCellLocation.z < - collisionBounds
//			&& DungeonGenerator.cellWalkableGrid[(int)Hero.heroCellLocation.x,(int)Hero.heroCellLocation.z - 1] == DungeonGenerator.CellWalkable.No)
//		{
//			southCollision = true;
//		} else 
//			southCollision = false;
//		
//		// East Collision
//		if (Hero.heroLocation.x - Hero.heroCellLocation.x > collisionBounds
//			&& DungeonGenerator.cellWalkableGrid[(int)Hero.heroCellLocation.x + 1,(int)Hero.heroCellLocation.z] == DungeonGenerator.CellWalkable.No)
//		{
//			eastCollision = true;
//		} else 
//			eastCollision = false;
//		
//		// West Collision
//		if (Hero.heroLocation.x - Hero.heroCellLocation.x < - collisionBounds
//			&& DungeonGenerator.cellWalkableGrid[(int)Hero.heroCellLocation.x - 1,(int)Hero.heroCellLocation.z] == DungeonGenerator.CellWalkable.No)
//		{
//			westCollision = true;
//		} else 
//			westCollision = false;
//		
//		
//		if (wPressed) {
//			
//			DungeonGenerator.SetHeroLocation();
//			if(!northCollision) {
//				
//				Hero.transform.position = new Vector3(Hero.transform.position.x, Hero.transform.position.y, Hero.transform.position.z + heroSpeed);
//			}
//		}
//		if (aPressed) {
//			DungeonGenerator.SetHeroLocation();
//			
//			if (!westCollision) {
//			Hero.transform.position = new Vector3(Hero.transform.position.x - heroSpeed, Hero.transform.position.y, Hero.transform.position.z);
//			}
//		}
//		
//		if (sPressed) {
//			DungeonGenerator.SetHeroLocation();
//			if (!southCollision) {
//				Hero.transform.position = new Vector3(Hero.transform.position.x, Hero.transform.position.y, Hero.transform.position.z - heroSpeed);
//			
//			}
//		}
//		if (dPressed) {
//			DungeonGenerator.SetHeroLocation();
//			if (!eastCollision) {
//				Hero.transform.position = new Vector3(Hero.transform.position.x + heroSpeed, Hero.transform.position.y, Hero.transform.position.z);
//			
//			}
//		}
//		
//		
//	    if (Input.GetKeyUp (KeyCode.W)) wPressed = false;
//		else if (Input.GetKeyDown(KeyCode.W)) wPressed = true;
//		
//		if (Input.GetKeyUp (KeyCode.A)) aPressed = false;
//	    else if (Input.GetKeyDown(KeyCode.A)) aPressed = true;
//		
//		if (Input.GetKeyUp (KeyCode.S)) sPressed = false;
//	    else if (Input.GetKeyDown(KeyCode.S)) sPressed = true;
//		
//		if (Input.GetKeyUp (KeyCode.D)) dPressed = false;
//	    else if (Input.GetKeyDown(KeyCode.D)) dPressed = true;

//        if (moveSuccess && GoToNextFloorWhenExitIsTouched && DungeonGenerator.IsHeroOnStairsDown_square())
//        {
//            MetaStateManager.SetNewMetaState(MetaState.Story);
//            return;
//        }
    }

//    void OnGUI()
//    {
//        if (MetaStateManager.GetCurrentMetaState() != MetaState.Room)
//        {
//            return;
//        }
//
//        GUI.Box(new Rect(25, 25, 150, 25), "Room Floor: " + DungeonGenerator.FloorNumber);
//        GUI.Label(new Rect(25, 55, 250, 25), "WASD = move");        
//        GUI.Label(new Rect(25, 75, 250, 25), "Z = zoom in/out");
//        GUI.Label(new Rect(25, 95, 250, 25), "H = back to home base");
//        GUI.Label(new Rect(25, 115, 250, 25), "I = open inventory");
//        if (!GoToNextFloorWhenExitIsTouched)
//        {
//            GUI.Label(new Rect(25, 135, 250, 25), "Spacebar on red = next floor");
//        }        
//    }
}
