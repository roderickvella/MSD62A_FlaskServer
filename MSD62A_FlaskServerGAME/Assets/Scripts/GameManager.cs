using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;
using Proyecto26;

public class GameManager : MonoBehaviour
{
    private string baseURI = "http://gld62arod.pythonanywhere.com";
    public GameObject boxPrefab;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    [ContextMenu("Save Boxes")]
    public void SaveBoxes()
    {
        print("Saving boxes on server");

        GameObject[] cubes = GameObject.FindGameObjectsWithTag("Box");
        List<Box> boxes = new List<Box>();

        foreach(GameObject cube in cubes)
        {
            Box mybox = new Box();
            mybox.positionX = cube.transform.position.x;
            mybox.positionY = cube.transform.position.y;
            mybox.positionZ = cube.transform.position.z;

            boxes.Add(mybox);

        }

        string jsonBoxes = JsonConvert.SerializeObject(boxes);
        print(jsonBoxes);

        RestClient.Post(baseURI + "/api/saveboxes", jsonBoxes).Then(response =>
        {
             print(response.StatusCode.ToString() + " " + response.Text);
        })
        .Catch(error =>
        {
            var err = error as RequestException;
            print(err.StatusCode);
            print(err.Response);
            print(err.Message);
        });


    }

    [ContextMenu("Get Boxes")]
    public void GetBoxes()
    {
        print("Getting boxes from server");
        RestClient.GetArray<Box>(baseURI + "/api/getboxes").Then(allBoxes =>
        {
            //instantiate every box in the scene with loaded values
            foreach(Box box in allBoxes)
            {
                GameObject boxCreated = Instantiate(boxPrefab, transform.position, Quaternion.identity);
                boxCreated.transform.position = new Vector3(box.positionX, box.positionY, box.positionZ);
            }
        })
        .Catch(error =>
        {
            var err = error as RequestException;
            print(err.StatusCode);
            print(err.Response);
            print(err.Message);
        });

    }
}
