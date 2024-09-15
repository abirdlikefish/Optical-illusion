using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

public class JsonIO
{
    static bool encrypt = false;
    public static void WriteCurSheet<T>(string f_pathPre,string f_name,T curEntity)
    {
        //Debug.Log("write");
        string path = f_pathPre +"/" + f_name + ".json";
        string pathShort = f_pathPre;
        if (!Directory.Exists(pathShort))
        {
            Directory.CreateDirectory(pathShort);
        }
        string str = JsonUtility.ToJson(curEntity, true);
        str = StringToByteString(str);
        File.WriteAllText(path, str);
    }
    public static T ReadCurSheet<T>(string f_pathPre, string f_name)
    {
        T curEntity = default;
        string path = f_pathPre + "/" + f_name + ".json";
        if (!File.Exists(path))
        {
            Debug.Log("path :" + path + " not exist");
        }
        else
        {
            string str = File.ReadAllText(path);
            str = ByteStringToString(str);
            curEntity = JsonUtility.FromJson<T>(str);
        }
        return curEntity;
    }
    public static void Delete(string f_pathPre, string f_name)
    {
        string path = f_pathPre + "/" + f_name + ".json";
        File.Delete(path);
    }
    //加密
    public static string StringToByteString(string str)
    {
        if (!encrypt)
            return str;
        return EncryptDES(Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(str)));
    }

    //解密
    public static string ByteStringToString(string str)
    {
        if (!encrypt)
            return str;
        return System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(DecryptDES(str)));
    }

    #region  字符串加密解密

    private static byte[] Keys = { 0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF };
    /// <summary>
    /// DES加密字符串
    /// </summary>
    /// <param name="encryptString">待加密的字符串</param>
    /// <param name="key">加密密钥,要求为8位</param>
    /// <returns>加密成功返回加密后的字符串，失败返回源串</returns>
    public static string EncryptDES(string encryptString, string key = "13717421")
    {
        try
        {
            byte[] rgbKey = Encoding.UTF8.GetBytes(key.Substring(0, 8));
            byte[] rgbIV = Keys;
            byte[] inputByteArray = Encoding.UTF8.GetBytes(encryptString);
            DESCryptoServiceProvider dCSP = new DESCryptoServiceProvider();
            MemoryStream mStream = new MemoryStream();
            CryptoStream cStream = new CryptoStream(mStream, dCSP.CreateEncryptor(rgbKey, rgbIV), CryptoStreamMode.Write);
            cStream.Write(inputByteArray, 0, inputByteArray.Length);
            cStream.FlushFinalBlock();
            cStream.Close();
            return Convert.ToBase64String(mStream.ToArray());
        }
        catch
        {
            //Debug.LogError("StringEncrypt/EncryptDES()/ Encrypt error!");
            return encryptString;
        }
    }

    /// <summary>
    /// DES解密字符串
    /// </summary>
    /// <param name="decryptString">待解密的字符串</param>
    /// <param name="key">解密密钥,要求为8位,和加密密钥相同</param>
    /// <returns>解密成功返回解密后的字符串，失败返源串</returns>
    public static string DecryptDES(string decryptString, string key = "13717421")
    {
        try
        {
            byte[] rgbKey = Encoding.UTF8.GetBytes(key);
            byte[] rgbIV = Keys;
            byte[] inputByteArray = Convert.FromBase64String(decryptString);
            DESCryptoServiceProvider DCSP = new DESCryptoServiceProvider();
            MemoryStream mStream = new MemoryStream();
            CryptoStream cStream = new CryptoStream(mStream, DCSP.CreateDecryptor(rgbKey, rgbIV), CryptoStreamMode.Write);
            cStream.Write(inputByteArray, 0, inputByteArray.Length);
            cStream.FlushFinalBlock();
            cStream.Close();
            return Encoding.UTF8.GetString(mStream.ToArray());
        }
        catch
        {
            //Debug.LogError("StringEncrypt/DecryptDES()/ Decrypt error!");
            return decryptString;
        }
    }

    #endregion
}
