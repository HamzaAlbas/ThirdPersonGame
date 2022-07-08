using Cinemachine;
using StarterAssets;
using UnityEngine;
public class TPSController : MonoBehaviour
{
   [SerializeField]private CinemachineVirtualCamera aimVirtualCamera;
   [SerializeField] private float normalSens;
   [SerializeField] private float aimSens;
   [SerializeField] private LayerMask aimColliderLayerMask;
   [SerializeField] private Transform debugTransform;
   [SerializeField] private Transform weaponPos;
   [SerializeField] private Transform vfxHitRed;
   [SerializeField] private Transform vfxHitGreen;
   [SerializeField] private AudioClip pistolClip;

   private int _selectedWeapon;
   
   [Range(0, 1)] public float pistolVolume = 0.5f;
   
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
      Transform hitTransform = null;
      if (Camera.main != null)
      {
         var ray = Camera.main.ScreenPointToRay(screenCenterPoint);
         if (Physics.Raycast(ray, out RaycastHit raycastHit, 999f, aimColliderLayerMask))
         {
            debugTransform.position = raycastHit.point;
            mouseWorldPosition = raycastHit.point;
            hitTransform = raycastHit.transform;
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


      

      if (_starterAssetsInputs.switching > 0)
      {
         _selectedWeapon++;
         if (_selectedWeapon == 3)
         {
            _selectedWeapon = 0;
         }
         Debug.Log(_selectedWeapon);
      }else if (_starterAssetsInputs.switching < 0)
      {
         _selectedWeapon--;
         if (_selectedWeapon == -1)
         {
            _selectedWeapon = 2;
         }
         Debug.Log(_selectedWeapon);
      }
      
      
      
      if (_starterAssetsInputs.shoot)
      {
         AudioSource.PlayClipAtPoint(pistolClip, transform.TransformPoint(weaponPos.position), pistolVolume);
         
         CameraShake.Instance.ShakeCamera();
         if (hitTransform != null)
         {
            Instantiate(hitTransform.GetComponent<BulletTarget>() != null ? vfxHitRed : vfxHitGreen, mouseWorldPosition,
               Quaternion.identity);
         }
         
         _starterAssetsInputs.shoot = false;
      }
   }


}