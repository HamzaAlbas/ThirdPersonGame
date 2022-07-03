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
   [SerializeField] private Transform vfxHitRed;
   [SerializeField] private Transform vfxHitGreen;

   private StarterAssetsInputs _starterAssetsInputs;
   private ThirdPersonController _thirdPersonController;

   private Animator _animator;

   private void Awake()
   {
      _thirdPersonController = GetComponent<ThirdPersonController>();
      _starterAssetsInputs = GetComponent<StarterAssetsInputs>();
      _animator = GetComponent<Animator>();
   }


   private void Update()
   {
      var mouseWorldPosition = Vector3.zero;
      var screenCenterPoint = new Vector2(Screen.width / 2f, Screen.height /2f);
      Transform _hitTransform = null;
      if (Camera.main != null)
      {
         var ray = Camera.main.ScreenPointToRay(screenCenterPoint);
         if (Physics.Raycast(ray, out RaycastHit raycastHit, 999f, aimColliderLayerMask))
         {
            debugTransform.position = raycastHit.point;
            mouseWorldPosition = raycastHit.point;
            _hitTransform = raycastHit.transform;
         }
      }

      if (_starterAssetsInputs.aim)
      {
         aimVirtualCamera.gameObject.SetActive(true);
         _thirdPersonController.SetSensitivity(aimSens);
         _thirdPersonController.SetRotateOnMove(false);
         _animator.SetLayerWeight(1, Mathf.Lerp(_animator.GetLayerWeight(1), 1f, Time.deltaTime * 10f));

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
         _animator.SetLayerWeight(1, Mathf.Lerp(_animator.GetLayerWeight(1), 0f, Time.deltaTime * 10f));

      }


      if (_starterAssetsInputs.shoot)
      {
         if (_hitTransform != null)
         {
            if (_hitTransform.GetComponent<BulletTarget>() != null)
            {
               Instantiate(vfxHitRed, mouseWorldPosition, Quaternion.identity);
            }
            else
            {
               Instantiate(vfxHitGreen, mouseWorldPosition, Quaternion.identity);
            }
         }
         /* Shooting With Projectile 
          
         var spawnPos = bulletSpawnPos.position;
         var aimDir = (mouseWorldPosition - spawnPos).normalized;
         Instantiate(bulletPf, spawnPos, Quaternion.LookRotation(aimDir, Vector3.up));*/
         _starterAssetsInputs.shoot = false;
      }
   }


}
