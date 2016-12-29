using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Shapes;
using Microsoft.Win32;
using MMDB.Enums;
using MMDB.Extensions;
using MMDB.Windows;

namespace MMDB
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Private fields

        private Shape clickedShape;
        private Brush color, color2;
        private EditWindow editWindow = new EditWindow();
        private bool graphicLoaded;
        private bool graphicNew;
        private string graphicPath = "";
        private Operations operationType = Operations.None;
        private Point p1;
        private Point p2;
        private SearchWindow searchWindow;
        private bool shapeCreated;
        private Shapes shapeType = Shapes.None;
        private TableFormWindow tableFormWindow;

        #endregion
        #region Ctors

        public MainWindow()
        {
            InitializeComponent();
            color = Brushes.White;
            color2 = Brushes.Black;
            fillRectangle.Fill = color;
            strokeRectangle.Fill = color2;
            KeyDown += Window_KeyDown;
            editWindow.MyEvent += childWindow_MyEvent;
        }

        #endregion
        #region Overrides

        /// <summary>
        ///     Shutdown on MainWindowClose
        /// </summary>
        /// <param name="e"></param>
        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            Application.Current.Shutdown();
        }

        #endregion
        #region Private methods

        //=====================================================================
        // Vector graphics
        private void canvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            p1 = Mouse.GetPosition(canvas);
            shapeCreated = false;
        }

        private void canvas_MouseMove(object sender, MouseEventArgs e)
        {
            p2 = Mouse.GetPosition(canvas);
            textBoxCoords.Text = p2.X + " x " + p2.Y;
            if (Mouse.LeftButton == MouseButtonState.Pressed && operationType == Operations.None)
                if (!shapeCreated)
                {
                    Shape shape;
                    switch (shapeType)
                    {
                        case Shapes.Line:
                            var line = new Line();
                            shape = line.Create(p1, p2, 2, color2);
                            break;
                        case Shapes.Ellipse:
                            var ellipse = new Ellipse();
                            shape = ellipse.Create(p1, p2, 2, color2, color);
                            break;
                        case Shapes.Rectangle:
                            var rectangle = new Rectangle();
                            shape = rectangle.Create(p1, p2, 2, color2, color);
                            break;
                        case Shapes.Triangle:
                            var triangle = new Polygon();
                            shape = triangle.Create(p1, p2, 2, color2, color);
                            break;
                        default:
                            shape = new Line();
                            break;
                    }
                    shape.MouseLeftButtonDown += shape_MouseLeftButtonDown;
                    shape.MouseRightButtonDown += shape_MouseRightButtonDown;
                    shape.MouseMove += shape_MouseMove;
                    canvas.Children.Add(shape);
                    listOfObjects.Items.Add(shape.ParametersToString());
                    clickedShape = shape;
                    shapeCreated = true;
                }
                else
                {
                    var index = canvas.Children.IndexOf(clickedShape);
                    clickedShape.Resize(p1, p2);
                    listOfObjects.Items[index] = clickedShape.ParametersToString();
                }
        }

        private void childWindow_MyEvent(object sender, EventArgs e)
        {
            try
            {
                var p1 = new Point(Convert.ToDouble(editWindow.x1.Text), Convert.ToDouble(editWindow.y1.Text));
                var p2 = new Point(Convert.ToDouble(editWindow.x2.Text), Convert.ToDouble(editWindow.y2.Text));
                var index = canvas.Children.IndexOf(clickedShape);
                clickedShape.Resize(p1, p2);
                listOfObjects.Items[index] = clickedShape.ParametersToString();
            }
            catch (FormatException)
            {
            }
        }

        private void ClearObjects()
        {
            canvas.Children.Clear();
            listOfObjects.Items.Clear();
        }

        private void colorButton_Click(object sender, RoutedEventArgs e)
        {
            var senderButton = (Button) sender;
            switch (senderButton.Name)
            {
                case "whiteButton":
                    color = Brushes.White;
                    break;
                case "grayButton":
                    color = Brushes.Gray;
                    break;
                case "blackButton":
                    color = Brushes.Black;
                    break;
                case "redButton":
                    color = Brushes.Red;
                    break;
                case "greenButton":
                    color = Brushes.Green;
                    break;
                case "blueButton":
                    color = Brushes.Blue;
                    break;
            }
            fillRectangle.Fill = color;
        }

        private void colorButton_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            var senderButton = (Button) sender;
            switch (senderButton.Name)
            {
                case "whiteButton":
                    color2 = Brushes.White;
                    break;
                case "grayButton":
                    color2 = Brushes.Gray;
                    break;
                case "blackButton":
                    color2 = Brushes.Black;
                    break;
                case "redButton":
                    color2 = Brushes.Red;
                    break;
                case "greenButton":
                    color2 = Brushes.Green;
                    break;
                case "blueButton":
                    color2 = Brushes.Blue;
                    break;
            }
            strokeRectangle.Fill = color2;
        }

        private void EnableButtons()
        {
            lineButton.IsEnabled = true;
            ellipseButton.IsEnabled = true;
            rectangleButton.IsEnabled = true;
            triangleButton.IsEnabled = true;

            paintButton.IsEnabled = true;
            grabButton.IsEnabled = true;
            removeButton.IsEnabled = true;
            editButton.IsEnabled = true;

            saveFile.IsEnabled = true;
        }

        private void menuOption_MouseEnter(object sender, MouseEventArgs e)
        {
            var menuItem = (MenuItem) sender;
            menuItem.Foreground = Brushes.Black;
        }

        private void menuOption_MouseLeave(object sender, MouseEventArgs e)
        {
            var menuItem = (MenuItem) sender;
            menuItem.Foreground = Brushes.White;
        }

        //=====================================================================
        // Menu
        private void NewFile_Click(object sender, EventArgs e)
        {
            if (graphicLoaded || graphicNew)
            {
                ClearObjects();
                textBoxFileName.Text = "New File";
            }
            canvas.Background = Brushes.White;
            graphicNew = true;
            graphicLoaded = false;
            textBoxFileName.Text = "";
            textBoxFileName.Foreground = Brushes.Black;
            textBoxFileName.FontStyle = FontStyles.Normal;
            textBoxFileName.Text = "New File";
            EnableButtons();
            saveFile.Foreground = Brushes.White;
        }

        private void OpenFile_Click(object sender, EventArgs e)
        {
            var dlg = new OpenFileDialog();
            dlg.Filter = "XAML |*.xaml";
            dlg.Multiselect = false; // default
            if (dlg.ShowDialog() == true)
            {
                graphicPath = dlg.FileName;

                textBoxFileName.Text = "";
                textBoxFileName.Foreground = Brushes.Black;
                textBoxFileName.FontStyle = FontStyles.Normal;
                textBoxFileName.Text = dlg.SafeFileName;

                if (graphicNew || graphicLoaded)
                    ClearObjects();
                canvas.Background = Brushes.White;
                graphicLoaded = true;
                graphicNew = false;
                EnableButtons();

                var file = new FileStream(graphicPath, FileMode.Open);
                var c = (Canvas) XamlReader.Load(file);
                while (c.Children.Count > 0)
                {
                    var s = (Shape) c.Children[0];
                    s.MouseLeftButtonDown += shape_MouseLeftButtonDown;
                    s.MouseRightButtonDown += shape_MouseRightButtonDown;
                    s.MouseMove += shape_MouseMove;
                    listOfObjects.Items.Add(s.ParametersToString());
                    c.Children.RemoveAt(0);
                    canvas.Children.Add(s);
                }
                saveFile.Foreground = Brushes.White;
            }
        }

        private void operationButton_Click(object sender, RoutedEventArgs e)
        {
            var senderButton = (Button) sender;

            switch (senderButton.Name)
            {
                case "paintButton":
                    operationType = Operations.Paint;
                    break;
                case "grabButton":
                    operationType = Operations.Grab;
                    break;
                case "removeButton":
                    operationType = Operations.Remove;
                    break;
                case "editButton":
                    operationType = Operations.Resize;
                    break;
            }
            shapeType = Shapes.None;
        }

        private void SaveFile_Click(object sender, EventArgs e)
        {
            var dlg = new SaveFileDialog();

            dlg.Filter = "XAML |*.xaml";
            dlg.RestoreDirectory = true;

            if (dlg.ShowDialog() == true)
            {
                var file = new FileStream(dlg.FileName, FileMode.Create, FileAccess.Write);
                XamlWriter.Save(canvas, file);
                foreach (var shape in canvas.Children)
                    XamlWriter.Save(shape);
                file.Close();
                textBoxFileName.Text = dlg.SafeFileName;
            }
        }

        private void shape_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            clickedShape = (Shape) sender;
            switch (operationType)
            {
                case Operations.Paint:
                    var index = canvas.Children.IndexOf((Shape) sender);
                    ((Shape) canvas.Children[index]).Fill = color;
                    break;
                case Operations.Grab:
                    p1 = Mouse.GetPosition(canvas);
                    break;
                case Operations.Remove:
                    index = canvas.Children.IndexOf((Shape) sender);
                    canvas.Children.RemoveAt(index);
                    listOfObjects.Items.RemoveAt(index);
                    break;
                case Operations.Resize:
                    if (!editWindow.IsVisible)
                    {
                        editWindow = new EditWindow();
                        editWindow.MyEvent += childWindow_MyEvent;
                        editWindow.SetShapeData(clickedShape.GetP1(), clickedShape.GetP2());
                        editWindow.Show();
                    }
                    else
                    {
                        editWindow.SetShapeData(clickedShape.GetP1(), clickedShape.GetP2());
                    }
                    break;
            }
        }

        private void shape_MouseMove(object sender, MouseEventArgs e)
        {
            if (operationType == Operations.Grab)
            {
                p2 = Mouse.GetPosition(canvas);
                if (Mouse.LeftButton == MouseButtonState.Pressed || Mouse.RightButton == MouseButtonState.Pressed)
                {
                    var index = canvas.Children.IndexOf(clickedShape);
                    var t = ((Shape) canvas.Children[index]).Margin;
                    var leftMargin = t.Left - (p1.X - p2.X);
                    var topMargin = t.Top - (p1.Y - p2.Y);
                    ((Shape) canvas.Children[index]).Margin = new Thickness(leftMargin, topMargin, 0, 0);
                    p1 = p2;
                    listOfObjects.Items[index] = clickedShape.ParametersToString();
                }
            }
        }

        private void shape_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            clickedShape = (Shape) sender;
            switch (operationType)
            {
                case Operations.Paint:
                    var index = canvas.Children.IndexOf((Shape) sender);
                    ((Shape) canvas.Children[index]).Stroke = color2;
                    break;
                case Operations.Grab:
                    p1 = Mouse.GetPosition(canvas);
                    break;
                case Operations.Remove:
                    index = canvas.Children.IndexOf((Shape) sender);
                    canvas.Children.RemoveAt(index);
                    listOfObjects.Items.RemoveAt(index);
                    break;
            }
        }

        //=====================================================================
        // Options management
        private void shapeButton_Click(object sender, RoutedEventArgs e)
        {
            var senderButton = (Button) sender;

            switch (senderButton.Name)
            {
                case "lineButton":
                    shapeType = Shapes.Line;
                    break;
                case "ellipseButton":
                    shapeType = Shapes.Ellipse;
                    break;
                case "rectangleButton":
                    shapeType = Shapes.Rectangle;
                    break;
                case "triangleButton":
                    shapeType = Shapes.Triangle;
                    break;
            }
            operationType = Operations.None;
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.N && Keyboard.Modifiers == ModifierKeys.Control)
                NewFile_Click(sender, e);
            if (e.Key == Key.O && Keyboard.Modifiers == ModifierKeys.Control)
                OpenFile_Click(sender, e);
            if (e.Key == Key.S && Keyboard.Modifiers == ModifierKeys.Control)
                SaveFile_Click(sender, e);
            if (e.Key == Key.F && Keyboard.Modifiers == ModifierKeys.Control)
            {
                searchWindow = new SearchWindow();
                searchWindow.Show();
            }
            if (e.Key == Key.T && Keyboard.Modifiers == ModifierKeys.Control)
            {
                tableFormWindow = new TableFormWindow();
                tableFormWindow.Show();
            }
        }

        #endregion
    }
}