using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Text;

namespace HC.Network
{
public class ServerSend
{
    #region Commom
    /// <summary>Sends a packet to a client via TCP.</summary>
    /// <param name="_toClient">The client to send the packet the packet to.</param>
    /// <param name="_packet">The packet to send to the client.</param>
    private static void SendTCPData(int _toClient, Packet _packet)
    {
        _packet.WriteLength();
        Server.clients[_toClient].tcp.SendData(_packet);
    }

    /// <summary>Sends a packet to a client via UDP.</summary>
    /// <param name="_toClient">The client to send the packet the packet to.</param>
    /// <param name="_packet">The packet to send to the client.</param>
    private static void SendUDPData(int _toClient, Packet _packet)
    {
        _packet.WriteLength();
        Server.clients[_toClient].udp.SendData(_packet);
    }

    /// <summary>Sends a packet to all clients via TCP.</summary>
    /// <param name="_packet">The packet to send.</param>
    private static void SendTCPDataToAll(Packet _packet)
    {
        _packet.WriteLength();
        for (int i = 1; i <= Server.MaxPlayers; i++)
        {
            Server.clients[i].tcp.SendData(_packet);
        }
    }
    /// <summary>Sends a packet to all clients except one via TCP.</summary>
    /// <param name="_exceptClient">The client to NOT send the data to.</param>
    /// <param name="_packet">The packet to send.</param>
    private static void SendTCPDataToAll(int _exceptClient, Packet _packet)
    {
        _packet.WriteLength();
        for (int i = 1; i <= Server.MaxPlayers; i++)
        {
            if (i != _exceptClient)
            {
                Server.clients[i].tcp.SendData(_packet);
            }
        }
    }

    /// <summary>Sends a packet to all clients via UDP.</summary>
    /// <param name="_packet">The packet to send.</param>
    private static void SendUDPDataToAll(Packet _packet)
    {
        _packet.WriteLength();
        for (int i = 1; i <= Server.MaxPlayers; i++)
        {
            Server.clients[i].udp.SendData(_packet);
        }
    }
    /// <summary>Sends a packet to all clients except one via UDP.</summary>
    /// <param name="_exceptClient">The client to NOT send the data to.</param>
    /// <param name="_packet">The packet to send.</param>
    private static void SendUDPDataToAll(int _exceptClient, Packet _packet)
    {
        _packet.WriteLength();
        for (int i = 1; i <= Server.MaxPlayers; i++)
        {
            if (i != _exceptClient)
            {
                Server.clients[i].udp.SendData(_packet);
            }
        }
    }
    #endregion
    #region Packets-Common
    /// <summary>Sends a welcome message to the given client.</summary>
    /// <param name="_toClient">The client to send the packet to.</param>
    /// <param name="_msg">The message to send.</param>
    /// Welcome format = XXXX-
    public static void Welcome(int _toClient, string _msg)
    {
        using (Packet _packet = new Packet((int)ServerPackets.welcome))
        {
            _packet.Write(_toClient);
            _packet.Write(_msg);
            SendTCPData(_toClient, _packet);
            Debug.Log($"send welcome to {_toClient}");
        }
    }

    public static void RegisterSuccess(int _toClient){
        using (Packet _packet = new Packet((int) ServerPackets.registerSuccess) ){
            SendTCPData(_toClient, _packet);
        }
    }
    public static void TransferPacketToDevice(int _toClient, Packet packetFromPlayer){
        using (Packet _packet = new Packet((int) ServerPackets.transferPackettoDevice) ){
            byte[] _packetFromPlayer = new byte[packetFromPlayer.UnreadLength()];
            Array.Copy(packetFromPlayer.ToArray(), packetFromPlayer.ReadPos, _packetFromPlayer, 0, packetFromPlayer.UnreadLength());
            _packet.Write(_packetFromPlayer);
            // // Debug: 
            // Debug.Log($"Packet ID: {_packet.ReadInt()}");
            // int functionID = _packet.ReadInt();
            // Debug.Log($"function ID: {functionID}");
            // if(functionID == 1){

            //     Debug.Log($"mode: {_packet.ReadInt()}");
            // }

            // // end
            SendTCPData(_toClient, _packet);
        }
    }

    public static void TransferPacketToPlayer(int _toClient, string deviceName, Packet packetFromDevice)
    {
        using (Packet _packet = new Packet((int)ServerPackets.transferPackettoPlayer))
        {
            _packet.Write(deviceName);
            byte[] _packetFromDevice = new byte[packetFromDevice.UnreadLength()];
            Array.Copy(packetFromDevice.ToArray(), packetFromDevice.ReadPos, _packetFromDevice, 0, packetFromDevice.UnreadLength());
            _packet.Write(_packetFromDevice);
            SendTCPData(_toClient, _packet);
        }
    }


    #endregion



}
}