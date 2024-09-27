using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using LittleTurtle.UI;
using UnityEngine.EventSystems;
using TMPro;
using System;
using UnityEngine.UI;
using LittleTurtle.Inventory;
using LittleTurtle.System_;

namespace LittleTurtle.Turtle {
    public class TurtleCtrl : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler {

        [Header("ability")]
        public TurtleData_ScriptableObject data_turtle;
        public float reachDistance;

        [Header("Path Finding")]
        public AIPath aiPath;
        public AIDestinationSetter setter;

        [Header("HP Bar")]
        public GameObject HPBarObj;
        public Vector2 HPBarOffset;
        public SpriteRenderer HPBarImage;


        [Header("Other")]
        public Animator animator;
        public GameObject highlightImage;
        public ParticleSystem particle_getFood;


        ///////////////////////////////////////////////

        public static TurtleCtrl Instance;
        private TMP_Text distanceText;
        private bool _isReached;
        private CurveLerping _lerping_HealthBar;
        private Image DirImage;
        private RectTransform DirImageParentRT;
        private float DirArrowPosDis;
        private Bag bag;


        [HideInInspector]
        public float HP { get => _HP; set { if (value <= data_turtle.MaxHP) _HP = value; } }
        private float _HP;
        public float Growth { get => _growth; set => OnGrowthChange(value); }
        private float _growth;

        public TurtleState State { get => _state; set { _state = value; OnTurtleStateChange(); } }
        private TurtleState _state;

        [HideInInspector]
        public bool IsOnScreen {
            get => _isOnScreen; set {
                if (value != _isOnScreen) {
                    _isOnScreen = value;
                    OnIsOnScreenChange();
                    itemInfoPanel.Feed_Btn.SetActive(value);
                }
            }
        }
        private bool _isOnScreen;

        private PlayerCtrl playerCtrl;
        private TurtleInfoPanel infoPanel;
        private ItemInfoPanel itemInfoPanel;
        private HealthBarPosCaculate HPBarCaculate;

        public bool IsReached {
            get => _isReached; set {
                if (value != _isReached) {
                    _isReached = value;
                    aiPath.maxSpeed = value ? 0 : data_turtle.speed;
                    SetAnimatorFloat("Move", value ? 0 : 1, 0.5f);
                }
            }
        }

        public Dictionary<int, int> Dict_ConsumedItem = new Dictionary<int, int>();

        /// ///////////////////////////////////////////////////////////

        private void Awake() {
            Instance = this;
            infoPanel = Canvas_Game.Instance.turtleInfoPanel;
            infoPanel.turtleCtrl = this;

            itemInfoPanel = Canvas_Game.Instance.itemInfoPanel;
            _lerping_HealthBar = ScriptableObject.CreateInstance<CurveLerping>();
            _lerping_HealthBar.SetAdjustVaule(HPBarImage);
            _lerping_HealthBar.SetTarget(HP / data_turtle.MaxHP, CurveLerping.CurveType.EaseOut_Slow);
        }
    

        void Start(){
            if (!MyGameManager.isLoadingSaved) {
                HP = data_turtle.MaxHP;
                Growth = 0;
            }

            playerCtrl = PlayerCtrl.Instance;
            setter.target = playerCtrl.transform;
            bag = Canvas_Game.Instance.bag;
            SetAnimatorFloat("Hide", 0, 0.1f);

            DirImage = Canvas_Game.Instance.turtleDirArrowIcon;
            DirImageParentRT = DirImage.transform.parent.GetComponent<RectTransform>();
            DirArrowPosDis = DirImageParentRT.sizeDelta.x / 2.3f;

            Vector2 v2 = GetComponentInChildren<Collider2D>().bounds.size;
            HPBarCaculate = new HealthBarPosCaculate(v2.x, v2.y, transform, HPBarObj, HPBarOffset);

            infoPanel._StateSwitch((int)State);
            distanceText = Canvas_Game.Instance.turtleDistanceText;
        }

        void Update() {
            
            Vector3 v3 = Camera.main.WorldToViewportPoint(transform.position);
            IsOnScreen = (v3.x > 0 && v3.x < 1 && v3.y > 0 && v3.y < 1) ? true : false;

            if (State == TurtleState.following) {
                IsReached = aiPath.remainingDistance < reachDistance ? true : false;
            }

            HPBarCaculate.UpdateHealthBarPosition();
            _lerping_HealthBar.Update();
            
            // show distance and dir arrow icon
            if (!IsOnScreen) {
                float f = Vector2.Distance(transform.position, playerCtrl.transform.position) * 0.6f;
                distanceText.text = f > 10 ? f.ToString("0") + "m" : f.ToString("0.0") + "m";

                Vector2 v2 = (transform.position - playerCtrl.transform.position).normalized;
                DirImage.rectTransform.anchoredPosition = v2 * DirArrowPosDis;
                DirImage.transform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(v2.y, v2.x) * Mathf.Rad2Deg - 90);
            }

            // temp
            if (Input.GetKeyDown(KeyCode.P)) {
                GetHurt(2);
            }
        }


        private bool turtleUpgraded = false;  //for bug  temp
        #region = Var Change =
        public Action Action_OnGrowthChange;
        private void OnGrowthChange(float nutrition) {
            _growth = nutrition;

            if (Growth >= data_turtle.MaxGrowth && !turtleUpgraded) {
                TurtleMgr.Instance.TurtleUpgrade(this);
                turtleUpgraded = true;
            }

            Action_OnGrowthChange?.Invoke();
        }

        public void OnIsOnScreenChange() {
            if (IsOnScreen) {
                distanceText.text = "";
                DirImage.gameObject.SetActive(false);
            }
            else {
                DirImage.gameObject.SetActive(true);
            }
        }
        #endregion


        #region Animation And Turtle State
        private void OnTurtleStateChange() {
            StopAllCoroutines();

            switch (State) {
                case TurtleState.following:
                    IsReached = false;
                    aiPath.maxSpeed = data_turtle.speed;

                    if (animator.GetFloat("Hide") == 0) {
                        SetAnimatorFloat("Move", 1, 0.5f);
                    }
                    else {
                        animator.SetTrigger("Unhide");
                        SetAnimatorFloat("Hide", 0, 0.1f);
                        SetAnimatorFloat("Move", 1, 0.5f);
                    }
                    break;

                case TurtleState.resting:
                    aiPath.maxSpeed = 0;

                    if (animator.GetFloat("Hide") == 0) {
                        SetAnimatorFloat("Move", 0, 0.5f);
                    }
                    else {
                        SetAnimatorFloat("Hide", 0, 0.1f);
                        animator.SetTrigger("Unhide");
                        SetAnimatorFloat("Move", 0, 0.5f);
                    }

                    break;

                case TurtleState.hiding:
                    aiPath.maxSpeed = 0;
                    SetAnimatorFloat("Hide", 1, 0.1f);
                    break;
            }
        }

        private void SetAnimatorFloat(string varName, float target, float time) {
            StartCoroutine(_SetAnimatorFloat(varName, target, time));
        }
        private IEnumerator _SetAnimatorFloat(string varName, float target, float time) {
            float currentTime = 0;
            float startValue = animator.GetFloat(varName);

            while (currentTime < time) {
                animator.SetFloat(varName, Mathf.Lerp(startValue, target, currentTime / time));
                currentTime += Time.deltaTime;

                yield return null;
            }

            animator.SetFloat(varName, Mathf.Lerp(startValue, target, 1));
        }

        public enum TurtleState {
            following,
            resting,
            hiding
        }

        #endregion


        public void GetHurt(float damage) {
            if (HP - damage > 0) {
                HP -= damage;
                infoPanel.OnHPChange();
                _lerping_HealthBar.SetTarget(HP / data_turtle.MaxHP, CurveLerping.CurveType.EaseOut_Slow);
            }
            else {
                HPBarImage.color = new Color(0, 0, 0, 0);
                
                Instantiate(Canvas_Game.Instance.GameOverPanel, Canvas_Game.Instance.gameObject.transform)
                    .GetComponent<GameOverPanel>().SetText("Turtle Die...");
                Time.timeScale = 0;
            }
        }


        public bool isHighlighting { get => _isHighlighting; set { _isHighlighting = value; highlightImage.SetActive(value); } }
        private bool _isHighlighting;

        public GrowthBarCanvas growthBar;

        #region = Feed =
        public void Feed(Slot slot) {
            Slider_QuantitySelect.DisplaySlider(slot, slot.Quantity);
            Slider_QuantitySelect.Action_confirm += FeedConfirm;
        }
        public void FeedConfirm(Slot slot, int quantity) {
            if (quantity == 0) {
                return;
            }

            if (Dict_ConsumedItem.ContainsKey(slot.item.ID)) {
                Dict_ConsumedItem[slot.item.ID] += quantity;
            }
            else {
                Dict_ConsumedItem[slot.item.ID] = quantity;
            }

            particle_getFood.Play();
            Growth += slot.item.Nutrition * quantity;
            growthBar.Display();

            if (slot.slotType == SlotType.bagSlot) {
                bag.RemoveItem(slot.item, quantity);
            }
            else if (slot.slotType == SlotType.equipmentSlot) {
                if (bag.Items_Equipment[slot.item].Quantity == 1) {
                    bag.Items_Equipment[slot.item].Quantity = 0;
                }
                else {
                    bag.Items_Equipment[slot.item].Quantity -= quantity;
                }
            }
        }
        #endregion

        #region = Pointer =

        public void OnPointerEnter(PointerEventData eventData) {
            if (MyGameManager.isGuiding) return;

            if (bag.draggingItem || bag.draggingItem_NotInBag) {
                highlightImage.SetActive(true);
                isHighlighting = true;
            }
        }

        public void OnPointerExit(PointerEventData eventData) {
            if (MyGameManager.isGuiding) return;

            if (bag.draggingItem || bag.draggingItem_NotInBag) {
                highlightImage.SetActive(false);
                isHighlighting = false;
            }
        }

        public void OnPointerClick(PointerEventData eventData) {
            if (MyGameManager.isGuiding) return;

            infoPanel.gameObject.SetActive(true);
        }
        #endregion

        #region = Data =

        public void InitTurtle() {
            HP = data_turtle.MaxHP;
            Growth = 0;

            infoPanel.InitTurtleData();
        }

        public void InitTurtle(TurtleData d) {
            HP = d.HP;
            Growth = d.Growth;

            transform.position = d.position;
            transform.rotation = d.rotation;
            Dict_ConsumedItem = d.Dict_ConsumedItem;

            infoPanel.InitTurtleData();
        }

        #endregion

        
    }
}
