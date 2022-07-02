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

   private StarterAssetsInputs _starterAssetsInputs;
   private ThirdPersonController _thirdPersonController;
   

   private void Awake()
   {
      _thirdPersonController = GetComponent<ThirdPersonController>();
      _starterAssetsInputs = GetComponent<StarterAssetsInputs>();
   }


   private void Update()
   {
      if (_starterAssetsInputs.aim)
      {
         aimVirtualCamera.gameObject.SetActive(true);
         _thirdPersonController.SetSensitivity(aimSens);
      }
      else
      {
         aimVirtualCamera.gameObject.SetActive(false);
         _thirdPersonController.SetSensitivity(normalSens);
      }

      var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
      if (Physics.Raycast(ray, out RaycastHit raycastHit, 999f, mouseColliderLayerMask))
      {
         transform.position = raycastHit.point;
      }
      {
         
      }
   }
}
