using System.Data;
using UnityEngine;

public static class MathUtil
{
    public static int Evaluate(string expression)
    {
        var table = new DataTable();
        object result = table.Compute(expression, "");
        return Mathf.RoundToInt(System.Convert.ToSingle(result));
    }
}