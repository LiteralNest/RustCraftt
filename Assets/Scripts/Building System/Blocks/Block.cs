using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Outline))]
public class Block : MonoBehaviour
{
   [field:SerializeField] public int Hp { get; protected set; }
   [field:SerializeField] public List<InventoryCell> CellForPlace { get; private set; }
   [SerializeField] private Outline _outline;

   private void Awake()
   {
      if (_outline == null)
         _outline = GetComponent<Outline>();
   }

   public void TurnOutline(bool value)
       => _outline.enabled = value;
}
