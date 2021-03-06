﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

//THIS IS AN OUTDATES VERSION OF THE ISLAND CREATION PIPELINE
//Please refer to IslandMaker.cs for up to date pipeline

public class PentInfo : MonoBehaviour
{
    //basic pentgon mesh making
	public Vector3[] pentVerts;
	public Vector2[] uv;
	public int[] Triangles;
    public Mesh mesh;
    public GameObject InputBit;
    public Material PentMat, HexMat, wireframeMat;

    public float meshScale = .1f,
        hexScale = 0.2f,
    favs = 1,
   floorLevel;

    public int dirtFailures;
    GameObject flag;

    bool CreateHexs = true;
    List<GameObject> hexs = new List<GameObject>();
    List<GameObject> itemsToDestroy = new List<GameObject>();

    float h = 0, w = 0;
    int finishedIslands=0;

    #region Initialization
    void Start()
	{
        MeshSetup();
        //Fill this pentagon in with hexagons.
	}

    void MeshSetup()
    {
        #region verts
        
        Vector3[] pentVerts =
        {
            new Vector3((-0.5f-0.5f)*meshScale,floorLevel,(-0.688f-0.688f) *meshScale),//0
            new Vector3((-0.809f-0.809f)*meshScale,floorLevel,(0.263f+0.263f)*meshScale),//1
            new Vector3(0,floorLevel, (0.851f+0.851f)*meshScale),//2
            new Vector3((0.809f+0.809f)*meshScale,floorLevel,(0.263f+0.263f) *meshScale),//3
            new Vector3((0.5f+0.5f)*meshScale,floorLevel,(-0.688f-0.688f) *meshScale),//4

            
            new Vector3((-0.5f-0.5f)*meshScale,floorLevel-1,(-0.688f-0.688f) *meshScale),//0
            new Vector3((-0.809f -0.809f)*meshScale,floorLevel-1,(0.263f+0.263f)*meshScale),//1
            new Vector3(0,floorLevel-1, (0.851f+(0.851f))*meshScale),//2
            new Vector3((0.809f+0.809f)*meshScale,floorLevel-1,(0.263f+0.263f) *meshScale),//3
            new Vector3((0.5f+0.5f)*meshScale,floorLevel-1,(-0.688f-0.688f) *meshScale),//4
        };
        #endregion

        #region triangles

        int[] triangles =
       {

            9,5,4,
            4,5,0,
            8,9,4,
            4,3,8,
            3,7,8,
            2,7,3,
            2,6,7,
            6,2,1,
            1,0,6,
            5,6,0,

            4,0,1,
            4,1,3,
            1,2,3,

            6,5,9,
            8,6,9,
            8,7,6
       };
        #endregion

        #region uv
        uv = new Vector2[]
        {
            new Vector2(-3,0),
            new Vector2(0,2),
            new Vector2(3,0),
            new Vector2(2,-4),
            new Vector2(-2,4),
        };
        #endregion

        #region finalize
        MeshFilter meshFilter = gameObject.AddComponent<MeshFilter>();
        MeshRenderer meshRenderer = gameObject.AddComponent<MeshRenderer>();
        mesh = new Mesh();

        MeshCollider meshCollider = gameObject.AddComponent<MeshCollider>();

        mesh.vertices = pentVerts;
        mesh.triangles = triangles;

        //add out UV coordinates to the mesh
        //mesh.uv = uv;

        meshRenderer.material = PentMat;

        //make it play nicely with lighting
        mesh.RecalculateNormals();
        mesh.name = "Pentagonal";
        //set the GO's meshFilter's mesh to be the one we just made
        meshFilter.mesh = mesh;

        meshCollider.sharedMesh = mesh;
        meshCollider.convex = true;
        meshCollider.isTrigger = true;

        //UV TESTING
        //renderer.material.mainTexture = texture;

        #endregion
    }
    #endregion

    #region HuD and interactivity functions

    public void setPentVisibility(bool hi)
    {
        GetComponent<MeshRenderer>().enabled = hi;
        GetComponent<MeshCollider>().enabled = hi;
    }

    public void setPentScale(float newScale)
    {
        meshScale = newScale;
    }

    public void setFavs(float newFavs)
    {
            favs = newFavs;
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.tag!="Player")
         itemsToDestroy.Remove(col.gameObject);
    }
    
    public void UpdatePentMesh()
    {
        InputField tweetData = InputBit.GetComponent<InputField>();
        string inputText = tweetData.text;
        inputText = inputText.ToLower();
        char[] vowels = { 'a', 'e', 'i', 'o', 'u' };
        int[] vowelCount = new int[] { 1, 1, 1, 1, 1 };

        for (int i = 0; i < inputText.Length; i++)
            for (int v = 0; v < 5; v++)
                if (inputText[i] == vowels[v])
                    ++vowelCount[v];

        //The max amount a vowel direction should go in is 8*amount;
        int mostVowels=0;
        for (int i=0; i < vowelCount.Length;i++)
            if (vowelCount[i] > mostVowels)
                mostVowels = vowelCount[i];

        float max = (1.0f / mostVowels);

        Vector3[] NewPentVerts =
        {
            new Vector3((-0.5f-(0.5f*(vowelCount[0]*max)))*meshScale,floorLevel,(-0.688f-(0.688f*(vowelCount[0]*max))) *meshScale),//0
            new Vector3((-0.809f-(0.809f*(vowelCount[1]*max)))*meshScale,floorLevel,(0.263f+(0.263f*(vowelCount[1]*max)))*meshScale),//1
            new Vector3(0,floorLevel, (0.851f+(0.851f*(vowelCount[2]*max)))*meshScale),//2
            new Vector3((0.809f+(0.809f*(vowelCount[3]*max)))*meshScale,floorLevel,(0.263f+(0.263f*(vowelCount[3]*max))) *meshScale),//3
            new Vector3((0.5f+(0.5f*(vowelCount[4]*max)))*meshScale,floorLevel,(-0.688f-(0.688f*(vowelCount[4]*max))) *meshScale),//4

            
            new Vector3((-0.5f-(0.5f*(vowelCount[0]*max)))*meshScale,floorLevel-1,(-0.688f-(0.688f*(vowelCount[0]*max))) *meshScale),//0
            new Vector3((-0.809f -(0.809f*(vowelCount[1]*max)))*meshScale,floorLevel-1,(0.263f+(0.263f*(vowelCount[1]*max)))*meshScale),//1
            new Vector3(0,floorLevel-1, (0.851f+(0.851f*(vowelCount[2]*max)))*meshScale),//2
            new Vector3((0.809f+(0.809f*(vowelCount[3]*max)))*meshScale,floorLevel-1,(0.263f+(0.263f*(vowelCount[3]*max))) *meshScale),//3
            new Vector3((0.5f+(0.5f*(vowelCount[4]*max)))*meshScale,floorLevel-1,(-0.688f-(0.688f*(vowelCount[4]*max))) *meshScale),//4
        };
        mesh.vertices = NewPentVerts;

        //Is this line actually did something, that'd be fucking marvelous, but noooo
        //gameObject.GetComponent<MeshCollider>().sharedMesh = mesh;


        //I am destorying and remaking the collider all the time
        //If anyone find this I want them to know I'm not proud of what I've done here
        DestroyImmediate(GetComponent<MeshCollider>());
        MeshCollider meshCollider = gameObject.AddComponent<MeshCollider>();
        meshCollider.sharedMesh = mesh;
        meshCollider.convex = true;
        meshCollider.isTrigger = true;
        GetComponent<MeshFilter>().mesh = mesh;
        GameObject.Find("Particle effects").GetComponent<mood>().move(mesh.bounds.center);
    }
    #endregion

    #region islandRelated

    public void updateHexs()
    {
        for (int i = 0; i < hexs.Count; i++)
        {
            Destroy(hexs[i].gameObject);
        }
        hexs.Clear();
        CreateHexs = true;
        h = 0;
        w = 0;
        Destroy(flag);
        dirtFailures = 0;
        LargestLowestValue = 0;
        SpawnHexNEW();
    }

    void SpawnHexNEW()
    {
        Vector3 max = GetComponent<MeshCollider>().bounds.max;
        Vector3 min = GetComponent<MeshCollider>().bounds.min;

        #region Hex creation
        while (CreateHexs)
        {
            //Make a hex
            GameObject hex = new GameObject("hex " + h + "," + w);
            HexInfo hexinf = hex.AddComponent<HexInfo>();
            hexinf.mat = HexMat;
            hexinf.MeshSetup(hexScale);
            hexinf.transform.position = min;

            #region Move Hex into position
            //Move along by 1 hex
            float x = w * 1.5f * (hexScale * 6);
            float y;
            if (w % 2 != 0)//If w is odd
            {
                //move up by 1 hex
                y = h * 2 * (hexScale * Mathf.Sqrt(36 - 9));
            }
            else
            {
                //Move p by a hex and a half
                y = (h * 2 * (hexScale * Mathf.Sqrt(36 - 9))) + (hexScale * Mathf.Sqrt(36 - 9));
            }

            hex.transform.Translate(x, .5f, y);
            #endregion

            //If we've reached the edge of the pentagon bounding box
            if (hex.transform.position.x > max.x + 2 * (hexScale * Mathf.Sqrt(36 - 9)))
            {
                //Start a new line
                h++;
                w = 0;
                Destroy(hex);
            }
            else
            {
                w++;
                hexs.Add(hex);
                itemsToDestroy.Add(hex);
            }

            if (hex.transform.position.z > max.z + 2 * (hexScale * Mathf.Sqrt(36 - 9)))
            {
                CreateHexs = false;
                hexs.Remove(hex);
                Destroy(hex);
            }
        }
        #endregion

        Invoke("hexRemoval",.1f);
    }

    void hexRemoval()
    {
        for (int j = 0; j < itemsToDestroy.Count; j++)
        {
            hexs.Remove(itemsToDestroy[j]);
            Destroy(itemsToDestroy[j]);
        }
        itemsToDestroy.Clear();
        setPentVisibility(false);
        //StartCoroutine(CreateIslandDELAY());
        Invoke("CreateIsland", .5f);
    }

    [HideInInspector]
    public float LargestLowestValue = 0;

    IEnumerator CreateIslandDELAY()
    {
        if (hexs[0].GetComponent<HexInfo>().heightValue == 0)
        {
            //Find pals
            detectHexEdges();
            for (int i = 0; i < hexs.Count; i++)
            {
                //Add weightings to the island based on the number of hexs between itself and the edge of the mesh
                hexs[i].GetComponent<HexInfo>().hexWeighter(hexs.Count);
            }

            //Find the highest point on the map (the hex with the largest [least] value
            for (int i = 0; i < hexs.Count; i++)
            {
                if (hexs[i].GetComponent<HexInfo>().heightValue > LargestLowestValue)
                    LargestLowestValue = hexs[i].GetComponent<HexInfo>().heightValue;
            }
            for (int i = 0; i < hexs.Count; i++)
            {
                //Add some height to the hexs
                yield return new WaitForSeconds(0.0001f);
                hexs[i].GetComponent<HexInfo>().addHeight(LargestLowestValue, favs, hexs.Count,true);
            }
        }
        
        //Smooth island out
        for (int i = hexs.Count - 1; i > -1; i--)
        {
            yield return new WaitForSeconds(0.0001f);
            for (int j = 0; j < 7; j++)
            {
                if (hexs[i].GetComponent<HexInfo>().camp != null)
                    Destroy(hexs[i].GetComponent<HexInfo>().camp);

                hexs[i].GetComponent<HexInfo>().interlopeCorner(j);
            }
            //Update collision mesh
            hexs[i].GetComponent<MeshCollider>().sharedMesh = hexs[i].GetComponent<MeshFilter>().mesh;
        }

        //Colour the hexs based on position
        for (int i = 0; i < hexs.Count; i++)
        {
            yield return new WaitForSeconds(0.0001f);
            hexs[i].GetComponent<HexInfo>().heightColour(LargestLowestValue,false);
        }
        float highestPoint = 0;
        Vector3 flagPos = Vector3.zero;

        //Find highest point on map
        for (int i = 0; i < hexs.Count; i++)
        {
            if (hexs[i].GetComponent<HexInfo>().heightValue == LargestLowestValue)
            {
                Vector3[] points = hexs[i].GetComponent<HexInfo>().getVerts();
                for (int j = 0; j < points.Length; j++)
                {
                    if (points[j].y > highestPoint)
                    {
                        highestPoint = points[j].y;
                        flagPos = points[j] + hexs[i].transform.position;
                    }
                }
            }
        }

        //Place flag at top point!
        if (flag != null)
            Destroy(flag);

        flag = Instantiate(Resources.Load("flagpole")) as GameObject;
        flag.transform.position = flagPos + new Vector3(0, -.05f, 0);
    }

    public void CreateIsland()
    {
        if (hexs[0].GetComponent<HexInfo>().heightValue == 0)
        {
            //Find pals
            detectHexEdges();
            for (int i = 0; i < hexs.Count; i++)
            {
                //Add weightings to the island based on the number of hexs between itself and the edge of the mesh
                hexs[i].GetComponent<HexInfo>().hexWeighter(hexs.Count);
            }

            //Find the highest point on the map (the hex with the largest [least] value
            for (int i = 0; i < hexs.Count; i++)
            {
                if (hexs[i].GetComponent<HexInfo>().heightValue > LargestLowestValue)
                    LargestLowestValue = hexs[i].GetComponent<HexInfo>().heightValue;
            }
            for (int i = 0; i < hexs.Count; i++)
            {
                //Add some height to the hexs
                hexs[i].GetComponent<HexInfo>().addHeight(LargestLowestValue, favs, hexs.Count,true);
            }
        }
    
        //Smooth island out
        for (int i = hexs.Count - 1; i > -1; i--)
        {
            for (int j = 0; j < 7; j++)
            {
                if (hexs[i].GetComponent<HexInfo>().camp != null)
                    Destroy(hexs[i].GetComponent<HexInfo>().camp);

                hexs[i].GetComponent<HexInfo>().interlopeCorner(j);
            }
            //Update collision mesh
            hexs[i].GetComponent<MeshCollider>().sharedMesh = hexs[i].GetComponent<MeshFilter>().mesh;
        }

        //Colour the hexs based on position
        for (int i = 0; i < hexs.Count; i++)
            hexs[i].GetComponent<HexInfo>().heightColour(LargestLowestValue,false);

        float highestPoint = 0;
        Vector3 flagPos = Vector3.zero;

        //Find highest point on map
        for (int i = 0; i < hexs.Count; i++)
        {
            if (hexs[i].GetComponent<HexInfo>().heightValue == LargestLowestValue)
            {
                Vector3[] points = hexs[i].GetComponent<HexInfo>().getVerts();
                for (int j = 0; j < points.Length; j++)
                {
                    if (points[j].y > highestPoint)
                    {
                        highestPoint = points[j].y;
                        flagPos = points[j] + hexs[i].transform.position;
                    }
                }
            }
        }

        //Place flag at top point!
        if (flag != null)
            Destroy(flag);

        flag = Instantiate(Resources.Load("flagpole")) as GameObject;
        flag.transform.position = flagPos + new Vector3(0, -.05f, 0);
    }
    
    public void detectHexEdges()
    {
        //For each hex, raycast in each direction around them and return the gameobject hit, store as a pal
        for (int i = 0; i < hexs.Count; i++)
        {
            Vector3 hexOrigin = hexs[i].transform.position;

            Vector3 above = hexOrigin + new Vector3(0, 0, (Mathf.Sqrt(36 - 9) * hexScale) * 2);
            Vector3 below = hexOrigin - new Vector3(0, 0, (Mathf.Sqrt(36 - 9) * hexScale) * 2);
            Vector3 upperLeft = hexOrigin + new Vector3(-4.5f * hexScale * 2, 0, Mathf.Sqrt(36 - 9) * hexScale);
            Vector3 upperRight = hexOrigin + new Vector3(4.5f * hexScale * 2, 0, Mathf.Sqrt(36 - 9) * hexScale);
            Vector3 lowerRight = hexOrigin - new Vector3(-4.5f * hexScale * 2, 0, Mathf.Sqrt(36 - 9) * hexScale);
            Vector3 lowerLeft = hexOrigin - new Vector3(4.5f * hexScale * 2, 0, Mathf.Sqrt(36 - 9) * hexScale);

            RaycastHit hit;
            Ray ray;
            hexOrigin += new Vector3(0, 1, 0);
            ray = new Ray(hexOrigin, lowerLeft - hexOrigin);
            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                hexs[i].GetComponent<HexInfo>().pals[0] = hit.transform.gameObject;
            }

            ray = new Ray(hexOrigin, upperLeft - hexOrigin);
            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                hexs[i].GetComponent<HexInfo>().pals[1] = hit.transform.gameObject;
            }

            ray = new Ray(hexOrigin, above - hexOrigin);
            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                hexs[i].GetComponent<HexInfo>().pals[2] = hit.transform.gameObject;
            }

            ray = new Ray(hexOrigin, upperRight - hexOrigin);
            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                hexs[i].GetComponent<HexInfo>().pals[3] = hit.transform.gameObject;
            }

            ray = new Ray(hexOrigin, lowerRight - hexOrigin);
            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                hexs[i].GetComponent<HexInfo>().pals[4] = hit.transform.gameObject;
            }

            ray = new Ray(hexOrigin, below - hexOrigin);
            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                hexs[i].GetComponent<HexInfo>().pals[5] = hit.transform.gameObject;
            }
        }
    }

    public void blendColours()
    {
        for (int i = 0; i < hexs.Count; i++)
        {
            for (int j = 0; j < 6; j++)
            {
                //hexs[i].GetComponent<HexInfo>().blendCols(j);
            }
        }

        //For each hex
        //Get current colour
        List<List<Color>> list = new List<List<Color>>();
        for (int j = 0; j < hexs.Count; j++)
        {
            var listB = new List<Color>();
            for (int i = 0; i < 6; i++)
            {
                var hex = hexs[j].GetComponent<HexInfo>();

                int i1 = i - 1;
                if (i1 == -1)
                    i1 = 5;

                int i3 = i + 2;
                if (i3 > 5)
                    i3 -= 6;

                int i4 = i + 4;
                if (i4 > 5)
                    i4 -= 6;

                if (hex.pals[i] != null && hex.pals[i1] != null)
                {
                    var borderHex = hex.pals[i].GetComponent<HexInfo>();
                    var borderHex2 = hex.pals[i1].GetComponent<HexInfo>();
                    Color[] border2Cols = borderHex2.getColors();
                    Color[] borderCols = borderHex.getColors();


                    var newCol = (borderCols[i4] + border2Cols[i3] + hex.getColors()[i]) / 3;
                    listB.Add(newCol);
                }
                else
                {
                    listB.Add(Color.black);
                }
            }
            list.Add(listB);
        }
        //Then for each hex
        //Lerp colours for corners.
        for (int j = 0; j < hexs.Count; j++)
        {
            for (int i = 0; i < 6; i++)
            {
                var hex = hexs[j].GetComponent<HexInfo>();
                hex.moveVert(i, -99, list[j][i]);
            }
        }
    }

    public void dirtPath()
    {
        bool dirtAdded = false;

        while (!dirtAdded)
        {
            //pick a random hex which is nest to a border hex (first green hex inward)
            var random = Random.Range(0, hexs.Count - 1);
            var randomHex = hexs[random].GetComponent<HexInfo>();

            //Is the random hex one we can use to start a path?
            if (((randomHex.heightValue / LargestLowestValue) > 0.25f))
            {
                if (randomHex.heightValue < ((int)(0.25 * LargestLowestValue)) + 2)
                {
                    //We have a hex we can use!
                    while (!dirtAdded)
                    {
                        var startingHex = (int)Random.Range(0, randomHex.pals.Length);

                        if (randomHex.pals[startingHex].GetComponent<HexInfo>().heightValue / LargestLowestValue <= 0.25f)
                        {
                            int temp2 = startingHex + 4;
                            if (temp2 > 5)
                                temp2 -= 6;
                            int temp3 = startingHex + 3;
                            if (temp3 > 5)
                                temp3 -= 6;
                            int pal = startingHex + 6;
                            if (pal > 5)
                                pal -= 6;

                            randomHex.pals[pal].GetComponent<HexInfo>().moveVert(temp2, -99, Color.Lerp(Color.black, new Color(0.96f, 0.64f, 0.38f), 0.75f));
                            randomHex.pals[pal].GetComponent<HexInfo>().moveVert(temp3, -99, Color.Lerp(Color.black, new Color(0.96f, 0.64f, 0.38f), 0.75f));
                            //randomHex.dirtPath(startingHex, LargestLowestValue, 1,this);
                            dirtAdded = true;
                        }
                    }
                }
            }
        }
    }

    GameObject lastIsland;

    public void mergeIsland()
    {
        #region combine mesh
        
        CombineInstance[] combine = new CombineInstance[hexs.Count];

        for (int i = 0; i < hexs.Count; i++)
        {

            if (hexs[i] != null)
            {
                combine[i].mesh = hexs[i].GetComponent<MeshFilter>().sharedMesh;
                combine[i].transform = hexs[i].GetComponent<MeshFilter>().transform.localToWorldMatrix;
            }
        }
        if (hexs.Count > 1)
        {
            hexs[0].GetComponent<MeshFilter>().mesh = new Mesh();
            hexs[0].GetComponent<MeshFilter>().mesh.CombineMeshes(combine);
            hexs[0].transform.position = Vector3.zero;
            for (int i = 1; i < hexs.Count; i++)
            {
                Destroy(hexs[i]);
            }

            DestroyImmediate(hexs[0].GetComponent<MeshCollider>());
            MeshCollider meshCollider = hexs[0].AddComponent<MeshCollider>();
            meshCollider.sharedMesh = hexs[0].GetComponent<MeshFilter>().mesh;
            finishedIslands++;
            hexs[0].name = "Finished Island " + finishedIslands;
            
        #endregion
            if (flag != null)
                flag.transform.parent = hexs[0].transform;

            flag = null;
            hexs[0].GetComponent<MeshCollider>().sharedMesh.RecalculateBounds();
            var newBounds = hexs[0].GetComponent<MeshCollider>().sharedMesh.bounds;

            //lastPos.x + Oldbounds/2 + newbounds/2
            if (lastIsland != null)
            {
                var oldBounds = lastIsland.GetComponent<MeshCollider>().sharedMesh.bounds;
                hexs[0].transform.position = new Vector3(lastIsland.transform.position.x + oldBounds.size.x + newBounds.size.x, 0, Random.Range(-newBounds.size.z, newBounds.size.z));
            }
            else
                hexs[0].transform.position = new Vector3(150 + (newBounds.size.x / 2), 0, Random.Range(-newBounds.size.z, newBounds.size.z));
            //store position of last island so we can make the new one correctly
            lastIsland = hexs[0];
            Camera.main.GetComponent<cameraOrbitControls>().islands.Add(hexs[0]);
            hexs.Clear();
        }

        GameObject.Find("flagpole(Clone)").name = "flagpole " + finishedIslands;
        Destroy(lastIsland.GetComponent<HexInfo>());
        var ilnd = lastIsland.AddComponent<finishedIsland>();
        ilnd.islandIndex = finishedIslands;
    }

    #endregion
}