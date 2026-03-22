using UnityEngine;

public class PlayerBombHandler : MonoBehaviour
{
    public Transform holdPoint; // จุดถือ (วางหน้าตัวละคร)

    BombPickup heldBomb;

    void Update()
    {
        //  กด E หยิบ
        if (Input.GetKeyDown(KeyCode.E))
        {
            TryPickUp();
        }

        //  กด F ปา
        if (Input.GetMouseButtonDown(0) && heldBomb != null)
        {
            ThrowBomb();
        }
    }

    void TryPickUp()
    {
        if (heldBomb != null) return;

        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, 1f);

        foreach (var hit in hits)
        {
            BombPickup bomb = hit.GetComponent<BombPickup>();
            if (bomb != null)
            {
                heldBomb = bomb;
                bomb.PickUp(holdPoint);
                break;
            }
        }
    }

    void ThrowBomb()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;

        Vector2 direction = (mousePos - heldBomb.transform.position).normalized;

        heldBomb.Throw(direction);

        heldBomb = null;
    }
}