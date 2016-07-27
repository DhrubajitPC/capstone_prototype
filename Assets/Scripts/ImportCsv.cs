using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;

public class ImportCsv
{
    List<string[]> CsvList = new List<string[]>();
    
    public ImportCsv(string FilePath, string separator = ",")
    //Get filepath and convert into list of strings
    {
        string CsvValues = "";
        //WWW www = new WWW("file://" + path);
        //yield return www;
        //String text = www.text;
        //CsvValues = ((TextAsset)Resources.Load(FilePath, typeof(TextAsset))).text;
        if (!FilePath.EndsWith(".csv"))
            FilePath += ".csv";
        using (StreamReader CsvReader = new StreamReader(File.OpenRead(FilePath)))
        {
            CsvValues = CsvReader.ReadToEnd();
        }

        string[] CsvLines = CsvValues.Split(Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
        foreach (string CsvLine in CsvLines)
        {
            this.CsvList.Add(CsvLine.Split(separator.ToCharArray(), StringSplitOptions.RemoveEmptyEntries));
        }

    }
    
    public int Count
    //Get length of list
    {
        get
        {
            return CsvList.Count;
        }
    }

    public string[] Line(int row)
    //Get Item in String
    {
        return CsvList[row];
    }


    public string Item(int row, int col)
    //Get Item in String
    {
        return CsvList[row][col];
    }

    public float Itemf(int row, int col)
    //Get Item in Float
    {
        return float.Parse(CsvList[row][col]);
    }   

}

