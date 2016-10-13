using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BSPLeaf
{

    public static int MIN_LEAF_SIZE = 8;
    public static int MAX_LEAF_SIZE = 20;

    public int x, y, width, height;
    public BSPLeaf leftChild;
    public BSPLeaf rightChild;
    public BSPRoom room;
    public List<BSPRoom> halls;

    public BSPLeaf(int _x, int _y, int _width, int _height)
    {
        // initialize our leaf

        this.x = _x;
        this.y = _y;
        this.width = _width;
        this.height = _height;
    }

    public bool split()
    {
        // begin splitting the leaf into 2 children

        if (leftChild != null || rightChild != null)
        {
            return false;   // we're already split! Abort!
        }

        // determine direction of split
        // if the width is >25% larger than height, we split vertically
        // if the height is >25% larger than the width, we split horizontally
        // otherwise we split randomly

        bool splitH = Random.Range(0, 1) > 0.5;

        if (width > height && width / height >= 1)   //1.25
            splitH = false;
        else if (height > width && height / width >= 1)  //1.25
            splitH = true;

        int max = (splitH ? height : width) - MIN_LEAF_SIZE;    // determine the maximum height or width
        if (max < MIN_LEAF_SIZE)
            return false;   // the area is too small to split any more...

        int split = (int)Random.Range(MIN_LEAF_SIZE, max);  // determine where we're going to split

        // create our left and right children based on the direction of the split

        if (splitH)
        {
            leftChild = new BSPLeaf(x, y, width, split);
            rightChild = new BSPLeaf(x, y + split, width, height - split);
        }
        else
        {
            leftChild = new BSPLeaf(x, y, split, height);
            rightChild = new BSPLeaf(x + split, y, width - split, height);
        }
        return true; // split successful!
    }

    public void createRoom()
    {
        if (leftChild != null || rightChild != null)
        {
            if (leftChild != null)
            {
                leftChild.createRoom();
            }
            if (rightChild != null)
            {
                rightChild.createRoom();
            }
            if (leftChild != null && rightChild != null)
            {
                // create Hall
                createHall(leftChild.room, rightChild.room);



            }
        }
        else
        {
            int ww = (int)Random.Range(5, width - 2);
            int hh = (int)Random.Range(5, height - 2);

            int xx = (int) Random.Range(1, width - ww - 2);
            int yy = (int) Random.Range(1, height -hh - 2);

            room = new BSPRoom(x + xx,y + yy, ww, hh);

            //room = new BSPRoom(x + 1, y + 1, width - 2, height - 2);
        }
    }

    private void createHall(BSPRoom l, BSPRoom r)
    {
        Vector2 p1 = new Vector2((int) Random.Range(l.x - l.width/2, l.x + l.width/2),
            (int) Random.Range(l.y - l.height/2, l.y + l.height/2));
        Vector2 p2 = new Vector2((int)Random.Range(r.x - r.width / 2, r.x + r.width / 2),
           (int)Random.Range(r.y - r.height / 2, r.y + r.height / 2));
    }
}
