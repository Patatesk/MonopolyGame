using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PK
{
    public class LuckTile : Tile
    {


        public override void ShowCanvas()
        {
        }

        public override void PerformTileAction()
        {
            AddMoneySginal.Trigger(baseIncome);
        }
    }
}
