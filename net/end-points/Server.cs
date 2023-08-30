namespace Jaket.Net.EndPoints;

using Jaket.Content;
using Jaket.Net.EntityTypes;
using Jaket.IO;

/// <summary> Endpoint of the host/lobby-owner to which clients connect. </summary>
public class Server : Endpoint
{
    public override void Load()
    {
        Listen(PacketType.Snapshot, (sender, r) =>
        {
            // if the player does not have a doll, then create it
            if (!entities.ContainsKey(sender)) entities[sender] = Entities.Get(sender, EntityType.Player);

            // sometimes players disappear for some unknown reason, and sometimes I destroy them myself
            if (entities[sender] == null) entities[sender] = Entities.Get(sender, EntityType.Player);

            // read player data
            entities[sender]?.Read(r);
        });

        Listen(PacketType.SpawnBullet, (sender, r) =>
        {
            Bullets.Read(r);

            // send bullet data to everyone else
            byte[] data = r.AllBytes();
            LobbyController.EachMemberExceptOwnerAnd(sender, member => Networking.Send(member.Id, data, PacketType.SpawnBullet));
        });

        Listen(PacketType.DamageEntity, (sender, r) =>
        {
            entities[r.Id()]?.Damage(r);

            // send damage data to everyone else
            byte[] data = r.AllBytes();
            LobbyController.EachMemberExceptOwnerAnd(sender, member => Networking.Send(member.Id, data, PacketType.DamageEntity));
        });

        Listen(PacketType.Punch, (sender, r) =>
        {
            var entity = entities[r.Id()];
            if (entity is RemotePlayer player) player.Punch(r);

            // send damage data to everyone else
            byte[] data = r.AllBytes();
            LobbyController.EachMemberExceptOwnerAnd(sender, member => Networking.Send(member.Id, data, PacketType.Punch));
        });
    }

    public override void Update()
    {
        // write snapshot
        Networking.EachEntity(entity =>
        {
            // when an entity is destroyed via Object.Destroy, the element in the list is replaced with null
            if (entity == null) return;

            byte[] data = Writer.Write(w =>
            {
                w.Id(entity.Id);
                w.Byte((byte)entity.Type);

                entity.Write(w);
            });

            // send snapshot
            LobbyController.EachMemberExceptOwner(member => Networking.SendSnapshot(member.Id, data));
        });

        // read incoming packets
        UpdateListeners();
    }
}