//using System.Diagnostics;
using System.Collections.Generic;
using UnityEngine;


public enum Direct { Forward, Back, Right, Left, None }
public class Player : MonoBehaviour
{
    private Vector3 mouseDown, mouseUp;
    private bool isMoving = false;
    private bool isControl = false;
    private Vector3 targetPoint;
    private List<Transform> playerBricks = new List<Transform>();

    public LayerMask layerBrick;
    public float speed = 10f;
    public Vector3 offset;
    public Transform playerBrickPrefabs;
    public Transform brickHolder;
    public Transform playerSkin;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.IsState(GameState.GamePlay) && !isMoving)
        {
            if (Input.GetMouseButtonDown(0) && !isControl)
            {
                mouseDown = Input.mousePosition;
                isControl = true;
            }

            if (Input.GetMouseButtonUp(0) && isControl)
            {
                isControl = false;
                mouseUp = Input.mousePosition;
                Direct direct = GetDirect(mouseDown, mouseUp);
                if (direct != Direct.None)
                {
                    targetPoint = GetNextPoint(direct);
                    isMoving = true;
                }
                
            }
        }
        else if (isMoving)
        {
            if (Vector3.Distance(transform.position, targetPoint) < 0.1f)
            {
                isMoving = false;
            }

            transform.position = Vector3.MoveTowards(transform.position, targetPoint, speed * Time.deltaTime);
            //Debug.Log(GetNextPoint(GetDirect(mouseDown, mouseUp)));
        }
    }

    public void OnInit()
    {
        isMoving = false;
        isControl = false;
        ClearBrick();
        playerSkin.localPosition = Vector3.zero;
    }

    private Direct GetDirect(Vector3 mouseDown, Vector3 mouseUp)
    {
        Direct dir = Direct.None;
        float deltaX = mouseUp.x - mouseDown.x;
        float deltaY = mouseUp.y - mouseDown.y;

        if (Vector3.Distance(mouseUp, mouseDown) < 10)
        {
            dir = Direct.None;
        }
        else
        {
            if (Mathf.Abs(deltaY) > Mathf.Abs(deltaX))
            {
                //vuot len tren
                if (deltaY > 0)
                {
                    dir = Direct.Forward;
                }
                //vuot xuong duoi
                else
                {
                    dir = Direct.Back;
                }
            }
            else
            {
                //vuot sng phai
                if (deltaX > 0)
                {
                    dir = Direct.Right;
                }
                else
                {
                    dir = Direct.Left;
                }
            }
        }
        return dir;
    }

    private Vector3 GetNextPoint(Direct dir)
    {
        RaycastHit hit;
        Vector3 nextPoint = transform.position;
        Vector3 direct = Vector3.zero;

        switch (dir)
        {
            case Direct.Forward:
                direct = Vector3.forward;
                break;
            case Direct.Back:
                direct = Vector3.back;
                break;
            case Direct.Right:
                direct = Vector3.right;
                break;
            case Direct.Left:
                direct = Vector3.left;
                break;
            case Direct.None:
                break;
            default:
                break;
        }
        for (int i = 0; i < 100; i++)
        {
            if (Physics.Raycast(transform.position + direct * i + Vector3.up * 2, Vector3.down, out hit, 10f, layerBrick))
            {

                nextPoint = hit.collider.transform.position;
            }
            else
            {
                break;
            }
        }
        return nextPoint;
    }

    public void AddBrick()
    {
        int index = playerBricks.Count;
        Transform playerBrick = Instantiate(playerBrickPrefabs, brickHolder);
        playerBrick.localPosition = Vector3.down * 0.5f + index * 0.25f * Vector3.up;

        playerBricks.Add(playerBrick);
        //playerSkin.localPosition = playerSkin.localPosition + Vector3.up * 0.25f;
        if (index > 1)
        {
            playerSkin.localPosition = Vector3.up * index * 0.25f;
        }
        else
        {
            playerSkin.localPosition = playerSkin.localPosition;
        }

    }

    public void RemoveBrick()
    {
        int index = playerBricks.Count - 1;
        if(index >= 0)
        {
            Transform playerBrick = playerBricks[index];
            playerBricks.RemoveAt(index);
            Destroy(playerBrick.gameObject);
            playerSkin.localPosition = playerSkin.localPosition - Vector3.up * 0.25f;
        }
        
    }

    public void ClearBrick()
    {
        for (int i = 0; i < playerBricks.Count; i++)
        {
            Destroy(playerBricks[i].gameObject);
        }
        playerBricks.Clear();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("brick"))
        {
            offset = new Vector3(0, 3, 0);
        }
        if(other.CompareTag("line"))
        {
            offset = new Vector3(0, 0.5f, 0);
        }
    }
}
