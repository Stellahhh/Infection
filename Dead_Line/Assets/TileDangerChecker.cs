// Linda Fan, Stella Huo, Hanbei Zhou

using UnityEngine;

// Attached to player/zombie object to detect if it's standing on a danger (disabled) tile
public class TileDangerChecker : MonoBehaviour
{
    public GameObject playerUIPrefab; // assign the canvas prefab in inspector

    private PlayerUI playerUI;
    private bool isInWarningZone = false; // whether the player is currently on a danger tile
    private Coroutine warningCoroutine;   // handle to damage coroutine so we can stop it

    void Start()
    {
        // Instantiate personal UI warning canvas for this player
        GameObject uiInstance = Instantiate(playerUIPrefab);
        playerUI = uiInstance.GetComponent<PlayerUI>();
        playerUI.ShowWarning(false); // hide warning at start

        // Start checking tile danger status every second
        InvokeRepeating(nameof(CheckTileStatus), 1f, 1f);
    }

    // Called regularly to check if player is in danger zone
    void CheckTileStatus()
    {
        bool onDangerTile = DynamicMapGenerator.Instance.IsInsideDisabledTileRegionXZ(transform.position);

        if (onDangerTile && !isInWarningZone)
        {
            // Entering a danger tile for the first time
            warningCoroutine = StartCoroutine(WarningAndDamage());
        }
        else if (!onDangerTile && isInWarningZone)
        {
            // Leaving the danger tile
            StopCoroutine(warningCoroutine);
            warningCoroutine = null;
            playerUI.ShowWarning(false);
            isInWarningZone = false;
        }
    }

    // Coroutine that shows warning, waits, and applies damage
    System.Collections.IEnumerator WarningAndDamage()
    {
        isInWarningZone = true;
        // playerUI.ShowWarning(true); // show warning UI

        // yield return new WaitForSeconds(10f); // grace period

        while (IsInsideDangerTileXZ(transform.position))
        {
            ApplyDamage(500); // deal damage repeatedly
            yield return new WaitForSeconds(1f); // cooldown between hits
        }

        // Player exited danger zone during loop
        playerUI.ShowWarning(false);
        isInWarningZone = false;
    }

    // Check if player is inside any disabled tile using XZ only
    bool IsInsideDangerTileXZ(Vector3 pos)
    {
        foreach (Vector3 dangerPos in DynamicMapGenerator.Instance.DisabledTilePositions)
        {
            if (IsWithinXZBounds(pos, dangerPos, 5f)) // 5f = half tile size estimate
                return true;
        }
        return false;
    }

    // Simple square region check (ignores Y)
    bool IsWithinXZBounds(Vector3 playerPos, Vector3 tileCenter, float halfSize)
    {
        // Only check X and Z; ignore height (Y)
        return Mathf.Abs(playerPos.x - tileCenter.x) < halfSize &&
               Mathf.Abs(playerPos.z - tileCenter.z) < halfSize;
    }

    // Apply damage — could later be hooked into a real health system
    // void ApplyDamage(int amount)
    // {
    //     Debug.Log($"{gameObject.name} is taking {amount} damage in danger zone.");
    //     // Hook this to a health system if desired
    // }

    void ApplyDamage(int amount)
    {
        Life life = GetComponent<Life>();
        if (life != null)
        {
            life.amount -= amount;
            Debug.Log($"{gameObject.name} took {amount} damage. Remaining life: {life.amount}");
        }
        else
        {
            Debug.LogWarning($"{gameObject.name} has no Life component.");
        }
    }
}
