## Unno

* Unno �ṩһ�ּ򵥵Ĳ�����ݵ�չʾ/�洢/�༭���������ϰ�߳ƺ�Ϊ��̬����
* Unit ���������ݵĽṹ��Լ���������� class����Node �����˷����ض� Unit �����ݣ������� class ��ʵ�� instance����
* ǰ�� html ҳ���ڵ� input ������Ҫ���� Unit �ṹ��ǰ��ҳ��ͨ�� javascript ʵ�����ݵ�ǰ��ά����
* Controller ����ƽ�滯�� Http Post ���ݺ���� Unit �ؽ� Node ����
* Repository ��ɴ洢��
* ���ݽṹ�������ʱ����Ҫ���� Unit �ṹ��ǰ����Ը� Unit �� View������������ݿ�洢������Ҫ���±�ṹ��

## Demo

#### ���ʵ�ַ

> http://localhost:14736/node/edit/47d687dd-c642-4e21-ac52-1d7afbec23b8

#### Repository.File

* demo unit �ļ� files/unit/47d687dd-c642-4e21-ac52-1d7afbec23b8
* demo node �ļ� files/unit/7496a4d3-ac92-4d96-85a8-d5756353c9a3
* ȷ�� Grit.Unno.Web/Web.Config �� configuration/appSettings/Repository = File

#### Repository.MongoDB

* �ָ� demo ����
> mongoimport -d unno -c unit Scripts/Grit.Unno.Repository.MongoDB.Demo.json

* �޸� Grit.Unno.Web/Web.Config �� configuration/connectionStrings/Grit.Unno.Repository.MongoDB �����ַ���
* ȷ�� Grit.Unno.Web/Web.Config �� configuration/appSettings/Repository = MongoDB

#### Repository.MySql

* �ָ� demo ����
> Grit.Unno.Repository.MySql.Demo.sql

* �޸� Grit.Unno.Web/Web.Config �� configuration/connectionStrings/Grit.Unno.Repository.MySql �����ַ���
* ȷ�� Grit.Unno.Web/Web.Config �� configuration/appSettings/Repository = MySql
