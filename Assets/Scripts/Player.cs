using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : NetworkBehaviour, IResetable {
    public static event EventHandler OnAnyPlayerSpawned;
    public static Player LocalInstance { get; private set; }
    public event EventHandler<OnSelectedElementChangedEventArgs> OnSelectedObjectChanged;
    public class OnSelectedElementChangedEventArgs : EventArgs {
        public SelectableObject selectedElement;
    }

    [Header("Player movement")]
    [SerializeField] private float moveSpeed = 1f;
    [SerializeField] private LayerMask objectLayerMask;
    [SerializeField] private Plank ghostPlank;
    [SerializeField] private float fallGravityScale = 5;
    [SerializeField] private GameObject ObjectHoldPoint;
    [SerializeField] private List<Vector3> spawnPositionList;
    [SerializeField] private PlayerVisual playerVisual;
    [SerializeField] private Plank plank;
    [SerializeField] private Plank staticPlank;
    [SerializeField] private PlayerAnimator myAnimator;

    private PlayerData playerData;
    private CharacterController controller;
    private float gravity = -9.81f;
    private Vector3 playerVelocity;



    //private Rigidbody rb;
    private bool isWalking;
    private float rotateSpeed = 10f;
    private Vector3 lastInteractDir;
    private Vector3 respawnPosition;
    private SelectableObject selectedElement;
    private bool carryingElement = false;


    // Start is called before the first frame update
    void Start() {
        controller = gameObject.GetComponent<CharacterController>();
        
        playerData = GameMultiplayer.Instance.GetPlayerDataFromClientId(OwnerClientId);
        
        playerVisual.SetPlayerColor(GameMultiplayer.Instance.GetPlayerColor(playerData.colorId));
    }

    public FixedString64Bytes GetPlayerId() {
        return playerData.playerId;
    }
    public override void OnNetworkSpawn() {
        if (IsOwner) {
            LocalInstance = this;
            Sub();
        }
        //SceneManager.sceneLoaded += this.OnLoadCallback;
        controller = gameObject.GetComponent<CharacterController>();
        controller.enabled = false;
        transform.position = spawnPositionList[(int)OwnerClientId];
        controller.enabled = true;
        respawnPosition = spawnPositionList[(int)OwnerClientId];
        OnAnyPlayerSpawned?.Invoke(this, EventArgs.Empty);
        if (IsServer) {
            NetworkManager.Singleton.OnClientConnectedCallback += NetworkManager_OnClientConnectedCallback;
        }
    }

    private void NetworkManager_OnClientConnectedCallback(ulong clientId) {
        //TODO ngo video 3:13:35

    }


    void Sub() {
        CinemachineVirtualCamera camera = CinemachineVirtualCamera.FindObjectOfType<CinemachineVirtualCamera>();
        camera.LookAt = gameObject.transform;
        camera.Follow = gameObject.transform;
        GameInput.Instance.OnInteractAction += GameInput_OnInteractAction;
        GameInput.Instance.OnJumpAction += GameInput_OnJumpAction;
        GameInput.Instance.OnPlaceSimpleAction += GameInput_OnPlaceSimpleAction;
        GameInput.Instance.OnPlaceForeverAction += GameInput_OnPlaceForeverAction;
        GameInput.Instance.RotateYAction += GameInput_RotateYAction;
        GameInput.Instance.RotateZAction += GameInput_RotateZAction;
    }


    //Rotate Plank on plane
    private void GameInput_RotateYAction(object sender, EventArgs e) {
        if (ghostPlank.isActiveAndEnabled) {
            //Vector3 newRotation = ghostPlank.transform.rotation.eulerAngles + new Vector3(0, 90, 0);
            ghostPlank.transform.Rotate(new Vector3(0, 90, 0));
        }
    }

    private void GameInput_RotateZAction(object sender, EventArgs e) {
        if (ghostPlank.isActiveAndEnabled) {
            //Vector3 newRotation = ghostPlank.transform.rotation.eulerAngles + new Vector3(0, 0, -30);
            ghostPlank.transform.Rotate(new Vector3(0, 0, 30));
        }
    }


    private void GameInput_OnPlaceForeverAction(object sender, EventArgs e) {
        if (carryingElement == true) {
            PlacePlank(true);
        }
    }

    private void GameInput_OnPlaceSimpleAction(object sender, EventArgs e) {
        throw new NotImplementedException();
    }

    private void GameInput_OnInteractAction(object sender, EventArgs e) {
        if (carryingElement == true) {
            PlacePlank(false);
        }
        else if (selectedElement != null) {
            if (selectedElement.TryGetComponent<StaticPlank>(out StaticPlank sp)) {
                return;
            }
            if (selectedElement.TryGetComponent<Plank>(out Plank plank)) {
                PlayerConsumables.Instance.AddPlank();
                SetCarryingElement(ObjectHoldPoint);
                selectedElement.Interact(this);
                ObjectHoldPoint.SetActive(true);
            }
            else if (selectedElement.TryGetComponent<Nail>(out Nail nail)) {
                PlayerConsumables.Instance.AddNail();
                selectedElement.Interact(this);
            }
        }
    }
    private void GameInput_OnJumpAction(object sender, EventArgs e) {
        if (!GameManager.Instance.IsGamePlaying()) return;
        if (controller.isGrounded) {
            myAnimator.SetJumping();
            playerVelocity.y += Mathf.Sqrt(-14 * gravity);
            
        }
    }

    // Update is called once per frame
    void Update() {
        if (!IsOwner) {
            return;
        }
        HandleMovement();
        HandleInteractions();

    }
    private void FixedUpdate() {
        if (!IsOwner) {
            return;
        }
        HandleGravity();
    }
    private void HandleGravity() {
        if (playerVelocity.y < 0) {
            if (controller.isGrounded) {
                playerVelocity.y = 0f;
            }
            else {
                //Player is falling
                playerVelocity.y += fallGravityScale * gravity * Time.deltaTime;
                myAnimator.SetFalling();
            }
        }
        playerVelocity.y += fallGravityScale * gravity * Time.deltaTime;
    }


    private void HandleMovement() {
        Vector2 inputVector = GameInput.Instance.GetMovementVector();
        Vector3 moveDir = new Vector3(inputVector.x, 0, inputVector.y);
        float moveDistance = moveSpeed * Time.deltaTime;
        float playerRadius = .27f;
        float playerHeight = 2f;
        bool canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDir, moveDistance);
        if (!canMove) {
            //Attempt only x movement
            //could normalize for more speed
            Vector3 moveDirX = new Vector3(moveDir.x, 0, 0);
            canMove = moveDir.x != 0 && !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirX, moveDistance);
            if (canMove) {
                moveDir = moveDirX;
            }
            else {
                //attempt only z movement
                //could normalize for more speed
                Vector3 moveDirZ = new Vector3(moveDir.x, 0, 0);
                canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirZ, moveDistance);
                if (canMove) {
                    moveDir = moveDirZ;
                }
                else {
                    //cannot move
                }
            }
        }
        
        Vector3 finalMove = new Vector3(moveDir.x * moveSpeed, playerVelocity.y * Time.deltaTime, moveDir.z * moveSpeed);
        if (finalMove.y < 0.01 && finalMove.y > -0.01) {
            if (finalMove.x != 0 || finalMove.z != 0) {
                myAnimator.SetRunning();
            }
            else {
                myAnimator.SetIdle();
            }
        }
        controller.Move(finalMove);
        //}
        isWalking = moveDir != Vector3.zero;
        if (isWalking) {
            transform.forward = Vector3.Slerp(transform.forward, moveDir, rotateSpeed * Time.deltaTime);
        }
    }
    
    private void HandleInteractions() {
        Vector2 inputVector = GameInput.Instance.GetMovementVector();
        Vector3 moveDir = new Vector3(inputVector.x, 0, inputVector.y);

        float interactDistance = 2f;
        if (moveDir != Vector3.zero) {
            lastInteractDir = moveDir;
        }
        //check if something is hit
        Debug.DrawRay(transform.position, lastInteractDir * interactDistance);
        var right45 = (lastInteractDir + transform.right).normalized;
        Debug.DrawRay(transform.position, right45 * interactDistance);
        var left45 = (lastInteractDir - transform.right).normalized;
        Debug.DrawRay(transform.position, left45 * interactDistance);
        if (Physics.Raycast(transform.position, lastInteractDir, out RaycastHit raycastHit, interactDistance, objectLayerMask)) {
            TryRayCastSelectableObject(raycastHit);
        }
        else if (Physics.Raycast(transform.position, right45, out RaycastHit raycastHitRight, interactDistance, objectLayerMask)) {
            TryRayCastSelectableObject(raycastHitRight);
        }
        else if (Physics.Raycast(transform.position, left45, out RaycastHit raycastHitLeft, interactDistance, objectLayerMask)) {
            TryRayCastSelectableObject(raycastHitLeft);
        }
        else if (selectedElement != null) {
            SetSelectedElement(null);
        }
    }

    private void TryRayCastSelectableObject(RaycastHit raycastHit) {
        if (raycastHit.transform.TryGetComponent(out SelectableObject selectableObject)) {
            if (selectedElement != selectableObject) {
                SetSelectedElement(selectableObject);
            }
        }
        else {
            SetSelectedElement(null);
        }
    }
    public void Hurt() {
        if (!IsOwner) return;
        if (PlayerConsumables.Instance.RemoveHealth()) {
            ResetPosition();
        }
        else {
            //TODO disable player
        }
    }
   

    private void PlacePlank(bool forever) {
        if (forever) {
            if (PlayerConsumables.Instance.RemoveNail()) {
                    GameMultiplayer.Instance.SpawnPlankServerRPC( ghostPlank.transform.position, ghostPlank.transform.rotation,true);

            }
            else {
                return;
            }
        }
        else {
            GameMultiplayer.Instance.SpawnPlankServerRPC(ghostPlank.transform.position, ghostPlank.transform.rotation,false);
        }
        ObjectHoldPoint.SetActive(false);
        SetCarryingElement(false);
        PlayerConsumables.Instance.RemovePlank();
        ghostPlank.gameObject.SetActive(false);

    }

    
    private void SetSelectedElement(SelectableObject selectedObject) {
        if(this.selectedElement != null) {
            this.selectedElement.HideSelected();
        }
        if (selectedObject != null) {
            selectedObject.ShowSelected();
        }
        this.selectedElement = selectedObject;
        OnSelectedObjectChanged?.Invoke(this, new OnSelectedElementChangedEventArgs { selectedElement = selectedObject });
    }
    public void SetCarryingElement(bool hasCarry) {
        carryingElement = hasCarry;
        ghostPlank.gameObject.SetActive(true);
    }
    
    public bool GetCarryingElement() {
        return carryingElement;
    }

    public void ResetPosition() {
        controller.enabled = false;
        transform.position = respawnPosition;
        controller.enabled = true;
    }

}
