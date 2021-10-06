using UnityEngine;
namespace TileMatch.Base
{
    [CreateAssetMenu(menuName = "ScriptableObjects/Base/IntValue", order = 1)]
    public class IntValue : ScriptableObject
    {
        [SerializeField] private int _value;
        public int Value { get => _value; }
    }
}
