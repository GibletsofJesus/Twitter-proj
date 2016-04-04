﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class numbersToSliders : MonoBehaviour
{
    public Slider scaleSlider, heightSlider;
    public InputField rtsInput;
    int[] rankings =
        {
            0,
            3,
            6,
            10,
            19,
            35,
            65,
            120,
            222,
            412,
            761,
            1409,
            2606,
            4821,
            8920,
            16502,
            30528,
            56477,
            104482,
            193291,
            357589,
            661540,
            1223848,
            2264119,
            4188620,
        };

    int max=0;

    public float findRank(float num, bool height)
    {
        if (height)
            if (max != 0)
                num = max;

        if (num > 4188620)
            num = 4188620;
        int rank = 0;
        for (int i = 0; i < rankings.Length-1; i++)
        {
            if (num >= rankings[i])
                rank++;
        }
        return rank +((num - rankings[rank - 1]) / (rankings[rank] - rankings[rank - 1]));
    }

    public void RTs(string RTs)
    {
        if (RTs != "")
        {
            float j = findRank(float.Parse(RTs),false);
            j /= 25;
            j *= scaleSlider.maxValue;
            scaleSlider.value = j;
            GameObject.Find("PentTest").GetComponent<PentInfo>().UpdatePentMesh();
        }
    }

    public float RTs(int RTs)
    {
        float j = findRank((float)(RTs),false);
        j /= 25;
        j *= 50;
        return j;
    }

    public void favs(string favs)
    {
        if (favs != "")
        {
            float j = findRank(float.Parse(favs),true);
            j /= 25;
            j *= heightSlider.maxValue;
            heightSlider.value = j;
        }
    }

    public float favs(int favs)
    {
        float j = findRank((float)favs,true);
        j /= 25;
        j *= 1;
        return j;
    }

    public void scaleToRTs(float scale)
    {
        scale /= scaleSlider.maxValue;
        scale *= 25;
        var rem = scale % 1;

        if ((int)scale-1 < 0)
        {
            rtsInput.text = "0";
        } 
        else if (scale > 24)
        {
            float RTs = rankings[(int)scale - 1];
            rtsInput.text = "" + (int)RTs;
        }
        else
        {
            scale -= rem;
            float RTs = rankings[(int)scale - 1] + (rem * (rankings[(int)scale] - rankings[(int)scale - 1]));
            rtsInput.text = "" + (int)RTs;
        }
    }

    public void heightToFavs(float favs)
    {
        favs /= heightSlider.maxValue;
        favs *= 25;
        var rem = favs % 1;
        favs -= rem;

        if ((int)favs - 1 < 0)
        { 
            GetComponent<InputField>().text = "0";
        }
        else if (favs > 24)
        {
            float favser = rankings[(int)favs - 1];
            GetComponent<InputField>().text = "" + (int)favser;
        }
        else
        {
            float favser = rankings[(int)favs - 1] + (rem * (rankings[(int)favs] - rankings[(int)favs - 1]));
            GetComponent<InputField>().text = "" + (int)favser;
        }
    }

    public void setMax(bool b)
    {
        if (b)
            max = 4188620;
        else
            max = 0;
    }
}
