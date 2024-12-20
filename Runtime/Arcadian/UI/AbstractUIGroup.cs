using Arcadian.Extensions;
using UnityEngine;

namespace Arcadian.UI
{
    public class AbstractUIGroup : MonoBehaviour
    {
        [SerializeField] private AbstractUI[] abstractUserInterfaces;

        public void CloseOthers(AbstractUI openUI)
        {
            foreach (var abstractUI in abstractUserInterfaces)
            {
                if (abstractUI == openUI) continue;
                
                abstractUI.Close();
            }
        }

        public void CloseAll()
        {
            abstractUserInterfaces.ForEach(aui => aui.Close());
        }
    }
}