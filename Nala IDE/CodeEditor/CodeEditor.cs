using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using NathanWiles.NalaIDE.Models;

namespace NathanWiles.NalaIDE.CodeEditor
{
    public class CodeEditor : Control
    {
        public static DependencyProperty DocumentProperty = DependencyProperty.Register("Document", typeof(DocumentModel), typeof(CodeEditor), new PropertyMetadata(null));

        private KeyInputHandler _keyHandler;
        private CursorPosition _cursorPosition;

        public DocumentModel Document
        {
            get { return (DocumentModel)GetValue(DocumentProperty); }
            set { SetValue(DocumentProperty, value); }
        }

        private float _lineHeight;
        private float _fontSize;

        static CodeEditor()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(CodeEditor), new FrameworkPropertyMetadata(typeof(CodeEditor)));
        }

        public override void OnApplyTemplate()
        {
            var window = Window.GetWindow(this);
            window.TextInput += HandleTextInput;
            window.KeyDown += HandleKeyDown;

            _keyHandler = new KeyInputHandler();

            Document = new DocumentModel();

            Document.Add("// Write your Nala program here.");

            _cursorPosition = new CursorPosition(Document);

            base.OnApplyTemplate();
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            _lineHeight = 17;
            _fontSize = 13;

            if (Document != null)
            {
                DrawEditor(drawingContext);
            }

            base.OnRender(drawingContext);
        }

        private void HandleTextInput(object sender, TextCompositionEventArgs e)
        {
            string temp = Document[_cursorPosition.Y];
            Document[_cursorPosition.Y] = temp.Insert(_cursorPosition.X, e.Text);
            _cursorPosition.X++;
            e.Handled = true;

            InvalidateVisual();
        }

        protected void HandleKeyDown(object sender, KeyEventArgs e)
        {
            e.Handled = _keyHandler.HandleKeyDown(Document, _cursorPosition, e.Key);

            if (e.Handled)
            {
                InvalidateVisual();
            }
        }

        protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            if (Document != null && Document.Count > 0)
            {
                _cursorPosition = new CursorPosition(Document);
                _cursorPosition.X = Document[Document.Count - 1].Length;
                _cursorPosition.Y = Document.Count - 1;
            }

            InvalidateVisual();

            base.OnPropertyChanged(e);
        }

        private void DrawEditor(DrawingContext drawingContext)
        {
            // Draw Background
            var backgroundRect = new Rect();
            backgroundRect.Width = ActualWidth;
            backgroundRect.Height = ActualWidth;
            backgroundRect.X = 0;
            backgroundRect.Y = 0;
            
            drawingContext.DrawRectangle(Background,null,backgroundRect);

            if (Document.Count == 0) { return; }

            var drawPosition = new System.Windows.Point();

            drawPosition.X = Padding.Left;
            drawPosition.Y = Padding.Top;

            var highlighter = new SyntaxHighlighter();
            List<string> words = highlighter.SplitToWordsAndWhitespace(Document);

            var typeface = new Typeface("Courier New");
            var cultureInfo = CultureInfo.GetCultureInfo("en-us");
            var flowDirection = FlowDirection.LeftToRight;

            var formattedText = new FormattedText("m", cultureInfo, flowDirection, typeface, _fontSize, Brushes.Transparent, 1.0);
            double emWidth = formattedText.Width;
            double emHeight = formattedText.Height;

            // Draw Text
            for (int i = 0; i < words.Count; i++)
            {
                var word = words[i];

                if (word == "\n")
                {
                    drawPosition.X = Padding.Left;
                    drawPosition.Y += _lineHeight;
                }

                if (word == " ")
                {
                    drawPosition.X += emWidth;
                }

                Brush brush = highlighter.GetBrush(word);

                // Create the formatted text based on the properties set.
                formattedText = new FormattedText(
                    word,
                    cultureInfo,
                    flowDirection,
                    typeface,
                    _fontSize,
                    brush,
                    1.0
                    );

                drawingContext.DrawText(formattedText, drawPosition);
                drawPosition.X += formattedText.Width;
            }
            

            var cursorPen = new Pen(Brushes.White, 1);

            var cursorDrawPosition = new System.Windows.Point(Padding.Left + _cursorPosition.X * emWidth, Padding.Top + _cursorPosition.Y * _lineHeight);

            //Draw Cursor
            drawingContext.DrawLine(cursorPen, cursorDrawPosition, new System.Windows.Point(cursorDrawPosition.X, cursorDrawPosition.Y + emHeight));
        }
    }
}
