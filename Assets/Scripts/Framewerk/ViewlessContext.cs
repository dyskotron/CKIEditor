using strange.extensions.command.api;
using strange.extensions.command.impl;
using strange.extensions.context.api;
using strange.extensions.context.impl;
using strange.extensions.injector.api;
using strange.extensions.signal.impl;
using strange.framework.api;
using strange.framework.impl;
using Object = System.Object;

namespace Framewerk
{
	public class ContextStartSignal : Signal
	{
        
	}
	
	public class ViewlessContext : CrossContext
	{
		/// A Binder that maps Signals to Commands
		public ICommandBinder commandBinder { get; set; }

		//Interprets implicit bindings
		//public IImplicitBinder implicitBinder { get; set; }

		/// Signal triggered on Context launch
		public Signal contextStartSignal { get; set; }

		/// A list of Views Awake before the Context is fully set up.
		protected static ISemiBinding viewCache = new SemiBinding();
		
		/// The recommended Constructor
		/// Everything will begin automatically.
		/// Other constructors offer the option of interrupting startup at useful moments.
		public ViewlessContext() : base(new Object())
		{
		}

		public ViewlessContext(ContextStartupFlags flags) : base(new Object(), flags)
		{
		}

		public ViewlessContext(bool autoMapping) : base(new Object(), autoMapping)
		{
		}

		/// Map the relationships between the Binders.
		/// Although you can override this method, it is recommended
		/// that you provide all your application bindings in `mapBindings()`.
		protected override void addCoreComponents()
		{
			base.addCoreComponents();
			injectionBinder.Bind<IInstanceProvider>().Bind<IInjectionBinder>().ToValue(injectionBinder);
			injectionBinder.Bind<IContext>().ToValue(this).ToName(ContextKeys.CONTEXT);
			injectionBinder.Bind<ICommandBinder>().To<SignalCommandBinder>().ToSingleton();
			//injectionBinder.Bind<IImplicitBinder>().To<ImplicitBinder>().ToSingleton();
			injectionBinder.Bind<ContextStartSignal>().ToSingleton();
		}

		protected override void instantiateCoreComponents()
		{
			base.instantiateCoreComponents();
			
			commandBinder = injectionBinder.GetInstance<ICommandBinder>() as ICommandBinder;

			contextStartSignal = injectionBinder.GetInstance<ContextStartSignal>();
		}

		/// Fires ContextStartSignal
		/// Whatever Command/Sequence you want to happen first should 
		/// be mapped to this signal.
		public override void Launch()
		{
			contextStartSignal.Dispatch();
		}

		/// Gets an instance of the provided generic type.
		/// Always bear in mind that doing this risks adding
		/// dependencies that must be cleaned up when Contexts
		/// are removed.
		override public object GetComponent<T>()
		{
			return GetComponent<T>(null);
		}

		/// Gets an instance of the provided generic type and name from the InjectionBinder
		/// Always bear in mind that doing this risks adding
		/// dependencies that must be cleaned up when Contexts
		/// are removed.
		override public object GetComponent<T>(object name)
		{
			IInjectionBinding binding = injectionBinder.GetBinding<T>(name);
			if (binding != null)
			{
				return injectionBinder.GetInstance<T>(name);
			}

			return null;
		}

		/// Clean up. Called by a ContextView in its OnDestroy method
		public override void OnRemove()
		{
			base.OnRemove();
			commandBinder.OnRemove();
		}
	}
}

