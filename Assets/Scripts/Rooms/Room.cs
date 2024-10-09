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
    [HideInInspector] public int distanceToStartRoom;
    [HideInInspector] private float playerLuckModifier = 1.0f;
    [SerializeField] private List<TileBase> tiles = new();
    [SerializeField] private TileBase holeTile;
    [SerializeField] private Tilemap objectTilemap;
    [SerializeField] private Tilemap holeTilemap;
    [SerializeField] private List<Door> doors = new();
    [SerializeField] private List<Rect> rejectRects;

    private Vector3Int generationOffset = new(-13, -8, 0);
    private Vector2Int tilemapSize = new(25, 12);

    [Space(10)]
    [Header("Testing values for room generation")]
    [SerializeField] private int enemies;
    [SerializeField] private int weapons; //TODO replace with chance of weapon
    [SerializeField] private int obstacles;
    [SerializeField] private int holes;
    [SerializeField] private int finalEnemyCount = 3; //3 minimum
    [SerializeField] private float maxHealthModifier;
    [SerializeField] private float damageModifier;

    private int enemiesRemaining;
    private float[] enemyModifiers = new float[3];
    public bool startingRoom;

    private void Start()
    {
        if(!startingRoom)
        {
            DifficultyScaler();
            GenerateRandomObstacleTiles();
            GenerateRandomEnemiesAndPickups();
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
        var obstaclePoints = OffsetPoints(RandomPoints(obstacles));

        var holePoints = OffsetPoints(RandomPoints(holes));

        var tilesToSet = new List<TileBase>();

        //choose random tiles
        for (int i = 0; i < obstacles; i++)
            tilesToSet.Add(tiles[Random.Range(0, tiles.Count)]);

        //place tiles
        objectTilemap.SetTiles(obstaclePoints.ToArray(), tilesToSet.ToArray());

        foreach (var point in holePoints)
            holeTilemap.SetTile(point, holeTile);

    }

    /// <summary>
    /// Place the specified amount of enemies and pickups into the room, evenly spread
    /// WIP
    /// </summary>
    /// <param name="enemyAmt"></param>
    /// <param name="pickupAmt"></param>
    public void GenerateRandomEnemiesAndPickups()
    {
        //load all enemy prefabs
        var enemyPrefabs = Resources.LoadAll("EnemyPrefabs/");
        List<GameObject> enemiesToPlace = new();

        //randomize which enemy prefabs are spawned
        for (int i = 0; i < finalEnemyCount; i++)
            enemiesToPlace.Add(enemyPrefabs[Random.Range(0, enemyPrefabs.Length)] as GameObject);

        //generate locations
        var enemyPoints = OffsetPoints(RandomPoints(finalEnemyCount));

        //spawn enemy, set room, disable it, and add into enemieslist
        for (int i = 0; i < finalEnemyCount; i++)
        {
            var enemy = Instantiate(enemiesToPlace[i], (Vector3)enemyPoints[i] + transform.position, Quaternion.identity).GetComponent<Enemy>();
            enemy.room = this;
            ScaleEnemyStats(enemy);
        }

        enemiesRemaining = finalEnemyCount;
        Debug.Log(enemiesRemaining + " enemies remaining");

        //same shit for weapons except there's only 1 prefab

        float weaponChance = Random.Range(0, 1000);
        if (weaponChance < 50 * ( 1.0f + (UpgradeMenu.currentTotalLuckUpgrade / 100))) { weapons = 2; }
        else if (weaponChance < 500 * (1.0f + (UpgradeMenu.currentTotalLuckUpgrade / 100))) { weapons = 1; }
        else { weapons = 0; }

        var weaponPickup = Resources.Load("WeaponPickup") as GameObject;
        var pickupPoints = OffsetPoints(RandomPoints(weapons));

        foreach (var point in pickupPoints)
        {
            var go = Instantiate(weaponPickup, (Vector3)point + transform.position, Quaternion.identity);
            go.GetComponent<WeaponPickup>().RandomizeWeapon();
        }
    }

    public void DifficultyScaler()
    {
        enemyModifiers = TestDifficultyScaler.instance.SetCurrentRoomDifficulty(distanceToStartRoom);
        finalEnemyCount = (int)enemyModifiers[0];
        maxHealthModifier = enemyModifiers[1];
        damageModifier = enemyModifiers[2]; //0 = count, 1 = health, 2 = dmg
    }

    public void ScaleEnemyStats(Enemy enemy)
    {
        enemy.maxHealthMultiplier = maxHealthModifier;
        enemy.damageMultiplier = damageModifier;
        //damaget tänne ym.
    }

    /// <summary>
    /// Called when an enemy is killed
    /// Updates room enemy count and unlocks doors when enemies are dead;
    /// </summary>
    public void EnemyKilled()
    {
        enemiesRemaining--;
        Debug.Log(enemiesRemaining + " enemies remaining");
        if (enemiesRemaining == 0)
        {
            UnlockDoors();
            if(StatDisplay.instance) StatDisplay.instance.RoomCleared();
        }
        if(StatDisplay.instance) StatDisplay.instance.KillCount();
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
    /// <returns>Offset list of points</returns>
    private List<Vector3Int> OffsetPoints(List<Vector3Int> points)
    {
        var offsetPoints = new List<Vector3Int>();

        foreach (var point in points)
        {
            offsetPoints.Add(point + generationOffset);
        }

        return offsetPoints;
    }

    /// <summary>
    /// Generate a list of random points
    /// </summary>
    /// <param name="length"></param>
    /// <returns>List of random points inside tileMapSize</returns>
    private List<Vector3Int> RandomPoints(int length)
    {
        var randomPoints = new List<Vector3Int>();

        for (int i = 0; i < length; i++)
            randomPoints.Add(RandomPoint());

        return randomPoints;
    }

    /// <summary>
    /// Generate a random point
    /// </summary>
    /// <returns>Random point inside tilemapSize</returns>
    private Vector3Int RandomPoint()
    {
        Vector3Int randPos;
        bool reject;

        do
        {
            reject = false;
            randPos = new Vector3Int(Random.Range(0, tilemapSize.x), Random.Range(0, tilemapSize.y));

            foreach (var rect in rejectRects) if (rect.Contains(randPos)) reject = true;
        }
        while (reject);

        return randPos;
    }
}