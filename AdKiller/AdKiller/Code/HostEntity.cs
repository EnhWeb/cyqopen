using System;
using System.Collections.Generic;
using System.Text;
using System.Net;

namespace AdKiller
{
    class HostEntity
    {
        public HostEntity()
        {
            Init(DomainType.Video, true, true);
        }
        public HostEntity(DomainType dt)
        {
            Init(dt, true, false);
        }
        //public HostEntity(DomainType dt, bool isEnabled)
        //{
        //    Init(dt, isEnabled);
        //}

        private void Init(DomainType domainType, bool isEnabled,bool isReturnNow)
        {
            this._IsEnabled = IsEnabled;
            this._DomainType = domainType;
            this._IsReturnNow = isReturnNow;
        }
        private bool _IsEnabled = true;
        /// <summary>
        /// 是否启用
        /// </summary>
        public bool IsEnabled
        {
            get
            {
                return _IsEnabled;
            }
            set
            {
                _IsEnabled = value;
            }
        }
        private DomainType _DomainType;
        /// <summary>
        /// 是否需要服务器支持
        /// </summary>
        public DomainType DomainType
        {
            get
            {
                return _DomainType;
            }
            set
            {
                _DomainType = value;
            }
        }

        private bool _IsReturnNow;
        /// <summary>
        /// 是否直接返回
        /// </summary>
        public bool IsReturnNow
        {
            get
            {
                return _IsReturnNow;
            }
            set
            {
                _IsReturnNow = value;
            }
        }


    }
}
