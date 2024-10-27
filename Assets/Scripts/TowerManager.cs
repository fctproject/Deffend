using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TowerManager : MonoBehaviour
{
    public TowerButton towerBtnPressed { get; set; }
    private SpriteRenderer spriteRenderer;
    private Collider2D buildTile;
    public List<GameObject> TowerList = new List<GameObject>();
    public List<Collider2D> BuildList = new List<Collider2D>();

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 worldPointPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(worldPointPosition, Vector2.zero);

            if (hit.collider != null && hit.collider.CompareTag("BuildPlace"))
            {
                if (!BuildList.Contains(hit.collider)) // Only place if the site is not full
                {
                    buildTile = hit.collider;
                    RegisterBuildPlace(buildTile);
                    PlaceTower(hit);
                    buildTile.tag = "BuildPlaceFull"; // Mark site as full
                }
            }
        }

        if (spriteRenderer.enabled)
        {
            FollowMouse();
        }
    }

    public void RegisterBuildPlace(Collider2D other)
    {
        if (!BuildList.Contains(other))
        {
            BuildList.Add(other); // Register only unique build places
        }
    }

    public void PlaceTower(RaycastHit2D hit)
    {
        if (!EventSystem.current.IsPointerOverGameObject() && towerBtnPressed != null)
        {
            GameObject newTower = Instantiate(towerBtnPressed.TowerObject);
            newTower.transform.position = hit.transform.position;
            RegisterTower(newTower);
            BuyTower(towerBtnPressed.TowerPrice);
            DisableDragSprite();
        }
    }

    public void RegisterTower(GameObject tower)
    {
        TowerList.Add(tower);
    }

    public void BuyTower(int price)
    {
        GameManager.instance.ReduceGold(price);
    }

    public void SelectedTower(TowerButton towerBtn)
    {
        if (towerBtn.TowerPrice <= GameManager.instance.currentGold)
        {
            towerBtnPressed = towerBtn;
            EnableDragSprite(towerBtn.DragSprite);
        }
    }

    private void FollowMouse()
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = new Vector2(mousePosition.x, mousePosition.y);
    }

    public void EnableDragSprite(Sprite sprite)
    {
        spriteRenderer.enabled = true;
        spriteRenderer.sprite = sprite;
    }

    public void DisableDragSprite()
    {
        spriteRenderer.enabled = false;
        towerBtnPressed = null;
    }

    public void DestroyAllTowers()
    {
        foreach (GameObject tower in TowerList)
        {
            Destroy(tower.gameObject);
        }
        TowerList.Clear();
        RenameTagsBuildPlace();
    }

    public void RenameTagsBuildPlace()
    {
        foreach (Collider2D buildTag in BuildList)
        {
            buildTag.tag = "BuildPlace";
        }
        BuildList.Clear();
    }
}
