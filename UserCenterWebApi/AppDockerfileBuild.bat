set USER_NAME=��Ҫ������
set REPOSITORY_URL=registry.cn-hangzhou.aliyuncs.com
set NAMESPACE=blog-regs
set INAGE_NAME=blog-ms-usercenter

echo ��ʼ��������...
docker build -t %INAGE_NAME% .
echo �������!!!

echo ��ʼ���;���...
docker login --username=%USER_NAME% %REPOSITORY_URL%
docker tag %INAGE_NAME% %REPOSITORY_URL%/%NAMESPACE%/%INAGE_NAME%
docker push %REPOSITORY_URL%/%NAMESPACE%/%INAGE_NAME%
echo �������!!!

echo ɾ�����ؾ���...
docker image rm -f %INAGE_NAME% %REPOSITORY_URL%/%NAMESPACE%/%INAGE_NAME%
echo ɾ�����!!!
pause