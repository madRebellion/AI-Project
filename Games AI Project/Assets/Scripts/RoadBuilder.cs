using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadBuilder : MonoBehaviour {

    public GameObject startPiece;
    public int roadsUp;
    public int roadsDown;
    public int roadsRight;
    public int roadsLeft;
    public GameObject[,] roadObjects;
    public int[,] roadRotates; //0 = no rotate, 1 = Clockwise 90, 2 = 180, 3 = 270 clockwise
    public int[,] roadExits; //Binary - 1 = has exit, 0 = has no exit - MSB (Most Significant Bit) = UP, 2nd MSB = LEFT, 3rd MSB = DOWN, LSB (Least SB) = RIGHT !NOTE! Added a 5th digit as new MSB so that taking a value away would no longer lose a 0 MSB
    public int[,] roadState; //State 0 = Available, State 1 = In Use, State 2 = Checked
    public int[,] smoothState; //State 0 = No Tile, State 1 = Unset tile, State 2 = Set Tile
    public int tileX = -1, tileY = -1;

    public GameObject[] tiles;
    public GameObject road;
    public GameObject[] lot;
    public GameObject ground;
    public Renderer grassText;


    // Use this for initialization
    void Awake()
    {
        //Setting the First Tiles Properties
        roadObjects = new GameObject[roadsLeft + roadsRight + 1, roadsUp + roadsDown + 1];
        roadRotates = new int[roadsLeft + roadsRight + 1, roadsUp + roadsDown + 1];
        roadExits = new int[roadsLeft + roadsRight + 1, roadsUp + roadsDown + 1];
        roadState = new int[roadsLeft + roadsRight + 1, roadsUp + roadsDown + 1];
        smoothState = new int[roadsLeft + roadsRight + 1, roadsUp + roadsDown + 1];
        for (int i = 0; i <= roadsLeft + roadsRight; i++)
        {
            for (int j = 0; j <= roadsUp + roadsDown; j++)
            {
                roadObjects[i, j] = null;
                roadRotates[i, j] = 0;
                roadExits[i, j] = 11111;
                roadState[i, j] = 0;
                smoothState[i, j] = 0;
            }
        }
        roadObjects[roadsLeft, roadsUp] = startPiece;
        roadRotates[roadsLeft, roadsUp] = 0;
        roadExits[roadsLeft, roadsUp] = 11111;
        roadState[roadsLeft, roadsUp] = 1;

        ground.GetComponent<Transform>().localScale = new Vector3(10 + (10 * (roadsLeft + roadsRight + 1)) + (10 * (Mathf.Abs(roadsLeft - roadsRight))), 1, 10 + (10 * (roadsUp + roadsDown + 1)) + (10 * (Mathf.Abs(roadsUp - roadsDown))));
        grassText.material.mainTextureScale = new Vector2(10 + (10 * (roadsLeft + roadsRight + 1)) + (10 * (Mathf.Abs(roadsLeft - roadsRight))), 10 + (10 * (roadsUp + roadsDown + 1)) + (10 * (Mathf.Abs(roadsUp - roadsDown))));

        checkGrid();
    }


    int checkUp(int exits)
    {
        int temp = exits;
        temp = temp / 1000;
        temp = temp % 10;
        return temp;
    }
    int checkRight(int exits)
    {
        int temp = exits;
        temp = temp / 100;
        temp = temp % 10;
        return temp;
    }
    int checkDown(int exits)
    {
        int temp = exits;
        temp = temp / 10;
        temp = temp % 10;
        return temp;
    }
    int checkLeft(int exits)
    {
        int temp = exits;
        temp = temp % 10;
        return temp;
    }

    void checkGrid()
    {
        for (int i = 0; i <= roadsLeft + roadsRight; i++)
        {
            for (int j = 0; j <= roadsUp + roadsDown; j++)
            {
                if (tileX == -1 && tileY == -1 && roadState[i, j] == 1)
                {
                    tileX = i;
                    tileY = j;
                }
            }
        }
        if (tileX == -1 && tileY == -1)
        {


            //instantiateGrid();
            smoothgrid();
            //Done
        }
        else
        {
            
            buildGrid();
            
        }
    }

    void buildGrid()
    {

        if (tileY < roadsDown + roadsUp)
        {
            if (checkUp(roadExits[tileX, tileY]) == 1 && roadState[tileX, tileY + 1] == 0)
            {
                int temp = Random.Range(0, 4);
                roadObjects[tileX, tileY + 1] = tiles[temp];
                roadState[tileX, tileY + 1] = 1;
                if (temp == 1)
                {
                    int rotTemp = Random.Range(0, 2);
                    if (rotTemp == 0)
                    {
                        roadExits[tileX, tileY + 1] = 10111;
                    }
                    else if (rotTemp == 1)
                    {
                        roadRotates[tileX, tileY + 1] = 1;
                        roadExits[tileX, tileY + 1] = 11011;
                    }
                    else if (rotTemp == 2)
                    {
                        roadRotates[tileX, tileY + 1] = 3;
                        roadExits[tileX, tileY + 1] = 11110;
                    }
                }
                else if (temp == 2)
                {
                    roadExits[tileX, tileY + 1] = 11010;
                }
                else if (temp == 3)
                {
                    int rotTemp = Random.Range(0, 1);
                    if (rotTemp == 0)
                    {
                        roadExits[tileX, tileY + 1] = 10110;
                    }
                    else if (rotTemp == 1)
                    {
                        roadRotates[tileX, tileY + 1] = 1;
                        roadExits[tileX, tileY + 1] = 10011;
                    }
                }
                else if (temp == 4)
                {
                    roadExits[tileX, tileY + 1] = 10010;
                }
            }
        }
        if (tileX < roadsRight + roadsLeft)
        {
            if (checkRight(roadExits[tileX, tileY]) == 1 && roadState[tileX + 1, tileY] == 0)
            {
                int temp = Random.Range(0, 4);
                roadObjects[tileX + 1, tileY] = tiles[temp];
                roadState[tileX + 1, tileY] = 1;
                if (temp == 1)
                {
                    int rotTemp = Random.Range(0, 2);
                    if (rotTemp == 0)
                    {
                        roadExits[tileX + 1, tileY] = 10111;
                    }
                    else if (rotTemp == 1)
                    {
                        roadRotates[tileX + 1, tileY] = 1;
                        roadExits[tileX + 1, tileY] = 11011;
                    }
                    else if (rotTemp == 2)
                    {
                        roadRotates[tileX + 1, tileY] = 2;
                        roadExits[tileX + 1, tileY] = 11101;
                    }
                }
                else if (temp == 2)
                {
                    roadRotates[tileX + 1, tileY] = 1;
                    roadExits[tileX + 1, tileY] = 10101;
                }
                else if (temp == 3)
                {
                    int rotTemp = Random.Range(0, 1);
                    if (rotTemp == 0)
                    {
                        roadRotates[tileX + 1, tileY] = 1;
                        roadExits[tileX + 1, tileY] = 10011;
                    }
                    else if (rotTemp == 1)
                    {
                        roadRotates[tileX + 1, tileY] = 2;
                        roadExits[tileX + 1, tileY] = 11001;
                    }
                }
                else if (temp == 4)
                {
                    roadRotates[tileX + 1, tileY] = 1;
                    roadExits[tileX + 1, tileY] = 10001;
                }
            }
        }
        if (tileY > 0 )
        {
            if (checkDown(roadExits[tileX, tileY]) == 1 && roadState[tileX, tileY - 1] == 0)
            {
                int temp = Random.Range(0, 4);
                roadObjects[tileX, tileY - 1] = tiles[temp];
                roadState[tileX, tileY - 1] = 1;
                if (temp == 1)
                {
                    int rotTemp = Random.Range(0, 2);
                    if (rotTemp == 0)
                    {
                        roadRotates[tileX, tileY - 1] = 1;
                        roadExits[tileX, tileY - 1] = 11011;
                    }
                    else if (rotTemp == 1)
                    {
                        roadRotates[tileX, tileY - 1] = 2;
                        roadExits[tileX, tileY - 1] = 11101;
                    }
                    else if (rotTemp == 2)
                    {
                        roadRotates[tileX, tileY - 1] = 3;
                        roadExits[tileX, tileY - 1] = 11110;
                    }
                }
                else if (temp == 2)
                {
                    roadExits[tileX, tileY - 1] = 11010;
                }
                else if (temp == 3)
                {
                    int rotTemp = Random.Range(0, 1);
                    if (rotTemp == 0)
                    {
                        roadRotates[tileX, tileY - 1] = 2;
                        roadExits[tileX, tileY - 1] = 11001;
                    }
                    else if (rotTemp == 1)
                    {
                        roadRotates[tileX, tileY - 1] = 3;
                        roadExits[tileX, tileY - 1] = 11100;
                    }
                }
                else if (temp == 4)
                {
                    roadRotates[tileX, tileY - 1] = 2;
                    roadExits[tileX, tileY - 1] = 11000;
                }
            }
        }
        if (tileX > 0)
        {
            if (checkLeft(roadExits[tileX, tileY]) == 1 && roadState[tileX - 1, tileY] == 0)
            {
                int temp = Random.Range(0, 4);
                roadObjects[tileX - 1, tileY] = tiles[temp];
                roadState[tileX - 1, tileY] = 1;
                if (temp == 1)
                {
                    int rotTemp = Random.Range(0, 2);
                    if (rotTemp == 0)
                    {
                        roadExits[tileX - 1, tileY] = 10111;
                    }
                    else if (rotTemp == 1)
                    {
                        roadRotates[tileX - 1, tileY] = 2;
                        roadExits[tileX - 1, tileY] = 11101;
                    }
                    else if (rotTemp == 2)
                    {
                        roadRotates[tileX - 1, tileY] = 3;
                        roadExits[tileX - 1, tileY] = 11110;
                    }
                }
                else if (temp == 2)
                {
                    roadRotates[tileX - 1, tileY] = 1;
                    roadExits[tileX - 1, tileY] = 10101;
                }
                else if (temp == 3)
                {
                    int rotTemp = Random.Range(0, 1);
                    if (rotTemp == 0)
                    {
                        roadExits[tileX - 1, tileY] = 10110;
                    }
                    else if (rotTemp == 1)
                    {
                        roadRotates[tileX - 1, tileY] = 3;
                        roadExits[tileX - 1, tileY] = 11100;
                    }
                }
                else if (temp == 4)
                {
                    roadRotates[tileX - 1, tileY] = 3;
                    roadExits[tileX - 1, tileY] = 10100;
                }
            }
        }
        roadState[tileX, tileY] = 2;
        tileX = -1;
        tileY = -1;
        checkGrid();
    }

    void smoothgrid()
    {
        for (int i = 0; i <= roadsLeft + roadsRight; i++)
        {
            for (int j = 0; j <= roadsUp + roadsDown; j++)
            {
                if (roadState[i, j] == 2)
                {
                    smoothState[i, j] = 1;
                }
            }
        }
        for (int i = 0; i <= roadsLeft + roadsRight; i++)
        {
            for (int j = 0; j <= roadsUp + roadsDown; j++)
            {
                int preTemp = 0;
                {
                    if (checkUp(roadExits[i, j]) == 1)
                    {
                        preTemp++;
                    }
                    if (checkRight(roadExits[i, j]) == 1)
                    {
                        preTemp++;
                    }
                    if (checkDown(roadExits[i, j]) == 1)
                    {
                        preTemp++;
                    }
                    if (checkLeft(roadExits[i, j]) == 1)
                    {
                        preTemp++;
                    }
                }
                if (j < (roadsUp + roadsDown) && smoothState[i, j] == 1)
                {
                    if (smoothState[i, j + 1] == 0 && checkUp(roadExits[i, j]) == 1)
                    {
                        roadExits[i, j] = roadExits[i, j] - 1000;

                    } else if(smoothState[i, j + 1] == 1)
                    {
                        if(checkUp(roadExits[i, j]) == 1 && checkDown(roadExits[i, j + 1]) == 0 && Random.Range(0, 1) == 0)
                        {
                            roadExits[i, j] = roadExits[i, j] - 1000;
                        } else if(checkUp(roadExits[i, j]) == 0 && checkDown(roadExits[i, j + 1]) == 1 && Random.Range(0, 1) == 1)
                        {
                            roadExits[i, j] = roadExits[i, j] + 1000;
                        }

                    }
                    else if(smoothState[i, j + 1] == 2 && checkDown(roadExits[i, j + 1]) == 1)
                    {
                        if (checkUp(roadExits[i, j]) != 1)
                        {
                            roadExits[i, j] = roadExits[i, j] + 1000;
                        }
                    } else if(smoothState[i, j + 1] == 2 && checkDown(roadExits[i, j + 1]) == 0)
                    {
                        if (checkUp(roadExits[i, j]) == 1)
                        {
                            roadExits[i, j] = roadExits[i, j] - 1000;
                        }
                    }
                }
                else if (j >= (roadsUp + roadsDown))
                {
                    if (checkUp(roadExits[i, j]) == 1)
                    {
                        roadExits[i, j] = roadExits[i, j] - 1000;
                    }
                }
                if (j > 0 && smoothState[i, j] == 1)
                {
                    if (smoothState[i, j - 1] == 0 && checkDown(roadExits[i, j]) == 1)
                    {
                        roadExits[i, j] = roadExits[i, j] - 10;

                    }
                    else if (smoothState[i, j - 1] == 1)
                    {
                        if (checkDown(roadExits[i, j]) == 1 && checkUp(roadExits[i, j - 1]) == 0 && Random.Range(0, 1) == 0)
                        {
                            roadExits[i, j] = roadExits[i, j] - 10;
                        }
                        else if (checkDown(roadExits[i, j]) == 0 && checkUp(roadExits[i, j - 1]) == 1 && Random.Range(0, 1) == 1)
                        {
                            roadExits[i, j] = roadExits[i, j] + 10;
                        }

                    }
                    else if (smoothState[i, j - 1] == 2 && checkUp(roadExits[i, j - 1]) == 1)
                    {
                        if (checkDown(roadExits[i, j]) != 1)
                        {
                            roadExits[i, j] = roadExits[i, j] + 10;
                        }
                    }
                    else if (smoothState[i, j - 1] == 2 && checkUp(roadExits[i, j - 1]) == 0)
                    {
                        if (checkDown(roadExits[i, j]) == 1)
                        {
                            roadExits[i, j] = roadExits[i, j] - 10;
                        }
                    }
                }
                else if (j <= 0)
                {
                    if (checkDown(roadExits[i, j]) == 1)
                    {
                        roadExits[i, j] = roadExits[i, j] - 10;
                    }
                }
                if (i > 0 && smoothState[i, j] == 1)
                {
                    if (smoothState[i - 1, j] == 0 && checkLeft(roadExits[i, j]) == 1)
                    {
                        roadExits[i, j] = roadExits[i, j] - 1;

                    }
                    else if (smoothState[i - 1, j] == 1)
                    {
                        if (checkLeft(roadExits[i, j]) == 1 && checkRight(roadExits[i - 1, j]) == 0 && Random.Range(0, 1) == 0)
                        {
                            roadExits[i, j] = roadExits[i, j] - 1;
                        }
                        else if (checkLeft(roadExits[i, j]) == 0 && checkRight(roadExits[i - 1, j]) == 1 && Random.Range(0, 1) == 1)
                        {
                            roadExits[i, j] = roadExits[i, j] + 1;
                        }

                    }
                    else if (smoothState[i - 1, j] == 2 && checkRight(roadExits[i - 1, j]) == 1)
                    {
                        if (checkLeft(roadExits[i, j]) != 1)
                        {
                            roadExits[i, j] = roadExits[i, j] + 1;
                        }
                    }
                    else if (smoothState[i - 1, j] == 2 && checkRight(roadExits[i - 1, j]) == 0)
                    {
                        if (checkLeft(roadExits[i, j]) == 1)
                        {
                            roadExits[i, j] = roadExits[i, j] - 1;
                        }
                    }
                }
                else if(i <= 0)
                {
                    if (checkLeft(roadExits[i, j]) == 1)
                    {
                        roadExits[i, j] = roadExits[i, j] - 1;
                    }
                }
                if (i < (roadsLeft + roadsRight) && smoothState[i, j] == 1)
                {
                    if (smoothState[i + 1, j] == 0 && checkRight(roadExits[i, j]) == 1)
                    {
                        roadExits[i, j] = roadExits[i, j] - 100;

                    }
                    else if (smoothState[i + 1, j] == 1)
                    {
                        if (checkRight(roadExits[i, j]) == 1 && checkLeft(roadExits[i + 1, j]) == 0 && Random.Range(0, 1) == 0)
                        {
                            roadExits[i, j] = roadExits[i, j] - 100;
                        }
                        else if (checkRight(roadExits[i, j]) == 0 && checkLeft(roadExits[i + 1, j]) == 1 && Random.Range(0, 1) == 1)
                        {
                            roadExits[i, j] = roadExits[i, j] + 100;
                        }

                    }
                    else if (smoothState[i + 1, j] == 2 && checkLeft(roadExits[i + 1, j]) == 1)
                    {
                        if (checkRight(roadExits[i, j]) != 1)
                        {
                            roadExits[i, j] = roadExits[i, j] + 100;
                        }
                    }
                    else if (smoothState[i + 1, j] == 2 && checkLeft(roadExits[i + 1, j]) == 0)
                    {
                        if (checkRight(roadExits[i, j]) == 1)
                        {
                            roadExits[i, j] = roadExits[i, j] - 100;
                        }
                    }
                }
                else if(i >= (roadsLeft + roadsRight))
                {
                    if (checkRight(roadExits[i, j]) == 1)
                    {
                        roadExits[i, j] = roadExits[i, j] - 100;
                    }
                }
                if (smoothState[i, j] == 1)
                {
                    smoothState[i, j] = 2;
                }
                int temp = 0;
                if (checkUp(roadExits[i, j]) == 1)
                {
                    temp++;
                }
                if (checkRight(roadExits[i, j]) == 1)
                {
                    temp++;
                }
                if (checkDown(roadExits[i, j]) == 1)
                {
                    temp++;
                }
                if (checkLeft(roadExits[i, j]) == 1)
                {
                    temp++;
                }
                if (temp != preTemp)
                {
                    if (roadExits[i, j] == 10000 || roadExits[i, j] == 0)
                    {
                        roadState[i, j] = 0;
                    }
                    else if (roadExits[i,j] == 10001)
                    {
                        roadObjects[i, j] = tiles[4];
                        roadRotates[i, j] = 1;
                    }
                    else if (roadExits[i, j] == 10010)
                    {
                        roadObjects[i, j] = tiles[4];
                        roadRotates[i, j] = 0;
                    }
                    else if (roadExits[i, j] == 10100)
                    {
                        roadObjects[i, j] = tiles[4];
                        roadRotates[i, j] = 3;
                    }
                    else if (roadExits[i, j] == 11000)
                    {
                        roadObjects[i, j] = tiles[4];
                        roadRotates[i, j] = 2;
                    }
                    else if (roadExits[i, j] == 10011)
                    {
                        roadObjects[i, j] = tiles[3];
                        roadRotates[i, j] = 1;
                    }
                    else if (roadExits[i, j] == 10110)
                    {
                        roadObjects[i, j] = tiles[3];
                        roadRotates[i, j] = 0;
                    }
                    else if (roadExits[i, j] == 11100)
                    {
                        roadObjects[i, j] = tiles[3];
                        roadRotates[i, j] = 3;
                    }
                    else if (roadExits[i, j] == 11001)
                    {
                        roadObjects[i, j] = tiles[3];
                        roadRotates[i, j] = 2;
                    }
                    else if (roadExits[i, j] == 11010)
                    {
                        roadObjects[i, j] = tiles[2];
                        roadRotates[i, j] = 0;
                    }
                    else if (roadExits[i, j] == 10101)
                    {
                        roadObjects[i, j] = tiles[2];
                        roadRotates[i, j] = 1;
                    }
                    else if (roadExits[i, j] == 10111)
                    {
                        roadObjects[i, j] = tiles[1];
                        roadRotates[i, j] = 0;
                    }
                    else if (roadExits[i, j] == 11110)
                    {
                        roadObjects[i, j] = tiles[1];
                        roadRotates[i, j] = 3;
                    }
                    else if (roadExits[i, j] == 11101)
                    {
                        roadObjects[i, j] = tiles[1];
                        roadRotates[i, j] = 2;
                    }
                    else if (roadExits[i, j] == 11011)
                    {
                        roadObjects[i, j] = tiles[1];
                        roadRotates[i, j] = 1;
                    }
                }
            }
        }
        instantiateGrid();
    }

    void instantiateGrid()
    {

        for (int i = 0; i <= roadsLeft + roadsRight; i++)
        {
            for (int j = 0; j <= roadsUp + roadsDown; j++)
            {
                if (roadState[i, j] == 2)
                {
                    GameObject g = Instantiate(roadObjects[i, j], new Vector3((i * 100) - (roadsLeft * 100), 0.02f, (j * 100) - (roadsUp *100)), Quaternion.Euler(0, roadRotates[i, j] * 90, 0));
                    if (checkUp(roadExits[i,j]) == 1)
                    {
                        GameObject r = Instantiate(road, new Vector3((i * 100) - (roadsLeft * 100), 0.02f, (j * 100) + 50 - (roadsUp * 100)), Quaternion.Euler(0, 0, 0));
                    }
                    if (checkLeft(roadExits[i, j]) == 1)
                    {
                        GameObject r = Instantiate(road, new Vector3((i * 100) - 50 - (roadsLeft * 100), 0.02f, (j * 100) - (roadsUp * 100)), Quaternion.Euler(0, 90, 0));
                    }
                    
                }
            }
        }
        lotPlotter();
    }

    public bool[,] lotState; //false = no lot, true = lot

    void lotPlotter()
    {
        lotState = new bool[roadsLeft + roadsRight + 2, roadsUp + roadsDown + 2];
        for (int i = 0; i <= roadsLeft + roadsRight + 1; i++)
        {
            for (int j = 0; j <= roadsUp + roadsDown + 1; j++)
            {
                lotState[i, j] = false;
                if (i == 0 && j == 0 || i == 0 && j == roadsUp + roadsDown + 1 || i == roadsLeft + roadsRight + 1  && j == 0 || i == roadsLeft + roadsRight + 1 && j == roadsUp + roadsDown + 1)
                {
                    //Do Nothing, this checks if it's on the edge of the map (if it is then it shouldn't be checked to avoid Array out of range)
                }
                else if (i == 0)
                {
                    if (roadState[i,j - 1] == 2 && checkUp(roadExits[i,j - 1]) == 1)
                    {
                        lotState[i, j] = true;
                    }
                } else if (i == roadsLeft + roadsRight + 1)
                {
                    if (roadState[i - 1, j] == 2 && checkDown(roadExits[i - 1, j]) == 1)
                    {
                        lotState[i, j] = true;
                    }
                } else if (j == 0)
                {
                    if (roadState[i - 1, j] == 2 && checkRight(roadExits[i - 1, j] ) == 1)
                    {
                        lotState[i, j] = true;
                    }
                } else if (j == roadsUp + roadsDown + 1)
                {
                    if (roadState[i, j - 1] == 2 && checkLeft(roadExits[i, j - 1]) == 1)
                    {
                        lotState[i, j] = true;
                    }
                } else
                {
                    if (roadState[i, j - 1] == 2 && checkUp(roadExits[i, j - 1]) == 1 || roadState[i, j - 1] == 2 && checkLeft(roadExits[i, j - 1]) == 1 || roadState[i - 1, j] == 2 && checkDown(roadExits[i - 1, j]) == 1 || roadState[i - 1, j] == 2 && checkRight(roadExits[i - 1, j]) == 1)
                    {
                        lotState[i, j] = true;
                    }
                }
            }
        }

        for (int i = 0; i <= roadsLeft + roadsRight + 1; i++)
        {
            for (int j = 0; j <= roadsUp + roadsDown + 1; j++)
            {
                if (lotState[i, j] == true)
                {
                    int type = Random.Range(0, 4);
                    if (type == 0)
                    {
                        GameObject g = Instantiate(lot[type], new Vector3((i * 100) - 50 - (roadsLeft * 100), 0.02f, (j * 100) - 50 - (roadsUp * 100)), Quaternion.Euler(0, 0, 0));
                    } else if (type == 1 || type == 2 || type == 3)
                    {
                        GameObject g = Instantiate(lot[type], new Vector3((i * 100) - 50 - (roadsLeft * 100), -.6f, (j * 100) - 50 - (roadsUp * 100)), Quaternion.Euler(0, 0, 0));
                    }
                }
            }
        }
    }
}
