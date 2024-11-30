
using MAIModel.Commands;
using MAIModel.Views;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
namespace MAIModel.ViewModels
{
    /// <summary>
    /// 0、当前类:主窗体（视图）视图模型类，用于数据绑定:MainViewModel<----->MainWindow
    /// 1、实现原因：
    /// 2、当前方案：
    /// 3、优于其他方案的原因：
    /// 4、技术标准/依赖：
    /// 5、技术引用：
    /// </summary>
    public class MainViewModel : INotifyPropertyChanged
    {
        #region 字段、属性、集合、命令

        #region 字段
        private object _currentView;                //当前视图
        private string _currentTime;                //当前时间
        private string _currentModel;               //当前模型
        private DispatcherTimer _timer;             //时间定时器
        private ShareOllamaObject _ollamaObject;    //ollama对象
        #endregion

        #region 属性
        public object CurrentView
        {
            get => _currentView;
            set
            {
                _currentView = value;
                OnPropertyChanged();
            }
        }
        public string CurrentTime
        {
            get => _currentTime;
            set
            {
                _currentTime = value;
                OnPropertyChanged();
            }
        }
        public string CurrentModel
        {
            get => _currentModel;
            set
            {
                _currentModel = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region 集合
        private ObservableCollection<UserControl> _viewList;    //视图列表
        private ObservableCollection<UserControl> ViewList
        {
            get => _viewList;
            set
            {
                _viewList = value;
                OnPropertyChanged();
            }
        }   
        #endregion

        #region 命令
        public ICommand SwitchToViewCommand { get; }
        public ICommand ClosingCommand { get; }

        #endregion

        #endregion

        #region 构造函数
        public MainViewModel()
        {
            _ollamaObject = new ShareOllamaObject(); //初始化Ollama对象   
            SwitchToViewCommand = new SwitchToViewCommand(OnSwitchToView);
            ClosingCommand = new EventsCommand<CancelEventArgs>(OnClosing);
            //创建视图
            _viewList = new ObservableCollection<UserControl>();
            ViewList.Add(new SettingView(_ollamaObject));
            ViewList.Add(new ChatProView(_ollamaObject));
            //ViewList.Add(new ChatView(_ollamaObject));
            // 默认显示子窗体1
            CurrentModel = _ollamaObject.Ollama.SelectedModel;
            InitializeTimer();
            CurrentView = ViewList[0];
        }
        #region 窗体关闭
        /// <summary>
        /// 触发关闭
        /// </summary>
        private void OnClosing(CancelEventArgs e)
        {
            if (MessageBox.Show("确定要关闭程序吗？", "确认关闭", MessageBoxButton.YesNo) == MessageBoxResult.No)
                e.Cancel = true;
            else  ClearingResources();
        }
        /// <summary>
        /// 清理资源
        /// </summary>
        private void ClearingResources()
        {
            // 执行清理操作
            ShareOllamaObject.CloseProcess("ollama_llama_server");
            Debug.Print($"{ShareOllamaObject.GetProgramName()}:关闭成功...");
        }


        #endregion
        #endregion

        #region 其他方法
        //初始化定时器
        private void InitializeTimer()
        {
            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromSeconds(1); // 每秒更新一次
            _timer.Tick += Timer_Tick;
            _timer.Start();
        }
        private void Timer_Tick(object sender, EventArgs e)
        {
            // 更新当前时间
            CurrentTime = DateTime.Now.ToString("HH:mm:ss");
            CurrentModel = _ollamaObject.Ollama.SelectedModel;
        }

        #endregion

        #region 命令触发的方法

        #region 视图切换
        //设置视图
        public void OnSwitchToView(object operationItem)
        {
            var viewObj = ViewList.FirstOrDefault(viewObj => viewObj.GetType().Name.Equals(operationItem));
            if (viewObj == null)
            {
                var newViewObj =new UserControl();
                switch (operationItem)
                {
                    case "ChatView":
                        //newViewObj = new ChatView(_ollamaObject);
                        break;
                    case "ChatProView":
                        newViewObj = new ChatProView(_ollamaObject);
                        break;
                    case "SettingView":
                        newViewObj = new SettingView(_ollamaObject);
                        break;
                    default:
                        break;
                }
                ViewList.Add(newViewObj);
                CurrentView = newViewObj;
            }
            else
            {
                CurrentView = viewObj;
            }
        }
        #endregion

        #endregion

        #region 属性变更事件
        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
