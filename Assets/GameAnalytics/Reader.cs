using System.IO;
using System.Collections.Generic;
using UnityEngine;

public class Reader : MonoBehaviour
{
    private static Reader _Instance;
    public static Reader Instance
    {
        get
        {
            if (_Instance == null)
                _Instance = new Reader();
            return _Instance;
        }
    }

    
    public string[][] Read(string filename) 
    {
        return _Instance._Read(filename);
    }

    private string[][] _Read (string filename)
    {
        string path = Application.dataPath + "/CSV/" + filename + ".csv";
        if (File.Exists(path)) 
        {
            
            string data;
            FileStream fileStream = new FileStream(path, FileMode.Open, FileAccess.ReadWrite);
            StreamReader read = new StreamReader(fileStream);
            data = read.ReadToEnd();

            if (data == "")
            {
                Debug.LogError("File at " + path + " is empty");
                return null;
            }

            string[] rows = data.Split('\n');

            List<string[]> ret = new List<string[]>();
            foreach (string row in rows) 
            {
                ret.Add(row.Split(','));
            }

            return ret.ToArray();
        }
        else
        {
            Debug.LogError("File at " + path + " does not exist");
            return null;
        }
    }
}


