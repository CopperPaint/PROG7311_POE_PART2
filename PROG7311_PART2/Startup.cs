using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using Owin;
using PROG7311_PART2.Models;

[assembly: OwinStartupAttribute(typeof(PROG7311_PART2.Startup))]
namespace PROG7311_PART2
{
    public partial class Startup
    {
        private PopulateDatabase populate = new PopulateDatabase();
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            populate.Populate();
        }
    }
}
