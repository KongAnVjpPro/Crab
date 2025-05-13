using UnityEngine;
using System.Linq;
using System;
using System.Collections.Generic;

[Serializable]
public class CombinedShellData
{
    [SerializeField] public string shellName = "Mysteriour Shell Level ";
    [SerializeField] public string shellDescription = "This shell is combined of";
    [Header("Part of Shell: ")]
    public List<ShellID> baseShellIDs;
    public List<ShellAbilityID> combinedAbilities;
    public Color decorColor;
    public Color mainColor;
    public Vector2 triangle1Scale, triangle2Scale, triangle3Scale, triangle4Scale;

    public CombinedShellData(List<ShellSO> baseShells)
    {
        baseShellIDs = baseShells.Select(s => s.id).OrderBy(id => id).ToList();
        combinedAbilities = baseShells.SelectMany(s => s.abilityID).Distinct().ToList();


        mainColor = AvgColor(baseShells.Select(s => s.mainColor));
        decorColor = AvgColor(baseShells.Select(s => s.decorColor));

        triangle1Scale = AvgVector(baseShells.Select(s => s.triangle1Scale));
        triangle2Scale = AvgVector(baseShells.Select(s => s.triangle2Scale));
        triangle3Scale = AvgVector(baseShells.Select(s => s.triangle3Scale));
        triangle4Scale = AvgVector(baseShells.Select(s => s.triangle4Scale));

        GenerateNameAndDescription(baseShells);
    }

    void GenerateNameAndDescription(List<ShellSO> baseShells)
    {
        if (baseShells.Count == 1)
        {
            shellName = baseShells[0].shellName;
            shellDescription = baseShells[0].shellDescription;
        }
        else
        {

            shellName = $"Ancient Shell Lv.{baseShells.Count}";

            var partNames = baseShells.Select(s => s.shellName);
            shellDescription = $"This shell is forged from:\n  " + string.Join("\n  ", partNames);
        }
    }
    Color AvgColor(IEnumerable<Color> colors)
    {
        var cList = colors.ToList();
        float r = 0, g = 0, b = 0, a = 0;
        foreach (var c in cList)
        {
            r += c.r; g += c.g; b += c.b; a += c.a;
        }
        float count = cList.Count;
        return new Color(r / count, g / count, b / count, a / count);
    }

    Vector2 AvgVector(IEnumerable<Vector2> vectors)
    {
        var vList = vectors.ToList();
        float x = 0, y = 0;
        foreach (var v in vList)
        {
            x += v.x; y += v.y;
        }
        float count = vList.Count;
        return new Vector2(x / count, y / count);
    }

    public string GetKey()
    {
        return string.Join("_", baseShellIDs.OrderBy(id => id));
    }
}