using System.Windows.Forms;

namespace Rubberduck.Winforms.DataAnnotations
{
    public class TextBinding : Binding
    {
        public TextBinding(object model, string dataMember)
            : base("Text", model, dataMember, true, DataSourceUpdateMode.OnPropertyChanged)
        { }
    }
}