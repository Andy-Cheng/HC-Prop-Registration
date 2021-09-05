using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace HC.Network
{
public class ServerHandle
{
    public static void WelcomeReceived(int _fromClient, Packet _packet)
    {
        Debug.Log($"{Server.clients[_fromClient].tcp.socket.Client.RemoteEndPoint} connected successfully. Client id: {_fromClient}.");
        int deviceType = _packet.ReadInt();
        string deviceName = _packet.ReadString().Trim('\0');
        string[] names = (deviceType == 0)? Server.RegisteredDevices: Server.PlayerNames;
        foreach(string name in names){
            if(name == deviceName){
                Debug.Log($"Register {deviceName}");
                Server.DeviceNameToClientID.Add(name, _fromClient);
                Server.ClientIDToDeviceName.Add(_fromClient, name);
                ServerSend.RegisterSuccess(_fromClient);
                return;
            }
        }
        // Server.clients[_fromClient].tcp.Disconnect();
    }
    
    public static void OnRequestDevice(int _fromClient, Packet _packet)
    {
        int deviceCount = _packet.ReadInt();
        for(int i = 0; i < deviceCount; ++i)
        {
            string deviceName = _packet.ReadString();
            if(Server.DeviceNameToClientID.ContainsKey(deviceName) && !Server.DeviceIDtoClientID.ContainsKey(Server.DeviceNameToClientID[deviceName]))
            {
                Server.DeviceIDtoClientID.Add(Server.DeviceNameToClientID[deviceName], _fromClient);
                Debug.Log($"Player {_fromClient} occupies device {deviceName}");
            }
            else{
                Debug.Log($"No device {deviceName} or alrealdy occupied");
            }
        }
    }

    public static void TransferPacketFromPlayer(int _fromClient, Packet _packet)
    {
        string deviceName = _packet.ReadString();
        if(Server.DeviceNameToClientID.ContainsKey(deviceName)){
            if(Server.DeviceIDtoClientID.ContainsKey(Server.DeviceNameToClientID[deviceName]) && Server.DeviceIDtoClientID[Server.DeviceNameToClientID[deviceName]] == _fromClient){
                Debug.Log($"Send packet to {deviceName}");
                ServerSend.TransferPacketToDevice(Server.DeviceNameToClientID[deviceName], _packet);
            }
            else{
                Debug.Log($"Device {deviceName} doesn't belong to Player {_fromClient}");
            }
        }
        else{
            Debug.Log($"No device {deviceName}");
        }

    }

    public static void TransferPacketFromDevice(int _fromClient, Packet _packet)
    {
        if (Server.DeviceIDtoClientID.ContainsKey(_fromClient))
        {
            ServerSend.TransferPacketToPlayer(Server.DeviceIDtoClientID[_fromClient], Server.ClientIDToDeviceName[_fromClient], _packet);
        }
        else
        {
            Debug.Log($"Device with id {_fromClient} doesn't belong to Player or wrong client ID");
        }

    }
    
}
}