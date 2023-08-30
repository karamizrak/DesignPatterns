using System.Text;
using WebApp.TemplatePattern.Models;

namespace WebApp.TemplatePattern.UserCards
{
    public abstract class UserCardTemplate
    {
        protected AppUser User { get; set; }
        public void SetUser(AppUser user)
        { this.User = user; }

        public string Build()
        {
            if (User == null)
                throw new ArgumentNullException(nameof(User));

            var sb = new StringBuilder();
            /*
             <div class="card" style="width: 18rem;">
              <img class="card-img-top" src="..." alt="Card image cap">
              <div class="card-body">
                <h5 class="card-title">Card title</h5>
                <p class="card-text">Some quick example text to build on the card title and make up the bulk of the card's content.</p>
                <a href="#" class="btn btn-primary">Go somewhere</a>
              </div>
            </div>
             */
            sb.Append("<div class=\"card\" style=\"width: 18rem;\">");
            sb.Append(SetPicture());
            sb.Append($@"<div class='card-body'> 
                        <h5 class='card-title'>{User.UserName} </h5>
                        <p class='card-text'>{User.Description}</p>");
            sb.Append(SetFooter());
            sb.Append("</div>");
            sb.Append("</div>");
            return sb.ToString();
        }
        protected abstract string SetFooter();
        protected abstract string SetPicture();
    }
}
