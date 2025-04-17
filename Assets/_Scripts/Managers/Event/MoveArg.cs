using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


public class MoveArg
{
    public Unit SOURCE;
    public int DAMAGE;
    public List<Vector3Int> PATH;
    public MoveArg(Unit source, int damage, List<Vector3Int> path)
    {
        SOURCE = source;
        DAMAGE = damage;
        PATH = path;
    }
}

