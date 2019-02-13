using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapController{

    //static vars that should not change in the code.
    public static int MIN_BLDGS = 5;
    public static int MAX_BLDGS = 15;
    public static int MIN_BLDG_SIZE = 3;
    public static int MAX_BLDG_SIZE = 6;
    //Some variables
    int height;
    int width;
    private char [,] map;

    //Constructor
    public MapController (int mapHeight, int mapWidth){
      height = mapHeight;
      width = mapWidth;
      map = new char [width, height];
    }

    //This returns a list width by height that is filled with #
    public void makeList (){
			//list of lists right here.
			for (int i = 0; i < width; i++){
					for (int j = 0; j < height; j++){
							map[i,j] = '#';
				}
			}
		}

    //This generates a randomly-sized bldg and returns an array of
    // [x, y, size]. All buildings are square for now.
    public int [] makeBuilding(){
      int [] origin = new int [] {Random.Range(1, width - 1), Random.Range(1, height - 1)};
      int x = origin [0];
      int y = origin [1];

      int size = Random.Range(MIN_BLDG_SIZE, MAX_BLDG_SIZE);

      if (origin[0] + size >= width || origin[1] + size >= height){
        //if we went off the space, try another time.
        return makeBuilding ();
      }

      return (new int [] {x, y, size});
    }

    public bool collides(int [] bldg1, int [] bldg2){
      //Checks whether there is overlap between two rectangular bldgs by checking each
      //corner for each building.
        bool coll = false;

        int bldg1_xCorner1 = bldg1[0];
        int bldg1_xCorner2 = bldg1[0] + bldg1[2];
        int bldg1_yCorner1 = bldg1[1];
        int bldg1_yCorner2 = bldg1[1] + bldg1[2];

        int bldg2_xCorner1 = bldg2[0];
        int bldg2_xCorner2 = bldg2[0] + bldg2[2];
        int bldg2_yCorner1 = bldg2[1];
        int bldg2_yCorner2 = bldg2[1] + bldg2[2];

        // check for an overlap between the four points of bldg2 with bldg1
        // bldg2 (x1,y1)
        if ((bldg1_xCorner1 <= bldg2_xCorner1 && bldg2_yCorner1 <= bldg1_xCorner2) &&
        (bldg1_yCorner1 <= bldg2_yCorner1 && bldg2_yCorner1 <= bldg1_yCorner2)){
          coll = true;
        }
        // bldg2 (x1,y2)
        else if ((bldg1_xCorner1 <= bldg2_xCorner1 && bldg2_xCorner1 <= bldg1_xCorner2) &&
        (bldg1_yCorner1 <= bldg2_yCorner2 && bldg2_yCorner2 <= bldg1_yCorner2)){
          coll = true;
        }
        // bldg2 (x2,y1)
        else if ((bldg1_xCorner1 <= bldg2_xCorner2 && bldg2_xCorner2 <= bldg1_xCorner2) &&
        (bldg1_yCorner1 <= bldg2_yCorner1 && bldg2_yCorner1 <= bldg1_yCorner2)){
          coll = true;
        }

        // bldg2 (x2, y2)
        else if ((bldg1_xCorner1 <= bldg2_xCorner2 && bldg2_xCorner2 <= bldg1_xCorner2) &&
        (bldg1_yCorner1 <= bldg2_yCorner2 && bldg2_yCorner2 <= bldg1_yCorner2)){
          coll = true;
        }

        // check for an overlap between the four points of bldg1 with bldg2
        if ((bldg2_xCorner1 <= bldg1_xCorner1 && bldg1_xCorner1 <= bldg2_xCorner2) &&
        (bldg2_yCorner1 <= bldg1_yCorner1 && bldg1_yCorner1 <= bldg2_yCorner2)){
          coll = true;
        }

        // bldg2 (x1,y2)
        else if (bldg2_xCorner1 <= bldg1_xCorner1 && bldg1_yCorner1 <= bldg2_xCorner2 &&
        bldg2_yCorner1 <= bldg1_yCorner2 && bldg1_yCorner2 <= bldg2_yCorner2){
          coll = true;
        }

        // bldg2 (x2,y1)
        else if (bldg2_xCorner1 <= bldg1_xCorner2 && bldg1_xCorner2 <= bldg2_xCorner2 &&
        bldg2_yCorner1 <= bldg1_yCorner1 && bldg1_xCorner1 <= bldg2_yCorner2){
          coll = true;
        }

        // bldg2 (x2, y2)
        else if (bldg2_xCorner1 <= bldg1_xCorner2 && bldg1_xCorner1 <= bldg2_xCorner2 &&
        bldg2_yCorner1 <= bldg1_yCorner2 && bldg1_xCorner2 <= bldg2_yCorner2){
          coll = true;
        }

        return coll;
    }
      //In this method, we make a list of buildings using our previous functions.
      //Then we place it in our map.
      public void placeBuildings(){

        List<int []> bldgs = new List<int[]>();
        int numBldgs = Random.Range(MIN_BLDGS, MAX_BLDGS);
        int counter = 0;
        //Generates all of the buildings. the numBldgs is defined above
        while (numBldgs > 0){

          bool coll = false;
          int [] newBldg = makeBuilding();
          //We can just add the first building.
          if (bldgs.Count == 0){
            bldgs.Add(newBldg);

          //We'll need to check any existing buildings
          } else {
            foreach (int [] currentBldg in bldgs){
              if (collides(newBldg, currentBldg)){
                coll = true;
              }
            }//Ends the foreach

            if (!coll){
               bldgs.Add(newBldg);
               numBldgs = numBldgs - 1;
               counter = 0;
            } else {
              counter = counter + 1;
            }

            if (counter >= 50){
              numBldgs = -100; //If you try more than 10 times, give up.
            }

          } //End the else here

        } //here is the end of the first while.

        //here we change our map to reflect the placement of our new bldgs.
        foreach (int [] currentBldg in bldgs){
            int xCorner = currentBldg[0];
            int yCorner = currentBldg[1];
            int size = currentBldg[2];

            for (int i = xCorner; i < xCorner + size; i++){
              for (int j = yCorner; j < yCorner + size; j++){
                map[i,j] = 'B';
              }
            }
        }

      }

      //I bet you can't guess what printMap() does!
      public void printMap(){
        string c;
        for (int i = 0; i < width; i++){
          c = "";
          for (int j = 0; j < height; j++){
            c += map[i,j];
          }
          Debug.Log(c);
        }
      }

      public char [,] getMap(){
          return map;
      }
}
