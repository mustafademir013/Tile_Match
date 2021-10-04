using UnityEngine;

namespace TileMatch.Base
{
    public abstract class AbtstractBaseInitalizable:MonoBehaviour
    {
        protected abstract void Initalize();

        private void Start()
        {
            Initalize();
        }

    }
}
