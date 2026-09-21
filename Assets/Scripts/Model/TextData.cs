using System.Collections.Generic;
using UnityEngine;
public partial class TextData {
    private TextData() {
        _texts_ZH = new() {
            {Text_ID.Draw, "player{0},抽取{1}张, 当前手牌{2}"},
            {Text_ID.DrawInfo, "卡牌信息{0}"},
            {Text_ID.NormalSummon, "player{0},通常召唤{1}到区域{2}"},
            {Text_ID.Battle_1, "战斗1"},
            {Text_ID.SelectCommand_1, "当前选择对象: {0}"},
            {Text_ID.SelectCommand_2, "选择召唤区域"},
            {Text_ID.GetAttackTarget, "当前可攻击对象id:{0}, name:{1}"},
            {Text_ID.DirectAttack, "直接攻击玩家"},
            {Text_ID.Attack_1, "player{0} {1} 攻击 player{2}的{3}"},
            {Text_ID.Attack_2, "{0} 攻击力: {1}, {2} 攻击力：{3}"},
            {Text_ID.Attack_3, "{0} 攻击力：{1}, {2} 防御力：{3}"},
            {Text_ID.Destroy, "player{0} 卡牌{1}被破坏"},
        };
    }
}

public partial class TextData {
    static TextData _instance;
    public static TextData Instance {
        get {
            _instance ??= new();
            return _instance;
        }
    }

    string _language = "ZH";
    Dictionary<Text_ID, string> _texts_ZH;
    Dictionary<Text_ID, string> _texts_JP;

    public string GetText(Text_ID id) {
        if (_language == "ZH") {
            return _texts_ZH[id];
        }
        else if (_language == "JP") {
            return _texts_JP[id];
        }
        return "";
    }

    public string GetFormatText(Text_ID id, params object[] args) {
        return string.Format(GetText(id), args);
    }
}