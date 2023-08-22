using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stats
{
    public Dictionary<string,float> stats;
    bool Rep = false;

    public Stats()
    {
        stats = new Dictionary<string, float>();
    }
    public Stats(string[] var)
    {
        stats = new Dictionary<string, float>();
        Set(var);
    }
    public Stats Copy()
    {
        Stats s = new Stats();
        foreach (string sts in stats.Keys)
        {
            s.stats[sts] = stats[sts];
        }
        return s;
    }

    public bool isRep
    {
        get { return Rep; }
        set { Rep = value; }
    }
    

    public Stats CopyBFilter(string name)
    {
        Stats s = new Stats();
        foreach (string sts in stats.Keys)
        {
            if (sts.Contains(name))
            {
                s.stats[sts] = stats[sts];
            }
        }
        return s;
    }
    public Stats CopyFilter(string name)
    {
        Stats s = new Stats();
        foreach (string sts in stats.Keys)
        {
            if (!sts.Contains(name))
            {
                s.stats[sts] = stats[sts];
            }
        }
        return s;
    }

    public float Get(string name)
    {
        try
        {
            return stats[name];
        } catch
        {
            return 0;
        }
    }

    public float GetP(string name)
    {
        try
        {
            if (stats.ContainsKey(name + "%"))
            {
                return stats[name] * (stats[name+"%"]*0.01f);
            }
            else
            {
                return stats[name];
            }
        }
        catch
        {
            return 0;
        }
    }
    public void Add(string name, float value)
    {
        if (!stats.ContainsKey(name))
        {
            if (name.Contains("%"))
                stats[name] = 100;
            else
                stats[name] = 0;

        }

        stats[name] += value;
        Rep = true;
    }

    public void Mul(string name, float value)
    {
        if (!stats.ContainsKey(name))
        {
            if (name.Contains("%"))
                stats[name] = 100;
            else
                stats[name] = 0;
        }

        stats[name] *= value;
        Rep = true;
    }
    public void Set(string name, float value)
    {
        stats[name] = value;
        Rep = true;
    }
    public void Set(string[] value)
    {
        foreach (string str in value)
        {
            stats[str.Split(":")[0]] = float.Parse(str.Split(":")[1]);
        }
        Rep = true;
    }
    public Stats ReSet()
    {
        stats.Clear();
        Rep = true;
        return this;
    }

    public static Stats operator -(Stats s1, Stats s2)
    {
        foreach (string sts in getKey(s2))
        {
            if (s1.stats.ContainsKey(sts))
            {
                s1.stats[sts] -= s2.stats[sts];
            }
            else
            {
                s1.stats[sts] = s2.stats[sts];
            }
        }
        s1.Rep = true;
        return s1;
    }
    public static Stats operator +(Stats s1, Stats s2)
    {
        foreach (string sts in getKey(s2))
        {
            if (s1.stats.ContainsKey(sts))
            {
                s1.stats[sts] += s2.stats[sts];
            }
            else
            {
                s1.stats[sts] = s2.stats[sts];
            }
        }
        s1.Rep = true;
        return s1;
    }
    public static Stats operator *(Stats s1, Stats s2)
    {
        foreach (string sts in getKey(s2))
        {
            if (s1.stats.ContainsKey(sts))
            {
                s1.stats[sts] *= s2.stats[sts];
            }
            else
            {
                s1.stats[sts] = s2.stats[sts];
            }
        }
        s1.Rep = true;
        return s1;
    }
    public static Stats operator /(Stats s1, Stats s2)
    {
        foreach (string sts in getKey(s2))
        {
            if (s1.stats.ContainsKey(sts))
            {
                s1.stats[sts] /= s2.stats[sts];
            }
            else
            {
                s1.stats[sts] = s2.stats[sts];
            }
        }
        s1.Rep = true;
        return s1;
    }
    public static Stats operator -(Stats s1, float val)
    {
        foreach (string sts in getKey(s1))
        {
            s1.stats[sts] -= val;
        }
        s1.Rep = true;
        return s1;
    }
    public static Stats operator +(Stats s1, float val)
    {
        foreach (string sts in getKey(s1))
        {
            s1.stats[sts] += val;
        }
        s1.Rep = true;
        return s1;
    }
    public static Stats operator *(Stats s1, float val)
    {
        foreach (string sts in getKey(s1))
        {
            s1.stats[sts] *= val;
        }
        s1.Rep = true;
        return s1;
    }
    public static Stats operator /(Stats s1, float val)
    {
        foreach (string sts in getKey(s1))
        {
            s1.stats[sts] /= val;
        }
        s1.Rep = true;
        return s1;
    }

    static List<string> getKey(Stats s)
    {
        try
        {
            return new List<string>(s.stats.Keys);
        }
        catch { }
        return new List<string>();
    }
}
