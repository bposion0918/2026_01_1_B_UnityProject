using UnityEngine;

public class GridCell : MonoBehaviour
{

    public int x, y;
    public DraggableRank currentRank;
    public SpriteRenderer cellRenderers;

    private void Awake()
    {
        cellRenderers = GetComponent<SpriteRenderer>();
    }

    //좌표 초기화
    public void Initialize(int gridX, int gridY)
    {
        x = gridX;
        y = gridY;
        name = "Cell_" + x + "," + y;
    }

    public bool isEmpty()
    {
        return currentRank == null;
    }

    public bool ContainPosition(Vector3 position)
    {
        Bounds bounds = cellRenderers.bounds;
        return bounds.Contains(position);
    }

    public void SetRank(DraggableRank rank)
    {
        currentRank = rank;

        if (rank != null)
        {
            rank.currentCell = this;
        }

        rank.originalPosition = new Vector3(transform.position.x, transform.position.y, 0);
        rank.transform.position = new Vector3(transform.position.x, transform.position.y, 0);
    }

    void Start()
    {
        
    }

    
    void Update()
    {
        
    }
}
