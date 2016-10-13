using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class BSPGenerator : MonoBehaviour
{ 
    List<BSPLeaf> _leafs = new List<BSPLeaf>();
    List<BSPRoom> _rooms = new List<BSPRoom>();
    int[,] BitmapData = new int[200,200];

    public GameObject cube1;
    public GameObject cube2;

    public void GenerateMap()
    {
        
        BSPLeaf root = new BSPLeaf(0, 0, 50, 50);
        
        _leafs.Add(root);

        bool did_split = true;

        int index = 0;

        while (did_split)
        {

            did_split = false;


//            BSPLeaf lc;
//            BSPLeaf rc;
            List<BSPLeaf> n_leafs = new List<BSPLeaf>();
            

            foreach (BSPLeaf l in _leafs)
            {
            //BSPLeaf l = _leafs[index];
                if (l.leftChild == null && l.rightChild == null) //if this leaf is not already split
                {
                    if (l.width > BSPLeaf.MAX_LEAF_SIZE || l.height > BSPLeaf.MAX_LEAF_SIZE /*|| Random.Range(0, 1) > 0*/)
                    {
                        if (l.split()) //split the leaf
                        {
//                            _leafs.Add(l.leftChild);
//                            _leafs.Add(l.rightChild);
                            n_leafs.Add(l.leftChild);
                            n_leafs.Add(l.rightChild);

                            did_split = true;
                            index ++;
                        }
                    }
                }
            }

//            _leafs.AddRange(n_leafs);
//            if(n_leafs.Count > 0)
            if(n_leafs.Count > 0)
                _leafs.AddRange(n_leafs);
        }

        root.createRoom();


        // create the rooms list
        foreach (BSPLeaf l in _leafs)
        {
            if (l.room != null)
            {
                _rooms.Add(l.room);
            }

            
        }
    }

    void Start()
    {
        GenerateMap();
        Debug.Log(_rooms.Count);
        CreateMap();
        showDebug();
    }

    void showDebug()
    {
        for (int i = 0; i < 150; i++)
        {
            for (int j = 0; j < 150; j++)
            {
                if (BitmapData[i,j] == 1)
                {
                    GameObject obj = GameObject.Instantiate(cube2);
                    obj.transform.localScale = new Vector3(1, 1, 1);
                    obj.transform.position = new Vector3(i, j, 0);
                }
                else
                {
                    GameObject obj = GameObject.Instantiate(cube1);
                    obj.transform.localScale = new Vector3(1, 1, 1);
                    obj.transform.position = new Vector3(i, j, 0);
                    
                }
            }
        }
    }

    void InitMap()
    {
        for (int i = 0; i < 150; i++)
        {
            for (int j = 0; j < 150; j++)
            {
                
            }
        }

    }

    void CreateMap()
    {
        foreach(BSPRoom rm in _rooms)
        {
            for (int i = rm.x; i < rm.x + rm.width; i++)
            {
                for (int j = rm.y; j < rm.y + rm.height; j++)
                {
                    BitmapData[i, j] = 1;
                }
            }
        }
    }
}