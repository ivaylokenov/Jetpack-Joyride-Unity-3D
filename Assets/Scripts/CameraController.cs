using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject objToFollow;

    private float offsetX;

    public void Start()
    {
        var height = Camera.main.orthographicSize * 2;
        var screenWidth = Camera.main.aspect * height;
        this.offsetX = screenWidth / 4;
    }

    public void Update()
    {
        var currentPosition = this.transform.position;
        currentPosition.x = this.objToFollow.transform.position.x
            + this.offsetX;
        this.transform.position = currentPosition;
    }
}
