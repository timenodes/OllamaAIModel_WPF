using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Interactivity;
using System.Windows;

namespace MAIModel.Commands
{
    /// <summary>
    ///RichTextBox 的 Document 属性不是一个依赖属性（DependencyProperty），因此不能直接进行双向数据绑定。
    ///我们需要使用一种间接的方式来实现数据绑定。
    ///解决方案:
    ///使用 Behavior 来实现 RichTextBox 的数据绑定。Behavior 是一个附加行为，可以在 XAML 中使用，
    ///通过代码来实现复杂的行为逻辑。
    /// </summary>
    public class RichTextBoxBehavior : Behavior<RichTextBox>
    {
        public static readonly DependencyProperty DocumentProperty =
            DependencyProperty.Register("Document", typeof(FlowDocument), typeof(RichTextBoxBehavior),
                new FrameworkPropertyMetadata(null, OnDocumentChanged));

        public FlowDocument Document
        {
            get { return (FlowDocument)GetValue(DocumentProperty); }
            set { SetValue(DocumentProperty, value); }
        }

        private static void OnDocumentChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var behavior = d as RichTextBoxBehavior;
            if (behavior != null)
            {
                behavior.UpdateRichTextBox((FlowDocument)e.NewValue);
            }
        }

        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.Loaded += OnLoaded;
            AssociatedObject.TextChanged += OnTextChanged;
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            AssociatedObject.Loaded -= OnLoaded;
            AssociatedObject.TextChanged -= OnTextChanged;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            UpdateRichTextBox(Document);
        }

        private void OnTextChanged(object sender, TextChangedEventArgs e)
        {
            Document = AssociatedObject.Document;
        }

        private void UpdateRichTextBox(FlowDocument document)
        {
            if (document != null && AssociatedObject.Document != document)
            {
                AssociatedObject.Document = document;
            }
        }
    }
}
