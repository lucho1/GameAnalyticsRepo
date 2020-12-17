using UnityEngine;

public class HeatMap : MonoBehaviour
{
    float MapWidth;
    float MapHeight;
    int MapWidthVoxels;
    int MapHeightVoxels;
    int[,] EventCounts;
    HeatMapCube[,] HeatMapCubes;
    public HeatMapCube HeatMapCubePrefab;

    public Transform LowerLeftCorner;
    public Transform UpperRightCorner;
    public float GridSquareSize = 10f;

    public Gradient ColorGradient;
    public int MaxCount = 100;

    // Start is called before the first frame update
    void Start()
    {
        MapWidth = Mathf.Abs(UpperRightCorner.position.x - LowerLeftCorner.position.x);
        MapHeight = Mathf.Abs(UpperRightCorner.position.z - LowerLeftCorner.position.z);
        MapWidthVoxels = Mathf.CeilToInt( MapWidth / GridSquareSize);
        MapHeightVoxels = Mathf.CeilToInt( MapHeight / GridSquareSize);

        EventCounts = new int[MapWidthVoxels, MapHeightVoxels];
        HeatMapCubes = new HeatMapCube[MapWidthVoxels, MapHeightVoxels];

        for (int i = 0; i < MapWidthVoxels; ++i)
            for (int j = 0; j < MapHeightVoxels; ++j)
                EventCounts[i,j] = 0;
    }

    public Vector2Int FindPositionInMap(Vector3 position)
    {
        Vector2Int ret = new Vector2Int();
        
        Vector3 relative_position = position - UpperRightCorner.position;
        ret.x = Mathf.FloorToInt(relative_position.x / GridSquareSize);
        ret.y = Mathf.FloorToInt(relative_position.z / GridSquareSize);

        return ret;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
