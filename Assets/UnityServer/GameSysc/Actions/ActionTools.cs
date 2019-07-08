using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace GameSysc
{
    public class ActionTools
    {

        /// <summary>
        /// 将消息序列化为二进制的方法
        /// </summary>
        /// <param name="model">要序列化的对象</param>
        public static byte[] Serialize(Action model)
        {
            try
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    ProtoBuf.Serializer.Serialize<Action>(ms, model);
                    byte[] result = new byte[ms.Length];
                    ms.Position = 0;
                    ms.Read(result, 0, result.Length);
                    return result;
                }
            }
            catch (Exception ex)
            {
                MessageBox.ASSERT("序列化失败: " + ex.ToString()); //檢查 Action.cs的[ProtoInclude]編號是否有重複的 或 Action里的參數
                return null;
            }
        }

        /// <summary>
        /// 将收到的消息反序列化成对象
        /// </summary>
        /// <returns>The serialize.</returns>
        /// <param name="msg">收到的消息.</param>
        public static Action DeSerialize(byte[] msg)
        {
            try
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    ms.Write(msg, 0, msg.Length);
                    ms.Position = 0;
                    Action result = ProtoBuf.Serializer.Deserialize<Action>(ms);
                    return result;
                }
            }
            catch (Exception ex)
            {
                MessageBox.ASSERT("反序列化失败: " + ex.ToString());
                return null;
            }
        }

    }

    public class ControllActionTools
    {

        /// <summary>
        /// 将消息序列化为二进制的方法
        /// </summary>
        /// <param name="model">要序列化的对象</param>
        public static byte[] Serialize(GameControllAction.BasePlayerAction model)
        {
            try
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    ProtoBuf.Serializer.Serialize<GameControllAction.BasePlayerAction>(ms, model);
                    byte[] result = new byte[ms.Length];
                    ms.Position = 0;
                    ms.Read(result, 0, result.Length);
                    return result;
                }
            }
            catch (Exception ex)
            {
                MessageBox.ASSERT("序列化失败: " + ex.ToString());
                return null;
            }
        }

        /// <summary>
        /// 将收到的消息反序列化成对象
        /// </summary>
        /// <returns>The serialize.</returns>
        /// <param name="msg">收到的消息.</param>
        public static GameControllAction.BasePlayerAction DeSerialize(byte[] msg)
        {
            try
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    ms.Write(msg, 0, msg.Length);
                    ms.Position = 0;
                    GameControllAction.BasePlayerAction result = ProtoBuf.Serializer.Deserialize<GameControllAction.BasePlayerAction>(ms);
                    return result;
                }
            }
            catch (Exception ex)
            {
                MessageBox.ASSERT(msg.Length + " 反序列化失败: " + ex.ToString());
                return null;
            }
        }

    }

}