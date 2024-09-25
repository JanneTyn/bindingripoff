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

    private void Start()
    {
        var points = PoissonDiscSampling.GeneratePoints(radius, tilemapSize, 30);
        var pointsInt = new List<Vector3Int>();
        var tilesToSet = new List<TileBase>();

        foreach (var point in points)
        {
            pointsInt.Add(new Vector3Int((int)point.x, (int)point.y, 0));
        }

        for(int i = 0; i < iterations; i++)
        {
            tilesToSet.Add(tiles[Random.Range(0, tiles.Count)]);
        }

        tilemap.SetTiles(pointsInt.ToArray(), tilesToSet.ToArray());
    }
}