using System;
using UnityEngine;

[RequireComponent(typeof(CharacterStatsDisplayer))]
public class CharacterStats : MonoBehaviour
{
   public static CharacterStats Singleton { get; private set; }

   [SerializeField] private CharacterStatsDisplayer _statsDisplayer;

   [field:SerializeField] public float Health { get; private set; }
   [field:SerializeField] public float Food { get; private set; }
   [field:SerializeField] public float Water { get; private set; }
   
   
   private float _initialHealth;
   private float _initialFood;
   private float _initialWater;

   private void Awake()
   {
      if (_statsDisplayer == null)
         _statsDisplayer = GetComponent<CharacterStatsDisplayer>();
      Singleton = this;
      
      _initialHealth = Health;
      _initialFood = Food;
      _initialWater = Water;
   }

   private void Update()
   {
      PlayerDead();
   }

   public void ResetStatsToDefault()
   {
      Health = _initialHealth;
      Food = _initialFood;
      Water = _initialWater;

      // Оновити відображення
      _statsDisplayer.DisplayHp((int)Health);
      _statsDisplayer.DisplayFood((int)Food);
      _statsDisplayer.DisplayWater((int)Water);
   }
   private float GetAddedStat(float stat, float addingValue)
   {
      float res = stat + addingValue;
      if(res > 100)
         res = 100;
      return res;
   }
   
   public void PlusStat(CharacterStatType type, float value)
   {
      switch (type)
      {
         case CharacterStatType.Health:
            Health = GetAddedStat(Health, value);
            _statsDisplayer.DisplayHp((int)Health);
            break;
         case CharacterStatType.Food:
            Food = GetAddedStat(Food, value);
            _statsDisplayer.DisplayFood((int)Food);
            break;
         case CharacterStatType.Water:
            Water = GetAddedStat(Water, value);
            _statsDisplayer.DisplayWater((int)Water);
            break;
      }
   }
   
   private float GetSubstractedStat(float stat, float substractingValue)
   {
      var res = stat - substractingValue;
      if(res < 0)
         res = 0;
      return res;
   }
   
   public void MinusStat(CharacterStatType type, float value)
   {
      switch (type)
      {
         case CharacterStatType.Health:
            Health = GetSubstractedStat(Health, value);
            _statsDisplayer.DisplayHp((int)Health);
            PlayerHealthStatus();
            
            break;
         case CharacterStatType.Food:
            Food = GetSubstractedStat(Food, value);
            _statsDisplayer.DisplayFood((int)Food);
            if (Food <= 0)
            {
               Health = GetSubstractedStat(Health, value);
               _statsDisplayer.DisplayHp((int)Health);
            }
            break;
         case CharacterStatType.Water:
            Water = GetSubstractedStat(Water, value);
            _statsDisplayer.DisplayWater((int)Water);
            if (Water <= 0)
            {
               Health = GetSubstractedStat(Health, value);
               _statsDisplayer.DisplayHp((int)Health);
            }
            break;
      }
   }



   private void PlayerHealthStatus()
   {
      switch (Health)
      {
         case <= 5 and > 0:
            PlayerKnockDowned();
            break;
         case <= 0:
            PlayerDead();
            break;
      }
   }
   private void PlayerDead() => GlobalEventsContainer.PlayerDied?.Invoke();
   private void PlayerKnockDowned() => GlobalEventsContainer.PlayerKnockDowned?.Invoke();
   
}
