# ConsulDemo

## 一、启动命令：
consul agent -server -ui -bootstrap-expect=1 -data-dir 'D:\Tools\Consul\ConsulData' -node=consul-1 -bind '192.168.0.103' -datacenter=dc1

注解：

    -server    表示已服务器方式启动
    -ui        使用Consul内置的UI系统
    -bootstrap-expect 设定此Consul服务需要几台服务器（一般可设为3或5）
    -data-dir  设定数据文件路径
    -node      设置节点名称
    -bind      绑定IP地址
    -datacenter   设置数据中心名称

## 二、Docker部署：

1.拉取镜像：docker pull consul

2.部署容器+启动节点：

// Server 节点 1

docker run --name cs1 -p 8500:8500 -v D:\DockerData\ConsulData\cs1:/consuldata consul agent -server -bind 172.17.0.2 -node consul-server-1 -data-dir /consuldata -bootstrap-expect 3 -client 0.0.0.0 -ui

// Server 节点 2

docker run --name cs2 -p 7500:8500 -v D:\DockerData\ConsulData\cs2:/consuldata consul agent -server -bind 172.17.0.3 -node consul-server-2 -data-dir /consuldata -bootstrap-expect 3 -client 0.0.0.0 -ui -join 172.17.0.2

// Server 节点 3

docker run --name cs3 -p 6500:8500 -v D:\DockerData\ConsulData\cs3:/consuldata consul agent -server -bind 172.17.0.4 -node consul-server-3 -data-dir /consuldata -bootstrap-expect 3 -client 0.0.0.0 -ui -join 172.17.0.2

// Client 节点 1

docker run --name cc1 -p 5500:8500 -v D:\DockerData\ConsulData\cc1:/consuldata consul agent -bind 172.17.0.5 -node consul-client-1 -data-dir /consuldata -client 0.0.0.0 -ui -join 172.17.0.2

3.查看节点状态：
docker exec -t cs1 consul members

4.查看Server状态：
docker exec -t cs1 consul operator raft list-peers

5.查看界面：
浏览器访问localhost:8500(7500/6500/5500)

## 三、运行程序

dotnet run --urls http://<IP>:<Port> --servicename **** --consuladdress http://<IP>:<Port>

注解：

    --urls            ASP.NET Core WebApi项目启动地址        
    --servicename     ConsulService的名称
    --consuladdress   Consul服务器的地址
