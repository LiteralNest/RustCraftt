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
      }
   }
}
