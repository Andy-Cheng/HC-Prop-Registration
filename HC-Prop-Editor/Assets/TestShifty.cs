using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HC.Shifty;
public class TestShifty : MonoBehaviour
{
    public int currentMode;
    public int nextMode;
    public List<Transform> trackerTransforms;
    Shifty myShifty;

    void RecieveTransform(int trackerID, Quaternion rotation, Vector3 position){
        trackerTransforms[trackerID].position = position;
        trackerTransforms[trackerID].rotation = rotation;
    }

    void RecieveMode(int mode){
        Debug.Log($"Shifty mode: {mode}");
        currentMode = mode;
    }
    
    void Start()
    {
        myShifty = new Shifty();
        myShifty.OnRecieveTransform += RecieveTransform;
        myShifty.OnRecieveMode += RecieveMode;
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.G)){
            myShifty.GetMode();
        }

        if(Input.GetKeyDown(KeyCode.S)){
            myShifty.SetMode(nextMode);
        }
    }

    void OnDestroy() {
        myShifty.OnRecieveTransform -= RecieveTransform;
        myShifty.OnRecieveMode -= RecieveMode;
    }
}
