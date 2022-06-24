using UnityEngine;

public class LampObject : MonoBehaviour
{
    public void DestroyThisLamp()
    {
        gameObject.SetActive(false);
        BossManager.BossCollapse();
    }
}
