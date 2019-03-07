using System.Security.Principal;
using System.Web.Mvc;

namespace sso.ActionFilter
{
    public class IncludeLayoutDataAttribute : ActionFilterAttribute
    {
        public string _user;

        public IncludeLayoutDataAttribute()
        {
            _user = WindowsIdentity.GetCurrent().Name.Split('\\')[1].Trim();
        }

        private string GetUserName()
        {
            string firstName = "Prezado";
            string secondName = "Visitante";
            if (_user.Contains("."))
            {
                firstName = _user.Split('.')[0].Trim();
                secondName = _user.Split('.')[1].Trim();
            }

            return string.Format("{0} {1}", char.ToUpper(firstName[0]) + firstName.Substring(1), char.ToUpper(secondName[0]) + secondName.Substring(1));
        }

        private string GetEnviromentUser() => WindowsIdentity.GetCurrent().Name.Split('\\')[1].Trim();

        public override void OnResultExecuting(ResultExecutingContext filterContext)
        {
            if (filterContext.Result is ViewResult)
            {
                var bag = (filterContext.Result as ViewResult).ViewBag;
                
                bag.EnviromentUser = GetEnviromentUser();
                bag.UserName = GetUserName();
            }
        }
    }
}