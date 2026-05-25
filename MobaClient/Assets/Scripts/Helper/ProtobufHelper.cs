using Google.Protobuf;
using System;
using System.IO;

public static class ProtobufHelper
{
    public static byte[] ToBytes(IMessage message)
    {
        if (message == null)
        {
            throw new ArgumentNullException(nameof(message));
        }
        return message.ToByteArray();
    }

    public static void ToStream(IMessage message, MemoryStream stream)
    {
        if (message == null)
        {
            throw new ArgumentNullException(nameof(message));
        }

        if (stream == null)
        {
            throw new ArgumentNullException(nameof(stream));
        }

        message.WriteTo(stream);
    }

    public static T FromBytes<T>(byte[] bytes) where T : IMessage<T>, new()
    {
        return new MessageParser<T>(() => new T()).ParseFrom(bytes);
    }

    public static T FromBytes<T>(byte[] bytes, int index, int count) where T : IMessage<T>, new()
    {
        return new MessageParser<T>(() => new T()).ParseFrom(bytes, index, count);
    }

    public static object FromBytes(Type type, byte[] bytes, int index, int count)
    {
        if (type == null)
        {
            throw new ArgumentNullException(nameof(type));
        }

        IMessage message = (IMessage)Activator.CreateInstance(type);
        message.MergeFrom(bytes, index, count);
        return message;
    }
}
