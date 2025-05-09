using UnityEngine;

public class ToggleVisibility : MonoBehaviour
{
    public GameObject targetObject;

    public void ToggleObject()
    {
        if (targetObject != null)
        {
            targetObject.SetActive(!targetObject.activeSelf);
        }
    }
}
