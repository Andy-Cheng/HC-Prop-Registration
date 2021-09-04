using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

namespace HC.Network
{
public class ClientHandle : MonoBehaviour
{
    public static void Welcome(Packet _packet)
    {
        int _myId = _packet.ReadInt();
        string _msg = _packet.ReadString();

        Debug.Log($"Message from server: {_msg}, my id is: {_myId}");
        Client.instance.myId = _myId;
        ClientSend.WelcomeReceived();
    }

    public static void OnRegisterSuccess(Packet _packet)
    {
        Client.instance.udp.Connect(((IPEndPoint)Client.instance.tcp.socket.Client.LocalEndPoint).Port);
    } 

    public static Dictionary<string, Action<Packet>> DeviceNameToHandle = new Dictionary<string, Action<Packet>>();

    public static void OnTransferPacket(Packet _packet)
    {
        string deviceName = _packet.ReadString();
        Debug.Log($"Device Name: {deviceName}");
        if(DeviceNameToHandle.ContainsKey(deviceName)){
            DeviceNameToHandle[deviceName](_packet);
        }
    }


}
}