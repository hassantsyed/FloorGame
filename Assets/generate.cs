using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class generate : MonoBehaviour
{

    private int[,] grid;
    private List<GameObject> tiles = new List<GameObject>();
    private System.Random rand;
    public Transform floor;
    public Transform lava;
    public Transform goalfloor;
    public GameObject goal;
    private int blockCount = 99999;
    public int length;
    public int width;
    public int lavaP;
    public Material yellow;
    public Material green;
    public float time = 0;
    public bool fall = false;
    float t = 0;
    int calls;
    List<int> pos;
    
    // Use this for initialization
    void Start()
    {
        grid = new int[length, width];
        rand = new System.Random();
        goal = Instantiate(goal, new Vector3(10, 10, 10), Quaternion.identity);
        setGoal();
        //goal.GetComponent<Renderer>().material.color = Color.grayscale;

        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < length; z++)
            {
                //Vector3 position = new Vector3(x, 0, z);
                //floor = Instantiate(floor, position, Quaternion.identity);
                grid[x, z] = 0;
                
            }
        }
        
        while (blockCount > (length * width) / 2)
        {
            for (int x = 0; x < width; x++)
            {
                for (int z = 0; z < length; z++)
                {
                    grid[x, z] = 0;
                }
            }
            setGoal();
            clean();
            
            setSpawn();
            ensurePath();
            //clean();
            makeBlocks();
        }
    }
    //int speed = 0;
    
    public void Update()
    {
        
        
        //speed++;
        //
        time += Time.deltaTime;
        if(time >= 3)
        {
           // print("5 seconds");
            time = 0;
            change();
        }
        int count = 0;
        foreach(GameObject t in tiles)
        {
            if (t.CompareTag("lava"))
            {
                count++;
            }
        }
        if(count >= tiles.Count)
        {
            fall = true;
            if (fall)
            {
                foreach (GameObject t in tiles)
                {
                    if (!t.GetComponent<Rigidbody>())
                    {
                        //print(goal.transform.position.x);
                        //print(goal.transform.position.z);
                        if (!(t.transform.position.x == goal.transform.position.x && t.transform.position.z == goal.transform.position.z))
                        {
                            Rigidbody r = t.AddComponent<Rigidbody>();
                            r.AddExplosionForce(90, new Vector3(0, 5, 0), 50);
                        }
                    }
                }
            }
        }
    }

    public void change()
    {
        pos = new List<int>();
        //adds index of all lava tiles to pos
        for (int x = 0; x < tiles.Count; x++)
        {
            if (!(tiles[x] == null)) { 
                //gets position of all lava tiles
                if (tiles[x].CompareTag("lava"))
                {
                    pos.Add(x);
                }
            }
        }
        calls = pos.Count-1;
        foreach(int x in pos){
            if (!(x+1 > tiles.Count - 1 || x-1 < 0))
            {
                if (calls<pos.Count)
                {
                    if (tiles[x + 1].CompareTag("grass"))
                    {
                        StartCoroutine(colorRoutine(x+1));
                    }
                    if (tiles[x - 1].CompareTag("grass"))
                    {
                        StartCoroutine(colorRoutine(x-1));
                    }
                    if (tiles[x + 1].CompareTag("goalstand"))
                    {
                        tiles[x+1].tag = "lava";
                    }
                    if (tiles[x - 1].CompareTag("goalstand"))
                    {
                        tiles[x - 1].tag = "lava";
                    }


                }
            }
        }
        
    }

    IEnumerator colorRoutine(int x)
    {
        
        t = 1;
        while (t>0)
        {
            t -= Time.deltaTime;
            //tiles[x].GetComponent<Renderer>().material.color = Color.Lerp(green.color, yellow.color, 1-t);
            //tiles[x].GetComponent<Renderer>().material.color = Color.Lerp(green.color, yellow.color, 1-t);
            tiles[x].GetComponent<Renderer>().material.color = Color.Lerp(Color.black, yellow.color, 1 - t);
            tiles[x].GetComponent<Renderer>().material.color = Color.Lerp(Color.black, yellow.color, 1-t);
            yield return new WaitForSeconds(.01f);
            yield return null;
        }
        tiles[x].tag = "lava";
        calls--;


    }


    public void clean()
    {
        fall = false;
        foreach (GameObject x in tiles)
        {
            Destroy(x);
        }
    }
    public void makeBlocks()
    {
        tiles = new List<GameObject>();
        grid[(int)goal.transform.position.x, (int)goal.transform.position.z] = 3;
        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < length; z++)
            {
                if (grid[x, z] == 1)
                {
                    tiles.Add(Instantiate(floor, new Vector3(x, 0, z), Quaternion.identity).gameObject);
                    tiles[tiles.Count - 1].GetComponent<Renderer>().material.color = green.color;
                    tiles[tiles.Count - 1].GetComponent<Renderer>().material.color = Color.black;
                }
                else if(grid[x,z] == 2)
                {
                    tiles.Add(Instantiate(lava, new Vector3(x, 0, z), Quaternion.identity).gameObject);
                }
                else if (grid[x, z] == 3)
                {
                    tiles.Add(Instantiate(goalfloor, new Vector3(x, 0, z), Quaternion.identity).gameObject);
                }
            }
        }

    }
    public void reset()
    {        
        setGoal();
        blockCount = 99999;
        //set entire grid to be off
        while (blockCount > (length * width) / 2)
        {
            for (int x = 0; x < width; x++)
            {
                for (int z = 0; z < length; z++)
                {
                    grid[x, z] = 0;
                }
            }
            grid[(int)goal.transform.position.x, (int)goal.transform.position.z] = 1;
            setSpawn();
            ensurePath();
            clean();
            makeBlocks();
            time = 0;
        }
    }

    public void setGoal()
    {
        int x = rand.Next(2, width);
        int z = rand.Next(2, length);
        goal.transform.position = new Vector3 (x, .75f, z);
        if (grid[x, z] == 0)
        {
            grid[x, z] = 3;
        }
    }
    public void setSpawn()
    {

        grid[0, 0] = 1;
        grid[0, 1] = 1;
        grid[1, 0] = 1;
        grid[1, 1] = 1;
    }

    public void ensurePath()
    {
        blockCount = 0;
        //create random path back to player
        bool path = false;
        int goalXpos = (int)goal.transform.position.x;
        int goalZpos = (int)goal.transform.position.z;
        int currentX = goalXpos;
        int currentZ = goalZpos;

        while (!path)
        {
            switch (rand.Next(4))
            {
                case 0: //north
                    {
                        if (checkBarrier(0, currentX))
                        {
                            currentX--;
                            determineTile(currentX, currentZ);
                            //grid[currentX, currentZ] = 2;
                            blockCount++;
                        }
                        break;
                    }
                case 1: //east
                    {
                        if (checkBarrier(1, currentZ))
                        {
                            currentZ++;
                            determineTile(currentX, currentZ);
                           // grid[currentX, currentZ] = 2;
                            blockCount++;
                        }
                        break;
                    }
                case 2: //south
                    {
                        if (checkBarrier(2, currentX))
                        {
                            currentX++;
                            determineTile(currentX, currentZ);
                            //grid[currentX, currentZ] = 2;
                            blockCount++;
                        }
                        break;
                    }
                case 3: //west
                    {
                        if (checkBarrier(3, currentZ))
                        {
                            currentZ--;
                            determineTile(currentX, currentZ);
                            //grid[currentX, currentZ] = 1;
                            blockCount++;
                        }
                        break;
                    }
            }
            if ((currentX == 0 && (currentZ == 1 || currentZ == 0)) || (currentX == 1 && (currentZ == 1 || currentZ == 0)))//condition that a path is there
            {
                path = true;
            }
        }


    }

    public bool checkBarrier(int direction, int current)
    {
        if (direction == 0)
        {
            //if out of top bounds
            if (current - 1 < 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        if (direction == 1)
        {
            //doesn't exceed close bound
            if (current + 1 > grid.GetLength(0) - 1)
            {
                return false;
            }
            else
            {
                return true;
            }

        }

        if (direction == 2)
        {
            //doesn't exceed bottom
            if (current + 1 > grid.GetLength(0) - 1)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        if (direction == 3)
        {
            //if too far west
            if (current - 1 < 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        print("this should never print, in checkBarriers");
        return false;
    }

    public void determineTile(int x, int z)
    {
        if (rand.Next(100) < lavaP)
        {
            grid[x, z] = 2;
            //print(2);
        }
        else
        {
            grid[x, z] = 1;
            //print(1);
        }
    }
}
