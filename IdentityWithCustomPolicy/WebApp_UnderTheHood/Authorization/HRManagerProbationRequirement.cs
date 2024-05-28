using Microsoft.AspNetCore.Authorization;

namespace WebApp_UnderTheHood.Authorization
{
	public class HRManagerProbationRequirement : IAuthorizationRequirement  //Any custom requirement must implement that interface
	{
        public int ProbationMonths { get; set; }
        public HRManagerProbationRequirement(int probationMonths)
		{
			this.ProbationMonths = probationMonths;
		}
	}

	public class HRManagerProbationRequirementHandler : AuthorizationHandler<HRManagerProbationRequirement>
	{
		protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, HRManagerProbationRequirement requirement)
		{
			if(!context.User.HasClaim(x => x.Type == "EmploymentDate")) 
				return Task.CompletedTask;

			if(DateTime.TryParse(context.User.FindFirst(x => x.Type == "EmploymentDate")?.Value, out DateTime employmentDate))
			{
				var period = DateTime.Now - employmentDate;
				if(period.Days > 30 * requirement.ProbationMonths) 
				{
					context.Succeed(requirement);
				}

			}
			return Task.CompletedTask;
		}
	}
}
