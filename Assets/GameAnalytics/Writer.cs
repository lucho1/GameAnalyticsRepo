using System.IO;
using UnityEngine;

public static class Writer
{
    public static void Write(string filename, string[,] data) 
    {
        string path = Application.dataPath + "/CSV/" + filename + ".csv";

        int n_columns = data.GetLength(1);
        int n_rows = data.GetLength(0);
        int position = 0;
        string serialized = "";
        foreach (string value in data)
        {
            position++;
            serialized += value;
            if (position % n_columns == 0)
            {
                if (position != n_columns * n_rows)
                    serialized += '\n';
            }
            else
                serialized += ',';
        }

        FileStream file = File.Open(path, FileMode.Create); // will create the file or overwrite it if it already exists
        StreamWriter writer = new StreamWriter (file);
        writer.Write(serialized);
        writer.Close();
        file.Close();
    }

}
