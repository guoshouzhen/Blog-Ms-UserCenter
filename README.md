# blog-ms-usercenter

#### 介绍
使用.NET5和EFCore搭建的微服务，作为博客项目的用户中心。对外提供用户登录、注册及用户管理相关的接口。为了支持属性注入，使用Autofac第三方DI容器接管了net core原始容器。使用mysql数据库，并支持多数据源切换。使用HttpClient的异步API，通过网关与其他服务通信。支持多环境配置文件。  

关键词：C#，.NET5，EFCore，Mysql，Autofac，Docker


#### 程序结构说明
```
blog-ms-usercenter
│  .dockerignore
│  .gitignore
│  Blog-Ms-UserCenter.sln ---------------------------解决方案文件
│  
├─Dal -----------------------------------------------DAL层
│  │  Dal.csproj ------------------------------------项目文件
│  │  ServiceCollectionExtension.cs -----------------IServiceCollection扩展类，通过Autofac Module注入当前程序集下需要注入的类
│  │  
│  ├─Dal --------------------------------------------DAL类
│  │  ├─Demo
│  │  │  │  IDemoDal.cs
│  │  │  │  
│  │  │  └─Impl
│  │  │          DemoDal.cs
│  │  │          
│  │  └─Users
│  │      │  IUserDal.cs
│  │      │  
│  │      └─Impl
│  │              UserDal.cs ------------------------用户表相关的数据库操作
│  │              
│  └─DalBases ---------------------------------------DALBase封装
│      │  IDalBase.cs
│      │  IDalContext.cs
│      │  IDalProvoder.cs
│      │  
│      └─Impl
│              DalBase.cs ---------------------------所有DAL类的父类，持有一个DalContext对象（属性自动注入），用来操作数据库
│              DalContext.cs ------------------------Dal上下文，根据当前请求域的DBId提供数据库工作单元（UnitOfWorkBase对象），支持切换数据源、重置数据源
│              DalProvider.cs -----------------------Dal对象的提供者，从IServiceProvider容器中解析指定的Dal对象，主要供Service层使用，构造方法中只要注入该对象就可以获取不同的Dal类，好处是可以避免某些情况下Service类构造方法注入Dal类过多时，参数列表过长的问题
│              
├─Infrastructure ------------------------------------基础设施层
│  │  Infrastructure.csproj -------------------------项目文件
│  │  ServiceCollectionExtension.cs -----------------IServiceCollection扩展类，注入当前程序集下需要注入的类
│  │  
│  ├─Autofac ----------------------------------------Autofac封装
│  │  │  ComponentLoader.cs -------------------------组件加载器，通过反射从程序集中扫描那些被标记为需要注入DI容器的类或者接口（使用Compnent特性及其子类标记是否被注入），然后注入Autofac容器
│  │  │  CustomizedServiceProviderFactory.cs --------自定义ServiceProviderFactory，实现IServiceProviderFactory接口，用于接管原始DI容器
│  │  │  
│  │  ├─Attributes ----------------------------------自定义特性
│  │  │      ComponentAttribute.cs ------------------用于标记注入公共类的特性
│  │  │      RepositoryAttribute.cs -----------------用于标记注入数据库操作相关类的特性
│  │  │      ServiceAttribute.cs --------------------用于标记注入Service层类的特性
│  │  │      
│  │  ├─Cache
│  │  │      ModuleCache.cs -------------------------缓存程序启动时，扫描程序集并构造的Autofac Module对象，在ServiceProviderFactory中调用ContainerBuilder对象的build()方法后，将其注入Autofac容器
│  │  │      
│  │  ├─Enums
│  │  │      ObjectLifeCycleEnum.cs -----------------Autofac组件的生命周期枚举，注入到Autofac时，为组件设置生命周期
│  │  │      
│  │  ├─Extensions
│  │  │      AutofacContainerBuilderExtension.cs ----ContainerBuilder对象扩展类，注入Autofac module及原始ServiceCollection容器中的服务
│  │  │      AutofacRegistrationExtension.cs --------Autofac IRegistrationBuilder对象的扩展类，用于配置注入autofac容器的组件
│  │  │      
│  │  ├─Models
│  │  │      ComponentInfo.cs -----------------------扫描到的组件信息
│  │  │      ComponentMetaData.cs -------------------组件元数据
│  │  │      
│  │  └─Modules
│  │          AssemblysModule.cs --------------------Autofac Module的派生类，用于注入扫描到的类
│  │          ServiceCollectionModule.cs ------------Autofac Module的派生类，用于注入原始容器中的服务
│  │          
│  ├─HttpClients ------------------------------------HttpClient封装
│  │  │  IHttpClientHelper.cs
│  │  │  
│  │  └─Impl
│  │          HttpClientHelper.cs
│  │          
│  ├─Log --------------------------------------------日志
│  │  │  ILoggerHelper.cs
│  │  │  
│  │  └─Impl
│  │          LoggerHelper.cs -----------------------日志帮助类
│  │          
│  ├─Models
│  │  ├─Attributes
│  │  │      RequiredAuthoritiesAttribute.cs --------自定义特性，使用该特性标记的控制器中对应接口（action），被访问时，表示需要所指定的全部权限
│  │  │      
│  │  ├─Exceptions
│  │  │      ServiceException.cs --------------------自定义业务异常
│  │  │      
│  │  └─Model
│  │          ApiResult.cs --------------------------定义接口返回数据的格式和规范
│  │          
│  ├─Sockets
│  │      SocketClient.cs ---------------------------socket client封装
│  │      
│  └─Utils ------------------------------------------工具类
│          DbUtil.cs --------------------------------数据库工具类
│          EncryptUtil.cs ---------------------------加密工具类
│          EnumUtil.cs ------------------------------枚举相关的工具类
│          JsonUtil.cs ------------------------------Json工具类
│          
├─Model ---------------------------------------------Model类定义
│  │  Model.csproj ----------------------------------项目文件
│  │  
│  ├─Bo ---------------------------------------------BO类（业务相关）
│  │  └─Auth
│  │          AuthorityBo.cs
│  │          LoginInfoBo.cs
│  │          
│  ├─Constant ---------------------------------------常量定义
│  │      ApiResultFailedConstant.cs
│  │      ApiResultSuccessConstant.cs
│  │      HttpContextConstant.cs
│  │      SocketServerConstant.cs
│  │      SplitorConstant.cs
│  │      
│  ├─Dto --------------------------------------------Dto类（接口入参定义）
│  │  └─User
│  │          LoginDto.cs
│  │          SignupDto.cs
│  │          
│  ├─Entities ---------------------------------------Entity类（数据库实体）
│  │  │  Role.cs
│  │  │  User.cs
│  │  │  UserRole.cs
│  │  │  
│  │  └─Base
│  │          Entity.cs
│  │          
│  ├─Enums ------------------------------------------枚举类
│  │      AuthorityEnum.cs
│  │      HttpClientNameEnum.cs
│  │      LogFolderEnum.cs
│  │      UserStatusEnum.cs
│  │      
│  ├─Options ----------------------------------------强类型配置类定义（对应到配置文件配置项）
│  │      AuthOptions.cs
│  │      DbConfigOptions.cs
│  │      DbIdConfig.cs
│  │      EncryptOptions.cs
│  │      FilePathOptions.cs
│  │      SocketServerOptions.cs
│  │      TraceIdOptionsSnapshot.cs
│  │      
│  └─Vo ---------------------------------------------Vo类（接口返回数据定义）
│      │  ErrorCodeVo.cs
│      │  
│      ├─Auth
│      │      AuthVo.cs
│      │      
│      └─User
│              LoginVo.cs
│              SignupVo.cs
│              UserInfoVo.cs
│              
├─Repository ----------------------------------------数据仓储层
│  │  Repository.csproj -----------------------------项目文件
│  │  ServiceCollectionExtension.cs -----------------IServiceCollection扩展类，注入当前程序集下需要注入的类
│  │  
│  ├─Db
│  │  │  IRepository.cs
│  │  │  MyRepository.cs ----------------------------泛型类，类型参数为实体类型，使用注入的DbContext，对指定实体对应的表进行增删查改
│  │  │  
│  │  ├─DbContexts ----------------------------------DbContext对象封装
│  │  │  │  DbContextBase.cs ------------------------继承DbContext的抽象类，根据子类传入的数据库连接字符串配置数据库
│  │  │  │  
│  │  │  └─DbPartial --------------------------------DbContextBase派生类，定义不同的数据源
│  │  │          Blog1DbContext.cs ------------------数据源1
│  │  │          Blog2DbContext.cs ------------------数据源2
│  │  │          
│  │  └─DbUnitOfWork --------------------------------数据库工作单元
│  │      │  UnitOfWorkBase.cs ----------------------数据库工作单元抽象类，其中的Table()泛型方法可以提供数据库实体对应的IRepository对象，用来操作数据库
│  │      │  
│  │      └─UnitPartial -----------------------------UnitOfWorkBase派生类，定义不同数据源的工作单元
│  │              Blog1UnitOfWork.cs ----------------数据源1的工作单元
│  │              Blog2UnitOfWork.cs ----------------数据源2的工作单元
│  │              
│  └─Extensions
│          EFCoreExtension.cs -----------------------EFCore扩展，支持原生sql的查询
│          
├─Service -------------------------------------------Service（业务逻辑）层
│  │  Service.csproj
│  │  ServiceCollectionExtension.cs -----------------IServiceCollection扩展类，注入当前程序集下需要注入的类
│  │  
│  ├─Auth -------------------------------------------鉴权相关
│  │  │  IAuthService.cs
│  │  │  
│  │  └─Impl
│  │          AuthService.cs ------------------------从当前的请求域的HttpContext对象中获取登录信息，用于注入到容器中
│  │          
│  ├─Demo
│  │  │  IDemoService.cs
│  │  │  
│  │  └─Impl
│  │          DemoService.cs
│  │          
│  ├─SocketApi --------------------------------------socket请求相关
│  │  │  ISocketService.cs
│  │  │  
│  │  └─Impl
│  │          SocketService.cs
│  │          
│  └─Users
│      │  IUserService.cs ---------------------------用户相关的业务逻辑
│      │  
│      └─Impl
│              UserService.cs
│              
├─UnitTest ------------------------------------------单元测试，暂时不用
│      UnitTest.csproj
│      UnitTest1.cs
│      
└─UserCenterWebApi ----------------------------------webapi项目
    │  AppDockerfileBuild.bat -----------------------批处理命令，用于docker镜像的构建和推送
    │  appsettings.dev.json -------------------------开发环境配置
    │  appsettings.json
    │  appsettings.prod.json ------------------------生产环境配置
    │  Dockerfile -----------------------------------Dockerfile文件
    │  Program.cs -----------------------------------主程序类
    │  Startup.cs -----------------------------------startup配置类，配置程序，注册服务到容器
    │  UserCenterWebApi.csproj ----------------------项目文件
    │  
    ├─Configs
    │      nlog.config ------------------------------nlog配置文件
    │      
    ├─Controllers -----------------------------------controller
    │  ├─InnerApi -----------------------------------对内接口
    │  └─OpenApi ------------------------------------对外接口
    │          DemoController.cs
    │          UserController.cs
    │          
    ├─Exceptions
    │      MyExceptionHandler.cs --------------------全局异常处理
    │      
    ├─Extensions
    │      ConfigurationBuilderExtension.cs ---------IConfigurationBuilder扩展类，用于根据当前运行环境启用对应的配置文件
    │      ServiceCollectionExtension.cs ------------IServiceCollection扩展类，用于注入配置项、HttpClientFactory等
    │      
    ├─Filters
    │      AuthFilterAttribute.cs -------------------鉴权相关的过滤器
    │      InnerAuthFilterAttribute.cs --------------对内接口鉴权过滤器
    │      OpenAuthFilterAttribute.cs ---------------对外接口鉴权过滤器
    │      
    └─Properties
           launchSettings.json ----------------------开发环境启动配置
```

#### 部署说明
* docker容器部署
    * release模式编译项目
    * 进入编译文件目录，执行AppDockerfileBuild.bat批处理命令，可自动构建镜像并推送到远程仓库
    * 在服务器上拉取镜像,并运行
    * 运行示例
    ```
    docker run --name blog-usercenter  \
    -p 8031:8031 \
    --net commonnetwork \
    --ip 172.18.3.1 \
    -v /root/appdata/blog/logs:/root/appdata/blog/logs \
    -v /root/appdata/blog/fileDoc:/root/appdata/blog/fileDoc \
    -d registry.cn-hangzhou.aliyuncs.com/blog-regs/blog-ms-usercenter
    ```