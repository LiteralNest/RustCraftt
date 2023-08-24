using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Resource")]
public class Resource : ScriptableObject
{
   [field:SerializeField] public int Id { get; private set; }
   [field:SerializeField] public string Name { get; private set; }
   [field:SerializeField] public Sprite Icon { get; private set; }
}
