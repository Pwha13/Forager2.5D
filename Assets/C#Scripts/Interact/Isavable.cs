using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISavable
{
    void RegisterSaveData() => DataManage.Instance.RegisterSaveData(this);
    void UnRegisterSaveData() => DataManage.Instance.UnRegisterSaveData(this);
    void Save(Data data);
    void Load(Data data);
}
