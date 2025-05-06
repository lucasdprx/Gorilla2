using System.Collections;
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
    [SerializeField] private int maxPlatform = 2;
    [SerializeField] private int platformDefaultPointCount = 4;
    [SerializeField] private float maxRangeSpawn = 3;
    [SerializeField] private float minRangeSpawn = 2;

    private void Start()
    {
        if (map == null || tilemap == null)
        {
            Debug.LogError("Map tile is not assigned.");
            return;
        }

        StartCoroutine(GenerateMap());
    }

    private IEnumerator GenerateMap()
    {
        GenerateTiles();
        yield return new WaitForEndOfFrame();
        GeneratePlatforms();
    }
    private void GeneratePlatforms()
    {
        for (int i = 0; i < platformDefaultPointCount; i++)
        {
            Vector3 origin = transform.position + new Vector3(Random.Range(1, (float)width), height + 1);
            Vector2 size = oneWayPlatformPrefab.transform.localScale * 2;

            RaycastHit2D hit = Physics2D.Raycast(origin, Vector2.down, 1000f, LayerMask.GetMask("Ground"));
            if (!hit)
            {
                Debug.LogWarning("No hit detected.");
                return;
            }

            Vector2 randomPoint = hit.point + GetRandomVector2() * Random.Range(minRangeSpawn, maxRangeSpawn + 1);
            if (Physics2D.OverlapBoxAll(randomPoint, size, 0f).Length > 0)
            {
                i--;
                continue;
            }

            GeneratePlatformChain(hit.point, randomPoint, size);
        }
    }

    private void GeneratePlatformChain(Vector2 initPoint, Vector2 randomPoint, Vector2 size)
    {
        for (int j = 0; j < maxPlatform; j++)
        {
            GameObject platform = Instantiate(oneWayPlatformPrefab, randomPoint, Quaternion.identity);
            platform.transform.SetParent(transform);

            DrawBox(randomPoint, size, Color.green, 5000f);
            Debug.DrawLine(initPoint, randomPoint, Color.red, 5000f);

            initPoint = randomPoint;
            randomPoint += GetRandomVector2() * Random.Range(minRangeSpawn, maxRangeSpawn + 1);

            if (Physics2D.OverlapBoxAll(randomPoint, size, 0f).Length > 0 && j < maxPlatform - 1)
            {
                DrawBox(randomPoint, size, Color.red, 5000f);
                Debug.DrawLine(initPoint, randomPoint, Color.red, 5000f);
                break;
            }
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
    
    private static void DrawBox(Vector2 point, Vector2 size, Color color, float duration = 5f)
    {
        Vector2 topLeft = point + new Vector2(-size.x / 2, size.y / 2);
        Vector2 topRight = point + new Vector2(size.x / 2, size.y / 2);
        Vector2 bottomRight = point + new Vector2(size.x / 2, -size.y / 2);
        Vector2 bottomLeft = point + new Vector2(-size.x / 2, -size.y / 2);
        
        Debug.DrawLine(topLeft, topRight, color, duration);
        Debug.DrawLine(topRight, bottomRight, color, duration);
        Debug.DrawLine(bottomRight, bottomLeft, color, duration);
        Debug.DrawLine(bottomLeft, topLeft, color, duration);
    }
    
    private static Vector2 GetRandomVector2()
    {
        Vector2 randomVector = new Vector2(Random.Range(-1f, 1f), Random.Range(0f, 1f)).normalized;
        return randomVector;
    }
}
