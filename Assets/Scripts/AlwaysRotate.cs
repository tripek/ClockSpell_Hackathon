using UnityEngine;

public class AlwaysRotate : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 1f;

    private void Update()
    {
        transform.localEulerAngles += new Vector3(0, 0, rotationSpeed * Time.deltaTime);
    }
}
