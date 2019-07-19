// 保存数据 居右
// bool Save.Reader();
// bool Save.Writer();

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJson;
using System.Security.Cryptography;
using System.IO;
using System.Text;
using System;

public class SaveData
{
    private static string fileName = Application.dataPath + "Data/PlayerData.txt";
    private static string key = "Beautiful Girls and Strong Men";

    // 游戏开始时加载
    public static bool Reader()
    {
        string txt = DataHelper.ReadText(fileName);
        if (string.IsNullOrEmpty(txt))
        {
            GameData.playerData = new PlayerData();
            Debug.LogWarning("login first time.");
            return false;
        }
        txt = DataHelper.DecryptString(txt, key);
        GameData.playerData  = SimpleJson.SimpleJson.DeserializeObject<PlayerData>(txt);
        return true;
    }

    // 游戏结束和数据改变的时候保存
    public static bool Writer()
    {
        if (GameData.playerData == null)
        {
            return false;
        }
        string jsd = SimpleJson.SimpleJson.SerializeObject(GameData.playerData);
        jsd = DataHelper.EncryptString(jsd, key);
        return DataHelper.WriteText(fileName, jsd);
    }
}

public class DataHelper
{    //加密;
    public static string EncryptString(string str, string key)
    {
        byte[] buffer;
        UTF8Encoding encoding = new UTF8Encoding();
        MD5CryptoServiceProvider provider = new MD5CryptoServiceProvider();
        byte[] buffer2 = provider.ComputeHash(encoding.GetBytes(key));
        TripleDESCryptoServiceProvider provider2 = new TripleDESCryptoServiceProvider
        {
            Key = buffer2,
            Mode = CipherMode.ECB,
            Padding = PaddingMode.PKCS7
        };
        byte[] bytes = encoding.GetBytes(str);
        try
        {
            buffer = provider2.CreateEncryptor().TransformFinalBlock(bytes, 0, bytes.Length);
        }
        finally
        {
            provider2.Clear();
            provider.Clear();
        }
        return Convert.ToBase64String(buffer);
    }

    //解密;
    public static string DecryptString(string str, string key)
    {
        byte[] buffer;
        UTF8Encoding encoding = new UTF8Encoding();
        MD5CryptoServiceProvider provider = new MD5CryptoServiceProvider();
        byte[] buffer2 = provider.ComputeHash(encoding.GetBytes(key));
        TripleDESCryptoServiceProvider provider2 = new TripleDESCryptoServiceProvider
        {
            Key = buffer2,
            Mode = CipherMode.ECB,
            Padding = PaddingMode.PKCS7
        };
        try
        {
            byte[] inputBuffer = Convert.FromBase64String(str);
            buffer = provider2.CreateDecryptor().TransformFinalBlock(inputBuffer, 0, inputBuffer.Length);
        }
        catch (Exception ex)
        {
            Debug.LogError("DecryptString failed. return empty string." + ex.Message);
            return "";
        }
        finally
        {
            provider2.Clear();
            provider.Clear();
        }
        return encoding.GetString(buffer);
    }

    //读取TXT文件
    public static string ReadText(string fileName)
    {
        string str = "";
        using(StreamReader sr = new StreamReader(fileName))
        {
            string line;
            while((line = sr.ReadLine()) != null)
            {
                str = str + line;
            }
        }
        return str;
    }

    //写入TXT文件
    public static bool WriteText(string fileName, string jsd)
    {
        using(StreamWriter sw = new StreamWriter(fileName))
        {
            sw.WriteLine(jsd);
        }
        return true;
    }
}

// 游戏结束或数据改变时保存
// public class SceneManager : MonoBehaviour
// {    
//     void Start()
//     {
//         PlayerDataStore.Reader();
//     }
// 　　 void OnDestroy()
//     {
//         if(PlayerDataStore.Writer()==false)
//         {
//             Debug.LogError("writer file failed.");
//         }     
//     }
// }   