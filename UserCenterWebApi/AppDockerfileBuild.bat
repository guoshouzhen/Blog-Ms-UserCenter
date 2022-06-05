set USER_NAME=不要凶凶哒
set REPOSITORY_URL=registry.cn-hangzhou.aliyuncs.com
set NAMESPACE=blog-regs
set INAGE_NAME=blog-ms-usercenter

echo 开始构建镜像...
docker build -t %INAGE_NAME% .
echo 构建完毕!!!

echo 开始推送镜像...
docker login --username=%USER_NAME% %REPOSITORY_URL%
docker tag %INAGE_NAME% %REPOSITORY_URL%/%NAMESPACE%/%INAGE_NAME%
docker push %REPOSITORY_URL%/%NAMESPACE%/%INAGE_NAME%
echo 推送完毕!!!

echo 删除本地镜像...
docker image rm -f %INAGE_NAME% %REPOSITORY_URL%/%NAMESPACE%/%INAGE_NAME%
echo 删除完毕!!!
pause