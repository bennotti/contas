namespace Contas.Core.ViewModels
{
    public class DropdownTextViewModel
    {
        public DropdownTextViewModel()
        {

        }

        public DropdownTextViewModel(string id, string text)
        {
            this.id = id;
            this.text = text;
        }

        public string id { get; set; }
        public string text { get; set; }
    }
}
