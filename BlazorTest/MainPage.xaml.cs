namespace BlazorTest;
using Microsoft.Maui.Platform;

public partial class MainPage : ContentPage
{
	public MainPage()
	{
		InitializeComponent();

		Loaded += MainPage_Loaded;
	}

	public Func<string, bool> DragOverCheckPath { get; set; }
	public Action<IEnumerable<string>> DropCallback { get; set; }

	private void MainPage_Loaded(object sender, EventArgs e)
	{
		var view = this.ToPlatform(Handler.MauiContext);
		MacCatalyst.DragDropHelper.RegisterDragDrop(view, OnDragOver, OnDrop);
	}

	private bool OnDragOver(string path)
	{
		return DragOverCheckPath?.Invoke(path) ?? false;
	}

	private void OnDrop(IEnumerable<string> paths)
	{
		DropCallback?.Invoke(paths);
	}
}
