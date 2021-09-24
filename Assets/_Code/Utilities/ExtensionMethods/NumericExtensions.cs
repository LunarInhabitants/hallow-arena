using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public static class NumericExtensions
{
    public static string AsMoney(this int v, string currency = "£")
    {
        return $"{currency}{(v / 100.0).ToString("0.00")}";
    }

    public static float Remap(this float v, float fS, float fE, float tS, float tE)
    {
        return (v - fS) / (fE - fS) * (tE - tS) + tS;
    }
}