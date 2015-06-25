using UnityEngine;
using System.Collections.Generic;

public class ObjectGenerator : MonoBehaviour
{
    public List<GameObject> availableObjects;
    public List<GameObject> currentObjects;

    private float minY;
    private float maxY;

    private float screenWidth;
    private float minDistanceBetweenObjs;

    public void Start()
    {
        var height = Camera.main.orthographicSize;
        this.minY = -height / 2;
        this.maxY = height / 2;

        this.screenWidth = height * 2 * Camera.main.aspect;
        this.minDistanceBetweenObjs = this.screenWidth;
    }

    public void Update()
    {
        var position = this.transform.position;

        var maxOffsetX = position.x + this.screenWidth;
        var minOffsetX = position.x - this.screenWidth;

        var farthestDistanceX = 0.0f;

        var objectsToRemove = new List<GameObject>();

        foreach (var obj in this.currentObjects)
        {
            var objCenterX = obj.transform.position.x;
            farthestDistanceX = Mathf.Max(farthestDistanceX, objCenterX);

            if (objCenterX < minOffsetX)
            {
                objectsToRemove.Add(obj);
            }
        }
        
        if (minDistanceBetweenObjs >= this.screenWidth / 3)
        {
            this.minDistanceBetweenObjs -= 0.001f;
        }

        foreach (var obj in objectsToRemove)
        {
            this.currentObjects.Remove(obj);
            Destroy(obj);
        }

        if (farthestDistanceX < maxOffsetX)
        {
            this.AddObject(farthestDistanceX + this.minDistanceBetweenObjs);
        }
    }

    public void AddObject(float maxDistanceX)
    {
        var randomObjIndex = Random.Range(0, this.availableObjects.Count);
        var obj = Instantiate(this.availableObjects[randomObjIndex]);
        this.currentObjects.Add(obj);

        var position = obj.transform.position;
        position.x = maxDistanceX;
        position.y = Random.Range(this.minY, this.maxY);
        obj.transform.position = position;
    }
}
