using UnityEngine;
using System.Collections;



public class HeatMap : MonoBehaviour
{

    [System.Serializable]
    public enum HeatMapType { None, Damage, Death, Kill, Interaction, Position };

    float MapWidth;
    float MapHeight;
    int MapWidthVoxels;
    int MapHeightVoxels;
    int[,] EventCounts;
    HeatMapCube[,] HeatMapCubes;
    public HeatMapCube HeatMapCubePrefab;
    public HeatMapType MapType;

    public Transform LowerLeftCorner;
    public Transform UpperRightCorner;
    public float GridSquareSize = 10f;
    public float CubeScale = 2f;

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
        
        Vector3 relative_position = position - LowerLeftCorner.position;
        ret.x = Mathf.FloorToInt(relative_position.x / GridSquareSize);
        ret.y = Mathf.FloorToInt(relative_position.z / GridSquareSize);

        //Should probably check if in bounds, idk

        return ret;
    }

    void AddToMap(Vector3 position)
    {

        Vector2Int map_position = FindPositionInMap(position);
        EventCounts[map_position.x, map_position.y] += 1;

        Vector3 cube_position = LowerLeftCorner.position;
        cube_position.y = position.y;
        cube_position.x += map_position.x * GridSquareSize + GridSquareSize / 2;
        cube_position.z += map_position.y * GridSquareSize + GridSquareSize / 2;
        if (HeatMapCubes[map_position.x, map_position.y] == null) 
        {
            GameObject new_cube = Instantiate(HeatMapCubePrefab.gameObject, cube_position, Quaternion.identity);
            new_cube.GetComponent<Transform>().localScale = new Vector3(CubeScale, CubeScale, CubeScale);
            HeatMapCubes[map_position.x, map_position.y] = new_cube.GetComponent<HeatMapCube>();
        }

        float color_ramp_amount = (float)EventCounts[map_position.x, map_position.y] / MaxCount;
        Color my_color = ColorGradient.Evaluate(color_ramp_amount);
        HeatMapCubes[map_position.x, map_position.y].SetColor(my_color);
    }

    public void RegisterDamageEvent(EventHandler myEventHandler)
    {
        if (MapType != HeatMapType.Damage || myEventHandler.damage_events.Count == 0)
            return;

        DamageEvent my_event = myEventHandler.damage_events[myEventHandler.damage_events.Count - 1];
        AddToMap(my_event.GetPosition());
    }

    public void RegisterDeathEvent(EventHandler myEventHandler)
    {
        if (MapType != HeatMapType.Death || myEventHandler.damage_events.Count == 0)
            return;

        DamageEvent my_event = myEventHandler.damage_events[myEventHandler.damage_events.Count - 1];
        AddToMap(my_event.GetPosition());
    }

    public void RegisterKillEvent(EventHandler myEventHandler)
    {
        if (MapType != HeatMapType.Kill || myEventHandler.damage_events.Count == 0)
            return;

        DamageEvent my_event = myEventHandler.damage_events[myEventHandler.damage_events.Count - 1];
        AddToMap(my_event.GetPosition());
    }

    public void RegisterInteractionEvent(EventHandler myEventHandler)
    {
        if (MapType != HeatMapType.Interaction || myEventHandler.interaction_events.Count == 0)
            return;

        InteractionEvent my_event = myEventHandler.interaction_events[myEventHandler.interaction_events.Count - 1];
        AddToMap(my_event.GetPosition());
    }

    public void RegisterPositionEvent(EventHandler myEventHandler)
    {
        if (MapType != HeatMapType.Position || myEventHandler.position_events.Count == 0)
            return;

        PositionEvent my_event = myEventHandler.position_events[myEventHandler.position_events.Count - 1];
        AddToMap(my_event.GetPosition());
    }
}
