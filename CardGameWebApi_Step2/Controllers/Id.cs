using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CardGameWebApi.Models;

//for reading assembly information
using System.Reflection;
using System.Configuration;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860
//https://docs.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.mvc.controllerbase.createdatroute?view=aspnetcore-6.0


namespace CardGameWebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]     //All actions (methods) in the controller will be named Id. They must differentiate in Url
    public class IdController : ControllerBase
    {
        //api/id
        [HttpGet(Name = nameof(WhoAmI))]
        public IEnumerable<string> WhoAmI()
        {
            
            Assembly asmExe = Assembly.GetExecutingAssembly();
            var asn = asmExe.GetName();
            var version = asn.Version;

            /*
            //If you want to explore Assembly information
            IEnumerable<CustomAttributeData> assemblyAttributes = asmExe.CustomAttributes;

            Console.WriteLine("\n\nAssembly Attributes:");
            foreach (CustomAttributeData assemblyAttribute in assemblyAttributes)
            {
                Type attributeType = assemblyAttribute.AttributeType;
                Console.WriteLine($"\nArguments of attribute type: {attributeType}");
                IList<CustomAttributeTypedArgument> arguments = assemblyAttribute.ConstructorArguments;
                foreach (CustomAttributeTypedArgument arg in arguments)
                {
                    Console.WriteLine($"   {arg.Value}");
                }
            }
            */


            return new string[] { asn.Name, version.ToString(), "An example of how to implement a CardGame as a WebApi", Url.Link(nameof(WhoAmI), null) };
        }

        //api/id/{someParam}/?authordetails={authordetails}
        [HttpGet("{someParam}", Name = nameof(Author))]            //"{someParam}" differentiates the actions, which otherwise have same url
        public IEnumerable<string> Author(string authorDetails)
        {
            if (authorDetails.ToLower().Trim() == "name")
                return new string[] { "Martin Lenart" };

            return new string[] { "Martin Lenart", "Backvagen1, 184 42 Akersberga", Url.Link(nameof(Author), null) };
        }

        // /Author
        [Route("/Developer")]
        [HttpGet]
        public IEnumerable<string> Developer() => Author("name");
    }
}

