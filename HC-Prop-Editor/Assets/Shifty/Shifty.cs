using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class Shifty{

    public string deviceName = "Shifty";
    
    public Shifty(){
        ClientHandle.DeviceNameToHandle.Add(deviceName, DeviceHandle);
    }
    public void DeviceHandle(Packet _packet){
        int functionID = _packet.ReadInt();
        if(functionID == 0)
        {
            OnRecieveTransform?.Invoke(_packet.ReadInt(), _packet.ReadQuaternion(), _packet.ReadVector3());
        }

        else if(functionID == 1){
            OnRecieveMode?.Invoke(_packet.ReadInt());
        }

    }


    // Player to device
    public void GetMode(){
        using(Packet _packet = new Packet()){
            _packet.Write(deviceName); 
            _packet.Write(0); // write function id
            ClientSend.TransferPacket(_packet);
        }
    }

    public void SetMode(int mode){
        using(Packet _packet = new Packet()){
            _packet.Write(deviceName); 
            _packet.Write(1); // write function id
            _packet.Write(mode); // write function id
            ClientSend.TransferPacket(_packet);
        }
    }
    // Device to player               // Function ID
    public event Action<int, Quaternion, Vector3> OnRecieveTransform; //Tracker ID, rotation, position  // 0

    public event Action<int> OnRecieveMode; // 1
    


}