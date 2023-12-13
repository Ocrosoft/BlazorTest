using Foundation;
using UIKit;

namespace BlazorTest.MacCatalyst
{
	public static class DragDropHelper
	{
		public static void RegisterDragDrop(UIView view, Func<string, bool> check, Action<IEnumerable<string>> callback)
		{
			var dropInteraction = new UIDropInteraction(new DropInteractionDelegate()
			{
				Check = check,
				Callback = callback,
			});
			view.AddInteraction(dropInteraction);
		}
	}

	class DropInteractionDelegate : UIDropInteractionDelegate
	{
		public Action<IEnumerable<string>> Callback { get; init; }
		public Func<string, bool> Check { get; init; }

		public override UIDropProposal SessionDidUpdate(UIDropInteraction interaction, IUIDropSession session)
		{
			return new UIDropProposal(UIDropOperation.Copy);
		}

		public override void PerformDrop(UIDropInteraction interaction, IUIDropSession session)
		{
			List<string> paths = new List<string>();
			foreach (var item in session.Items)
			{
				item.ItemProvider.LoadItem(UniformTypeIdentifiers.UTTypes.Json.Identifier, null, (data, error) =>
				{
					if (data is NSUrl nsData && !string.IsNullOrEmpty(nsData.Path))
					{
						paths.Add(nsData.Path);
					}
				});
			}
			Callback.Invoke(paths);
		}
	}
}
