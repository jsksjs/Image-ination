using System;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Image_ination
{
	public partial class MainWindow
	{
		private int totalLayers = 0;
		private int layers = 0;

		/// <summary>
		/// Creates a new layer as a Grid with contained Image and adds it to layers' parent container.
		/// The new layer is set on top of all other layers by default.
		/// TODO: set new layer on top of currentLayer, not necessarily all other layers
		/// </summary>
		/// <returns>A copy of the newly created layer</returns>
		private Grid CreateNewImageLayer()
		{

			Grid grid = new Grid
			{
				Width = ImageCanvas.Width,
				Height = ImageCanvas.Height,
				Name = "Layer" + (layers + 1),
			};
			grid.SetValue(Panel.ZIndexProperty, totalLayers);

			BitmapSource img = BitmapFactory.New((int)ImageCanvas.Width, (int)ImageCanvas.Height);
			Image image = new Image
			{
				Source = img,
			};

			//add new Layer to Canvas
			grid.Children.Add(image);
			ImageCanvas.Children.Add(grid);

			RegisterName(grid.Name, grid);

			if (layers == 1)
				currentLayer = grid;

			totalLayers++;
			layers++;

			return grid;
		}

		private Grid CreateNewImageLayer(string filepath)
		{
			Grid grid = new Grid
			{
				Width = ImageCanvas.Width,
				Height = ImageCanvas.Height,
				Name = "Layer" + (layers + 1),                
			};
            Canvas.SetLeft(grid, 0);
            Canvas.SetTop(grid, 0);
			grid.SetValue(Panel.ZIndexProperty, totalLayers);
            using (FileStream s = File.OpenRead(filepath))
            {
                WriteableBitmap img = BitmapFactory.FromStream(s);
                Image image = new Image
                {
                    Source = img,
                };

                grid.Width = img.PixelWidth;
                grid.Height = img.PixelHeight;
                ImageCanvas.Width = img.PixelWidth;
                ImageCanvas.Height = img.PixelHeight;
                //add new Layer to Canvas
                grid.Children.Add(image);
                ImageCanvas.Children.Add(grid);
                RegisterName(grid.Name, grid);

                if (layers == 1)
                    currentLayer = grid;

                totalLayers++;
                layers++;
                return grid;
            }
		}


		/// <summary>
		/// Adds a button to the window that links to the specified layer.
		/// </summary>
		/// <param name="layerIndex">The ZIndex of the layer to be linked to.</param>
		private void AddButtonLink(Grid layer)
		{
			int layerIndex = (Int32)layer.GetValue(Panel.ZIndexProperty);
			TextBlock a = new TextBlock();
			a.Style = (System.Windows.Style)Content.FindResource("RadioText");
			a.Text = "Layer " + totalLayers;

			Border b = new Border();
			b.Style = (System.Windows.Style)Content.FindResource("RadioBorder");

			RadioButton c = new RadioButton();
			c.Style = (System.Windows.Style)Content.FindResource("LayerButtons");
			c.Height = 45;
			c.Name = "UI" + (layerIndex + 1);

			b.Child = a;
			c.Content = b;

			LayerStack.Children.Insert(layers - layerIndex - 1, c);
			if (LayerStack.Children.Count == 1)
			{
				RadioButton_Checked(LayerStack.Children[0], null);
				((RadioButton)LayerStack.Children[0]).IsChecked = true;
			}
		}

		private void NewLayer_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (open == true)
                e.CanExecute = true;
            else
                e.CanExecute = false;
        }
		/// <summary>
		/// Creates a new layer at the top of the canvas (highest ZIndex) and a new button in the window linking to it
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void NewLayer_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			NewCanvas();
		}

		private void DeleteLayer_CanExecute(object sender, CanExecuteRoutedEventArgs e) { e.CanExecute = true; }

		private void DeleteLayer_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			//TODO: Should we make the canvas uneditable and the buttons unclickable during this operation?
			//TODO: use radiobutton's position within the stack to determine the index of the layer that should be deleted INSTEAD of name
			//RadioButton r = (RadioButton)sender;
			//TODO: remove and delete button
			//TODO: remove and delete layer
			//TODO: update currentLayer to be another layer
		}

		private void LayerUp_CanExecute(object sender, CanExecuteRoutedEventArgs e) { e.CanExecute = true; }

		/// <summary>
		/// Moves currentLayer up by 1 ZIndex if it isn't already the topmost layer, updating the RadioButton stack on success.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void LayerUp_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			//Debug.WriteLine("LayerUp_Executed:");
			//ImageCanvas.ApplyTemplate();
			//Grid current = (Grid)LogicalTreeHelper.FindLogicalNode(ImageCanvas, currentLayer.Name);
			//current.SetValue(Panel.ZIndexProperty, (Int32)current.GetValue(Panel.ZIndexProperty)+1);
			//if(currentLayer.GetValue(Panel.NameProperty).Equals("Layer"+ImageCanvas.Children.Count))
			int currentZIndex = (Int32)currentLayer.GetValue(Panel.ZIndexProperty);

			//Debug.WriteLine("All zindexes before");
			//PrintAll();

			//find layer whose ZIndex matches current
			//find position of that layer in .Children 
			//swap ZIndexes
			foreach (UIElement child in ImageCanvas.Children)
			{
				if (child is Grid)
				{
					Grid g = (Grid)child;
					if ((Int32)child.GetValue(Panel.ZIndexProperty) == currentZIndex)
					{

					}
				}
				else
				{
					Debug.WriteLine("Child is not grid");
				}
			}
			currentLayer.SetValue(Panel.ZIndexProperty, (Int32)currentLayer.GetValue(Panel.ZIndexProperty) + 1);
			//Debug.WriteLine("All zindexes after");
			//PrintAll();

			//update button positions using ZIndexes
			/*
            if (currentLayer != null)
            {
                int index = ImageCanvas.Children.IndexOf(currentLayer);
                UIElement[] children = new UIElement[ImageCanvas.Children.Count];
                Debug.WriteLine("index:" + index + ",children.length:" + children.Length);
                if (index + 2 < children.Length) //if currentLayer not topmost layer already
                {
                    ImageCanvas.Children.CopyTo(children, 0); //store LayerStack.Children into children array

                    //update currentLayer and next uppermost layers' ZIndex values
                    currentLayer.SetValue(Panel.ZIndexProperty, (Int32)currentLayer.GetValue(Panel.ZIndexProperty) + 1);
                    children[index].SetValue(Panel.ZIndexProperty, (Int32)children[index].GetValue(Panel.ZIndexProperty) + 1);

                    //update the RadioButtons' positions

                    //RadioButton currentButton = (RadioButton)FindName("UI" + currentLayer.Name.Substring(5));
                    RadioButton currentButton = (RadioButton)LogicalTreeHelper.FindLogicalNode(LayerStack, ("UI" + currentLayer.Name.Substring(5)));
                    Debug.WriteLine("Moving this element up:" + ("\"UI" + currentLayer.Name.Substring(5)) + "\"");
                    foreach (RadioButton child in LayerStack.Children)
                    {
                        Debug.WriteLine(child.Name);
                        Debug.WriteLine("(FindName(" + child.Name + ") == null) == " + (FindName(child.Name) == null));
                    }
                    Debug.WriteLine("Layer names:");
                    foreach (Grid child in ImageCanvas.Children)
                    {
                        Debug.WriteLine(child.Name);
                    }
                    Debug.WriteLine("");
                    index = LayerStack.Children.IndexOf(currentButton);
                    LayerStack.Children.Remove(currentButton);
                    Debug.WriteLine("Current button name = " + currentButton.Name);
                    LayerStack.Children.Insert(index-1, currentButton);
                }
            }
            */
			// x.Move()
			//.Move([LayerStack.Children.IndexOf(layerButton)];
		}


		private void LayerDown_CanExecute(object sender, CanExecuteRoutedEventArgs e) { e.CanExecute = true; }

		private void LayerDown_Executed(object sender, ExecutedRoutedEventArgs e)
		{

		}

		private void Debug_CanExecute(object sender, CanExecuteRoutedEventArgs e) { e.CanExecute = true; }

		/// <summary>
		/// Executes code on Ctrl+T for debugging purposes.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void Debug_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			//if (currentLayer != null)
				//Debug.WriteLine("Debug - currentLayer = " + currentLayer.Name);
			//else Debug.WriteLine("Debug - currentLayer = null");
			//PrintAll();
		}

		/// <summary>
		/// Switch from current layer to the layer linked to this RadioButton
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void RadioButton_Checked(object sender, RoutedEventArgs e)
		{
			//Debug.WriteLine("currentlayer == null in button_checked before:" + (currentLayer == null));
			//set currentLayer to the layer linked to sender
			string layerName = "Layer" + ((RadioButton)sender).Name.Substring(2);
			currentLayer = ((Grid)FindName(layerName));
			//Debug.WriteLine("currentlayer == null in button_checked after:" + (currentLayer == null));
		}

		/// <summary>
		/// Deletes this button and its attached layer if there is more than one layer
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void RadioButton_RightClicked(object sender, RoutedEventArgs e)
		{
			if (LayerStack.Children.Count == 1)
				return;
			layers--;

			RadioButton r = (RadioButton)sender;
			if ((bool)r.IsChecked)
			{
				if (LayerStack.Children.IndexOf(r) < LayerStack.Children.Count - 1)
					((RadioButton)LayerStack.Children[LayerStack.Children.IndexOf(r) + 1]).IsChecked = true;
				else
					((RadioButton)LayerStack.Children[LayerStack.Children.IndexOf(r) - 1]).IsChecked = true;
			}

			LayerStack.Children.Remove(r);

			string layerName = "Layer" + r.Name.Substring(2);
			Grid layer = (Grid)FindName(layerName);
			if (layer == currentLayer)
				currentLayer = null;
			UnregisterName(layer.Name);
			ImageCanvas.Children.Remove(layer);

			UpdateLayerNames();
			UpdateLayerButtonNames();
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }
		/// <summary>
		/// Guarantees that all layers' ZIndexes are continuous. If a layer's ZIndex is more than 1 higher than all other ZIndexes, its ZIndex is set to be 1 higher than all other ZIndexes.
		/// If a given ZIndex, Z, in the range (lowestZ, highestZ) is not represented, the layers whose ZIndexes are above Z have their ZIndexes decremented.
		/// </summary>
		public void UpdateLayerZIndexes()
		{
			//int previous = int.MinValue;
			//int current;
			Grid[] children = new Grid[layers];
			int[] OGZIndexes = new int[layers];
			int[] SortedZindexes = new int[layers];
			int i = 0;
			foreach (UIElement child in ImageCanvas.Children)
			{
				if (child is Grid)
				{
					children[i] = (Grid)child;
				}
				i++;
			}
			int[] range = new int[layers]; //TODO: Remove potentially unnecessary variable
			for (i = 0; i < range.Length; i++) //populate range with values [0,1,2 ..., layers]
			{
				range[i] = i;
			}
			for (i = 0; i < OGZIndexes.Length; i++) //populate OGZIndexes with layers' ZIndexes in the order they exist
			{
				OGZIndexes[i] = (Int32)children[i].GetValue(Panel.ZIndexProperty);
			}
			Array.Copy(OGZIndexes, SortedZindexes, OGZIndexes.Length);
			Array.Sort(SortedZindexes);
			//Debug.Write("All values contained within OGZIndexes: ");
			//foreach (int z in OGZIndexes)
			//{
			//	Debug.Write(z + ", ");
			//}
			bool needsAdjusting = false;
			for (i = 0; i < SortedZindexes.Length; i++) //determine if there is an internal ZIndex value missing, i.e. [0,1,2,4,5]
			{
				if (SortedZindexes[i] != i)
				{
					needsAdjusting = true;
					break;
				}
			}
			if (needsAdjusting)
			{

			}

			/*int previous = int.MinValue; //previous ZIndex
            int current; //current ZIndex
            UIElement[] children = new UIElement[ImageCanvas.Children.Count];
            ImageCanvas.Children.CopyTo(children, 0);
            Grid child; //current layer
            int[] ZIndexes = new int[layers];
            for (int i = 0; i < children.Length; i++)
            {
                if (children[i] is Grid)
                {
                    child = (Grid)children[i];
                    current = (Int32)child.GetValue(Panel.ZIndexProperty);
                    if(previous != int.MinValue)
                    {
                        if(current)
                    }

                    previous = current;
                }
            }*/
		}

		/// <summary>
		/// Rename all layer buttons so that their names evenly increment, e.g. UI1, UI2, 3, 4, etc.
		/// </summary>
		private void UpdateLayerButtonNames()
		{
			int i = 0;
			foreach (RadioButton child in LayerStack.Children)
			{
				child.SetValue(Panel.NameProperty, "UI" + (LayerStack.Children.Count - i));
				i++;
			}
		}

		/// <summary>
		/// Rename all Layers so that their names evenly increment, e.g. Layer1, Layer2, 3, 4, etc.
		/// </summary>
		private void UpdateLayerNames()
		{
			int i = 0;
			Grid grid;
			foreach (UIElement child in ImageCanvas.Children)
			{
				try
				{
					grid = (Grid)child;
					UnregisterName(grid.Name);
					grid.SetValue(Panel.NameProperty, "Layer" + (i + 1));
					RegisterName(grid.Name, child);
					i++;
				}
				catch (InvalidCastException) { }
			}
		}

		public void NewCanvas()
		{
			AddButtonLink(CreateNewImageLayer());
		}

		public void NewCanvas(String filepath)
		{
			foreach (FrameworkElement child in ImageCanvas.Children)
			{
				if (!(child is Rectangle) && child.Name != "")
					UnregisterName(child.Name);
			}
			ImageCanvas.Children.Clear();
			LayerStack.Children.Clear();
			totalLayers = 0;
			layers = 0;
			if (filepath == null)
				AddButtonLink(CreateNewImageLayer());
			else
				AddButtonLink(CreateNewImageLayer(filepath));
		}
		/// <summary>
		/// Finds the Grid the given RadioButton is linked to by name
		/// </summary>
		/// <param name="b"></param>
		/// <returns></returns>
		private Grid GetGridFromButton(RadioButton b)
		{
			return (Grid)FindName(b.Name.Substring(2));
		}
		private RadioButton GetButtonFromGrid(Grid g)
		{
			return (RadioButton)FindName(g.Name.Substring(5));
		}
		/// <summary>
		/// Debugging method. Prints the names of 
		/// </summary>
		private void PrintAll()
		{
			if (currentLayer != null)
			{
				Debug.WriteLine("currentLayer = " + currentLayer.Name);
			}
			else
			{
				Debug.WriteLine("currentLayer = null");
			}
			Debug.WriteLine("****************************************\nAll layer names and ZIndexes:");
			foreach (UIElement child in ImageCanvas.Children)
			{
				if (child is Grid)
				{
					Debug.WriteLine(((Grid)child).Name + ": " + ((Grid)child).GetValue(Panel.ZIndexProperty));
				}
				else
				{
					Debug.WriteLine("<<<Selection Rectangle>>>");
				}
			}
			Debug.WriteLine("All RadioButton names and indexes:");
			foreach (RadioButton child in LayerStack.Children)
			{
				Debug.WriteLine(child.Name + ": " + LayerStack.Children.IndexOf(child));
			}
			Debug.WriteLine("****************************************");
		}
		/// <summary>
		/// Gets a count of all Layers in ImageCanvas
		/// </summary>
		/// <returns></returns>
		private int GetLayerCount()
		{
			int count = 0;
			foreach (UIElement child in ImageCanvas.Children)
			{
				if (child is Grid) count++;
			}
			return count;
		}
	}
}
