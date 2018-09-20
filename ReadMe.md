# 介绍

1. 在Datalogic的应用中，常常有保存缺陷图像的需求，然而Datalogic软件保存图像的方式比较单一，即通过FTP软件上传到本地
2. 本软件通过监控FTP公有目录，通过配置文件，过滤指定类目的缺陷图像，并将图像移动到指定类目的文件夹，使得图像以更有条理的形式存储在硬盘中
3. Scanner为exe可执行文件项目
4. Datalogic.ImageArchiving 为dll项目
5. 引用了[CommonLibrary](https://github.com/DaneSpiritGOD/CommonLibrary)中的BaseL和BaseL.Industry

# 使用方法

1. 编译所有项目
2. 配置Setting.xml文件
3. 常驻运行exe程序