using System.Text;
using System.Windows.Forms;

namespace CrawlDataTruyen.ConsoleSetup
{
    // Class dùng để chuyển hướng Console Output
    public class ConsoleOutput : TextWriter
    {
        private readonly TextBoxBase _output;

        public ConsoleOutput(TextBoxBase output)
        {
            _output = output;
        }

        public override void Write(char value)
        {
            base.Write(value);
            _output.Invoke(new Action(() => _output.AppendText(value.ToString())));
        }

        public override Encoding Encoding
        {
            get { return Encoding.UTF8; }
        }
    }
}
