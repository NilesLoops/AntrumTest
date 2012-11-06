using UnityEngine;
using System.Collections;

public class GeneratorTemplate : MonoBehaviour {
	
	private enum CellType
	{
		None,
		Empty,
		Blocked,
		Room,
		Perimeter,
		Entrance,
		Corridor
	}
	
	public class CellLocation
    {
        public CellLocation(int setX, int setZ)
        {
            x = setX;
            z = setZ;
        }

        public int x;
        public int z;

        public override bool Equals(object obj)
        {
            if (obj is CellLocation)
            {
                CellLocation testObj = (CellLocation)obj;
                if (testObj.x == x && testObj.z == z)
                {
                    return true;
                }
            }

            return false;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
	
	public Transform PrefabCell_Empty;
    public Transform PrefabCell_Blocked;
    public Transform PrefabCell_Room;
    public Transform PrefabCell_Perimeter;
    public Transform PrefabCell_Entrance;
    public Transform PrefabCell_Corridor;
	
	public Transform PrefabCellParent;
    public Transform OtherStuffParent;
	
	public int NumCellsAcross;
    public int NumCellsUp;
	
	private float prefabCellWidth;
    private float prefabCellHeight;
	
	[HideInInspector]
    public int FloorNumber = 1;
	
	private CellType[,] cellTypeGrid;

    Transform GetPrefabFromCellType(CellType cellType)
    {
        switch (cellType)
        {
        	case CellType.Empty:
            	return PrefabCell_Empty;
			case CellType.Blocked:
				return PrefabCell_Blocked;
			case CellType.Room:
				return PrefabCell_Room;
			case CellType.Perimeter:
				return PrefabCell_Perimeter;
			case CellType.Entrance:
				return PrefabCell_Entrance;
			case CellType.Corridor:
				return PrefabCell_Corridor;
			
			default:
				return PrefabCell_Empty;
        }
    }
	
	void InstantiateCellsFromGrid()
    {
        prefabCellWidth = PrefabCell_Room.localScale.x;
        prefabCellHeight = PrefabCell_Room.localScale.z;

        for (int i = 0; i < NumCellsAcross; ++i)
        {
            for (int j = 0; j < NumCellsUp; ++j)
            {                
                if (cellTypeGrid[i, j] != CellType.None)
                {
                    float instantiateXPosition = transform.position.x + (i * prefabCellWidth);
                    float instantiateZPosition = transform.position.z + (j * prefabCellHeight);
                    Transform prefabToMake = GetPrefabFromCellType(cellTypeGrid[i, j]);
                    Transform createCell = (Transform)Instantiate(prefabToMake, new Vector3(instantiateXPosition, 0.0f, instantiateZPosition), Quaternion.identity);

                    createCell.parent = PrefabCellParent;
                }
            }
        }
    }
	
	void FillCellGrid()
    {
        for (int i = 0; i < NumCellsAcross; ++i)
        {
            for (int j = 0; j < NumCellsUp; ++j)
            {
                cellTypeGrid[i, j] = CellType.Empty;
            }
        }
        
        // Call Algorithms here
    }
	
	void CreateFloor()
    {
        FillCellGrid();
        InstantiateCellsFromGrid();
    }
	
	void DeleteFloor()
    {
        for (int i = 0; i < PrefabCellParent.childCount; ++i)
        {
            Destroy(PrefabCellParent.GetChild(i).gameObject);
        }

        for (int i = 0; i < OtherStuffParent.childCount; ++i)
        {
            Destroy(OtherStuffParent.GetChild(i).gameObject);
        }
    }
	
	public void GoToNextFloor()
    {
        DeleteFloor();
        CreateFloor();
        FloorNumber++;
    }
	
	void Start()
	{	    
        cellTypeGrid = new CellType[NumCellsAcross, NumCellsUp];
        CreateFloor();
	}
}
