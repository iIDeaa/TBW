using UnityEngine;

public class TextComparer : MonoBehaviour
{
    public bool IsMatch(string input)
    {
        if (string.IsNullOrEmpty(input)) return false;

        input = input.Replace(" ", "");

        if (input.Contains("เปิด") && input.Contains("ประตู"))
            return true;

        return false;
    }
}