using UnityEngine;
using UnityEngine.UI;

namespace Main
{
    public static class Helper
    {
        public static void ToggleVisibleButton(
            Button btnOld,
            out Button btnNew,
            bool isActive,
            bool isTransparent = false,
            string btnText = "")
        {
            btnNew = btnOld;
            if (isActive)
            {
                btnNew.interactable = true;
                if (!isTransparent)
                    btnNew.image.color = new Color(
                        btnNew.image.color.r,
                        btnNew.image.color.g,
                        btnNew.image.color.b,
                        255);

                btnNew.GetComponentInChildren<Text>().text = btnText;
            }
            else
            {
                btnNew.interactable = false;
                if (!isTransparent) 
                    btnNew.image.color = new Color(
                        btnNew.image.color.r,
                        btnNew.image.color.g,
                        btnNew.image.color.b,
                        0);
                btnNew.GetComponentInChildren<Text>().text = btnText;
            }
        }
    }
}