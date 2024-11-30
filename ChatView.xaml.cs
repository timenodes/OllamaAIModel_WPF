
using MAIModel.ViewModels;
using System.Windows.Controls;

namespace MAIModel.Views
{
    /// <summary>
    /// ChatView2.xaml 的交互逻辑
    /// </summary>
    public partial class ChatView : UserControl
    {
        ChatViewModel viewModel;
        public ChatView(ShareOllamaObject shareOllama)
        {
            InitializeComponent();
            viewModel = new ChatViewModel();
            viewModel.SetOllama(shareOllama);
            this.DataContext = viewModel;
        }
    }
}
