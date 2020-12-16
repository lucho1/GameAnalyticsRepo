using System.IO;
using UnityEngine;

public class Writer : MonoBehaviour
{
    private static Writer _Instance;
    public static Writer Instance
    {
        get
        {
            if (_Instance == null)
                _Instance = new Writer();
            return _Instance;
        }
    }

    //filename without extension or path
    public void Write(string filename, string[,] data) 
    { 
        _Instance._Write(filename, data);
    }

    private void _Write(string filename, string[,] data) 
    {
        string path = Application.dataPath + "/CSV/" + filename + ".csv";

        int n_columns = data.GetLength(0);
        int position = 0;
        string serialized = "";
        foreach (string value in data)
        {
            position++;
            serialized += value;
            if (position % n_columns == 0)
                serialized += '\n';
            else
                serialized += ',';
        }

         StreamWriter writer = new StreamWriter (path);
         writer.WriteLine(serialized);
         writer.Close();
    }

}
