using System.IO;
using UnityEngine;

namespace UnityGame.Tests.DesignPatterns
{
    class Memento
    {
        private interface ISnapshot
        {
            public byte[] SaveSnapshot();
            public void LoadSnapshot(byte[] data);
            public void Print();
        }

        private class PlayerEntity : ISnapshot
        {
            private string _id;
            private int _health;

            public PlayerEntity(string id, int health)
            {
                _id = id;
                _health = health;
            }

            public PlayerEntity()
            {
                _id = null;
                _health = 0;
            }

            public void LoadSnapshot(byte[] data)
            {
                MemoryStream stream = new MemoryStream(data);
                BinaryReader reader = new BinaryReader(stream);
                _id = reader.ReadString();
                _health = reader.ReadInt32();
            }

            public byte[] SaveSnapshot()
            {
                MemoryStream stream = new MemoryStream();
                BinaryWriter writer = new BinaryWriter(stream);

                writer.Write(_id);
                writer.Write(_health);

                writer.Close();

                byte[] data = stream.GetBuffer();

                stream.Close();

                return data;
            }

            public void Print()
            {
                Debug.Log($"[Memento][PlayerEntity] Id:{_id}; Health:{_health}");
            }
        }

        public class Tester
        {
            public void Test()
            {
                PlayerEntity entity = new PlayerEntity("plr_1", 203);
                PlayerEntity entity2 = new PlayerEntity("plr_2", 345);
                PlayerEntity entity3 = new PlayerEntity("plr_3", 600);

                Debug.Log("[Memento] Before: ");
                entity.Print();
                entity2.Print();
                entity3.Print();
                byte[] data = entity.SaveSnapshot();
                byte[] data2 = entity2.SaveSnapshot();
                byte[] data3 = entity3.SaveSnapshot();

                Debug.Log("[Memento] After: ");
                entity = new PlayerEntity();
                entity2 = new PlayerEntity();
                entity3 = new PlayerEntity();
                entity.LoadSnapshot(data);
                entity2.LoadSnapshot(data2);
                entity3.LoadSnapshot(data3);
                entity.Print();
                entity2.Print();
                entity3.Print();
            }
        }
    }
}
