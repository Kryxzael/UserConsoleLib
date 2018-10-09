using System;
using System.ComponentModel;
using System.Linq;
using UserConsoleLib;

namespace Test
{
    [DesignerCategory("")]
    public class TestDropDownExpansion : UserConsoleLib.UserConsole
    {
        public TestDropDownExpansion()
        {
            TextboxInput.TextChanged += TextboxInput_TextChanged;
            TextboxInput.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
            TextboxInput.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
        }

        private void TextboxInput_TextChanged(object sender, EventArgs e)
        {
            if (TextboxInput.Text.LastOrDefault() == ' ')
            {
                Command cmd = Command.GetByName(TextboxInput.Text.Split(' ').FirstOrDefault());

                if (cmd != null)
                {
                    int o = TextboxInput.Text.Split(' ').Skip(1).Count();

                    Syntax syn = cmd.GetSyntax(new Params(TextboxInput.Text.Split(' ').Skip(1)));

                    if (o < syn.ArgumentCount && syn.ArgumentIsChoice(o))
                    {
                        TextboxInput.AutoCompleteCustomSource = new System.Windows.Forms.AutoCompleteStringCollection();
                        foreach (string i in syn.GetArgumentValidItems(o))
                        {
                            TextboxInput.AutoCompleteCustomSource.Add(TextboxInput.Text + i);
                        }
                    }
                }
            }
        }
    }
}
