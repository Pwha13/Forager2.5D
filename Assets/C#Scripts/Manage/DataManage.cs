using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DataManage : MonoBehaviour
{
    public static DataManage Instance;
    private List<ISavable> _savableList = new List<ISavable>();
    private Data _saveData;
    private string SavePath => Path.Combine(Application.persistentDataPath, "save.json");

    //初始化
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this.gameObject);
        
        _saveData = new Data();
    }

    private void Start()
    {
        if (TitleMenu.HasSave)
        {
            StartCoroutine(LoadAfterFrame());
        }
    }

    private IEnumerator LoadAfterFrame()
    {
        yield return null;
        Load();
    }

    //时间订阅与取消
    private void OnEnable()
    {
        SaveLoadEventSo.SaveEvent += Save;
        SaveLoadEventSo.LoadEvent += Load;
    }
    private void OnDisable()
    {
        SaveLoadEventSo.SaveEvent -= Save;
        SaveLoadEventSo.LoadEvent -= Load;
    }

    public void RegisterSaveData(ISavable saveable)
    {
        if(!_savableList.Contains(saveable))
            _savableList.Add(saveable);
    }
    public void UnRegisterSaveData(ISavable saveable)
    {
        _savableList.Remove(saveable);
    }

    public void SaveAndExit()
    {
        Save();
        SceneManager.LoadScene(0);
    }
    public void Save()
    {
        //存储静态物体数据
        foreach (var saveable in _savableList)
        {
            saveable.Save(_saveData);
        }
        //遍历动态生成物体存储数据
        PoolID[] spawns = FindObjectsOfType<PoolID>();
        Debug.Log(spawns.Length);
        foreach (var spawn in spawns)
        {
            if (spawn.gameObject.activeSelf)
                _saveData.SaveSpawn.TryAdd(spawn.transform.position, spawn.id);
        }
        //存入序列化存档
        SaveData saveData = new SaveData();
        saveData.playerHp = _saveData.PlayerHp;
        saveData.playerStm = _saveData.PlayerStm;
        saveData.playerHungryTime = _saveData.PlayerHungryTime;
        saveData.posX = _saveData.PlayerPos.x;
        saveData.posY = _saveData.PlayerPos.y;
        saveData.posZ = _saveData.PlayerPos.z;

        saveData.saveBackpack = DictToList(_saveData.SaveBackpack);
        saveData.saveTool = _saveData.SaveTool;
        saveData.saveEqual = _saveData.SaveEqual;
        saveData.saveSpawn = DictToList(_saveData.SaveSpawn);
        saveData.saveIsland = new List<IntPair>();
        if (_saveData.SaveIsland != null)
            foreach (var kv in _saveData.SaveIsland)
                saveData.saveIsland.Add(new IntPair { key = kv.Key, value = kv.Value ? 1 : 0 });
        saveData.timeAngle = _saveData.TimeAngle;
        saveData.isNight = _saveData.IsNight;
        saveData.day = _saveData.Day;
        Debug.Log(saveData.saveSpawn.Count);
        //存盘
        string json = JsonUtility.ToJson(saveData,true);
        File.WriteAllText(SavePath, json);
        Debug.Log("Saved");
    }
    public void Load()
    {
        if (!File.Exists(SavePath))
        {
            Debug.Log("NoSaveData");
            return;
        }
        string json = File.ReadAllText(SavePath);
        SaveData loadData = JsonUtility.FromJson<SaveData>(json);
        if (_saveData.SaveSpawn == null)
        {
            Debug.Log("NoSpawn");
            _saveData.SaveSpawn = new Dictionary<Vector3, int>();
        }
        _saveData.PlayerHp = loadData.playerHp;
        _saveData.PlayerStm = loadData.playerStm;
        _saveData.PlayerHungryTime = loadData.playerHungryTime;
        _saveData.PlayerPos = new Vector3(loadData.posX, loadData.posY, loadData.posZ);

        _saveData.SaveBackpack = ListToDict(loadData.saveBackpack);
        if (loadData.saveTool != null)
            _saveData.SaveTool = loadData.saveTool;
        if (loadData.saveEqual != null)
            _saveData.SaveEqual = loadData.saveEqual;

        if (loadData.saveSpawn != null)
        {
            _saveData.SaveSpawn = ListToDict(loadData.saveSpawn);
            Debug.Log(loadData.saveSpawn.Count);
        }
        _saveData.SaveIsland = new Dictionary<int, bool>();
        if (loadData.saveIsland != null)
            foreach (var p in loadData.saveIsland)
                _saveData.SaveIsland[p.key] = p.value != 0;
        _saveData.TimeAngle = loadData.timeAngle;
        _saveData.IsNight = loadData.isNight;
        _saveData.Day = loadData.day;
        //加载数据
        foreach (var saveable in _savableList)
        {
            saveable.Load(_saveData);
        }
        //删除旧有
        PoolID[] existing = FindObjectsOfType<PoolID>();
        foreach (var obj in existing)
        {
            if (obj.gameObject.activeSelf)
                Destroy(obj.gameObject);
        }
        //加载生成物
        foreach (var entry in _saveData.SaveSpawn)
        {
            Debug.Log(_saveData.SaveSpawn.Count);
            ObjectPool.Instance.GetObject(entry.Value, entry.Key);
        }
    }
    private List<IntPair> DictToList(Dictionary<int, int> dict)
    {
        var list = new List<IntPair>();
        if (dict == null) return list;
        foreach (var kv in dict)
            list.Add(new IntPair { key = kv.Key, value = kv.Value });
        return list;
    }
    private List<IntXVector3> DictToList(Dictionary<Vector3, int> dict)
    {
        var list = new List<IntXVector3>();
        if (dict == null) return list;
        foreach (var kv in dict)
            list.Add(new IntXVector3 { posX = kv.Key.x,posY = kv.Key.y,posZ = kv.Key.z,id = kv.Value });
        return list;
    }
    private Dictionary<int, int> ListToDict(List<IntPair> list)
    {
        var dict = new Dictionary<int, int>();
        if (list == null) return dict;
        foreach (var p in list)
            dict[p.key] = p.value;
        return dict;
    }
    private Dictionary<Vector3, int> ListToDict(List<IntXVector3> list)
    {
        var dict = new Dictionary<Vector3, int>();
        if (list == null) return dict;
        foreach (var p in list)
        {
            Vector3 pos = new Vector3(p.posX,p.posY,p.posZ);
            dict[pos] = p.id;
        }
        return dict;
    }



}


