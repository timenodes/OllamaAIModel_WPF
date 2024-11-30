using OllamaSharp;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;

namespace MAIModel.ViewModels
{
    /// <summary>
    /// 0、当前类: 共享的Ollama对象
    /// 1、实现原因：
    /// 2、当前方案：
    /// 3、优于其他方案的原因：
    /// 4、技术标准/依赖：
    /// 5、技术引用：
    /// </summary>
    public class ShareOllamaObject
    {
        #region 字段、属性、集合、命令

        #region 字段
        private OllamaApiClient _ollama;            //OllamaAPI对象。
        private Chat chat;                          //构建交互式聊天
        private bool _ollamaEnabled = false;        //ollama启动状态
        private string _ollamaAppPath;              //ollama程序路径,启动Ollama服务
        #endregion

        #region 属性
        public string OllamaAppPath
        {
            get { return _ollamaAppPath; }
            set { _ollamaAppPath = value; }
        }
        public bool OllamaEnabled
        {
            get { return _ollamaEnabled; }
            set { _ollamaEnabled = value; }
        }
        public OllamaApiClient Ollama
        {
            get { return _ollama; }
            set { _ollama = value; }
        }
        public Chat Chat
        {
            get { return chat; }
            set { chat = value; }
        }
        #endregion

        #region 集合
        public ObservableCollection<string> ModelList { get; set; }
        #endregion

        #endregion

        #region 构造函数
        public ShareOllamaObject()
        {
            Init(OllamaAppPath, "llama3.2:9b");
        }
        #endregion

        #region 其他方法
        /// <summary>
        /// 初始化对象
        /// </summary>
        private void Init(string appPath,string modelName)
        {
            OllamaAppPath =appPath;
            try
            {
                // 设置默认设备为GPU
                Environment.SetEnvironmentVariable("OLLAMA_DEFAULT_DEVICE", "gpu");
                //判断路径是否存在
                if (OllamaAppPath == string.Empty || OllamaAppPath == null) OllamaAppPath = @"ollama app.exe";
                //路径存在获取应用名
                if (File.Exists(OllamaAppPath)) OllamaAppPath = Path.GetFileName(OllamaAppPath);
                //获取环境Ollama环境变量：用于找到 ：ollama app.exe
                var filePath = FindExeInPath(OllamaAppPath);
                //如果路径存在，启动Ollama
                if (File.Exists(filePath)) CheckStartProcess(OllamaAppPath);
                //连接Ollama，并设置初始模型
                _ollama = new OllamaApiClient(new Uri("http://localhost:11434"));
                //获取本地可用的模型列表
                ModelList = (ObservableCollection<string>)GetModelList();
                var tmepModelName = ModelList.FirstOrDefault(name => name.ToLower().Contains("llama3.2"));
                if (tmepModelName!=null) _ollama.SelectedModel = tmepModelName;
                else if (ModelList.Count>0) _ollama.SelectedModel = ModelList[ModelList.Count-1];

                if (ModelList.FirstOrDefault(name => name.Equals(modelName))!=null) _ollama.SelectedModel = modelName;

                //Ollama服务启用成功
                OllamaEnabled = true;
            }
            catch (Exception)
            {
                OllamaEnabled = false;
            }
        }
        /// <summary>
        /// 更新Ollama的选择模型
        /// </summary>
        public void UpdataSelectedModel(string model)
        {
            Ollama.SelectedModel = model;
            OllamaEnabled = true;
        }


        public async void StartOllama(string appPath,string modelName)
        {
            Init(appPath,modelName); await Task.Delay(1);
        }
        /// <summary>
        /// 获取模型列表
        /// </summary>
        public Collection<string> GetModelList()
        {
            var models = _ollama.ListLocalModelsAsync();
            var modelList = new ObservableCollection<string>();
            foreach (var model in models.Result)
            {
                modelList.Add(model.Name);
            }
            return modelList;
        }
        #endregion

        #region 启动|关闭(Ollama服务程序)的方法
        /// <summary>
        /// 查找指定应用名是否配置在系统环境中，如果存在返回完整路径，否则返回null
        /// </summary>
        public static string FindExeInPath(string exeName)
        {
            // 获取环境变量 PATH 的值
            var pathVariable = Environment.GetEnvironmentVariable("PATH");

            // 将 PATH 分割成多个路径
            string[] paths = pathVariable.Split(Path.PathSeparator);

            // 遍历每个路径，检查是否存在指定的 .exe 文件
            foreach (string path in paths)
            {
                string fullPath = Path.Combine(path, exeName);
                if (File.Exists(fullPath))
                {
                    return fullPath;
                }
            }
            return null;
        }

        /// <summary>
        /// 启动程序指定程序，输入程序名称，判断程序是否在运行。
        ///     如果正在运行，直接退出，否则根据输入路径运行程序。
        /// </summary>
        public static void CheckStartProcess(string processPath)
        {
            string processName = Path.GetFileName(processPath);
            CheckStartProcess(processName, processPath);
        }

        /// <summary>
        /// 启动程序指定程序，输入程序名称，判断程序是否在运行。
        /// 如果正在运行，直接退出，否则根据输入路径运行程序。
        /// </summary>
        public static void CheckStartProcess(string processName, string processPath)
        {
            // 检查程序是否正在运行
            if (!IsProcessRunning(processName))
            {
                Console.WriteLine($"{processName} is not running. Starting the process...");
                StartProcess(processPath);
            }
            else Console.WriteLine($"{processName} is already running.");
        }
        

        /// <summary>
        /// 输入程序路径，启动程序
        /// </summary>
        public static void StartProcess(string processPath)
        {
            try
            {
                Process.Start(processPath);
                Console.WriteLine("Process started successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error starting process: {ex.Message}");
            }
        }

        /// <summary>
        /// 判断进程是否正在运行
        /// </summary>
        public static bool IsProcessRunning(string processName)
        {
            Process[] processes = Process.GetProcessesByName(processName);
            return processes.Length > 0;
        }

        /// <summary>
        /// 根据指定名称，关闭进程
        /// </summary>
        /// <param name="processName"></param>
        public static void CloseProcess(string processName)
        {
            try
            {
                foreach (var process in Process.GetProcessesByName(processName))
                {
                    process.Kill();
                    process.WaitForExit(); // 等待进程完全退出
                    Application.Current.Shutdown();
                }
            }
            catch (Exception ex)
            {
                // 处理异常
                MessageBox.Show($"无法关闭【{processName}】进程: {ex.Message}");
            }
        }
        /// <summary>
        /// 获取当前程序名
        /// </summary>
        public static string GetProgramName()
        {
            // 获取当前执行的程序集
            Assembly assembly = Assembly.GetExecutingAssembly();
            // 获取程序集的名称
            return assembly.GetName().Name;
        }
        #endregion
    }
}
