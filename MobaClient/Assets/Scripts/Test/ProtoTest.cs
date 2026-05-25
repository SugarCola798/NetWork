using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ProtoMsg;
using Game.Net;

public class ProtoTest : MonoBehaviour
{
    USocket uSocket;
    void Start()
    {
        uSocket = new USocket(DispatchNetEvent);

        TestSend();



        ////byte[] 字节数组 
        ////反序列化 举例
        //UserRegisterC2S userRegisterC2S1= ProtobufHelper.FromBytes<UserRegisterC2S>(bufferEntity.proto);
    }

    private static void TestSend()
    {
        UserInfo userInfo = new UserInfo();
        userInfo.ID = 1;
        userInfo.Account = "11111";
        userInfo.Password = "kkkkk";

        UserRegisterC2S userRegisterC2S = new UserRegisterC2S();
        userRegisterC2S.UserInfo = userInfo;
        BufferEntity bufferEntity = BufferFactory.CreateAndSendPackage(1001, userInfo);
    }

    void DispatchNetEvent(BufferEntity buffer)
    { 
        //进行报文分发
    }


    // Update is called once per frame
    void Update()
    {
        if (uSocket!=null)
        {
            uSocket.Handle();
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            TestSend();
        }
    }
}
