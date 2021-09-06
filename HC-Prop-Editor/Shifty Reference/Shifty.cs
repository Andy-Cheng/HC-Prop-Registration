using UnityEngine;
using System;
using HC.Network;

namespace HC.Shifty{
public class Shifty{
    public string deviceName = "Shifty";
    public Shifty(){
        ClientHandle.DeviceNameToHandle.Add(deviceName, DeviceHandle);
    }

    // Device to player
    public event Action<int, Quaternion, Vector3> OnRecieveTransform;
    public event Action<int> OnRecieveMode;

    public void DeviceHandle(Packet _packet){
        int functionID = _packet.ReadInt();
        if(functionID == 0)
        {
            OnRecieveTransform.Invoke(_packet.ReadInt(), _packet.ReadQuaternion(), _packet.ReadVector3());
        }
        else if(functionID == 1){
            OnRecieveMode.Invoke(_packet.ReadInt());
        }
    }

    // Player to device
    public void GetMode(){
        using(Packet _packet = new Packet()){
            _packet.Write(deviceName); 
            _packet.Write(0);
            ClientSend.TransferPacket(_packet);
        }
    }

    public void SetMode(int mode){
        using(Packet _packet = new Packet()){
            _packet.Write(deviceName); 
            _packet.Write(1);
            _packet.Write(mode);
            ClientSend.TransferPacket(_packet);
        }
    }
}
}