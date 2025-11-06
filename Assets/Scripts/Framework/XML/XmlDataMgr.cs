using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using UnityEngine;

public class XmlDataMgr 
{
    private static XmlDataMgr instance = new XmlDataMgr();
    public static XmlDataMgr Instance => instance;
    private XmlDataMgr() { }

    public void SaveData(object data,string flieName)
    {
        string path = Application.persistentDataPath + "/" + flieName + ".xml";
        using (StreamWriter writer = new StreamWriter(path))
        {
            XmlSerializer s = new XmlSerializer(data.GetType());
            s.Serialize(writer, data);
        }
    }

    public object LoadData(Type type, string flieName)
    {
        string path = Application.persistentDataPath + "/" + flieName + ".xml";
        if (!File.Exists(path)) 
        {
            path = Application.streamingAssetsPath + "/" + flieName + ".xml";
            if (!File.Exists(path)) 
            {
                return Activator.CreateInstance(type);
            }
        }

        using (StreamReader reader = new StreamReader(path))
        {
            XmlSerializer s = new XmlSerializer(type);
            return s.Deserialize(reader);
        }
    }
}
