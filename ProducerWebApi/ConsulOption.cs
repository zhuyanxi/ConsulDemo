using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConsumerWebApi
{
    public class ConsulOption
    {
        /// <summary>
        /// 服务名称
        /// </summary>
        public string ServiceName { get; set; }

        /// <summary>
        /// 服务IP
        /// </summary>
        public string IP { get; set; }

        /// <summary>
        /// 服务端口
        /// </summary>
        public int Port { get; set; }

        /// <summary>
        /// Consul服务器地址
        /// </summary>
        public string ConsulAddress { get; set; }
    }
}