using AccesoDatos.Data;
using System.Windows.Forms;
namespace VideoClubWinForms;
internal static class Program {
 [STAThread] static void Main() {
  ApplicationConfiguration.Initialize();
  using(var db=new AplicationDbContext()) db.Database.EnsureCreated();
  Application.Run(new MainForm());
 }
}
