using UnityEngine;

public class DraggableRank : MonoBehaviour
{

    public int rankLevel = 1;
    public float dragSpeed = 30f;
    public float snapBackSpeed = 20f;

    public bool isDragging = false;

    public Vector3 originalPosition;
    public GridCell currentCell;

    public Camera MainCamera;
    public Vector3 dragOffset;
    public SpriteRenderer spriteRenderer;

    public RankGameManager GameManager;

    private void Awake()
    {
        MainCamera = Camera.main;
        spriteRenderer = GetComponent<SpriteRenderer>();
        GameManager = FindAnyObjectByType<RankGameManager>();
    }

    void Start()
    {
        originalPosition = transform.position;
    }


    void Update()
    {
        if (isDragging)
        {
            Vector3 targetPosition = GetMouseWorldPosition() + dragOffset;
            transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * dragSpeed);
        }
        else if (transform.position != originalPosition && currentCell != null)
        {
            transform.position = Vector3.Lerp(transform.position, originalPosition, Time.deltaTime * snapBackSpeed);
        }
    }

    private void OnMouseDown()
    {
        StartDragging();
    }
    private void OnMouseUp()
    {
        if (!isDragging) return;
        StopDragging();
    }

    void StartDragging()
    {
        isDragging = true;
        dragOffset = transform.position - GetMouseWorldPosition();
        spriteRenderer.sortingOrder = 0;
    }

    void StopDragging()
    {
        isDragging = false;
        spriteRenderer.sortingOrder = 1;
        GridCell targetCell = GameManager.FindClosesteCell(transform.position);

        if (targetCell != null)
        {
            if (targetCell.currentRank == null)
            {
                MoveToCell(targetCell);
            }
            else if (targetCell.currentRank != null && targetCell.currentRank.rankLevel == rankLevel)
            {
                MergeWithCell(targetCell);
            }
            else
            {
                ReturnTooriginalPosition();
            }

        }

        else
        {
            ReturnTooriginalPosition();
        }
    }

    public void MoveToCell(GridCell targetCell)
    {
        if (currentCell != null)
        {
            currentCell.currentRank = null;
        }
        currentCell = targetCell;
        targetCell.currentRank = this;

        originalPosition = new Vector3(targetCell.transform.position.x, targetCell.transform.position.y, 0);
        transform.position = originalPosition;
    }

    public void ReturnTooriginalPosition()
    {
        transform.position = originalPosition;
    }

    public void MergeWithCell(GridCell targetCell)
    {
        if(targetCell.currentRank == null || targetCell.currentRank.rankLevel != rankLevel)
        {
            ReturnTooriginalPosition();
            return;
        }

        if (currentCell != null)
        {
            currentCell.currentRank = null;
        }

        GameManager.MergeRanks(this, targetCell.currentRank);
    }

    public Vector3 GetMouseWorldPosition()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = -MainCamera.transform.position.z;
        return MainCamera.ScreenToWorldPoint(mousePos);
    }

    public void SetRankLevel(int level)
    {
        rankLevel = level;

        if (GameManager != null && GameManager.rankSprites.Length > level -1)
        {
            spriteRenderer.sprite = GameManager.rankSprites[level - 1];
        }
    }
}
