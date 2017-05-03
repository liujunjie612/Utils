using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Text.RegularExpressions;

public class RegexTest : MonoBehaviour {

    public InputField input;
    public Button checkBtn;
    public Text resultTxt;

    private string reg_type = REG_2;
    //数字0-9
    private const string reg_Num = @"^[0-9]+$";
    
    private const string REG_2 = @"^[a-zA-Z][a-zA-Z\d!@#$%^&*.,_+-=]{6,9}$";

    void Start()
    {
        checkBtn.onClick.AddListener(this.__check);
    }

    private void __check()
    {
        string s = input.text;
        bool b = Regex.IsMatch(s, reg_type);
        resultTxt.text = b ? "验证通过" : "验证失败";
    }
}
