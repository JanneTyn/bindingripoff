using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

/// <summary>
/// WIP random tiles
/// </summary>
public class Room : MonoBehaviour
{
    [SerializeField] private Vector2Int tilemapSize;
    [SerializeField] private List<TileBase> tiles = new();
    [SerializeField] private int iterations;
    private float radius => (tilemapSize.x / iterations) * 1.2f;
    [SerializeField] private Tilemap tilemap;

    [Space(5)]
    [Header("Testing values for room generation")]
    [SerializeField] private int enemies;
    [SerializeField] private int weapons; //TODO replace with chance of weapon

    private void Start()
    {
        GenerateRandomObstacleTiles();
        GenerateRandomEnemiesAndPickups(enemies, weapons);
    }

    /// <summary>
    /// Place random obstacle tiles from the "tiles" array onto the tilemap
    /// </summary>
    public void GenerateRandomObstacleTiles()
    {
        //generate points, convert them to int
        var points = PoissonDiscSampling.GeneratePoints(radius, tilemapSize, 30);
        var pointsInt = new List<Vector3Int>();
        var tilesToSet = new List<TileBase>();

        foreach (var point in points)
        {
            pointsInt.Add(new Vector3Int((int)point.x, (int)point.y, 0));
        }

        //choose random tiles

        for (int i = 0; i < iterations; i++)
        {
            tilesToSet.Add(tiles[Random.Range(0, tiles.Count)]);
        }

        //place tiles
        tilemap.SetTiles(pointsInt.ToArray(), tilesToSet.ToArray());
    }

    /// <summary>
    /// Place the specified amount of enemies and pickups into the room, evenly spread
    /// WIP
    /// </summary>
    /// <param name="enemyAmt"></param>
    /// <param name="pickupAmt"></param>
    public void GenerateRandomEnemiesAndPickups(int enemyAmt, int pickupAmt)
    {
        //TODO actually get the correct amount of spawns, lol

        //load all enemy prefabs
        var enemies = Resources.LoadAll("EnemyPrefabs/");
        List<GameObject> enemiesToPlace = new();

        //randomize which enemy prefabs are spawned
        for(int i = 0; i < enemyAmt; i++)
        {
            enemiesToPlace.Add(enemies[Random.Range(0, enemies.Length)] as GameObject);
        }

        //generate locations
        var enemyPoints = PoissonDiscSampling.GeneratePoints((tilemapSize.x / enemyAmt) * 1.2f, tilemapSize, 30);

        //spawn
        for(int i = 0; i < enemiesToPlace.Count; i++)
        {
            Instantiate(enemiesToPlace[i], (Vector3)enemyPoints[i] + transform.position, Quaternion.identity);
        }

        //same shit for weapons except there's only 1 prefab

        var weaponPickup = Resources.Load("WeaponPickup") as GameObject;
        var pickupPoints = PoissonDiscSampling.GeneratePoints((tilemapSize.x / pickupAmt) * 1.2f, tilemapSize, 30);
        foreach(var point in pickupPoints)
        {
            var go = Instantiate(weaponPickup, (Vector3)point + transform.position, Quaternion.identity);
            go.GetComponent<WeaponPickup>().RandomizeWeapon();
        }
    }
}