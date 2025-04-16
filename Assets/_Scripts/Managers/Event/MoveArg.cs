using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class MoveArg
{
    public Unit SOURCE;
    public int DAMAGE;
    public HexTile TARGET;
    public MoveArg(Unit source, int damage, HexTile target)
    {
        SOURCE = source;
        DAMAGE = damage;
        TARGET = target;
    }
}

