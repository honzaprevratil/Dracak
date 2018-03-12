using Dracak.Classes.Locations;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Dracak.Pages
{
    /// <summary>
    /// Interakční logika pro GamePage.xaml
    /// </summary>
    public partial class StoryPage : Page
    {
        SlowWriter writer = new SlowWriter();
        private int Page { get; set; } = 1;
        public StoryPage()
        {
            InitializeComponent();
        }

        public StoryPage StartWriting()
        {
            writer.Target = StoryBlock;
            writer.StoryFull = "Plavíš se na moři se svým bratrem Johnem. Máte namířeno na vaše tajné místo, ze kterého se pokaždé vracíte s plnými sítěmi sardinek. Docela dobře vám to vynáší, tudíž se sem vyplavíte vždy jen 2 krát do měsíce a pak žijete z výdělku. Na druhou stranu je to docela daleko od vašeho břehu, téměř 4 dny cesty. A díky tomu se sem vyplavit zrovna dneska asi nebylo to nejlepší Johnovo rozhodnutí, o kterém ses nechal přemluvit. Už když jste vyráželi, byla obloha ve směru vaší plavby bouřkově černá. A teď se ocitáte pod tímhle uhelně tmavým mrakem …";
            writer.StartWriting();
            return this;
        }
        private void Click_Skip(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new GamePage().BindData().StartWriting());
        }

        private void Click_Next(object sender, RoutedEventArgs e)
        {
            if (writer.IsWriting())
            {
                writer.StopWriting();
            }
            else
            {
                switch (Page)
                {
                    case 1:
                        StoryHeader.Content = "Probuzení z Reality";
                        StoryImage.Source = new BitmapImage(new Uri(@"/Dracak;component/Images/castaway.jpg", UriKind.Relative));
                        Filter.Background = (Brush)new BrushConverter().ConvertFrom("#4CA49C15");

                        writer.StoryFull = "Přes hlavu se Ti přelila přílivová vlna, která Tě probouzí ze snu. S odplouvající vodou otevíráš oči a vykašláváš nechutně slanou vodu, kterou jsi právě vdechl. S menší motolicí se zvedáš ze země. Na obličeji máš ještě písek, který ti sám postupně odpadává. Jak se tak rozhlížíš kolem sebe, začínáš cítit tupou bolest na levé části hlavy. Rukou nahmatáváš to místo, a když se pak díváš na prsty, spatříš krev. „Co se to sakra stalo? Od čeho mám tu ránu na hlavě? Ztroskotali jsme? A kde je John?!“ ptáš se sám sebe. Avšak na žádnou z těchto otázek zatím neznáš odpověď.";
                        writer.StartWriting();
                        break;

                    case 2:
                        this.NavigationService.Navigate(new GamePage().BindData().StartWriting());
                        break;
                }
                Page++;
            }
        }
    }
}
