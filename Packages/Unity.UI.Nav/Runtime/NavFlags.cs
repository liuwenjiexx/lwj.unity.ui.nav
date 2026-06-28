using UnityEngine;

namespace Unity.UI.Navs
{
    public enum NavFlags
    {
        None,
        /// <summary>
        /// 主界面，不能回退来关闭
        /// </summary>
        Home = 1 << 0,
        /// <summary>
        /// 禁用回退关闭界面
        /// </summary>
        DisableBack = 1 << 1,
        /// <summary>
        /// 浮动层界面，上一个界面不被隐藏，如：对话框，仓库和背包多开
        /// </summary>
        Float = 1 << 2,
        /// <summary>
        /// 独占，下层界面不可交互，如：对话框
        /// </summary>
        Exclusive = 1 << 3,
   
    }
}
