using System.Text;

namespace WebApp.TemplatePattern.UserCards
{
    public class PrimeUserCardTemplate:UserCardTemplate
    {
        protected override string SetFooter()
        {
            var sb = new StringBuilder();
            sb.Append(@"<a href='#' class='card-link'> Mesaj Gönder</a>");
            sb.Append(@"<a href='#' class='card-link'> Profil</a>");
            return sb.ToString();
        }

        protected override string SetPicture()
        {
            return $@"<img class='card-img-top' src='{User.PictureUrl}' >";
        }
    }
}
