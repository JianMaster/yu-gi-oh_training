using System.Collections.Generic;
using UnityEngine;
public partial class TextData {
    private TextData() {
        _texts_ZH = new() {
            {Text_ID.Battle_1, "战斗1"},
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
}