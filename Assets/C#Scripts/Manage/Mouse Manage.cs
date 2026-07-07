using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class MouseManage : MonoBehaviour
{
    private RaycastHit hitInfo;
    // private UITip currentUITip;
    public Vector3 hitPos;
    // private ToolType currentToolType;
    private ToolSO currentTool;
    //isUI
    private bool isUIing;
    private bool isConstucting;
    private bool isBuying;
    
    public GameObject lasttarget;
    private ItemSO theBuilding;
    
    public GameObject currentConstruct;
    public ConstuctProperty constructProperty;
    
    public ConRecipes currentRecipe;
    
    //TODO:传输挖掘效率等
    public float followDistance;
    public float clickCd = 1.5f;
    public float clickDistance = 15;
    //Interface
    private IPlayerDataProvider _playerDataProvider;
    //Event
    public static event Action<Vector3> MouseHitInfo;
    public static event Action<bool> AttackHander;
    public static event Action OnClick;
    public static event Action OnConstructFinish;
    public static event Action<ToolSO> OnEat;
    //TODO mouseClickAnim
    //Coroutine
    private Coroutine clickCdCount;
    
    private void OnEnable()
    {
        UIEventCenter.OnToolChange += ChangeTool;
        UIEventCenter.OnUiOpen += ChangeIsUI;
        UIEventCenter.OnMake += ChangeIsUI;
        UIEventCenter.OnBuild += GetBuild;
        UIEventCenter.OnUICancel += CancelConstruct;
        MyCamera.ToBuyVisonHandler += IsBuyTrue;
        MyCamera.ToPlayerVisonHandler += IsBuyFalse;
        InventoryHander.OnGloveAdd += OnGloveAdd;
    }

    private void OnDisable()
    {
        UIEventCenter.OnToolChange -= ChangeTool;
        UIEventCenter.OnUiOpen -= ChangeIsUI;
        UIEventCenter.OnMake -= ChangeIsUI;
        UIEventCenter.OnBuild -= GetBuild;
        UIEventCenter.OnUICancel -= CancelConstruct;
        MyCamera.ToPlayerVisonHandler -= IsBuyFalse;
        MyCamera.ToBuyVisonHandler -= IsBuyFalse;
        InventoryHander.OnGloveAdd -= OnGloveAdd;
    }

    private void Update()
    {
        GetHitInfo();
    }
    void GetHitInfo()
    {
        //获取鼠标屏幕位置
        Vector2 mousePos = Mouse.current.position.ReadValue();
        //投射射线
        Ray ray = Camera.main.ScreenPointToRay(mousePos);
        LayerMask mask;
        if (isConstucting || isBuying)
            mask = ~LayerMask.GetMask("Construct", "Spawn", "Player");
        else
            mask = ~LayerMask.GetMask("Player");
        if (Physics.Raycast(ray, out hitInfo,Mathf.Infinity,mask))
        {
            hitPos = hitInfo.point;
            GameObject hitObject = hitInfo.transform.gameObject;
            //建筑
            if (isConstucting)
            {
                ConstructLogic(hitObject,ray,hitPos);
                return;
            }
            //购买
            if (isBuying)
            {
                BuyingLogic(hitObject);
            }
            //背包/打开UITop return
            if(isUIing) return;
            //控制角色方向,判断攻击方向
            MouseHitInfo?.Invoke(hitPos);
            //打开
            if (Mouse.current.rightButton.wasPressedThisFrame)
            {
                if (hitInfo.transform.TryGetComponent(out IMouseRightClickHandler click))
                {
                    click.OnMyMouseRightClick(currentTool);
                }
            }
            //攻击
            if (currentTool.toolType == ToolType.Sword)
            {
                SwordLogic();
                return;
            }
            //取消攻击
            AttackHander?.Invoke(false);
            //Food
            if (currentTool.toolType == ToolType.Food)
            {
                if (Mouse.current.leftButton.isPressed)
                {
                    if (clickCdCount == null)
                    {
                        clickCdCount = StartCoroutine(ClickCdCount());
                        OnEat?.Invoke(currentTool);
                        OnClick?.Invoke();
                    }
                }
            }
            //判断手距离  TODO:配合手套handmanage
            if ((_playerDataProvider.GetPosition() - hitPos).magnitude > clickDistance) return;
            switch (currentTool.toolType)
            {
                //pickaxe,hammer,shover,block,Block暂未实现
                case ToolType.Pickaxe:
                {
                    if(hitObject.CompareTag("Spawn")||hitObject.CompareTag("Construct"))
                        ToolsLogic(hitObject);
                    else 
                        if(lasttarget!= null)
                            if(lasttarget.TryGetComponent(out IMouseExitHandler exit)) exit.OnMyMouseExit(currentTool);
                    return;
                }
                case ToolType.Hammer:
                {
                    if (hitObject.CompareTag("Construct"))
                        ToolsLogic(hitObject);
                    else 
                    if(lasttarget!= null)
                        if(lasttarget.TryGetComponent(out IMouseExitHandler exit)) exit.OnMyMouseExit(currentTool);
                    return;
                }
                case ToolType.Shovel:
                {
                    if (hitObject.CompareTag("Ground"))
                        ToolsLogic(hitObject);
                    else 
                    if(lasttarget!= null)
                        if(lasttarget.TryGetComponent(out IMouseExitHandler exit)) exit.OnMyMouseExit(currentTool);
                    break;
                }
            }
        }
        else
        {
            if(lasttarget!= null)
                if(lasttarget.TryGetComponent(out IMouseExitHandler exit)) exit.OnMyMouseExit(currentTool);
        }
    }
    private void ConstructLogic(GameObject hitObject,Ray ray,Vector3 hitPos)
    {
        if (hitObject.CompareTag(constructProperty.GetBuildType()))
        {
            //绑定位置
            currentConstruct.transform.position = GridManage.Instance.GridPos(hitPos) + constructProperty.posOffset;
            //执行判断逻辑
            if (constructProperty.PhysicCheck())
            {
                //click逻辑
                if (Mouse.current.leftButton.wasPressedThisFrame)
                {
                    currentRecipe.SubMaterial();
                    currentRecipe = null;
                    currentConstruct = null;
                    isConstucting = false;
                    UIEventCenter.Instance.RaiseOutBuild();
                    OnConstructFinish?.Invoke();
                }

            }
        }
        else
        {
            constructProperty.Red();
            Vector3 targetPos = ray.GetPoint(followDistance);
            currentConstruct.transform.position = targetPos;
        }
        if (Mouse.current.rightButton.wasPressedThisFrame)
        { 
            CancelConstruct();
        }
    }

    private void CancelConstruct()
    {
        Destroy(currentConstruct);
        currentRecipe = null;
        currentConstruct = null;
        isConstucting = false;
        UIEventCenter.Instance.RaiseOutBuild();
    }
    private void BuyingLogic(GameObject hitObject)
    {
        //Exit调用
        if (hitObject != lasttarget)
        { 
            if(lasttarget != null)
                if (lasttarget.TryGetComponent(out IMouseExitHandler exit)) exit.OnMyMouseExit(currentTool);
        }
        //Enter&&Click
        if (hitObject == null) return;
        if(hitObject.TryGetComponent(out IMouseEnterHandler enter)) enter.OnMyMouseEnter(currentTool);
        lasttarget = hitObject;
        if (hitInfo.transform.TryGetComponent(out IMouseClickHandler click))
            if(Mouse.current.leftButton.wasPressedThisFrame)
                click.OnMyMouseClick(currentTool);
    }
    private void SwordLogic()
    {
        if (lasttarget != null) 
            if(lasttarget.TryGetComponent(out IMouseExitHandler exit)) exit.OnMyMouseExit(currentTool);
        AttackHander?.Invoke(true);
        if (Mouse.current.leftButton.isPressed)
        {
            if (clickCdCount == null)
            {
                clickCdCount = StartCoroutine(ClickCdCount());
                OnClick?.Invoke();
            }
        }
        
    }
    private void ToolsLogic(GameObject hitObject)
    {
        //Exit调用
        if (hitObject != lasttarget)
        { 
            if(lasttarget != null)
                if (lasttarget.TryGetComponent(out IMouseExitHandler exit)) exit.OnMyMouseExit(currentTool);
        }
        //Enter&&Click
        if (hitObject == null) return;
        if(hitObject.TryGetComponent(out IMouseEnterHandler enter)) enter.OnMyMouseEnter(currentTool);
        lasttarget = hitObject;
        if (Mouse.current.leftButton.isPressed)
        {
            if (clickCdCount == null)
            {
                clickCdCount = StartCoroutine(ClickCdCount());
                OnClick?.Invoke();
                if (currentTool.toolType == ToolType.Hammer)
                {
                    if(hitObject.TryGetComponent<TableController>(out var click)) click.OnMyMouseClick(currentTool);
                }
                else
                {
                    if(hitObject.TryGetComponent(out IMouseClickHandler click)) click.OnMyMouseClick(currentTool);
                }
            }
        }
    }

    public IEnumerator ClickCdCount()
    {
        yield return new WaitForSeconds(clickCd);
        clickCdCount = null;
    }
    public void Initialize(IPlayerDataProvider playerDataProvider)
    {
        _playerDataProvider = playerDataProvider;
    }

    private void ChangeTool(ItemSO item)
    {
        if(item ==null)
        {
            // currentToolType = ToolType.None;
            return;
        }
        if (item is ToolSO tool)
        {
            // currentToolType = tool.toolType;
            currentTool = tool;
        }
    }

    private void IsBuyTrue()
    {
        isBuying = true;
    }
    private void IsBuyFalse()
    {
        isBuying = false;
    }

    private void ChangeIsUI(bool b)
    {
        isUIing = b;
    }
    private void GetBuild(ItemSO item,ConRecipes recipe)
    {
        if(isConstucting) return;
        theBuilding = item;
        currentConstruct = Instantiate(item.itemPrefab);
        constructProperty = currentConstruct.GetComponent<ConstuctProperty>();
        currentRecipe = recipe;
        isConstucting = true;
    }

    private void OnGloveAdd(ToolSO tool)
    {
        clickDistance = tool.num1;
        clickCd = tool.num2;
    }
}
