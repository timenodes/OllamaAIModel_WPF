
using MAIModel.ViewModels;
using System.Windows.Controls;

namespace MAIModel.Views
{
    /// <summary>
    /// ChatView2.xaml 的交互逻辑
    /// </summary>
    public partial class ChatProView : UserControl
    {
        ChatProViewModel viewModel;
        public ChatProView(ShareOllamaObject shareOllama)
        {
            InitializeComponent();
            viewModel = new ChatProViewModel();
            viewModel.SetOllama(shareOllama);
            this.DataContext = viewModel;

        }
    }
}
