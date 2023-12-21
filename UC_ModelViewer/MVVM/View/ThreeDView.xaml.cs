using System;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media.Media3D;
using UC_ModelViewer.MVVM.ViewModel;

namespace UC_ModelViewer.MVVM.View
{
    public partial class ThreeDView : UserControl
    {
        public ThreeDView()
        {
            InitializeComponent();
            DataContextChanged += OnDataContextChanged;
        }

        private void OnDataContextChanged(object sender, System.Windows.DependencyPropertyChangedEventArgs e)
        {
            if (DataContext is ThreeDViewModel vm)
            {
                vm.PropertyChanged += ViewModel_PropertyChanged;
            }
        }

        private void ViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (sender is ThreeDViewModel viewModel)
            {
                if (e.PropertyName == nameof(ThreeDViewModel.Model))
                {
                    helixViewport.Children.Clear();
                    helixViewport.Children.Add(new ModelVisual3D { Content = viewModel.Model });

                    // Create and add the directional light visual and make it face the model
                    var directionalLightVisual = new ModelVisual3D { Content = viewModel.DirectionalLight };
                    helixViewport.Children.Add(directionalLightVisual);
                    var binding = new Binding("LookDirection")
                    {
                        Source = helixViewport.Camera
                    };
                    BindingOperations.SetBinding(viewModel.DirectionalLight, DirectionalLight.DirectionProperty, binding);

                    // Ensure that model is zoomed to fit into the Viwewport window
                    Dispatcher.BeginInvoke((Action)(() =>
                    {
                        helixViewport.ZoomExtents(500);
                    }), System.Windows.Threading.DispatcherPriority.Background);
                }
            }
        }
    }
}
