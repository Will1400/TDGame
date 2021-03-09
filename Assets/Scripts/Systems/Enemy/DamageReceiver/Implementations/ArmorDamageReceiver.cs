﻿using Mirror;
using TDGame.Systems.Enemy.DamageReceiver.Base;
using TDGame.Systems.Enemy.DamageReceiver.Processes;
using TDGame.Systems.Health.Unit;
using TDGame.Systems.Stats;
using UnityEngine;

namespace TDGame.Systems.Enemy.DamageReceiver.Implementations
{
    public class ArmorDamageReceiver : BaseDamageReceiver
    {
        [SerializeField]
        private NetworkedStatsController statsController;
        [SerializeField]
        private NetworkedHealthSystem healthSystem;
        
        private StatWrapper armorStat;

        [SerializeField]
        private string statName = "Armor";

        private void Start()
        {
            armorStat = statsController.GetStatByName(statName);
        }
        
        [ServerCallback]
        public override void Damage(float damage)
        {
            healthSystem.Damage(ArmorProcess.CalculateDamageTaken(damage, armorStat.stat));
        }
    }
}