using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HC.Network
{
public class ClientSend : MonoBehaviour
{
    /// <summary>Sends a packet to the server via TCP.</summary>
    /// <param name="_packet">The packet to send to the sever.</param>
    private static void SendTCPData(Packet _packet)
    {
        _packet.WriteLength();
        Client.instance.tcp.SendData(_packet);
    }

    /// <summary>Sends a packet to the server via UDP.</summary>
    /// <param name="_packet">The packet to send to the sever.</param>
    private static void SendUDPData(Packet _packet)
    {
        _packet.WriteLength();
        Client.instance.udp.SendData(_packet);
    }

    #region Packets
    /// <summary>Lets the server know that the welcome message was received.</summary>
    public static void WelcomeReceived()
    {
        using (Packet _packet = new Packet((int)ClientPackets.welcomeReceived))
        {
            _packet.Write(Client.instance.DeviceType);
            _packet.Write(Client.instance.DeviceName);

            SendTCPData(_packet);
        }
    }

    public static void RequestDevice(int deviceCount, string[] deviceNames)
    {
        using(Packet _packet = new Packet((int)ClientPackets.requestDevice))
        {
            _packet.Write(deviceCount);
            foreach(string deviceName in deviceNames)
            {
                _packet.Write(deviceName);
            }
            SendTCPData(_packet);
        }
    }

    public static void TransferPacket(Packet _packet)
    {
        _packet.InsertInt((int)ClientPackets.transferPacketPlayer);
        SendTCPData(_packet);
    }
    

    #endregion
}
}