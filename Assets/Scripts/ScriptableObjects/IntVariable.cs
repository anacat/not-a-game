using UnityEngine;

[CreateAssetMenu(menuName = "Variables/Int")]
public class IntVariable : ScriptableObject
{
    [SerializeField]
    private int value;

    public void SetValue(int value)
    {
        this.value = value;
    }

    public int GetValue()
    {
        return this.value;
    }
}