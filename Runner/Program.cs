using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Http;
using System.Web.Mvc;
using static System.Console;

namespace Runner
{
  class Program
  {
    static void Main(string[] args)
    {
      var controllers = GetControllerTypes<ApiTestGen.MvcApplication>();

      foreach (Type controller in controllers)
      {
        var info = GetControllerInfo(controller);
        WriteLine($"{info.Name}");
        
        foreach (var method in info.Methods)
        {
          var methodParams = method
            .GetParameters()
            .Select(a => $"{a.ParameterType} {a.Name}");

          WriteLine($"\t{method.Name}({string.Join(",", methodParams)}) -> {method.ReturnType}");
        }

        WriteLine("\n------------------------------------\n");
      }

      Read();
    }

    static IEnumerable<Type> GetControllerTypes<T>()
    {
      return typeof(T).Assembly.GetTypes().Where(ex =>
        ex.BaseType == typeof(Controller) ||
        ex.BaseType == typeof(ApiController));
    }

    static ControllerInfo GetControllerInfo(Type controller)
    {
      var controllerType = typeof(Controller);
      return new ControllerInfo
      {
        Type = controllerType,
        Name = controllerType.Name,
        Methods = controllerType
          .GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly)
      };
    }
  }

  public class ControllerInfo
  {
    public string Name { get; set; }
    public Type Type { get; set; }
    public IEnumerable<MethodInfo> Methods { get; set; }
  }
}
