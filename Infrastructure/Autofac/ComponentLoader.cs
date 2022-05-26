using Autofac;
using Infrastructure.Autofac.Attributes;
using Infrastructure.Autofac.Enums;
using Infrastructure.Autofac.Extensions;
using Infrastructure.Autofac.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Infrastructure.Autofac
{
    /// <summary>
    /// 组件加载器
    /// </summary>
    public class ComponentLoader
    {
        /// <summary>
        /// 从程序集中获取所有需要注入DI容器的组件
        /// </summary>
        /// <param name="lstAssemblies">程序集</param>
        /// <returns></returns>
        public static IList<ComponentInfo> GetAllComponentsFromAssemblies(IList<Assembly> lstAssemblies) 
        {
            if (lstAssemblies == null || lstAssemblies.Count == 0)
            {
                throw new ArgumentException("assemblies registered must be not null or empty", nameof(lstAssemblies));
            }

            IList<ComponentMetaData> lstMetaDatas = new List<ComponentMetaData>();

            //解析含有注入特性（Component、Service、Repository）的组件
            foreach (Assembly assembly in lstAssemblies) 
            {
                //忽略通过反射动态生成的程序集
                if (assembly.IsDynamic) 
                {
                    continue;
                }

                //获取暴露的公共类
                Type[] publicTypes = assembly.GetExportedTypes();
                if (publicTypes == null || publicTypes.Length == 0) 
                {
                    continue;
                }

                foreach (Type type in publicTypes) 
                {
                    //判断是否包含注入组件标记特性
                    IEnumerable<Attribute> attributes = type.GetCustomAttributes();
                    if (attributes == null || attributes.Count() == 0) 
                    {
                        //无标记，不注入
                        continue;
                    }
                    var attribute = attributes.Where(x => x is ComponentAttribute || x is ServiceAttribute || x is RepositoryAttribute).FirstOrDefault();
                    if (attribute == null)
                    {
                        //未标记为注入组件，不注入
                        continue;
                    }

                    //收集
                    lstMetaDatas.Add(new ComponentMetaData() 
                    {
                        CType = type,
                        DIMetaData = attribute as ComponentAttribute
                    });
                }
            }

            //注入顺序处理
            lstMetaDatas = lstMetaDatas.OrderBy(x => x.DIMetaData.Order).ToList();

            IList<ComponentInfo> lstComponents = new List<ComponentInfo>();
            foreach (var compMetaData in lstMetaDatas) 
            {
                var componentInfo = GetComponentInfo(compMetaData);
                lstComponents.Add(componentInfo);
            }
            return lstComponents;
        }

        /// <summary>
        /// 将组件注册到容器
        /// </summary>
        /// <param name="containerBuilder"></param>
        /// <param name="lstComponents"></param>
        public static void RegisterComponents(ContainerBuilder containerBuilder, IList<ComponentInfo> lstComponents) 
        {
            if (lstComponents == null || lstComponents.Count == 0) 
            {
                return;
            }
            foreach (var component in lstComponents) 
            {
                var services = component.ServiceDict.SelectMany(x => x.Value);
                if (services == null || services.Count() == 0) 
                {
                    continue;
                }
                foreach (var item in services) 
                {
                    var service = item.Value;
                    var typeInfo = service.ServiceType.GetTypeInfo();
                    if (typeInfo.IsGenericTypeDefinition)
                    {
                        //注册泛型组件
                        RegisterGenericTypeComponent(containerBuilder, component, service);
                    }
                    else 
                    {
                        //注册普通类型组件
                        RegisterComponent(containerBuilder, component, service);
                    }
                }
            }
        }

        /// <summary>
        /// 根据注入标记信息，获取所有可以注入的组件信息
        /// </summary>
        /// <param name="componentMetaData"></param>
        /// <returns></returns>
        private static ComponentInfo GetComponentInfo(ComponentMetaData componentMetaData)
        {
            if (componentMetaData == null || componentMetaData.CType == null || componentMetaData.DIMetaData == null)
            {
                return null;
            }
            var currentType = componentMetaData.CType;
            ComponentInfo component = new ComponentInfo() { ComponentType = currentType };

            HashSet<Type> registerTypes = new HashSet<Type>();
            var compAttr = componentMetaData.DIMetaData;
            string serviceName = compAttr.Name ?? "";
            if (compAttr.ServiceType != null)
            {
                registerTypes.Add(compAttr.ServiceType);
            }
            else 
            {
                //获取所实现的接口，然后注入这些类型
                IList<Type> lstAllTypes = GetImplementdInterfaces(currentType);
                if (lstAllTypes != null && lstAllTypes.Count > 0) 
                {
                    foreach (var type in lstAllTypes) 
                    {
                        registerTypes.Add(type);
                    }
                }
            }

            foreach (var type in registerTypes) 
            {
                var lifeCycle = compAttr.LifeCycle == ObjectLifeCycleEnum.Default ? ObjectLifeCycleEnum.InstancePerLifetimeScope : compAttr.LifeCycle;
                var serviceDetail = new ServiceDetail() 
                {
                    Name = serviceName,
                    LifeCycle = lifeCycle,
                    ServiceType = type
                };
                IDictionary<string, ServiceDetail> keyValuePairs = new Dictionary<string, ServiceDetail>()
                {
                    { serviceName, serviceDetail}
                };
                if (component.ServiceDict.ContainsKey(type) == false)
                {
                    component.ServiceDict.Add(type, keyValuePairs);
                }
                else 
                {
                    component.ServiceDict[type] = keyValuePairs;
                }
            }

            //当前类型也注入
            if (component.ServiceDict.ContainsKey(currentType) == false) 
            {
                var service = new ServiceDetail() 
                {
                    Name = "",
                    ServiceType = currentType,
                    LifeCycle = compAttr.LifeCycle == ObjectLifeCycleEnum.Default ? ObjectLifeCycleEnum.InstancePerLifetimeScope : compAttr.LifeCycle
            };

                component.ServiceDict.Add(currentType, new Dictionary<string, ServiceDetail>() { { "", service } });
            }
            return component;
        }

        /// <summary>
        /// 获取类型所有实现的接口
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private static IList<Type> GetImplementdInterfaces(Type type) 
        {
            //如果是泛型类型的构造者
            if (type.IsGenericTypeDefinition)
            {
                //获取所有实现的泛型接口的构造者类型
                return type.GetTypeInfo().ImplementedInterfaces
                    .Where(x => x.IsGenericType)
                    .Select(x => x.GetGenericTypeDefinition())
                    .ToList();
            }
            else 
            {
                //获取非泛型接口
                var interfaces = type.GetTypeInfo().ImplementedInterfaces
                    .Where(x => x != typeof(IDisposable)).ToList();

                if (type.GetTypeInfo().IsInterface) 
                {
                    interfaces.Add(type);
                }
                return interfaces;
            }
        }

        /// <summary>
        /// 注册泛型组件
        /// </summary>
        /// <param name="containerBuilder"></param>
        /// <param name="component"></param>
        /// <param name="service"></param>
        private static void RegisterGenericTypeComponent(ContainerBuilder containerBuilder, ComponentInfo component, ServiceDetail service) 
        {
            containerBuilder
                //相当于注入IXxxxx<>，入参为实现类类型
                .RegisterGeneric(component.ComponentType)
                //实例开启属性注入（避免部分被继承类构造器注入时子类产生不必要的base调用链）
                .PropertiesAutowired()
                .ConfigureComponentService(service)
                .ConfigureComponentLifeCycle(service);
        }

        /// <summary>
        /// 注册普通类型组件
        /// </summary>
        /// <param name="containerBuilder"></param>
        /// <param name="component"></param>
        /// <param name="service"></param>
        private static void RegisterComponent(ContainerBuilder containerBuilder, ComponentInfo component, ServiceDetail service)
        {
            containerBuilder
                .RegisterType(component.ComponentType)
                .PropertiesAutowired()
                .ConfigureComponentService(service)
                .ConfigureComponentLifeCycle(service);
        }
    }
}
