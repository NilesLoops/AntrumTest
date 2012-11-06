using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DungeonGenerator : MonoBehaviour {
	
	private enum CellType {
		
		None,
		Empty,
		Blocked,
		Room,
		Perimeter,
		Entrance,
		Corridor
	}
	
	public enum CellWalkable {
		
		Yes,
		No
	}
	
	public class CellLocation {
		
        public CellLocation(int setX, int setZ) {
			
            x = setX;
            z = setZ;
        }

        public int x;
        public int z;

        public override bool Equals(object obj) {
			
            if (obj is CellLocation) {
				
                CellLocation testObj = (CellLocation)obj;
                if (testObj.x == x && testObj.z == z) {
					
                    return true;
                }
            }

            return false;
        }

        public override int GetHashCode() {
			
            return base.GetHashCode();
        }
    }
	
	public class Room {
		
		public Room(int setX, int setZ, int setWidth, int setHeight) {
			
            x = setX;
            z = setZ;
			width = setWidth;
			height = setHeight;
        }
		
		public int x;
		public int z;
		public int width;
		public int height;
	}
//	
//	public class Chest : MonoBehaviour {
//		
//		public Chest(CellLocation location) {
//			
//			Transform prefabToMake = TreasureChest;
//			float instantiateXPosition = transform.position.x + (location.x * prefabCellWidth);
//    		float instantiateZPosition = transform.position.z + (location.z * prefabCellHeight);
//			
//			transform = (Transform)Instantiate(prefabToMake, new Vector3(instantiateXPosition, 0.0f, instantiateZPosition), Quaternion.identity);
//			transform.Rotate (0, rotation, 0);	
//        }
//		
//		public bool opened = false;
//	}
	
	public Hero Hero;
	
	public Transform Cell_Empty;
	public Transform Cell_Floor;
	public Transform Cell_Wall_Corner;
	public Transform Cell_Wall;
	public Transform Cell_Arch;
	public Transform Cell_Torch;
	public Transform Cell_Door;
	
	public Transform TreasureChest;
	
	public Transform PrefabCellParent;
    public Transform OtherStuffParent;
	
	public int NumCellsAcross;
    public int NumCellsUp;
	
	public int minRoomLength = 3;
	public int maxRoomLength = 9;
	public int targetNumOfRooms = 6;
	List<Room> roomList = new List<Room> ();
	
	List<CellLocation> entranceLocationList = new List<CellLocation> ();
	List<CellLocation> corridorLocationList = new List<CellLocation> ();
	
	//List<Chest> chestList = new List<Chest> ();
	
	private float prefabCellWidth;
    private float prefabCellHeight;
	
	[HideInInspector]
    public int FloorNumber = 1;
	
	private CellType[,] cellTypeGrid;
	public CellWalkable[,] cellWalkableGrid;
	
	public bool straightCorridors = false;

    Transform GetPrefabFromCellType(CellType cellType) {
		
        switch (cellType) {
			
			case CellType.Room:
				return Cell_Floor;
			case CellType.Entrance:
				return Cell_Floor;
			case CellType.Corridor:
				return Cell_Floor;
			
			default:
				return Cell_Empty;
        }
    }
	
	void InstantiateCellsFromGrid() {
		
        prefabCellWidth = Cell_Empty.localScale.x;
        prefabCellHeight = Cell_Empty.localScale.z;

        for (int i = 0; i < NumCellsAcross; i++) {
			
            for (int j = 0; j < NumCellsUp; j++) {
				
                if (cellTypeGrid[i, j] != CellType.None) {
					
					Transform prefabToMake;
					
					//if (cellTypeGrid[i, j] == CellType.Room || cellTypeGrid[i, j] == CellType.Corridor || cellTypeGrid[i, j] == CellType.Entrance) {
						
						//CellLocation location = new CellLocation(i, j);
						//prefabToMake = GetFloorPrefabWithWalls(location);
					//} else {
						prefabToMake = GetPrefabFromCellType(cellTypeGrid[i, j]);
					//}
					
                    float instantiateXPosition = transform.position.x + (i * prefabCellWidth);
                    float instantiateZPosition = transform.position.z + (j * prefabCellHeight);
					
                    Transform createCell = (Transform)Instantiate(prefabToMake, new Vector3(instantiateXPosition, 0.0f, instantiateZPosition), Quaternion.identity);

                    createCell.parent = PrefabCellParent;
                }
            }
        }
    }
	
	void InstantiateWallsFromGrid() {
		
		prefabCellWidth = Cell_Empty.localScale.x;
        prefabCellHeight = Cell_Empty.localScale.z;

        for (int i = 1; i < NumCellsAcross - 1; i++) {
			
            for (int j = 1; j < NumCellsUp - 1; j++) {
				
				bool hasNorth = false;
				bool hasSouth = false;
				bool hasEast = false;
				bool hasWest = false;
				bool hasNorthEast = false;
				bool hasNorthWest = false;
				bool hasSouthEast = false;
				bool hasSouthWest = false;
				
				CellLocation location = new CellLocation(i, j);
					
				if (HasNorth(location)) hasNorth = true;
				if (HasSouth(location)) hasSouth = true;
				if (HasEast(location)) hasEast = true;
				if (HasWest(location)) hasWest = true;
				if (HasNorthEast(location)) hasNorthEast = true;
				if (HasNorthWest(location)) hasNorthWest = true;
				if (HasSouthEast(location)) hasSouthEast = true;
				if (HasSouthWest(location)) hasSouthWest = true;
				
				if (cellTypeGrid[i, j] == CellType.Room || cellTypeGrid[i, j] == CellType.Corridor || cellTypeGrid[i, j] == CellType.Entrance) {
					
					// Walls
					if (!hasNorth) InstantiateWall (location, 90);
					if (!hasSouth) InstantiateWall (location, 270);
					if (!hasEast) InstantiateWall (location, 180);
					if (!hasWest) InstantiateWall (location, 0);
					
					// Corners
					if (!hasNorthEast) {
						if (hasNorth && hasEast) InstantiateCorner (location, 90);
						if (!hasNorth && !hasEast) InstantiateCorner (location, 90);
						if (hasNorth && hasWest && !hasEast && !hasSouth && !hasNorthWest) InstantiateCorner (location, 90);
						if (!hasNorth && !hasWest && hasEast && hasSouth && !hasSouthEast) InstantiateCorner (location, 90);
					}
					if (!hasNorthWest) {
						if (hasNorth && hasWest) InstantiateCorner (location, 0);
						if (!hasNorth && !hasWest) InstantiateCorner (location, 0);
						if (hasNorth && hasEast && !hasWest && !hasSouth && !hasNorthEast) InstantiateCorner (location, 0);
						if (!hasNorth && !hasEast && hasWest && hasSouth && !hasSouthWest) InstantiateCorner (location, 0);
					}
					if (!hasSouthEast) {
						if (hasSouth && hasEast) InstantiateCorner (location, 180);
						if (!hasSouth && !hasEast) InstantiateCorner (location, 180);
						if (hasSouth && hasWest && !hasEast && !hasNorth && !hasSouthWest) InstantiateCorner (location, 180);
						if (!hasSouth && !hasWest && hasEast && hasNorth && !hasNorthEast) InstantiateCorner (location, 180);
					}
					if (!hasSouthWest) {
						if (hasSouth && hasWest) InstantiateCorner (location, 270);
						if (!hasSouth && !hasWest) InstantiateCorner (location, 270);
						if (hasSouth && hasEast && !hasWest && !hasNorth && !hasSouthEast) InstantiateCorner (location, 270);
						if (!hasSouth && !hasEast && hasWest && hasNorth && !hasNorthWest) InstantiateCorner (location, 270);
					}
				}
				
				// Doors
				if (cellTypeGrid[i, j] == CellType.Entrance) {
					if (hasNorth && hasSouth && !hasEast && !hasWest) InstantiatePrefab (Cell_Door, location, 90);
					if (!hasNorth && !hasSouth && hasEast && hasWest) InstantiatePrefab (Cell_Door, location, 0);
				}
				
				// Arches
				if (cellTypeGrid[i, j] == CellType.Corridor) {
					if (hasNorth && hasSouth && !hasEast && !hasWest) InstantiatePrefab (Cell_Arch, location, 90);
					if (!hasNorth && !hasSouth && hasEast && hasWest) InstantiatePrefab (Cell_Arch, location, 0);
				}
				
				if (cellTypeGrid[i, j] == CellType.Room) {
					
					// Torches
					if (i % 2 == 0 && j % 2 != 0) {
						if (!hasNorth) InstantiateTorch (location, 90);
						if (!hasSouth) InstantiateTorch (location, 270);
						if (!hasEast) InstantiateTorch (location, 180);
						if (!hasWest) InstantiateTorch (location, 0);
					}
					if (i % 2 != 0 && j % 2 == 0) {
						if (!hasNorth) InstantiateTorch (location, 90);
						if (!hasSouth) InstantiateTorch (location, 270);
						if (!hasEast) InstantiateTorch (location, 180);
						if (!hasWest) InstantiateTorch (location, 0);
					}
				}
			}
		}
	}
	
	void InstantiateWall(CellLocation location, int rotation) {
		
		Transform prefabToMake = Cell_Wall;
		float instantiateXPosition = transform.position.x + (location.x * prefabCellWidth);
    	float instantiateZPosition = transform.position.z + (location.z * prefabCellHeight);
		
    	Transform createCorner = (Transform)Instantiate(prefabToMake, new Vector3(instantiateXPosition, 0.0f, instantiateZPosition), Quaternion.identity);
		createCorner.Rotate (0, rotation, 0);
						
        createCorner.parent = OtherStuffParent;
	}
	
	void InstantiateCorner(CellLocation location, int rotation) {
		
		Transform prefabToMake = Cell_Wall_Corner;
		float instantiateXPosition = transform.position.x + (location.x * prefabCellWidth);
    	float instantiateZPosition = transform.position.z + (location.z * prefabCellHeight);
		
    	Transform createCorner = (Transform)Instantiate(prefabToMake, new Vector3(instantiateXPosition, 0.0f, instantiateZPosition), Quaternion.identity);
		createCorner.Rotate (0, rotation, 0);
						
        createCorner.parent = OtherStuffParent;
	}
	
	void InstantiateArch(CellLocation location, int rotation) {
		
		Transform prefabToMake = Cell_Arch;
		float instantiateXPosition = transform.position.x + (location.x * prefabCellWidth);
    	float instantiateZPosition = transform.position.z + (location.z * prefabCellHeight);
		
    	Transform createArch = (Transform)Instantiate(prefabToMake, new Vector3(instantiateXPosition, 0.0f, instantiateZPosition), Quaternion.identity);
		createArch.Rotate (0, rotation, 0);
						
        createArch.parent = OtherStuffParent;
	}
	
	void InstantiateTorch(CellLocation location, int rotation) {
		
		Transform prefabToMake = Cell_Torch;
		float instantiateXPosition = transform.position.x + (location.x * prefabCellWidth);
    	float instantiateZPosition = transform.position.z + (location.z * prefabCellHeight);
		
    	Transform createTorch = (Transform)Instantiate(prefabToMake, new Vector3(instantiateXPosition, 0.0f, instantiateZPosition), Quaternion.identity);
		createTorch.Rotate (0, rotation, 0);
						
        createTorch.parent = OtherStuffParent;
	}
	
	void InstantiateDoor(CellLocation location, int rotation) {
		
		Transform prefabToMake = Cell_Door;
		float instantiateXPosition = transform.position.x + (location.x * prefabCellWidth);
    	float instantiateZPosition = transform.position.z + (location.z * prefabCellHeight);
		
    	Transform createDoor = (Transform)Instantiate(prefabToMake, new Vector3(instantiateXPosition, 0.0f, instantiateZPosition), Quaternion.identity);
		createDoor.Rotate (0, rotation, 0);
						
        createDoor.parent = OtherStuffParent;
	}
	
//	void InstantiateChest(CellLocation location, int rotation) {
//		
//		//Transform prefabToMake = TreasureChest;
//		//float instantiateXPosition = transform.position.x + (location.x * prefabCellWidth);
//    	//float instantiateZPosition = transform.position.z + (location.z * prefabCellHeight);
//		
//    	//Transform createChest = (Transform)Instantiate(prefabToMake, new Vector3(instantiateXPosition, 0.0f, instantiateZPosition), Quaternion.identity);
//		//createChest.Rotate (0, rotation, 0);
//		
//		Chest chest = new Chest(location);
//		
//		//chest.transform = (Transform)Instantiate(prefabToMake, new Vector3(instantiateXPosition, 0.0f, instantiateZPosition), Quaternion.identity);
//		//chest.transform.Rotate (0, rotation, 0);	
//		
//        chest.parent = OtherStuffParent;
//		
//		chestList.Add (chest);
//		print (chestList.Count);
//		
//		//chest.parent = OtherStuffParent;
//	}
	
	void InstantiatePrefab(Transform prefabToMake, CellLocation location, int rotation) {
		
		float instantiateXPosition = transform.position.x + (location.x * prefabCellWidth);
    	float instantiateZPosition = transform.position.z + (location.z * prefabCellHeight);
		
    	Transform createDoor = (Transform)Instantiate(prefabToMake, new Vector3(instantiateXPosition, 0.0f, instantiateZPosition), Quaternion.identity);
		createDoor.Rotate (0, rotation, 0);
						
        createDoor.parent = OtherStuffParent;
	}
	
	void FillCellGrid() {
		
        for (int i = 0; i < NumCellsAcross; i++) {
			
            for (int j = 0; j < NumCellsUp; j++) {
				
                cellTypeGrid[i, j] = CellType.Empty;
				cellWalkableGrid[i, j] = CellWalkable.No;
            }
        }
        
		BlockEdges();
		CreateRooms ();
		BlockEdges ();
		
		foreach (Room room in roomList) {
			CreateEntrances(room);
		}
		
		
		CellLocation testLocation = new CellLocation(1, 1);
		bool started = false;
		
		while (!started) {
		
			if (cellTypeGrid[testLocation.x, testLocation.z] == CellType.Empty) {
				CreateCorridor (testLocation.x, testLocation.z);
				started = true;
			}
			else {
				testLocation.x += 2;
				testLocation.z += 2;
			}
		}
		
		for (int i = 1; i < NumCellsAcross - 2; i+= 2) {
			
            for (int j = 1; j < NumCellsUp - 2; j+= 2) {
				
				if (cellTypeGrid[i, j] == CellType.Empty) {
					
					CreateCorridor (i, j);
				}
			}
		}
		CleanDeadEnds();
		
		MakeCellsWalkable();
    }
	
	void BlockEdges() {
		
		for (var i = 0; i < NumCellsAcross; i++) {
			cellTypeGrid[i, 0] = CellType.Blocked;
			cellTypeGrid[i, NumCellsUp - 1] = CellType.Blocked;
		}
		
		for (var j = 0; j < NumCellsUp; j++) {
			cellTypeGrid[0, j] = CellType.Blocked;
			cellTypeGrid[NumCellsAcross - 1, j] = CellType.Blocked;
		}
	}
	
	void CreateRooms() {
		
		while (roomList.Count < targetNumOfRooms) AttemptRandomRoom ();
	}
	
	void AttemptRandomRoom() {
		
		int locationX = randomOdd (1, NumCellsAcross - maxRoomLength);
		int locationZ = randomOdd (1, NumCellsUp - maxRoomLength);
		
		CellLocation roomLocation = new CellLocation(locationX, locationZ);
		
		int sizeX = randomOdd (minRoomLength, maxRoomLength);
		int sizeZ = randomOdd (minRoomLength, maxRoomLength);
		
		if (sizeZ > sizeX + 2) sizeZ = sizeX + 2;
		if (sizeX > sizeZ + 2) sizeX = sizeZ + 2;
		
		bool roomCollision = false;
		
		for (var i = roomLocation.x; i < roomLocation.x + sizeX; i++) {
			
			for (var j = roomLocation.z; j < roomLocation.z + sizeZ; j++) {
				
				CellType testCell = cellTypeGrid[i, j];
				if (testCell == CellType.Blocked || testCell == CellType.Perimeter || testCell == CellType.Room) {
					roomCollision = true;
					break;
				}
			}
		}
		if (!roomCollision) {
			
			Room room = new Room(roomLocation.x, roomLocation.z, sizeX, sizeZ);
			roomList.Add (room);
			
			CreateRoom (room);
		}
	}
	
	void CreateRoom(Room room) {
		
		for (var i = room.x; i < room.x + room.width; i++) {
			
			for (var j = room.z; j < room.z + room.height; j++) {
				
				cellTypeGrid[i, j] = CellType.Room;
			}
		}
		
		for (var i = room.x - 1; i < room.x + room.width + 1; i++) {
			cellTypeGrid[i, room.z - 1] = CellType.Perimeter;
			cellTypeGrid[i, room.z + room.height] = CellType.Perimeter;
		}
		
		for (var j = room.z - 1; j < room.z + room.height + 1; j++) {
			cellTypeGrid[room.x - 1, j] = CellType.Perimeter;
			cellTypeGrid[room.x + room.width, j] = CellType.Perimeter;
		}
	}
	
	void CreateEntrances(Room room) {
		
		int possibleEntrances = room.width + room.height + 2;
		
		if (room.x == 1 || room.x +room.width - 1 == NumCellsAcross - 1) {
			
			possibleEntrances -= (room.height + 1) / 2;
		}
		if (room.z == 1 || room.z +room.height - 1 == NumCellsUp - 1) {
			
			possibleEntrances -= (room.width + 1) / 2;
		}
		
		int entrances = 0;
		int entrancesToMake = (int)(possibleEntrances / 4) - 1;
		
		if (entrancesToMake < 1) entrancesToMake = 1;
		else if (entrancesToMake > 3) entrancesToMake = 3;
		
		while (entrances < entrancesToMake) {
			
			int side = Random.Range (1, 5);
			
			if (side == 1) {
				CellLocation testLocation = new CellLocation(randomOdd (room.x, room.x + room.width - 1), room.z - 1);
				
				if (cellTypeGrid[testLocation.x, testLocation.z] != CellType.Entrance && cellTypeGrid[testLocation.x, testLocation.z] != CellType.Blocked) {
					CreateEntrance (testLocation.x, testLocation.z);
					entrances++;
				}
			}
			else if (side == 2) {
				CellLocation testLocation = new CellLocation(randomOdd (room.x, room.x + room.width - 1), room.z + room.height);
				
				if (cellTypeGrid[testLocation.x, testLocation.z] != CellType.Entrance && cellTypeGrid[testLocation.x, testLocation.z] != CellType.Blocked) {
					CreateEntrance (testLocation.x, testLocation.z);
					entrances++;
				}
			}
			else if (side == 3) {
				CellLocation testLocation = new CellLocation(room.x - 1, randomOdd (room.z, room.z + room.height - 1));
				
				if (cellTypeGrid[testLocation.x, testLocation.z] != CellType.Entrance && cellTypeGrid[testLocation.x, testLocation.z] != CellType.Blocked) {
					CreateEntrance (testLocation.x, testLocation.z);
					entrances++;
				}
			}
			else if (side == 4) {
				CellLocation testLocation = new CellLocation(room.x + room.width, randomOdd (room.z, room.z + room.height - 1));
				
				if (cellTypeGrid[testLocation.x, testLocation.z] != CellType.Entrance && cellTypeGrid[testLocation.x, testLocation.z] != CellType.Blocked) {
					CreateEntrance (testLocation.x, testLocation.z);
					entrances++;
				}
			}
		}
	}
	
	void CreateEntrance(int locationX, int locationZ) {
		
		CellLocation location = new CellLocation(locationX, locationZ);
		
		cellTypeGrid[location.x, location.z] = CellType.Entrance;
		entranceLocationList.Add (location);
		
	}
	
	void ChooseCorridorSplitLocation() {
		
		//CellLocation chosenCell = null;
		
		//while (chosenCell == null) {
			
			for (int i = 1; i < NumCellsAcross - 2; i+= 2) {
			
            	for (int j = 1; j < NumCellsUp - 2; j+= 2) {
					
					CellLocation testCell = new CellLocation(i, j);
					
					if (cellTypeGrid[testCell.x, testCell.z] == CellType.Corridor) {
				
						if(CorridorCanGoNorth(testCell) ||
							CorridorCanGoSouth(testCell) ||
							CorridorCanGoEast(testCell) ||
							CorridorCanGoWest(testCell)) {
							
							//chosenCell = testCell;
							//CreateCorridor(chosenCell.x, chosenCell.z);
							CreateCorridor(testCell.x, testCell.z);
						}
					}
				}
			}
		//}
	}
	
	string currentDirection ="North";
	
	bool HasNorth(CellLocation location) {
		
		CellType testType = cellTypeGrid[location.x, location.z + 1];
		
		if (testType == CellType.Room || testType == CellType.Corridor || testType == CellType.Entrance) {
			
			return true;
		}
		
		return false;
	}
	
	bool HasSouth(CellLocation location) {
		
		CellType testType = cellTypeGrid[location.x, location.z - 1];
		
		if (testType == CellType.Room || testType == CellType.Corridor || testType == CellType.Entrance) {
			
			return true;
		}
		
		return false;
	}
	
	bool HasEast(CellLocation location) {
		
		CellType testType = cellTypeGrid[location.x + 1, location.z];
		
		if (testType == CellType.Room || testType == CellType.Corridor || testType == CellType.Entrance) {
			
			return true;
		}
		
		return false;
	}
	
	bool HasWest(CellLocation location) {
		
		CellType testType = cellTypeGrid[location.x - 1, location.z];
		
		if (testType == CellType.Room || testType == CellType.Corridor || testType == CellType.Entrance) {
			
			return true;
		}
		
		return false;
	}
	
	bool HasNorthEast(CellLocation location) {
		
		CellType testType = cellTypeGrid[location.x + 1, location.z + 1];
		
		if (testType == CellType.Room || testType == CellType.Corridor || testType == CellType.Entrance) {
			
			return true;
		}
		
		return false;
	}
	
	bool HasNorthWest(CellLocation location) {
		
		CellType testType = cellTypeGrid[location.x - 1, location.z + 1];
		
		if (testType == CellType.Room || testType == CellType.Corridor || testType == CellType.Entrance) {
			
			return true;
		}
		
		return false;
	}
	
	bool HasSouthEast(CellLocation location) {
		
		CellType testType = cellTypeGrid[location.x + 1, location.z - 1];
		
		if (testType == CellType.Room || testType == CellType.Corridor || testType == CellType.Entrance) {
			
			return true;
		}
		
		return false;
	}
	
	bool HasSouthWest(CellLocation location) {
		
		CellType testType = cellTypeGrid[location.x - 1, location.z - 1];
		
		if (testType == CellType.Room || testType == CellType.Corridor || testType == CellType.Entrance) {
			
			return true;
		}
		
		return false;
	}
	
	void CreateCorridor(int locationX, int locationZ) {
		
		CellLocation location = new CellLocation(locationX, locationZ);
		corridorLocationList.Add (location);
		
		cellTypeGrid[location.x, location.z] = CellType.Corridor;
		
		string wayToGo = "Undecided";
		
		List<string> possibleDirections = new List<string> ();
		
		if (CorridorCanGoNorth(location)) {
			possibleDirections.Add ("North");
			
			if (currentDirection == "North") {
				wayToGo = "North";
			}
		}
		if (CorridorCanGoSouth(location)) {
			possibleDirections.Add ("South");
			
			if (currentDirection == "South") {
				wayToGo = "South";
			}
		}
		if (CorridorCanGoEast(location)) {
			possibleDirections.Add ("East");
			
			if (currentDirection == "East") {
				wayToGo = "East";
			}
		}
		if (CorridorCanGoWest(location)) {
			possibleDirections.Add ("West");
			
			if (currentDirection == "West") {
				wayToGo = "West";
			}
		}
		if (possibleDirections.Count != 0) {
			
			if (straightCorridors) {
				if (wayToGo == "Undecided") {
					int chooseRandom = Random.Range (0, possibleDirections.Count);
					wayToGo = possibleDirections[chooseRandom];
				}
			} else {
				int chooseRandom = Random.Range (0, possibleDirections.Count);
				wayToGo = possibleDirections[chooseRandom];
			}
			
			
			if (wayToGo == "North") {
				
				currentDirection = "North";
				
				CellLocation evenLocation = new CellLocation(location.x, location.z + 1);
				
				cellTypeGrid[evenLocation.x, evenLocation.z] = CellType.Corridor;
				corridorLocationList.Add (evenLocation);
				
				CreateCorridor(location.x, location.z + 2);
			}
			else if (wayToGo == "South") {
				
				currentDirection = "South";
				
				CellLocation evenLocation = new CellLocation(location.x, location.z - 1);
				
				cellTypeGrid[evenLocation.x, evenLocation.z] = CellType.Corridor;
				corridorLocationList.Add (evenLocation);
				
				CreateCorridor(location.x, location.z - 2);
			}
			else if (wayToGo == "East") {
				
				currentDirection = "East";
				
				CellLocation evenLocation = new CellLocation(location.x + 1, location.z);
				
				cellTypeGrid[evenLocation.x, evenLocation.z] = CellType.Corridor;
				corridorLocationList.Add (evenLocation);
				
				CreateCorridor(location.x + 2, location.z);
			}
			else if (wayToGo == "West") {
				
				currentDirection = "West";
				
				CellLocation evenLocation = new CellLocation(location.x - 1, location.z);
				
				cellTypeGrid[evenLocation.x, evenLocation.z] = CellType.Corridor;
				corridorLocationList.Add (evenLocation);
				
				CreateCorridor(location.x - 2, location.z);
			}
			
		} else {
			
			int unusedCells = 0;
			
			for (int i = 1; i < NumCellsAcross - 2; i+= 2) {
			
            	for (int j = 1; j < NumCellsUp - 2; j+= 2) {
					
					if (cellTypeGrid[i, j] == CellType.Empty) {
						
						unusedCells++;
					}
				}
			}
			
			if (unusedCells > 0) {
				
				ChooseCorridorSplitLocation();
			}
			
		}
	}
	
	void CreateCorridor(CellLocation corridorLocation) {
		
		cellTypeGrid[corridorLocation.x, corridorLocation.z] = CellType.Corridor;
	}
	
	bool CorridorCanGoNorth(CellLocation corridorLocation) {
		
		if (corridorLocation.z == NumCellsUp - 2) {
			
			return false;
		}
		else if (cellTypeGrid[corridorLocation.x, corridorLocation.z + 2] == CellType.Empty) {
			
			return true;
		}
		
		return false;
	}
	
	bool CorridorCanGoSouth(CellLocation corridorLocation) {
		
		if (corridorLocation.z == 1) {
			
			return false;
		}
		else if (cellTypeGrid[corridorLocation.x, corridorLocation.z - 2] == CellType.Empty) {
			
			return true;
		}
		
		return false;
	}
	
	bool CorridorCanGoEast(CellLocation corridorLocation) {
		
		if (corridorLocation.x == NumCellsAcross - 2) {
			
			return false;
		}
		else if (cellTypeGrid[corridorLocation.x + 2, corridorLocation.z] == CellType.Empty) {
			
			return true;
		}
		
		return false;
	}
	
	bool CorridorCanGoWest(CellLocation corridorLocation) {
		
		if (corridorLocation.x == 1) {
			
			return false;
		}
		else if (cellTypeGrid[corridorLocation.x - 2, corridorLocation.z] == CellType.Empty) {
			
			return true;
		}
		
		return false;
	}
	
	void CleanDeadEnds() {
		
		List<CellLocation> deadEnds = new List<CellLocation> ();
		List<CellLocation> possibleDeadEnds = new List<CellLocation> ();
		List<CellLocation> corridorsToTest = new List<CellLocation> ();
		
		foreach (CellLocation location in corridorLocationList) {
			corridorsToTest.Add (location);
		}
		
		//int corridorTestCount = corridorsToTest.Count;
		
		int count = 0;
		
		while (count < 200) {
			
			count ++;
			
			foreach (CellLocation location in corridorLocationList) {
			
				int numConnections = 0;
				
				CellLocation north = new CellLocation(location.x, location.z + 1);
				CellLocation south = new CellLocation(location.x, location.z - 1);
				CellLocation east = new CellLocation(location.x + 1, location.z);
				CellLocation west = new CellLocation(location.x - 1, location.z);
				
				
				if (cellTypeGrid[north.x, north.z] == CellType.Corridor) {
					numConnections ++;
				}
				if (cellTypeGrid[east.x, east.z] == CellType.Corridor) {
					numConnections ++;
				}
				if (cellTypeGrid[south.x, south.z] == CellType.Corridor) {
					numConnections ++;
				}
				if (cellTypeGrid[west.x, west.z] == CellType.Corridor) {
					numConnections ++;
				}
				if (cellTypeGrid[north.x, north.z] == CellType.Entrance) {
					numConnections ++;
				}
				if (cellTypeGrid[east.x, east.z] == CellType.Entrance) {
					
					numConnections ++;
				}
				if (cellTypeGrid[south.x, south.z] == CellType.Entrance) {
					numConnections ++;
				}
				if (cellTypeGrid[west.x, west.z] == CellType.Entrance) {
					numConnections ++;
				}
				
				
				if (numConnections < 2) {
					
					cellTypeGrid[location.x, location.z] = CellType.Empty;
					deadEnds.Add (location);
					
				}
			}
			
			foreach (CellLocation deadEnd in deadEnds) {
				corridorLocationList.Remove (deadEnd);
			}
			
			foreach (CellLocation location in entranceLocationList) {
				
				if (cellTypeGrid[location.x, location.z + 1] != CellType.Corridor &&
					cellTypeGrid[location.x, location.z - 1] != CellType.Corridor &&
					cellTypeGrid[location.x + 1, location.z] != CellType.Corridor &&
					cellTypeGrid[location.x - 1, location.z] != CellType.Corridor) {
					
					if (cellTypeGrid[location.x, location.z + 1] == CellType.Room &&
					cellTypeGrid[location.x, location.z - 1] == CellType.Room) {
						
					}
					else if (cellTypeGrid[location.x + 1, location.z] == CellType.Room &&
					cellTypeGrid[location.x - 1, location.z] == CellType.Room) {
						
					}
					else {
						cellTypeGrid[location.x, location.z] = CellType.Perimeter;
					}
				}
			}
		}
	}
	
	void AddChests() {
		
		foreach (Room room in roomList) {
			
			int chestLocationX = room.x  + room.width / 2;
			int chestLocationZ = room.z  + room.height / 2;
			
			CellLocation chestLocation = new CellLocation(chestLocationX, chestLocationZ);
			
			InstantiatePrefab (TreasureChest, chestLocation, 0);
		}
	}
	
	int randomOdd(int min, int max) {
		
		min = (min - 1) / 2;
		max = (max - 1) / 2;
		
		int result = Random.Range (min, max + 1) * 2 + 1;
		
		return result;
	}
	
	void MakeCellsWalkable() {
		
		for (int i = 0; i < NumCellsAcross; i++) {
			
            for (int j = 0; j < NumCellsUp; j++) {
				
				CellType testCell = cellTypeGrid[i, j];
				
				if (testCell == CellType.Entrance || testCell == CellType.Corridor || testCell == CellType.Room) {
					
					cellWalkableGrid[i, j] = CellWalkable.Yes;
				}
			}
		}
	}
	
	void CreateFloor() {
		
        FillCellGrid();
        InstantiateCellsFromGrid();
		InstantiateWallsFromGrid();
		AddChests ();
		PlaceHeroAtRandom ();
    }
	
	void DeleteFloor() {
		
        for (int i = 0; i < PrefabCellParent.childCount; i++) {
			
            Destroy(PrefabCellParent.GetChild(i).gameObject);
        }

        for (int i = 0; i < OtherStuffParent.childCount; i++) {
			
            Destroy(OtherStuffParent.GetChild(i).gameObject);
        }
		
		roomList.Clear ();
		entranceLocationList.Clear ();
		corridorLocationList.Clear ();
    }
	
	public void GoToNextFloor()
    {
        DeleteFloor();
        CreateFloor();
        FloorNumber++;
    }
	
	void Start() {
		
        cellTypeGrid = new CellType[NumCellsAcross, NumCellsUp];
		cellWalkableGrid = new CellWalkable[NumCellsAcross, NumCellsUp];
        CreateFloor();
	}
	
	void Update() {
		
		if (Input.GetKeyUp(KeyCode.N)) GoToNextFloor();
	}
	
	void PlaceHeroAtRandom() {
		
		for (int i = 0; i < NumCellsAcross; i++) {
			
            for (int j = 0; j < NumCellsUp; j++) {
				
				CellLocation testLocation = new CellLocation(i, j);
				
				if (cellWalkableGrid[i, j] == CellWalkable.Yes) {
					PlaceHero(testLocation);
					break;
				}
			}
		}
	}
	
	void PlaceHero(CellLocation newLocation)
    {
        Hero.heroCellLocation = new Vector3(newLocation.x, 0, newLocation.z);
		
		//print (heroLocation_square.x + " " + heroLocation_square.z);
		
        prefabCellWidth = Cell_Empty.localScale.x;
        prefabCellHeight = Cell_Empty.localScale.z;
		
        float heroPositionX = transform.position.x + (newLocation.x * prefabCellWidth);
        float heroPositionZ = transform.position.z + (newLocation.z * prefabCellHeight);

        Hero.transform.position = new Vector3(heroPositionX, 5f, heroPositionZ);
	}
	
	public void SetHeroLocation() {
		
		float heroLocationX = (Hero.transform.position.x - transform.position.x)/prefabCellWidth;
		float heroLocationZ = (Hero.transform.position.z - transform.position.z)/prefabCellHeight;
		
		int heroCellX = (int)(heroLocationX + 0.5f);
		int heroCellZ = (int)(heroLocationZ + 0.5f);
		
		Hero.heroLocation = new Vector3(heroLocationX,5f,heroLocationZ);
		Hero.heroCellLocation = new Vector3(heroCellX,0,heroCellZ);
		
	}
}
