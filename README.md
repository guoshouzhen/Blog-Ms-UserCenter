# blog-ms-usercenter

#### ����
ʹ��.NET5��EFCore���΢������Ϊ������Ŀ���û����ġ������ṩ�û���¼��ע�ἰ�û�������صĽӿڡ�Ϊ��֧������ע�룬ʹ��Autofac������DI�����ӹ���net coreԭʼ������ʹ��mysql���ݿ⣬��֧�ֶ�����Դ�л���ʹ��HttpClient���첽API��ͨ����������������ͨ�š�֧�ֶ໷�������ļ���  

�ؼ��ʣ�C#��.NET5��EFCore��Mysql��Autofac��Docker


#### ����ṹ˵��
```
blog-ms-usercenter
��  .dockerignore
��  .gitignore
��  Blog-Ms-UserCenter.sln ---------------------------��������ļ�
��  
����Dal -----------------------------------------------DAL��
��  ��  Dal.csproj ------------------------------------��Ŀ�ļ�
��  ��  ServiceCollectionExtension.cs -----------------IServiceCollection��չ�࣬ͨ��Autofac Moduleע�뵱ǰ��������Ҫע�����
��  ��  
��  ����Dal --------------------------------------------DAL��
��  ��  ����Demo
��  ��  ��  ��  IDemoDal.cs
��  ��  ��  ��  
��  ��  ��  ����Impl
��  ��  ��          DemoDal.cs
��  ��  ��          
��  ��  ����Users
��  ��      ��  IUserDal.cs
��  ��      ��  
��  ��      ����Impl
��  ��              UserDal.cs ------------------------�û�����ص����ݿ����
��  ��              
��  ����DalBases ---------------------------------------DALBase��װ
��      ��  IDalBase.cs
��      ��  IDalContext.cs
��      ��  IDalProvoder.cs
��      ��  
��      ����Impl
��              DalBase.cs ---------------------------����DAL��ĸ��࣬����һ��DalContext���������Զ�ע�룩�������������ݿ�
��              DalContext.cs ------------------------Dal�����ģ����ݵ�ǰ�������DBId�ṩ���ݿ⹤����Ԫ��UnitOfWorkBase���󣩣�֧���л�����Դ����������Դ
��              DalProvider.cs -----------------------Dal������ṩ�ߣ���IServiceProvider�����н���ָ����Dal������Ҫ��Service��ʹ�ã����췽����ֻҪע��ö���Ϳ��Ի�ȡ��ͬ��Dal�࣬�ô��ǿ��Ա���ĳЩ�����Service�๹�췽��ע��Dal�����ʱ�������б����������
��              
����Infrastructure ------------------------------------������ʩ��
��  ��  Infrastructure.csproj -------------------------��Ŀ�ļ�
��  ��  ServiceCollectionExtension.cs -----------------IServiceCollection��չ�࣬ע�뵱ǰ��������Ҫע�����
��  ��  
��  ����Autofac ----------------------------------------Autofac��װ
��  ��  ��  ComponentLoader.cs -------------------------�����������ͨ������ӳ�����ɨ����Щ�����Ϊ��Ҫע��DI����������߽ӿڣ�ʹ��Compnent���Լ����������Ƿ�ע�룩��Ȼ��ע��Autofac����
��  ��  ��  CustomizedServiceProviderFactory.cs --------�Զ���ServiceProviderFactory��ʵ��IServiceProviderFactory�ӿڣ����ڽӹ�ԭʼDI����
��  ��  ��  
��  ��  ����Attributes ----------------------------------�Զ�������
��  ��  ��      ComponentAttribute.cs ------------------���ڱ��ע�빫���������
��  ��  ��      RepositoryAttribute.cs -----------------���ڱ��ע�����ݿ��������������
��  ��  ��      ServiceAttribute.cs --------------------���ڱ��ע��Service���������
��  ��  ��      
��  ��  ����Cache
��  ��  ��      ModuleCache.cs -------------------------�����������ʱ��ɨ����򼯲������Autofac Module������ServiceProviderFactory�е���ContainerBuilder�����build()�����󣬽���ע��Autofac����
��  ��  ��      
��  ��  ����Enums
��  ��  ��      ObjectLifeCycleEnum.cs -----------------Autofac�������������ö�٣�ע�뵽Autofacʱ��Ϊ���������������
��  ��  ��      
��  ��  ����Extensions
��  ��  ��      AutofacContainerBuilderExtension.cs ----ContainerBuilder������չ�࣬ע��Autofac module��ԭʼServiceCollection�����еķ���
��  ��  ��      AutofacRegistrationExtension.cs --------Autofac IRegistrationBuilder�������չ�࣬��������ע��autofac���������
��  ��  ��      
��  ��  ����Models
��  ��  ��      ComponentInfo.cs -----------------------ɨ�赽�������Ϣ
��  ��  ��      ComponentMetaData.cs -------------------���Ԫ����
��  ��  ��      
��  ��  ����Modules
��  ��          AssemblysModule.cs --------------------Autofac Module�������࣬����ע��ɨ�赽����
��  ��          ServiceCollectionModule.cs ------------Autofac Module�������࣬����ע��ԭʼ�����еķ���
��  ��          
��  ����HttpClients ------------------------------------HttpClient��װ
��  ��  ��  IHttpClientHelper.cs
��  ��  ��  
��  ��  ����Impl
��  ��          HttpClientHelper.cs
��  ��          
��  ����Log --------------------------------------------��־
��  ��  ��  ILoggerHelper.cs
��  ��  ��  
��  ��  ����Impl
��  ��          LoggerHelper.cs -----------------------��־������
��  ��          
��  ����Models
��  ��  ����Attributes
��  ��  ��      RequiredAuthoritiesAttribute.cs --------�Զ������ԣ�ʹ�ø����Ա�ǵĿ������ж�Ӧ�ӿڣ�action����������ʱ����ʾ��Ҫ��ָ����ȫ��Ȩ��
��  ��  ��      
��  ��  ����Exceptions
��  ��  ��      ServiceException.cs --------------------�Զ���ҵ���쳣
��  ��  ��      
��  ��  ����Model
��  ��          ApiResult.cs --------------------------����ӿڷ������ݵĸ�ʽ�͹淶
��  ��          
��  ����Sockets
��  ��      SocketClient.cs ---------------------------socket client��װ
��  ��      
��  ����Utils ------------------------------------------������
��          DbUtil.cs --------------------------------���ݿ⹤����
��          EncryptUtil.cs ---------------------------���ܹ�����
��          EnumUtil.cs ------------------------------ö����صĹ�����
��          JsonUtil.cs ------------------------------Json������
��          
����Model ---------------------------------------------Model�ඨ��
��  ��  Model.csproj ----------------------------------��Ŀ�ļ�
��  ��  
��  ����Bo ---------------------------------------------BO�ࣨҵ����أ�
��  ��  ����Auth
��  ��          AuthorityBo.cs
��  ��          LoginInfoBo.cs
��  ��          
��  ����Constant ---------------------------------------��������
��  ��      ApiResultFailedConstant.cs
��  ��      ApiResultSuccessConstant.cs
��  ��      HttpContextConstant.cs
��  ��      SocketServerConstant.cs
��  ��      SplitorConstant.cs
��  ��      
��  ����Dto --------------------------------------------Dto�ࣨ�ӿ���ζ��壩
��  ��  ����User
��  ��          LoginDto.cs
��  ��          SignupDto.cs
��  ��          
��  ����Entities ---------------------------------------Entity�ࣨ���ݿ�ʵ�壩
��  ��  ��  Role.cs
��  ��  ��  User.cs
��  ��  ��  UserRole.cs
��  ��  ��  
��  ��  ����Base
��  ��          Entity.cs
��  ��          
��  ����Enums ------------------------------------------ö����
��  ��      AuthorityEnum.cs
��  ��      HttpClientNameEnum.cs
��  ��      LogFolderEnum.cs
��  ��      UserStatusEnum.cs
��  ��      
��  ����Options ----------------------------------------ǿ���������ඨ�壨��Ӧ�������ļ������
��  ��      AuthOptions.cs
��  ��      DbConfigOptions.cs
��  ��      DbIdConfig.cs
��  ��      EncryptOptions.cs
��  ��      FilePathOptions.cs
��  ��      SocketServerOptions.cs
��  ��      TraceIdOptionsSnapshot.cs
��  ��      
��  ����Vo ---------------------------------------------Vo�ࣨ�ӿڷ������ݶ��壩
��      ��  ErrorCodeVo.cs
��      ��  
��      ����Auth
��      ��      AuthVo.cs
��      ��      
��      ����User
��              LoginVo.cs
��              SignupVo.cs
��              UserInfoVo.cs
��              
����Repository ----------------------------------------���ݲִ���
��  ��  Repository.csproj -----------------------------��Ŀ�ļ�
��  ��  ServiceCollectionExtension.cs -----------------IServiceCollection��չ�࣬ע�뵱ǰ��������Ҫע�����
��  ��  
��  ����Db
��  ��  ��  IRepository.cs
��  ��  ��  MyRepository.cs ----------------------------�����࣬���Ͳ���Ϊʵ�����ͣ�ʹ��ע���DbContext����ָ��ʵ���Ӧ�ı������ɾ���
��  ��  ��  
��  ��  ����DbContexts ----------------------------------DbContext�����װ
��  ��  ��  ��  DbContextBase.cs ------------------------�̳�DbContext�ĳ����࣬�������ഫ������ݿ������ַ����������ݿ�
��  ��  ��  ��  
��  ��  ��  ����DbPartial --------------------------------DbContextBase�����࣬���岻ͬ������Դ
��  ��  ��          Blog1DbContext.cs ------------------����Դ1
��  ��  ��          Blog2DbContext.cs ------------------����Դ2
��  ��  ��          
��  ��  ����DbUnitOfWork --------------------------------���ݿ⹤����Ԫ
��  ��      ��  UnitOfWorkBase.cs ----------------------���ݿ⹤����Ԫ�����࣬���е�Table()���ͷ��������ṩ���ݿ�ʵ���Ӧ��IRepository���������������ݿ�
��  ��      ��  
��  ��      ����UnitPartial -----------------------------UnitOfWorkBase�����࣬���岻ͬ����Դ�Ĺ�����Ԫ
��  ��              Blog1UnitOfWork.cs ----------------����Դ1�Ĺ�����Ԫ
��  ��              Blog2UnitOfWork.cs ----------------����Դ2�Ĺ�����Ԫ
��  ��              
��  ����Extensions
��          EFCoreExtension.cs -----------------------EFCore��չ��֧��ԭ��sql�Ĳ�ѯ
��          
����Service -------------------------------------------Service��ҵ���߼�����
��  ��  Service.csproj
��  ��  ServiceCollectionExtension.cs -----------------IServiceCollection��չ�࣬ע�뵱ǰ��������Ҫע�����
��  ��  
��  ����Auth -------------------------------------------��Ȩ���
��  ��  ��  IAuthService.cs
��  ��  ��  
��  ��  ����Impl
��  ��          AuthService.cs ------------------------�ӵ�ǰ���������HttpContext�����л�ȡ��¼��Ϣ������ע�뵽������
��  ��          
��  ����Demo
��  ��  ��  IDemoService.cs
��  ��  ��  
��  ��  ����Impl
��  ��          DemoService.cs
��  ��          
��  ����SocketApi --------------------------------------socket�������
��  ��  ��  ISocketService.cs
��  ��  ��  
��  ��  ����Impl
��  ��          SocketService.cs
��  ��          
��  ����Users
��      ��  IUserService.cs ---------------------------�û���ص�ҵ���߼�
��      ��  
��      ����Impl
��              UserService.cs
��              
����UnitTest ------------------------------------------��Ԫ���ԣ���ʱ����
��      UnitTest.csproj
��      UnitTest1.cs
��      
����UserCenterWebApi ----------------------------------webapi��Ŀ
    ��  AppDockerfileBuild.bat -----------------------�������������docker����Ĺ���������
    ��  appsettings.dev.json -------------------------������������
    ��  appsettings.json
    ��  appsettings.prod.json ------------------------������������
    ��  Dockerfile -----------------------------------Dockerfile�ļ�
    ��  Program.cs -----------------------------------��������
    ��  Startup.cs -----------------------------------startup�����࣬���ó���ע���������
    ��  UserCenterWebApi.csproj ----------------------��Ŀ�ļ�
    ��  
    ����Configs
    ��      nlog.config ------------------------------nlog�����ļ�
    ��      
    ����Controllers -----------------------------------controller
    ��  ����InnerApi -----------------------------------���ڽӿ�
    ��  ����OpenApi ------------------------------------����ӿ�
    ��          DemoController.cs
    ��          UserController.cs
    ��          
    ����Exceptions
    ��      MyExceptionHandler.cs --------------------ȫ���쳣����
    ��      
    ����Extensions
    ��      ConfigurationBuilderExtension.cs ---------IConfigurationBuilder��չ�࣬���ڸ��ݵ�ǰ���л������ö�Ӧ�������ļ�
    ��      ServiceCollectionExtension.cs ------------IServiceCollection��չ�࣬����ע�������HttpClientFactory��
    ��      
    ����Filters
    ��      AuthFilterAttribute.cs -------------------��Ȩ��صĹ�����
    ��      InnerAuthFilterAttribute.cs --------------���ڽӿڼ�Ȩ������
    ��      OpenAuthFilterAttribute.cs ---------------����ӿڼ�Ȩ������
    ��      
    ����Properties
           launchSettings.json ----------------------����������������
```

#### ����˵��
* docker��������
    * releaseģʽ������Ŀ
    * ��������ļ�Ŀ¼��ִ��AppDockerfileBuild.bat������������Զ������������͵�Զ�ֿ̲�
    * �ڷ���������ȡ����,������
    * ����ʾ��
    ```
    docker run --name blog-usercenter  \
    -p 8031:8031 \
    --net commonnetwork \
    --ip 172.18.3.1 \
    -v /root/appdata/blog/logs:/root/appdata/blog/logs \
    -v /root/appdata/blog/fileDoc:/root/appdata/blog/fileDoc \
    -d registry.cn-hangzhou.aliyuncs.com/blog-regs/blog-ms-usercenter
    ```