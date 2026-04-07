using UnityEngine;

public class CubeGenerator : MonoBehaviour
{
    public GameObject cubePrefab;
    public int totalCaubes = 10;
    public float cubeSpacing = 1.0f;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GenCube();
    }

    public void GenCube()
    {
        Vector3 myPosition = transform.position;
        GameObject firstCube = Instantiate(cubePrefab, myPosition, Quaternion.identity);

        for (int i = 1; i < totalCaubes; i++)
        {
            Vector3 Position = new Vector3(myPosition.x, myPosition.y, myPosition.z + (i * cubeSpacing));
            Instantiate(cubePrefab, Position, Quaternion.identity);
        }
    }

    
}