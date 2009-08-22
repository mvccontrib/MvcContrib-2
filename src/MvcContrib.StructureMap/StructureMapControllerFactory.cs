using System;
using System.Web.Mvc;
using System.Web.Routing;
using StructureMap;

namespace MvcContrib.StructureMap
{
    public class StructureMapControllerFactory : DefaultControllerFactory
    {
        public override IController CreateController(RequestContext context, string controllerName)
        {
            Type controllerType = base.GetControllerType(context, controllerName);
            return ObjectFactory.GetInstance(controllerType) as IController;
        }
    }
}