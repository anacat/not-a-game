using UnityEngine;

[CreateAssetMenu(menuName = "Variables/Bool")]
public class BoolVariable : ScriptableObject
{
	[SerializeField]
    private bool value;

    public void SetValue(bool value)
    {
        this.value = value;
    }

    public bool GetValue()
    {
		return this.value;
    }
}
