using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PK
{
    public class HouseTile : Tile
    {
        [SerializeField] private float houseMultiplayer;
       
        public override void PerformTileAction()
        {
            if (isSold == false) return;
            if (baseIncome == 0 && spawnPoint.houseBaseIncome == 0) return;
            if (spawnPoint._isFree)
            {
                AddMoneySginal.Trigger(baseIncome);

            }
            else
            {
                AddMoneySginal.Trigger(Mathf.RoundToInt((spawnPoint.houseBaseIncome + baseIncome)*houseMultiplayer));
            }
        }
       

    }
}
