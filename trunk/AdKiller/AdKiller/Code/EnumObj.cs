using System;
using System.Collections.Generic;
using System.Text;

namespace AdKiller
{
    /// <summary>
    /// ���ģʽ
    /// </summary>
    public enum SearchMode
    {  
        /// <summary>
        /// ���ι����ʾ
        /// </summary>
        NoAd,
        /// <summary>
        /// ������ʾ���
        /// </summary>
        ShowAd,
        ///// <summary>
        ///// ���ܴ���ͨ����ʾ�������������
        ///// </summary>
        //AutoAd,
    }
    /// <summary>
    /// ��������
    /// </summary>
    public enum SearchEngine
    {
        /// <summary>
        /// �ٶ� baidu.com
        /// </summary>
        Baidu,
        /// <summary>
        /// �ȸ� google.com
        /// </summary>
       // Google,
        /// <summary>
        /// ��Ѷ soso.com
        /// </summary>
        Soso,
        /// <summary>
        /// �ѹ� sogou.com
        /// </summary>
        Sogou,
        /// <summary>
        /// 360 so.com
        /// </summary>
      //  So,

    }
    /// <summary>
    /// ����������
    /// </summary>
    public enum DomainType
    {
        /// <summary>
        /// ��������
        /// </summary>
        Search,
        /// <summary>
        /// ��Ƶ��
        /// </summary>
        Video,
    }

}
