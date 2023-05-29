using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SettingsUI : MonoBehaviour
{
    public static SettingsUI Instance { get; private set; }

    [SerializeField] private Button closeButton;
    [SerializeField] private Button moveForwardButton;
    [SerializeField] private Button moveDownButton;
    [SerializeField] private Button moveLeftButton;
    [SerializeField] private Button moveRightButton;
    [SerializeField] private Button jumpButton;
    [SerializeField] private Button interactButton;
    [SerializeField] private Button interact2Button;
    [SerializeField] private Button rotateYButton;
    [SerializeField] private Button rotateZButton;
    [SerializeField] private TextMeshProUGUI moveForwardText;
    [SerializeField] private TextMeshProUGUI moveDownText;
    [SerializeField] private TextMeshProUGUI moveLeftText;
    [SerializeField] private TextMeshProUGUI moveRightText;
    [SerializeField] private TextMeshProUGUI jumpText;
    [SerializeField] private TextMeshProUGUI interactText;
    [SerializeField] private TextMeshProUGUI interact2Text;
    [SerializeField] private TextMeshProUGUI rotateYText;
    [SerializeField] private TextMeshProUGUI rotateZText;
    [SerializeField] private Transform pressToRebindKeyTransform;


    private void Awake() {
        Instance = this;
        closeButton.onClick.AddListener(() => {
            Hide();
        });
        moveForwardButton.onClick.AddListener(() => {
            RebindBinding(GameInput.Binding.Move_Forward);
        });
        moveDownButton.onClick.AddListener(() => {
            RebindBinding(GameInput.Binding.Move_Down);
        });
        moveLeftButton.onClick.AddListener(() => {
            RebindBinding(GameInput.Binding.Move_Left);
        });
        moveRightButton.onClick.AddListener(() => {
            RebindBinding(GameInput.Binding.Move_Right);
        });
        jumpButton.onClick.AddListener(() => {
            RebindBinding(GameInput.Binding.Jump);
        });
        interactButton.onClick.AddListener(() => {
            RebindBinding(GameInput.Binding.Interact);
        });
        interact2Button.onClick.AddListener(() => {
            RebindBinding(GameInput.Binding.Interact2);
        });
        rotateYButton.onClick.AddListener(() => {
            RebindBinding(GameInput.Binding.RotateY);
        });
        rotateZButton.onClick.AddListener(() => {
            RebindBinding(GameInput.Binding.RotateZ);
        });
    }

    private void Start() {
        UpdateVisual();
        Hide();
    }
    private void UpdateVisual() {
        moveForwardText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Move_Forward); 
        moveDownText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Move_Down); 
        moveLeftText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Move_Left); 
        moveRightText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Move_Right); 
        jumpText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Jump); 
        interactText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Interact); 
        interact2Text.text = GameInput.Instance.GetBindingText(GameInput.Binding.Interact2); 
        rotateYText.text = GameInput.Instance.GetBindingText(GameInput.Binding.RotateY); 
        rotateZText.text = GameInput.Instance.GetBindingText(GameInput.Binding.RotateZ); 
    }
    public void Show() {
        gameObject.SetActive(true);
    }
    public void Hide() {
        gameObject.SetActive(false);
    }

    private void ShowPressToRebindKey() {
        pressToRebindKeyTransform.gameObject.SetActive(true);
    }
    private void HidePressToRebindKey() {
        pressToRebindKeyTransform.gameObject.SetActive(false);
    }
    private void RebindBinding(GameInput.Binding binding) {
        ShowPressToRebindKey();
        GameInput.Instance.RebindBinding(binding, () => { HidePressToRebindKey(); UpdateVisual(); });
    }
}
