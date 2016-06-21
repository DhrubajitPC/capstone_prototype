﻿using UnityEngine;
using System.Collections;
using System;

public class CFDClosestPt
{
    //Coordinates XYZ are in architecture coordinates not unity coordinates (this.Z = Unity Y)
    ImportCsv cfdData = new ImportCsv(WWWLoader.resources_path + "cfd");
    int id = 0;
    public float Vx = 0;
    public float Vy = 0;
    public float Vz = 0;
    public float Wx = 0;
    public float Wy = 0;
    public float Wz = 0;
    public float V = 0;
    public float T = 0;
    public float PMV = 0;
    public float PPD = 0;
    public float PPS = 0;

    public CFDClosestPt(float x, float y)
    {
        float min = 99999999;
        for (int i = 0; i < cfdData.Count; i++)
        {
            float xTest = Math.Abs(x - cfdData.Itemf(i, 0));
            float yTest = Math.Abs(y - cfdData.Itemf(i, 1));

            float Test = xTest + yTest;

            if (Test < min)
            {
                min = Test;
                this.id = i;
            }
        }
        this.Vx = cfdData.Itemf(this.id, 2);
        this.Vy = cfdData.Itemf(this.id, 3);
        this.Vz = cfdData.Itemf(this.id, 4);
        /*
        this.Wx = Mathf.Atan(this.Vz / this.Vy);
        this.Wy = Mathf.Atan(this.Vz / this.Vx);
        this.Wz = Mathf.Atan(this.Vy / this.Vx);
        */
        this.V = Mathf.Sqrt(this.Vx * this.Vx + this.Vy * this.Vy + this.Vz * this.Vz);

        this.T = cfdData.Itemf(this.id, 6);
        this.PMV = cfdData.Itemf(this.id, 7);
        this.PPD = 100 - 95 * Mathf.Exp(-0.03353f * Mathf.Pow(this.PMV, 4f) - 0.2179f * Mathf.Pow(this.PMV, 2f));
        this.PPS = 100 - this.PPD;
    }



}


