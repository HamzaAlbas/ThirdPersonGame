using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using StarterAssets;
using UnityEngine;

public class TPSController : MonoBehaviour
{
   [SerializeField]private CinemachineVirtualCamera aimVirtualCamera;
   [SerializeField] private float normalSens;
   [SerializeField] private float aimSens;
   [SerializeField] private LayerMask aimColliderLayerMask = new LayerMask();
   [SerializeField] private Transform debugTransform;
   [SerializeField] private Transform bulletPf;
   [SerializeField] private Transform bulletSpawnPos;

   private StarterAssetsInputs _starterAssetsInputs;
   private ThirdPersonController _thirdPersonController;
   

   private void Awake()
   {
      _thirdPersonController = GetComponent<ThirdPersonController>();
      _starterAssetsInputs = GetComponent<StarterAssetsInputs>();
   }


   private void Update()
   {
      var mouseWorldPosition = Vector3.zero;
      var screenCenterPoint = new Vector2(Screen.width / 2f, Screen.height /2f);
      if (Camera.main != null)
      {
         var ray = Camera.main.ScreenPointToRay(screenCenterPoint);
         if (Physics.Raycast(ray, out RaycastHit raycastHit, 999f, aimColliderLayerMask))
         {
            debugTransform.position = raycastHit.point;
            mouseWorldPosition = raycastHit.point;
         }
      }

      if (_starterAssetsInputs.aim)
      {
         aimVirtualCamera.gameObject.SetActive(true);
         _thirdPersonController.SetSensitivity(aimSens);
         _thirdPersonController.SetRotateOnMove(false);

         var worldAimTarget = mouseWorldPosition;
         var transform1 = transform;
         var position = transform1.position;
         worldAimTarget.y = position.y;
         var aimDirection = (worldAimTarget - position).normalized;
         
         transform1.forward = Vector3.Lerp(transform1.forward, aimDirection,Time.deltaTime * 20f);
      }
      else
      {
         aimVirtualCamera.gameObject.SetActive(false);
         _thirdPersonController.SetSensitivity(normalSens);
         _thirdPersonController.SetRotateOnMove(true);
      }


      if (_starterAssetsInputs.shoot)
      {
         var spawnPos = bulletSpawnPos.position;
         var aimDir = (mouseWorldPosition - spawnPos).normalized;
         Instantiate(bulletPf, spawnPos, Quaternion.LookRotation(aimDir, Vector3.up));
         _starterAssetsInputs.shoot = false;
      }
   }
}
