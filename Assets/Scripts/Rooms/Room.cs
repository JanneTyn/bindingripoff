using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Tilemaps;

/// <summary>
/// Random generation of room and game logic about room, doors, etc.
/// </summary>
public class Room : MonoBehaviour
{
    [HideInInspector] public Vector2Int coordinate;
    [SerializeField] public int distanceToStartRoom;
    [SerializeField] private Vector2Int tilemapSize;
    [SerializeField] private List<TileBase> tiles = new();
    [SerializeField] private Tilemap tilemap;
    [SerializeField] private List<Door> doors = new();
    private Vector2 generationOffset = new Vector2(-13, -8);

    [Space(5)]
    [Header("Testing values for room generation")]
    [SerializeField] private int enemies;
    [SerializeField] private int weapons; //TODO replace with chance of weapon
    [SerializeField] private int obstacles;
    [SerializeField] private int finalEnemyCount = 3; //3 minimum
    [SerializeField] private float maxHealthModifier;

    private int enemiesRemaining;
    public bool startingRoom;

    private void Start()
    {
        if(!startingRoom)
        {
            DifficultyScaler();
            GenerateRandomObstacleTiles();
            GenerateRandomEnemiesAndPickups(finalEnemyCount, weapons);
        }
        else
        {
            //TODO boss huone ovi tähä
            foreach(var d in doors) d.Unlock();
        }
    }

    /// <summary>
    /// Place random obstacle tiles from the "tiles" array onto the tilemap
    /// </summary>
    public void GenerateRandomObstacleTiles()
    {
        //generate points, convert them to int
        var points = PoissonDiscSampling.GeneratePoints((tilemapSize.x / obstacles) * 1.2f, tilemapSize, 30);
        points = OffsetPoints(points);

        var pointsInt = new List<Vector3Int>();
        var tilesToSet = new List<TileBase>();

        foreach (var point in points)
        {
            pointsInt.Add(new Vector3Int((int)point.x, (int)point.y, 0));
        }

        //choose random tiles

        for (int i = 0; i < obstacles; i++)
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
        for (int i = 0; i < enemyAmt; i++)
        {
            enemiesToPlace.Add(enemies[Random.Range(0, enemies.Length)] as GameObject);
        }

        //generate locations
        var enemyPoints = PoissonDiscSampling.GeneratePoints((tilemapSize.x / enemyAmt), tilemapSize, 30);
        enemyPoints = OffsetPoints(enemyPoints);

        //spawn enemy, set room, disable it, and add into enemieslist
        for (int i = 0; i < enemiesToPlace.Count; i++)
        {
            var enemy = Instantiate(enemiesToPlace[i], (Vector3)enemyPoints[i] + transform.position, Quaternion.identity).GetComponent<Enemy>();
            enemy.room = this;
            ScaleEnemyStats(enemy);
        }

        enemiesRemaining = enemiesToPlace.Count;
        Debug.Log(enemiesRemaining + " enemies remaining");

        //same shit for weapons except there's only 1 prefab

        var weaponPickup = Resources.Load("WeaponPickup") as GameObject;
        var pickupPoints = PoissonDiscSampling.GeneratePoints((tilemapSize.x / pickupAmt), tilemapSize, 30);
        pickupPoints = OffsetPoints(pickupPoints);

        foreach (var point in pickupPoints)
        {
            var go = Instantiate(weaponPickup, (Vector3)point + transform.position, Quaternion.identity);
            go.GetComponent<WeaponPickup>().RandomizeWeapon();
        }
    }

    public void DifficultyScaler()
    {
        finalEnemyCount = Random.Range(3 + distanceToStartRoom, 5 + distanceToStartRoom);
        maxHealthModifier = 1f + (0.15f * (distanceToStartRoom - 1));
    }

    public void ScaleEnemyStats(Enemy enemy)
    {
        enemy.maxHealthMultiplier = maxHealthModifier;
        //damaget tänne ym.
    }

    /// <summary>
    /// Called when an enemy is killed
    /// Updates room enemy count and unlocks doors when enemies are dead;
    /// </summary>
    public void EnemyKilled()
    {
        enemiesRemaining -= 1;
        Debug.Log(enemiesRemaining + " enemies remaining");
        if (enemiesRemaining == 0)
        {
            UnlockDoors();
            StatDisplay.instance.RoomCleared();
        }
    }

    /// <summary>
    /// Call Unlock() on all doors in this room
    /// </summary>
    public void UnlockDoors()
    {
        Debug.Log("Doors unlocked");

        foreach (var door in doors)
        {
            door.Unlock();
        }
    }

    /// <summary>
    /// Offset array of points by generationOffset variable
    /// </summary>
    /// <param name="points"></param>
    /// <returns></returns>
    private List<Vector2> OffsetPoints(List<Vector2> points)
    {
        var offsetPoints = new List<Vector2>();

        foreach (var point in points)
        {
            offsetPoints.Add(point + generationOffset);
        }

        return offsetPoints;
    }
}