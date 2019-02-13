using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapTest : MonoBehaviour
{

    public GameObject sprite;

    // Start is called before the first frame update
    void Start () {
  		MapController mappie = new MapController(20, 20);
      mappie.makeList();
      mappie.placeBuildings();
  		//mappie.printMap();
      char [,] myMap = mappie.getMap();

      GenerateMap (myMap);
  	}

    // Update is called once per frame
    void Update()
    {

    }

    void GenerateMap (char [,] map)
    {
      for (int x = 0; x < 20; x++)
      {
        for (int y = 0; y < 20; y++)
        {
          if (map [x, y] == 'B')
          {
            Instantiate (sprite, new Vector3 (-10 + x, -10 + y), Quaternion.identity);
          }
        }
      }
    }
}
