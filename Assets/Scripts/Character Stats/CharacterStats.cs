using System;
using System.Threading.Tasks;
using UnityEngine;

[RequireComponent(typeof(CharacterStatsDisplayer))]
public class CharacterStats : MonoBehaviour
{
   public static CharacterStats Singleton { get; private set; }

   [SerializeField] private CharacterStatsDisplayer _statsDisplayer;

   [field:SerializeField] public float Health { get; private set; }
   [field:SerializeField] public float Food { get; private set; }
   [field:SerializeField] public float Water { get; private set; }
   [field:SerializeField] public float Oxygen { get; private set; }

   [SerializeField] private GameObject _OxygenPanel;

   private float _currentOxygen;
   private float _initialHealth;
   private float _initialFood;
   private float _initialWater;
   private float _minussingValueOxygen = 10f;
   
   private void Awake()
   {
      if (_statsDisplayer == null)
         _statsDisplayer = GetComponent<CharacterStatsDisplayer>();
      Singleton = this;
      
      _initialHealth = Health;
      _initialFood = Food;
      _initialWater = Water;
      _currentOxygen = Oxygen;
   }

   private void Update()
   {
      if (_currentOxygen >= 100)
      {
         _OxygenPanel.SetActive(false);
      }
   }

   public void ResetStatsToDefault()
   {
      Health = _initialHealth;
      Food = _initialFood;
      Water = _initialWater;
      Oxygen = _currentOxygen;

      // Оновити відображення
      _statsDisplayer.DisplayHp((int)Health);
      _statsDisplayer.DisplayFood((int)Food);
      _statsDisplayer.DisplayWater((int)Water);
      _statsDisplayer.DisplayOxygen((int)Oxygen);
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
         case CharacterStatType.Oxygen:
            Oxygen = GetAddedStat(Oxygen, value);
            _statsDisplayer.DisplayOxygen((int)Oxygen);
            break;
      }
   }
   
   private float GetSubstractedStat(float stat, float substractingValue)
   {
      float res = stat - substractingValue;
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
            if (Health <= 0)
            {
               // GlobalEventsContainer.PlayerDied?.Invoke();
               // _statsDisplayer.DisplayDeathMessage("You died!", Color.red);
            }
            break;
         case CharacterStatType.Food:
            Food = GetSubstractedStat(Food, value);
            _statsDisplayer.DisplayFood((int)Food);
            if (Food <= 0)
            {
               // GlobalEventsContainer.PlayerDied?.Invoke();
               // _statsDisplayer.DisplayDeathMessage("You died!", Color.green);
            }
            break;
         case CharacterStatType.Water:
            Water = GetSubstractedStat(Water, value);
            _statsDisplayer.DisplayWater((int)Water);
            if (Water <= 0)
            {
               // GlobalEventsContainer.PlayerDied?.Invoke();
               // _statsDisplayer.DisplayDeathMessage("You died!", Color.blue);
            }
            break;
         case CharacterStatType.Oxygen:
            Oxygen = GetSubstractedStat(Oxygen, value);
            _statsDisplayer.DisplayOxygen((int)Oxygen);
            if (Oxygen < 0)
            {
               
            }
            break;
      }
   }
   
   
   public async void SubstractStatsOxygenMinus(bool statement)
   {
      if (!statement) return;
      _OxygenPanel.SetActive(true);
      await Task.Delay(1000);
      MinusStat(CharacterStatType.Oxygen, _minussingValueOxygen);
   }

   public async void SubstractStatsOxygenPlus(bool statement)
   {
      if (!statement) return;
      await Task.Delay(1000);
      PlusStat(CharacterStatType.Oxygen, 1);
   }
}
