## Unno

* Unno 提供一种简单的层次数据的展示/存储/编辑方案。大家习惯称呼为动态表单。
* Unit 描述了数据的结构和约束（类似于 class），Node 描述了符合特定 Unit 的数据（类似于 class 的实例 instance）。
* 前端 html 页面内的 input 命名需要符合 Unit 结构，前端页面通过 javascript 实现内容的前端维护。
* Controller 接收平面化的 Http Post 数据后根据 Unit 重建 Node 树。
* Repository 完成存储。
* 数据结构发生变更时，需要更新 Unit 结构；前端针对该 Unit 的 View；如果采用数据库存储，还需要更新表结构。

## Demo

#### 访问地址

> http://localhost:14736/node/edit/47d687dd-c642-4e21-ac52-1d7afbec23b8

#### Repository.File

* demo unit 文件 files/unit/47d687dd-c642-4e21-ac52-1d7afbec23b8
* demo node 文件 files/unit/7496a4d3-ac92-4d96-85a8-d5756353c9a3
* 确认 Grit.Unno.Web/Web.Config 中 configuration/appSettings/Repository = File

#### Repository.MongoDB

* 恢复 demo 数据
> mongoimport -d unno -c unit Scripts/Grit.Unno.Repository.MongoDB.Demo.json

* 修改 Grit.Unno.Web/Web.Config 中 configuration/connectionStrings/Grit.Unno.Repository.MongoDB 连接字符串
* 确认 Grit.Unno.Web/Web.Config 中 configuration/appSettings/Repository = MongoDB

#### Repository.MySql

* 恢复 demo 数据
> Grit.Unno.Repository.MySql.Demo.sql

* 修改 Grit.Unno.Web/Web.Config 中 configuration/connectionStrings/Grit.Unno.Repository.MySql 连接字符串
* 确认 Grit.Unno.Web/Web.Config 中 configuration/appSettings/Repository = MySql
