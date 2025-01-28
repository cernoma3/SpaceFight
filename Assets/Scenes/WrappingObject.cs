using UnityEngine;

public class WrappingObject : MonoBehaviour
{
    private void LateUpdate()
    {
        Vector3 position = transform.position;

        if (position.x > LevelManager.MapSize.x / 2)
            position.x = -LevelManager.MapSize.x / 2;
        else if (position.x < -LevelManager.MapSize.x / 2)
            position.x = LevelManager.MapSize.x / 2;

        if (position.y > LevelManager.MapSize.y / 2)
            position.y = -LevelManager.MapSize.y / 2;
        else if (position.y < -LevelManager.MapSize.y / 2)
            position.y = LevelManager.MapSize.y / 2;

        transform.position = position;
    }
}