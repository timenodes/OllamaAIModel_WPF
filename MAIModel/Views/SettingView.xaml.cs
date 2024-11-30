using MAIModel.ViewModels;
using System.Windows.Controls;

namespace MAIModel.Views
{
    /// <summary>
    /// SettingView.xaml 的交互逻辑
    /// </summary>
    public partial class SettingView : UserControl
    {
        SettingViewModel _viewModel;
        public SettingView(ShareOllamaObject ollama)
        {
            InitializeComponent();
            _viewModel = new SettingViewModel(ollama);
            this.DataContext = _viewModel;
        }
    }
}
