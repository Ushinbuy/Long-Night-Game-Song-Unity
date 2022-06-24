using System.Collections;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    private readonly float coordAmplitutde = 0.05f;
    private readonly float timeForUpadte = 0.05f;
    public bool ShakerEnable { get; set; }

    private void Start()
    {
        ShakerEnable = false;
        StartCoroutine(MoveObject());
    }

    IEnumerator MoveObject()
    {
        Vector3 moveRight   = Vector3.right * coordAmplitutde;
        Vector3 moveLeft    = Vector3.left  * coordAmplitutde;
        Vector3 moveUp      = Vector3.up    * coordAmplitutde;
        Vector3 moveDown    = Vector3.down  * coordAmplitutde;

        Vector3[] allPosition = new Vector3[] { moveRight, moveUp, moveLeft, moveDown };
        while(true)
        {
            for (int i = 0; i < allPosition.Length; i++)
            {
                yield return new WaitForSeconds(timeForUpadte);
                if (ShakerEnable)
                {
                    transform.Translate(allPosition[i]);
                }
            }
        }
    }
}
