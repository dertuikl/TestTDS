using UnityEngine;
using Zenject;

public class ZenjectInstaller : MonoInstaller
{
	public override void InstallBindings()
	{
		// Register your dependencies and bindings here
		// For example:
		Container.Bind<Board>().FromComponentInHierarchy().AsSingle();
		Container.Bind<ItemsConfig>().FromComponentInHierarchy().AsSingle();
		// ...
	}
}