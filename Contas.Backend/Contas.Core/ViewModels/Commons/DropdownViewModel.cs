namespace Contas.Core.ViewModels
{
    public class DropdownViewModel
    {
        public DropdownViewModel()
        {

        }

        public DropdownViewModel(int id, string text)
        {
            this.id = id;
            this.text = text;
        }

        public int id { get; set; }
        public string text { get; set; }
    }
}
