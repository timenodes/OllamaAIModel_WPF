using MAIModel.Commands;
using MAIModel.Models;
using Microsoft.Win32;
using OllamaSharp;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace MAIModel.ViewModels
{
    /// <summary>
    /// 0、当前类:
    /// 0、方式方法：
    /// 1、实现原因：
    /// 2、当前方案：
    /// 3、优于其他方案的原因：
    /// 4、技术标准/依赖：
    /// 5、技术引用：
    /// </summary>
    public class SettingViewModel:INotifyPropertyChanged
    {
        #region 字段、属性、集合、命令

        #region 字段
        private string _ollamaAppPath;                  //模型服务APP路径
        private string _selectedModel;                  //选择的模型
        private string _modelInfo;                      //模型信息
        private SolidColorBrush _labelBackgroundColor;  //颜色样式
        private readonly ShareOllamaObject _ollama;      //Ollama模型对象
        #endregion

        #region 属性
        public string OllamaAppPath
        {
            get { return _ollama.OllamaAppPath; }
            set { _ollama.OllamaAppPath = value; OnPropertyChanged(); }
        }
        public string SelectedModel
        {
            get => _selectedModel;
            set
            {
                if (_selectedModel != value)
                {
                    _selectedModel = value;
                    ResetModelName();
                }
                OnPropertyChanged();
            }
        }
        public string ModelInformation
        {
            get => _modelInfo;
            set
            {
                _modelInfo = value;
                OnPropertyChanged();
            }
        }
        public SolidColorBrush LabelBackgroundColor
        {
            get => _labelBackgroundColor;
            set
            {
                if (_labelBackgroundColor != value)
                {
                    _labelBackgroundColor = value;
                    OnPropertyChanged();
                }
            }
        }
        #endregion

        #region 集合
        public ObservableCollection<string> ModelList
        {
            get { return _ollama.ModelList; }
            set { _ollama.ModelList = value; OnPropertyChanged(); }
        }
        #endregion

        #region 命令
        public ICommand OpenFileDialogCommand { get; }      //选择Ollama文件路径
        public ICommand GetModelListCommand { get; }        //获取模型列表
        public ICommand ModelListUpdateCommand { get; }     //模型列表更新
        public ICommand StartOllamaServerCommand { get; }
        #endregion

        #endregion

        #region 构造函数
        public SettingViewModel(ShareOllamaObject ollama)
        {
            _ollama = ollama;
            Task task = OnGetModelList();
            OpenFileDialogCommand = new RelayCommand(() => OnSelectOllamaAppPathDialog());
            GetModelListCommand = new RelayCommand(async () => await OnGetModelList());
            ModelListUpdateCommand = new RelayCommand(async () => await OnModelListUpdate());
            StartOllamaServerCommand = new RelayCommand(async () => OnStartOllamaServer());
            SetConnected();
        }
        #endregion

        #region 其他方法
        /// 设置当前模型服务APP对象
        public void SetOllamaApiClient(OllamaApiClient ollama)
        {
            _ollama.Ollama = ollama;
        }
        //设置连接状态颜色
        public void SetConnected()
        {
            if (_ollama.OllamaEnabled)
            {
                LabelBackgroundColor = Brushes.Green;
            }
            else
            {
                LabelBackgroundColor = Brushes.Red;
            }
        }
        /// <summary>
        /// 重设模型
        /// </summary>
        private void ResetModelName()
        {
            _ollama.OllamaEnabled = false;
            _ollama.Ollama.SelectedModel = SelectedModel;
            ModelInformationChanged();
            _ollama.OllamaEnabled = true;
        }
        /// <summary>
        /// 模型信息变更
        /// </summary>
        public void ModelInformationChanged()
        {
            string modelName = SelectedModel.Split(':')[0].ToLower();
            string modelInfoPath = $"{Environment.CurrentDirectory}\\model introduction\\{modelName}.txt";
            string info = string.Empty;
            if (File.Exists(modelInfoPath))
            {
                info = File.ReadAllText(modelInfoPath);
            }
            //MessageBox.Show(modelInfoPath);
            switch (modelName)
            {
                case ModelDescription.Llama32:
                    ModelInformation = info;
                    break;
                case ModelDescription.CodeGemma:
                    ModelInformation = info;
                    break;
                default:
                    ModelInformation = "";
                    break;
            }
        }
        #endregion

        #region  命令触发的方法
        private void OnStartOllamaServer()
        {
            if (!_ollama.OllamaEnabled)
            {
                _ollama.StartOllama(OllamaAppPath, SelectedModel);
            }
        }
        //选择Ollama App路径
        private void OnSelectOllamaAppPathDialog()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                OllamaAppPath = openFileDialog.FileName;
            }
        }
        /// <summary>
        /// 获取模型列表
        /// </summary>
        private async Task OnGetModelList()
        {
            try
            {
                ModelList.Clear(); 
                ModelList = (ObservableCollection<string>)_ollama.GetModelList();
                Debug.Print($"ModelList count: {ModelList.Count}");
                SelectedModel = _ollama.Ollama.SelectedModel;
                var modelName = ModelList.FirstOrDefault(name=>name.Equals(SelectedModel));
                if (ModelList.Count>0 && modelName != null)
                {
                    SelectedModel = ModelList[ModelList.Count-1];
                }
               
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        /// <summary>
        /// 模型列表更新
        /// </summary>
        private async Task OnModelListUpdate()
        {
            MessageBox.Show($"List Update");
        }
        #endregion

        #region 属性变更事件、触发方法
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

    }
}
