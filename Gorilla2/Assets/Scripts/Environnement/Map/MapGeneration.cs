using System;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

public class MapGeneration : MonoBehaviour
{
    [Header("TileMap")]
    [SerializeField] private Tile map;
    [SerializeField] private Tilemap tilemap;
    
    [Header("Platform")]
    [SerializeField] private GameObject oneWayPlatformPrefab;
    
    [Header("Parameters Tiles")]
    [SerializeField] private int height = 7;
    [SerializeField] private int width = 25;
    [SerializeField] private int distance = 3;
    [Min(1)] [SerializeField] private int maxGap = 3;
    
    [Header("Parameters Platforms")]
    [SerializeField] private int maxPlatform;
    [SerializeField] private int platformDefaultPointCount;
    [SerializeField] private float maxRangeSpawn;
    [SerializeField] private float minRangeSpawn;
    private void Awake()
    {
        GenerateTiles();
    }

    private void Start()
    {
        if (map == null || tilemap == null)
        {
            Debug.LogError("Map tile is not assigned.");
            return;
        }
        GeneratePlatforms();
    }
    private void GeneratePlatforms()
    {
        for (int i = 0; i < platformDefaultPointCount; i++)
        {
            Vector3 origin = transform.position + new Vector3(Random.Range(0, width), height + 1);
            print(origin);
            print(Physics2D.Raycast(origin , Vector2.down, Mathf.Infinity).collider);
            // print(hit.point);
            // print(hit.collider);
            // print(origin);
            // if (!hit)
            // {
            //     Debug.LogWarning("No hit detected.");
            //     return;
            // }
            // Vector2 randomPoint = hit.point + new Vector2(Random.Range(0f, 1f), Random.Range(0f, 1f)) * Random.Range(minRangeSpawn, maxRangeSpawn);
            // print(Physics2D.OverlapBoxAll(randomPoint, (Vector2)oneWayPlatformPrefab.transform.localScale, 0f).Length);
        }
    }
    private void GenerateTiles()
    {
        int randHeight = Random.Range(1, height);
        int gap = Random.Range(0, maxGap);
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < randHeight; j++)
            {
                tilemap.SetTile(new Vector3Int(i, j, 0), map);
            }
            
            if (gap > 0)
            {
                gap--;
                continue;
            }
            
            randHeight += Random.Range(-distance, distance + 1);
            randHeight = Mathf.Clamp(randHeight, 1, height);
            gap = Random.Range(0, maxGap);
        }
    }
}
