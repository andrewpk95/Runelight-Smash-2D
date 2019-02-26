using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    Camera mainCamera;
    Rigidbody2D mainCameraRB;
    List<Rigidbody2D> characters;

    public float minimumOrthographicSize;
    public float extraHorizontalBoundarySize;
    public float extraVerticalBoundarySize;
    public float lerpStrength;
    public Bounds bounds;
    
    // Start is called before the first frame update
    void Start()
    {
        mainCamera = GetComponent<Camera>();
        mainCameraRB = GetComponent<Rigidbody2D>();
        characters = new List<Rigidbody2D>();
        GameObject[] entities = GameObject.FindGameObjectsWithTag("Entity");
        foreach (GameObject entity in entities) {
            characters.Add(entity.GetComponent<Rigidbody2D>());
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float screenHeightInUnits = Camera.main.orthographicSize * 2;
        float screenWidthInUnits = screenHeightInUnits * Screen.width / Screen.height;
        //Get the maximum horizontal and vertical units the camera has to cover
        float minCameraX = Mathf.Infinity;
        float maxCameraX = -Mathf.Infinity;
        float minCameraY = Mathf.Infinity;
        float maxCameraY = -Mathf.Infinity;
        foreach (Rigidbody2D character in characters) {
            minCameraX = Mathf.Min(minCameraX, character.position.x);
            maxCameraX = Mathf.Max(maxCameraX, character.position.x);
            minCameraY = Mathf.Min(minCameraY, character.position.y);
            maxCameraY = Mathf.Max(maxCameraY, character.position.y);
        }
        //Trim camera bounds with extra boundary size and bounds
        float targetScreenWidth = maxCameraX - minCameraX;
        float targetScreenHeight = maxCameraY - minCameraY;
        minCameraX = Mathf.Max(bounds.center.x - bounds.extents.x, minCameraX - extraHorizontalBoundarySize);
        maxCameraX = Mathf.Min(bounds.center.x + bounds.extents.x, maxCameraX + extraHorizontalBoundarySize);
        minCameraY = Mathf.Max(bounds.center.y - bounds.extents.y, minCameraY - extraVerticalBoundarySize);
        maxCameraY = Mathf.Min(bounds.center.y + bounds.extents.y, maxCameraY + extraVerticalBoundarySize);
        //Debug.Log("MinX: " + minCameraX + " MaxX: " + maxCameraX + " MinY: " + minCameraY + " MaxY: " + maxCameraY);
        targetScreenWidth = maxCameraX - minCameraX;
        targetScreenHeight = maxCameraY - minCameraY;
        Vector2 targetScreenPosition = new Vector2((minCameraX + maxCameraX) / 2, (minCameraY + maxCameraY) / 2);

        //Update Camera position
        MoveCameraTo(targetScreenPosition);
        AdjustSizeTo(targetScreenWidth, targetScreenHeight);

    }

    public void MoveCameraTo(Vector2 position) {
        mainCameraRB.position = Vector3.Lerp(mainCamera.transform.position, 
            new Vector3(position.x, position.y, mainCamera.transform.position.z), lerpStrength);
    }

    public void AdjustSizeTo(float width, float height) {
        float orthographicSizeX = width * Screen.height / Screen.width / 2;
        float orthographicSizeY = height / 2;
        //Debug.Log("OrthoX: " + orthographicSizeX + " OrthoY: " + orthographicSizeY);
        mainCamera.orthographicSize = Mathf.Lerp(mainCamera.orthographicSize, 
            Mathf.Max(orthographicSizeX, orthographicSizeY, minimumOrthographicSize), lerpStrength);
    }
}
