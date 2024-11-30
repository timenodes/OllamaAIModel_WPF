using MAIModel.Commands;
using OllamaSharp;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace MAIModel.ViewModels
{
    /// <summary>
    /// 0、当前类:
    /// 1、实现原因：
    /// 2、当前方案：
    /// 3、优于其他方案的原因：
    /// 4、技术标准/依赖：
    /// 5、技术引用：
    /// </summary>
    public class ChatViewModel : INotifyPropertyChanged
    {
        #region 字段、属性、集合、命令

        #region 字段
        private string? _inputText;                  // 用户输入的文本。
        private string? _message;                    // 返回消息输入的文本。
        private Chat? chat;                          //构建交互式聊天
        private ShareOllamaObject _ollama; //ollama对象
        private CancellationTokenSource _cancellationTokenSource =new CancellationTokenSource();   //取消令牌来源
        
        #endregion

        #region 属性 
        //InputText：用户输入的文本，支持属性更改通知。
        public string? InputText
        {
            get => _inputText;
            set
            {
                _inputText = value;
                OnPropertyChanged();
            }
        }
        //AI回复消息
        public string? Message
        {
            get => _message;
            set
            {
                _message = value;
                OnPropertyChanged();
            }
        }

        
        #endregion

        #region 集合
        //存储对话记录的集合。
        private ObservableCollection<string> _conversation;
        // Conversation：对话记录集合，支持属性更改通知。
        public ObservableCollection<string> Conversation
        {
            get => _conversation;
            set
            {
                _conversation = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region 命令
        // GenerateCommand：生成响应的命令。
        public ICommand? SubmitQuestionCommand { get; }
        //终止消息
        public ICommand? StopCurrentChatCommand { get; }
        //新建会话
        public ICommand? NewSessionCommand { get; }
        #endregion

        #endregion

        #region 构造函数
        public ChatViewModel()
        {
            _cancellationTokenSource = new CancellationTokenSource();
            _conversation = new ObservableCollection<string>();
            SubmitQuestionCommand = new RelayCommand(()=>OnSubmitQuestion());
            StopCurrentChatCommand = new RelayCommand(() => OnStopCurrentChat());
            NewSessionCommand = new RelayCommand(() => OnNewSessionCommand());
           
            
        }
        #endregion

        #region 其他方法

        //设置Ollama
        public void SetOllama(ShareOllamaObject ollama)
        {
            _ollama = ollama;
        }
        private bool CheckChatState()
        {
            if (_ollama.Ollama == null || _ollama.OllamaEnabled == false)
            {
                Message += "服务未打开...";
                return false;
            }
            if (_ollama.Ollama.SelectedModel == null)
            {
                Message += "模型未选择...";
                return false;
            }
            if (string.IsNullOrWhiteSpace(InputText))
            {
                Message += "文本为空...";
                return false;
            }
            return true;
        }

        
        #endregion

        #region 命令方法
        /// <summary>
        /// 提交问题:提交问题给AI并获取输出
        /// </summary>
        private async void OnSubmitQuestion()
        {
            try
            {
                if (CheckChatState())       //指示指定的字符串是空、空还是只包含空白字符。
                {
                    string input = InputText;
                    Message += "\r\n###################################################################\r\n\r\n";
                    InputText = "";                                               // 清空输入框
                    Conversation.Add($"【User】: {input}{Environment.NewLine}");  //添加信息,到绑定控件
                    Message += $"【User】 => {input}{Environment.NewLine}";       //
                    
                    var responseBuilder = new System.Text.StringBuilder();        //创建响应
                    Message += $"【AI】=>： {Environment.NewLine}";               // 
                    string tempText = string.Empty;                               //临时文本，异步方法不直接赋值给属性。
                    if (input.Equals("/clearContext"))
                    {
                        chat = new Chat(_ollama.Ollama);
                        return;
                    }
                    //开始回答
                    #region 方式2=>聊天模式
                    if (chat == null)
                    {
                        chat = new Chat(_ollama.Ollama);
                    }
                    _cancellationTokenSource = new CancellationTokenSource();
                    await foreach (var answerToken in chat.SendAsync(input, _cancellationTokenSource.Token))
                    {
                        await Task.Delay(20);
                        tempText = answerToken;
                        responseBuilder.Append(tempText);    //字符串
                        Message += tempText;
                        Debug.Print(answerToken);
                    }
                    #endregion
                    //回答结束
                    Conversation.Add($"AI: {responseBuilder.ToString()}");
                }
            }
            catch (Exception ex)
            {
                Message += $"{ex.Message}\r\n";
                Conversation.Add($"Error: {ex.Message}");
            }
        }
        private void OnNewSessionCommand()
        {
            OnStopCurrentChat();
            if (chat != null)
            {
                chat.SendAsync("/clearContext");
                if (_ollama != null)
                    chat = new Chat(_ollama.Ollama);
            }
            Message = string.Empty;
        }

        private void OnStopCurrentChat()
        {
            _cancellationTokenSource?.Cancel();
            Task.Delay(100);
            Message = string.Empty;
        }
        #endregion

        #region 触发属性变更事件的方法
        /// <summary>
        /// OnPropertyChanged：触发属性更改事件。
        /// </summary>
        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
