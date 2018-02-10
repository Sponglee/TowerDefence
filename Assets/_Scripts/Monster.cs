using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour {

    [SerializeField]
    private float speed=1f;
    //path 
    private Stack<Node> path;
    // position
    public Point GridPosition { get; set; }
    // set destination (not final point, but next point from stack
    private Vector3 destination;
    //Spawn trigger (don't move before spawn finishes)
    public bool IsActive { get; set; }

    private void Update()
    {
        Move();
    }


    public void Spawn()
    {
        transform.position = LevelManager.Instance.BluePortal.transform.position;
        StartCoroutine(Scale(new Vector3(0.1f,0.1f,0.1f), new Vector3(0.3f,0.3f,0.1f)));
        Debug.Log("PATH BEFORE SETPATH: " + LevelManager.Instance.Path.Count);
        SetPath(LevelManager.Instance.Path);
      
    }

    public IEnumerator Scale (Vector3 from, Vector3 to)
    {
        IsActive = false;
        float progress = 0;
        while (progress<=1)
        {
            transform.localScale = Vector3.Lerp(from, to,progress);
            progress += Time.deltaTime;
            yield return null; 
        }
        transform.localScale = to;
        IsActive = true;
    }


    private void Move()
    {
        if (IsActive)
        {
            transform.position = Vector2.MoveTowards(transform.position, destination, speed*Time.deltaTime);

            if (transform.position == destination)
            {
                Debug.Log("Reached "+ path);
                if (path != null && path.Count > 0)
                {
                    Debug.Log("Setting path to " + path);
                    GridPosition = path.Peek().GridPosition;
                    destination = path.Pop().WorldPosition;
                }

            }
        }
       
    }

    private void SetPath(Stack<Node> newPath)
    {
        Debug.Log("SET PATH: ");
        if (newPath != null)
        {
            path = newPath;
            Debug.Log(path.Count);
            if (path.Count>0)
            {
                Debug.Log("PATH NOT EMPTY");
                GridPosition = path.Peek().GridPosition;
                destination = path.Pop().WorldPosition;
            }
           
        }
    }
}
