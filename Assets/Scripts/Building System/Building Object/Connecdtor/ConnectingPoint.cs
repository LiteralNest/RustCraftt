using System.Threading.Tasks;
using UnityEngine;

[RequireComponent(typeof(BoxCollider), typeof(Rigidbody))]
public class ConnectingPoint : MonoBehaviour
{
   [SerializeField] private BuildingConnector _buildingConnector;
   
   private Rigidbody _rb;
   private BoxCollider _boxCollider;
   
   private void Start()
   {
      gameObject.tag = "ConnectingPoint";
      _rb = GetComponent<Rigidbody>();
      _rb.useGravity = false;
      _boxCollider = GetComponent<BoxCollider>();
      _boxCollider.isTrigger = true;
   }
   
   private void OnTriggerEnter(Collider other)
   {
      if (!other.CompareTag("ConnectingPoint")) return;
      var otherConnectingPoint = other.GetComponent<ConnectingPoint>();
      _buildingConnector.AddConnectedBuilding(otherConnectingPoint._buildingConnector);
   }

   private void OnTriggerExit(Collider other)
   {
      if (!other.CompareTag("ConnectingPoint")) return;
      var otherConnectingPoint = other.GetComponent<ConnectingPoint>();
      _buildingConnector.RemoveConnectedBuilding(otherConnectingPoint._buildingConnector);
   }
}
