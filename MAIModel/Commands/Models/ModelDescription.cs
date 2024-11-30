using System.ComponentModel;

namespace MAIModel.Models
{
    public class ModelDescription
    {
        public const string CodeGemma = "codegemma";
        public const string Llama32 = "llama3.2";


        public const string Codellama34b = "codellama:34b";
        public const string Llava13b = "llava:13b";
        public const string CommandRLatest = "command-r:latest";
        public const string Wizardlm2Latest = "wizardlm2:latest";
        public const string Qwen25CoderLatest = "qwen2.5-coder:latest";
        public const string Qwen25_14b = "qwen2.5:14b";
        public const string SamanthaMistralLatest = "samantha-mistral:latest";
        public const string MistralSmallLatest = "mistral-small:latest";
        public const string Gemma29b = "gemma2:9b";
        public const string NemotronMiniLatest = "nemotron-mini:latest";
        public const string Phi35Latest = "phi3.5:latest";
        public const string Llama32VisionLatest = "llama3.2-vision:latest";
        public const string Llama31_8b = "llama3.1:8b";
        public const string Gemma22b = "gemma2:2b";
        public const string Qwen27b = "qwen2:7b";
        public const string Qwen20_5b = "qwen2:0.5b";
        public const string Llama31_70b = "llama3.1:70b";
        public const string Llama31Latest = "llama3.1:latest";
        public const string Llama32Latest = "llama3.2:latest";
        public const string Llama32_3b = "llama3.2:3b";

       
    }

    public enum ModelNames
    {
        [Description("codellama:34b")]
        Codellama34b,

        [Description("llava:13b")]
        Llava13b,

        [Description("command-r:latest")]
        CommandRLatest,

        [Description("wizardlm2:latest")]
        Wizardlm2Latest,

        [Description("qwen2.5-coder:latest")]
        Qwen25CoderLatest,

        [Description("qwen2.5:14b")]
        Qwen25_14b,

        [Description("samantha-mistral:latest")]
        SamanthaMistralLatest,

        [Description("mistral-small:latest")]
        MistralSmallLatest,

        [Description("gemma2:9b")]
        Gemma29b,

        [Description("nemotron-mini:latest")]
        NemotronMiniLatest,

        [Description("phi3.5:latest")]
        Phi35Latest,

        [Description("llama3.2-vision:latest")]
        Llama32VisionLatest,

        [Description("llama3.1:8b")]
        Llama31_8b,

        [Description("gemma2:2b")]
        Gemma22b,

        [Description("qwen2:7b")]
        Qwen27b,

        [Description("qwen2:0.5b")]
        Qwen20_5b,

        [Description("llama3.1:70b")]
        Llama31_70b,

        [Description("llama3.1:latest")]
        Llama31Latest,

        [Description("llama3.2:latest")]
        Llama32Latest,

        [Description("llama3.2:3b")]
        Llama32_3b
    }
}
