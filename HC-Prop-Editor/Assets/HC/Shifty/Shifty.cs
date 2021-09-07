using UnityEngine;
using System;
using HC.Network;

namespace HC.Shifty
{
    public class Shifty
    {
        public string deviceName = "Shifty";
        public Shifty()
        {
            ClientHandle.DeviceNameToHandle.Add(deviceName, DeviceHandle);
        }

        // Device to player
        /// <summary>Recieve the transform of the tracker with id. int: id, Quaternion: rotation, Vector3: position</summary>
        public event Action<int, Quaternion, Vector3> OnRecieveTransform;
        /// <summary>Recieve the current mode of shifty. int: mode</summary>
        public event Action<int> OnRecieveMode;

        public void DeviceHandle(Packet _packet)
        {
            int functionID = _packet.ReadInt();
            if (functionID == 0)
            {
                OnRecieveTransform.Invoke(_packet.ReadInt(), _packet.ReadQuaternion(), _packet.ReadVector3());
            }
            else if (functionID == 1)
            {
                OnRecieveMode.Invoke(_packet.ReadInt());
            }
        }

        // Player to device
        /// <summary>Get the current mode of Shifty.</summary>
        public void GetMode()
        {
            using (Packet _packet = new Packet())
            {
                _packet.Write(deviceName);
                ClientSend.TransferPacket(_packet);
            }
        }
        /// <summary>Set the mode of Shifty.</summary>
        /// <param name="nextMode">Next mode to be set. Tennis mode: 0, Sword mode: 1.</param>
        public void SetMode(int nextMode)
        {
            using (Packet _packet = new Packet())
            {
                _packet.Write(deviceName);
                _packet.Write(nextMode);
                ClientSend.TransferPacket(_packet);
            }
        }
    }
}
