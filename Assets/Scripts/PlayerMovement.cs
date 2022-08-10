using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public static PlayerMovement Current;
    public float limitX;

    public float runSpeed;
    public float xSpeed;

    public GameObject RidingCylinderPrefab;
    public List<RidingCylinder> cylinders;

    public bool _spawnBridge;
    public GameObject bridgePiecePrefab;
    private BridgeSPW _bridgeSpawner;
    float _creatingBridgeTimer;

    private float _currentRunSpeed;
    void Start()
    {
        Current = this;
        
        
        
        
    }

    
    void Update()
    {
        if (LevelController.Current == null || !LevelController.Current.gameActive)
        {
            return;

        }
        float newX = 0f;
        float touchXDelta = 0f;

        if(Input.touchCount> 0 && Input.GetTouch(0).phase == TouchPhase.Moved)
        {
            touchXDelta = Input.GetTouch(0).deltaPosition.x / Screen.width;

        }
        else if (Input.GetMouseButton(0))
        {
            touchXDelta = Input.GetAxis("Mouse X");
        }
        newX = transform.position.x + xSpeed * touchXDelta * Time.deltaTime;
        newX = Mathf.Clamp(newX, -limitX, limitX);

        Vector3 newPosition = new Vector3(newX, transform.position.y, transform.position.z + _currentRunSpeed * Time.deltaTime);
        transform.position = newPosition;

        if (_spawnBridge)
        {
            _creatingBridgeTimer -= Time.deltaTime;
            if (_creatingBridgeTimer < 0)
            {
                _creatingBridgeTimer = 0.01f;
                CylinderVolume(-0.01f);
                GameObject createdBridgePiece = Instantiate(bridgePiecePrefab);
                Vector3 direction = _bridgeSpawner.EndReference.transform.position - _bridgeSpawner.StartReference.transform.position;
                float distance = direction.magnitude;
                direction = direction.normalized;
                createdBridgePiece.transform.forward = direction;
                float characterDistance = transform.position.z - _bridgeSpawner.StartReference.transform.position.z;
                characterDistance = Mathf.Clamp(characterDistance, 0, distance);
                Vector3 newPiecePosition = _bridgeSpawner.StartReference.transform.position + direction * characterDistance;
                newPiecePosition.x = transform.position.x;
                createdBridgePiece.transform.position = newPiecePosition;
            }
        }
    }
    public void ChangeSpeed(float value)
    {
        _currentRunSpeed = value;

    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "AddCylinder")
        {
            CylinderVolume(0.1f);
            Destroy(other.gameObject);
        }
        else if(other.tag == "SpawnBridge")
        {
            StartSpawnBridge(other.transform.parent.GetComponent<BridgeSPW>());
        }
        else if(other.tag == "StopSpawnBridge")
        {
            StopSpawnBridge();
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if(other.tag == "Trap")
        {
            CylinderVolume(-Time.fixedDeltaTime);
        }
    }

    public void CylinderVolume(float value)
    {
        if(cylinders.Count == 0)
        {
            if (value > 0)
            {
                CreateCylinder(value);
            }
            else
            {
                
            }

        }
        else
        {
            cylinders[cylinders.Count - 1].CylinderVolume(value);
        }

    }

    public void CreateCylinder(float value)
    {
        RidingCylinder createdCylinder = Instantiate(RidingCylinderPrefab, transform).GetComponent<RidingCylinder>();
        cylinders.Add(createdCylinder);
        createdCylinder.CylinderVolume(value);
    }

    public void DestroyCylinder(RidingCylinder cylinder)
    {
        cylinders.Remove(cylinder);
        Destroy(cylinder.gameObject);

    }

    public void StartSpawnBridge(BridgeSPW spawner)
    {
        _bridgeSpawner = spawner;
        _spawnBridge = true;


    }

    public void StopSpawnBridge()
    {
        _spawnBridge = false;
    }
}
