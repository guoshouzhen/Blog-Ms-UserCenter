#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

# 基础镜像
FROM mcr.microsoft.com/dotnet/aspnet:5.0
FROM mcr.microsoft.com/dotnet/sdk:5.0
# 设置环境变量
ENV ASPNETCORE_ENVIRONMENT=prod
ENV ASPNETCORE_URLS=http://*:8031
# 暴露端口
EXPOSE 8031
# 工作目录
WORKDIR /app
# 复制文件
COPY . /app/
# 启动命令
ENTRYPOINT ["dotnet", "UserCenterWebApi.dll"]