namespace WhoCanHelpMe.Tasks.Registrars
{
	#region Using Directives

	using System.ComponentModel.Composition;
	using System.Reflection;

	using Castle.MicroKernel.Registration;
	using Castle.Windsor;

	using Domain.Contracts.Container;

	using Framework.Extensions;

	using Properties;

	using SharpArch.Web.Castle;

	#endregion

	[Export(typeof(IComponentRegistrar))]
	public class TasksRegistrar : IComponentRegistrar
	{
		public void Register(IWindsorContainer container)
		{
			container.Register(
					AllTypes.Pick()
							.FromAssembly(Assembly.GetAssembly(typeof(TasksRegistrarMarker)))
							.WithService.FirstNonGenericInterface("WhoCanHelpMe.Domain.Contracts.Tasks"));
		}
	}
}