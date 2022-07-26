using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlayTimePackets
{
    public class DisplayLobbiesPacket : BasePacket
    {
        public DisplayLobbiesPacket()
        {

        }
        public DisplayLobbiesPacket(Player player) :
            base(PacketType.DisplayLobby, player)
        {

        }
        public override byte[] Serialize()
        {
            base.Serialize();
            return ms.ToArray();
        }

        public override BasePacket DeSerialize(byte[] buffer)
        {
            return base.DeSerialize(buffer);
        }
    }
}

